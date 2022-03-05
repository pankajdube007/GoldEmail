<%@ Application Language="C#" %>
<%@ Import Namespace="Goldmedal.Emails" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Net.Mail" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Goldmedal.Emails.App_Code.BLL.Common.Settings" %>

<script RunAt="server">

    void Application_BeginRequest(object sender, EventArgs e)
    {
        GoldConfig.Init();//
    }

    void Application_Start(object sender, EventArgs e)
    {
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);

        GlobalConfiguration.Configure(WebApiConfig.Register);
        GoldConfig.Init();
        TaskManager.Instance.Initialize(GoldConfig.ScheduleTasks);
        TaskManager.Instance.Start();
    }

    void Application_End(object sender, EventArgs e)
    {
        TaskManager.Instance.Stop();
    }
    //void Application_Error(object sender, EventArgs e)
    //{
    //    HttpException lastErrorWrapper = Server.GetLastError() as HttpException;
    //    Exception lastError = lastErrorWrapper;
    //    if (lastErrorWrapper.InnerException != null)
    //        lastError = lastErrorWrapper.InnerException;

    //    string lastErrorTypeName = lastError.GetType().ToString();
    //    string lastErrorMessage = lastError.Message;
    //    string lastErrorStackTrace = lastError.StackTrace;
    //    string source = "Email";
    //    string errorCode = lastErrorWrapper.ErrorCode.ToString();
    //    string help = lastError.HelpLink;
    //    string IP = "127.0.0.1";
    //    var val=ClientDetails.GetClientDetails();
    //    IP = val.Select(s => s.UserHostAddress).FirstOrDefault();
    //    string Subject = "Oops!! An error has been occurred in " + source + " @ " + DateTime.Now.ToString();

    //    MailMessage mm = new MailMessage();
    //    mm.From = new MailAddress("software.support@goldmedalindia.com", "Goldmedal:Error(Email)");
    //    mm.To.Add(new MailAddress("pankajdube007@gmail.com"));
    //    mm.CC.Add(new MailAddress("it@goldmedalindia.com"));
    //    mm.CC.Add(new MailAddress("santanu.goldmedalindia@gmail.com"));
    //    mm.IsBodyHtml = true;
    //    mm.Priority = MailPriority.High;
    //    mm.Subject = Subject;
    //    mm.Body = string.Format(@"
    //                                <html>
    //                                <body>
    //                                  <h1>This exception is coming from {0}. </h1>
    //                                  <table cellpadding=""5"" cellspacing=""0"" border=""1"">
    //                                  <tr>
    //                                  <tdtext-align: right;font-weight: bold"">Application Name:</td>
    //                                  <td>{0}</td>
    //                                  </tr>
    //                                  <tr>
    //                                  <tdtext-align: right;font-weight: bold;color:green!important"">Raw Url:</td>
    //                                  <td color:green!important"">{1}</td>
    //                                  </tr>
    //                                  <tr>
    //                                  <tdtext-align: right;font-weight: bold"">User:</td>
    //                                  <td>{2}</td>
    //                                  </tr>
    //                                  <tr>
    //                                  <tdtext-align: right;font-weight: bold"">Log No:</td>
    //                                  <td>{3}</td>
    //                                  </tr>
    //                                  <tr>
    //                                  <tdtext-align: right;font-weight: bold"">From IP:</td>
    //                                  <td>{3}</td>
    //                                  </tr>
    //                                  <tr>
    //                                  <tdtext-align: right;font-weight: bold"">Error Code:</td>
    //                                  <td>{4}</td>
    //                                  </tr>
    //                                  <tr>
    //                                  <tdtext-align: right;font-weight: bold"">Exception Type:</td>
    //                                  <td>{5}</td>
    //                                  </tr>
    //                                  <tr>
    //                                  <tdtext-align: right;font-weight: bold;color:red!important"">Message:</td>
    //                                  <td color:red!important"">{6}</td>
    //                                  </tr>
    //                                  <tr>
    //                                  <tdtext-align: right;font-weight: bold;color:saddlebrown!important"">Help:</td>
    //                                  <td color:saddlebrown!important"">{7}</td>
    //                                  </tr>
    //                                  <tr>
    //                                  <tdtext-align: right;font-weight: bold"">Stack Trace:</td>
    //                                  <td>{8}</td>
    //                                  </tr> 
    //                                  </table>
    //                                </body>
    //                                </html>",
    //        source,
    //        Request.RawUrl,
    //        Request.Cookies["name"].Value,
    //        Request.Cookies["logno"].Value,
    //        IP,
    //        errorCode,
    //        lastErrorTypeName,
    //        lastErrorMessage,
    //        help,
    //        lastErrorStackTrace.Replace(Environment.NewLine, "<br />"));

    //    string YSODmarkup = lastErrorWrapper.GetHtmlErrorMessage();
    //    if (!string.IsNullOrEmpty(YSODmarkup))
    //    {
    //        Attachment YSOD = Attachment.CreateAttachmentFromString(YSODmarkup, "YSOD.htm");
    //        mm.Attachments.Add(YSOD);
    //    }
    //    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
    //    smtpClient.Credentials = new System.Net.NetworkCredential("software.support@goldmedalindia.com", "8080772544");
    //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
    //    smtpClient.EnableSsl = true;
    //    smtpClient.Send(mm);
    //}




</script>
