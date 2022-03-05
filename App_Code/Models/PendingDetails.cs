using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for PendingDetails
/// </summary>
public class PendingDetails
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
    public string PartyName { get; set; }
    public int UserID { get; set; }
    public int LogNO { get; set; }
    public string Attachment { get; set; }
    public string ExtraRemarks { get; set; }
    public string CC { get; set; }
    public string Date { get; set; }
    public byte SendNow { get; set; }
    public string For { get; set; }
}

public class PendingDetailsDetails
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<DetailsTemplatePendingDetails> data { get; set; }
}

public class DetailsTemplatePendingDetails
{
    public string SubjectTEMP { get; set; }
    public string BodyTEMP { get; set; }
}