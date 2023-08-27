using DeliveryServiceApp.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceAppTests.DataAnnotations
{
    public class ValidEmailAddressTests
    {
        [Theory]
        [InlineData("valid@example.com")]
        [InlineData("user.name@example.co.uk")]
        [InlineData("first.last@example.com")]
        public void IsValid_ValidEmailAddresses_ReturnsTrue(string email)
        {
            var attribute = new ValidEmailAddress();

            bool isValid = attribute.IsValid(email);

            Assert.True(isValid);
        }

        [Theory]
        [InlineData("invalid.email")]
        [InlineData("user@example")]
        [InlineData("user@example.")]
        [InlineData("user.example.com")]
        [InlineData("")]
        [InlineData(null)]
        public void IsValid_InvalidEmailAddresses_ReturnsFalse(string email)
        {
            var attribute = new ValidEmailAddress();

            bool isValid = attribute.IsValid(email);

            Assert.False(isValid);
        }
    }
}
