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
				context.Shipments.Add(shipment);
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
				return context.Shipments.Find(id);
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
				return context.Shipments.ToList();
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
				return context.Shipments.SingleOrDefault(s => s.ShipmentCode == code);
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
				return context.Shipments.Where(s => s.CustomerId == userId).ToList();
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

			context.Set<AdditionalServiceShipment>().RemoveRange(existingAdditionalServiceShipments);
			context.Entry(shipment).State = EntityState.Deleted;
		}
	}
}
