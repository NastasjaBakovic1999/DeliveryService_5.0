using Autofac.Extras.Moq;
using Dapper;
using DeliveryServiceData.Implementation;
using DeliveryServiceDomain;
using Moq;
using System.Data;
using System.Data.Common;
using Xunit;

namespace DeliveryServiceDataTests
{
    public class RepositoryAdditionalServiceTests
    {
        [Fact]
        public void GetAll_ReturnsListOfAdditionalServices()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Query<AdditionalService>(It.IsAny<string>(), CommandType.StoredProcedure))
                .Returns(GetSampleAdditionalServices());

            var repository = new RepositoryAdditionalService(databaseMock.Object);

            var result = repository.GetAll();

            Assert.NotNull(result);
            Assert.Equal(GetSampleAdditionalServices().Count, result.Count);
            foreach (var additionalService in result)
            {
                Assert.NotEqual(0, additionalService.AdditionalServiceId);
                Assert.NotNull(additionalService.AdditionalServiceName);
                Assert.NotEqual(0.0, additionalService.AdditionalServicePrice);
            }
        }

        [Fact]
        public void GetAll_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.Query<AdditionalService>(It.IsAny<string>(), CommandType.StoredProcedure))
                .Throws(new Exception("Database error occurred!"));

            var repository = new RepositoryAdditionalService(databaseMock.Object);

            Assert.Throws<Exception>(()=> repository.GetAll());
        }

        [Fact]
        public void FindById_ReturnValidAdditionalService()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<AdditionalService>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Returns((string procedure, object parameters, CommandType commandType) =>
                {
                    var inputParams = (DynamicParameters)parameters;
                    var id = inputParams.Get<int>("@AdditionalServiceId");
                    var additionalService = GetSampleAdditionalServices().FirstOrDefault(s => s.AdditionalServiceId == id);
                    return additionalService;
                });

            var repository = new RepositoryAdditionalService(databaseMock.Object);

            var result = repository.FindByID(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.AdditionalServiceId);
            Assert.NotNull(result.AdditionalServiceName);
            Assert.NotEqual(0.0, result.AdditionalServicePrice);
        }

        [Fact]
        public void FindById_ReturnsNullForInvalidId()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<AdditionalService>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                .Returns((string procedure, object parameters, CommandType commandType) =>
                {
                    var inputParams = (DynamicParameters)parameters;
                    var id = inputParams.Get<int>("@AdditionalServiceId");
                    var additionalService = GetSampleAdditionalServices().FirstOrDefault(s => s.AdditionalServiceId == id);
                    return additionalService;
                });

            var repository = new RepositoryAdditionalService(databaseMock.Object);  

            var result = repository.FindByID(999);

            Assert.Null(result);
        }

        [Fact]
        public void FindById_ThrowsExceptionOnDatabaseError()
        {
            var databaseMock = new Mock<IDatabaseOperations>();
            databaseMock.Setup(db => db.QuerySingleOrDefault<AdditionalService>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                    .Throws(new Exception("Database error occurred"));

            var repository = new RepositoryAdditionalService(databaseMock.Object);

            Assert.Throws<Exception>(() => repository.FindByID(1));
        }


        private List<AdditionalService> GetSampleAdditionalServices()
        {
            List<AdditionalService> output = new List<AdditionalService> 
            {
                new AdditionalService
                {
                    AdditionalServiceId = 1,
                    AdditionalServiceName = "Express Delivery",
                    AdditionalServicePrice = 5.99
                },
                new AdditionalService
                {
                    AdditionalServiceId = 2,
                    AdditionalServiceName = "Signature Confirmation",
                    AdditionalServicePrice = 2.49
                },
                new AdditionalService
                {
                    AdditionalServiceId = 3,
                    AdditionalServiceName = "Insurance Coverage",
                    AdditionalServicePrice = 8.75
                },
                new AdditionalService
                {
                    AdditionalServiceId = 4,
                    AdditionalServiceName = "Saturday Delivery",
                    AdditionalServicePrice = 12.99
                },
                new AdditionalService
                {
                    AdditionalServiceId = 5,
                    AdditionalServiceName = "Special Handling",
                    AdditionalServicePrice = 6.25
                },
                new AdditionalService
                {
                    AdditionalServiceId = 6,
                    AdditionalServiceName = "Delivery Confirmation",
                    AdditionalServicePrice = 1.99
                },
                new AdditionalService
                {
                    AdditionalServiceId = 7,
                    AdditionalServiceName = "Custom Packaging",
                    AdditionalServicePrice = 4.50
                }
            };
            return output;
        }
    }
}