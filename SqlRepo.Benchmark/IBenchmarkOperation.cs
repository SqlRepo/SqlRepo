namespace SqlRepo.Benchmark
{
    public interface IBenchmarkOperation
    {
        BenchmarkResult Run();
        string GetNotes();
    }
}