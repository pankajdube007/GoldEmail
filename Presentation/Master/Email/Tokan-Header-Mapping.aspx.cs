using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Threading;

public partial class Tokan_Header_Mapping : System.Web.UI.Page
{
    #region Initialization
    //  General g1 = new General();
    DataTable dt = new DataTable();
    DataTable dt1 = new DataTable();
    DataTable dt2 = new DataTable();
    DataTable dt3 = new DataTable();
    int rows = 0;
    public string PageName = string.Empty;
    private string FileName = string.Empty;
    private bool IsExportable = false;
    private readonly IEmailTemplateDA da = null;

    private readonly IExport xpt = null;
    private readonly IUserManagement userManagement;
    private long LogNo = 0;
    private int UserLogID = 0;

    public Tokan_Header_Mapping()
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
        if (IsPostBack)
        {
           
            
        }
        ASPxPageControl1.ActiveTabIndex = 0;
        headerbind();
        // GetTokanList();
        //  bindgrid();
        ASPxGridView1.DataBind();
        
    }
    protected void ASPxCallback1_Callback(object source, CallbackEventArgs e)
    {
        Thread.Sleep(200);
    }
    protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView1.DataSource = GetDataTable();
    }

    protected DataTable GetDataTable()
    {
        return da.GetAllTokanHeaderMapping();
    }
    #region Method
    protected void headerbind()
    {
        dt = da.GetAllHeaderAfterSelection();
       
        if (dt.Rows.Count > 0)
        {
            ls1.DataSource = dt;

            ls1.TextField = "name";
            ls1.ValueField = "messagetemplateid";
            ls1.DataBind();


        }
        dt.Reset();

    }

    protected void GetTokanList()
    {
        dt1 = da.GetAllHeaderAfterSelection();
        if (dt1.Rows.Count > 0)
        {
            dt = da.GetAllAvailableTokanCreation();
            if (dt.Rows.Count > 0)
            {

                ls2.DataSource = dt;

                ls2.TextField = "TokanName";
                ls2.ValueField = "TokanId";
                ls2.DataBind();

            }
        }
        dt.Reset();
        dt1.Reset();
    }





   
   
    protected void Select_HeaderList(object sender, EventArgs e)
    {
        ls3.Items.Clear();
        ls2.Items.Clear();
        //lbltokan.Visible = true;
        ls2.Visible = true;
        GetTokanList();
        btnsave.Text = "Save";

    }

 

    #endregion
    protected void Click_Savedata(object sender, EventArgs e)
    {

        if (btnsave.Text == "Save")
        {
            if (ls1.SelectedItems.Count > 0 && ls3.Items.Count > 0)
            {
                string listvalue = "";
                string listvaluetext = "";
                for (int i = 0; i <= ls3.Items.Count - 1; i++)
                {


                    string list = ls3.Items[i].Value.ToString();
                    string list1 = ls3.Items[i].Text.ToString();

                    if (listvalue == "")
                    {
                        listvalue = listvalue + list;
                        listvaluetext = listvaluetext + list1;
                    }
                    else
                    {

                        listvalue = listvalue + "," + list;
                        listvaluetext = listvaluetext + "," + list1;
                    }
                }

                da.AddHeaderTokanMapping(Convert.ToInt32(ls1.SelectedItem.Value), listvalue,listvaluetext, Convert.ToInt32(LogNo), UserLogID);
                ASPxGridView1.DataBind();
                ASPxPageControl1.ActiveTabIndex = 0;
                ls1.Items.Clear();
                ls2.Items.Clear();
                ls3.Items.Clear();
                headerbind(); 
               // System.Windows.Forms.MessageBox.Show("Data Added Successfully");
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Data Added Successfully!');", true);
            }
            else
            {
               // System.Windows.Forms.MessageBox.Show("Please Select Tokan");
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select Tokan!');", true);

            }
        }
        else if(btnsave.Text=="Edit")
        {
            string listvalue1 = "";
            string listvaluetext1 = "";
            if (ls2.Items.Count>0)
            {
               
              
                for (int i = 0; i <= ls2.Items.Count - 1; i++)
                {


                    string list = ls2.Items[i].Value.ToString();
                    string list1 = ls2.Items[i].Text.ToString();

                    if (listvalue1 == "")
                    {
                        listvalue1 = listvalue1 + list;
                        listvaluetext1 = listvaluetext1 + list1;
                    }
                    else
                    {

                        listvalue1 = listvalue1 + "," + list;
                        listvaluetext1 = listvaluetext1 + "," + list1;
                    }
                }

            }

            dt = da.CheckExistTokanMapping(listvaluetext1, Convert.ToInt32(Label1.Text));
            if (dt.Rows.Count > 0)
            {
               // System.Windows.Forms.MessageBox.Show("Edit is not possible, Tokan is Already in Use..");

                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Edit is not possible, Tokan is Already in Use..');", true);
            }
            else
            {
                if (ls1.SelectedItems.Count > 0 )
                {
                    string listvalue = "";
                    string listvaluetext = "";
                    for (int i = 0; i <= ls3.Items.Count - 1; i++)
                    {


                        string list = ls3.Items[i].Value.ToString();
                        string list1 = ls3.Items[i].Text.ToString();

                        if (listvalue == "")
                        {
                            listvalue = listvalue + list;
                            listvaluetext = listvaluetext + list1;
                        }
                        else
                        {

                            listvalue = listvalue + "," + list;
                            listvaluetext = listvaluetext + "," + list1;
                        }
                    }

                    da.EditHeaderTokanMapping(Convert.ToInt32(Label1.Text), listvalue, listvaluetext, Convert.ToInt32(LogNo), UserLogID, Convert.ToInt32(Label2.Text));
                    btnsave.Text = "Save";
                    ASPxGridView1.DataBind();
                    ASPxPageControl1.ActiveTabIndex = 0;
                    ls1.Items.Clear();
                    ls2.Items.Clear();
                    ls3.Items.Clear();
                    headerbind();

                   // System.Windows.Forms.MessageBox.Show("Data Edited Successfully");
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Data Edited Successfully');", true);
                    //   ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Data Edited Successfully" + "');", true);
                }
            }
        }
    }


    protected void Click_resetdata(object sender, EventArgs e)
    {
        ls1.Items.Clear();
        ls2.Items.Clear();
        ls3.Items.Clear();
        headerbind();
        GetTokanList();
    }



    protected void CmdEdit_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
    {
        ls1.Items.Clear();
        ls2.Items.Clear();
        ls3.Items.Clear();
        int FieldTripID = Convert.ToInt32(e.CommandArgument);
        Label2.Text = e.CommandArgument.ToString();


        dt =da.TokanHeaderSelectById(FieldTripID);
        string Header=dt.Rows[0]["HeaderId"].ToString();
        string HeaderName= dt.Rows[0]["Name"].ToString();
        Label1.Text = Header;
     ls1.Items.Add(HeaderName, Header).Selected=true;
        
// ls1.Items.Add(new ListBoxItem(HeaderName, Header));
        ls1.Items[0].Selected = true;
        //TokanId = TokanId.Replace(","," "+"or"+" "+ "TokanId" + "=");
        string TokanId = dt.Rows[0]["TokanId"].ToString();
        dt1 = da.GetAvailableTokanCreationByString(TokanId);
        if(dt1.Rows.Count>0)
             {
            ls3.DataSource = dt1;
            ls3.TextField = "TokanName";
            ls3.ValueField = "TokanId";
            ls3.DataBind();
        }

        dt2 = da.GetAvailableTokanCreationaAfterMapping(TokanId);
        if (dt2.Rows.Count > 0)
        {
            ls2.DataSource = dt2;
            ls2.TextField = "TokanName";
            ls2.ValueField = "TokanId";
            ls2.DataBind();
        }

        btnsave.Text = "Edit";

        //ls1.SelectedItem.Value = Header;


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