using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for PartywiseLedger
/// </summary>
public class PartywiseLedger
{
    [Required]
    public int ID { get; set; }

    public string To { get; set; }
    public string ToName { get; set; }
    public int PartyCode { get; set; }
    public string SaleExName { get; set; }
    public string cin { get; set; }
    public string PartyType { get; set; }
    public string HomeBranch { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Area { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public string VatNo { get; set; }
    public string CstNo { get; set; }
    public string PartyName { get; set; }
    public int UserID { get; set; }
    public int LogNO { get; set; }
    public string Attachment { get; set; }

    public string ExtraRemarks { get; set; }
    public string CC { get; set; }

    public string Date { get; set; }
    public string Dealer { get; set; }
    public string FullAdd { get; set; }
    public byte SendNow { get; set; }

    public string For { get; set; }
}

public class PartywiseLedgerDetails
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<DetailsTemplatePartywiseLedger> data { get; set; }
}

public class DetailsTemplatePartywiseLedger
{
    public string SubjectTEMP { get; set; }
    public string BodyTEMP { get; set; }
}