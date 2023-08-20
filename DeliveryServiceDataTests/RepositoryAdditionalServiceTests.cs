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
        public void GetAll_ValidCall()
        {
            
                throw new NotImplementedException();
            

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