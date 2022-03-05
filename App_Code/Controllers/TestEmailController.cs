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
using System.Web.Http;

public class TestEmailController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/TestMail")]
    public HttpResponseMessage GetTestMail(SchemeUpload mails)
    {
        Common cm = new Common();
        MessageService ms = new MessageService();

        try
        {
            string data1;
            string Subject;
            string Body = "";

            EmailTemplateDA da = new EmailTemplateDA();
            PrivateMessage pm = new PrivateMessage();

            DataTable dt = da.GetEmailTemplateDatabyID(mails.ID);
            //ListCI.ToEmail = "pankaj@gmail.com";
            //ListCI.ToName = "Pankaj Dube";
            List<SchemeUpload> testmail = new List<SchemeUpload>();
            List<Listtestmail> Listtestmail = new List<Listtestmail>();

            Subject = HttpUtility.HtmlDecode(ms.ReplaceEmailTemplateSchemeUpload(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), mails));
            //    Subject =ms.ReplaceEmailTemplateSchemeUpload(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(),mails);
            Body = ms.ReplaceEmailTemplateSchemeUpload(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), mails);

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
                    ea.Attchment = mails.Attachment;
                }

                ms.SendEmail(Subject, Body,
                      new MailAddress(ea.Email, ea.DisplayName),
                      new MailAddress(mails.To, "Test"), ls, ls, ea);
            }

            Listtestmail.Add(new Listtestmail
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

    //public string ProperAttachmentUrl(string Attachment, int MailId, int UserId)

    //{
    //    string AttachmentUrl = string.Empty;
    //    foreach (string str1 in Attachment.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
    //    {
    //        AttachmentUrl = AttachmentUrl + MailId + "\\" + UserId + "\\" + str1 + ",";
    //        //   AttachmentUrl = "~/Upload/ErpAttachment/"+MailId+"/"+UserId+"/" +str1+",";
    //        // AttachmentUrl = AttachmentUrl + "~/Upload/ErpAttachment/" + MailId + "/" + UserId + "/" + str1 + ",";
    //    }
    //    return AttachmentUrl;
    //}
}