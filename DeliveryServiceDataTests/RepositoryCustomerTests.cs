using Castle.Core.Resource;
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
    public class RepositoryCustomerTests
    {
        [Fact]
        public void GetAll_ReturnsListOfCustomers()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Query<Customer>(It.IsAny<string>(), CommandType.StoredProcedure))
                .Returns(GetSampleCustomers());

            var repository = new RepositoryCustomer(databaseMock.Object);

            var result = repository.GetAll();

            Assert.NotNull(result);
            Assert.Equal(GetSampleCustomers().Count, result.Count);
            foreach (var customer in result)
            {
                Assert.NotEqual(0, customer.Id);
                Assert.NotNull(customer.FirstName);
                Assert.NotNull(customer.LastName);
                Assert.NotNull(customer.UserName);
                Assert.NotNull(customer.Address);
                Assert.NotNull(customer.PostalCode);
                Assert.NotNull(customer.Email);
                Assert.NotNull(customer.PhoneNumber);
            }
        }

        [Fact]
        public void GetAll_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Query<Customer>(It.IsAny<string>(), CommandType.StoredProcedure))
                .Throws(new Exception("Database error occurred!"));

            var repository = new RepositoryCustomer(databaseMock.Object);

            Assert.Throws<Exception>(() => repository.GetAll());
        }

        [Fact]
        public void FindById_ReturnValidCustomer()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<Customer>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Returns((string procedure, object parameters, CommandType commandType) =>
                {
                    var inputParams = (DynamicParameters)parameters;
                    var id = inputParams.Get<int>("@CustomerId");
                    var customer = GetSampleCustomers().FirstOrDefault(s => s.Id == id);
                    return customer;
                });

            var repository = new RepositoryCustomer(databaseMock.Object);

            var result = repository.FindByID(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.NotEqual(0, result.Id);
            Assert.NotNull(result.FirstName);
            Assert.NotNull(result.LastName);
            Assert.NotNull(result.UserName);
            Assert.NotNull(result.Address);
            Assert.NotNull(result.PostalCode);
            Assert.NotNull(result.Email);
            Assert.NotNull(result.PhoneNumber);
        }

        [Fact]
        public void FindById_ReturnsNullForInvalidId()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<Customer>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Returns((string procedure, object parameters, CommandType commandType) =>
                {
                    var inputParams = (DynamicParameters)parameters;
                    var id = inputParams.Get<int>("@CustomerId");
                    var customer = GetSampleCustomers().FirstOrDefault(s => s.Id == id);
                    return customer;
                });

            var repository = new RepositoryCustomer(databaseMock.Object);

            var result = repository.FindByID(999);

            Assert.Null(result);
        }

        [Fact]
        public void FindById_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<Customer>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                    .Throws(new Exception("Database error occurred"));

            var repository = new RepositoryCustomer(databaseMock.Object);

            Assert.Throws<Exception>(() => repository.FindByID(1));
        }

        [Fact]
        public void Edit_UpdatesCustomerData()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            var repository = new RepositoryCustomer(databaseMock.Object);

            Customer customerToUpdate = new Customer
            {
                Id = 1,
                Address = "New Address",
                PostalCode = "12345"
            };

            repository.Edit(customerToUpdate);
            databaseMock.Verify(db => db.Execute(
                       "[dbo].[EditCustomer]",
                       It.Is<DynamicParameters>(param =>
                           param.Get<string>("@Address") == customerToUpdate.Address &&
                           param.Get<string>("@PostalCode") == customerToUpdate.PostalCode &&
                           param.Get<int>("@CustomerId") == customerToUpdate.Id),
                       CommandType.StoredProcedure),
                       Times.Once
                   );
        }

        [Fact]
        public void Edit_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Throws(new Exception("Database error occurred"));

            var repository = new RepositoryCustomer(databaseMock.Object);

            Customer customerToUpdate = new Customer
            {
                Id = 1,
                Address = "New Address",
                PostalCode = "12345"
            };

            Assert.Throws<Exception>(() => repository.Edit(customerToUpdate));
        }


        private List<Customer> GetSampleCustomers()
        {
            List<Customer> output = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Address = "Address 1",
                    Email = "john@gmail.com",
                    UserName = "john123",
                    PhoneNumber = "0123456",
                    PostalCode = "12345"
                },
                new Customer
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Address = "Address 2",
                    Email = "jane@gmail.com",
                    UserName = "jane456",
                    PhoneNumber = "9876543",
                    PostalCode = "67890"
                },
                new Customer
                {
                    Id = 3,
                    FirstName = "Michael",
                    LastName = "Johnson",
                    Address = "Address 3",
                    Email = "michael@gmail.com",
                    UserName = "mikej",
                    PhoneNumber = "5551234",
                    PostalCode = "54321"
                },
                new Customer
                {
                    Id = 4,
                    FirstName = "Emily",
                    LastName = "Brown",
                    Address = "Address 4",
                    Email = "emily@gmail.com",
                    UserName = "emilyb",
                    PhoneNumber = "9871230",
                    PostalCode = "54321"
                },
                new Customer
                {
                    Id = 5,
                    FirstName = "David",
                    LastName = "Wilson",
                    Address = "Address 5",
                    Email = "david@gmail.com",
                    UserName = "davidw",
                    PhoneNumber = "7894562",
                    PostalCode = "45678"
                },
                new Customer
                {
                    Id = 6,
                    FirstName = "Olivia",
                    LastName = "Martinez",
                    Address = "Address 6",
                    Email = "olivia@gmail.com",
                    UserName = "oliviam",
                    PhoneNumber = "3216540",
                    PostalCode = "13579"
                }
        };
        return output;
        }
    }
}
