using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for POApproval
/// </summary>
public class POApproval
{
    [Required]
    public int ID { get; set; }

    public string TO { get; set; }
    public string TOName { get; set; }
    public int PartyCode { get; set; }
    public string OrderNo { get; set; }
    public string OrderAmt { get; set; }
    public string OrderDt { get; set; }
    public string dispatchfrom { get; set; }
    public string BranchId { get; set; }
    public string BranchName { get; set; }
    public string PartyName { get; set; }
    public string EmailId { get; set; }
    public string Mobile { get; set; }
    public string SalesExName { get; set; }
    public string SalesExContact { get; set; }
    public string cin { get; set; }
    public string PartyType { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Area { get; set; }
    public string VatNo { get; set; }
    public string CstNo { get; set; }
    public string Attachment { get; set; }
    public int UserID { get; set; }
    public int LogNO { get; set; }
    public string For { get; set; }
}

public class POApprovalDetails
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<DetailsTemplatePOApproval> data { get; set; }
}

public class DetailsTemplatePOApproval
{
    public string SubjectTEMP { get; set; }
    public string BodyTEMP { get; set; }
}