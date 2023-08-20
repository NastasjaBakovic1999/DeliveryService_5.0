using DeliveryServiceData.Implementation;
using DeliveryServiceDomain;
using DeliveryServiceDomain.DatabaseOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceData.UnitOfWork.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseOperations _databaseOperations;
        public UnitOfWork(IDatabaseOperations databaseOperations)
        {
            _databaseOperations = databaseOperations;
            Shipment = new RepositoryShipment(databaseOperations);
            AdditionalService = new RepositoryAdditionalService(databaseOperations);
            AdditionalServiceShipment = new RepositoryAdditionalServiceShipment(databaseOperations);
            ShipmentWeight = new RepositoryShipmentWeight(databaseOperations);
        }

        public IRepositoryAdditionalService AdditionalService { get; set; }
        public IRepositoryAdditionalServiceShipment AdditionalServiceShipment { get; set; }
        public IRepositoryShipment Shipment { get; set; }
        public IRepositoryShipmentWeight ShipmentWeight { get; set; }
    }
}
