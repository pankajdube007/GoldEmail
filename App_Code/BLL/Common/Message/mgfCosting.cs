using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using iTextSharp.tool.xml;

/// <summary>
/// Summary description for mgfCosting
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class mgfCosting :ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da;
        private readonly IGoldMedia _goldMedia;
        private MessageService ms = new MessageService();

        private SqlDataReader rdr = null;

        public mgfCosting()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
            _goldMedia = new GoldMedia();
        }

        public void Execute(XmlNode node)
        {
            //   int DealerTemplateId = 3025;
            //   int ExTemplateId = 3024;

            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }
            PartyMail();
            //  PartyAgentMail(ExTemplateId);
        }


        //protected void PartyMail()
        //{
        //    string AttachmentName = string.Empty;
        //    string FileName = string.Empty;
        //    DataTable dt = _messageService.return_dt("exec mfgcostingpdf");

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        try
        //        {
        //            DataTable dt1 = _messageService.return_dt("exec mfgcostingpdfupdate " + Convert.ToInt32(dt.Rows[i]["SlNo"]));

        //            byte[] report = generatePDF(Convert.ToString(dt.Rows[i]["cin"]), Convert.ToString(dt.Rows[i]["uniquekey"]));
        //            Stream stream = new MemoryStream(report);
        //            FileName = "test";
        //            //  string uniquefoldernm = dtparty.Rows[0]["cin"].ToString() + "CreditNote";

        //            var retStr = _goldMedia.GoldMediaUpload(FileName, "creditnote", ".pdf", stream, "application/pdf", false, false, true);

        //           // DataTable dt1 = _messageService.return_dt("exec mfgcostingpdfupdate " + Convert.ToInt32(dt.Rows[i]["SlNo"]));
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //}





        //protected byte[] generatePDF(string slno, string uniquekey)
        //{
        //    string url = "https://ax.goldmedalindia.in:81/UnitCastingPrintNew?id=" + slno + "&uniquekey=" + uniquekey;

        //    var webClient = new WebClient();
        //    byte[] report = webClient.DownloadData(new Uri(url));
        //    return report;
        //}


        //protected void PartyMail()
        //{
        //    string AttachmentName = string.Empty;
        //    string FileName = string.Empty;
        //    DataTable dt = _messageService.return_dt("exec getschemedata");

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        try
        //        {
        //            byte[] report = generatePDF(Convert.ToString(dt.Rows[i]["cin"]));
        //        Stream stream = new MemoryStream(report);
        //        FileName = string.Format(@"{0}", dt.Rows[i]["cin"].ToString()+"jun2021");
        //      //  string uniquefoldernm = dtparty.Rows[0]["cin"].ToString() + "CreditNote";

        //        var retStr = _goldMedia.GoldMediaUpload(FileName, "creditnote", ".pdf", stream, "application/pdf", false, false, true);

        //        DataTable dt1 = _messageService.return_dt("exec updatescheme '"+ dt.Rows[i]["partyid"].ToString()+"','"+ dt.Rows[i]["typecat"].ToString() +"','"+ FileName +".pdf"+ "'");
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //}




        //protected byte[] generatePDF(string PartyCin)
        //{
        //    string url = WebConfigurationManager.AppSettings["ErpBranchActiveSchemeReport"].ToString() + PartyCin;

        //    var webClient = new WebClient();
        //    byte[] report = webClient.DownloadData(new Uri(url));
        //    return report;
        //}

        protected void PartyMail()
        {
            string AttachmentName = string.Empty;
            string FileName = string.Empty;
            DataTable dt = _messageService.return_dt("exec mfgcostingpdf");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    // byte[] report = generatePDFmfg(Convert.ToString(dt.Rows[i]["SlNo"]), Convert.ToString(dt.Rows[i]["uniquekey"]));
                    Stream stream = createPDF(Convert.ToString(dt.Rows[i]["SlNo"]), Convert.ToString(dt.Rows[i]["uniquekey"]));
                    FileName = Convert.ToString(dt.Rows[i]["filenm"]);
                    //  string uniquefoldernm = dtparty.Rows[0]["cin"].ToString() + "CreditNote";

                    var retStr = _goldMedia.GoldMediaUpload(FileName, "costing/" + Convert.ToString(dt.Rows[i]["foldernm"]), ".pdf", stream, "application/pdf", false, false, true);

                    //  DataTable dt1 = _messageService.return_dt("exec mfgcostingpdfupdate " + Convert.ToInt32(dt.Rows[i]["SlNo"]));
                }
                catch (Exception ex)
                {
                }
            }
        }




        //protected byte[] generatePDFmfg(string slno, string uniquekey)
        //{
        //    string url = "https://ax.goldmedalindia.in/UnitCastingPrintNew?id=" + slno + "&uniquekey=" + uniquekey;
        //   // string url = "https://ax.goldmedalindia.in/NewProductionProcessReport?id=1737&uniquekey=1a480612-a727-4c57-bf7d-5a68e09ca3f9";

        //    var webClient = new WebClient();
        //     string html = webClient.DownloadString(new Uri(url));

        //    return Encoding.ASCII.GetBytes(html);

        //}

        protected byte[] generatePDFmfg(string slno, string uniquekey)
        {


            string url = "https://ax.goldmedalindia.in/UnitCastingPrintNew?id=" + slno + "&uniquekey=" + uniquekey;


            var webClient = new WebClient();
            string html = webClient.DownloadString(new Uri(url));

            StringReader sr = new StringReader(html.ToString());

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            //XMLWorkerHelper htmlparser = new XMLWorkerHelper(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                XMLWorkerHelper xml = XMLWorkerHelper.GetInstance();
                xml.ParseXHtml(writer, pdfDoc, memoryStream, System.Text.Encoding.UTF8);
                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();


                return bytes;
            }

        }

        private MemoryStream createPDF(string slno, string uniquekey)
        {

            string url = "https://ax.goldmedalindia.in/UnitCastingPrintNew?id=" + slno + "&uniquekey=" + uniquekey;


            var webClient = new WebClient();
            string html = webClient.DownloadString(new Uri(url));

            // string html = File.ReadAllText(@"D:\Live Projects\GoldEmail\App_Data\UserFiles\test.html");
            //    html = html.Replace("px", "");
            //    html = html.Replace("<br>", "<br/>");
            // html = html.Replace("../images/GoldmedalManufacturing.png", "https://ax.goldmedalindia.in/images/GoldmedalManufacturing.png");

            //    html = html.Replace("C:\\Program Files (x86)\\images\\GoldmedalManufacturing.png", "");




            //// Document document = new Document();
            var bytes = System.Text.Encoding.UTF8.GetBytes(html);

            using (var input = new MemoryStream(bytes))
            {
                var output = new MemoryStream(); // this MemoryStream is closed by FileStreamResult

                var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();

                var xmlWorker = XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, document, input, System.Text.Encoding.UTF8);
                document.Close();
                output.Position = 0;
                return output;
            }


        }
    }
}