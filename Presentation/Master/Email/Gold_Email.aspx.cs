using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.IO;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data.SqlClient;



public partial class Gold_Email : System.Web.UI.Page
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

    public Gold_Email()
    {
        da = new EmailTemplateDA();
        xpt = new Export();
        FileName = GetFileName();
        userManagement = new UserManagement();
        UserLogID = userManagement.GetUserInfoInt(UserEnum.UserInfoEnum.UserLogID);
        LogNo = userManagement.GetUserInfoLong(UserEnum.UserInfoEnum.LogNo);
        IsExportable = userManagement.IsDataExportable("Export.EmailTemplate");
    }

    protected string GetFileName()
    {
        return string.Format("Email_Template_{0}", DateTime.Now.ToString());
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ASPxPageControl1.ActiveTabIndex = 0;
        headerbind();
        ASPxGridView1.DataBind();
       



    }

    protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView1.DataSource = GetDataTable();
    }

    protected DataTable GetDataTable()
    {
        return da.GetAllEmailTemplates();
    }
    #region Method
    protected void headerbind()
    {
        dt = da.GetAllHeaderForEmail();
        if (dt.Rows.Count > 0)
        {
            ls1.DataSource = dt;

            ls1.TextField = "name";
            ls1.ValueField = "messagetemplateid";
            ls1.DataBind();
        }
        dt.Reset();

    }

    protected void ASPxListBox1_Init(object sender, EventArgs e)
    {
        PopulateListBox();
    }
    protected void ASPxListBox1_Callback(object sender, CallbackEventArgsBase e)
    {
        PopulateListBox();
    }
    private void PopulateListBox()
    {
        ASPxListBox1.Items.Clear();
        for (int i = 0; i < chkemail.Items.Count; i++)
            if (chkemail.Items[i].Selected == true && chkemail.Items[i].Text != "Select all")
            {
                ASPxListBox1.Items.Add(chkemail.Items[i].Text, chkemail.Items[i].Value);
            }
    }


    protected void Check_Index(object sender, EventArgs e)
    {
        PopulateListBox();
    }
    protected void emailbind()
    {
        ASPxListBox myListB = ASPxDropDownEdit1.FindControl("listBox") as ASPxListBox;
      
        txtbody.Html = "";
        dt = da.GetAllEmail();

        if (dt.Rows.Count > 0)
        {
            dt.Columns.Add("FullDropdown", typeof(string), "DisplayName + '(' + Email + ')'");

        
            myListB.DataSource = dt;
            myListB.TextField = "Email";
            myListB.ValueField = "EmailAccountId";
            myListB.DataBind();
            myListB.Items.Insert(0, new ListEditItem("Select all", 0));


            chkemail.DataSource = dt;
            chkemail.TextField = "FullDropdown";
            chkemail.ValueField = "EmailAccountId";
            chkemail.DataBind();
            chkemail.Items.Insert(0, new ListEditItem("Select all", 0));

       

        }
        dt.Reset();
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
    protected void addheader()
    {
        dt = da.CheckHeader(txtheaderpopup.Text);
        if (dt.Rows.Count > 0)
        {
            lblheadererror1.Visible = true;
            lblheadererror1.Text = "Header Exist, Please try different one.";
            hederpopup.ShowOnPageLoad = true;

        }
        else
        {
            lblheadererror.Visible = false;
            //   da.AddHeader(txtheaderpopup.Text, UserLogID, Convert.ToInt32(LogNo));

            headerbind();
            hederpopup.ShowOnPageLoad = false;
            txtheaderpopup.Text = "";
        }

    }

    protected void enablecontrols()
    {
        // cbemail.Enabled = true;
        cbtokan.Enabled = true;
        txtsubject.Enabled = true;
        //  txtbcc.Enabled = true;
        txtbody.Enabled = true;
        ckactive.Enabled = true;
        btnsave.Visible = true;
        txtbody.Enabled = true;
        ckattachment.Enabled = true;
        ckattachment.Checked = false;
        edittime.Enabled = true;
        editdate.Enabled = true;
        ASPxDropDownEdit1.Enabled = true;
        ASPxListBox1.Enabled = true;
        chkemail.Enabled = true;
        btnUp.Enabled = true;
        btnDown.Enabled = true;
        txtsign.Enabled = true;
        txtinterval.Enabled = true;
       

    }

    protected void disablecontrols()
    {
        // cbemail.Enabled = false;
        cbtokan.Enabled = false;
        txtsubject.Enabled = false;
        //    txtbcc.Enabled = false;
        txtbody.Enabled = false;
        ckactive.Enabled = false;
        btnsave.Visible = false;
        txtbody.Enabled = false;
        ckattachment.Enabled = false;
        fileUpload1.Visible = false;
        Attachment.Text = "";
        edittime.Enabled = false;
        editdate.Enabled = false;
        ASPxDropDownEdit1.Enabled = false;
        ASPxListBox1.Enabled = false;
        chkemail.Enabled = false;
        btnUp.Enabled = false;
        btnDown.Enabled = false;
        txtsign.Enabled = false;
        txtinterval.Enabled = false;
       

    }

    protected void BindData()
    {
        string abc = string.Empty;
        dt = da.GetEmailAllData(Convert.ToInt32(ls1.SelectedItem.Value));
        if (dt.Rows.Count > 0)
        {
            abc = dt.Rows[0]["MessageTemplateLocalizedID"].ToString();
            dt1 = da.GetEmailMappingByHeader(Convert.ToInt32(abc));
        }


        if (dt1.Rows.Count > 0)
        {
            for (int i = 0; i <= dt1.Rows.Count - 1; i++)
            {
                for (int j = 1; j < chkemail.Items.Count ; j++)
                {
                    if (Convert.ToInt32(dt1.Rows[i]["EmailAccountId"].ToString()) == Convert.ToInt32(chkemail.Items[j].Value))
                    {

                        chkemail.Items[j].Selected = true;

                    }
                }
            }
        }
        PopulateListBox();
        if (dt.Rows.Count > 0)
        {
            ASPxListBox myListB = ASPxDropDownEdit1.FindControl("listBox") as ASPxListBox;
            hdid.Value = dt.Rows[0]["messagetemplatelocalizedid"].ToString();           
            txtsubject.Text = dt.Rows[0]["subject"].ToString();           
            ASPxDropDownEdit1.Text = dt.Rows[0]["bccemailaddresses"].ToString();
          
            txtbody.Html = dt.Rows[0]["body"].ToString();
            if (dt.Rows[0]["body"].ToString() == "0")
            {
                ckactive.Checked = false;
            }
            else
            {
                ckactive.Checked = true;
            }
            if(dt.Rows[0]["EmailTime"].ToString()!="" && dt.Rows[0]["EmailTime"]!=null)
            {
                string[] st1 = dt.Rows[0]["EmailTime"].ToString().Split(' ');

                string[] st2 = st1[0].Split('-');

                if (st2.Count() > 2)
                {
                   // string sq = st2[2] + "-" + st2[1] + "-" + st2[0] + " " + st1[1];
                    edittime.Text = st2[2] + "-" + st2[1] + "-" + st2[0] + " " + st1[1];
                }
            }

            //  edittime.Text =Convert.ToDateTime(dt.Rows[0]["EmailTime"]).ToString("dd/MM/yyyy hh:mm");

            if (dt.Rows[0]["EmailLastDate"].ToString() != "" && dt.Rows[0]["EmailLastDate"] != null)
            {
                string[] st1 = dt.Rows[0]["EmailLastDate"].ToString().Split(' ');

                string[] st2 = st1[0].Split('-');

                if (st2.Count() > 2)
                {
                    editdate.Text = st2[2] + "-" + st2[1] + "-" + st2[0] ;
                }
            }

          //  editdate.Text = Convert.ToDateTime(dt.Rows[0]["EmailLastDate"]).ToString("dd/MM/yyyy");
           
            Attachment.Text = dt.Rows[0]["AttachmentUrl"].ToString();
            txtsign.Html = dt.Rows[0]["Signature"].ToString();
            txtinterval.Text = dt.Rows[0]["Interval"].ToString();


        }
        else
        {          
            ckactive.Checked = false;
            txtsubject.Text = "";


        }
    }

    private void FindUsedTokan()
    {

        foreach (Match match in Regex.Matches(txtbody.Html, @"%(.*?)%"))
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


        foreach (Match match in Regex.Matches(txtsubject.Text, @"%(.*?)%"))
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

    protected void SaveEmailTemplate()
    {

        if (txtsubject.Text == "")
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Subject Should not be Blank!');", true);
        }
        else if(edittime.Text=="")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Set Sending Time!');", true);

        }

        else if (editdate.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Set Expiry Date!');", true);

        }
        else if (txtbody.Html == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Write Something in Body!');", true);

        }
        else if (ckattachment.Checked==true && fileUpload1.PostedFiles.Count<=0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Attach Attachment!');", true);

        }
        else if (txtinterval.Text == "" || txtinterval.Text =="0")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please set interval (put default as 1)!');", true);

        }
        else
        {
            lblLink.Text = "";
            string FileName = Attachment.Text;
            string strFileType = "";
            string abc = "";
            string singleFile = string.Empty;
            string Startdate = string.Empty;
            string Expdate = string.Empty;




            foreach (var file in fileUpload1.PostedFiles)
            {
                if (file.FileName != "")
                {
                    strFileType = Path.GetExtension(file.FileName).ToLower();
                    string theFileName = string.Empty;

                    decimal size = Math.Round(((decimal)file.ContentLength / (decimal)1024), 2);

                    var time = DateTime.Now;
                    string formattedTime = time.ToString("yyyyMMddhhmmss");
                    singleFile = string.Format(@"{0}{1}", Guid.NewGuid().ToString() + formattedTime, strFileType);
                    //   fileUpload1.SaveAs(Server.MapPath("~/Upload/" + abc + "/") + singleFile);


                    FileName += string.Format(@"{0},", singleFile);

                }
            }
            //   FileName = FileName.TrimEnd(',');
            dt = da.GetEmailAllData(Convert.ToInt32(ls1.SelectedItem.Value));
            if (dt.Rows.Count > 0)
            {
                string checkactive;
                if (ckactive.Checked == true)
                { checkactive = "1"; }
                else
                { checkactive = "0"; }



                FindUsedTokan();

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

                //  string time = Convert.ToDateTime(edittime.Text).ToString("yyyy-MM-dd hh:mm");
                da.EditEmailTemplates(Convert.ToInt32(ls1.SelectedItem.Value), ASPxDropDownEdit1.Text, txtsubject.Text, Convert.ToString(txtbody.Html), Convert.ToByte(checkactive), Convert.ToInt32(LogNo), Convert.ToInt32(UserLogID), Convert.ToInt32(hdid.Value), UsedTokan, Startdate, Expdate, FileName,txtsign.Html,Convert.ToInt32(txtinterval.Text));
                abc = dt.Rows[0]["MessageTemplateLocalizedID"].ToString();

                for (int i = 0; i <= ASPxListBox1.Items.Count - 1; i++)
                {

                    dt1 = da.CheckEmailMapping(Convert.ToInt32(abc), Convert.ToInt32(ASPxListBox1.Items[i].Value));

                    if (dt1.Rows.Count > 0)
                    {
                        da.EditEmailMapping(Convert.ToInt32(abc), Convert.ToInt32(ASPxListBox1.Items[i].Value), Convert.ToInt32(ASPxListBox1.Items[i].Index), Convert.ToInt32(UserLogID), Convert.ToInt32(LogNo), Convert.ToInt32(dt1.Rows[0]["SlNo"].ToString()));

                    }
                    else
                    {
                        da.ADDEmailMapping(Convert.ToInt32(abc), Convert.ToInt32(ASPxListBox1.Items[i].Value), Convert.ToInt32(ASPxListBox1.Items[i].Index), Convert.ToInt32(UserLogID), Convert.ToInt32(LogNo));
                    }


                }



                for (int i = 0; i <= chkemail.Items.Count - 1; i++)
                {

                    string i1 = abc;
                    int i2 = Convert.ToInt32(chkemail.Items[i].Value);

                    dt1 = da.CheckEmailMapping(Convert.ToInt32(abc), Convert.ToInt32(chkemail.Items[i].Value));

                    if (dt1.Rows.Count > 0)
                    {
                        if (chkemail.Items[i].Selected == false && chkemail.Items[i].Text != "Select all")
                        {
                            da.DeleteEmailMapping(Convert.ToInt32(abc), Convert.ToInt32(chkemail.Items[i].Value));
                        }
                    }



                }

                string link = string.Format("~/Upload/{0}/", abc);
                string directoryPath = Server.MapPath(link);
                if (directoryPath != "")
                {
                    Directory.CreateDirectory(directoryPath);
                    System.Threading.Thread.Sleep(10);
                }
                lblLink.Text = link;
                int count = fileUpload1.PostedFiles.Count();
                fileUpload1.SaveAs(Server.MapPath("~/Upload/" + abc + "/") + singleFile);

                disablecontrols();
                ASPxGridView1.DataBind();
                // System.Windows.Forms.MessageBox.Show("Data Edited Successfully");
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Data Edited Successfully!');", true);

            }
            else
            {

                string checkactive;
                if (ckactive.Checked == true)
                { checkactive = "1"; }
                else
                { checkactive = "0"; }

                FindUsedTokan();

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

              //  string time = Convert.ToDateTime(edittime.Text).ToString("yyyy-MM-dd hh:mm");
                da.AddEmailTemplates(Convert.ToInt32(ls1.SelectedItem.Value), ASPxDropDownEdit1.Text, txtsubject.Text, Convert.ToString(txtbody.Html), Convert.ToByte(checkactive), Convert.ToInt32(LogNo), Convert.ToInt32(UserLogID), UsedTokan, Startdate, Expdate, FileName,txtsign.Html,Convert.ToInt32(txtinterval.Text));

                dt = da.GetEmailAllData(Convert.ToInt32(ls1.SelectedItem.Value));
                abc = dt.Rows[0]["MessageTemplateLocalizedID"].ToString();


                for (int i = 0; i <= ASPxListBox1.Items.Count - 1; i++)
                {

                    dt1 = da.CheckEmailMapping(Convert.ToInt32(abc), Convert.ToInt32(ASPxListBox1.Items[i].Value));

                    if (dt1.Rows.Count > 0)
                    {
                        da.EditEmailMapping(Convert.ToInt32(abc), Convert.ToInt32(ASPxListBox1.Items[i].Value), i, Convert.ToInt32(UserLogID), Convert.ToInt32(LogNo), Convert.ToInt32(dt1.Rows[0]["SlNo"].ToString()));

                    }
                    else
                    {
                        da.ADDEmailMapping(Convert.ToInt32(abc), Convert.ToInt32(ASPxListBox1.Items[i].Value), i, Convert.ToInt32(UserLogID), Convert.ToInt32(LogNo));
                    }


                }

                //for (int i = 0; i <= chkemail.Items.Count - 1; i++)
                //{
                //    if (chkemail.Items[i].Text != "Select all" && chkemail.Items[i].Selected == true)
                //    {
                //        dt1 = da.CheckEmailMapping(Convert.ToInt32(abc), Convert.ToInt32(chkemail.Items[i].Value));
                //        if (dt1.Rows.Count > 0)
                //        {
                //            da.EditEmailMapping(Convert.ToInt32(abc), Convert.ToInt32(chkemail.Items[i].Value), Convert.ToInt32(chkemail.Items[i].Index), Convert.ToInt32(UserLogID), Convert.ToInt32(LogNo), Convert.ToInt32(dt1.Rows[0]["SlNo"].ToString()));

                //        }
                //        else
                //        {
                //            da.ADDEmailMapping(Convert.ToInt32(abc), Convert.ToInt32(chkemail.Items[i].Value), Convert.ToInt32(chkemail.Items[i].Index), Convert.ToInt32(UserLogID), Convert.ToInt32(LogNo));
                //        }

                //    }
                //}

                string link = string.Format("~/Upload/{0}/", abc);

                string directoryPath = Server.MapPath(link);

                if (directoryPath != "")
                {
                    Directory.CreateDirectory(directoryPath);
                }

                lblLink.Text = link;
                int count = fileUpload1.PostedFiles.Count();
                fileUpload1.SaveAs(Server.MapPath("~/Upload/" + abc + "/") + singleFile);

                disablecontrols();
                ASPxGridView1.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Data Added Successfully!');", true);
               
                
            }

        }

    }
    #endregion


    #region Event
    protected void click_headerpopupreset(object sender, EventArgs e)
    {
        txtheaderpopup.Text = "";
        lblheadererror1.Text = "";
    }

    protected void Check_Attachment(object sender, EventArgs e)
    {
        if (ckattachment.Checked == true)
        {

            fileUpload1.Visible = true;
            btnsave.Visible = true;
        }
        else
        {
            fileUpload1.Visible = false;
            btnsave.Visible = true;
        }
    }

    protected void click_headerpopupadd(object sender, EventArgs e)
    {

        if (txtheaderpopup.Text == "")
        {
            lblheadererror1.Text = "Please Type Header...";
            lblheadererror1.Visible = true;

        }
        else
        {
            lblheadererror1.Visible = false;
            addheader();           
            headerbind();


        }

    }

    protected void click_Passdata(object sender, EventArgs e)
    {
        ASPxDropDownEdit1.Enabled = true;
        ASPxListBox myListB = ASPxDropDownEdit1.FindControl("listBox") as ASPxListBox;
        chkemail.Items.Clear();
        ASPxDropDownEdit1.Text = "";
       
        myListB.Items.Clear();
        ASPxListBox1.Items.Clear();

        if (ls1.SelectedIndex < 0)
        {
            lblheadererror.Visible = true;
            lblheadererror.Text = "Select Header First.";
        }
        else
        {
            lblheadererror.Visible = false;
            txtheader.Text = ls1.SelectedItem.Text;
            emailbind();
            Tokanbind();
            BindData();
            enablecontrols();

        }


    }



    protected void click_save(object sender, EventArgs e)
    {

        SaveEmailTemplate();


    }

    #endregion

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