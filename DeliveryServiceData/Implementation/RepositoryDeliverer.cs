﻿using DeliveryServiceDomain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceData.Implementation
{
    public class RepositoryDeliverer : GenericRepository<Deliverer>, IRepositoryDeliverer
    {
        public RepositoryDeliverer(DbContext context) : base(context)
        {
        }
    }
}
