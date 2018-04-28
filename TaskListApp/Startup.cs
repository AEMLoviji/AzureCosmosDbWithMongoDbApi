using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaskListApp.Startup))]
namespace TaskListApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
