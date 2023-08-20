using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceData.UnitOfWork
{
    public interface IPersonUnitOfWork
    {
        public IRepositoryCustomer Customer { get; set; }
    }
}
