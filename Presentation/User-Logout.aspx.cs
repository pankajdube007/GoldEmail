using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] myCookies = Request.Cookies.AllKeys;
        foreach (string cookie in myCookies)
        {
            if(!string.Equals(cookie, "__AntiXsrfToken"))
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
        }
        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();
        //Session.Clear();
        //Session.Abandon();
        Response.Redirect("~/Default.aspx");
    }
}