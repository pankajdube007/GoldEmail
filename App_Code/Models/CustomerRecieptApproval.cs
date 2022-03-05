﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for CustomerRecieptApproval
/// </summary>
public class CustomerRecieptApproval
{
    [Required]
    // public int ID { get; set; }
    public string TO { get; set; }

    public string TOName { get; set; }
    public int PartyCode { get; set; }
    public string OrderAmt { get; set; }
    public string OrderDt { get; set; }
    public string ChequeNo { get; set; }
    public string ChequeAmt { get; set; }
    public string instrumenttype { get; set; }
    public string BranchId { get; set; }
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
    public string VatNo { get; set; }
    public string CstNo { get; set; }
    public int UserID { get; set; }
    public int LogNO { get; set; }
    public string For { get; set; }
}

public class CustomerRecieptApprovalDetails
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<DetailsTemplateCustomerRecieptApproval> data { get; set; }
}

public class DetailsTemplateCustomerRecieptApproval
{
    public string SubjectTEMP { get; set; }
    public string BodyTEMP { get; set; }
}