﻿using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;

namespace Infinite.Server.Controllers;

public class OidcConfigurationController : Controller
{
    private readonly IClientRequestParametersProvider _clientRequestParametersProvider;

    public OidcConfigurationController(IClientRequestParametersProvider clientRequestParametersProvider)
    {
        _clientRequestParametersProvider = clientRequestParametersProvider;
    }
    
    [HttpGet("_configuration/{clientId}")]
    public IActionResult GetClientRequestParameters([FromRoute] string clientId)
    {
        var parameters = _clientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
        return Ok(parameters);
    }
}