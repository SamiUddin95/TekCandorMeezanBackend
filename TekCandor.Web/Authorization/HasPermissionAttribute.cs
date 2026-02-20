using Microsoft.AspNetCore.Authorization;

namespace TekCandor.Web.Authorization
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission)
        {
            Policy = permission;
        }
    }
}
