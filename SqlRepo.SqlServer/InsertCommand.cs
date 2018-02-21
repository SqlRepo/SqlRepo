using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SqlRepo.SqlServer
{
    public class InsertCommand<TEntity> : SqlCommand<TEntity, TEntity>, IInsertCommand<TEntity>
        where TEntity: class, new()
    {
        private const string StatementTemplate = "INSERT [{0}].[{1}]({2})\nVALUES({3}){4};"
                                                 + "\nSELECT *\nFROM [{0}].[{1}]\nWHERE [Id] = SCOPE_IDENTITY();"
            ;

        private readonly IList<Expression<Func<TEntity, object>>> selectors =
            new List<Expression<Func<TEntity, object>>>();

        private readonly IList<object> values = new List<object>();
        private readonly IWritablePropertyMatcher writablePropertyMatcher;
        private TEntity entity;

        public InsertCommand(ICommandExecutor commandExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher)
            : base(commandExecutor, entityMapper)
        {
            this.writablePropertyMatcher = writablePropertyMatcher;
        }

        [Obsolete]
        public IInsertCommand<TEntity> For(TEntity entity)
        {
            if(this.selectors.Any())
            {
                throw new InvalidOperationException(
                    "For cannot be used once With has been used, please use FromScratch to reset the command before using With.");
            }
            this.IsClean = false;
            this.entity = entity;
            return this;
        }

        public IInsertCommand<TEntity> FromScratch()
        {
            this.selectors.Clear();
            this.values.Clear();
            this.entity = null;
            this.IsClean = true;
            return this;
        }

        public override TEntity Go(string connectionString = null)
        {
            if(string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = this.ConnectionString;
            }
            using(var reader = this.CommandExecutor.ExecuteReader(connectionString, this.Sql()))
            {
                return this.EntityMapper.Map<TEntity>(reader)
                           .FirstOrDefault();
            }
        }

        public override async Task<TEntity> GoAsync(string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = this.ConnectionString;
            }
            using (var reader = await this.CommandExecutor.ExecuteReaderAsync(connectionString, this.Sql()))
            {
                return this.EntityMapper.Map<TEntity>(reader)
                           .FirstOrDefault();
            }
        }

        public override string Sql()
        {
            if(this.entity == null && !this.selectors.Any())
            {
                throw new InvalidOperationException(
                    "Sql cannot be used on a command that has not been initialised using With or For.");
            }

            return string.Format(StatementTemplate,
                this.TableSchema,
                this.TableName,
                this.GetColumnsList(),
                this.GetValuesList(),
                string.Empty);
        }

        public IInsertCommand<TEntity> UsingTableName(string tableName)
        {
            this.TableName = tableName;
            return this;
        }

        public IInsertCommand<TEntity> UsingTableSchema(string tableSchema)
        {
            this.TableSchema = tableSchema;
            return this;
        }

        public IInsertCommand<TEntity> With<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember @value)
        {
            if(this.entity != null)
            {
                throw new InvalidOperationException(
                    "With cannot be used once For has been used, please use FromScratch to reset the command before using With.");
            }

            this.IsClean = false;
            var expression = this.ConvertExpression(selector);
            this.selectors.Add(expression);
            this.values.Add(@value);
            return this;
        }

        private string FormatColumnNames(IEnumerable<string> names)
        {
            return string.Join(", ", names.Select(n => $"[{n}]"));
        }

        private string FormatValues(IEnumerable<object> values)
        {
            return string.Join(", ", values.Select(this.FormatValue));
        }

        private string GetColumnsList()
        {
            return this.selectors.Any()? this.GetColumnsListFromSelectors(): this.GetColumnsListFromEntity();
        }

        private string GetColumnsListFromEntity()
        {
            var names = typeof(TEntity).GetProperties()
                                       .Where(p => p.Name != "Id"
                                                   && this.writablePropertyMatcher.Test(p.PropertyType))
                                       .Select(p => p.Name);
            return this.FormatColumnNames(names);
        }

        private string GetColumnsListFromSelectors()
        {
            var names = this.selectors.Select(this.GetMemberName);
            return this.FormatColumnNames(names);
        }

        private string GetValuesFromEntity()
        {
            var entityValues = typeof(TEntity).GetProperties()
                                              .Where(p => p.Name != "Id"
                                                          && this.writablePropertyMatcher.Test(
                                                              p.PropertyType))
                                              .Select(p => p.GetValue(this.entity));
            return this.FormatValues(entityValues);
        }

        private string GetValuesList()
        {
            return this.selectors.Any()? this.FormatValues(this.values): this.GetValuesFromEntity();
        }
    }
}