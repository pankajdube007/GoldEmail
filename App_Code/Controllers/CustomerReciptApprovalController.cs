using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

public class CustomerReciptApprovalController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/CustomerRecieptApproval")]
    public HttpResponseMessage GetTestMail(CustomerRecieptApproval mailsms)
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

            List<CustomerRecieptApproval> testmail = new List<CustomerRecieptApproval>();
            List<CustomerRecieptApprovalDetails> Listtestmail = new List<CustomerRecieptApprovalDetails>();
            string Attach = string.Empty;

            if (mailsms.For == "Email")
            {
            }
            else
                if (mailsms.For == "SMS")
            {
                if (mailsms.TO != "")
                {
                    DataTable dt1 = da.GetMeassageById(3009);
                    if (dt1.Rows.Count > 0)
                    {
                        Body = ms.ReplaceTemplateCustomerRecieptApproval(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mailsms).Replace("&", "and");
                        da.AddMessageQueue(0, mailsms.TO, mailsms.PartyName, Body, Convert.ToDateTime(dt1.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt1.Rows[0]["LastMessageDate"]).ToString(), mailsms.UserID, mailsms.LogNO, 3009, "Customer-Reciept-Approval", 1);

                        if (mailsms.BranchId == "1")
                        {
                            da.AddMessageQueue(0, "9830173819", mailsms.PartyName, Body, Convert.ToDateTime(dt1.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt1.Rows[0]["LastMessageDate"]).ToString(), mailsms.UserID, mailsms.LogNO, 3009, "Customer-Reciept-Approval", 1);
                        }

                        if (mailsms.BranchId == "18")
                        {
                            da.AddMessageQueue(0, "9980997799", mailsms.PartyName, Body, Convert.ToDateTime(dt1.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt1.Rows[0]["LastMessageDate"]).ToString(), mailsms.UserID, mailsms.LogNO, 3009, "Customer-Reciept-Approval", 1);
                        }
                    }
                }

                if (mailsms.Mobile != "")
                {
                    if (mailsms.instrumenttype != "Cheque")
                    {
                        DataTable dt1 = da.GetMeassageById(3011);
                        if (dt1.Rows.Count > 0)
                        {
                            Body = ms.ReplaceTemplateCustomerRecieptApproval(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mailsms).Replace("&", "and");
                            da.AddMessageQueue(0, mailsms.Mobile, mailsms.PartyName, Body, Convert.ToDateTime(dt1.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt1.Rows[0]["LastMessageDate"]).ToString(), mailsms.UserID, mailsms.LogNO, 3011, "Customer-Reciept-Approval", 1);
                        }
                    }
                    else
                    {
                        DataTable dt1 = da.GetMeassageById(3010);
                        if (dt1.Rows.Count > 0)
                        {
                            Body = ms.ReplaceTemplateCustomerRecieptApproval(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mailsms).Replace("&", "and");
                            da.AddMessageQueue(0, mailsms.Mobile, mailsms.PartyName, Body, Convert.ToDateTime(dt1.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt1.Rows[0]["LastMessageDate"]).ToString(), mailsms.UserID, mailsms.LogNO, 3010, "Customer-Reciept-Approval", 1);
                        }
                    }
                }
            }
            Listtestmail.Add(new CustomerRecieptApprovalDetails
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