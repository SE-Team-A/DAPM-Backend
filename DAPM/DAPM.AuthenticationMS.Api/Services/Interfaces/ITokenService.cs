using System;
using Microsoft.AspNetCore.Identity;

namespace DAPM.AuthenticationMS.Api.Services.Interfaces;

public interface ITokenService
{
    public string CreateToken(IdentityUser user);
}
