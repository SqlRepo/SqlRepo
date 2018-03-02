namespace SqlRepo.Benchmark
{
    public interface IBenchmarkOperation
    {
        BenchmarkResult Run();
        string GetNotes();
        Component Component { get; set; }
    }
}