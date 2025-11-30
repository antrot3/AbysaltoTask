using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AplicationLayer.Entities
{

    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public virtual Cart DeliveryCart { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required] public string DeliveryAddress { get; set; } = null!;
        [Required] public string DeliveryCountry { get; set; } = null!;
        [Required] public string DeliveryName { get; set; } = null!;
        [Required] public string DeliveryCardNumberEncrypted { get; set; } = null!;
        public bool IsDelivered { get; set; }
    }
}
