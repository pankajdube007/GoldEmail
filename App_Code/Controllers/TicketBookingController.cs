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

public class TicketBookingController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/TicketBooking")]
    public HttpResponseMessage GetDetails(TicketBooking mails)
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

            List<DetailsTemplateTicketBooking> Detailmail = new List<DetailsTemplateTicketBooking>();
            List<TicketBookingDetails> Listtestmail = new List<TicketBookingDetails>();
            string Attach = string.Empty;

            if (mails.For == "Email")
            {
                if (dt.Rows.Count > 0)
                {
                    Subject = HttpUtility.HtmlDecode(ms.ReplaceTemplateTicketBooking(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), mails));

                    Body = ms.ReplaceTemplateTicketBooking(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), mails);

                    Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                    Detailmail.Add(new DetailsTemplateTicketBooking
                    {
                        BodyTEMP = Body,
                        SubjectTEMP = Subject
                    }

                        );

                    da.AddEmailQueue(0, mails.To, mails.ToName, "", "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), mails.UserID, mails.LogNO, mails.ID, Attach, "Ticket Booking", mails.SendNow);
                }
            }
            else
                if (mails.For == "SMS")
            {
                if (dt1.Rows.Count > 0)
                {
                    Body = ms.ReplaceTemplateTicketBooking(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mails).Replace("&", "and");

                    Detailmail.Add(new DetailsTemplateTicketBooking
                    {
                        BodyTEMP = Body,
                        SubjectTEMP = ""
                    }

                       );

                    da.AddMessageQueue(0, mails.To, mails.ToName, Body, dt1.Rows[0]["MessageTime"].ToString(), dt1.Rows[0]["LastMessageDate"].ToString(), mails.UserID, mails.LogNO, mails.ID, "Ticket Booking", mails.SendNow);
                }
            }
            Listtestmail.Add(new TicketBookingDetails
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