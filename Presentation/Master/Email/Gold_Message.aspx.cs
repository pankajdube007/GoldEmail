using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class Presentation_Master_Email_Gold_Message : System.Web.UI.Page
{

    #region Initialization
    //  General g1 = new General();
    DataTable dt = new DataTable();
    DataTable dt1 = new DataTable();
    DateTime now = DateTime.Now;
    int rows = 0;
    public string PageName = string.Empty;
    private string FileName = string.Empty;
    private bool IsExportable = false;
    private readonly IEmailTemplateDA da = null;

    private readonly IExport xpt = null;
    private readonly IUserManagement userManagement;
    private long LogNo = 0;
    private int UserLogID = 0;
    string UsedTokan = "";

    public Presentation_Master_Email_Gold_Message()
    {
        da = new EmailTemplateDA();
        xpt = new Export();
        FileName = GetFileName();
        userManagement = new UserManagement();
        UserLogID = userManagement.GetUserInfoInt(UserEnum.UserInfoEnum.UserLogID);
        LogNo = userManagement.GetUserInfoLong(UserEnum.UserInfoEnum.LogNo);
        IsExportable = userManagement.IsDataExportable("Export.MessageTemplate");
    }

    protected string GetFileName()
    {
        return string.Format("Message_Template_{0}", DateTime.Now.ToString());
    }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        headerbind();
        //     ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "CharacterCount();", true);
        ASPxGridView1.DataBind();
    }
    protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView1.DataSource = GetDataTable();
    }

    protected DataTable GetDataTable()
    {
        return da.GetAllMessageTemplates();
    }
    protected void headerbind()
    {
        dt = da.GetAllHeaderForMessage();
        if (dt.Rows.Count > 0)
        {
            ls1.DataSource = dt;

            ls1.TextField = "name";
            ls1.ValueField = "messagetemplateid";
            ls1.DataBind();
        }
        dt.Reset();

    }

    protected void EnableControls()
    {
    //    txtheader.Enabled = true;
        edittime.Enabled = true;
        editdate.Enabled = true;
        ckactive.Enabled = true;
        btnsave.Enabled = true;
        btnsave.Visible = true;
        cbtokan.Enabled = true;
        lbChosen.Enabled = true;
        txtinterval.Enabled = true;



    }


    protected void DisableControls()
    {
        txtheader.Enabled = false;
        edittime.Enabled = false;
        editdate.Enabled = false;
        ckactive.Enabled = false;
        btnsave.Enabled = false;
        btnsave.Visible = false;
        cbtokan.Enabled = false;
        lbChosen.Enabled = false;
        txtinterval.Enabled = false;


    }


    protected void Tokanbind()
    {
        cbtokan.Items.Clear();
        dt = da.GetHaderTokanMappingByHeaderId(Convert.ToInt32(ls1.SelectedItem.Value));


        if (dt.Rows.Count > 0)
        {
            string tokan = dt.Rows[0]["TokanId"].ToString();
            dt1 = da.GetAvailableTokanCreationByString(tokan);
            if (dt1.Rows.Count > 0)
            {
                cbtokan.DataSource = dt1;

                cbtokan.TextField = "TokanName";
                cbtokan.ValueField = "TokanId";
                cbtokan.DataBind();
            }
        }
        dt.Reset();
        dt1.Reset();
    }
    protected void Click_PassData(object sender, EventArgs e)

    {
        ClearControlsData();

        if (ls1.SelectedIndex < 0)
        {
            lblheadererror.Visible = true;
            lblheadererror.Text = "Select Header First.";
        }
        else
        {
            lblheadererror.Visible = false;
            txtheader.Text = ls1.SelectedItem.Text;
          
            Tokanbind();
             BindData();
            EnableControls();
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "CharacterCount();", true);

        }



    }

    protected void ClearControlsData()
    {

        txtheader.Text = string.Empty;
        edittime.Text = string.Empty;
        editdate.Text = string.Empty;
        ckactive.Checked = false;
        lbChosen.Text = string.Empty;
        cbtokan.Items.Clear();
    }

    protected void BindData()

    {

        dt = da.GetMeassageTemplateByHeaderId(Convert.ToInt32(ls1.SelectedItem.Value));

        if (dt.Rows.Count > 0)
        {

            hdid.Value = dt.Rows[0]["MessageTemplateLocalizedID"].ToString();

            if (dt.Rows[0]["MessageTime"].ToString() != "" && dt.Rows[0]["MessageTime"] != null)
            {
                string[] st1 = dt.Rows[0]["MessageTime"].ToString().Split(' ');

                string[] st2 = st1[0].Split('-');

                if (st2.Count() > 2)
                {
                    // string sq = st2[2] + "-" + st2[1] + "-" + st2[0] + " " + st1[1];
                    edittime.Text = st2[2] + "-" + st2[1] + "-" + st2[0] + " " + st1[1];
                }
            }

            //   edittime.Text= Convert.ToDateTime(dt.Rows[0]["MessageTime"]).ToString("dd-MM-yyyy hh:mm");
            if (dt.Rows[0]["LastMessageDate"].ToString() != "" && dt.Rows[0]["LastMessageDate"] != null)
            {
                string[] st1 = dt.Rows[0]["LastMessageDate"].ToString().Split(' ');

                string[] st2 = st1[0].Split('-');

                if (st2.Count() > 2)
                {
                    editdate.Text = st2[2] + "-" + st2[1] + "-" + st2[0];
                }
            }
         //   editdate.Text= dt.Rows[0]["LastMessageDate"].ToString();
            if (dt.Rows[0]["IsActive"].ToString() == "0")
            {
                ckactive.Checked = false;
            }
            else
            {
                ckactive.Checked = true;
            }
            lbChosen.Text= dt.Rows[0]["Body"].ToString();
            txtinterval.Text = dt.Rows[0]["Interval"].ToString();
        }
    }
    protected void Click_SaveData(object sender, EventArgs e)
    {

        string Startdate = string.Empty;
        string Expdate = string.Empty;
        if (edittime.Text == "")
        {
           
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Set Sending Time!');", true);

        }
        else if (txtinterval.Text == "" || txtinterval.Text == "0")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please set interval (put default as 1)!');", true);

        }
        else if (editdate.Text == "")
        {
           
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Set Expiry Date!');", true);
        }
        else

        {
            dt = da.GetMeassageTemplateByHeaderId(Convert.ToInt32(ls1.SelectedItem.Value));
            if (dt.Rows.Count > 0)
            {
                FindUsedTokan();
                //  string time = Convert.ToDateTime(edittime.Text).ToString("yyyy-MM-dd hh:mm");

                if (edittime.Text != "" && edittime.Text != null)
                {
                    string[] st1 = edittime.Text.Split(' ');

                    string[] st2 = st1[0].Split('-');

                    if (st2.Count() > 2)
                    {
                        Startdate = st2[2] + "-" + st2[1] + "-" + st2[0] + " " + st1[1];
                        //  edittime.Text = sq;
                    }
                }

                if (editdate.Text != "" && editdate.Text != null)
                {
                    string[] st1 = editdate.Text.Split('-');



                    if (st1.Count() > 2)
                    {
                        Expdate = st1[2] + "-" + st1[1] + "-" + st1[0];
                    }
                }


                da.EditMessageTemplates(Convert.ToInt32(ls1.SelectedItem.Value), Convert.ToString(lbChosen.Text), Convert.ToByte(ckactive.Value), Convert.ToInt32(LogNo), Convert.ToInt32(UserLogID), Convert.ToInt32(hdid.Value), UsedTokan, Startdate, Expdate,Convert.ToInt32(txtinterval.Text));
                DisableControls();
                ASPxGridView1.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Data Updated!');", true);
            }
            else
            {
                FindUsedTokan();
                //  string time = Convert.ToDateTime(edittime.Text).ToString("yyyy-MM-dd hh:mm");

                if (edittime.Text != "" && edittime.Text != null)
                {
                    string[] st1 = edittime.Text.Split(' ');

                    string[] st2 = st1[0].Split('-');

                    if (st2.Count() > 2)
                    {
                        Startdate = st2[2] + "-" + st2[1] + "-" + st2[0] + " " + st1[1];
                        //  edittime.Text = sq;
                    }
                }

                if (editdate.Text != "" && editdate.Text != null)
                {
                    string[] st1 = editdate.Text.Split('-');



                    if (st1.Count() > 2)
                    {
                        Expdate = st1[2] + "-" + st1[1] + "-" + st1[0];
                    }
                }



                da.AddMessageTemplates(Convert.ToInt32(ls1.SelectedItem.Value), Convert.ToString(lbChosen.Text), Convert.ToByte(ckactive.Value), Convert.ToInt32(LogNo), Convert.ToInt32(UserLogID), UsedTokan, Startdate, Expdate,Convert.ToInt32(txtinterval.Text));
                DisableControls();
                ASPxGridView1.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Data added!');", true);
            }
        }
       
    }
   
    private void FindUsedTokan()
        {

        foreach (Match match in Regex.Matches(lbChosen.Text, @"%(.*?)%"))
        {

            if (UsedTokan == "")
            {
                UsedTokan = match.Groups[1].Value;


            }
            else
            {
                UsedTokan = UsedTokan + "," + match.Groups[1].Value;
            }
        }
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
}