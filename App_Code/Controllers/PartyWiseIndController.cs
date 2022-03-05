using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

public class PartyWiseIndController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/PartyWiseInd")]
    public HttpResponseMessage GetTestMail(PartyWiseInd mails)
    {
        Common cm = new Common();
        MessageService ms = new MessageService();

        try
        {
            string data1;
            string Subject;
            string Body = string.Empty;

            EmailTemplateDA da = new EmailTemplateDA();
            PrivateMessage pm = new PrivateMessage();

            List<PartyWiseInd> testmail = new List<PartyWiseInd>();
            List<PartyWiseIndDetails> Listtestmail = new List<PartyWiseIndDetails>();

            if (mails.For == "Email")
            {
                Subject = mails.Subject;
                Body = mails.Body;

                da.AddEmailQueue(0, mails.To, mails.ToName, mails.CC, "", Subject, Body, mails.SendingTime, "9999-12-01", mails.UserID, mails.LogNO, 2017, "", "Written By", 0);
            }
            else
                if (mails.For == "SMS")
            {
                Body = mails.Body;
                da.AddMessageQueue(0, mails.To, mails.ToName, Body, mails.SendingTime, "9999-12-01", mails.UserID, mails.LogNO, 2017, "Written By", 0);
            }
            Listtestmail.Add(new PartyWiseIndDetails
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