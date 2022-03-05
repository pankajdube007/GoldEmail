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

public class POApprovalController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/POApproval")]
    public HttpResponseMessage GetTestMail(POApproval mailsms)
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

            List<POApproval> testmail = new List<POApproval>();
            List<POApprovalDetails> Listtestmail = new List<POApprovalDetails>();
            //string Attach = string.Empty;

            //if (mailsms.Attachment != "")
            //{
            //    Attach = ProperAttachmentUrl(mailsms.Attachment, mailsms.ID, mailsms.UserID);
            //}

            if (mailsms.For == "Email")
            {
                DataTable dt = da.GetEmailTemplateDatabyID(mailsms.ID);
                if (ms.ValidateEmailId(mailsms.TO) == true)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Subject = HttpUtility.HtmlDecode(ms.ReplaceTemplatePOApproval(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), mailsms));

                        Body = ms.ReplaceTemplatePOApproval(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), mailsms);

                        Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                        da.AddEmailQueue(0, mailsms.TO, mailsms.TOName, "", "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, mailsms.Attachment, "PO-Approval-Dealer", 1);
                    }
                }

                if (ms.ValidateEmailId(mailsms.SalesExContact) == true)
                {
                    DataTable dt1 = da.GetEmailTemplateDatabyID(3026);

                    if (dt1.Rows.Count > 0)
                    {
                        string Subject1;
                        string Body1 = string.Empty;

                        Subject1 = HttpUtility.HtmlDecode(ms.ReplaceTemplatePOApproval(dt1.Rows[0]["Subject"].ToString(), dt1.Rows[0]["Subject"].ToString(), mailsms));

                        Body1 = ms.ReplaceTemplatePOApproval(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mailsms);

                        Body1 = Body1 + "&nbsp;&nbsp;" + dt1.Rows[0]["Signature"].ToString();

                        da.AddEmailQueue(0, mailsms.SalesExContact, mailsms.SalesExName, "", "", Subject1, Body1, dt1.Rows[0]["EmailTime"].ToString(), dt1.Rows[0]["EmailLastDate"].ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, mailsms.Attachment, "PO-Approval-Excutive", 1);
                    }
                }
            }
            else
                if (mailsms.For == "SMS")
            {
                DataTable dt1 = da.GetMeassageById(mailsms.ID);
                if (dt1.Rows.Count > 0)
                {
                    Body = ms.ReplaceTemplatePOApproval(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mailsms).Replace("&", "and");
                    da.AddMessageQueue(0, mailsms.TO, mailsms.TOName, Body, Convert.ToDateTime(dt1.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt1.Rows[0]["LastMessageDate"]).ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, "PO-Approval-Dealer", 1);
                }

                if (mailsms.SalesExContact != "")
                {
                    DataTable dt2 = da.GetMeassageById(3014);

                    if (dt2.Rows.Count > 0)
                    {
                        string Body1 = string.Empty;

                        Body1 = ms.ReplaceTemplatePOApproval(dt2.Rows[0]["Body"].ToString(), dt2.Rows[0]["Body"].ToString(), mailsms).Replace("&", "and");
                        da.AddMessageQueue(0, mailsms.SalesExContact, mailsms.SalesExName, Body1, Convert.ToDateTime(dt2.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt2.Rows[0]["LastMessageDate"]).ToString(), mailsms.UserID, mailsms.LogNO, 3014, "PO-Approval-Excutive", 1);
                    }
                }
            }
            Listtestmail.Add(new POApprovalDetails
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