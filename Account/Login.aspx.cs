using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.UI;
using Goldmedal.Emails;
using System.Net;

public partial class Account_Login : Page
{  
    protected void Page_Load(object sender, EventArgs e)
    {
        hip.Value = "127.0.0.1";
        hhost.Value = "Unknown";
        GetUserIPAddress();
        mpeImage.Show();
        RegisterHyperLink.NavigateUrl = "Register";
        OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
        var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        if (!String.IsNullOrEmpty(returnUrl))
        {
            RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
        }

    }

    protected void LogIn(object sender, EventArgs e)
    {
        if (IsValid)
        {
            // Validate the user password
            var manager = new UserManager();
            ApplicationUser user = manager.Find(UserName.Text, Password.Text);
            if (user != null)
            {
                IdentityHelper.SignIn(manager, user, RememberMe.Checked);
                using (var context=new ApplicationDbContext())
                {
                    context.UserLog.Add(new UserLog { UserLogID = Guid.NewGuid(), IP = hip.Value, MachineName = hhost.Value, LoginDate = DateTime.UtcNow, UserId = user.Id });
                    context.SaveChanges();
                }
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else
            {
                FailureText.Text = "Invalid username or password.";
                ErrorMessage.Visible = true;
            }
        }
    }
    public void GetUserIPAddress()
    {
        try
        {
            var context = System.Web.HttpContext.Current;
            string ip = String.Empty;
            string name = context.Request.UserHostName;
            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
                ip = context.Request.UserHostAddress;

            if (ip == "::1")
                ip = "127.0.0.1";

            hip.Value = ip;
            hhost.Value = (Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName);
        }
        catch(Exception ex)
        {

        }
        
    }
}