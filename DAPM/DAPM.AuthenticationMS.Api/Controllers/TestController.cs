using DAPM.AuthenticationMS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.AuthenticationMS.Api.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUserService _userService;

        public TestController(IUserService userService) {
            _userService = userService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<IdentityResult>> CreateUser()
        {
            IdentityResult result = await _userService.CreateUserAsync("username1", "Password1@");
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<bool>> LoginUser()
        {
            try
            {
                string token = await _userService.LoginUserAsync("username1--", "Password1@--");
                return Ok(token);
            } catch (Exception e) {
                return Problem(
                    detail: e.Message,
                    statusCode: StatusCodes.Status403Forbidden
                );
            }
        }
    }
}
