using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace TekCandor.Web.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
        {
            var permissions = context.User.Claims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
