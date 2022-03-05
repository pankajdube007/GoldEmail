using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

public class TestPartywiseOutController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/TestPartyWiseOutstanding")]
    public HttpResponseMessage GetTestMail(PartywiseOutstanding mails)
    {
        Common cm = new Common();
        MessageService ms = new MessageService();
        WebClient web = new WebClient();

        try
        {
            string data1;
            string Subject;
            string Body = string.Empty;
            byte[] bufData = null;

            EmailTemplateDA da = new EmailTemplateDA();
            PrivateMessage pm = new PrivateMessage();

            DataTable dt = da.GetEmailTemplateDatabyID(mails.ID);
            DataTable dt2 = da.GetMeassageById(mails.ID);

            List<PartywiseOutstanding> testmail = new List<PartywiseOutstanding>();
            List<PartywiseOutstandingDetails> Listtestmail = new List<PartywiseOutstandingDetails>();
            if (mails.For == "Email")
            {
                Subject = HttpUtility.HtmlDecode(ms.ReplaceEmailTemplatePartyWiseOutstanding(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), mails));
                Body = ms.ReplaceEmailTemplatePartyWiseOutstanding(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), mails);
                Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                if (dt.Rows.Count > 0)
                {
                    List<string> ls = new List<string>();
                    EmailAccount ea = new EmailAccount();

                    DataTable dt1 = ms.GetEmailAccountForTestMail();

                    if (dt1.Rows.Count > 0)
                    {
                        ea.EmailAccountId = Convert.ToInt32(dt1.Rows[0]["EmailAccountId"]);
                        ea.Email = Convert.ToString(dt1.Rows[0]["Email"]);
                        ea.DisplayName = Convert.ToString(dt1.Rows[0]["DisplayName"]);
                        ea.Host = Convert.ToString(dt1.Rows[0]["Host"]);
                        ea.Port = Convert.ToInt32(dt1.Rows[0]["Port"]);
                        ea.Username = Convert.ToString(dt1.Rows[0]["Username"]);
                        ea.Password = Convert.ToString(dt1.Rows[0]["Password"]);
                        ea.EnableSSL = Convert.ToBoolean(dt1.Rows[0]["EnableSSL"]);
                        ea.UseDefaultCredentials = Convert.ToBoolean(dt1.Rows[0]["UseDefaultCredentials"]);
                        ea.Attchment = ProperAttachmentUrl(mails.Attachment, mails.ID, mails.UserID);
                    }

                    ms.SendEmail(Subject, Body,
                           new MailAddress(ea.Email, ea.DisplayName),
                           new MailAddress(mails.To, mails.ToName), ls, ls, ea);
                }
            }
            else if (mails.For == "SMS")
            {
                if (dt2.Rows.Count > 0)
                {
                    Body = ms.ReplaceEmailTemplatePartyWiseOutstanding(dt2.Rows[0]["Body"].ToString(), dt2.Rows[0]["Body"].ToString(), mails).Replace("&", "and");
                    //   Body = "We Are testing Please ignore prevous messages";

                    bufData = web.DownloadData("http://sms6.routesms.com:8080/bulksms/bulksms?username=" + WebConfigurationManager.AppSettings["SmsUser"].ToString() + "&password=" + WebConfigurationManager.AppSettings["SmsPassword"].ToString() + "&type=0&dlr=0&destination=" + mails.To + "&source=GLDMDL&message=" + Body);
                }
            }

            Listtestmail.Add(new PartywiseOutstandingDetails
            {
                result = "True",
                message = "Test Mail Send",
                servertime = DateTime.Now.ToString(),
                // data = testmail,
            });

            data1 = JsonConvert.SerializeObject(Listtestmail, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(data1, Encoding.Unicode);
            return response;
        }
        catch (Exception ex)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(cm.StatusTime(false, "Oops! Something is wrong, try again later!!!!!!!!" + ex.ToString()), Encoding.Unicode);

            return response;
        }
    }

    public string ProperAttachmentUrl(string Attachment, int MailId, int UserId)

    {
        string AttachmentUrl = string.Empty;
        foreach (string str1 in Attachment.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            //  AttachmentUrl = AttachmentUrl + "Upload\\ErpAttachment\\" + MailId + "\\" + UserId + "\\" + str1 + ",";
            AttachmentUrl = AttachmentUrl + MailId + "\\" + UserId + "\\" + str1 + ",";
        }
        return AttachmentUrl;
    }
}