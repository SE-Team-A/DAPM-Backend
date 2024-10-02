using DAPM.AuthenticationMS.Api.Models;
using DAPM.AuthenticationMS.Api.Services.Interfaces;
using DAPM.AuthenticationMS.Api.Repositories.Interfaces;

namespace DAPM.AuthenticationMS.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IAuthenticationRepository _authenticationRepository;
        private readonly ILogger<IAuthenticationService> _logger;

        public AuthenticationService(ILogger<IAuthenticationService> logger, IAuthenticationRepository _authenticationRepository) 
        {
            _authenticationRepository = authenticationRepository;
            _logger = logger;
        }

        public async Task<bool> PostLogin(string username, string password)
        {
            return await _authenticationRepository.PostLogin(username, password);
        }
    }
    
}

