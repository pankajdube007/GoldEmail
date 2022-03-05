using Goldmedal.Emails.App_Code.BLL.Common.Settings;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(Goldmedal.Emails.Startup))]
namespace Goldmedal.Emails
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);


          //  GlobalConfiguration.Configure(WebApiConfig.Register);
        }
      
    }
}
