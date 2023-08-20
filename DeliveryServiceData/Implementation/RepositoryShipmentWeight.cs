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
        private readonly IDatabaseOperations _database;

        public RepositoryShipmentWeight(IDatabaseOperations database)
        {
            _database = database;
        }

        public ShipmentWeight FindByID(int id, params int[] ids)
        {
            try
            {
                var procedure = "[dbo].[GetShipmentWeightById]";
                var parameters = new DynamicParameters();
                parameters.Add("@ShipmentWeightId", id);

                return _database.QuerySingleOrDefault<ShipmentWeight>(procedure, parameters, commandType: CommandType.StoredProcedure);
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
                var procedure = "[dbo].[GetAllShipmentWeights]";

                return _database.Query<ShipmentWeight>(procedure, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading all shipment weights! {Environment.NewLine}" +
                                    $"System Error: {ex.Message}");
            }
        }
    }
}
