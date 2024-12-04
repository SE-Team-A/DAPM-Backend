using DAPM.PeerApi.Models;
using DAPM.PeerApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using RabbitMQLibrary.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace DAPM.PeerApi.Services
{
    public class HttpService : IHttpService
    {
        private IHttpClientFactory _httpClientFactory;
        private ITokenService _tokenService;
        private ILogger<HttpService> _logger;
        public HttpService(IHttpClientFactory httpClientFactory, ITokenService tokenService, ILogger<HttpService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<string> SendPostRequestAsync(string url, string body)
        {
            var client = _httpClientFactory.CreateClient();

            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var token = _tokenService.createToken();

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await client.PostAsync(url, content);

            _logger.LogInformation($"INITIAL URL {url}");
            _logger.LogInformation(response.StatusCode.ToString());

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        public async Task<bool> verifyExternalToken(string externalDomain, string header)
        {

            if (string.IsNullOrEmpty(header))
            {
                return false;
            }

            string token = header.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);

            try {

                var verifyTokenDto = new VerifyTokenDto()
                {
                    Token = token
                };

                var url = "http://" + externalDomain + PeerApiEndpoints.VerifyTokenEndpoint;
                var body = JsonSerializer.Serialize(verifyTokenDto);

                await SendPostRequestAsync(url, body);

            } 
            catch (HttpRequestException)
            {
                return false;
            }

            return true;
        }
    }
}
