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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DeliveryServiceData.Implementation
{
    public class RepositoryAdditionalService :  IRepositoryAdditionalService
    {
		private readonly DapperContext context;

		public RepositoryAdditionalService(DapperContext context)
		{
			this.context = context;
		}

		public AdditionalService FindByID(int id, params int[] ids)
		{
			try
			{
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetAdditionalServiceById]";
                    var parameters = new DynamicParameters();
					parameters.Add("@AdditionalServiceId", id);
					var additionalService = connection.QuerySingleOrDefault<AdditionalService>(procedure, parameters, commandType: CommandType.StoredProcedure);

                    return additionalService;
                }
            }
			catch (Exception ex)
			{
				throw new Exception($"Error loading additional service! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public List<AdditionalService> GetAll()
		{
			try
			{
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetAllAdditionalServices]";
                    var parameters = new DynamicParameters();
                    var additionalServices = connection.Query<AdditionalService>(procedure, commandType: CommandType.StoredProcedure);

                    return additionalServices.ToList();
                }
            }
			catch (Exception ex)
			{
				throw new Exception($"Error returning all additional services! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}
	}
}
