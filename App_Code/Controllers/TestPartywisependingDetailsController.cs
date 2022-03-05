using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Http;

public class TestPartywisependingDetailsController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/TestPartywisependingDetails")]
    public HttpResponseMessage GetTestMail(PendingDetails mails)
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

            DataTable dt = da.GetEmailTemplateDatabyID(mails.ID);
            DataTable dt1 = da.GetMeassageById(mails.ID);

            List<PendingDetails> testmail = new List<PendingDetails>();
            List<PendingDetailsDetails> Listtestmail = new List<PendingDetailsDetails>();
            //   string Attach = string.Empty;
            //if (mails.Attachment != "")
            //{
            //    Attach = ProperAttachmentUrl(mails.Attachment, mails.ID, mails.UserID);
            //}

            if (mails.For == "Email")
            {
                if (dt.Rows.Count > 0)
                {
                    Subject = HttpUtility.HtmlDecode(ms.ReplaceTemplatePartyWisePendingDetails(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), mails));

                    Body = ms.ReplaceTemplatePartyWisePendingDetails(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), mails);

                    Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                    if (dt.Rows.Count > 0)
                    {
                        List<string> ls = new List<string>();
                        EmailAccount ea = new EmailAccount();

                        DataTable dt2 = ms.GetEmailAccountForTestMail();

                        if (dt2.Rows.Count > 0)
                        {
                            ea.EmailAccountId = Convert.ToInt32(dt2.Rows[0]["EmailAccountId"]);
                            ea.Email = Convert.ToString(dt2.Rows[0]["Email"]);
                            ea.DisplayName = Convert.ToString(dt2.Rows[0]["DisplayName"]);
                            ea.Host = Convert.ToString(dt2.Rows[0]["Host"]);
                            ea.Port = Convert.ToInt32(dt2.Rows[0]["Port"]);
                            ea.Username = Convert.ToString(dt2.Rows[0]["Username"]);
                            ea.Password = Convert.ToString(dt2.Rows[0]["Password"]);
                            ea.EnableSSL = Convert.ToBoolean(dt2.Rows[0]["EnableSSL"]);
                            ea.UseDefaultCredentials = Convert.ToBoolean(dt2.Rows[0]["UseDefaultCredentials"]);
                            ea.Attchment = mails.Attachment;
                        }

                        ms.SendEmail(Subject, Body,
                               new MailAddress(ea.Email, ea.DisplayName),
                               new MailAddress(mails.To, mails.ToName), ls, ls, ea);
                    }
                }
            }
            else
                if (mails.For == "SMS")
            {
                if (dt1.Rows.Count > 0)
                {
                    Body = ms.ReplaceTemplatePartyWisePendingDetails(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mails).Replace("&", "and");
                    da.AddMessageQueue(0, mails.To, mails.ToName, Body, dt1.Rows[0]["MessageTime"].ToString(), dt1.Rows[0]["LastMessageDate"].ToString(), mails.UserID, mails.LogNO, mails.ID, "PartyWise-Ledger", mails.SendNow);
                }
            }
            Listtestmail.Add(new PendingDetailsDetails
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