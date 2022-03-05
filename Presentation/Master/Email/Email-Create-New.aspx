<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Email-Create-New.aspx.cs" Inherits="Email_Create_New" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">


    <style type="text/css">
        .ui-datepicker {
            font-size: 8pt !important;
        }

        .tabsB {
            width: 100%;
        }
    </style>

    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }



        function isHostKey(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;
            if (charCode == 46)
                return true
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

    </script>

    <script type="text/javascript">
        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblerror.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
     <asp:Label ID="Label1" Visible="false" Text="0" runat="server"></asp:Label>
    <strong> <asp:Label ID="lbmsg" runat="server" ClientIDMode="Static"></asp:Label></strong>
    <div class="wrapper row-offcanvas row-offcanvas-left">
        <section class="content">
    

    

       <asp:UpdatePanel ID="UpdatePanel17" runat="server">
        <contenttemplate>
                 	<dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"
				GridViewID="ASPxGridView1">
			</dx:ASPxGridViewExporter>

             <asp:Label ID="lbcusid" Visible="false" Text="0" runat="server"></asp:Label>
       <asp:Label ID="lbpartycat" Visible="false" Text="0" runat="server"></asp:Label>
    <asp:Label ID="lbid" Visible="false" Text="0" runat="server"></asp:Label>
            </contenttemplate></asp:UpdatePanel>
  <div class="row">
    <div class="col-md-12">

    <h2>Email Create</h2>

  </div>
</div>



<div class="row">
    <div class="col-md-12" style="width:95%;margin-left:25px;margin-top:35px">

 <asp:UpdatePanel ID="UpdatePanel24" runat="server">
        <contenttemplate>





<dx:ASPxPageControl  EnableHierarchyRecreation="false" ID="ASPxPageControl1" CssClass="tabsB" align="left" runat="server" 
         ActiveTabIndex="0">
        <TabPages>
            <dx:TabPage Text="Add">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl1" runat="server">
    
<div class="row ">
    <dx:ASPxLabel ID="lblerror" runat="server" Visible="false" Style="margin-left:15px"></dx:ASPxLabel>
   <div class="col-md-12">




 <div class="row">
          


     

       
         
           

        



              <div class="col-md-2">
         
            <label>Email Id:*</label>
          <asp:UpdatePanel ID="UpdatePanel18" runat="server" ><contenttemplate>
          <asp:TextBox ID="txtemail" TabIndex="2" runat="server" MaxLength="255"  CssClass="form-control" required></asp:TextBox></contenttemplate></asp:UpdatePanel>
                  </div>


         

        

         <div class="col-md-2">
         
            <label>Display Name:*</label>
          <asp:UpdatePanel ID="UpdatePanel1" runat="server" ><contenttemplate>
          <asp:TextBox ID="txtdisplay" TabIndex="3" runat="server" MaxLength="255"  CssClass="form-control" required></asp:TextBox></contenttemplate></asp:UpdatePanel>
                  </div>


       
              


        

         
       <div class="col-md-2">
         
            <label>User Id:*</label>
          <asp:UpdatePanel ID="UpdatePanel4" runat="server" ><contenttemplate>
          <asp:TextBox ID="txtuserid" TabIndex="4" runat="server" MaxLength="255"  CssClass="form-control" required></asp:TextBox></contenttemplate></asp:UpdatePanel>
                  </div>


         

        

         <div class="col-md-2">
         
            <label>Password:*</label>
          <asp:UpdatePanel ID="UpdatePanel6" runat="server" ><contenttemplate>
          <asp:TextBox ID="txtpassword" TabIndex="5" runat="server"  CssClass="form-control" MaxLength="255" required></asp:TextBox></contenttemplate></asp:UpdatePanel>
                  </div>


       
             <div class="col-md-2">
         
            <label>Host:*</label>
          <asp:UpdatePanel ID="UpdatePanel2" runat="server" ><contenttemplate>
          <asp:TextBox ID="txthost" TabIndex="6" runat="server"   CssClass="form-control"  MaxLength="15" required></asp:TextBox></contenttemplate></asp:UpdatePanel>
                  </div>
 

        
              


          







       

       </div>

       <div class="row">
          


     

       
       

          <div class="col-md-2">
         
            <label>Port:*</label>
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" ><contenttemplate>
          <asp:TextBox ID="txtport" TabIndex="7" runat="server"  CssClass="form-control" onkeypress="return isNumberKey(event)" MaxLength="3" required ></asp:TextBox></contenttemplate></asp:UpdatePanel>
                  </div>

            


       
              


             <div class="col-md-2">
         
            <label>Enable SSL:</label>
          <asp:UpdatePanel ID="UpdatePanel7" runat="server" ><contenttemplate>
         
<dx:ASPxCheckBox ID="ckenablessl" runat="server" TabIndex="8" ></dx:ASPxCheckBox>
                                                             </contenttemplate></asp:UpdatePanel>
                  </div>


          


          <div class="col-md-2">
         
            <label>User Default Credential:</label>
          <asp:UpdatePanel ID="UpdatePanel8" runat="server" ><contenttemplate>
         <dx:ASPxCheckBox ID="ckuserdefault" runat="server" TabIndex="9" ></dx:ASPxCheckBox>

                                                             </contenttemplate></asp:UpdatePanel>
                  </div>


         
            <div class="col-md-2">
         
            <label>Make as Default:</label>
          <asp:UpdatePanel ID="UpdatePanel13" runat="server" ><contenttemplate>
         <dx:ASPxCheckBox ID="ckdefaultemail" runat="server" TabIndex="10" ></dx:ASPxCheckBox>

                                                             </contenttemplate></asp:UpdatePanel>
                  </div>


              <div class="col-md-2">
         
            <label>Limits of Email Id:*</label>
          <asp:UpdatePanel ID="UpdatePanel9" runat="server" ><contenttemplate>
          <asp:TextBox ID="txtemailLimit" TabIndex="11" runat="server"   CssClass="form-control" onkeypress="return isNumberKey(event)" MaxLength="5" required></asp:TextBox></contenttemplate></asp:UpdatePanel>
                  </div>
 

     
            <div class="col-md-2">
         
            <label>Deactivate:</label>
          <asp:UpdatePanel ID="UpdatePanel10" runat="server" ><contenttemplate>
         <dx:ASPxCheckBox ID="ckdeactivate" runat="server" TabIndex="12" ></dx:ASPxCheckBox>

                                                             </contenttemplate></asp:UpdatePanel>
                  </div>

 


 <div class="col-md-3">

       </div>

       </div>


       

        <div class="row" style="margin-top:15px;margin-left:0px">
            <dx:ASPxButton ID="btnsave" runat="server" TabIndex="12" Text="Save" CssClass="btn-success"  OnClick="Click_btnsave"></dx:ASPxButton>
             <dx:ASPxButton ID="btnreset" runat="server" TabIndex="13" Text="Reset" CssClass="btn-primary" OnClick="Click_btnreset"></dx:ASPxButton>

            </div>
       </div>

</div>
 </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="List">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl2" runat="server">



                         <div class="row">
                        <div class="col-xs-12">
                           <div class="box"> 
                           
                           
                               <div class="box-body table-responsive"> 





 <asp:UpdatePanel ID="UpdatePanel23" runat="server">     
                       <Triggers>
        <asp:PostBackTrigger ControlID="btnPdfExport" />
          <asp:PostBackTrigger ControlID="btnXlsExport"  />
            <asp:PostBackTrigger ControlID="btnRtfExport"  />
              <asp:PostBackTrigger ControlID="btnXlsxExport"  />
                <asp:PostBackTrigger ControlID="btnCsvExport"  />

</Triggers>
<contenttemplate>
<table class="BottomMargin">
        <tr>
            <td style="padding-right: 4px">
               <asp:UpdatePanel ID="UpdatePanel12" runat="server">
  

<contenttemplate>
               <dx:ASPxButton ID="btnPdfExport" runat="server" Text="Export to PDF" UseSubmitBehavior="False" CssClass="listin" Theme="Default" OnClick="btnPdfExport_Click" EnableTheming="false"
                     /> 
                    
                             </contenttemplate>
                                
            </asp:UpdatePanel>
            
            
                </td>
            <td style="padding-right: 4px">
                <dx:ASPxButton ID="btnXlsExport" runat="server" Text="Export to XLS" UseSubmitBehavior="False" CssClass="listin" Theme="Default"  OnClick="btnXlsExport_Click" EnableTheming="false"
                     />
            </td>
            <td style="padding-right: 4px">
                <dx:ASPxButton ID="btnXlsxExport" runat="server" Text="Export to XLSX" UseSubmitBehavior="False" CssClass="listin" Theme="Default" OnClick="btnXlsxExport_Click" EnableTheming="false"
                     />
            </td>
            <td style="padding-right: 4px">
                <dx:ASPxButton ID="btnRtfExport" runat="server" Text="Export to RTF" UseSubmitBehavior="False" CssClass="listin" Theme="Default" OnClick="btnRtfExport_Click" EnableTheming="false"
                   />
            </td>
            <td>
                <dx:ASPxButton ID="btnCsvExport" runat="server" Text="Export to CSV" UseSubmitBehavior="False" CssClass="listin" Theme="Default"  OnClick="btnCsvExport_Click" EnableTheming="false"
                    />
            </td>
        </tr>
    </table>
    <br /><br />


 </contenttemplate></asp:UpdatePanel>


 <asp:UpdatePanel ID="UpdatePanel5" runat="server"> <contenttemplate> 


<dx:ASPxGridView  Settings-ShowGroupPanel="true" SettingsBehavior-AllowDragDrop="true"   SettingsBehavior-AllowGroup="true" OnDataBinding="ASPxGridView1_DataBinding"  SettingsBehavior-AllowSort="true" ID="ASPxGridView1" runat="server" align="left" 
                    AutoGenerateColumns="False" EnablePagingCallbackAnimation="True" 
                    KeyFieldName="EmailAccoutId"  Width="100%" EnableTheming="True" Theme="Office2010Black">
                    <TotalSummary>
                        <dx:ASPxSummaryItem FieldName="Size" SummaryType="Sum" />
                    </TotalSummary>
                    <GroupSummary>
                        <dx:ASPxSummaryItem SummaryType="Count" />
                    </GroupSummary>
                    <Columns>
                      



                        


                        <dx:GridViewDataTextColumn Caption="Email" FieldName="Email" Width="100px"
                            ShowInCustomizationForm="True" VisibleIndex="2">
                            <Settings AllowDragDrop="True" AutoFilterCondition="Contains" 
                                ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" 
                                ShowInFilterControl="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Display Name" FieldName="DisplayName" Width="400px"
                            ShowInCustomizationForm="True" VisibleIndex="3">
                            <Settings AllowDragDrop="True" AutoFilterCondition="Contains" 
                                ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" 
                                ShowInFilterControl="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                        </dx:GridViewDataTextColumn>



                        <dx:GridViewDataTextColumn Caption="Host" FieldName="Host" Width="200px"
                            ShowInCustomizationForm="True" VisibleIndex="4">
                            <Settings AllowDragDrop="True" AutoFilterCondition="Contains" 
                                ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" 
                                ShowInFilterControl="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                        </dx:GridViewDataTextColumn>


                        
                        <dx:GridViewDataTextColumn Caption="Port" FieldName="Port" Width="100px"
                            ShowInCustomizationForm="True" VisibleIndex="5">
                            <Settings AllowDragDrop="True" AutoFilterCondition="Contains" 
                                ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" 
                                ShowInFilterControl="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                        </dx:GridViewDataTextColumn>

                         <dx:GridViewDataTextColumn Caption="User Name" FieldName="Username" Width="100px"
                            ShowInCustomizationForm="True" VisibleIndex="6">
                            <Settings AllowDragDrop="True" AutoFilterCondition="Contains" 
                                ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" 
                                ShowInFilterControl="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                        </dx:GridViewDataTextColumn>

                         <dx:GridViewDataTextColumn Caption="Password" FieldName="Password" Width="100px"
                            ShowInCustomizationForm="True" VisibleIndex="7">
                            <Settings AllowDragDrop="True" AutoFilterCondition="Contains" 
                                ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" 
                                ShowInFilterControl="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Email Limit" FieldName="EmailLimit" Width="100px"
                            ShowInCustomizationForm="True" VisibleIndex="7">
                            <Settings AllowDragDrop="True" AutoFilterCondition="Contains" 
                                ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" 
                                ShowInFilterControl="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                        </dx:GridViewDataTextColumn>
                          <dx:GridViewDataTextColumn VisibleIndex="8" Caption="Enable SSL" Width="50px">
                                                                                            <DataItemTemplate>
                                                                                                <dx:ASPxCheckBox ID="chbox" runat="server" enabled="false" Checked='<%# Eval("EnableSSL").ToString() == "True" ? true : false  %>'>
                                                                                                </dx:ASPxCheckBox>
                                                                                            </DataItemTemplate>
                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains" 
                                ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" 
                                ShowInFilterControl="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                                                                                        </dx:GridViewDataTextColumn>
                      
                           <dx:GridViewDataTextColumn VisibleIndex="9" Caption="Use Default Credentials" Width="50px">
                                                                                            <DataItemTemplate>
                                                                                                <dx:ASPxCheckBox ID="chbox1" runat="server" enabled="false" Checked='<%# Eval("UseDefaultCredentials").ToString() == "True" ? true : false  %>'>
                                                                                                </dx:ASPxCheckBox>
                                                                                            </DataItemTemplate>
                                 <Settings AllowDragDrop="True" AutoFilterCondition="Contains" 
                                ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" 
                                ShowInFilterControl="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                                                                                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="9" Caption="Default Email" Width="50px">
                                                                                            <DataItemTemplate>
                                                                                                <dx:ASPxCheckBox ID="chbox1" runat="server" enabled="false" Checked='<%# Eval("DefaultEmail").ToString() == "True" ? true : false  %>'>
                                                                                                </dx:ASPxCheckBox>
                                                                                            </DataItemTemplate>
                                 <Settings AllowDragDrop="True" AutoFilterCondition="Contains" 
                                ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" 
                                ShowInFilterControl="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                            <settings allowdragdrop="True" autofiltercondition="Contains" 
                                showfilterrowmenu="True" showfilterrowmenulikeitem="True" 
                                showinfiltercontrol="True" />
                                                                                        </dx:GridViewDataTextColumn>
                         
                          <dx:GridViewDataTextColumn  Settings-AllowDragDrop="True" Caption="Edit"   ReadOnly="True" Width="100px"
                VisibleIndex="10">
                              <Settings AllowDragDrop="True" />
                     <DataItemTemplate>
                    <asp:LinkButton ID="CmdEdit"  EnableViewState="false" runat="server" 
                        CommandArgument='<%# Eval("EmailAccountId") %>' OnCommand="CmdEdit_Command"
                        Text='Edit' ></asp:LinkButton>
                </DataItemTemplate>

                     </dx:GridViewDataTextColumn>

                    </Columns>
                    <SettingsBehavior EnableRowHotTrack="True" 
                        ProcessFocusedRowChangedOnServer="True" />
                    <SettingsPager>
                        <PageSizeItemSettings Items="10, 20, 50, 100, 500" Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>

    
     <Settings ShowGroupPanel="True" ShowFooter="True" ShowGroupFooter="VisibleIfExpanded" />
           
                      


              

                    <Settings ShowFilterRow="True" />
                    <SettingsCommandButton>
                        <ShowAdaptiveDetailButton ButtonType="Image">
                        </ShowAdaptiveDetailButton>
                        <HideAdaptiveDetailButton ButtonType="Image">
                        </HideAdaptiveDetailButton>
                    </SettingsCommandButton>
                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                        AllowInsert="False" />
                    <settings showfilterrow="True" />
                    <settingsdatasecurity allowdelete="False" allowedit="False" 
                        allowinsert="False" />
                    <settings showfilterrow="True" />
                    <settingsdatasecurity allowdelete="False" allowedit="False" 
                        allowinsert="False" />
                    <settings showfilterrow="True" />
                    <settingsdatasecurity allowdelete="False" allowedit="False" 
                        allowinsert="False" />
                    <Styles>
                        <RowHotTrack BackColor="#E9E9E9">
                        </RowHotTrack>
                    </Styles>
                </dx:ASPxGridView>




               

 </contenttemplate></asp:UpdatePanel>




       </div><!-- /.box-body -->
                         </div>  <!-- /.box -->
                      </div>
                   </div>   <!-- .row Tab-2 -->




 </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>



     </contenttemplate></asp:UpdatePanel>
     
   </div>
</div>




</section>
    </div>

</asp:Content>
