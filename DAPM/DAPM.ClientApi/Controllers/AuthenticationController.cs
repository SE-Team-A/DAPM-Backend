using DAPM.ClientApi.Models;
using DAPM.ClientApi.Models.DTOs;
using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("authentication")]
    public class AuthenticationController : ControllerBase
    {

        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        [SwaggerOperation(Description = "Handles user login and returns a JWT token.")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await _authenticationService.RequestJwtTokenAsync(loginRequest.Username, loginRequest.Password);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new ApiResponse { RequestName = "Login", Message = "Invalid credentials" });
            }
            return Ok(new ApiResponse { RequestName = "Login", Token = token });
        }
    }
}