using Hangfire.Dashboard;
using NewsByTheMood.Core.Settings;

public class HangfireAdminAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        return httpContext.User.Identity?.IsAuthenticated == true &&
               httpContext.User.IsInRole(AccessLevels.Admininistrator);
    }
}