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
/// Summary description for BranchParty_Active_Scheme_Report
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class BranchParty_Active_Scheme_Report : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da;
        private MessageService ms = new MessageService();
        private readonly IGoldMedia _goldMedia;

        private SqlDataReader rdr = null;

        public BranchParty_Active_Scheme_Report()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
            _goldMedia = new GoldMedia();
        }

        public void Execute(XmlNode node)
        {
            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }

            DataTable dt = new DataTable();

            dt = _messageService.getdata("APIbranchselect");
            if (dt.Rows.Count > 0)
            {
                BranchActiveSchemeMail(dt);
            }
        }

        protected void BranchActiveSchemeMail(DataTable dt)
        {
            string AttachmentName = string.Empty;
            string FileName = string.Empty;
            int TemplateId = 3032;

            DataTable dtcheck = da.GetEmailForTimeCheckbyID(TemplateId);
            if (dtcheck.Rows.Count > 0)
            {
                int flag = 0;
                DataTable dtActiveTime = da.GetActiveSchemeTimeID(TemplateId);
                if (dtActiveTime.Rows.Count <= 0)
                {
                    flag = 1;
                }
                else
                {
                    DateTime lastsend = Convert.ToDateTime(dtActiveTime.Rows[0]["lastSend"]);
                    int days = (DateTime.Now.Date - lastsend.Date).Days;
                    int interval = Convert.ToInt32(dtcheck.Rows[0]["interval"]);
                    if (days >= interval)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag = 0;
                    }
                }

                if (flag == 1)
                {
                    da.AddActiveScheme(TemplateId);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string cc = string.Empty;
                        string Attachment = string.Empty;
                        string FolderName = string.Empty;
                        string PDFName = string.Empty;
                        string link = string.Empty;
                        string email = "arpit.j@goldmedalindia.com";
                        //  string email = "pdube@gmail.com";
                        int count = 1;

                        //   DataTable dtcheckhistory = da.GetQueuedEmailHistoryforExandag("pdube@gmail.com", TemplateId);
                        //   if (dtcheckhistory.Rows.Count <= 0)
                        //   {
                        string uniquefoldernm = string.Empty;
                        if (dt.Rows[i]["SlNo"].ToString() != "" && dt.Rows[i]["SlNo"].ToString() != null)
                        {
                            DataTable dtParty1 = new DataTable();
                            dtParty1 = _messageService.return_dt("GetallPartyForActiveSchemeBranchWise " + dt.Rows[i]["SlNo"].ToString());
                            if (dtParty1.Rows.Count > 0)
                            {
                                List<DataTable> dtParty = SplitTable(dtParty1, 100);
                                for (int k = 0; k < dtParty.Count; k++)
                                {
                                    if (dtParty[k].Rows.Count > 0)
                                    {
                                        for (int j = 0; j < dtParty[k].Rows.Count; j++)
                                        {
                                            try
                                            {
                                                byte[] report = generatePDF(Convert.ToInt32(dtParty[k].Rows[j]["cin"]));

                                                FileName = string.Format(@"{0}", dtParty[k].Rows[j]["cin"].ToString() + dtParty[k].Rows[j]["printnm"].ToString() + ".pdf");
                                                uniquefoldernm = dt.Rows[i]["locnm"].ToString().Replace("/", "-") + count;
                                                Attachment = dt.Rows[i]["locnm"].ToString().Replace("/", "-") + count + ".zip";
                                                link = HostingEnvironment.MapPath(string.Format("~/App_Data/Scheme/" + "{0}/{1}", TemplateId, uniquefoldernm));
                                                FolderName = HostingEnvironment.MapPath(string.Format("~/App_Data/Scheme/" + "{0}/", TemplateId) + Attachment);
                                                //  //  PDFName = link;

                                                ////  string FileCheck = link + "/" + FileName;
                                                // // string directoryPath = HttpContext.Current.Server.MapPath(link);
                                                string directoryPath = link;
                                                if (directoryPath != "")
                                                {
                                                    Directory.CreateDirectory(directoryPath);
                                                }

                                                if (File.Exists(directoryPath + FileName) == true)
                                                {
                                                    File.Delete(directoryPath + FileName);
                                                }

                                                File.WriteAllBytes(directoryPath + "/" + FileName, report);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        count++;

                                        if (File.Exists(FolderName) == true)
                                        {
                                            File.Delete(FolderName);
                                        }

                                        ZipFile.CreateFromDirectory(link, FolderName);
                                        DeleteDirectory(link);

                                        byte[] data = File.ReadAllBytes(link + ".zip");
                                        Stream stream = new MemoryStream(data);
                                        var retStr = _goldMedia.GoldMediaUpload(uniquefoldernm, TemplateId.ToString(), ".zip", stream, "application/x-zip-compressed", false, false, true);
                                        File.Delete(link + ".zip");

                                        SendMail(TemplateId, email, dt.Rows[i]["locnm"].ToString(), Attachment, cc, dt.Rows[i]["locnm"].ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<DataTable> SplitTable(DataTable tableToClone, int countLimit)
        {
            List<DataTable> tables = new List<DataTable>();
            int count = 0;
            DataTable copyTable = null;
            foreach (DataRow dr in tableToClone.Rows)
            {
                if ((count++ % countLimit) == 0)
                {
                    copyTable = new DataTable();
                    copyTable = tableToClone.Clone();
                    copyTable.TableName = "TableCount" + count;
                    tables.Add(copyTable);
                }
                copyTable.ImportRow(dr);
            }
            return tables;
        }

        //private static List<DataTable> SplitTable(DataTable originalTable, int batchSize)
        //{
        //    List<DataTable> tables = new List<DataTable>();
        //    int i = 0;
        //    int j = 1;
        //    DataTable newDt = originalTable.Clone();
        //    newDt.TableName = "Table_" + j;
        //    newDt.Clear();
        //    foreach (DataRow row in originalTable.Rows)
        //    {
        //        DataRow newRow = newDt.NewRow();
        //        newRow.ItemArray = row.ItemArray;
        //        newDt.Rows.Add(newRow);
        //        i++;
        //        if (i == batchSize)
        //        {
        //            tables.Add(newDt);
        //            j++;
        //            newDt = originalTable.Clone();
        //            newDt.TableName = "Table_" + j;
        //            newDt.Clear();
        //            i = 0;
        //        }
        //    }
        //    return tables;
        //}

        private void DeleteDirectory(string link)
        {
            DirectoryInfo dir = new DirectoryInfo(@link);
            foreach (FileInfo files in dir.GetFiles())
            {
                files.Delete();
            }

            dir.Delete(true);
        }

        private void SendMail(int TemplateId, string Email, string Name, string Attachment, string cc, string BranchName)
        {
            //    DataTable dtcheck = da.GetQueuedEmailHistoryforExandag(Email, TemplateId);
            //    if (dtcheck.Rows.Count <= 0)
            //    {
            string Attach = string.Empty;
            string Subject = string.Empty;
            string Body = string.Empty;
            if (Attachment != "")
            {
                Attach = TemplateId + "\\" + Attachment;
            }
            DataTable dt = new DataTable();
            dt = da.GetEmailTemplateDatabyID(TemplateId);

            if (dt.Rows.Count > 0)
            {
                Subject = HttpUtility.HtmlDecode(dt.Rows[0]["Subject"].ToString());

                Body = ms.ReplaceEmailTemplateBranchWiseActiveScheme(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), BranchName);

                Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                da.AddEmailQueue(0, Email, Name, cc, "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), 0, 0, TemplateId, Attach, "Active-Scheme-Report", 1);
            }
            // }
        }

        protected byte[] generatePDF(int PartyCin)
        {
            string url = WebConfigurationManager.AppSettings["ErpBranchActiveSchemeReport"].ToString() + PartyCin;
            var webClient = new WebClient();
            byte[] report = webClient.DownloadData(new Uri(url));
            return report;
        }
    }
}