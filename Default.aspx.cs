using System;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
//using System.Text;
using System.Configuration;

public partial class _Default : System.Web.UI.Page
{
    #region Initialization
    private UserManagement userManagement = null;
    private UserValidateModel.UserInputs userLogin = null;
    public _Default()
    {
        userManagement = new UserManagement();
        userLogin = new UserValidateModel.UserInputs();
    }
    #endregion
    #region pageEvent
    protected void Page_Load(object sender, EventArgs e)
    {
        mpeImage.Show();
        if (!IsPostBack)
        {
            var info = ClientDetails.GetClientDetails();
            string msg = string.Empty;
            foreach (var item in info)
            {
                hip.Value = item.UserHostAddress;
            }            
        }
    }
    #endregion
    #region Method
    protected void PrintResult(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            var responseString = userManagement.AuthUser(str);
            if (!string.IsNullOrEmpty(responseString))
            {
                FailureText.Text = responseString;
                ErrorMessage.Visible = true;
            }
        }
        else
        {
            FailureText.Text = "Something Wrong";
            ErrorMessage.Visible = true;
            const string logged = "no";
            CommonHelper.SetCookie("logged", logged, TimeSpan.FromHours(12));
        }
    }
    protected string ValidateAsync(UserValidateModel.UserInputs userLogin)
    {
        string retstring = string.Empty;
        try
        {
            string data = JsonConvert.SerializeObject(userLogin);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["url"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsync("ValidateOtherUser", new StringContent(data, Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent content = response.Content)
                    {
                        retstring = content.ReadAsStringAsync().Result;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new GoldException(string.Format("Exception In login {0}", ex.InnerException));
        }
        return retstring;
    }
    protected bool CheckValidation()
    {
        string error = string.Empty;
        if (string.IsNullOrEmpty(UserName.Text.Trim()))
        {
            error += "Please Input User Name";
        }
        else if (!CommonHelper.IsValidEmail(UserName.Text.Trim()))
        {
            error += "Please Input Valid Email";
        }
        else if (string.IsNullOrEmpty(Password.Text.Trim()))
        {
            error += "Please Input Valid Password";
        }
        else if (string.IsNullOrEmpty(hip.Value))
        {
            error += "Try from Different Browser";
        }
        else if (UserName.Text.Trim() != "it@goldmedalindia.com")
        {
            error += "Not Valid User";
        }


        if (!string.IsNullOrEmpty(error))
        {
            FailureText.Text = error;
            ErrorMessage.Visible = true;
            return false;
        }
        else
        {

            return true;
        }
    }
    #endregion
    #region Event
    protected void LogIn(object sender, EventArgs e)
    {
        if (IsValid)
        {
            FailureText.Text = "";
            if (CheckValidation())
            {
                string[] myCookies = Request.Cookies.AllKeys;
                foreach (string cookie in myCookies)
                {
                    if (!string.Equals(cookie, "__AntiXsrfToken"))
                    {
                        Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                    }
                }
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();

                userLogin.usernm = HttpUtility.HtmlEncode(CommonHelper.EnsureMaximumLength(UserName.Text.Trim(), 50));
                userLogin.pwd = HttpUtility.HtmlEncode(CommonHelper.EnsureMaximumLength(Password.Text.Trim(), 16));
                userLogin.ip = hip.Value;

                var result = ValidateAsync(userLogin);
                PrintResult(result);

                if (string.Equals(CommonHelper.GetCookieString("logged", true), "yes"))
                {
                    Response.Redirect("~/Presentation/Dashboard/DashBoard.aspx");
                }
                else
                {
                    String[] myCookie = Request.Cookies.AllKeys;
                    foreach (string cookie in myCookie)
                    {
                        if (!string.Equals(cookie, "__AntiXsrfToken"))
                        {
                            Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                        }
                    }
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Session.Abandon();
                }
            }
        }
    }
    #endregion
}