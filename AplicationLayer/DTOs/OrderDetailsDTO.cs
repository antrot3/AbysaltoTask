using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicationLayer.DTOs
{
    public class OrderDetailsDTO
    {
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public string DeliveryCountry { get; set; }
        [Required]
        public string DeliveryName { get; set; }
        [Required]
        public string DeliveryCardNumber { get; set; }
    }
}
