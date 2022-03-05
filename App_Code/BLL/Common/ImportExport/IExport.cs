using DevExpress.Web;

/// <summary>
/// Summary description for IExport
/// </summary>
public interface IExport
{
    void GoldGridExportToPdf(ASPxGridViewExporter agve, string FileName, bool SaveAs);
    void GoldGridExportToXls(ASPxGridViewExporter agve, string FileName, bool SaveAs);
    void GoldGridExportToXlsx(ASPxGridViewExporter agve, string FileName, bool SaveAs);
    void GoldGridExportToRtf(ASPxGridViewExporter agve, string FileName, bool SaveAs);
    void GoldGridExportToCsv(ASPxGridViewExporter agve, string FileName, bool SaveAs);
}