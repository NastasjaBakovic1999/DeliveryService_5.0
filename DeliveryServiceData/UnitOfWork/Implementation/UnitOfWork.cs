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
        private readonly DapperContext _dapperContext;

        public UnitOfWork(DeliveryServiceContext deliveryServiceContext, DapperContext dapperContext)
        {
            _deliveryServiceContext = deliveryServiceContext;
            _dapperContext = dapperContext;
            Shipment = new RepositoryShipment(dapperContext);
            AdditionalService = new RepositoryAdditionalService(dapperContext);
            AdditionalServiceShipment = new RepositoryAdditionalServiceShipment(dapperContext);
            ShipmentWeight = new RepositoryShipmentWeight(dapperContext);
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
