﻿using DAPM.ClientApi.Models;
using DAPM.ClientApi.Models.DTOs;
using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [SwaggerOperation(Description = "Send user login request")]
        public async Task<ActionResult<Guid>> PostLogin([FromBody] LoginRequestDTO loginRequestDTO)
        {
            Guid id = _authenticationService.PostLogin(loginRequestDTO.username, loginRequestDTO.password);
            return Ok(new ApiResponse { RequestName = "PostLogin", TicketId = id });
        }

        [HttpPost("registration")]
        [SwaggerOperation(Description = "Send user registration request")]
        public async Task<ActionResult<Guid>> PostRegistration([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            Guid id = _authenticationService.PostRegistration(registrationRequestDTO.username, registrationRequestDTO.password, registrationRequestDTO.name, registrationRequestDTO.role);
            return Ok(new ApiResponse { RequestName = "PostRegistration", TicketId = id });
        }
    }
}