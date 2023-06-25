using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceData.DTOs
{
    public class InsertShipmentDto
    {
        public string ShipmentCode;

        public int ShipmentWeightId;

        public string ShipmentContent;

        public string ContactPersonName;

        public string ContactPersonPhone;

        public int CustomerId;

        public double Price;

        public string Note;

        public string Sending_City;

        public string Sending_Street;

        public string Sending_PostalCode;

        public string Receiving_City;

        public string Receiving_Street;

        public string Receiving_PostalCode;
    }
}
