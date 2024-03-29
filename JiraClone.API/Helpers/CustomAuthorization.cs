﻿using JiraClone.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace JiraClone.API.Helpers
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute(bool adminOnly)
        {
            Policy = "adminonly=" + adminOnly;
        }

        public CustomAuthorizeAttribute(params PrivilegeList[] privileges)
        {
            Policy = "privileges=" + string.Join(",", privileges.Select(x => x.ToString()));
        }
        public CustomAuthorizeAttribute(string objectCode, params PrivilegeList[] privileges)
        {
            Policy = "privileges=" + string.Join(",", privileges.Select(x => objectCode + "-" + ((int)x > 9 ? "" + (int)x : "0" + (int)x)));
        }
        public CustomAuthorizeAttribute(string objectCode, bool isAll)
        {
            List<PrivilegeList> lstper = new List<PrivilegeList>();
            if (isAll)
            {
                lstper.Add(PrivilegeList.Add);
                lstper.Add(PrivilegeList.Approved);
                lstper.Add(PrivilegeList.Delete);
                lstper.Add(PrivilegeList.Edit);
                lstper.Add(PrivilegeList.Permission);
            }
            Policy = "privileges=" + string.Join(",", lstper.Select(x => objectCode + "-" + ((int)x > 9 ? "" + (int)x : "0" + (int)x)));
        }
    }

    public class CustomAuthorizationRequirement : IAuthorizationRequirement
    {
        public string[] Privileges { get; private set; }
        public bool AdminOnly { get; private set; }

        public CustomAuthorizationRequirement(bool adminOnly)
        {
            AdminOnly = adminOnly;
        }

        public CustomAuthorizationRequirement(params string[] privileges)
        {
            Privileges = privileges;
        }
    }

    public class CustomAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies (including default policies, etc.) it should fall back to an
            // alternate provider.
            //
            // In this sample, a default authorization policy provider (constructed with options from the 
            // dependency injection container) is used if this custom provider isn't able to handle a given
            // policy name.
            //
            // If a custom policy provider is able to handle all expected policy names then, of course, this
            // fallback pattern is unnecessary.
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        // Policies are looked up by string name, so expect 'parameters' (like age)
        // to be embedded in the policy names. This is abstracted away from developers
        // by the more strongly-typed attributes derived from AuthorizeAttribute
        // (like [MinimumAgeAuthorize] in this sample)
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("adminonly"))
            {
                var policy = new AuthorizationPolicyBuilder();

                var adminOnly = Convert.ToBoolean(policyName.Substring(10));

                policy.AddRequirements(new CustomAuthorizationRequirement(adminOnly));

                return Task.FromResult(policy.Build());
            }
            else if (policyName.StartsWith("privileges"))
            {
                var policy = new AuthorizationPolicyBuilder();

                var privileges = policyName.Substring(11).Split(",").ToArray();

                policy.AddRequirements(new CustomAuthorizationRequirement(privileges));

                return Task.FromResult(policy.Build());
            }

            // If the policy name doesn't match the format expected by this policy provider,
            // try the fallback provider. If no fallback provider is used, this would return 
            // Task.FromResult<AuthorizationPolicy>(null) instead.
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();
    }

    public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
        {

            bool success = true;

            if (!context.User.Identity.IsAuthenticated)
            {
                success = false;
            }
            else
            {
                var userIdentity = new UserIdentity(context.User);
                if (userIdentity.IsAdministrator)
                {
                    success = true;
                }
                else
                {
                    if (requirement.AdminOnly)
                    {
                        success = userIdentity.IsAdministrator;
                    }
                    else if (requirement.Privileges.Any())
                    {
                        success = userIdentity.Privileges.Any(x => requirement.Privileges.Contains(x));
                    }
                }
            }
            if (success)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
