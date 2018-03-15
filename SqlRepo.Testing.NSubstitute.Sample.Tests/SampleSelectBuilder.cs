using System;
using SqlRepo.Abstractions;

namespace SqlRepo.Testing.NSubstitute.Sample.Tests
{
    internal class SampleSelectBuilder
    {
        private readonly ISelectStatement<TestEntity> selectStatement;

        public SampleSelectBuilder(ISelectStatement<TestEntity> selectStatement)
        {
            this.selectStatement = selectStatement;
        }

        public void ExecuteSimpleSelectAll()
        {
            this.selectStatement.SelectAll()
                .Go();
        }

        public void ExecuteSimpleSelectAllAsync()
        {
            this.selectStatement.SelectAll()
                .GoAsync();
        }

        public void BuildSingleMemberSelect()
        {
            this.selectStatement.Select(e => e.StringProperty);
        }
    }
}