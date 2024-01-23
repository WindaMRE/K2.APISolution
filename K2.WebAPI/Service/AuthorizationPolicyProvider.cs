using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace K2.WebAPI.Service
{
    public class AuthorizationPolicyProvider
    {
        public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
        {
            private readonly Core.Interfaces.IUsersService _User;
            public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)//, Core.Interfaces.IUsersService User)
               : base(options)
            {
               // _User = User;
            }

            public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
            {

                //if ( _User.IsTokenValidate ())
                //{
                if (!policyName.StartsWith(PermissionAuthorizeAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    return base.GetPolicyAsync(policyName);
                }

                var permissionNames = policyName[PermissionAuthorizeAttribute.PolicyPrefix.Length..].Split(',');

                var policy = new AuthorizationPolicyBuilder()
                    .RequireClaim(CustomClaimTypes.Permission, permissionNames)
                    .Build();

                return Task.FromResult(policy);

                //}
                //return base.GetPolicyAsync(policyName);

            }
        }

        public class PermissionAuthorizeAttribute : AuthorizeAttribute
        {
            internal const string PolicyPrefix = "PERMISSION:";
            /// <summary>
            /// Creates a new instance of <see cref="AuthorizeAttribute"/> class.
            /// </summary>
            /// <param name="permissions">A list of permissions to authorize</param>
            public PermissionAuthorizeAttribute(params string[] permissions)
            {
                Policy = $"{PolicyPrefix}{string.Join(",", permissions)}";
            }
        }
    }
}