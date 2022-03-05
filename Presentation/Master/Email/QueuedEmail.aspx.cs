using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Presentation_Master_Email_QueuedEmail : System.Web.UI.Page
{
    #region Intionalization

    DataTable dt = new DataTable();
    int rows = 0;
    public string PageName = string.Empty;
    private string FileName = string.Empty;
    private bool IsExportable = false;
    private readonly IEmailTemplateDA da = null;

    private readonly IExport xpt = null;
    private readonly IUserManagement userManagement;
    private long LogNo = 0;
    private int UserLogID = 0;

    #endregion

    public Presentation_Master_Email_QueuedEmail()
    {
        da = new EmailTemplateDA();
        xpt = new Export();
        FileName = GetFileName();
        userManagement = new UserManagement();
        UserLogID = userManagement.GetUserInfoInt(UserEnum.UserInfoEnum.UserLogID);
        LogNo = userManagement.GetUserInfoLong(UserEnum.UserInfoEnum.LogNo);
        IsExportable = userManagement.IsDataExportable("Export.Email");
    }

    protected string GetFileName()
    {
        return string.Format("Email_Master_{0}", DateTime.Now.ToString());
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ASPxPageControl1.ActiveTabIndex = 0;
        }
        ASPxGridView1.DataBind();
    }

    protected void CmdDelete_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
    {

        try
        {
            int FieldTripID = Convert.ToInt32(e.CommandArgument);
            da.DeleteQueuedEmailByID(FieldTripID);
            ASPxGridView1.DataBind();
        }
        catch (Exception ex)
        {

        }
    }
    protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView1.DataSource = GetDataTable();
    }

    protected DataTable GetDataTable()
    {
        return da.GetQueuedEmail();
    }

    #region export
    protected void btnPdfExport_Click(object sender, EventArgs e)
    {

        ASPxGridViewExporter1.WritePdfToResponse();
    }

    protected void btnXlsExport_Click(object sender, EventArgs e)
    {

        ASPxGridViewExporter1.WriteXlsToResponse();
    }

    protected void btnXlsxExport_Click(object sender, EventArgs e)
    {

        ASPxGridViewExporter1.WriteXlsxToResponse();
    }

    protected void btnRtfExport_Click(object sender, EventArgs e)
    {

        ASPxGridViewExporter1.WriteRtfToResponse();
    }

    protected void btnCsvExport_Click(object sender, EventArgs e)
    {

        ASPxGridViewExporter1.WriteCsvToResponse();
    }

    #endregion

    #region Event

    protected void btndeleteall_Click(object sender, EventArgs e)
    {
        da.DeleteAllQueuedEmail();
        ASPxGridView1.DataBind();
        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('All Data Queue Email Removed successfully!');", true);
    }




    #endregion

    #region Method




    protected void BindGrid()
    {
        //      dt = g1.return_dt("exec  Gold_EmailAccountShow ");
        ASPxGridView1.DataSource = dt;
        ASPxGridView1.DataBind();
    }
    #endregion


   
}