using Dapper;
using DeliveryServiceDomain;
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
    public class RepositoryPerson : IRepositoryPerson
    {
		private readonly DapperContext context;

		public RepositoryPerson(DapperContext context)
		{
			this.context = context;
		}

		public Person FindByID(int id, params int[] ids)
		{
			try
			{
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetPersonById]";
                    var parameters = new DynamicParameters();
                    parameters.Add("@PersonId", id);
                    var person = connection.QuerySingleOrDefault<Person>(procedure, parameters, commandType: CommandType.StoredProcedure);

                    return person;
                }
            }
			catch (Exception ex)
			{
				throw new Exception($"Error loading person! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public List<Person> GetAll()
		{
			try
			{
               //
			   return new List<Person> { new Person() };
            }
			catch (Exception ex)
			{
				throw new Exception($"Error loading all persons! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}
	}
}
