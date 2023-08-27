using Dapper;
using DeliveryServiceData.DTOs;
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

        [Fact]
        public void Add_SavesNewShipment()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            var repository = new RepositoryShipment(databaseMock.Object);

            Shipment shipmentToAdd = new Shipment
            {
                ShipmentCode = "SH123456",
                ShipmentWeightId = 2,
                ShipmentContent = "Electronics",
                ContactPersonName = "John Doe",
                ContactPersonPhone = "555-123-4567",
                CustomerId = 3,
                Price = 25.50,
                Note = "Fragile items",
                Sending = new Address
                {
                    City = "Cityville",
                    Street = "123 Main Street",
                    PostalCode = "12345"
                },
                Receiving = new Address
                {
                    City = "Townsville",
                    Street = "456 Elm Avenue",
                    PostalCode = "67890"
                },
                AdditionalServices = new List<AdditionalServiceShipment>
                {
                    new AdditionalServiceShipment
                    {
                        AdditionalServiceId = 1
                    }
                }
            };

            repository.Add(shipmentToAdd);

            databaseMock.Verify(db => db.Execute(
                "[dbo].[InsertShipment]",
                It.Is<DynamicParameters>(param =>
                    param.Get<string>("@ShipmentCode") == shipmentToAdd.ShipmentCode &&
                    param.Get<int>("@ShipmentWeightId") == shipmentToAdd.ShipmentWeightId &&
                    param.Get<string>("@ShipmentContent") == shipmentToAdd.ShipmentContent &&
                    param.Get<string>("@ContactPersonName") == shipmentToAdd.ContactPersonName &&
                    param.Get<string>("@ContactPersonPhone") == shipmentToAdd.ContactPersonPhone &&
                    param.Get<int>("@CustomerId") == shipmentToAdd.CustomerId &&
                    param.Get<double>("@Price") == shipmentToAdd.Price &&
                    param.Get<string>("@Note") == shipmentToAdd.Note &&
                    param.Get<string>("@Sending_City") == shipmentToAdd.Sending.City &&
                    param.Get<string>("@Sending_Street") == shipmentToAdd.Sending.Street &&
                    param.Get<string>("@Sending_PostalCode") == shipmentToAdd.Sending.PostalCode &&
                    param.Get<string>("@Receiving_City") == shipmentToAdd.Receiving.City &&
                    param.Get<string>("@Receiving_Street") == shipmentToAdd.Receiving.Street &&
                    param.Get<string>("@Receiving_PostalCode") == shipmentToAdd.Receiving.PostalCode &&
                    param.Get<int>("@ShipmentId") == shipmentToAdd.ShipmentId &&
                    param.Get<string>("@AdditionalServiceIds") != null &&
                    param.Get<string>("@AdditionalServiceIds") != ""),
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

            var repository = new RepositoryShipment(databaseMock.Object);

            Shipment shipmentToAdd = new Shipment
            {
                ShipmentId = 10,
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
            };

            Assert.Throws<Exception>(() => repository.Add(shipmentToAdd));
        }

        [Fact]
        public void FindByCode_ReturnsShipment()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
         
            databaseMock.Setup(db => db.QuerySingleOrDefault<Shipment>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
               .Returns((string procedure, object parameters, CommandType commandType) =>
               {
                   var inputParams = (DynamicParameters)parameters;
                   var shipmentCode = inputParams.Get<string>("@ShipmentCode");
                   var shipment = GetSampleShipments().FirstOrDefault(s => s.ShipmentCode.Equals(shipmentCode));
                   return shipment;
               });

            var repository = new RepositoryShipment(databaseMock.Object);

            string shipmentCodeToFind = "SH123456";

            var result = repository.FindByCode(shipmentCodeToFind);

            Assert.NotNull(result);
            Assert.Equal(1, result.ShipmentId);
            Assert.Equal(shipmentCodeToFind, result.ShipmentCode);
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
        public void FindByCode_ReturnsNullIfNotFound()
        {
            var databaseMock = new Mock<IDatabaseOperations>();

            databaseMock.Setup(db => db.QuerySingleOrDefault<Shipment>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
               .Returns((string procedure, object parameters, CommandType commandType) =>
               {
                   var inputParams = (DynamicParameters)parameters;
                   var shipmentCode = inputParams.Get<string>("@ShipmentCode");
                   var shipment = GetSampleShipments().FirstOrDefault(s => s.ShipmentCode.Equals(shipmentCode));
                   return shipment;
               });

            var repository = new RepositoryShipment(databaseMock.Object);

            string nonExistentShipmentCode = "SH999999";

            var result = repository.FindByCode(nonExistentShipmentCode);

            Assert.Null(result);
        }

        [Fact]
        public void FindByCode_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<Shipment>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Throws(new Exception("Database error occurred"));

            var repository = new RepositoryShipment(databaseMock.Object);

            string shipmentCodeToFind = "SH123456";

            Assert.Throws<Exception>(() => repository.FindByCode(shipmentCodeToFind));
        }

        [Fact]
        public void GetAllOfSpecifiedUser_ReturnsListOfShipments()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            var repository = new RepositoryShipment(databaseMock.Object);

            int customerId = 3; 

            databaseMock.Setup(db => db.Query<ShipmentDto>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Returns((string procedure, object parameters, CommandType commandType) =>
                {
                    var inputParams = (DynamicParameters)parameters;
                    var customerId = inputParams.Get<int>("@CustomerId");
                    var shipmentsDto = GetSampleShipmentsDto().FindAll(s => s.CustomerId == customerId);
                    return shipmentsDto;
                });

            var expected = GetSampleShipments().FindAll(s => s.CustomerId == customerId);

            var result = repository.GetAllOfSpecifiedUser(customerId);

            Assert.NotNull(result);
            Assert.Equal(expected.Count, result.Count);
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
        public void GetAllOfSpecifiedUser_ReturnsEmptyListIfNoShipments()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            var repository = new RepositoryShipment(databaseMock.Object);

            int customerId = 10;

            databaseMock.Setup(db => db.Query<Shipment>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Returns((string procedure, object parameters, CommandType commandType) =>
                {
                    var shipmentsDto = GetSampleShipmentsDto().FindAll(s => s.CustomerId == customerId);
                    var shipments = new List<Shipment>();
                    if (shipmentsDto != null)
                    {
                        foreach (var shipment in shipmentsDto)
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
                });

            List<Shipment> result = repository.GetAllOfSpecifiedUser(customerId);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetAllOfSpecifiedUser_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Query<ShipmentDto>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Throws(new Exception("Database error occurred"));

            var repository = new RepositoryShipment(databaseMock.Object);

            int userId = 3; 

            Assert.Throws<Exception>(() => repository.GetAllOfSpecifiedUser(userId));
        }

        [Fact]
        public void RemoveShipment_RemovesShipment()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            var repository = new RepositoryShipment(databaseMock.Object);

            int shipmentIdToRemove = 1; 

            repository.RemoveShipment(shipmentIdToRemove);

            databaseMock.Verify(db => db.Execute(
                "[dbo].[DeleteShipment]",
                It.Is<DynamicParameters>(param => param.Get<int>("@Id") == shipmentIdToRemove),
                CommandType.StoredProcedure),
                Times.Once
            );
        }

        [Fact]
        public void RemoveShipment_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Throws(new Exception("Database error occurred"));

            var repository = new RepositoryShipment(databaseMock.Object);

            int shipmentIdToRemove = 1;

            Assert.Throws<Exception>(() => repository.RemoveShipment(shipmentIdToRemove));
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
                    ShipmentContent = "Electronics",
                    ContactPersonName = "John Doe",
                    ContactPersonPhone = "555-123-4567",
                    CustomerId = 3,
                    Price = 25.50,
                    Note = "Fragile items"
                },
                new Shipment
                {
                    ShipmentId = 2,
                    ShipmentCode = "SH987654",
                    ShipmentWeightId = 3,
                    ShipmentContent = "Clothing",
                    ContactPersonName = "Jane Smith",
                    ContactPersonPhone = "555-987-6543",
                    CustomerId = 4,
                    Price = 35.75,
                    Note = "Handle with care"
                },
                new Shipment
                {
                    ShipmentId = 3,
                    ShipmentCode = "SH555666",
                    ShipmentWeightId = 1,
                    ShipmentContent = "Books",
                    ContactPersonName = "Amy Johnson",
                    ContactPersonPhone = "555-555-1234",
                    CustomerId = 2,
                    Price = 20.00,
                    Note = ""
                },
                new Shipment
                {
                    ShipmentId = 4,
                    ShipmentCode = "SH777888",
                    ShipmentWeightId = 4,
                    ShipmentContent = "Furniture",
                    ContactPersonName = "David Wilson",
                    ContactPersonPhone = "555-777-8888",
                    CustomerId = 5,
                    Price = 60.50,
                    Note = "Heavy items"
                },
                new Shipment
                {
                    ShipmentId = 5,
                    ShipmentCode = "SH222333",
                    ShipmentWeightId = 5,
                    ShipmentContent = "Sports Equipment",
                    ContactPersonName = "Frank Smith",
                    ContactPersonPhone = "555-222-3333",
                    CustomerId = 3,
                    Price = 85.75,
                    Note = "Fragile items"
                }
            };
        }

        private List<ShipmentDto> GetSampleShipmentsDto()
        {
            return new List<ShipmentDto>
            {
                new ShipmentDto
                {
                    ShipmentId = 1,
                    ShipmentCode = "SH123456",
                    ShipmentWeightId = 2,
                    ShipmentContent = "Electronics",
                    ContactPersonName = "John Doe",
                    ContactPersonPhone = "555-123-4567",
                    CustomerId = 3,
                    Price = 25.50,
                    Note = "Fragile items",
                    Sending_Street = "Street 1",
                    Sending_City = "City 1",
                    Sending_PostalCode = "12345",
                    Receiving_Street = "Street 2",
                    Receiving_City = "City 2",
                    Receiving_PostalCode = "67890"
                },
                new ShipmentDto
                {
                    ShipmentId = 2,
                    ShipmentCode = "SH987654",
                    ShipmentWeightId = 3,
                    ShipmentContent = "Clothing",
                    ContactPersonName = "Jane Smith",
                    ContactPersonPhone = "555-987-6543",
                    CustomerId = 4,
                    Price = 35.75,
                    Note = "Handle with care",
                    Sending_Street = "Street 3",
                    Sending_City = "City 3",
                    Sending_PostalCode = "54321",
                    Receiving_Street = "Street 4",
                    Receiving_City = "City 4",
                    Receiving_PostalCode = "09876"
                },
                new ShipmentDto
                {
                    ShipmentId = 3,
                    ShipmentCode = "SH555666",
                    ShipmentWeightId = 1,
                    ShipmentContent = "Books",
                    ContactPersonName = "Amy Johnson",
                    ContactPersonPhone = "555-555-1234",
                    CustomerId = 2,
                    Price = 20.00,
                    Note = "",
                    Sending_Street = "Street 5",
                    Sending_City = "City 5",
                    Sending_PostalCode = "54321",
                    Receiving_Street = "Street 6",
                    Receiving_City = "City 6",
                    Receiving_PostalCode = "09876"
                },
                new ShipmentDto
                {
                    ShipmentId = 4,
                    ShipmentCode = "SH777888",
                    ShipmentWeightId = 4,
                    ShipmentContent = "Furniture",
                    ContactPersonName = "David Wilson",
                    ContactPersonPhone = "555-777-8888",
                    CustomerId = 5,
                    Price = 60.50,
                    Note = "Heavy items",
                    Sending_Street = "Street 7",
                    Sending_City = "City 7",
                    Sending_PostalCode = "83874",
                    Receiving_Street = "Street 8",
                    Receiving_City = "City 8",
                    Receiving_PostalCode = "27403"
                },
                new ShipmentDto
                {
                    ShipmentId = 5,
                    ShipmentCode = "SH222333",
                    ShipmentWeightId = 5,
                    ShipmentContent = "Sports Equipment",
                    ContactPersonName = "Frank Smith",
                    ContactPersonPhone = "555-222-3333",
                    CustomerId = 3,
                    Price = 85.75,
                    Note = "Fragile items",
                    Sending_Street = "Street 9",
                    Sending_City = "City 9",
                    Sending_PostalCode = "22918",
                    Receiving_Street = "Street 10",
                    Receiving_City = "City 10",
                    Receiving_PostalCode = "31330"
                }
            };
        }
    }
}
