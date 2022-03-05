using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

public class SchemeUploadSMSController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/SchemeUploadSMS")]
    public HttpResponseMessage AddSchemeUploadSMS(SchemeUpload sms)
    {
        Common cm = new Common();
        MessageService ms = new MessageService();

        try
        {
            string data1;
            //  string Subject;
            string Body = "";

            EmailTemplateDA da = new EmailTemplateDA();
            PrivateMessage pm = new PrivateMessage();

            DataTable dt = da.GetMeassageById(sms.ID);
            //   List<testmails> testmail = new List<testmails>();
            List<ScemeUploadSMSs> Listsms = new List<ScemeUploadSMSs>();
            //  Subject = ms.ReplaceEmailTemplateCreateInvoice(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString());

            if (dt.Rows.Count > 0)
            {
                Body = ms.ReplaceEmailTemplateSchemeUpload(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), sms).Replace("&", "and");
            }
            //   string Attach = ProperAttachmentUrl(mails.Attachment, mails.ID, mails.UserID);
            if (dt.Rows.Count > 0)
            {
                da.AddMessageQueue(0, sms.To, sms.ToName, Body, Convert.ToDateTime(dt.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt.Rows[0]["LastMessageDate"]).ToString(), sms.UserID, sms.LogNO, sms.ID, "Scheme-Upload", Convert.ToByte(sms.SendNow));
            }

            Listsms.Add(new ScemeUploadSMSs
            {
                result = "True",
                message = "Test Mail Send",
                servertime = DateTime.Now.ToString(),
                // data = testmail,
            });

            data1 = JsonConvert.SerializeObject(Listsms, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
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