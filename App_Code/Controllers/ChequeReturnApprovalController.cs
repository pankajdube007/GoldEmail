using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

public class ChequeReturnApprovalController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/ChequeReturnApproval")]
    public HttpResponseMessage GetTestMail(ChequeReturnApproval mails)
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

            //  DataTable dt = da.GetEmailTemplateDatabyID(mails.ID);

            List<ChequeReturnApproval> testmail = new List<ChequeReturnApproval>();
            List<ChequeReturnApprovalDetails> Listtestmail = new List<ChequeReturnApprovalDetails>();
            string Attach = string.Empty;

            if (mails.For == "Email")
            {
            }
            else
                if (mails.For == "SMS")
            {
                if (ms.ValidateMobileNo(mails.SalesExContact) == true)
                {
                    DataTable dt1 = da.GetMeassageById(2005);
                    if (dt1.Rows.Count > 0)
                    {
                        Body = ms.ReplaceTemplateChequeReturnApproval(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mails).Replace("&", "and");
                        da.AddMessageQueue(0, mails.SalesExContact, "Excutive", Body, Convert.ToDateTime(dt1.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt1.Rows[0]["LastMessageDate"]).ToString(), mails.UserID, mails.LogNO, 2005, "Cheque-Return-Approval-Excutive", 1);
                    }
                }
                if (ms.ValidateMobileNo(mails.TO) == true)
                {
                    DataTable dt2 = da.GetMeassageById(2006);
                    if (dt2.Rows.Count > 0)
                    {
                        Body = ms.ReplaceTemplateChequeReturnApproval(dt2.Rows[0]["Body"].ToString(), dt2.Rows[0]["Body"].ToString(), mails).Replace("&", "and");
                        da.AddMessageQueue(0, mails.TO, mails.PartyName, Body, Convert.ToDateTime(dt2.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt2.Rows[0]["LastMessageDate"]).ToString(), mails.UserID, mails.LogNO, 2006, "Cheque-Return-Approval-Party", 1);
                    }
                }
                if (ms.ValidateMobileNo(mails.AgentContact) == true)
                {
                    DataTable dt3 = da.GetMeassageById(2007);
                    if (dt3.Rows.Count > 0)
                    {
                        Body = ms.ReplaceTemplateChequeReturnApproval(dt3.Rows[0]["Body"].ToString(), dt3.Rows[0]["Body"].ToString(), mails).Replace("&", "and");
                        da.AddMessageQueue(0, mails.AgentContact, "Agent", Body, Convert.ToDateTime(dt3.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt3.Rows[0]["LastMessageDate"]).ToString(), mails.UserID, mails.LogNO, 2007, "Cheque-Return-Approval-Agent", 1);
                    }
                }
            }
            Listtestmail.Add(new ChequeReturnApprovalDetails
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