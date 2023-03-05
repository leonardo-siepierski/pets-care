using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoBogus;
using AutoFixture;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Moq;
using pets_care.Controllers;
using pets_care.Models;
using pets_care.Repository;
using pets_care.Requests;
using pets_care.Services;

namespace pets_care.Test;

    public class ClientControllerTest
    {
        private Mock<IClientRepository> _repositoryMock;
        private Mock<IViaCepService> _viaCepMock;
        private Fixture _fixture;
        private ClientController _controller;

        public ClientControllerTest()
        {
            _fixture = new Fixture();
            _repositoryMock = new Mock<IClientRepository>();
            _viaCepMock = new Mock<IViaCepService>();
        }

        [Fact]
        public async Task Get_Clients_ReturnsOk()
        {
            _fixture.Customize<Client>(o => o.Without(foo => foo.Pets));
            var clients = _fixture.CreateMany<Client>(3).ToList();
            _repositoryMock.Setup(repo => repo.GetClients()).ReturnsAsync(clients);

            _controller = new ClientController(_repositoryMock.Object, _viaCepMock.Object);

            var response = await _controller.GetClients();
            var result = response.Result as OkObjectResult;

            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            response.Should().BeOfType<ActionResult<IEnumerable<Client>>>();
        }

        [Fact]
        public async Task Get_Clients_ThrowException()
        {
            _repositoryMock.Setup(repo => repo.GetClients()).Throws(new Exception());

            _controller = new ClientController(_repositoryMock.Object, _viaCepMock.Object);

            var response = await _controller.GetClients();
            var result = response.Result as ObjectResult;

            result?.StatusCode.Should().Be(400);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Get_ClientById_ReturnsOk()
        {
            _fixture.Customize<Client>(o => o.Without(foo => foo.Pets));
            var client = _fixture.Create<Client>();
            _repositoryMock.Setup(repo => repo.GetClientById(client.ClientId)).ReturnsAsync(client);

            _controller = new ClientController(_repositoryMock.Object, _viaCepMock.Object);

            var response = await _controller.GetClientById(client.ClientId);
            var result = response.Result as OkObjectResult;

            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            response.Should().BeOfType<ActionResult<Client>>();
        }

        [Fact]
        public async Task Get_ClientById_ThrowsException()
        {
            var guid = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetClientById(guid)).Throws(new Exception());

            _controller = new ClientController(_repositoryMock.Object, _viaCepMock.Object);

            var response = await _controller.GetClientById(guid);
            var result = response.Result as ObjectResult;

            result?.StatusCode.Should().Be(400);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Post_CreateClient_ReturnsOk()
        {
            _fixture.Customize<Client>(o => o.Without(foo => foo.Pets));
            var clientRequest = _fixture.Create<ClientCreateRequest>();
            var client = _fixture.Create<Client>();

            _viaCepMock.Setup(repo => repo.FindAdress(clientRequest.Cep)).ReturnsAsync(new {});
            _repositoryMock.Setup(repo => repo.CreateClient(clientRequest)).ReturnsAsync(client);


            _controller = new ClientController(_repositoryMock.Object, _viaCepMock.Object);

            var response = await _controller.CreateClient(clientRequest);
            var result = response.Result as ObjectResult;

            response.Should().NotBeNull();
            result?.StatusCode.Should().Be(201);
            result.Should().BeOfType<CreatedAtActionResult>();
            result.Value.Should().Be(client);
        }

        [Fact]
        public async Task Post_CreateClient_ThrowsExceptionCepNotFound()
        {
            var clientRequest = _fixture.Create<ClientCreateRequest>();
            _repositoryMock.Setup(repo => repo.CreateClient(clientRequest)).Throws(new Exception());

            _controller = new ClientController(_repositoryMock.Object, _viaCepMock.Object);

            var response = await _controller.CreateClient(clientRequest);
            var result = response.Result as ObjectResult;

            result?.StatusCode.Should().Be(400);
            result.Should().BeOfType<BadRequestObjectResult>();
            result.Value.Should().Be("Cep not found");
        }

        [Fact]
        public async Task Post_CreateClient_ThrowsException()
        {
            _fixture.Customize<Client>(o => o.Without(foo => foo.Pets));
            var clientRequest = _fixture.Create<ClientCreateRequest>();
            var client = _fixture.Create<Client>();

            _viaCepMock.Setup(repo => repo.FindAdress(clientRequest.Cep)).ReturnsAsync(new {});
            _repositoryMock.Setup(repo => repo.CreateClient(clientRequest)).ReturnsAsync(client);


            _controller = new ClientController(_repositoryMock.Object, _viaCepMock.Object);

            var response = await _controller.CreateClient(null);
            var result = response.Result as ObjectResult;

            result?.StatusCode.Should().Be(404);
            result.Should().BeNull();
        }

        [Fact]
        public async Task Post_CreateClient_ThrowsExceptionBadRequest()
        {
            var clientRequest = _fixture.Create<ClientCreateRequest>();

            _viaCepMock.Setup(repo => repo.FindAdress(clientRequest.Cep)).ReturnsAsync(new {});
            _repositoryMock.Setup(repo => repo.CreateClient(clientRequest)).Throws(new Exception());
            _controller = new ClientController(_repositoryMock.Object, _viaCepMock.Object);

            var response = await _controller.CreateClient(clientRequest);
            var result = response.Result as ObjectResult;

            result?.StatusCode.Should().Be(400);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Put_UpdateClient_ReturnsOk()
        {
            _fixture.Customize<Client>(o => o.Without(foo => foo.Pets));
            var client = _fixture.Create<Client>();
            var clientRequest = _fixture.Create<ClientUpdateRequest>();
            _repositoryMock.Setup(repo => repo.GetClientById(client.ClientId)).ReturnsAsync(client);
            _repositoryMock.Setup(repo => repo.UpdateClient(client, clientRequest)).Returns(true);

            _controller = new ClientController(_repositoryMock.Object, _viaCepMock.Object);

            var response = await _controller.UpdateClient(client.ClientId, clientRequest);
            var result = response.Result as ObjectResult;

            response.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().Be("Client Updated!");
        }

        [Fact]
        public async Task Put_UpdateClient_ThrowsExceptionNotFound()
        {
            _fixture.Customize<Client>(o => o.Without(foo => foo.Pets));
            var client = _fixture.Create<Client>();
            var clientRequest = _fixture.Create<ClientCreateRequest>();
            var clientUpdateRequest = _fixture.Create<ClientUpdateRequest>();
            _repositoryMock.Setup(repo => repo.UpdateClient(client, clientUpdateRequest)).Throws(new Exception());

            _controller = new ClientController(_repositoryMock.Object, _viaCepMock.Object);

            var response = await _controller.UpdateClient(client.ClientId, clientUpdateRequest);

            var result = response.Result as ObjectResult;

            result?.StatusCode.Should().Be(404);
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Delete_DeleteClient_ReturnsOk()
        {
            _fixture.Customize<Client>(o => o.Without(foo => foo.Pets));
            var client = _fixture.Create<Client>();
            var clientCreateRequest = _fixture.Create<ClientCreateRequest>();

            _repositoryMock.Setup(repo => repo.GetClientById(client.ClientId)).ReturnsAsync(client);
            _repositoryMock.Setup(repo => repo.DeleteClient(client));

            _controller = new ClientController(_repositoryMock.Object, _viaCepMock.Object);

            var response = await _controller.DeleteClient(client.ClientId);
            var result = response.Result as ObjectResult;

            response.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().Be("Client Deleted!");
        }

        [Fact]
        public async Task Delete_DeleteClient_TrhowsException()
        {
            _fixture.Customize<Client>(o => o.Without(foo => foo.Pets));
            var client = _fixture.Create<Client>();
            _repositoryMock.Setup(repo => repo.DeleteClient(client));

            _controller = new ClientController(_repositoryMock.Object, _viaCepMock.Object);

            var response = await _controller.DeleteClient(client.ClientId);
            var result = response.Result as ObjectResult;

            result?.StatusCode.Should().Be(404);
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
