using FluentAssertions;
using NUnit.Framework;

namespace SqlRepo.Benchmark.Tests
{
    public abstract class SqlRepoBenchmarkTestBase : BenchmarkOperationTestBase
    {
        [Test]
        public void ShouldReturnCorrectComponent()
        {
            AssumeTargetIsExecuted().Component.Should().Be(Component.SqlRepo.ToString());
        }
    }
}