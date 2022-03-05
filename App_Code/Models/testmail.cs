using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for testmail
/// </summary>

public class testmail
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
    public string BranchId { get; set; }
    public string Status { get; set; }
    public string BranchStatus { get; set; }
    public string BranchName { get; set; }
    public int SchemeTypeId { get; set; }
    public string ToEmail { get; set; }
    public string ToEmailName { get; set; }
    public int UserID { get; set; }
    public int LogNO { get; set; }
}

public class testmails
{
    public int ID { get; set; }
    public int HeaderID { get; set; }
    public string BCCEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsActive { get; set; }
    public string UsedTokan { get; set; }
    public string EmailTime { get; set; }
    public string EmailLastDate { get; set; }
}

public class Listtestmail
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<testmail> data { get; set; }
}