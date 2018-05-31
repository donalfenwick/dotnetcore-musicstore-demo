using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Endpoints.Results;
using IdentityServer4.Extensions;
using IdentityServer4.ResponseHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace IdentityServerControllers
{
    /// <summary>
    /// Controller provides an alternate openid-configuration json document where the authorization_endpoint, 
    /// check_session_iframe and end_session_endpoint url values can be configured with custom values. 
    /// This is usefull when running both identity server and a consuming oidc application within a docker 
    /// environment. In this scenario the hostname for a redirect url from the client during authorization will be 
    /// different from the hostname in the url used for backend communication with identity server so we need to
    /// override any urls that will be used from the users browser so they dont use the container name in the browser.
    /// see the following issue for more background info https://github.com/aspnet/Security/issues/1175
    /// </summary>
    [Route(".well-known/custom-openid-configuration")]
    public class CustomOidcConfigController : Controller
    {
        private readonly IDiscoveryResponseGenerator _responseGenerator;
        private readonly ILogger _logger;
        private readonly IdentityServerOptions _options;
        private readonly IConfiguration _config;

        public CustomOidcConfigController(IDiscoveryResponseGenerator responseGenerator, IdentityServerOptions options, IConfiguration config, ILogger<CustomOidcConfigController> logger)
        {
            _responseGenerator = responseGenerator;
            _options = options;
            _logger = logger;
            _config = config;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetConfig()
        {
            var context = this.Request.HttpContext;   
            var baseUrl = EnsureTrailingSlash(context.GetIdentityServerBaseUrl());
            var issuerUri = context.GetIdentityServerIssuerUri();

            Dictionary<string, object> response = await _responseGenerator.CreateDiscoveryDocumentAsync(baseUrl, issuerUri);

            // if the document is being accessed via the initernal docker hostname then rewrite the urls
            // used in client side browser redirects to use the base url exposed by docker to the host OS
            
            string browserAccessibleBaseUrl = _config.GetValue<string>("IdentityServerIssuerUri");
            if(!string.IsNullOrWhiteSpace(browserAccessibleBaseUrl)){
                response["authorization_endpoint"] = $"{browserAccessibleBaseUrl}/connect/authorize";
                response["check_session_iframe"]   = $"{browserAccessibleBaseUrl}/connect/checksession";
                response["end_session_endpoint"]   = $"{browserAccessibleBaseUrl}/connect/endsession";
            }
        
            return Json(response);
        }

        private string EnsureTrailingSlash(string input){
            if(input == null) return null;
            return input.TrimEnd('/')+"/";
        }
    }
}