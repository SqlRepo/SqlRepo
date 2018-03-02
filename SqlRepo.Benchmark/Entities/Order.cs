namespace SqlRepo.Benchmark.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalCost { get; set; }
    }
}