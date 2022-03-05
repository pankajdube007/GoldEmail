using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

public class EveryDayInvoiceController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/EveryDayInvoice")]
    public HttpResponseMessage GetTestMail(EveryInvoice mailsms)
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

            DataTable dt = da.GetEmailTemplateDatabyID(mailsms.ID);

            List<EveryInvoice> testmail = new List<EveryInvoice>();
            List<EveryInvoiceDetails> Listtestmail = new List<EveryInvoiceDetails>();
            //  string Attach = string.Empty;

            //if(mailsms.Attachment!="")
            //{
            //    Attach = ProperAttachmentUrl(mailsms.Attachment,mailsms.ID,mailsms.UserID);
            //}

            if (mailsms.For == "Email")
            {
                Subject = HttpUtility.HtmlDecode(dt.Rows[0]["Subject"].ToString());

                Body = dt.Rows[0]["Body"].ToString();

                Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                da.AddEmailQueue(0, mailsms.To, mailsms.ToName, "", "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, mailsms.Attachment, "EveryDay-Invoice", 0);
            }
            else
                if (mailsms.For == "SMS")
            {
            }
            Listtestmail.Add(new EveryInvoiceDetails
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
            //   AttachmentUrl = AttachmentUrl + "Upload\\ErpAttachment\\" + MailId + "\\" + UserId + "\\" + str1 + ",";

            AttachmentUrl = AttachmentUrl + MailId + "\\" + UserId + "\\" + str1 + ",";
        }
        return AttachmentUrl;
    }
}