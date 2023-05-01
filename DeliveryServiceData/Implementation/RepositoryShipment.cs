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
				var parameters = new List<SqlParameter>()
				{
					new SqlParameter("@ShipmentCode", shipment.ShipmentCode),
					new SqlParameter("@ShipmentWeightId", shipment.ShipmentWeightId),
					new SqlParameter("@ShipmentContent", shipment.ShipmentContent),
					new SqlParameter("@ContactPersonName", shipment.ContactPersonName),
					new SqlParameter("@ContactPersonPhone", shipment.ContactPersonPhone),
					new SqlParameter("@CustomerId", shipment.CustomerId),
					new SqlParameter("@Price", shipment.Price),
					new SqlParameter("@Note", shipment.Note),
					new SqlParameter("@Sending_City", shipment.Sending.City),
					new SqlParameter("@Sending_Street", shipment.Sending.Street),
					new SqlParameter("@Sending_PostalCode", shipment.Sending.PostalCode),
					new SqlParameter("@Receiving_City", shipment.Receiving.City),
					new SqlParameter("@Receiving_Street", shipment.Receiving.Street),
					new SqlParameter("@Receiving_PostalCode", shipment.Receiving.PostalCode)
				};
				 context.Shipments.FromSqlRaw<Shipment>(@"InsertShipment 
																@ShipmentCode,
																@ShipmentWeightId,
																@ShipmentContent,
																@ContactPersonName,
																@ContactPersonPhone,
																@CustomerId,
																@Price,
																@Note,
																@Sending_City,
																@Sending_Street,
																@Sending_PostalCode,
																@Receiving_City,
																@Receiving_Street,
																@Receiving_PostalCode", parameters.ToArray());
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
				return context.Shipments.FromSqlRaw<Shipment>("GetShipmentById").FirstOrDefault();
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
				return context.Shipments.FromSqlRaw<Shipment>("GetAllShipments").ToList();
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
				return context.Shipments.FromSqlRaw<Shipment>("GetShipmentByShipmentCode {0}", code).FirstOrDefault();
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
				return context.Shipments.FromSqlRaw<Shipment>("GetShipmentsByCustomerId {0}", userId).AsEnumerable().ToList();
			}
			catch (Exception ex)
			{
				throw new Exception($"Error returning all shipments of a specific user!{Environment.NewLine}" +
									$"System Error: {ex.Message}");
			}
		}

		public void RemoveShipment(Shipment shipment)
		{
			var existingAdditionalServiceShipments = context.Set<AdditionalServiceShipment>().
				Where(ass => ass.ShipmentId == shipment.ShipmentId);

			var existingShipment = context.Set<Shipment>().Find(shipment.ShipmentId);
			if (existingShipment != null)
			{
				context.Entry(existingShipment).State = EntityState.Detached;
			}

			context.Set<AdditionalServiceShipment>().FromSqlRaw("DeleteShipment {0}", shipment.ShipmentId);
			context.Entry(shipment).State = EntityState.Deleted;
		}
	}
}
