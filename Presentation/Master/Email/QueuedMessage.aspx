<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="QueuedMessage.aspx.cs" Inherits="Presentation_Master_Email_QueuedMessage" %>


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
          
            <dx:TabPage Text="List">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl2" runat="server">



                         <div class="row">
                        <div class="col-xs-12">
                           <div class="box"> 
                           
                           
                               <div class="box-body table-responsive"  style="height:750px"> 





  <div class="col-xs-8">


                                                                    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="btnPdfExport" />
                                                                            <asp:PostBackTrigger ControlID="btnXlsExport" />
                                                                            <asp:PostBackTrigger ControlID="btnRtfExport" />
                                                                            <asp:PostBackTrigger ControlID="btnXlsxExport" />
                                                                            <asp:PostBackTrigger ControlID="btnCsvExport" />

                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <table class="BottomMargin">
                                                                                <tr>
                                                                                    <td style="padding-right: 4px">
                                                                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">


                                                                                            <ContentTemplate>
                                                                                                <dx:ASPxButton ID="btnPdfExport" runat="server" Text="Export to PDF" UseSubmitBehavior="False" CssClass="listin" Theme="Default" OnClick="btnPdfExport_Click" EnableTheming="false" />

                                                                                            </ContentTemplate>

                                                                                        </asp:UpdatePanel>


                                                                                    </td>
                                                                                    <td style="padding-right: 4px">
                                                                                        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="Export to XLS" UseSubmitBehavior="False" CssClass="listin" Theme="Default" OnClick="btnXlsExport_Click" EnableTheming="false" />
                                                                                    </td>
                                                                                    <td style="padding-right: 4px">
                                                                                        <dx:ASPxButton ID="btnXlsxExport" runat="server" Text="Export to XLSX" UseSubmitBehavior="False" CssClass="listin" Theme="Default" OnClick="btnXlsxExport_Click" EnableTheming="false" />
                                                                                    </td>
                                                                                    <td style="padding-right: 4px">
                                                                                        <dx:ASPxButton ID="btnRtfExport" runat="server" Text="Export to RTF" UseSubmitBehavior="False" CssClass="listin" Theme="Default" OnClick="btnRtfExport_Click" EnableTheming="false" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <dx:ASPxButton ID="btnCsvExport" runat="server" Text="Export to CSV" UseSubmitBehavior="False" CssClass="listin" Theme="Default" OnClick="btnCsvExport_Click" EnableTheming="false" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <br />
                                                                            <br />


                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>

                                                                </div>
                                                                <div class="col-xs-4">
                                                                    <asp:Button ID="btndeleteall" OnClientClick="javascript:return confirm('Do you really want to delete?');'yes,no';" CssClass="btn-success" runat="server" Text="Delete All Queue Message" OnClick="btndeleteall_Click">
                                                                        
                                                                    </asp:Button>
                                                                </div>


 <asp:UpdatePanel ID="UpdatePanel5" runat="server"> <contenttemplate> 


<dx:ASPxGridView  Settings-ShowGroupPanel="true" SettingsBehavior-AllowDragDrop="true"   SettingsBehavior-AllowGroup="true" OnDataBinding="ASPxGridView1_DataBinding"  SettingsBehavior-AllowSort="true" ID="ASPxGridView1" runat="server" align="left" 
                    AutoGenerateColumns="False" EnablePagingCallbackAnimation="True" 
                    KeyFieldName="QueuedMessageID"  Width="100%" EnableTheming="True" Theme="Office2010Black">
                    <TotalSummary>
                        <dx:ASPxSummaryItem FieldName="Size" SummaryType="Sum" />
                    </TotalSummary>
                    <GroupSummary>
                        <dx:ASPxSummaryItem SummaryType="Count" />
                    </GroupSummary>
                    <Columns>
                      



                        


                        <dx:GridViewDataTextColumn Caption="Mobile" FieldName="To" Width="100px"
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
                        <dx:GridViewDataTextColumn Caption="Display Name" FieldName="ToName" Width="400px"
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



                        <dx:GridViewDataDateColumn Caption="Sending Time" FieldName="SendTime" Width="200px"
                            ShowInCustomizationForm="True" VisibleIndex="4">
                            <PropertiesDateEdit  DisplayFormatString="dd/MM/yyyy HH:mm">
            </PropertiesDateEdit>
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
                        </dx:GridViewDataDateColumn>


                        
                        <dx:GridViewDataDateColumn Caption="Expiry Date" FieldName="ExpiryDate" Width="100px"
                            ShowInCustomizationForm="True" VisibleIndex="5">
                              <PropertiesDateEdit  DisplayFormatString="dd/MM/yyyy">
            </PropertiesDateEdit>
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
                        </dx:GridViewDataDateColumn>

                         <dx:GridViewDataTextColumn Caption="Body" FieldName="Body" Width="100px" PropertiesTextEdit-EncodeHtml="true" 
                            ShowInCustomizationForm="True" VisibleIndex="6">
                              <PropertiesTextEdit EncodeHtml="False" />
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

                        <dx:GridViewDataTextColumn  Settings-AllowDragDrop="True" Caption="Delete"   ReadOnly="True" Width="100px"
                VisibleIndex="10">
                              <Settings AllowDragDrop="True" />
                     <DataItemTemplate>
                    <asp:LinkButton ID="CmdEdit"  EnableViewState="false" runat="server" 
                        CommandArgument='<%# Eval("QueuedMessageID") %>' OnCommand="CmdDelete_Command" OnClientClick="javascript:return confirm('Do you really want to Delete?');'yes,no'"
                        Text='Delete' ></asp:LinkButton>
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