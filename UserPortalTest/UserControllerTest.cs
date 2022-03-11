using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserPortal.Controllers;
using UserPortal.Entities;
using UserPortal.Interfaces;
using UserPortal.Models;
using UserPortalTest;
using Xunit;

namespace UserPortalTest
{
    public class UserControllerTest
    {
        private readonly UserController _controller;
        private readonly IUserService _userService;
        private readonly IKafkaService _kafkaService;
        public UserControllerTest()
        {
            _userService = new UserServiceFake();
            _kafkaService = new KafkaServiceFake();
            _controller = new UserController(_userService, _kafkaService);
        }

        [Fact]
        public async void Authenticate_InvalidObjectPassed_ReturnsBadRequest()
        {
            var missingItem = new AuthenticateRequest { Username = "haktor", };
            _controller.ModelState.AddModelError("Password", "Required");
            // Act
            var badResponse = await _controller.Authenticate(missingItem);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public async void Authenticate_ValidObjectPassed_ReturnsOkResult()
        {
            var request = new AuthenticateRequest { Username = "haktor", Password = "1" };
            // Act
            var result = await _controller.Authenticate(request);
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Authenticate_WhenCalled_ReturnsRightItem()
        {
            var request = new AuthenticateRequest { Username = "haktor", Password = "1" };
            // Act
            var result = (await _controller.Authenticate(request)) as OkObjectResult;
            // Assert
            var item = result.Value as AuthenticateResponse;
            var expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2NDY5NTUzODYsImV4cCI6MTk1NjU3MDA1MiwiaWF0IjoxNjQ2OTU1Mzg2fQ.gl5eVAwr05x-hbz9oC2AbF_4e-srVnT2g0Gmo6f8IWk";
            Assert.IsType<AuthenticateResponse>(item);
            Assert.Equal(expectedToken, item.Token);
        }

        [Fact]
        public async void Registration_InvalidObjectPassed_ReturnsBadRequest()
        {
            var missingItem = new RegistrationRequest { Email = "new@gmail.com", FirstName = "new", LastName = "newLastName", };
            _controller.ModelState.AddModelError("Username", "Required");
            _controller.ModelState.AddModelError("Password", "Required");
            // Act
            var badResponse = await _controller.Registration(missingItem);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public async void Registration_ValidObjectPassed_ReturnsOkResult()
        {
            var request = new RegistrationRequest { Email = "new@gmail.com", FirstName = "new", LastName = "newLastName", Username = "newUser", Password = "2" };

            var result = await _controller.Registration(request);
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Registration_WhenCalled_ReturnsRightItem()
        {
            var request = new RegistrationRequest { Email = "new@gmail.com", FirstName = "new", LastName = "newLastName", Username = "newUser", Password = "2" };
            // Act
            var result = (await _controller.Registration(request)) as OkObjectResult;
            // Assert
            var item = result.Value as User;
            Assert.IsType<User>(item);
            Assert.Equal("newUser", item.Username);
        }

        [Fact]
        public async void Update_InvalidObjectPassed_ReturnsBadRequest()
        {
            var missingItem = new UserUpdateRequest { Username = "changedUser", Email = "changedUser@gmail.com", FirstName = "changedUser", LastName = "changedUserLastName", Password = "2" };
            _controller.ModelState.AddModelError("Id", "Required");
            // Act
            var badResponse = await _controller.UpdateCurrentUserData(missingItem);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public async void Update_ValidObjectPassed_ReturnsOkResult()
        {
            var request = new UserUpdateRequest { Id = 3, Username = "changedUser", Email = "changedUser@gmail.com", FirstName = "changedUser", LastName = "changedUserLastName", Password = "2" };

            var result = await _controller.UpdateCurrentUserData(request);
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Update_WhenCalled_ReturnsRightItem()
        {
            var request = new UserUpdateRequest { Id = 3, Username = "changedUser", Email = "changedUser@gmail.com", FirstName = "changedUser", LastName = "changedUserLastName", Password = "2" };
            // Act
            var result = (await _controller.UpdateCurrentUserData(request)) as OkObjectResult;
            // Assert
            var item = result.Value as User;
            Assert.IsType<User>(item);
            Assert.Equal("changedUser", item.Username);
        }

        [Fact]
        public async void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = (await _controller.GetUserList(null)) as OkObjectResult;
            // Assert
            var items = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public async void Approvement_InvalidObjectPassed_ReturnsBadRequest()
        {
            var missingItem = new RegisterApprovementRequest { Approvement = true };
            _controller.ModelState.AddModelError("UserId", "Required");
            // Act
            var badResponse = await _controller.RegisterApprovement(missingItem);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public async void Approvement_ValidObjectPassed_ReturnsOkResult()
        {
            var request = new RegisterApprovementRequest { UserId = 2, Approvement = true };
            var result = await _controller.RegisterApprovement(request);
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Approvement_WhenCalled_ReturnsRightItem()
        {
            var request = new RegisterApprovementRequest { UserId = 2, Approvement = true };
            // Act
            var result = (await _controller.RegisterApprovement(request)) as OkObjectResult;
            // Assert
            var item = result.Value as User;
            Assert.IsType<User>(item);
            Assert.Equal(request.Approvement, item.Approvement);
        }

        [Fact]
        public async void ChangeStatus_InvalidObjectPassed_ReturnsBadRequest()
        {
            var missingItem = new ChangeStatusRequest { Active = true };
            _controller.ModelState.AddModelError("UserId", "Required");
            // Act
            var badResponse = await _controller.ChangeStatus(missingItem);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public async void ChangeStatus_ValidObjectPassed_ReturnsOkResult()
        {
            var request = new ChangeStatusRequest { UserId = 2, Active = true };
            var result = await _controller.ChangeStatus(request);
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void ChangeStatus_WhenCalled_ReturnsRightItem()
        {
            var request = new ChangeStatusRequest { UserId = 2, Active = true };
            // Act
            var result = (await _controller.ChangeStatus(request)) as OkObjectResult;
            // Assert
            var item = result.Value as User;
            Assert.IsType<User>(item);
            Assert.Equal(request.Active, item.Active);
        }


    }


}
