using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Xml;

/// <summary>
/// Summary description for EveryDay_Invoice
/// </summary>
///
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class EveryDay_Invoice : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da;
        private MessageService ms = new MessageService();
        private readonly IGoldMedia _goldMedia;

        private SqlDataReader rdr = null;

        public EveryDay_Invoice()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
            _goldMedia = new GoldMedia();
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        /// <param name="node">Xml node that represents a task description</param>
        public void Execute(XmlNode node)
        {
            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }

            DataTable dt = new DataTable();

            dt = _messageService.getdata("InvoiceheadselectprintOnDaily ");
            if (dt.Rows.Count > 0)
            {
                PartyMail(dt);
                ExcuativeMail(dt);
                AgentMail(dt);
            }

            //   _messageService.DeleteSendQueuedEmail();
        }

        //protected void PartyMail(DataTable dt)
        //{
        //    int TemplateId = 3020;
        //    EveryDayInvoice MailSMS = new EveryDayInvoice();
        //    DataTable dtcheck = da.GetEmailForTimeCheckbyID(TemplateId);
        //    if (dtcheck.Rows.Count > 0)
        //    {
        //        string FileName = string.Empty;
        //        dt.DefaultView.Sort = "partyid ASC";
        //        dt = dt.DefaultView.ToTable();

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            DataTable dtparty = _messageService.return_dt("exec PartyDetailsbyidandcat " + dt.Rows[i]["partyid"] + "," + dt.Rows[i]["typeofparty"]);

        //            if (dtparty.Rows.Count > 0)
        //            {
        //                if (_messageService.ValidateEmailId(dtparty.Rows[0]["emailid"].ToString()) == true)
        //                {
        //                    byte[] report = generatePDF(Convert.ToInt32(dt.Rows[i]["SlNo"]), dt.Rows[i]["uniquekey"].ToString());

        //                    FileName = string.Format(@"{0}{1}", dtparty.Rows[0]["cin"].ToString() + dtparty.Rows[0]["printnm"].ToString() + ".pdf", "");
        //                    string uniquefoldernm = dtparty.Rows[0]["cin"].ToString() + " " + dtparty.Rows[0]["printnm"].ToString();
        //                    string link = string.Format(WebConfigurationManager.AppSettings["AttachmentUrl"].ToString() + "{0}\\" + "{1}", TemplateId, uniquefoldernm);
        //                    string FileCheck = link + "\\" + FileName;
        //                    string directoryPath = Path.GetFullPath(link);

        //                    MailSMS.PartyName = dtparty.Rows[0]["printnm"].ToString();
        //                    MailSMS.ExName = "";
        //                    MailSMS.cin = dtparty.Rows[0]["cin"].ToString();

        //                    if (directoryPath != "")
        //                    {
        //                        Directory.CreateDirectory(directoryPath);
        //                    }

        //                    if (File.Exists(link + FileName) == true)
        //                    {
        //                        File.Delete(link + FileName);
        //                    }

        //                    File.WriteAllBytes(link + "//" + FileName, report);

        //                    SendMail(TemplateId, dtparty.Rows[0]["emailid"].ToString(), dtparty.Rows[0]["printnm"].ToString(), FileName, uniquefoldernm, MailSMS, "Everyday-Invoice-Dealer");
        //                }
        //            }
        //        }

        //    }

        //}

        protected void PartyMail(DataTable dt)
        {
            int TemplateId = 3020;
            string PartyName = string.Empty;
            DataTable dtcheck = da.GetEmailForTimeCheckbyID(TemplateId);
            if (dtcheck.Rows.Count > 0)
            {
                string FileName = string.Empty;
                dt.DefaultView.Sort = "partyid ASC";
                dt = dt.DefaultView.ToTable();

                List<DataTable> tables = new List<DataTable>();
                tables = SplitTableByParty(dt);

                for (int i = 0; i < tables.Count; i++)
                {
                    string FolderName = string.Empty;
                    string PDFName = string.Empty;
                    string Partyemail = string.Empty;
                    string ExName = string.Empty;
                    string Attachment = string.Empty;
                    string uniquefoldernm = string.Empty;
                    EveryDayInvoice MailSMS = new EveryDayInvoice();
                    int flag = 0;
                    int flag1 = 0;

                    for (int k = 0; k < tables[i].Rows.Count; k++)
                    {
                        int ExId = Convert.ToInt32(tables[i].Rows[0]["salesexid"]);
                        int Partyid = Convert.ToInt32(tables[i].Rows[0]["partyid"]);
                        int Partycat = Convert.ToInt32(tables[i].Rows[0]["typeofparty"]);

                        DataTable dtEx = _messageService.return_dt("exec salesexselectByID " + ExId);
                        DataTable dtparty = _messageService.return_dt("exec PartyDetailsbyidandcat " + tables[i].Rows[0]["partyid"] + "," + tables[i].Rows[0]["typeofparty"]);
                        if (dtparty.Rows.Count > 0)
                        {
                            PartyName = dtparty.Rows[0]["printnm"].ToString();
                        }

                        if (dtparty.Rows.Count > 0)
                        {
                            flag = 1;

                            Partyemail = dtparty.Rows[0]["emailid"].ToString();
                            if (dtEx.Rows.Count > 0)
                            {
                                ExName = dtEx.Rows[0]["salesexnm"].ToString();
                            }

                            byte[] report = generatePDF(Convert.ToInt32(tables[i].Rows[k]["SlNo"]), tables[i].Rows[k]["uniquekey"].ToString());

                            FileName = string.Format(@"{0}{1}", tables[i].Rows[k]["SlNo"].ToString() + tables[i].Rows[k]["PartyName2"], ".pdf");
                            uniquefoldernm = Partyid.ToString() + "Party" + Partycat.ToString();
                            Attachment = Partyid.ToString() + "Party" + Partycat.ToString() + ".zip";
                            string link = HostingEnvironment.MapPath(string.Format("~/App_Data/Scheme/" + "{0}/{1}", TemplateId, uniquefoldernm));
                            FolderName = HostingEnvironment.MapPath(string.Format("~/App_Data/Scheme/" + "{0}/", TemplateId) + Attachment);
                            PDFName = link;

                            MailSMS.PartyName = PartyName;
                            MailSMS.ExName = ExName;
                            MailSMS.cin = dtparty.Rows[0]["cin"].ToString();

                            string directoryPath = link;
                            if (directoryPath != "")
                            {
                                if (flag1 == 0)
                                {
                                    if (File.Exists(FolderName))
                                    {
                                        File.Delete(FolderName);
                                    }
                                }

                                Directory.CreateDirectory(directoryPath);
                            }

                            File.WriteAllBytes(link + "/" + FileName, report);
                        }

                        flag1++;
                    }
                    if (flag == 1)
                    {
                        if (File.Exists(@FolderName) == true)
                        {
                            File.Delete(FolderName);
                        }

                        ZipFile.CreateFromDirectory(PDFName, FolderName);
                        Directory.Delete(PDFName, true);

                        byte[] data = File.ReadAllBytes(PDFName + ".zip");
                        Stream stream = new MemoryStream(data);
                        var retStr = _goldMedia.GoldMediaUpload(uniquefoldernm, TemplateId.ToString(), ".zip", stream, "application/x-zip-compressed", false, false, true);
                        File.Delete(PDFName + ".zip");

                        if (_messageService.ValidateEmailId(Partyemail) == true)
                        {
                            SendMail(TemplateId, Partyemail, PartyName, Attachment, MailSMS, "Everday-Invoice-Dealer");
                        }
                    }
                }
            }
        }

        protected void ExcuativeMail(DataTable dt)
        {
            int TemplateId = 3022;
            string PartyName = string.Empty;
            DataTable dtcheck = da.GetEmailForTimeCheckbyID(TemplateId);
            if (dtcheck.Rows.Count > 0)
            {
                string FileName = string.Empty;
                dt.DefaultView.Sort = "salesexid ASC";
                dt = dt.DefaultView.ToTable();

                List<DataTable> tables = new List<DataTable>();
                tables = SplitTableByExcutive(dt);

                for (int i = 0; i < tables.Count; i++)
                {
                    string FolderName = string.Empty;
                    string PDFName = string.Empty;
                    string ExEmail = string.Empty;
                    string ExName = string.Empty;
                    string Attachment = string.Empty;
                    string uniquefoldernm = string.Empty;
                    EveryDayInvoice MailSMS = new EveryDayInvoice();
                    int flag = 0;
                    int flag1 = 0;

                    for (int k = 0; k < tables[i].Rows.Count; k++)
                    {
                        int ExId = Convert.ToInt32(tables[i].Rows[0]["salesexid"]);

                        DataTable dtEx = _messageService.getdatabyuser("salesexselectByID", ExId, "@Id");
                        DataTable dtparty = _messageService.return_dt("exec PartyDetailsbyidandcat " + tables[i].Rows[0]["partyid"] + "," + tables[i].Rows[0]["typeofparty"]);
                        if (dtparty.Rows.Count > 0)
                        {
                            PartyName = dtparty.Rows[0]["printnm"].ToString();
                        }

                        if (dtEx.Rows.Count > 0)
                        {
                            flag = 1;

                            ExEmail = dtEx.Rows[0]["email"].ToString();
                            ExName = dtEx.Rows[0]["salesexnm"].ToString();

                            byte[] report = generatePDF(Convert.ToInt32(tables[i].Rows[k]["SlNo"]), tables[i].Rows[k]["uniquekey"].ToString());
                            FileName = string.Format(@"{0}{1}", tables[i].Rows[k]["SlNo"].ToString() + tables[i].Rows[k]["PartyName2"] + ".pdf", "");
                            uniquefoldernm = ExId + "Ex";
                            Attachment = ExId + "Ex" + ".zip";
                            string link = HostingEnvironment.MapPath(string.Format("~/App_Data/Invoice/" + "{0}/{1}", TemplateId, uniquefoldernm));
                            FolderName = HostingEnvironment.MapPath(string.Format("~/App_Data/Invoice/" + "{0}/", TemplateId) + Attachment);
                            PDFName = link;

                            MailSMS.PartyName = PartyName;
                            MailSMS.ExName = ExName;
                            MailSMS.cin = dtparty.Rows[0]["cin"].ToString();

                            string directoryPath = Path.GetFullPath(link);
                            if (directoryPath != "")
                            {
                                if (flag1 == 0)
                                {
                                    if (File.Exists(FolderName))
                                    {
                                        File.Delete(FolderName);
                                    }
                                }

                                Directory.CreateDirectory(directoryPath);
                            }

                            File.WriteAllBytes(link + "//" + FileName, report);
                        }

                        flag1++;
                    }
                    if (flag == 1)
                    {
                        if (File.Exists(@FolderName) == true)
                        {
                            File.Delete(FolderName);
                        }

                        ZipFile.CreateFromDirectory(@PDFName, FolderName);
                        Directory.Delete(PDFName, true);
                        byte[] data = File.ReadAllBytes(PDFName + ".zip");
                        Stream stream = new MemoryStream(data);
                        var retStr = _goldMedia.GoldMediaUpload(uniquefoldernm, TemplateId.ToString(), ".zip", stream, "application/x-zip-compressed", false, false, true);
                        File.Delete(PDFName + ".zip");

                        if (_messageService.ValidateEmailId(ExEmail) == true)
                        {
                            SendMail(TemplateId, ExEmail, ExName, Attachment, MailSMS, "Everday-Invoice-Excutive");
                        }
                    }
                }
            }
        }

        protected void AgentMail(DataTable dt)
        {
            int TemplateId = 3022;

            DataTable dtcheck = da.GetEmailForTimeCheckbyID(TemplateId);
            if (dtcheck.Rows.Count > 0)
            {
                string FileName = string.Empty;
                dt.DefaultView.Sort = "agentid ASC";
                dt = dt.DefaultView.ToTable();

                List<DataTable> tables = new List<DataTable>();
                tables = SplitTableByAgent(dt);

                for (int i = 0; i < tables.Count; i++)
                {
                    string FolderName = string.Empty;
                    string PDFName = string.Empty;
                    string ExEmail = string.Empty;
                    string ExName = string.Empty;
                    string Attachment = string.Empty;
                    string uniquefoldernm = string.Empty;
                    string PartyName = string.Empty;
                    EveryDayInvoice MailSMS = new EveryDayInvoice();
                    int flag = 0;
                    int flag1 = 0;
                    for (int k = 0; k < tables[i].Rows.Count; k++)
                    {
                        int AgentId = Convert.ToInt32(tables[i].Rows[0]["agentid"]);

                        DataTable dtAgt = _messageService.getdatabyuser("AgentselectByID", AgentId, "@recid");
                        DataTable dtparty = _messageService.return_dt("exec PartyDetailsbyidandcat " + tables[i].Rows[0]["partyid"] + "," + tables[i].Rows[0]["typeofparty"]);
                        if (dtparty.Rows.Count > 0)
                        {
                            PartyName = dtparty.Rows[0]["printnm"].ToString();
                        }

                        if (dtAgt.Rows.Count > 0)
                        {
                            flag = 1;
                            ExEmail = dtAgt.Rows[0]["email"].ToString();
                            ExName = dtAgt.Rows[0]["agentnm"].ToString();

                            byte[] report = generatePDF(Convert.ToInt32(tables[i].Rows[k]["SlNo"]), tables[i].Rows[k]["uniquekey"].ToString());

                            FileName = string.Format(@"{0}{1}", tables[i].Rows[k]["SlNo"].ToString() + tables[i].Rows[k]["PartyName2"] + ".pdf", "");

                            Attachment = "zipAgent" + AgentId + ".zip";
                            uniquefoldernm = AgentId + "Agt";
                            string link = HostingEnvironment.MapPath(string.Format("~/App_Data/Invoice/" + "{0}/{1}", TemplateId, uniquefoldernm));
                            FolderName = HostingEnvironment.MapPath(string.Format("~/App_Data/Invoice/" + "{0}/", TemplateId) + Attachment);
                            PDFName = link;

                            MailSMS.PartyName = PartyName;
                            MailSMS.ExName = ExName;
                            MailSMS.cin = dtparty.Rows[0]["cin"].ToString();

                            string directoryPath = Path.GetFullPath(link);
                            if (directoryPath != "")
                            {
                                if (flag1 == 0)
                                {
                                    if (File.Exists(FolderName))
                                    {
                                        File.Delete(FolderName);
                                    }
                                }

                                Directory.CreateDirectory(directoryPath);
                            }

                            File.WriteAllBytes(link + "//" + FileName, report);
                        }

                        flag1++;
                    }

                    if (flag == 1)
                    {
                        if (File.Exists(@FolderName) == true)
                        {
                            File.Delete(FolderName);
                        }
                        ZipFile.CreateFromDirectory(PDFName, FolderName);
                        Directory.Delete(PDFName, true);
                        byte[] data = File.ReadAllBytes(PDFName + ".zip");
                        Stream stream = new MemoryStream(data);
                        var retStr = _goldMedia.GoldMediaUpload(uniquefoldernm, TemplateId.ToString(), ".zip", stream, "application/x-zip-compressed", false, false, true);
                        File.Delete(PDFName + ".zip");

                        if (_messageService.ValidateEmailId(ExEmail) == true)
                        {
                            SendMail(TemplateId, ExEmail, ExName, Attachment, MailSMS, "Everday-Invoice-Agent");
                        }
                    }
                }
            }
        }

        private void SendMail(int TemplateId, string Email, string Name, string Attachment, EveryDayInvoice MailSMS, string type)
        {
            DataTable dtcheck = da.GetQueuedEmailHistoryforExandag(Email, TemplateId);

            if (dtcheck.Rows.Count <= 0)
            {
                string Attach = string.Empty;
                string Subject = string.Empty;
                string Body = string.Empty;
                if (Attachment != "")
                {
                    Attach = ProperAttachmentUrl(Attachment, TemplateId);
                }
                DataTable dt = new DataTable();
                dt = da.GetEmailTemplateDatabyID(TemplateId);

                if (dt.Rows.Count > 0)
                {
                    Subject = HttpUtility.HtmlDecode(ms.ReplaceTemplateAutoInvoice(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), MailSMS));

                    Body = ms.ReplaceTemplateAutoInvoice(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), MailSMS);

                    Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                    da.AddEmailQueue(0, Email, Name, "", "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), 0, 0, TemplateId, Attach, type, 1);
                }
            }
        }

        public string ProperAttachmentUrl(string Attachment, int MailId)

        {
            string AttachmentUrl = string.Empty;
            foreach (string str1 in Attachment.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                AttachmentUrl = AttachmentUrl + MailId + "\\" + str1 + ",";
            }
            return AttachmentUrl;
        }

        private static List<DataTable> SplitTableByParty(DataTable originalTable)
        {
            List<DataTable> tables = new List<DataTable>();

            int i = 1;
            int j = 1;

            DataTable newDt = originalTable.Clone();
            newDt.TableName = "Table_" + j;
            newDt.Clear();

            if (originalTable.Rows.Count > 1)
            {
                foreach (DataRow row in originalTable.Rows)
                {
                    DataRow newRow = newDt.NewRow();
                    newRow.ItemArray = row.ItemArray;
                    newDt.Rows.Add(newRow);

                    if (originalTable.Rows[i]["partyid"].ToString().TrimEnd().TrimStart() != originalTable.Rows[i - 1]["partyid"].ToString().TrimEnd().TrimStart())
                    {
                        tables.Add(newDt);
                        j++;
                        newDt = originalTable.Clone();
                        newDt.TableName = "Table_" + j;
                        newDt.Clear();
                        //i=0;
                    }

                    if (i < originalTable.Rows.Count - 1)
                    { i++; }
                }

                tables.Add(newDt);
            }
            else
            {
                tables.Add(originalTable);
            }
            return tables;
        }

        private static List<DataTable> SplitTableByExcutive(DataTable originalTable)
        {
            List<DataTable> tables = new List<DataTable>();

            int i = 1;
            int j = 1;

            DataTable newDt = originalTable.Clone();
            newDt.TableName = "Table_" + j;
            newDt.Clear();

            if (originalTable.Rows.Count > 1)
            {
                foreach (DataRow row in originalTable.Rows)
                {
                    DataRow newRow = newDt.NewRow();
                    newRow.ItemArray = row.ItemArray;
                    newDt.Rows.Add(newRow);

                    if (originalTable.Rows[i]["salesexid"].ToString().TrimEnd().TrimStart() != originalTable.Rows[i - 1]["salesexid"].ToString().TrimEnd().TrimStart())
                    {
                        tables.Add(newDt);
                        j++;
                        newDt = originalTable.Clone();
                        newDt.TableName = "Table_" + j;
                        newDt.Clear();
                        //i=0;
                    }

                    if (i < originalTable.Rows.Count - 1)
                    { i++; }
                }

                tables.Add(newDt);
            }
            else
            {
                tables.Add(originalTable);
            }
            return tables;
        }

        private static List<DataTable> SplitTableByAgent(DataTable originalTable)
        {
            List<DataTable> tables = new List<DataTable>();

            int i = 1;
            int j = 1;

            DataTable newDt = originalTable.Clone();
            newDt.TableName = "Table_" + j;
            newDt.Clear();

            if (originalTable.Rows.Count > 1)
            {
                foreach (DataRow row in originalTable.Rows)
                {
                    DataRow newRow = newDt.NewRow();
                    newRow.ItemArray = row.ItemArray;
                    newDt.Rows.Add(newRow);

                    if (originalTable.Rows[i]["agentid"].ToString().TrimEnd().TrimStart() != originalTable.Rows[i - 1]["agentid"].ToString().TrimEnd().TrimStart())
                    {
                        tables.Add(newDt);
                        j++;
                        newDt = originalTable.Clone();
                        newDt.TableName = "Table_" + j;
                        newDt.Clear();
                        //i=0;
                    }

                    if (i < originalTable.Rows.Count - 1)
                    { i++; }
                }

                tables.Add(newDt);
            }
            else
            {
                tables.Add(originalTable);
            }
            return tables;
        }

        protected byte[] generatePDF(int invoceid, string uniquekey)
        {
            string url = WebConfigurationManager.AppSettings["ErpDailyInvoiceReport"].ToString() + "&id=" + invoceid + "&uniquekey=" + uniquekey;
            WebClient webClient = new WebClient();
            byte[] report = webClient.DownloadData(new Uri(url)); 
            return report;
        }
    }
}