using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Presentation_Master_Email_Send_Message : System.Web.UI.Page
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

    public Presentation_Master_Email_Send_Message()
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
            txtfromdate.Text = DateTime.Now.AddDays(-30).ToString("dd/MM/yy");
            txttodate.Text = DateTime.Now.Date.ToString("dd/MM/yy");
        }
        ASPxGridView1.DataBind();
    }

    protected void CmdDelete_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
    {
        try
        {
            int FieldTripID = Convert.ToInt32(e.CommandArgument);
            DataTable dt1 = da.GetSendMessageByID(FieldTripID);

            if (dt1.Rows.Count > 0)
            {
                txtMessageBody.Text = dt1.Rows[0]["Body"].ToString();
                ASPxPopupControl1.ShowOnPageLoad = true;
            }


            
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
        string fromdate4 = string.Empty, todate4 = string.Empty;

        if (txtfromdate.Text != "")
        {
            string[] st1 = txtfromdate.Text.Split('/');

            if (st1.Count() > 2)
            {
                fromdate4 = st1[1] + "/" + st1[0] + "/" + st1[2];
            }


        }



        if (txttodate.Text != "")
        {
            string[] st1 = txttodate.Text.Split('/');

            if (st1.Count() > 2)
            {
                todate4 = st1[1] + "/" + st1[0] + "/" + st1[2];
            }


        }
        return da.GetSendMessage(fromdate4,todate4);
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


    protected void CmdSearch_Click(object sender, EventArgs e)
    {
        ASPxGridView1.DataBind();
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