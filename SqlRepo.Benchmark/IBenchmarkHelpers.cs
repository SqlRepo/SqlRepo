using System;

namespace SqlRepo.Benchmark
{
    public interface IBenchmarkHelpers
    {
        void ClearBufferPool();
        void ClearRecords();
        void InsertRecords(int amount);
        void RunActionMultiple(Action action, int timesToRun);
    }
}