using System.ComponentModel.DataAnnotations;

namespace AplicationLayer.Entities
{

    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int CartId { get; set; }

        public Cart DeliveryCart { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string DeliveryAdress { get; set; } = string.Empty;
        public string DeliveryCountry { get; set; } = string.Empty;
        public string DeliveryName { get; set; } = string.Empty;
        public string DeliveryCardNumber { get; set; } = string.Empty;
        public bool IsDelivered { get; set; }
    }
}
