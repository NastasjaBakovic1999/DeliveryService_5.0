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
        private readonly IDatabaseOperations _database;

        public RepositoryAdditionalService(IDatabaseOperations database)
        {
            _database = database;
        }

        public AdditionalService FindByID(int id, params int[] ids)
        {
            try
            {
                var procedure = "[dbo].[GetAdditionalServiceById]";
                var parameters = new DynamicParameters();
                parameters.Add("@AdditionalServiceId", id);

                return _database.QuerySingleOrDefault<AdditionalService>(procedure, parameters, commandType: CommandType.StoredProcedure);
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
                var procedure = "[dbo].[GetAllAdditionalServices]";

                return _database.Query<AdditionalService>(procedure, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error returning all additional services! {Environment.NewLine}" +
                                    $"System Error: {ex.Message}");
            }
        }
    }
}
