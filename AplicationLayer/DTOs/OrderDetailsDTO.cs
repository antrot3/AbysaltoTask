using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicationLayer.DTOs
{
    public class OrderDetailsDTO
    {
        public string DeliveryAddress { get; set; }
        public string DeliveryCountry { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryCardNumber { get; set; }
    }
}
