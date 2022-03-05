using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

public class UserEmailorSmsController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/UserEmailorSms")]
    public HttpResponseMessage GetTestMail(UserEmailorSms mails)
    {
        Common cm = new Common();
        MessageService ms = new MessageService();

        try
        {
            string data1;
            //string Subject;
            //string Body = string.Empty;

            EmailTemplateDA da = new EmailTemplateDA();
            PrivateMessage pm = new PrivateMessage();

            List<UserEmailorSmsDetails> Listtestmail = new List<UserEmailorSmsDetails>();
            string Attach = string.Empty;
            if (mails.AtachmentUrl != "")
            {
                Attach = ProperAttachmentUrl(mails.AtachmentUrl, 9999, mails.UserId);
            }

            if (mails.Perpose == "Email")
            {
                da.AddEmailQueue(0, mails.To, mails.ToName, mails.Cc, mails.Bcc, mails.Subject, mails.Body, mails.SendOn, "9999-12-31", mails.UserId, mails.Logno, 2017, Attach, "User-Password-Reset", mails.SendNow);
            }
            else
                if (mails.Perpose == "SMS")
            {
                da.AddMessageQueue(0, mails.To, mails.ToName, mails.Body, mails.SendOn, "9999-12-31", mails.UserId, mails.Logno, 2003, "User-Password-Reset", mails.SendNow);
            }
            Listtestmail.Add(new UserEmailorSmsDetails
            {
                result = "True",
                message = "Added in Queue",
                servertime = DateTime.Now.ToString(),
                data = "",
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