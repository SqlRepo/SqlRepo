using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class SqlLoggerShould
    {
        [SetUp]
        public void Setup()
        {
            this.sqlLogWriter = Substitute.For<ISqlLogWriter>();

            var sqlLogWriters = new List<ISqlLogWriter>();
            sqlLogWriters.Add(this.sqlLogWriter);

            this.sqlLogger = new SqlLogger(sqlLogWriters);
        }

        [Test]
        public void WriteLogUsingSqlLogWriter()
        {
            var testSql = "Test SQL";
            this.sqlLogger.Log(testSql);
            this.sqlLogWriter.Received()
                .Log(testSql);
        }

        private ISqlLogger sqlLogger;
        private ISqlLogWriter sqlLogWriter;
    }
}