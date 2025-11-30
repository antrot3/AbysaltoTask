using System.ComponentModel.DataAnnotations;

namespace AplicationLayer.Entities
{

    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public Cart DeliveryCart { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string DeliveryAdress { get; set; }
        public string DeliveryCountry { get; set; }
        public string DeliveryName{ get; set; }
        public string DeliveryCardNumber { get; set; }

        public bool IsDelivered;
    }
}
