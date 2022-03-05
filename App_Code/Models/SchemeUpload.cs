using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class SchemeUpload
{
    [Required]
    public int ID { get; set; }

    public int SchemeID { get; set; }
    public string SchemeType { get; set; }
    public string SchemeName { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }
    public string Remark { get; set; }
    public string Attachment { get; set; }

    public string BranchName { get; set; }
    public string PartyName { get; set; }
    public string EmailId { get; set; }
    public string Mobile { get; set; }
    public string SalesExName { get; set; }
    public string cin { get; set; }
    public string PartyType { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Area { get; set; }
    public string To { get; set; }
    public string ToName { get; set; }
    public int UserID { get; set; }
    public int LogNO { get; set; }
    public string ExtraRemark { get; set; }
    public string BCC { get; set; }
    public string Date { get; set; }
    public string FullAdd { get; set; }
    public string Dealer { get; set; }
    public byte SendNow { get; set; }
}

public class SchemeUploads
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<DetailsTemplate> data { get; set; }
}

public class DetailsTemplate
{
    public string SubjectTEMP { get; set; }
    public string BodyTEMP { get; set; }
}