using Microsoft.AspNetCore.Mvc;
using UserPortal.Extensions;
using UserPortal.Helper;
using UserPortal.Interfaces;
using UserPortal.Models;

namespace UserPortal.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IKafkaService _kafkaService;

        public UserController(IUserService userService, IKafkaService kafkaService)
        {
            _userService = userService;
            _kafkaService = kafkaService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = await _userService.Authenticate(model);
            if (token == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            var response = new AuthenticateResponse { Token = token };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _userService.Registration(model);
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateCurrentUserData(UserUpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _userService.UpdateCurrentUserDataAsync(model);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserList([FromQuery] PageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var list = await _kafkaService.GetUserList(request);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterApprovement(RegisterApprovementRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _kafkaService.RegisterApprovement(request);
            return Ok(response);
        }
    }
}
