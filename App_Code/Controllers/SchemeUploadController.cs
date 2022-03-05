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

public class SchemeUploadController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/SchemeUpload")]
    public HttpResponseMessage AddSchemeUploadEmail(SchemeUpload mails)
    {
        Common cm = new Common();
        MessageService ms = new MessageService();

        try
        {
            string data1 = string.Empty;
            string Subject = string.Empty;
            string Body = string.Empty;
            string BCC = string.Empty;
            EmailTemplateDA da = new EmailTemplateDA();
            PrivateMessage pm = new PrivateMessage();

            DataTable dt = da.GetEmailTemplateDatabyID(mails.ID);
            //   List<testmails> testmail = new List<testmails>();
            List<SchemeUploads> Listtestmail = new List<SchemeUploads>();
            Subject = HttpUtility.HtmlDecode(ms.ReplaceEmailTemplateSchemeUpload(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), mails));
            Body = ms.ReplaceEmailTemplateSchemeUpload(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), mails);
            Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

          //  string Attach = ProperAttachmentUrl(mails.Attachment, mails.ID, mails.UserID);
            if (mails.BCC != "")
            {
                BCC = dt.Rows[0]["BCCEmailAddresses"].ToString() + ";" + mails.BCC;
            }

            if (dt.Rows.Count > 0)
            {
                da.AddEmailQueue(0, mails.To, mails.ToName, "", BCC, Subject, Body, Convert.ToDateTime(dt.Rows[0]["EmailTime"]).ToString(), Convert.ToDateTime(dt.Rows[0]["EmailLastDate"]).ToString(), mails.UserID, mails.LogNO, mails.ID, mails.Attachment, "Scheme-Upload", Convert.ToByte(mails.SendNow));
            }

            Listtestmail.Add(new SchemeUploads
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