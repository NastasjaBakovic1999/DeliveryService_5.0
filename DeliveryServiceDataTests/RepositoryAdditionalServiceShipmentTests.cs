using Dapper;
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
    public class RepositoryAdditionalServiceShipmentTests
    {
        [Fact]
        public void GetAll_ReturnsListOfCustomers()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Query<AdditionalServiceShipment>(It.IsAny<string>(), CommandType.StoredProcedure))
                .Returns(GetSampleAdditionalServiceShipments());

            var repository = new RepositoryAdditionalServiceShipment(databaseMock.Object);

            var result = repository.GetAll();

            Assert.NotNull(result);
            Assert.Equal(GetSampleAdditionalServiceShipments().Count, result.Count);
            foreach (var customer in result)
            {
                Assert.NotEqual(0, customer.ShipmentId);
                Assert.NotEqual(0, customer.AdditionalServiceId);
            }
        }

        [Fact]
        public void GetAll_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Query<AdditionalServiceShipment>(It.IsAny<string>(), CommandType.StoredProcedure))
                .Throws(new Exception("Database error occurred!"));

            var repository = new RepositoryAdditionalServiceShipment(databaseMock.Object);

            Assert.Throws<Exception>(() => repository.GetAll());
        }

        [Fact]
        public void FindById_ReturnValidAdditionalServiceShipment()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<AdditionalServiceShipment>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Returns((string procedure, object parameters, CommandType commandType) =>
                {
                    var inputParams = (DynamicParameters)parameters;
                    var additionalServiceId = inputParams.Get<int>("@AdditionalServiceId");
                    var shipmentId = inputParams.Get<int>("@ShipmentId");
                    var additionalServiceShipment = GetSampleAdditionalServiceShipments().FirstOrDefault(s => s.AdditionalServiceId == additionalServiceId &&
                                                                            s.ShipmentId == shipmentId);
                    return additionalServiceShipment;
                });

            var repository = new RepositoryAdditionalServiceShipment(databaseMock.Object);

            var result = repository.FindByID(1, 2);

            Assert.NotNull(result);
            Assert.Equal(1, result.AdditionalServiceId);
            Assert.Equal(2, result.ShipmentId);
        }

        [Fact]
        public void FindById_ReturnsNullForInvalidId()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<AdditionalServiceShipment>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Returns((string procedure, object parameters, CommandType commandType) =>
                {
                    var inputParams = (DynamicParameters)parameters;
                    var additionalServiceId = inputParams.Get<int>("@AdditionalServiceId");
                    var shipmentId = inputParams.Get<int>("@ShipmentId");
                    var shipmentWeight = GetSampleAdditionalServiceShipments().FirstOrDefault(s => s.AdditionalServiceId == additionalServiceId &&
                                                                            s.ShipmentId == shipmentId);
                    return shipmentWeight;
                });

            var repository = new RepositoryAdditionalServiceShipment(databaseMock.Object);

            var result = repository.FindByID(999, 999);

            Assert.Null(result);
        }

        [Fact]
        public void FindById_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<AdditionalServiceShipment>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                    .Throws(new Exception("Database error occurred"));

            var repository = new RepositoryAdditionalServiceShipment(databaseMock.Object);

            Assert.Throws<Exception>(() => repository.FindByID(1));
        }

        [Fact]
        public void Add_SavesAdditionalServiceShipment()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            var repository = new RepositoryAdditionalServiceShipment(databaseMock.Object);

            AdditionalServiceShipment shipmentToAdd = new AdditionalServiceShipment
            {
                AdditionalServiceId = 1,
                ShipmentId = 101
            };

            repository.Add(shipmentToAdd);

            databaseMock.Verify(db => db.Execute(
                "[dbo].[InsertAdditionalServiceShipment]",
                It.Is<DynamicParameters>(param =>
                    param.Get<int>("@AdditionalServiceId") == shipmentToAdd.AdditionalServiceId &&
                    param.Get<int>("@ShipmentId") == shipmentToAdd.ShipmentId),
                CommandType.StoredProcedure),
                Times.Once
            );
        }

        [Fact]
        public void Add_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Throws(new Exception("Database error occurred"));

            var repository = new RepositoryAdditionalServiceShipment(databaseMock.Object);

            AdditionalServiceShipment shipmentToAdd = new AdditionalServiceShipment
            {
                AdditionalServiceId = 1,
                ShipmentId = 101
            };

            Assert.Throws<Exception>(() => repository.Add(shipmentToAdd));
        }

        private List<AdditionalServiceShipment> GetSampleAdditionalServiceShipments()
        {
            List<AdditionalServiceShipment> output = new List<AdditionalServiceShipment>
            {
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 1,
                    ShipmentId = 1
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 2,
                    ShipmentId = 1
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 3,
                    ShipmentId = 1
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 1,
                    ShipmentId = 2
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 2,
                    ShipmentId = 2
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 5,
                    ShipmentId = 3
                }
            };
            return output;
        }
    }
}
