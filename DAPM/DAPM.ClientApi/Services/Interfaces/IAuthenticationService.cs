﻿using DAPM.ClientApi.Models;
using RabbitMQLibrary.Messages.Authentication;
using System.Xml.Linq;

namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IAuthenticationService

    {
        Guid PostLogin(string username, string password);
    }

}