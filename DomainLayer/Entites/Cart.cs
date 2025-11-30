using System.ComponentModel.DataAnnotations;

namespace AplicationLayer.Entities
{

    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        // Store articles as JSON
        public string ArticlesJson { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
