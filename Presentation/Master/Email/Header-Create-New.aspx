<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Header-Create-New.aspx.cs" Inherits="Presentation_Master_Email_Header_Create_New" %>

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
    <strong>
        <asp:Label ID="lbmsg" runat="server" ClientIDMode="Static"></asp:Label></strong>
    <div class="wrapper row-offcanvas row-offcanvas-left">
        <section class="content">




            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                <ContentTemplate>
                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"
                        GridViewID="ASPxGridView1">
                    </dx:ASPxGridViewExporter>

                    <asp:Label ID="lbcusid" Visible="false" Text="0" runat="server"></asp:Label>
                    <asp:Label ID="lbpartycat" Visible="false" Text="0" runat="server"></asp:Label>
                    <asp:Label ID="lbid" Visible="false" Text="0" runat="server"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="row">
                <div class="col-md-12">

                    <h2>Email Create</h2>

                </div>
            </div>



            <div class="row">
                <div class="col-md-12" style="width: 95%; margin-left: 25px; margin-top: 35px">

                    <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                        <ContentTemplate>





                            <dx:ASPxPageControl EnableHierarchyRecreation="false" ID="ASPxPageControl1" CssClass="tabsB" align="left" runat="server"
                                ActiveTabIndex="0">
                                <TabPages>
                                    <dx:TabPage Text="Add">
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl1" runat="server">

                                                <div class="row ">
                                                    <dx:ASPxLabel ID="lblerror" runat="server" Visible="false" Style="margin-left: 15px"></dx:ASPxLabel>
                                                    <div class="col-md-12">




                                                        <div class="row">



                                                            <div class="col-md-3">

                                                                <label>Header:*</label>
                                                                <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtheader" MaxLength="200" TabIndex="2" runat="server" CssClass="form-control" required></asp:TextBox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>


                                                            <div class="col-md-3">

                                                                <label>Email:</label>
                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                    <ContentTemplate>
                                                                        <dx:ASPxCheckBox ID="ckemail" runat="server"></dx:ASPxCheckBox>

                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>




                                                            <div class="col-md-3">

                                                                <label>SMS:</label>
                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                    <ContentTemplate>
                                                                        <dx:ASPxCheckBox ID="cksms" runat="server"></dx:ASPxCheckBox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>














                                                        </div>






                                                        <div class="row" style="margin-top: 15px; margin-left: 0px">
                                                            <dx:ASPxButton ID="btnsave" runat="server" TabIndex="12" Text="Save" CssClass="btn-success" OnClick="Click_btnsave"></dx:ASPxButton>
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
                                                                                            <dx:ASPxButton ID="btnPdfExport" runat="server" Text="Export to PDF" UseSubmitBehavior="False" CssClass="listin btn" Theme="Default" OnClick="btnPdfExport_Click" EnableTheming="false" />

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


                                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                    <ContentTemplate>


                                                                        <dx:ASPxGridView Settings-ShowGroupPanel="true" SettingsBehavior-AllowDragDrop="true" SettingsBehavior-AllowGroup="true" OnDataBinding="ASPxGridView1_DataBinding" SettingsBehavior-AllowSort="true" ID="ASPxGridView1" runat="server" align="left"
                                                                            AutoGenerateColumns="False" EnablePagingCallbackAnimation="True"
                                                                            KeyFieldName="messagetemplateid" Width="100%" EnableTheming="True" Theme="Office2010Black">
                                                                            <TotalSummary>
                                                                                <dx:ASPxSummaryItem FieldName="Size" SummaryType="Sum" />
                                                                            </TotalSummary>
                                                                            <GroupSummary>
                                                                                <dx:ASPxSummaryItem SummaryType="Count" />
                                                                            </GroupSummary>
                                                                            <Columns>







                                                                                <dx:GridViewDataTextColumn Caption="HeaderName" FieldName="name" Width="100px"
                                                                                    ShowInCustomizationForm="True" VisibleIndex="2">
                                                                                    <Settings AllowDragDrop="True" AutoFilterCondition="Contains"
                                                                                        ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True"
                                                                                        ShowInFilterControl="True" />
                                                                                    <Settings AllowDragDrop="True" AutoFilterCondition="Contains"
                                                                                        ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True"
                                                                                        ShowInFilterControl="True" />
                                                                                    <Settings AllowDragDrop="True" AutoFilterCondition="Contains"
                                                                                        ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True"
                                                                                        ShowInFilterControl="True" />
                                                                                    <Settings AllowDragDrop="True" AutoFilterCondition="Contains"
                                                                                        ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True"
                                                                                        ShowInFilterControl="True" />
                                                                                </dx:GridViewDataTextColumn>


                                                                                <dx:GridViewDataTextColumn VisibleIndex="9" Caption="For Email" Width="50px">
                                                                                    <DataItemTemplate>
                                                                                        <dx:ASPxCheckBox ID="chbox1" runat="server" Enabled="false" Checked='<%# Eval("Email").ToString() == "True" ? true : false  %>'>
                                                                                        </dx:ASPxCheckBox>
                                                                                    </DataItemTemplate>
                                                                                    <Settings AllowDragDrop="True" AutoFilterCondition="Contains"
                                                                                        ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True"
                                                                                        ShowInFilterControl="True" />
                                                                                   
                                                                                </dx:GridViewDataTextColumn>
                                                                                <dx:GridViewDataTextColumn VisibleIndex="9" Caption="For SMS" Width="50px">
                                                                                    <DataItemTemplate>
                                                                                        <dx:ASPxCheckBox ID="chbox1" runat="server" Enabled="false" Checked='<%# Eval("SMS").ToString() == "True" ? true : false  %>'>
                                                                                        </dx:ASPxCheckBox>
                                                                                    </DataItemTemplate>
                                                                                    <Settings AllowDragDrop="True" AutoFilterCondition="Contains"
                                                                                        ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True"
                                                                                        ShowInFilterControl="True" />
                                                                                   
                                                                                </dx:GridViewDataTextColumn>

                                                                                <dx:GridViewDataTextColumn Settings-AllowDragDrop="True" Caption="Edit" ReadOnly="True" Width="100px"
                                                                                    VisibleIndex="10">
                                                                                    <Settings AllowDragDrop="True" />
                                                                                    <DataItemTemplate>
                                                                                        <asp:LinkButton ID="CmdEdit" EnableViewState="false" runat="server"
                                                                                            CommandArgument='<%# Eval("messagetemplateid") %>' OnCommand="CmdEdit_Command"
                                                                                            Text='Edit'></asp:LinkButton>
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
                                                                            <Settings ShowFilterRow="True" />
                                                                            <SettingsDataSecurity AllowDelete="False" AllowEdit="False"
                                                                                AllowInsert="False" />
                                                                            <Settings ShowFilterRow="True" />
                                                                            <SettingsDataSecurity AllowDelete="False" AllowEdit="False"
                                                                                AllowInsert="False" />
                                                                            <Settings ShowFilterRow="True" />
                                                                            <SettingsDataSecurity AllowDelete="False" AllowEdit="False"
                                                                                AllowInsert="False" />
                                                                            <Styles>
                                                                                <RowHotTrack BackColor="#E9E9E9">
                                                                                </RowHotTrack>
                                                                            </Styles>
                                                                        </dx:ASPxGridView>






                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>




                                                            </div>
                                                            <!-- /.box-body -->
                                                        </div>
                                                        <!-- /.box -->
                                                    </div>
                                                </div>
                                                <!-- .row Tab-2 -->




                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                </TabPages>
                            </dx:ASPxPageControl>



                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>




        </section>
    </div>

</asp:Content>


