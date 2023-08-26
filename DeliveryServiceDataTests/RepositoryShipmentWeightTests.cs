using Dapper;
using DeliveryServiceData;
using DeliveryServiceData.Implementation;
using DeliveryServiceDomain;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceDataTests
{
    public class RepositoryShipmentWeightTests
    {
        [Fact]
        public void GetAll_ReturnsListOfShipmentWeights()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Query<ShipmentWeight>(It.IsAny<string>(), CommandType.StoredProcedure))
                .Returns(GetSampleShipmentWeights());

            var repository = new RepositoryShipmentWeight(databaseMock.Object);

            var result = repository.GetAll();

            Assert.NotNull(result);
            Assert.Equal(GetSampleShipmentWeights().Count, result.Count);
            foreach (var additionalService in result)
            {
                Assert.NotEqual(0, additionalService.ShipmentWeightId);
                Assert.NotNull(additionalService.ShipmentWeightDescription);
                Assert.NotEqual(0.0, additionalService.ShipmentWeightPrice);
            }
        }

        [Fact]
        public void GetAll_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Query<ShipmentWeight>(It.IsAny<string>(), CommandType.StoredProcedure))
                .Throws(new Exception("Database error occurred!"));

            var repository = new RepositoryShipmentWeight(databaseMock.Object);

            Assert.Throws<Exception>(() => repository.GetAll());
        }

        [Fact]
        public void FindById_ReturnValidShipmentWeight()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<ShipmentWeight>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Returns((string procedure, object parameters, CommandType commandType) =>
                {
                    var inputParams = (DynamicParameters)parameters;
                    var id = inputParams.Get<int>("@ShipmentWeightId");
                    var shipmentWeight = GetSampleShipmentWeights().FirstOrDefault(s => s.ShipmentWeightId == id);
                    return shipmentWeight;
                });

            var repository = new RepositoryShipmentWeight(databaseMock.Object);

            var result = repository.FindByID(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.ShipmentWeightId);
            Assert.NotNull(result.ShipmentWeightDescription);
            Assert.NotEqual(0.0, result.ShipmentWeightPrice);
        }

        [Fact]
        public void FindById_ReturnsNullForInvalidId()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<ShipmentWeight>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Returns((string procedure, object parameters, CommandType commandType) =>
                {
                    var inputParams = (DynamicParameters)parameters;
                    var id = inputParams.Get<int>("@ShipmentWeightId");
                    var shipmentWeight = GetSampleShipmentWeights().FirstOrDefault(s => s.ShipmentWeightId == id);
                    return shipmentWeight;
                });

            var repository = new RepositoryShipmentWeight(databaseMock.Object);

            var result = repository.FindByID(999);

            Assert.Null(result);
        }

        [Fact]
        public void FindById_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<ShipmentWeight>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                    .Throws(new Exception("Database error occurred"));

            var repository = new RepositoryShipmentWeight(databaseMock.Object);

            Assert.Throws<Exception>(() => repository.FindByID(1));
        }


        private List<ShipmentWeight> GetSampleShipmentWeights()
        {
            List<ShipmentWeight> output = new List<ShipmentWeight>
            {
                new ShipmentWeight
                {
                    ShipmentWeightId = 1,
                    ShipmentWeightDescription = "Up to 1 kg",
                    ShipmentWeightPrice = 10.99,
                    Shipments = new List<Shipment>()
                },

                new ShipmentWeight
                {
                    ShipmentWeightId = 2,
                    ShipmentWeightDescription = "1 - 5 kg",
                    ShipmentWeightPrice = 18.75,
                    Shipments = new List<Shipment>()
                },

                new ShipmentWeight
                {
                    ShipmentWeightId = 3,
                    ShipmentWeightDescription = "5 - 10 kg",
                    ShipmentWeightPrice = 25.50,
                    Shipments = new List<Shipment>()
                },

                new ShipmentWeight
                {
                    ShipmentWeightId = 4,
                    ShipmentWeightDescription = "10 - 20 kg",
                    ShipmentWeightPrice = 40.25,
                    Shipments = new List<Shipment>()
                },

                new ShipmentWeight
                {
                    ShipmentWeightId = 5,
                    ShipmentWeightDescription = "20 - 30 kg",
                    ShipmentWeightPrice = 55.00,
                    Shipments = new List<Shipment>()
                },

                new ShipmentWeight
                {
                    ShipmentWeightId = 6,
                    ShipmentWeightDescription = "Over 30 kg",
                    ShipmentWeightPrice = 70.50,
                    Shipments = new List<Shipment>()
                }
            };
            return output;
        }
    }
}
