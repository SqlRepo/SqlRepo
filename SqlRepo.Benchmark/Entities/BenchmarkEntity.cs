namespace SqlRepo.Benchmark.Entities
{
    public class BenchmarkEntity
    {
        public int Id { get; set; }
        public string TextValue { get; set; }
        public int IntegerValue { get; set; }
        public decimal DecimalValue { get; set; }
        public bool? NullableBoolean { get; set; }
    }
}