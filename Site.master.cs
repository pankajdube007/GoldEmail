using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using DevExpress.Web;

public partial class SiteMaster : MasterPage
{
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;
    private UserManagement userManagement = null;
    private static string Role = string.Empty;
    private  string Name = string.Empty;
    public string unreadmessage = string.Empty;
    public SiteMaster()
    {
        userManagement = new UserManagement();
        Role = userManagement.GetUserInfoString(UserEnum.UserInfoEnum.Usercat);
        Name = userManagement.GetUserInfoString(UserEnum.UserInfoEnum.UserLogName);
    }
    //public class MySiteMapProvider : UnboundSiteMapProvider
    //{
    //    public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
    //    {
    //        return IsRolesAccessibleToCurrentUser(node.Roles);
    //    }
    //}
    //public static bool IsRolesAccessibleToCurrentUser(IList roles)
    //{
    //    if(roles.Count>0)
    //    {
    //        if(roles.Contains("*"))
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            if (roles.Contains(Role))
    //            {
    //                return true;
    //            }
    //        }           
    //        return false;
    //    }
    //    return true;   
    //}
    protected void Page_Init(object sender, EventArgs e)
    {
        // The code below helps to protect against XSRF attacks
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;
        if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            // Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value;
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        else
        {
            // Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                HttpOnly = true,
                Value = _antiXsrfTokenValue
            };
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;
    }

    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set Anti-XSRF token
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            //ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            //ViewState[AntiXsrfUserNameKey] = CommonHelper.GetCookieString("logged", true) ?? String.Empty;
            ViewState[AntiXsrfUserNameKey] = Name ?? String.Empty;
        }
        else
        {
            // Validate the Anti-XSRF token
            //if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue  || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue || (string)ViewState[AntiXsrfUserNameKey] != (Name ?? String.Empty))
            {
                var log = new GoldLogging();
                log.SendErrorToText( new InvalidOperationException("Validation of Anti-XSRF token failed."));
                throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (userManagement.GetUserInfoLong(UserEnum.UserInfoEnum.LogNo) == 0 || string.Equals(CommonHelper.GetCookieString("logged", Convert.ToBoolean(1)), "no") || userManagement.GetUserInfoInt(UserEnum.UserInfoEnum.UserLogID) == 0 || userManagement.GetUserInfoString(UserEnum.UserInfoEnum.Usercat) == "")
        {
            const string logged = "no";
            CommonHelper.SetCookie("logged", logged, TimeSpan.FromHours(12));
            Response.Redirect("~/Presentation/User-Logout.aspx");
        }
        //MySiteMapProvider provider = new MySiteMapProvider()
        //{
        //    SiteMapFileName = "~/FactoryMenu.sitemap",
        //    EnableRoles = true
        //};
        //sideNavBarSiteMapDataSource.Provider = provider;
    }

    protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        Context.GetOwinContext().Authentication.SignOut();
    }
}