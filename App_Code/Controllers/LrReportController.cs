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

public class LrReportController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/LrReport")]
    public HttpResponseMessage GetCreateInvoice(LrReport mailsms)
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

            //  DataTable dt = da.GetEmailTemplateDatabyID(mailsms.ID);

            List<LrReport> testmail = new List<LrReport>();
            List<LrReportDetails> Listtestmail = new List<LrReportDetails>();
            string Attach = string.Empty;

            //if (mailsms.Attachment != "")
            //{
            //    Attach = ProperAttachmentUrl(mailsms.Attachment, mailsms.ID, mailsms.UserID);
            //}

            if (mailsms.For == "Email")
            {
                if (ms.ValidateEmailId(mailsms.TO) == true)
                {
                    DataTable dt = da.GetEmailTemplateDatabyID(mailsms.ID);

                    if (dt.Rows.Count > 0)
                    {
                        Subject = HttpUtility.HtmlDecode(ms.ReplaceEmailTemplateLrReport(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), mailsms));

                        Body = ms.ReplaceEmailTemplateLrReport(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), mailsms);

                        Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                        da.AddEmailQueue(0, mailsms.TO, mailsms.TOName, "", "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, Attach, "LR-Report-Dealer", 1);
                    }
                }

                if (ms.ValidateEmailId(mailsms.ExContact) == true)
                {
                    DataTable dt = da.GetEmailTemplateDatabyID(mailsms.ExID);

                    if (dt.Rows.Count > 0)
                    {
                        string Subject1 = string.Empty;
                        Subject1 = HttpUtility.HtmlDecode(ms.ReplaceEmailTemplateLrReport(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), mailsms));

                        string Body1 = ms.ReplaceEmailTemplateLrReport(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), mailsms);

                        Body1 = Body1 + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                        da.AddEmailQueue(0, mailsms.ExContact, mailsms.SalesExName, "", "", Subject1, Body1, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, Attach, "LR-Report-Excutive", 1);
                    }
                }
            }
            else
                if (mailsms.For == "SMS")
            {
                if (ms.ValidateMobileNo(mailsms.TO) == true)
                {
                    DataTable dt1 = da.GetMeassageById(mailsms.ID);
                    if (dt1.Rows.Count > 0)
                    {
                        Body = ms.ReplaceEmailTemplateLrReport(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mailsms).Replace("&", "and");
                        da.AddMessageQueue(0, mailsms.TO, mailsms.TOName, Body, Convert.ToDateTime(dt1.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt1.Rows[0]["LastMessageDate"]).ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, "LR-Report-Dealer", 1);
                    }
                }
                if (ms.ValidateMobileNo(mailsms.ExContact) == true)
                {
                    DataTable dt2 = da.GetMeassageById(mailsms.ExID);
                    string Body1 = ms.ReplaceEmailTemplateLrReport(dt2.Rows[0]["Body"].ToString(), dt2.Rows[0]["Body"].ToString(), mailsms).Replace("&", "and");
                    if (dt2.Rows.Count > 0)
                    {
                        da.AddMessageQueue(0, mailsms.ExContact, mailsms.TOName, Body1, Convert.ToDateTime(dt2.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt2.Rows[0]["LastMessageDate"]).ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ExID, "LR-Report-Excutive", 1);
                    }
                }
            }
            Listtestmail.Add(new LrReportDetails
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