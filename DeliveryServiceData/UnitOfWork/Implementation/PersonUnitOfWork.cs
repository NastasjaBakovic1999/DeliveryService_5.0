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
        private readonly PersonContext context;

        public PersonUnitOfWork(PersonContext context)
        {
            this.context = context;
            Customer = new RepositoryCustomer(context);
            Person = new RepositoryPerson(context);
        }

        public IRepositoryCustomer Customer { get; set; }
        public IRepositoryPerson Person { get; set; }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
