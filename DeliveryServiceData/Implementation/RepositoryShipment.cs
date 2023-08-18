using Dapper;
using DeliveryServiceData.DTOs;
using DeliveryServiceDomain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceData.Implementation
{
    public class RepositoryShipment : IRepositoryShipment
    {
		private readonly DeliveryServiceContext context;

		public RepositoryShipment(DeliveryServiceContext context)
		{
			this.context = context;
		}

		public void Add(Shipment shipment)
		{
			try
			{
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[InsertShipment]";
                    var parameters = new DynamicParameters();
					parameters.Add("@ShipmentCode", shipment.ShipmentCode);
                    parameters.Add("@ShipmentWeightId", shipment.ShipmentWeightId);
                    parameters.Add("@ShipmentContent", shipment.ShipmentContent);
                    parameters.Add("@ContactPersonName", shipment.ContactPersonName);
                    parameters.Add("@ContactPersonPhone", shipment.ContactPersonPhone);
                    parameters.Add("@CustomerId", shipment.CustomerId);
                    parameters.Add("@Price", shipment.Price);
                    parameters.Add("@Note", shipment.Note);
                    parameters.Add("@Sending_City", shipment.Sending.City);
                    parameters.Add("@Sending_Street", shipment.Sending.Street);
                    parameters.Add("@Sending_PostalCode", shipment.Sending.PostalCode);
                    parameters.Add("@Receiving_City", shipment.Receiving.City);
                    parameters.Add("@Receiving_Street", shipment.Receiving.Street);
                    parameters.Add("@Receiving_PostalCode", shipment.Receiving.PostalCode);
                    parameters.Add("@ShipmentId", shipment.ShipmentId, DbType.Int32, ParameterDirection.Output);

                    var additionalServiceIds = shipment.AdditionalServices.Select(service => service.AdditionalServiceId);
                    var additionalServiceIdsJson = JsonConvert.SerializeObject(additionalServiceIds);
                    parameters.Add("@AdditionalServiceIds", additionalServiceIdsJson);

                    var person = connection.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);

                }
			}
			catch (Exception ex)
			{
				throw new Exception($"Error saving new shipment! {Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public Shipment FindByID(int id, params int[] ids)
		{
			try
			{
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetShipmentById]";
                    var parameters = new DynamicParameters();
                    parameters.Add("@ShipmentId", id);
                    var shipment = connection.QuerySingleOrDefault<Shipment>(procedure, parameters, commandType: CommandType.StoredProcedure);

                    return shipment;
                }
            }
			catch (Exception ex)
			{
				throw new Exception($"Error loading shipment!{Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public List<Shipment> GetAll()
		{
			try
			{
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetAllShipments]";
                    var shipments = connection.Query<Shipment>(procedure, commandType: CommandType.StoredProcedure);

                    return shipments.ToList();
                }
            }
			catch (Exception ex)
			{
				throw new Exception($"Error loading all shipments!{Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public Shipment FindByCode(string code)
		{
			try
			{
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetShipmentByShipmentCode]";
                    var parameters = new DynamicParameters();
                    parameters.Add("@ShipmentCode", code);
                    var shipment = connection.QuerySingleOrDefault<Shipment>(procedure, parameters, commandType: CommandType.StoredProcedure);

                    return shipment;
                }
            }
			catch (Exception ex)
			{
				throw new Exception($"Error loading shipment based on its code!{Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public List<Shipment> GetAllOfSpecifiedUser(int? userId)
		{
			try
			{
                using (var connection = context.CreateConnection())
                {
                    var procedure = "[dbo].[GetShipmentsByCustomerId]";
                    var parameters = new DynamicParameters();
                    parameters.Add("@CustomerId", userId);
                    var shipmentsDto = connection.Query<ShipmentDto>(procedure, parameters, commandType: CommandType.StoredProcedure);

                    var shipments = new List<Shipment>();

                    if(shipments != null)
                    {
                        foreach(var shipment in shipmentsDto)
                        {
                            shipments.Add(new Shipment
                            {
                                ShipmentId = shipment.ShipmentId,
                                ShipmentCode = shipment.ShipmentCode,
                                ShipmentWeightId = shipment.ShipmentWeightId,
                                ShipmentContent = shipment.ShipmentContent,
                                Sending = new Address
                                {
                                    City = shipment.Sending_City,
                                    Street = shipment.Sending_Street,
                                    PostalCode = shipment.Sending_PostalCode,
                                },
                                Receiving = new Address
                                {
                                    City = shipment.Receiving_City,
                                    Street = shipment.Receiving_Street,
                                    PostalCode = shipment.Receiving_PostalCode
                                },
                                ContactPersonName = shipment.ContactPersonName,
                                ContactPersonPhone = shipment.ContactPersonPhone,
                                CustomerId = shipment.CustomerId,
                                Price = shipment.Price,
                                Note = shipment.Note
                            });
                        }
                    }
                    return shipments.ToList();
                }
            }
			catch (Exception ex)
			{
				throw new Exception($"Error returning all shipments of a specific user!{Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public void RemoveShipment(int shipmentId)
		{
            using (var connection = context.CreateConnection())
            {
                var procedure = "[dbo].[DeleteShipment]";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", shipmentId);
                connection.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }
	}
}
