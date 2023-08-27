using DeliveryServiceApp.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceAppTests.DataAnnotations
{
    public class ValidShipmentCodeTests
    {
        [Theory]
        [InlineData("Ab123456789")]
        [InlineData("123456789Aa")]
        [InlineData("abc1234567D")]
        [InlineData("Abcde4ghijk")]
        [InlineData("ABCDEFGHIj9")]
        [InlineData("D987654321e")]
        public void IsValid_ValidShipmentCodes_ReturnsTrue(string shipmentCode)
        {
            var attribute = new ValidShipmentCode();

            bool isValid = attribute.IsValid(shipmentCode);

            Assert.True(isValid);
        }

        [Theory]
        [InlineData("ABC123456789")]
        [InlineData("ABC-12345678")]
        [InlineData("ABC 12345678")]
        [InlineData("abc12345678")]
        [InlineData("abcdefghijk")]
        [InlineData("ABCDEFGHIJK")]
        [InlineData("12345678901")]
        [InlineData("")]
        [InlineData(null)]
        public void IsValid_InvalidShipmentCodes_ReturnsFalse(string shipmentCode)
        {
            var attribute = new ValidShipmentCode();

            bool isValid = attribute.IsValid(shipmentCode);

            Assert.False(isValid);
        }
    }
}
