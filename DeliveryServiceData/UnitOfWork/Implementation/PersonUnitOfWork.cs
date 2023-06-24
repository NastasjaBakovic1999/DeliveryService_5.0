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

        public PersonUnitOfWork(PersonContext personContext)
        {
            _personContext = personContext;
            Customer = new RepositoryCustomer(personContext);
        }

        public IRepositoryCustomer Customer { get; set; }

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
