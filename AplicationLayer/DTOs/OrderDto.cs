using AplicationLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicationLayer.DTOs
{
    public class OrderDto
    {
        public virtual CartDto CartResponse { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required] 
        public string DeliveryAddress { get; set; } = null!;
        [Required] 
        public string DeliveryCountry { get; set; } = null!;
        [Required]
        public string DeliveryName { get; set; } = null!;
        [Required]
        public string DeliveryCardNumberEncrypted { get; set; } = null!;
        public bool IsDelivered { get; set; }
    }

}
