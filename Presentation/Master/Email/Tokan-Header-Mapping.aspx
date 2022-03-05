<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Tokan-Header-Mapping.aspx.cs" Inherits="Tokan_Header_Mapping" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v18.1, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">

    <script type="text/javascript">
        function AddSelectedItems() {
            MoveSelectedItems(ls2, ls3);
            UpdateButtonState();
        }
        function AddAllItems() {
            MoveAllItems(ls2, ls3);
            UpdateButtonState();
        }
        function RemoveSelectedItems() {
            MoveSelectedItems(ls3, ls2);
            UpdateButtonState();
        }
        function RemoveAllItems() {
            MoveAllItems(ls3, ls2);
            UpdateButtonState();
        }
        function MoveSelectedItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            dstListBox.BeginUpdate();
            var items = srcListBox.GetSelectedItems();
            for (var i = items.length - 1; i >= 0; i = i - 1) {
                dstListBox.AddItem(items[i].text, items[i].value);
                srcListBox.RemoveItem(items[i].index);
            }
            srcListBox.EndUpdate();
            dstListBox.EndUpdate();
        }
        function MoveAllItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            var count = srcListBox.GetItemCount();
            for (var i = 0; i < count; i++) {
                var item = srcListBox.GetItem(i);
                dstListBox.AddItem(item.text, item.value);
            }
            srcListBox.EndUpdate();
            srcListBox.ClearItems();
        }
        function UpdateButtonState() {
            btnMoveAllItemsToRight.SetEnabled(ls2.GetItemCount() > 0);
            btnMoveAllItemsToLeft.SetEnabled(ls3.GetItemCount() > 0);
            btnMoveSelectedItemsToRight.SetEnabled(ls2.GetSelectedItems().length > 0);
            btnMoveSelectedItemsToLeft.SetEnabled(ls3.GetSelectedItems().length > 0);
        }
    </script>

    <style type="text/css">
        .ui-datepicker {
            font-size: 8pt !important;
        }

        .tabsB {
            width: 100%;
        }

      
     
    </style>



    <script type="text/javascript">

        function ShowPopup() {
            debugger
            ASPxClientPopupControlInstance.Show();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Label ID="Label1" Visible="false" runat="server"></asp:Label>
    <asp:Label ID="Label2" Visible="false" runat="server"></asp:Label>
    <div class="wrapper row-offcanvas row-offcanvas-left">
        <div class="content">
            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                <ContentTemplate>
                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"
                        GridViewID="ASPxGridView1">
                    </dx:ASPxGridViewExporter>
                    <dx:ASPxLoadingPanel ID="ASPxLoadingPanel2" runat="server" ClientInstanceName="lp" Modal="true"></dx:ASPxLoadingPanel>
                    <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="ASPxCallback1" OnCallback="ASPxCallback1_Callback">
                        <ClientSideEvents CallbackComplete="function(s, e) { lp.Hide(); }" />
                    </dx:ASPxCallback>
                    <asp:Label ID="lbid" Visible="false" Text="0" runat="server"></asp:Label>
                    <asp:Label ID="lbid2" Visible="false" Text="0" runat="server"></asp:Label>
                    <asp:Label ID="lbledgerid" Visible="false" Text="0" runat="server"></asp:Label>
                    <asp:Label ID="lbsubledgerid" Visible="false" Text="0" runat="server"></asp:Label>
                    <asp:Label ID="lbcashbankid" Visible="false" Text="0" runat="server"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="row">
                <div class="col-md-12">
                    <h2>ADD Email Template</h2>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                        <ContentTemplate>
                            <dx:ASPxPageControl EnableHierarchyRecreation="false" ID="ASPxPageControl1"
                                runat="server" ActiveTabIndex="0" CssClass="tabsB">
                                <TabPages>
                                    <dx:TabPage Text="Add">
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl1" runat="server">
                                                <div class="row ">
                                                    <div class="col-md-2" style="height: 100%;">
                                                        <dx:ASPxLabel runat="server" ID="lblheadererror" Visible="false" Style="color: red"></dx:ASPxLabel>
                                                        <br />
                                                        <dx:ASPxLabel runat="server" Text="Select HEADER" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                        <dx:ASPxListBox runat="server" ID="ls1" Style="height: 300px; width: 200px;" CssClass="list-group-item-heading" Theme="Office2010Black" AutoPostBack="true" OnSelectedIndexChanged="Select_HeaderList"></dx:ASPxListBox>

                                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:mycon %>" SelectCommand="SELECT [Name], [MessageTemplateID] FROM [Gold_MessageTemplate]"></asp:SqlDataSource>
                                                        <%-- <dx:ASPxPopupControl runat="server"></dx:ASPxPopupControl>--%>
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <dx:ASPxLabel ID="lbmsg" runat="server" ForeColor="Red"></dx:ASPxLabel>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2" style="height: 100%; width: 80px; margin-left: -100px">
                                                        <dx:ASPxButton runat="server" Text=">" Style="margin-top: 135px" ID="btnforward" Theme="Office2010Black"></dx:ASPxButton>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2">

                                                        <dx:ASPxLabel runat="server" Text="Select TOKAN" Style="font-weight: bold; font-size: 12px" ID="lbltokan"></dx:ASPxLabel>

                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <%--  <dx:ASPxListBox ID="ls2" runat="server" ClientInstanceName="ls2"
                                                                    Width="450px" JS-Filter-Chk="True" CssClass="form-control js-filter-chk-class" ValueType="System.String"
                                                                    TabIndex="1" SelectionMode="CheckColumn" Theme="Glass" Height="250px" Style="overflow: auto; padding: 0 !important; margin: 0 !important;">
                                                                    <CaptionSettings Position="Top" />
                                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){UpdateButtonState();}" />
                                                                </dx:ASPxListBox>--%>

                                                                <dx:ASPxListBox ID="ls2" ClientInstanceName="ls2" JS-Filter="True" runat="server" Theme="Office2010Black"
                                                                    Style="padding: 8px 0px !important; height: 300px; width: 200px;"
                                                                    Width="250" CssClass="form-control js-filter-class" ValueType="System.String" SelectionMode="CheckColumn"
                                                                    TabIndex="1" Height="100">
                                                                    <CaptionSettings Position="Top" />
                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }"></ClientSideEvents>
                                                                </dx:ASPxListBox>

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>

                                                    </div>
                                                    <div class="col-md-2" style="height: 100%; width: 80px; margin-top: 80px;" id="dvpass" runat="server">
                                                        <dx:ASPxButton ID="btnMoveSelectedItemsToRight" runat="server" ClientInstanceName="btnMoveSelectedItemsToRight" Theme="Material"
                                                            AutoPostBack="False" Text="Add >" Width="130px" ClientEnabled="False"
                                                            ToolTip="Add selected items">
                                                            <ClientSideEvents Click="function(s, e) { AddSelectedItems(); }" />
                                                        </dx:ASPxButton>
                                                        <br />
                                                        <dx:ASPxButton ID="btnMoveAllItemsToRight" runat="server" ClientInstanceName="btnMoveAllItemsToRight" Theme="Material" Style="margin-top: 5px"
                                                            AutoPostBack="False" Text="Add All >>" Width="130px" ToolTip="Add all items">
                                                            <ClientSideEvents Click="function(s, e) { AddAllItems(); }" />
                                                        </dx:ASPxButton>
                                                        <br />

                                                        <dx:ASPxButton ID="btnMoveSelectedItemsToLeft" runat="server" ClientInstanceName="btnMoveSelectedItemsToLeft" Theme="Material" Style="margin-top: 5px"
                                                            AutoPostBack="False" Text="< Remove" Width="130px" ClientEnabled="False"
                                                            ToolTip="Remove selected items">
                                                            <ClientSideEvents Click="function(s, e) { RemoveSelectedItems(); }" />
                                                        </dx:ASPxButton>
                                                        <br />
                                                        <dx:ASPxButton ID="btnMoveAllItemsToLeft" runat="server" ClientInstanceName="btnMoveAllItemsToLeft" Theme="Material" Style="margin-top: 5px"
                                                            AutoPostBack="False" Text="<< Remove All" Width="130px" ClientEnabled="False"
                                                            ToolTip="Remove all items">
                                                            <ClientSideEvents Click="function(s, e) { RemoveAllItems(); }" />
                                                        </dx:ASPxButton>
                                                        <%--<dx:ASPxButton runat="server" Text=">" Style="margin-top: 135px" ID="btnpass" OnClick="click_passdatatonext"></dx:ASPxButton>
                                                        <dx:ASPxButton runat="server" Text="<" Style="margin-top: 5px" ID="btnremove" OnClick="click_passdatatoback"></dx:ASPxButton>--%>
                                                        <br />

                                                    </div>

                                                    <div class="col-md-2" style="margin-left: 110px;" id="dvslected" runat="server">

                                                        <dx:ASPxLabel runat="server" Text="Selected TOKAN" Style="font-weight: bold; font-size: 12px" ID="ASPxLabel1"></dx:ASPxLabel>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <%--<dx:ASPxListBox ID="ls3" runat="server" ClientInstanceName="ls3" Width="100%"
                                                                    Height="240px" SelectionMode="CheckColumn" Caption="Chosen">
                                                                    <CaptionSettings Position="Top" />
                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }"></ClientSideEvents>
                                                                </dx:ASPxListBox>--%>

                                                                <dx:ASPxListBox ID="ls3" ClientInstanceName="ls3" EnableSynchronization="True" JS-Filter="True" runat="server" Theme="Office2010Black"
                                                                    Style="padding: 8px 0px !important; height: 300px; width: 200px; overflow: auto;"
                                                                    Width="250" CssClass="form-control js-filter-class" ValueType="System.String" SelectionMode="CheckColumn"
                                                                    TabIndex="1" Height="100">
                                                                    <CaptionSettings Position="Top" />
                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }"></ClientSideEvents>
                                                                </dx:ASPxListBox>


                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>

                                                <div class="row" style="margin-top: 20px; margin-left: 666px">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <dx:ASPxButton ID="btnsave" runat="server" Text="Save" CssClass="btn-success" OnClick="Click_Savedata"></dx:ASPxButton>
                                                            <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Reset" OnClick="Click_resetdata"></dx:ASPxButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>

                                                <%--.row Tab-1--%>
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


                                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                                    <ContentTemplate>

                                                                        <br />

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
                                                                                            <asp:UpdatePanel ID="UpdatePanel13" runat="server">


                                                                                                <ContentTemplate>
                                                                                                    <dx:ASPxButton ID="btnPdfExport" runat="server" Text="Export to PDF" UseSubmitBehavior="False" CssClass="listin" Theme="Default"
                                                                                                        OnClick="btnPdfExport_Click" />

                                                                                                </ContentTemplate>

                                                                                            </asp:UpdatePanel>


                                                                                        </td>
                                                                                        <td style="padding-right: 4px">
                                                                                            <dx:ASPxButton ID="btnXlsExport" runat="server" Text="Export to XLS" UseSubmitBehavior="False" CssClass="listin" Theme="Default"
                                                                                                OnClick="btnXlsExport_Click" />
                                                                                        </td>
                                                                                        <td style="padding-right: 4px">
                                                                                            <dx:ASPxButton ID="btnXlsxExport" runat="server" Text="Export to XLSX" UseSubmitBehavior="False" CssClass="listin" Theme="Default"
                                                                                                OnClick="btnXlsxExport_Click" />
                                                                                        </td>
                                                                                        <td style="padding-right: 4px">
                                                                                            <dx:ASPxButton ID="btnRtfExport" runat="server" Text="Export to RTF" UseSubmitBehavior="False" CssClass="listin" Theme="Default"
                                                                                                OnClick="btnRtfExport_Click" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <dx:ASPxButton ID="btnCsvExport" runat="server" Text="Export to CSV" UseSubmitBehavior="False" CssClass="listin" Theme="Default" OnClick="btnCsvExport_Click" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <br />
                                                                                <br />


                                                                                <div class="row" style="width: 98%">
                                                                                    <div class="col-md-12">
                                                                                    </div>

                                                                                    <dx:ASPxGridView Settings-ShowGroupPanel="true" SettingsBehavior-AllowDragDrop="true" SettingsBehavior-AllowGroup="true" OnDataBinding="ASPxGridView1_DataBinding" SettingsBehavior-AllowSort="true" ID="ASPxGridView1" runat="server" align="left"
                                                                                        AutoGenerateColumns="False" EnablePagingCallbackAnimation="True"
                                                                                        KeyFieldName="SlNo" Width="100%" EnableTheming="True" Theme="Office2010Black" Style="margin-left: 35px">
                                                                                        <Settings ShowFilterRow="True" />
                                                                                        <SettingsCommandButton>
                                                                                            <ShowAdaptiveDetailButton ButtonType="Image">
                                                                                            </ShowAdaptiveDetailButton>
                                                                                            <HideAdaptiveDetailButton ButtonType="Image">
                                                                                            </HideAdaptiveDetailButton>
                                                                                        </SettingsCommandButton>
                                                                                        <Columns>




                                                                                            <dx:GridViewDataTextColumn Caption="Header Name" FieldName="Name" Width="70px"
                                                                                                ShowInCustomizationForm="True" VisibleIndex="1">
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

                                                                                            <dx:GridViewDataTextColumn Caption="Tokan Names" FieldName="TokanNames" Width="70px"
                                                                                                ShowInCustomizationForm="True" VisibleIndex="1">
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
                                                                                            <dx:GridViewDataTextColumn Settings-AllowDragDrop="True" Caption="Edit" ReadOnly="True" Width="100px"
                                                                                                VisibleIndex="10">
                                                                                                <Settings AllowDragDrop="True" />
                                                                                                <DataItemTemplate>
                                                                                                    <asp:LinkButton ID="CmdEdit" EnableViewState="false" runat="server"
                                                                                                        CommandArgument='<%# Eval("SlNo") %>' OnCommand="CmdEdit_Command"
                                                                                                        Text='Edit'></asp:LinkButton>
                                                                                                </DataItemTemplate>

                                                                                            </dx:GridViewDataTextColumn>
                                                                                        </Columns>
                                                                                    </dx:ASPxGridView>

                                                                                </div>


                                                                                <br />
                                                                                <br />
                                                                                <br />

                                                                                <asp:SqlDataSource ID="SqlDataSourcLedger" runat="server"
                                                                                    ConnectionString="<%$ ConnectionStrings:mycon %>"
                                                                                    SelectCommand="exec Generalledgerselect"></asp:SqlDataSource>

                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
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




        </div>
    </div>

</asp:Content>

