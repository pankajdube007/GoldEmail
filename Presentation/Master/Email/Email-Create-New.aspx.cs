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

public partial class Email_Create_New : System.Web.UI.Page
{
    #region Intionalization
   
    DataTable dt = new DataTable();
    int rows = 0;
    public string PageName = string.Empty;
    private string FileName = string.Empty;
    private bool IsExportable = false;
    private readonly IEmailDA da = null;
    
    private readonly IExport xpt = null;
    private readonly IUserManagement userManagement;
    private long LogNo = 0;
    private int UserLogID = 0;
   
    #endregion

    public Email_Create_New()
    {
        da = new EmailDA();
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
        return da.GetEmailData();
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
    protected void Click_btnsave(object sender,EventArgs e)
    {
        string pattern = null;
        pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
        

        if (Regex.IsMatch(txtemail.Text, pattern))
        {
            Save();

        }
        else 
        {
            lblerror.Visible = true;
            lblerror.Text = "Please Enter Valid Email Id.";
           
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
        dt = da.GetEmailById(FieldTripID);
        if (dt.Rows.Count > 0)
        {
           txtemail.Text= dt.Rows[0]["Email"].ToString();
            txtdisplay.Text= dt.Rows[0]["DisplayName"].ToString(); 
            txthost.Text= dt.Rows[0]["Host"].ToString();
            txtport.Text= dt.Rows[0]["Port"].ToString();
            txtuserid.Text= dt.Rows[0]["UserName"].ToString();
            txtpassword.Text= dt.Rows[0]["Password"].ToString();
            ckenablessl.Checked= Convert.ToBoolean(dt.Rows[0]["EnableSSL"].ToString());
            ckuserdefault.Checked = Convert.ToBoolean(dt.Rows[0]["UseDefaultCredentials"].ToString());
            ckdefaultemail.Checked = Convert.ToBoolean(dt.Rows[0]["DefaultEmail"].ToString());
            txtemailLimit.Text = dt.Rows[0]["EmailLimit"].ToString();
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
        btnsave.Text = "Save";
        lblerror.Text = "";
        txtemail.Text = "";
        txtdisplay.Text = "";
        txthost.Text = "";
        txtport.Text = "";
        txtuserid.Text = "";
        txtpassword.Text = "";
        ckenablessl.Checked = false;
        ckuserdefault.Checked = false;
        ckdefaultemail.Checked = false;
        txtemailLimit.Text = "";
        txtemail.Focus();

    }

   protected void Save()
    {
        if (txtemailLimit.Text != "")
        {
            if (ckdeactivate.Checked == true)
            {
                dt = da.EmailMappingCheckByEmailId(Convert.ToInt32(Label1.Text));
            }

            if (dt.Rows.Count > 0)
            {

                lblerror.Text = "Email Id Already in Use, Please Remove from Template First...";
                lblerror.Visible = true;
                lblerror.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (btnsave.Text == "Edit")
                {
                    string Id = Label1.Text;
                    da.UpdateEmailData(txtemail.Text, txtdisplay.Text, txthost.Text, Convert.ToInt32(txtport.Text), txtuserid.Text, txtpassword.Text, Convert.ToByte(ckenablessl.Value), Convert.ToByte(ckuserdefault.Value), Convert.ToByte(ckdefaultemail.Value), Convert.ToInt32(txtemailLimit.Text), Convert.ToInt32(LogNo), UserLogID, Convert.ToInt32(Id));
                    clear();
                    lblerror.Visible = true;
                    lblerror.Text = "Data Updated Sussessfully";
                    lblerror.ForeColor = System.Drawing.Color.Green;
                    btnsave.Text = "Save";
                    ASPxGridView1.DataBind();

                }
                else if (btnsave.Text == "Save")
                {
                    bool check = false;

                    check = da.CheckEmailData(txtemail.Text);
                    if (check == true)
                    {
                        lblerror.Text = "Record already Exist";
                        // clear();
                        lblerror.Visible = true;
                        lblerror.ForeColor = System.Drawing.Color.Red;

                    }
                    else
                    {
                        da.SaveEmailData(txtemail.Text, txtdisplay.Text, txthost.Text, Convert.ToInt32(txtport.Text), txtuserid.Text, txtpassword.Text, Convert.ToByte(ckenablessl.Value), Convert.ToByte(ckuserdefault.Value), Convert.ToByte(ckdefaultemail.Value), Convert.ToInt32(txtemailLimit.Text), Convert.ToInt32(LogNo), UserLogID);
                        clear();
                        lblerror.Text = "Email Id Added Sussessfully";
                        lblerror.Visible = true;
                        lblerror.ForeColor = System.Drawing.Color.Green;
                        ASPxGridView1.DataBind();


                    }
                }
            }
        }
        else
        {
            lblerror.Text = "Please Enter Email Limit";
            lblerror.Visible = true;
            lblerror.ForeColor = System.Drawing.Color.Red;
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