using DeliveryServiceData.Implementation;
using DeliveryServiceDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceData.UnitOfWork.Implementation
{
    public class PersonUnitOfWork : IPersonUnitOfWork
    {
        private readonly PersonContext _personContext;
        private readonly DapperContext _dapperContext;

        public PersonUnitOfWork(PersonContext personContext, DapperContext dapperContext)
        {
            _personContext = personContext;
            _dapperContext = dapperContext;
            Customer = new RepositoryCustomer(dapperContext);
            Person = new RepositoryPerson(dapperContext);
        }

        public IRepositoryCustomer Customer { get; set; }
        public IRepositoryPerson Person { get; set; }

        public void Commit()
        {
            _personContext.SaveChanges();
        }

        public void Dispose()
        {
            _personContext.Dispose();
        }
    }
}
