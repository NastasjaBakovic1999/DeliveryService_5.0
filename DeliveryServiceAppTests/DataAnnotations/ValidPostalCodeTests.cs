using DeliveryServiceApp.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceAppTests.DataAnnotations
{
    public class ValidPostalCodeTests
    {
        [Theory]
        [InlineData("12345")]
        [InlineData("98765")]
        public void IsValid_ValidPostalCodes_ReturnsTrue(string postalCode)
        {
            var attribute = new ValidPostalCode();

            bool isValid = attribute.IsValid(postalCode);

            Assert.True(isValid);
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("123456")]
        [InlineData("ABC12")]
        [InlineData("")]
        [InlineData(null)]
        public void IsValid_InvalidPostalCodes_ReturnsFalse(string postalCode)
        {
            var attribute = new ValidPostalCode();

            bool isValid = attribute.IsValid(postalCode);

            Assert.False(isValid);
        }
    }
}
