using DeliveryServiceDomain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceData.Implementation
{
    public class RepositoryCustomer : IRepositoryCustomer
    {
		private readonly PersonContext context;

		public RepositoryCustomer(PersonContext context)
		{
			this.context = context;
		}

		public Customer FindByID(int id, params int[] ids)
		{
			try
			{
				return (Customer)context.Persons.FromSqlRaw("GetCustomerById {0}", id).FirstOrDefault();

			}
			catch (Exception ex)
			{
				throw new Exception($"Error loading user! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public List<Customer> GetAll()
		{
			try
			{
				return context.Customers.FromSqlRaw("GetAllCustomers").ToList();
			}
			catch (Exception ex)
			{
				throw new Exception($"Error loading all users! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public void Edit(Customer entity)
		{
			try
			{
				var parameters = new List<SqlParameter>()
				{
					new SqlParameter("@Address", entity.Address),
					new SqlParameter("@PostalCode", entity.PostalCode)
				};
				context.Customers.FromSqlRaw("EditCustomer @Address, @PostalCode", parameters).ToList();
			}
			catch (Exception ex)
			{
				throw new Exception($"Error changing user data! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}
	}
}
