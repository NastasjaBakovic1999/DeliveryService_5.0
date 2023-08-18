using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceData.DTOs
{
    public class ShipmentDto
    {
        public int ShipmentId { get; set; }
        public string ShipmentCode { get; set; }
        public int ShipmentWeightId { get; set; }
        public string ShipmentContent { get; set; }
        public string Sending_City { get; set; }
        public string Sending_Street { get; set; }
        public string Sending_PostalCode { get; set; }
        public string Receiving_City { get; set; }
        public string Receiving_Street { get; set; }
        public string Receiving_PostalCode { get; set; }
        public string ContactPersonPhone { get; set; }
        public string ContactPersonName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double Price { get; set; }
        public string Note { get; set; }

    }
}
