using DevExpress.Web;

/// <summary>
/// Summary description for Export
/// </summary>
public class Export : IExport
{
    public void GoldGridExportToPdf(ASPxGridViewExporter agve,string FileName,bool SaveAs)
    {
        agve.WritePdfToResponse(FileName, SaveAs);
    }
    public void GoldGridExportToXls(ASPxGridViewExporter agve, string FileName, bool SaveAs)
    {
        agve.WriteXlsToResponse(FileName, SaveAs);
    }
    public void GoldGridExportToXlsx(ASPxGridViewExporter agve, string FileName, bool SaveAs)
    {
        agve.WriteXlsxToResponse(FileName, SaveAs);
    }
    public void GoldGridExportToRtf(ASPxGridViewExporter agve, string FileName, bool SaveAs)
    {
        agve.WriteRtfToResponse(FileName, SaveAs);
    }
    public void GoldGridExportToCsv(ASPxGridViewExporter agve, string FileName, bool SaveAs)
    {
        agve.WriteCsvToResponse(FileName, SaveAs);
    }
}