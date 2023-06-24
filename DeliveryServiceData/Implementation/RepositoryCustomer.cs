using Dapper;
using DeliveryServiceDomain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetCustomerById]";
                    var parameters = new DynamicParameters();
                    parameters.Add("@CustomerId", id);
                    var customer = connection.QuerySingleOrDefault<Customer>(procedure, parameters, commandType: CommandType.StoredProcedure);

                    return customer;
                }
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
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetAllCustomers]";
                    var customers = connection.Query<Customer>(procedure, commandType: CommandType.StoredProcedure);

                    return customers.ToList();
                }
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
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[EditCustomer]";
                    var parameters = new DynamicParameters();
					parameters.Add("@Address", entity.Address, DbType.String);
					parameters.Add("@PostalCode", entity.PostalCode, DbType.String);
					parameters.Add("@CustomerId", entity.Id, DbType.Int32);
                    var customers = connection.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);
                }
            }
			catch (Exception ex)
			{
				throw new Exception($"Error changing user data! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}
	}
}
