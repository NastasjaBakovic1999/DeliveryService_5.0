﻿using DeliveryServiceDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceData
{
    public interface IRepositoryShipment : IRepository<Shipment>
    {
        public void Add(Shipment shipment);
        public List<Shipment> GetAllOfSpecifiedUser(int? userId);
		public Shipment FindByCode(string code);
		public void RemoveShipment(Shipment shipment);
    }
}
