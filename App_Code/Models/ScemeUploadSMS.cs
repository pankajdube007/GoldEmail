using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for ScemeUploadSMS
/// </summary>
public class ScemeUploadSMS
{
    [Required]
    public int ID { get; set; }

    public int SchemeID { get; set; }
    public string SchemeType { get; set; }
    public string SchemeName { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }
    public string Remark { get; set; }
    public string BranchId { get; set; }
    public string Status { get; set; }
    public string BranchStatus { get; set; }
    public string BranchName { get; set; }
    public int SchemeTypeId { get; set; }
    public string ToSMS { get; set; }
    public string ToSMSName { get; set; }
    public int UserID { get; set; }
    public int LogNO { get; set; }
}

public class ScemeUploadSMSs
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<ScemeUploadSMS> data { get; set; }
}