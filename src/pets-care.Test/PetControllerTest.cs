using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using pets_care.Auth;
using pets_care.Controllers;
using pets_care.Models;
using pets_care.Repository;
using pets_care.Requests;
using pets_care.Services;

namespace pets_care.Test;

    public class PetControllerTest
    {
        private Mock<IPetRepository> _repositoryMock;
        private Mock<INominatimService> _nominatimMock;
        private Fixture _fixture;
        private PetController _controller;
        private Mock<ITokenGenerator> _tokenGenerator;

        public PetControllerTest()
        {
            _fixture = new Fixture();
            _repositoryMock = new Mock<IPetRepository>();
            _nominatimMock = new Mock<INominatimService>();
            _tokenGenerator = new Mock<ITokenGenerator>();
        }

        [Fact]
        public async Task Get_Pets_ReturnsOk()
        {
            _fixture.Customize<Pet>(o => o.Without(foo => foo.Client));
            var pets = _fixture.CreateMany<Pet>();
            _repositoryMock.Setup(repo => repo.GetPets()).ReturnsAsync(pets);

            _controller = new PetController(_repositoryMock.Object, _tokenGenerator.Object, _nominatimMock.Object);

            var response = await _controller.GetPets();
            var result = response.Result as OkObjectResult;

            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            response.Should().BeOfType<ActionResult<IEnumerable<Pet>>>();
        }

        [Fact]
        public async Task Get_Pets_ThrowException()
        {
            _repositoryMock.Setup(repo => repo.GetPets()).Throws(new Exception());

            _controller = new PetController(_repositoryMock.Object, _tokenGenerator.Object, _nominatimMock.Object);

            var response = await _controller.GetPets();
            var result = response.Result as ObjectResult;

            result?.StatusCode.Should().Be(400);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Get_PetById_ReturnsOk()
        {
            _fixture.Customize<Pet>(o => o.Without(foo => foo.Client));
            var pet = _fixture.Create<Pet>();
            _repositoryMock.Setup(repo => repo.GetPetById(pet.PetId)).ReturnsAsync(pet);

            _controller = new PetController(_repositoryMock.Object, _tokenGenerator.Object, _nominatimMock.Object);

            var response = await _controller.GetPetById(pet.PetId);
            var result = response.Result as OkObjectResult;

            result?.StatusCode.Should().Be(200);
            response.Should().BeOfType<ActionResult<Pet>>();
        }

        [Fact]
        public async Task Get_PetById_ThrowsException()
        {
            var guid = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetPetById(guid)).Throws(new Exception());

            _controller = new PetController(_repositoryMock.Object, _tokenGenerator.Object, _nominatimMock.Object);

            var response = await _controller.GetPetById(guid);
            var result = response.Result as ObjectResult;

            result?.StatusCode.Should().Be(400);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Post_CreatePet_ThrowsExceptionBadRequest()
        {

        }

        [Fact]
        public async Task Delete_DeletePet_ReturnsOk()
        {
            _fixture.Customize<Pet>(o => o.Without(foo => foo.Client));
            var pet = _fixture.Create<Pet>();
            var petCreateRequest = _fixture.Create<PetCreateRequest>();

            _repositoryMock.Setup(repo => repo.GetPetById(pet.PetId)).ReturnsAsync(pet);
            _repositoryMock.Setup(repo => repo.DeletePet(pet));

            _controller = new PetController(_repositoryMock.Object, _tokenGenerator.Object, _nominatimMock.Object);

            var response = await _controller.DeletePet(pet.PetId);
            var result = response.Result as ObjectResult;

            response.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Delete_DeletePet_ThrowsException()
        {
            _fixture.Customize<Pet>(o => o.Without(foo => foo.Client));
            var pet = _fixture.Create<Pet>();
            _repositoryMock.Setup(repo => repo.DeletePet(pet));

            _controller = new PetController(_repositoryMock.Object, _tokenGenerator.Object, _nominatimMock.Object);

            var response = await _controller.DeletePet(pet.ClientId);
            var result = response.Result as ObjectResult;

            result?.StatusCode.Should().Be(404);
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
