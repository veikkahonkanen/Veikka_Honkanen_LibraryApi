using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veikka_Honkanen_LibraryApi
{
    // Bonus 2
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationAttribute : Attribute, IResourceFilter
    {
        private const string AuthorizationHeaderKey = "Authorization";
        private const string AuthorizationKeyValue = "e9a69565-947e-4000-8d2c-58be70c7e9f";

        /// <summary>
        /// Uses default authorization
        /// </summary>
        public AuthorizationAttribute()
        {

        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {

            if (!context.HttpContext.Request.Headers.TryGetValue(AuthorizationHeaderKey, out var secretKey))
            {
                context.Result = new UnauthorizedResult();
                Console.WriteLine("No access for key" + secretKey);
            }

            else
            {
                if (secretKey != AuthorizationKeyValue)
                {
                    context.Result = new UnauthorizedResult();
                }
            }


        }
    }
}
