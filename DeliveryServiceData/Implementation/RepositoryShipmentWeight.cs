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
	internal class RepositoryShipmentWeight : IRepositoryShipmentWeight
	{
		private readonly DeliveryServiceContext context;

		public RepositoryShipmentWeight(DeliveryServiceContext context)
		{
			this.context = context;
		}

		public ShipmentWeight FindByID(int id, params int[] ids)
		{
			try
			{
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetShipmentWeightById]";
                    var parameters = new DynamicParameters();
                    parameters.Add("@ShipmentWeightId", id);
                    var shipmentWeight = connection.QuerySingleOrDefault<ShipmentWeight>(procedure, parameters, commandType: CommandType.StoredProcedure);

                    return shipmentWeight;
                }
            }
			catch (Exception ex)
			{
				throw new Exception($"Error loading shipment weight! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public List<ShipmentWeight> GetAll()
		{
			try
			{
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetShipmentWeightById]";
                    var shipmentWeights = connection.Query<ShipmentWeight>(procedure, commandType: CommandType.StoredProcedure);

                    return shipmentWeights.ToList();
                }
            }
			catch (Exception ex)
			{
				throw new Exception($"Error loading all shipment weights! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}
	}
}
