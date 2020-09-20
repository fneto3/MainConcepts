using System.Web;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Calculator.API.Extension
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}
