using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LenzoGlobalAPI
{
    public class RestrictDomainAttribute : Attribute, IAuthorizationFilter
    {
        public IEnumerable<string> AllowedHosts { get; }

        public RestrictDomainAttribute(params string[] allowedHosts) => AllowedHosts = allowedHosts;

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var hasOrigin = context.HttpContext.Request.Headers.TryGetValue("Origin", out var origin);

            //  Get host from the request and check if it's in the enumeration of allowed hosts
            string host = context.HttpContext.Request.Host.Host;
            if (!AllowedHosts.Contains(origin.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                //  Request came from an authorized host, return bad request
                context.Result = new BadRequestObjectResult("Host is not allowed");
            }
        }
    }

    public class Domains
    {
        public string[] AllDomains { get; set; }
    }
}