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
    public class PersonUnitOfWork : IPersonUnitOfWork
    {
        private readonly IDatabaseOperations _databaseOperations;

        public PersonUnitOfWork(IDatabaseOperations databaseOperations)
        {
            _databaseOperations = databaseOperations;
            Customer = new RepositoryCustomer(databaseOperations);
        }

        public IRepositoryCustomer Customer { get; set; }
    }
}
