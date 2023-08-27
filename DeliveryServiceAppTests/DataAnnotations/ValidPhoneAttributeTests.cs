using DeliveryServiceApp.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceAppTests.DataAnnotations
{
    public class ValidPhoneAttributeTests
    {
        [Theory]
        [InlineData("0612345678")]
        [InlineData("06234567890")]
        [InlineData("06987654321")]
        public void IsValid_ValidPhoneNumbers_ReturnsTrue(string phoneNumber)
        {
            var attribute = new ValidPhoneAttribute();

            bool isValid = attribute.IsValid(phoneNumber);

            Assert.True(isValid);
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("0123456789")]
        [InlineData("07123456789")]
        [InlineData("")]
        [InlineData(null)]
        public void IsValid_InvalidPhoneNumbers_ReturnsFalse(string phoneNumber)
        {
            var attribute = new ValidPhoneAttribute();

            bool isValid = attribute.IsValid(phoneNumber);

            Assert.False(isValid);
        }
    }
}
