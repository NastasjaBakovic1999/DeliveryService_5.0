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
    public class RepositoryShipmentTests
    {
        [Fact]
        public void GetAll_ReturnsListOfShipments()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Query<Shipment>(It.IsAny<string>(), CommandType.StoredProcedure))
                .Returns(GetSampleShipments());

            var repository = new RepositoryShipment(databaseMock.Object);

            var result = repository.GetAll();

            Assert.NotNull(result);
            Assert.Equal(GetSampleShipments().Count, result.Count);
            foreach (var shipment in result)
            {
                Assert.NotEqual(0, shipment.ShipmentId);
                Assert.NotNull(shipment.ShipmentCode);
                Assert.NotEqual(0, shipment.ShipmentWeightId);
                Assert.NotNull(shipment.ShipmentContent);
                Assert.NotNull(shipment.ContactPersonName);
                Assert.NotNull(shipment.ContactPersonPhone);
                Assert.NotEqual(0, shipment.CustomerId);
                Assert.NotEqual(0.0, shipment.Price);
            }
        }

        [Fact]
        public void GetAll_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Query<Shipment>(It.IsAny<string>(), CommandType.StoredProcedure))
                .Throws(new Exception("Database error occurred!"));

            var repository = new RepositoryShipment(databaseMock.Object);

            Assert.Throws<Exception>(() => repository.GetAll());
        }

        [Fact]
        public void FindById_ReturnValidShipment()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<Shipment>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Returns((string procedure, object parameters, CommandType commandType) =>
                {
                    var inputParams = (DynamicParameters)parameters;
                    var id = inputParams.Get<int>("@ShipmentId");
                    var shipment = GetSampleShipments().FirstOrDefault(s => s.ShipmentId == id);
                    return shipment;
                });

            var repository = new RepositoryShipment(databaseMock.Object);

            var result = repository.FindByID(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.ShipmentId);
            Assert.NotEqual(0, result.ShipmentId);
            Assert.NotNull(result.ShipmentCode);
            Assert.NotEqual(0, result.ShipmentWeightId);
            Assert.NotNull(result.ShipmentContent);
            Assert.NotNull(result.ContactPersonName);
            Assert.NotNull(result.ContactPersonPhone);
            Assert.NotEqual(0, result.CustomerId);
            Assert.NotEqual(0.0, result.Price);
        }

        [Fact]
        public void FindById_ReturnsNullForInvalidId()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<Shipment>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Returns((string procedure, object parameters, CommandType commandType) =>
                {
                    var inputParams = (DynamicParameters)parameters;
                    var id = inputParams.Get<int>("@ShipmentId");
                    var shipment = GetSampleShipments().FirstOrDefault(s => s.ShipmentId == id);
                    return shipment;
                });

            var repository = new RepositoryShipment(databaseMock.Object);

            var result = repository.FindByID(999);

            Assert.Null(result);
        }

        [Fact]
        public void FindById_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<Shipment>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                    .Throws(new Exception("Database error occurred"));

            var repository = new RepositoryShipment(databaseMock.Object);

            Assert.Throws<Exception>(() => repository.FindByID(1));
        }


        private List<Shipment> GetSampleShipments()
        {
            return new List<Shipment>
            {
                new Shipment
                {
                    ShipmentId = 1,
                    ShipmentCode = "SH123456",
                    ShipmentWeightId = 2,
                    ShipmentWeight = new ShipmentWeight
                    {
                        ShipmentWeightId = 2,
                        ShipmentWeightDescription = "1 - 5 kg",
                        ShipmentWeightPrice = 18.75,
                        Shipments = new List<Shipment>()
                    },
                    ShipmentContent = "Electronics",
                    Sending = new Address
                    {
                        Street = "123 Main Street",
                        City = "Cityville",
                        PostalCode = "12345"
                    },
                    Receiving = new Address
                    {
                        Street = "456 Elm Avenue",
                        City = "Townsville",
                        PostalCode = "67890"
                    },
                    ContactPersonName = "John Doe",
                    ContactPersonPhone = "555-123-4567",
                    CustomerId = 3,
                    Customer = new Customer
                    {
                        Id = 3,
                        FirstName = "Alice",
                        LastName = "Smith",
                        Email = "alice@example.com"
                    },
                    Price = 25.50,
                    AdditionalServices = new List<AdditionalServiceShipment>
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
                        }
                    },
                    Note = "Fragile items"
                },
                new Shipment
                {
                    ShipmentId = 2,
                    ShipmentCode = "SH987654",
                    ShipmentWeightId = 3,
                    ShipmentWeight = new ShipmentWeight
                    {
                        ShipmentWeightId = 3,
                        ShipmentWeightDescription = "5 - 10 kg",
                        ShipmentWeightPrice = 28.50,
                        Shipments = new List<Shipment>()
                    },
                    ShipmentContent = "Clothing",
                    Sending = new Address
                    {
                        Street = "789 Oak Lane",
                        City = "Villagetown",
                        PostalCode = "54321"
                    },
                    Receiving = new Address
                    {
                        Street = "321 Pine Road",
                        City = "Hamletville",
                        PostalCode = "87654"
                    },
                    ContactPersonName = "Jane Smith",
                    ContactPersonPhone = "555-987-6543",
                    CustomerId = 4,
                    Customer = new Customer
                    {
                        Id = 4,
                        FirstName = "Bob",
                        LastName = "Johnson",
                        Email = "bob@example.com"
                    },
                    Price = 35.75,
                    AdditionalServices = new List<AdditionalServiceShipment>(),
                    Note = "Handle with care"
                },
                new Shipment
                {
                    ShipmentId = 3,
                    ShipmentCode = "SH555666",
                    ShipmentWeightId = 1,
                    ShipmentWeight = new ShipmentWeight
                    {
                        ShipmentWeightId = 1,
                        ShipmentWeightDescription = "Up to 1 kg",
                        ShipmentWeightPrice = 15.25,
                        Shipments = new List<Shipment>()
                    },
                    ShipmentContent = "Books",
                    Sending = new Address
                    {
                        Street = "456 Maple Street",
                        City = "Foreston",
                        PostalCode = "98765"
                    },
                    Receiving = new Address
                    {
                        Street = "789 Birch Avenue",
                        City = "Woodville",
                        PostalCode = "23456"
                    },
                    ContactPersonName = "Amy Johnson",
                    ContactPersonPhone = "555-555-1234",
                    CustomerId = 2,
                    Customer = new Customer
                    {
                        Id = 2,
                        FirstName = "Charlie",
                        LastName = "Brown",
                        Email = "charlie@example.com"
                    },
                    Price = 20.00,
                    AdditionalServices = new List<AdditionalServiceShipment>
                    {
                        new AdditionalServiceShipment
                        {
                            AdditionalServiceId = 3,
                            ShipmentId = 3
                        }
                    },
                    Note = ""
                },
                new Shipment
                {
                    ShipmentId = 4,
                    ShipmentCode = "SH777888",
                    ShipmentWeightId = 4,
                    ShipmentWeight = new ShipmentWeight
                    {
                        ShipmentWeightId = 4,
                        ShipmentWeightDescription = "10 - 20 kg",
                        ShipmentWeightPrice = 45.00,
                        Shipments = new List<Shipment>()
                    },
                    ShipmentContent = "Furniture",
                    Sending = new Address
                    {
                        Street = "987 Oak Street",
                        City = "Groveville",
                        PostalCode = "56789"
                    },
                    Receiving = new Address
                    {
                        Street = "654 Elm Road",
                        City = "Branchville",
                        PostalCode = "43210"
                    },
                    ContactPersonName = "David Wilson",
                    ContactPersonPhone = "555-777-8888",
                    CustomerId = 5,
                    Customer = new Customer
                    {
                        Id = 5,
                        FirstName = "Ella",
                        LastName = "Davis",
                        Email = "ella@example.com"
                    },
                    Price = 60.50,
                    AdditionalServices = new List<AdditionalServiceShipment>(),
                    Note = "Heavy items"
                },
                new Shipment
                {
                    ShipmentId = 5,
                    ShipmentCode = "SH222333",
                    ShipmentWeightId = 5,
                    ShipmentWeight = new ShipmentWeight
                    {
                        ShipmentWeightId = 5,
                        ShipmentWeightDescription = "20 - 30 kg",
                        ShipmentWeightPrice = 70.25,
                        Shipments = new List<Shipment>()
                    },
                    ShipmentContent = "Sports Equipment",
                    Sending = new Address
                    {
                        Street = "123 Pine Lane",
                        City = "Greenville",
                        PostalCode = "34567"
                    },
                    Receiving = new Address
                    {
                        Street = "456 Oak Avenue",
                        City = "Parkville",
                        PostalCode = "67890"
                    },
                    ContactPersonName = "Frank Smith",
                    ContactPersonPhone = "555-222-3333",
                    CustomerId = 1,
                    Customer = new Customer
                    {
                        Id = 1,
                        FirstName = "Grace",
                        LastName = "Wilson",
                        Email = "grace@example.com"
                    },
                    Price = 85.75,
                    AdditionalServices = new List<AdditionalServiceShipment>
                    {
                        new AdditionalServiceShipment
                        {
                            AdditionalServiceId = 4,
                            ShipmentId = 5
                        }
                    },
                    Note = "Fragile items"
                }
            };
        }
    }
}
