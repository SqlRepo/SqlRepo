using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlRepo.SqlServer
{
    public class InsertCommand<TEntity> : SqlCommand<TEntity, TEntity>, IInsertCommand<TEntity>
        where TEntity : class, new()
    {
        private const string StatementTemplate =
            "INSERT [{0}].[{1}]({2})\nVALUES({3}){4};" + "\nSELECT *\nFROM [{0}].[{1}]\nWHERE [Id] = SCOPE_IDENTITY();";

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

        public IInsertCommand<TEntity> For(TEntity entity)
        {
            if (selectors.Any())
            {
                throw new InvalidOperationException(
                    "For cannot be used once With has been used, please use FromScratch to reset the command before using With.");
            }
            IsClean = false;
            this.entity = entity;
            return this;
        }

        public IInsertCommand<TEntity> FromScratch()
        {
            selectors.Clear();
            values.Clear();
            entity = null;
            IsClean = true;
            return this;
        }

        public override TEntity Go(string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = ConnectionString;
            }
            using (var reader = CommandExecutor.ExecuteReader(connectionString, Sql()))
            {
                return EntityMapper.Map<TEntity>(reader)
                    .FirstOrDefault();
            }
        }

        public override string Sql()
        {
            if (entity == null && !selectors.Any())
            {
                throw new InvalidOperationException(
                    "Sql cannot be used on a command that has not been initialised using With or For.");
            }

            return string.Format(StatementTemplate,
                TableSchema,
                TableName,
                GetColumnsList(),
                GetValuesList(),
                string.Empty);
        }

        public IInsertCommand<TEntity> UsingTableName(string tableName)
        {
            TableName = tableName;
            return this;
        }

        public IInsertCommand<TEntity> UsingTableSchema(string tableSchema)
        {
            TableSchema = tableSchema;
            return this;
        }

        public IInsertCommand<TEntity> With<TMember>(Expression<Func<TEntity, TMember>> selector, TMember @value)
        {
            if (entity != null)
            {
                throw new InvalidOperationException(
                    "With cannot be used once For has been used, please use FromScratch to reset the command before using With.");
            }

            IsClean = false;
            var expression = ConvertExpression(selector);
            selectors.Add(expression);
            values.Add(@value);
            return this;
        }

        private string FormatColumnNames(IEnumerable<string> names)
        {
            return string.Join(", ", names.Select(n => string.Format("[{0}]", n)));
        }

        private string FormatValues(IEnumerable<object> values)
        {
            return string.Join(", ", values.Select(FormatValue));
        }

        private string GetColumnsList()
        {
            return selectors.Any() ? GetColumnsListFromSelectors() : GetColumnsListFromEntity();
        }

        private string GetColumnsListFromEntity()
        {
            var names = typeof (TEntity).GetProperties()
                .Where(p => p.Name != "Id" && writablePropertyMatcher.Test(p.PropertyType))
                .Select(p => p.Name);
            return FormatColumnNames(names);
        }

        private string GetColumnsListFromSelectors()
        {
            var names = selectors.Select(GetMemberName);
            return FormatColumnNames(names);
        }

        private string GetValuesFromEntity()
        {
            var entityValues = typeof (TEntity).GetProperties()
                .Where(
                    p =>
                        p.Name != "Id" && writablePropertyMatcher.Test(p.PropertyType))
                .Select(p => p.GetValue(entity));
            return FormatValues(entityValues);
        }

        private string GetValuesList()
        {
            return selectors.Any() ? FormatValues(values) : GetValuesFromEntity();
        }
    }
}