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

public class CreateInvoiceController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/CreateInvoice")]
    public HttpResponseMessage GetCreateInvoice(CreateInvoice mailsms)
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

            List<CreateInvoice> testmail = new List<CreateInvoice>();
            List<CreateInvoiceDetails> Listtestmail = new List<CreateInvoiceDetails>();
            //string Attach = string.Empty;

            //if (mailsms.Attachment != "")
            //{
            //    Attach = ProperAttachmentUrl(mailsms.Attachment, mailsms.ID, mailsms.UserID);
            //}

            if (mailsms.For == "Email")
            {
                DataTable dt = da.GetEmailTemplateDatabyID(mailsms.ID);

                if (dt.Rows.Count > 0)
                {
                    Subject = HttpUtility.HtmlDecode(ms.ReplaceEmailTemplateCreateInvoice(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), mailsms));

                    Body = ms.ReplaceEmailTemplateCreateInvoice(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), mailsms);

                    Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                    da.AddEmailQueue(0, mailsms.TO, mailsms.TOName, "", "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, mailsms.Attachment, "Create-Invoice", 1);
                }
                if (mailsms.ExContact != "")
                {
                    DataTable dt1 = da.GetEmailTemplateDatabyID(3030);

                    if (dt1.Rows.Count > 0)
                    {
                        Subject = HttpUtility.HtmlDecode(ms.ReplaceEmailTemplateCreateInvoice(dt1.Rows[0]["Subject"].ToString(), dt1.Rows[0]["Subject"].ToString(), mailsms));

                        Body = ms.ReplaceEmailTemplateCreateInvoice(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mailsms);

                        Body = Body + "&nbsp;&nbsp;" + dt1.Rows[0]["Signature"].ToString();

                        da.AddEmailQueue(0, mailsms.ExContact, mailsms.SalesExName, "", "", Subject, Body, dt1.Rows[0]["EmailTime"].ToString(), dt1.Rows[0]["EmailLastDate"].ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, mailsms.Attachment, "Create-Invoice", 1);
                    }
                }

                if (mailsms.AgentContact != "")
                {
                    DataTable dt1 = da.GetEmailTemplateDatabyID(3030);

                    if (dt1.Rows.Count > 0)
                    {
                        Subject = HttpUtility.HtmlDecode(ms.ReplaceEmailTemplateCreateInvoice(dt1.Rows[0]["Subject"].ToString(), dt1.Rows[0]["Subject"].ToString(), mailsms));

                        Body = ms.ReplaceEmailTemplateCreateInvoice(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mailsms);

                        Body = Body + "&nbsp;&nbsp;" + dt1.Rows[0]["Signature"].ToString();

                        da.AddEmailQueue(0, mailsms.AgentContact, mailsms.SalesExName, "", "", Subject, Body, dt1.Rows[0]["EmailTime"].ToString(), dt1.Rows[0]["EmailLastDate"].ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, mailsms.Attachment, "Create-Invoice", 1);
                    }
                }
            }
            else
                if (mailsms.For == "SMS")
            {
                DataTable dt1 = da.GetMeassageById(mailsms.ID);
                if (dt1.Rows.Count > 0)
                {
                    Body = ms.ReplaceEmailTemplateCreateInvoice(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mailsms).Replace("&", "and");
                    da.AddMessageQueue(0, mailsms.TO, mailsms.TOName, Body, Convert.ToDateTime(dt1.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt1.Rows[0]["LastMessageDate"]).ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, "Create-Invoice-Dealer", 1);
                    if (mailsms.ExContact != "")
                    {
                        da.AddMessageQueue(0, mailsms.ExContact, mailsms.SalesExName, Body, Convert.ToDateTime(dt1.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt1.Rows[0]["LastMessageDate"]).ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, "Create-Invoice-Excutive", 1);
                    }

                    if (mailsms.AgentContact != "")
                    {
                        da.AddMessageQueue(0, mailsms.AgentContact, "Agent", Body, Convert.ToDateTime(dt1.Rows[0]["MessageTime"]).ToString(), Convert.ToDateTime(dt1.Rows[0]["LastMessageDate"]).ToString(), mailsms.UserID, mailsms.LogNO, mailsms.ID, "Create-Invoice-Agent", 1);
                    }
                }
            }
            Listtestmail.Add(new CreateInvoiceDetails
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