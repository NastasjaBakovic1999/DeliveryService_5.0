using DeliveryServiceData.Implementation;
using DeliveryServiceDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceData.UnitOfWork.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DeliveryServiceContext _deliveryServiceContext;

        public UnitOfWork(DeliveryServiceContext deliveryServiceContext)
        {
            _deliveryServiceContext = deliveryServiceContext;
            Shipment = new RepositoryShipment(deliveryServiceContext);
            AdditionalService = new RepositoryAdditionalService(deliveryServiceContext);
            AdditionalServiceShipment = new RepositoryAdditionalServiceShipment(deliveryServiceContext);
            ShipmentWeight = new RepositoryShipmentWeight(deliveryServiceContext);
        }

        public IRepositoryAdditionalService AdditionalService { get; set; }
        public IRepositoryAdditionalServiceShipment AdditionalServiceShipment { get; set; }
        public IRepositoryShipment Shipment { get; set; }
        public IRepositoryShipmentWeight ShipmentWeight { get; set; }

        public void Commit()
        {
            _deliveryServiceContext.SaveChanges();
        }

        public void Dispose()
        {
            _deliveryServiceContext.Dispose();
        }
    }
}
