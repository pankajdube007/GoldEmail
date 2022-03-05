using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for AutoPendingSalesOrder
/// </summary>
///
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoPendingSalesOrder : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da;
        private MessageService ms = new MessageService();
        private readonly IGoldMedia _goldMedia;

        private SqlDataReader rdr = null;

        public AutoPendingSalesOrder()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
            _goldMedia = new GoldMedia();
        }

        public void Execute(XmlNode node)
        {
            int TemplateId = 3031;

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
            da.AddActiveScheme(DealerTemplateId);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dtparty = _messageService.return_dt("exec getpopendingitempowiseAutoEmail " + dt.Rows[i]["cin"].ToString() + ",'" + DateTime.Now.Date + "'");
                if (dtparty.Rows.Count > 0)
                {
                    if (_messageService.ValidateEmailId(dtparty.Rows[0]["emailid"].ToString()) == true)
                    {
                        //DataTable dtcheckhistory = da.GetQueuedEmailHistoryforExandag(dtparty.Rows[0]["emailid"].ToString(), DealerTemplateId);
                        //if (dtcheckhistory.Rows.Count <= 0)
                        //{
                        DataTable dtfinal = dtparty.DefaultView.ToTable(false, "itemnm", "BaseCode", "MaterialIssueBranch", "SchemePer", "ApproveQty", "stockqty", "DispatchQty", "Pending", "SchemeQty");

                        dtfinal.Columns["SchemeQty"].ColumnName = "Promotional Discount Qty";
                        dtfinal.Columns["SchemePer"].ColumnName = "Promotional Discount(%)";
                        Stream data = new MemoryStream(ExportToExcel(dtfinal));
                        //  long testiyi = data.Length;
                        PendingDetails MailSMS = FillPendingDetails(dtparty);

                        FileName = dtparty.Rows[0]["name"].ToString();
                        //  string uniquefoldernm = "/";
                        var retStr = _goldMedia.GoldMediaUpload(FileName, DealerTemplateId.ToString(), ".xls", data, "application/vnd.ms-excel", false, false, true);

                        SendMail(DealerTemplateId, dtparty.Rows[0]["emailid"].ToString(), dtparty.Rows[0]["partyname"].ToString(), FileName + ".xls", MailSMS);
                    }
                    // }
                }
            }
        }

        private PendingDetails FillPendingDetails(DataTable dtparty)
        {
            PendingDetails MailSMS = new PendingDetails();
            MailSMS.ID = 3030;
            MailSMS.To = dtparty.Rows[0]["emailid"].ToString();
            MailSMS.ToName = dtparty.Rows[0]["PartyName"].ToString();
            MailSMS.PartyCode = Convert.ToInt32(dtparty.Rows[0]["Code"].ToString());
            MailSMS.SaleExName = dtparty.Rows[0]["salesexname"].ToString();
            MailSMS.cin = dtparty.Rows[0]["cin"].ToString();
            MailSMS.PartyType = dtparty.Rows[0]["PartyType"].ToString();
            MailSMS.HomeBranch = dtparty.Rows[0]["HomeBranch"].ToString();
            MailSMS.City = dtparty.Rows[0]["city"].ToString();
            MailSMS.State = dtparty.Rows[0]["statenm"].ToString();
            MailSMS.Area = dtparty.Rows[0]["areanm"].ToString();
            MailSMS.Mobile = dtparty.Rows[0]["Mobile"].ToString();
            MailSMS.Email = dtparty.Rows[0]["emailid"].ToString();
            MailSMS.PartyName = dtparty.Rows[0]["PartyName"].ToString();
            MailSMS.Email = dtparty.Rows[0]["emailid"].ToString();
            MailSMS.UserID = 9999;
            MailSMS.LogNO = 9999;
            MailSMS.Attachment = "";
            MailSMS.ExtraRemarks = "";
            MailSMS.CC = "";
            MailSMS.Date = DateTime.Now.Date.ToString();
            MailSMS.SendNow = 1;
            MailSMS.For = "Email";

            return MailSMS;
        }

        private byte[] ExportToExcel(DataTable table)
        {
            using (var memStream = new MemoryStream())
            using (var sw = new StreamWriter(memStream))
            {
                sw.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                sw.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                sw.Write("<BR><BR><BR>");
                sw.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                int columnscount = table.Columns.Count;

                for (int j = 0; j < columnscount; j++)
                {
                    sw.Write("<Td>");
                    sw.Write("<B>");
                    sw.Write(table.Columns[j].ToString());
                    sw.Write("</B>");
                    sw.Write("</Td>");
                }
                sw.Write("</TR>");
                foreach (DataRow row in table.Rows)
                {
                    sw.Write("<TR>");
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        sw.Write("<Td>");
                        sw.Write(row[i].ToString());
                        sw.Write("</Td>");
                    }
                    sw.Write("</TR>");
                }
                sw.Write("</Table>");
                sw.Write("</font>");
                //  sw.Close();

                sw.Flush();
                sw.Close();
                return memStream.ToArray();
                //  ms.Seek(0, SeekOrigin.Begin);
            }
        }

        private void SendMail(int TemplateId, string Email, string Name, string FileName, PendingDetails MailSMS)
        {
            //DataTable dtcheck = da.GetQueuedEmailHistoryforExandag(Email, TemplateId);
            //if (dtcheck.Rows.Count <= 0)
            //{
            string Attach = string.Empty;
            string Subject = string.Empty;
            string Body = string.Empty;
            if (FileName != "")
            {
                Attach = ProperAttachmentUrl(TemplateId, FileName);
            }
            DataTable dt = new DataTable();
            dt = da.GetEmailTemplateDatabyID(TemplateId);

            if (dt.Rows.Count > 0)
            {
                Subject = HttpUtility.HtmlDecode(ms.ReplaceTemplatePartyWisePendingDetails(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), MailSMS));

                Body = ms.ReplaceTemplatePartyWisePendingDetails(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), MailSMS);

                Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                da.AddEmailQueue(0, Email, Name, "", "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), 0, 0, TemplateId, Attach, "Auto-SalesOrder-Pending", 1);
            }
            // }
        }

        public string ProperAttachmentUrl(int MailId, string FileName)

        {
            string AttachmentUrl = string.Empty;
            foreach (string str1 in FileName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                AttachmentUrl = AttachmentUrl + MailId + "\\" + str1 + ",";
            }
            return AttachmentUrl;
        }
    }
}