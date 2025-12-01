namespace AplicationLayer.DTOs
{
    public record CartDto
    {
        public List<ArticleDto> Articles { get; set; } = new();
        public decimal TotalValue => Articles.Sum(a => a.Price * a.Quantity);
    }
}
