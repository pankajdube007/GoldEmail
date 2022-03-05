using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.UI;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Threading;
using DevExpress.Web;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;

public partial class Presentation_Master_Email_Header_Create_New : System.Web.UI.Page
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

    public Presentation_Master_Email_Header_Create_New()
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

    protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView1.DataSource = GetDataTable();
    }

    protected DataTable GetDataTable()
    {
        return da.GetAllHeader();
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
    protected void Click_btnsave(object sender, EventArgs e)
    {
        if (txtheader.Text == "")
        {
            lblerror.Visible = true;
            lblerror.Text = "Please Enter Header Before Saving..";
            lblerror.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            if (ckemail.Checked == false && cksms.Checked == false)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select Email or SMS..";
                lblerror.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                Save();
            }
        }
    }


    protected void Click_btnreset(object sender, EventArgs e)
    {
        clear();
    }

    protected void CmdEdit_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
    {

        lblerror.Text = "";
        int FieldTripID = Convert.ToInt32(e.CommandArgument);
        //  bool check = false;
        Label1.Text = Convert.ToString(FieldTripID);
        ASPxPageControl1.ActiveTabIndex = 0;
      dt = da.GetHaderById(FieldTripID);
        if (dt.Rows.Count > 0)
        {
            txtheader.Text = dt.Rows[0]["Name"].ToString();
            ckemail.Checked = Convert.ToBoolean(dt.Rows[0]["Email"].ToString());
            cksms.Checked = Convert.ToBoolean(dt.Rows[0]["SMS"].ToString());

            btnsave.Text = "Edit";

        }
        else
        {
            lblerror.Text = "Data Not Found.";
        }

    }
    #endregion

    #region Method
    protected void clear()
    {
        lblerror.Text = "";
        txtheader.Text = "";
        ckemail.Checked = false;
        cksms.Checked = false;
        btnsave.Text = "Save";
        txtheader.Focus();

    }

    protected void Save()
    {
        if (btnsave.Text == "Edit")
        {
            string Id = Label1.Text;
            da.EditHeader(txtheader.Text, Convert.ToByte(ckemail.Value), Convert.ToByte(cksms.Value), Convert.ToInt32(LogNo), UserLogID, Convert.ToInt32(Id));
            clear();
            lblerror.Visible = true;
            lblerror.Text = "Data Updated Sussessfully";
            lblerror.ForeColor = System.Drawing.Color.Green;
            btnsave.Text = "Save";
            ASPxGridView1.DataBind();

        }
        else if (btnsave.Text == "Save")
        {
          //  bool check = false;

            dt = da.CheckHeader(Convert.ToString(txtheader.Text));
            if (dt.Rows.Count>0)
            {
                lblerror.Text = "Record already Exist";
                // clear();
                lblerror.Visible = true;
                lblerror.ForeColor = System.Drawing.Color.Red;

            }
            else
            {
              da.AddHeader(txtheader.Text,Convert.ToByte(ckemail.Value), Convert.ToByte(cksms.Value), Convert.ToInt32(LogNo), Convert.ToInt32(UserLogID));
                clear();
                lblerror.Text = "Email Id Added Sussessfully";
                lblerror.Visible = true;
                lblerror.ForeColor = System.Drawing.Color.Green;
                ASPxGridView1.DataBind();


            }
        }
    }

    protected void BindGrid()
    {
        //      dt = g1.return_dt("exec  Gold_EmailAccountShow ");
        ASPxGridView1.DataSource = dt;
        ASPxGridView1.DataBind();
    }
    #endregion
}