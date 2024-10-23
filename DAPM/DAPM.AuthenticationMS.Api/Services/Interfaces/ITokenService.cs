using System;
using Microsoft.AspNetCore.Identity;

namespace DAPM.AuthenticationMS.Api.Services.Interfaces;
/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
public interface ITokenService
{
    public Task<string> CreateToken(IdentityUser user);
}
