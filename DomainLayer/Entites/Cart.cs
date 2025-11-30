using System.ComponentModel.DataAnnotations;

namespace AplicationLayer.Entities
{

    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual List<CartItem> Items { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp] // Concurrency token
        public byte[] RowVersion { get; set; } = null!;
    }
}
