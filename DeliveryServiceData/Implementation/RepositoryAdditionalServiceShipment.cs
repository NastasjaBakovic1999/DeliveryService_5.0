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
    public class RepositoryAdditionalServiceShipment : IRepositoryAdditionalServiceShipment
    {
		private readonly DapperContext context;

		public RepositoryAdditionalServiceShipment(DapperContext context)
		{
			this.context = context;
		}

		public void Add(AdditionalServiceShipment additionalServiceShipment)
		{
			try
			{
				// 
			}
			catch (Exception ex)
			{
				throw new Exception($"Error saving shipment and its additional services! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public AdditionalServiceShipment FindByID(int id, params int[] ids)
		{
			try
			{
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetAdditionalServiceShipmentByIDs]";
                    var parameters = new DynamicParameters();
                    parameters.Add("@AdditionalServiceId", id);
                    parameters.Add("@ShipmentId", ids[0]);
                    var additionalServiceShipment = connection.QuerySingleOrDefault<AdditionalServiceShipment>(procedure, parameters, commandType: CommandType.StoredProcedure);

                    return additionalServiceShipment;
                }
            }
			catch (Exception ex)
			{
				throw new Exception($"Error loading shipment and its additional service! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public List<AdditionalServiceShipment> GetAll()
		{
			try
			{
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetAllAdditionalServiceShipments]";
                    var additionalServiceShipments = connection.Query<AdditionalServiceShipment>(procedure, commandType: CommandType.StoredProcedure);

                    return additionalServiceShipments.ToList();
                }
            }
			catch (Exception ex)
			{
				throw new Exception($"Error returning all shipments and their additional services! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}
	}
}
