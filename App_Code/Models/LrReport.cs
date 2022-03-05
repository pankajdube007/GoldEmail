using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for LrReport
/// </summary>
public class LrReport
{
    [Required]
    public int ID { get; set; }

    public int ExID { get; set; }
    public string TO { get; set; }
    public string TOName { get; set; }
    public string ExContact { get; set; }
    public int PartyCode { get; set; }
    public string LrNo { get; set; }
    public string OrderAmt { get; set; }
    public string LrDt { get; set; }
    public string dispatchfrom { get; set; }
    public string TraspoterName { get; set; }
    public string Attachment { get; set; }
    public string BranchId { get; set; }
    public string BranchName { get; set; }
    public string PartyName { get; set; }

    public string Mobile { get; set; }
    public string SalesExName { get; set; }
    public string cin { get; set; }
    public string PartyType { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Area { get; set; }
    public string VatNo { get; set; }
    public string CstNo { get; set; }
    public int UserID { get; set; }
    public int LogNO { get; set; }
    public string For { get; set; }
}

public class LrReportDetails
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<DetailsTemplateCreateInvoice> data { get; set; }
}

public class DetailsTemplateLrReport
{
    public string SubjectTEMP { get; set; }
    public string BodyTEMP { get; set; }
}