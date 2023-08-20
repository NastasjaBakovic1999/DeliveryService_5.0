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
        private readonly IDatabaseOperations _database;

        public RepositoryAdditionalServiceShipment(IDatabaseOperations database)
        {
            _database = database;
        }

        public void Add(AdditionalServiceShipment additionalServiceShipment)
        {
            try
            {
                var procedure = "[dbo].[InsertAdditionalServiceShipment]";
                var parameters = new DynamicParameters();
                parameters.Add("@AdditionalServiceId", additionalServiceShipment.AdditionalServiceId);
                parameters.Add("@ShipmentId", additionalServiceShipment.ShipmentId);

                _database.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);
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
                var procedure = "[dbo].[GetAdditionalServiceShipmentByIDs]";
                var parameters = new DynamicParameters();
                parameters.Add("@AdditionalServiceId", id);
                parameters.Add("@ShipmentId", ids[0]);

                return _database.QuerySingleOrDefault<AdditionalServiceShipment>(procedure, parameters, commandType: CommandType.StoredProcedure);
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
                var procedure = "[dbo].[GetAllAdditionalServiceShipments]";

                return _database.Query<AdditionalServiceShipment>(procedure, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error returning all shipments and their additional services! {Environment.NewLine}" +
                                    $"System Error: {ex.Message}");
            }
        }
    }
}
