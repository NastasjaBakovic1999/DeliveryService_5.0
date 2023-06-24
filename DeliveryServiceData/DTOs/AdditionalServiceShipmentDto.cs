using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceData.DTOs
{
    public class AdditionalServiceShipmentDto
    {
        public int AdditionalServiceId { get; set; }
        public int ShipmentId { get; set; }
        public string AdditionalServiceName { get; set; }
    }
}
