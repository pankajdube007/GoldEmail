﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

public class PartyWiseOutController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/PartyWiseOutstanding")]
    public HttpResponseMessage GetTestMail(PartywiseOutstanding mails)
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

            List<PartywiseOutstanding> testmail = new List<PartywiseOutstanding>();
            List<PartywiseOutstandingDetails> Listtestmail = new List<PartywiseOutstandingDetails>();
            string Attach = string.Empty;
            if (mails.Attachment != "")
            {
                Attach = ProperAttachmentUrl(mails.Attachment, mails.ID, mails.UserID);
            }

            if (mails.For == "Email")
            {
                if (dt.Rows.Count > 0)
                {
                    Subject = HttpUtility.HtmlDecode(ms.ReplaceEmailTemplatePartyWiseOutstanding(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), mails));

                    Body = ms.ReplaceEmailTemplatePartyWiseOutstanding(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), mails);

                    Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                    da.AddEmailQueue(0, mails.To, mails.ToName, mails.CC, "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), mails.UserID, mails.LogNO, mails.ID, Attach, "PartyWise-Outstanding", mails.SendNow);
                }
            }
            else
                if (mails.For == "SMS")
            {
                if (dt1.Rows.Count > 0)
                {
                    Body = ms.ReplaceEmailTemplatePartyWiseOutstanding(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mails).Replace("&", "and");
                    da.AddMessageQueue(0, mails.To, mails.ToName, Body, dt1.Rows[0]["MessageTime"].ToString(), dt1.Rows[0]["LastMessageDate"].ToString(), mails.UserID, mails.LogNO, mails.ID, "PartyWise-Outstanding", mails.SendNow);
                }
            }
            Listtestmail.Add(new PartywiseOutstandingDetails
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
            //  AttachmentUrl = AttachmentUrl + "Upload\\ErpAttachment\\" + MailId + "\\" + UserId + "\\" + str1 + ",";

            AttachmentUrl = AttachmentUrl + MailId + "\\" + UserId + "\\" + str1 + ",";
        }
        return AttachmentUrl;
    }
}