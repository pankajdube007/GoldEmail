using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for AutoPartyWiseCDInvoice
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoPartyWiseCDInvoice : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da;
        private MessageService ms = new MessageService();

        private SqlDataReader rdr = null;

        public AutoPartyWiseCDInvoice()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
        }

        public void Execute(XmlNode node)
        {
            int TemplateId = 3033;

            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }
            //   DealderMail(DealerTemplateId);
            PartyEMail(TemplateId);
        }

        protected void PartyEMail(int TemplateId)
        {
            DataTable dttemplate = da.GetEmailForTimeCheckbyID(TemplateId);
            if (dttemplate.Rows.Count > 0)
            {
                DateTime lastsend = DateTime.Now.Date;
                int flag = 0;

                DataTable dtActiveTime = da.GetActiveSchemeTimeID(TemplateId);
                if (dtActiveTime.Rows.Count > 0)
                {
                    lastsend = Convert.ToDateTime(dtActiveTime.Rows[0]["lastSend"]);
                    flag = 0;
                }
                else
                {
                    lastsend = DateTime.Now.Date;
                    flag = 1;
                }
                int days = (DateTime.Now.Date - lastsend).Days;
                int interval = Convert.ToInt32(dttemplate.Rows[0]["interval"]);

                if (days >= interval || flag == 1)
                {
                    DataTable dt = _messageService.return_dt("exec GetAllPartyCollectionEmail");

                    if (dt.Rows.Count > 0)
                    {
                        PartyMail(dt, TemplateId, dttemplate);
                    }
                }
            }
        }

        protected void PartyMail(DataTable dt, int DealerTemplateId, DataTable dttemplate)
        {
            string AttachmentName = string.Empty;
            string FileName = string.Empty;
            string cc = string.Empty;
            da.AddActiveScheme(DealerTemplateId);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dtCDDetails = _messageService.return_dt("exec PartyWisecddateinvoicewise " + dt.Rows[i]["typecat"].ToString() + ",'04/01/2015','" + DateTime.Now.Date + "',''," + dt.Rows[i]["partyid"].ToString());
                if (dtCDDetails.Rows.Count > 0)
                {
                    if (_messageService.ValidateEmailId(dt.Rows[i]["emailid"].ToString()) == true)
                    {
                        DataTable dtcheckhistory = da.GetQueuedEmailHistoryforExandag(dt.Rows[i]["emailid"].ToString(), DealerTemplateId);
                        if (dtcheckhistory.Rows.Count <= 0)
                        {
                            DataTable dtexcutive = new DataTable();
                            dtexcutive = _messageService.return_dt("exec salesexselectByID " + dt.Rows[i]["salesexid"].ToString());
                            if (dtexcutive.Rows.Count > 0)
                            {
                                if (_messageService.ValidateEmailId(dtexcutive.Rows[0]["email"].ToString()) == true)
                                {
                                    cc = dtexcutive.Rows[0]["email"].ToString();
                                }
                            }

                            PartyWiseCDInvoice MailSMS = FillCDDetails(dt, dtCDDetails, i, cc);

                            SendMail(DealerTemplateId, dt.Rows[i]["emailid"].ToString(), dt.Rows[i]["partyname"].ToString(), MailSMS);
                        }
                    }
                }
            }
        }

        private PartyWiseCDInvoice FillCDDetails(DataTable dtparty, DataTable dtCDDetails, int i, string cc)
        {
            string CDDetails = string.Empty;
            dtCDDetails = ProperData(dtCDDetails);
            CDDetails = Convertintohtml(dtCDDetails, dtparty.Rows[i]["cin"].ToString());

            PartyWiseCDInvoice MailSMS = new PartyWiseCDInvoice();
            MailSMS.ID = 3033;
            MailSMS.To = dtparty.Rows[i]["emailid"].ToString();
            MailSMS.ToName = dtparty.Rows[i]["PartyName"].ToString();
            MailSMS.PartyCode = Convert.ToInt32(dtparty.Rows[i]["cin"].ToString());
            MailSMS.SaleExName = dtparty.Rows[i]["salesexname"].ToString();
            MailSMS.cin = dtparty.Rows[i]["cin"].ToString();
            MailSMS.PartyType = dtparty.Rows[i]["PartyType"].ToString();
            MailSMS.HomeBranch = dtparty.Rows[i]["HomeBranch"].ToString();
            MailSMS.City = dtparty.Rows[i]["city"].ToString();
            MailSMS.State = dtparty.Rows[i]["statenm"].ToString();
            MailSMS.Area = dtparty.Rows[i]["areanm"].ToString();
            MailSMS.Mobile = dtparty.Rows[i]["Mobile"].ToString();
            MailSMS.Email = dtparty.Rows[i]["emailid"].ToString();
            MailSMS.PartyName = dtparty.Rows[i]["PartyName"].ToString();
            MailSMS.Email = dtparty.Rows[i]["emailid"].ToString();
            MailSMS.VatNo = dtparty.Rows[i]["vat"].ToString();
            MailSMS.CstNo = dtparty.Rows[i]["cst"].ToString();
            MailSMS.Dealer = dtparty.Rows[i]["dealer"].ToString();
            MailSMS.FullAdd = dtparty.Rows[i]["fulladdress"].ToString();
            MailSMS.CDDetails = CDDetails;
            MailSMS.UserID = 9999;
            MailSMS.LogNO = 9999;
            MailSMS.Attachment = "";
            MailSMS.ExtraRemarks = "";
            MailSMS.CC = cc;
            MailSMS.Date = DateTime.Now.Date.ToString();
            MailSMS.SendNow = 1;
            MailSMS.For = "Email";

            return MailSMS;
        }

        protected DataTable ProperData(DataTable dt)
        {
            if (dt.Columns.Contains("invoicetype") == true)
            {
                dt.Columns.Remove("invoicetype");
            }
            if (dt.Columns.Contains("invoicecat") == true)
            {
                dt.Columns.Remove("invoicecat");
            }
            if (dt.Columns.Contains("invoicedivision") == true)
            {
                dt.Columns.Remove("invoicedivision");
            }

            if (dt.Columns.Contains("invoicedate") == true)
            {
                dt.Columns.Remove("invoicedate");
            }
            if (dt.Columns.Contains("PartyName") == true)
            {
                dt.Columns.Remove("PartyName");
            }
            if (dt.Columns.Contains("PCategory") == true)
            {
                dt.Columns.Remove("PCategory");
            }
            if (dt.Columns.Contains("locnm") == true)
            {
                dt.Columns.Remove("locnm");
            }
            if (dt.Columns.Contains("branchid") == true)
            {
                dt.Columns.Remove("branchid");
            }
            if (dt.Columns.Contains("fromdate") == true)
            {
                dt.Columns.Remove("fromdate");
            }
            if (dt.Columns.Contains("todate") == true)
            {
                dt.Columns.Remove("todate");
            }
            if (dt.Columns.Contains("cin") == true)
            {
                dt.Columns.Remove("cin");
            }
            if (dt.Columns.Contains("partyid") == true)
            {
                dt.Columns.Remove("partyid");
            }
            if (dt.Columns.Contains("typeofparty") == true)
            {
                dt.Columns.Remove("typeofparty");
            }
            if (dt.Columns.Contains("city") == true)
            {
                dt.Columns.Remove("city");
            }

            if (dt.Columns.Contains("todays") == true)
            {
                dt.Columns.Remove("todays");
            }

            if (dt.Columns.Contains("MasterDivisionnm") == true)
            {
                dt.Columns.Remove("MasterDivisionnm");
            }

            dt.Columns["Divisionnm"].ColumnName = "Division";
            dt.Columns["invoicedate1"].ColumnName = "Invoice Date";
            dt.Columns["invoiceno"].ColumnName = "Invoice No";
            dt.Columns["finalamount"].ColumnName = "Net Amount";
            dt.Columns["outstandingamt"].ColumnName = "Outstanding Amount";
            dt.Columns["duedays"].ColumnName = "Due Days";

            dt.DefaultView.Sort = "Division ASC";
            dt = dt.DefaultView.ToTable();

            DataTable dtclone = dt.Clone();

            if (dtclone.Columns.Contains("date1") == true)
            {
                dtclone.Columns.Remove("date1");
            }
            if (dtclone.Columns.Contains("percentage") == true)
            {
                dtclone.Columns.Remove("percentage");
            }
            if (dtclone.Columns.Contains("slno") == true)
            {
                dtclone.Columns.Remove("slno");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                {
                    if (dt.Rows[i]["slno"].ToString() != dt.Rows[i - 1]["slno"].ToString())
                    {
                        if (dtclone.Columns.Contains(Convert.ToString(dt.Rows[i]["percentage"]) + "%"))
                        {
                        }
                        else
                        {
                            DataColumn Col = dtclone.Columns.Add(Convert.ToString(dt.Rows[i]["percentage"]) + "%", typeof(string));
                        }
                        dtclone.Rows.Add(dt.Rows[i]["Invoice Date"], dt.Rows[i]["Invoice No"], Convert.ToDouble(dt.Rows[i]["Net Amount"]),
                                         Convert.ToInt32(dt.Rows[i]["Due Days"]), dt.Rows[i]["Division"], Convert.ToDouble(dt.Rows[i]["Outstanding Amount"].ToString()));

                        var rowsToUdate = dtclone.AsEnumerable().Where(r => r.Field<double>("Outstanding Amount") == Convert.ToDouble(dt.Rows[i]["Outstanding Amount"]));
                        foreach (DataRow row in rowsToUdate)
                            row.SetField(dt.Rows[i]["percentage"].ToString() + "%", dt.Rows[i]["date1"].ToString());
                    }
                    else
                    {
                        if (dtclone.Columns.Contains(Convert.ToString(dt.Rows[i]["percentage"]) + "%"))
                        {
                        }
                        else
                        {
                            DataColumn Col = dtclone.Columns.Add(Convert.ToString(dt.Rows[i]["percentage"]) + "%", typeof(string));
                        }
                        var rowsToUdate = dtclone.AsEnumerable().Where(r => r.Field<double>("Outstanding Amount") == Convert.ToDouble(dt.Rows[i]["Outstanding Amount"]));
                        foreach (DataRow row in rowsToUdate)
                            row.SetField(dt.Rows[i]["percentage"].ToString() + "%", dt.Rows[i]["date1"].ToString());
                    }
                }
                else
                {
                    dtclone.Columns.Add(Convert.ToString(dt.Rows[i]["percentage"]) + "%", typeof(string));
                    dtclone.Rows.Add(dt.Rows[i]["Invoice Date"], dt.Rows[i]["Invoice No"], Convert.ToDouble(dt.Rows[i]["Net Amount"]),
                                         Convert.ToInt32(dt.Rows[i]["Due Days"]), dt.Rows[i]["Division"], Convert.ToDouble(dt.Rows[i]["Outstanding Amount"].ToString())
                                         , dt.Rows[i]["date1"]);
                }
            }

            return dtclone;
        }

        public string Convertintohtml(DataTable dt, string PartyName)
        {
            dt.Columns["Division"].SetOrdinal(0);
            dt.Columns["Invoice No"].SetOrdinal(1);
            dt.Columns["Invoice Date"].SetOrdinal(2);
            dt.Columns["Net Amount"].SetOrdinal(3);
            dt.Columns["Outstanding Amount"].SetOrdinal(4);
            dt.Columns["Due Days"].SetOrdinal(5);

            dt.DefaultView.Sort = "Due Days DESC";
            int noofcolumn = dt.Columns.Count;

            string output = string.Empty;

            StringBuilder html = new StringBuilder();
            html.Append("<table style='border-spacing: 1;border-bottom: 1pt solid windowtext;'>");
            html.Append("<tr>");
            html.Append("<th style='background-color: red;' colspan='1'>");
            html.Append(PartyName);
            html.Append("<th style='background-color: red;' colspan='5'>");
            html.Append("UNIQUE DELIGHT");
            html.Append("<th style='background-color: red;' colspan='" + (noofcolumn - 6) + "'>");
            html.Append("Due Date to avil CD");
            html.Append("</th>");
            html.Append("</tr>");
            html.Append("<tr>");

            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th style='background-color: #BFBFBF;'>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td style=' border-top:1pt solid windowtext;border-right: 1pt solid black;'>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }
            html.Append("</tr>");

            html.Append("</table>");

            output = html.ToString();

            return output;
        }

        private void SendMail(int TemplateId, string Email, string Name, PartyWiseCDInvoice MailSMS)
        {
            string Attach = string.Empty;
            string Subject = string.Empty;
            string Body = string.Empty;

            DataTable dt = new DataTable();
            dt = da.GetEmailTemplateDatabyID(TemplateId);

            if (dt.Rows.Count > 0)
            {
                Subject = HttpUtility.HtmlDecode(ms.ReplaceEmailTemplatePartyWiseCDInvoice(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), MailSMS));

                Body = ms.ReplaceEmailTemplatePartyWiseCDInvoice(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), MailSMS);

                Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                da.AddEmailQueue(0, Email, Name, MailSMS.CC, "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), 0, 0, TemplateId, Attach, "Auto-CD-Invoice", 1);
            }
        }

        public string ProperAttachmentUrl(string Attachment, int MailId, string FileName)

        {
            string AttachmentUrl = string.Empty;
            foreach (string str1 in FileName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                AttachmentUrl = AttachmentUrl + MailId + "\\" + Attachment + "\\" + str1 + ",";
            }
            return AttachmentUrl;
        }
    }
}