namespace AplicationLayer.DTOs
{
    public record CartResponse
    {
        public int Id { get; set; }
        public List<ArticleDto> Articles { get; set; } = new();
        public decimal TotalValue => Articles.Sum(a => a.Price * a.Quantity);
    }
}
