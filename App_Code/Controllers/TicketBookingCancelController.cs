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

public class TicketBookingCancelController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/TicketBookingCancel")]
    public HttpResponseMessage GetDetails(TicketBookingCancel mails)
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

            List<DetailsTemplateTicketBookingCancel> Detailmail = new List<DetailsTemplateTicketBookingCancel>();
            List<TicketBookingCancelDetails> Listtestmail = new List<TicketBookingCancelDetails>();
            string Attach = string.Empty;

            if (mails.For == "Email")
            {
                if (dt.Rows.Count > 0)
                {
                    Subject = HttpUtility.HtmlDecode(ms.ReplaceTemplateTicketBookingCancel(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), mails));

                    Body = ms.ReplaceTemplateTicketBookingCancel(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), mails);

                    Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                    Detailmail.Add(new DetailsTemplateTicketBookingCancel
                    {
                        BodyTEMP = Body,
                        SubjectTEMP = Subject
                    }

                      );

                    da.AddEmailQueue(0, mails.To, mails.ToName, "", "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), mails.UserID, mails.LogNO, mails.ID, Attach, "Ticket Cancel", mails.SendNow);
                }
            }
            else
                if (mails.For == "SMS")
            {
                if (dt1.Rows.Count > 0)
                {
                    Body = ms.ReplaceTemplateTicketBookingCancel(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mails).Replace("&", "and");

                    Detailmail.Add(new DetailsTemplateTicketBookingCancel
                    {
                        BodyTEMP = Body,
                        SubjectTEMP = ""
                    }

                    );

                    da.AddMessageQueue(0, mails.To, mails.ToName, Body, dt1.Rows[0]["MessageTime"].ToString(), dt1.Rows[0]["LastMessageDate"].ToString(), mails.UserID, mails.LogNO, mails.ID, "Ticket Cancel", mails.SendNow);
                }
            }
            Listtestmail.Add(new TicketBookingCancelDetails
            {
                result = "True",
                message = "",
                servertime = DateTime.Now.ToString(),
                data = Detailmail,
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
}