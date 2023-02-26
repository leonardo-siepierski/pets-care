using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Moq;
using pets_care.Services;

namespace pets_care.Test
{
    public class ViaCepServiceTest
    {
        [Theory]
        [InlineData("04321020")]
        [InlineData("14730000")]
        public async Task ShouldReturnUniversityByCountryAndName(string cep)
        {
            //  Arrange
            var mockClient = new Mock<HttpClient>();
            var viaCepService = new ViaCepService(mockClient.Object);

            // Action
            var result = await viaCepService.FindAdress(cep);

            // Assert
            result.Should().BeOfType<JsonElement>();
        }
    }
}