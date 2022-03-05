using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;

public class TestSmsController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/TestSMS")]
    public HttpResponseMessage GetTestMail(SchemeUpload sms)
    {
        Common cm = new Common();
        MessageService ms = new MessageService();
        WebClient web = new WebClient();
        byte[] bufData = null;

        try
        {
            string data1;
            string Subject;
            string Body = "";

            EmailTemplateDA da = new EmailTemplateDA();
            PrivateMessage pm = new PrivateMessage();

            DataTable dt = da.GetMeassageById(sms.ID);
            //ListCI.ToEmail = "pankaj@gmail.com";
            //ListCI.ToName = "Pankaj Dube";

            List<TestSmss> TestEms = new List<TestSmss>();

            Body = ms.ReplaceEmailTemplateSchemeUpload(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), sms).Replace("&", "and");

            if (dt.Rows.Count > 0)
            {
                bufData = web.DownloadData("http://sms6.routesms.com:8080/bulksms/bulksms?username=" + WebConfigurationManager.AppSettings["SmsUser"].ToString() + "&password=" + WebConfigurationManager.AppSettings["SmsPassword"].ToString() + "&type=0&dlr=0&destination=" + sms.To + "&source=GLDMDL&message=" + Body);
            }

            TestEms.Add(new TestSmss
            {
                result = "True",
                message = "Test Mail Send",
                servertime = DateTime.Now.ToString(),
                // data = testmail,
            });

            data1 = JsonConvert.SerializeObject(TestEms, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
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
            //    AttachmentUrl = AttachmentUrl + "Upload\\ErpAttachment\\" + MailId + "\\" + UserId + "\\" + str1 + ",";
            AttachmentUrl = AttachmentUrl + MailId + "\\" + UserId + "\\" + str1 + ",";
        }
        return AttachmentUrl;
    }
}