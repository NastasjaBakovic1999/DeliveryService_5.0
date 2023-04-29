using DeliveryServiceDomain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceData.Implementation
{
    public class RepositoryShipment : GenericRepository<Shipment>, IRepositoryShipment
    {
        public RepositoryShipment(DbContext context) : base(context)
        {
        }

        public void Add(Shipment shipment)
        {
            var shipmentIdParameter = new SqlParameter("@ShipmentId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ShipmentCode", shipment.ShipmentCode),
                new SqlParameter("@ShipmentWeightId", shipment.ShipmentWeightId),
                new SqlParameter("@ShipmentContent", shipment.ShipmentContent),
                new SqlParameter("@ContactPersonName", shipment.ContactPersonName),
                new SqlParameter("@ContactPersonPhone", shipment.ContactPersonPhone),
                new SqlParameter("@CustomerId", shipment.CustomerId),
                new SqlParameter("@Price", shipment.Price),
                new SqlParameter("@Note", shipment.Note),
                new SqlParameter("@Sending_Id", shipment.Sending.Id),
				new SqlParameter("@Sending_City", shipment.Sending.City),
                new SqlParameter("@Sending_Street", shipment.Sending.Street),
                new SqlParameter("@Sending_PostalCode", shipment.Sending.PostalCode),
                new SqlParameter("@Receiving_Id", shipment.Sending.Id),
				new SqlParameter("@Receiving_City", shipment.Receiving.City),
                new SqlParameter("@Receiving_Street", shipment.Receiving.Street),
                new SqlParameter("@Receiving_PostalCode", shipment.Receiving.PostalCode)
		    };


			//Context.Set<Shipment>().ExecuteSqlCommand(@"EXEC @ShipmentId =  dbo.InsertShipment 
   //                                                                  @ShipmentCode,
   //                                                                  @ShipmentWeightId,
   //                                                                  @ShipmentContent,
   //                                                                  @ContactPersonName,
   //                                                                  @ContactPersonPhone,
   //                                                                  @CustomerId,
   //                                                                  @Price,
   //                                                                  @Note,
   //                                                                  @Sending_Id
   //                                                                  @Sending_City,
   //                                                                  @Sending_Street,
   //                                                                  @Sending_PostalCode,
   //                                                                  Receiving_Id,
   //                                                                  @Receiving_City,
   //                                                                  @Receiving_Street,
   //                                                                  @Receiving_PostalCode", parameters);

   //         var shipmentId = Convert.ToInt32(shipmentIdParameter.Value);
		}

        public List<Shipment> GetAllOfSpecifiedUser(int? userId)
        {
            return Context.Set<Shipment>().Where(sh => sh.CustomerId == userId).ToList();
        }

		public void RemoveShipment(Shipment shipment)
		{
            var existingAdditionalServiceShipments = Context.Set<AdditionalServiceShipment>().
                Where(ass => ass.ShipmentId == shipment.ShipmentId);

			var existingShipment = Context.Set<Shipment>().Find(shipment.ShipmentId);
			if (existingShipment != null)
			{
				Context.Entry(existingShipment).State = EntityState.Detached;
			}

            Context.Set<AdditionalServiceShipment>().RemoveRange(existingAdditionalServiceShipments);
			Context.Entry(shipment).State = EntityState.Deleted;
		}
	}
}
