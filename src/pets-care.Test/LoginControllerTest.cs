using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoBogus;
using AutoFixture;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Moq;
using pets_care.Auth;
using pets_care.Controllers;
using pets_care.Models;
using pets_care.Repository;
using pets_care.Requests;
using pets_care.Services;

namespace pets_care.Test;

    public class LoginControllerTest
    {
        private Mock<IClientRepository> _repositoryMock;
        private Mock<ITokenGenerator> _tokenGenerator;
        private Fixture _fixture;
        private LoginController _controller;

        public LoginControllerTest()
        {
            _fixture = new Fixture();
            _repositoryMock = new Mock<IClientRepository>();
            _tokenGenerator = new Mock<ITokenGenerator>();
        }

        [Fact]
        public async Task Post_Login_ReturnsOk()
        {
            _fixture.Customize<Client>(o => o.Without(foo => foo.Pets));
            var cloginRequest = _fixture.Create<LoginRequest>();
            var client = _fixture.Create<Client>();
            var token = _fixture.Create<string>();
            _repositoryMock.Setup(repo => repo.AuthClientAsync(cloginRequest)).ReturnsAsync(client);
            _tokenGenerator.Setup(repo => repo.Generate(client)).Returns(token);

            _controller = new LoginController(_repositoryMock.Object, _tokenGenerator.Object);

            var response = await _controller.ClientLogin(cloginRequest);
            var result = response.Result as ObjectResult;

            var objExpected = new {user = client, token};
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(objExpected);
        }

        [Fact]
        public async Task Post_Login_ThrowsExceptionNotFound()
        {
            _fixture.Customize<Client>(o => o.Without(foo => foo.Pets));
            var cloginRequest = _fixture.Create<LoginRequest>();
            var client = _fixture.Create<Client>();
            _repositoryMock.Setup(repo => repo.AuthClientAsync(null)).ReturnsAsync(client);

            _controller = new LoginController(_repositoryMock.Object, _tokenGenerator.Object);

            var response = await _controller.ClientLogin(cloginRequest);
            var result = response.Result as ObjectResult;

            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(404);
            result?.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Post_Login_ThrowsExceptionBadRequest()
        {
            _fixture.Customize<Client>(o => o.Without(foo => foo.Pets));
            var cloginRequest = _fixture.Create<LoginRequest>();
            var client = _fixture.Create<Client>();
            _repositoryMock.Setup(repo => repo.AuthClientAsync(null)).Throws(new Exception());

            _controller = new LoginController(_repositoryMock.Object, _tokenGenerator.Object);

            var response = await _controller.ClientLogin(null);
            var result = response.Result as ObjectResult;

            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(400);
            result?.Should().BeOfType<BadRequestObjectResult>();
        }
    }
