<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Send_Message.aspx.cs" Inherits="Presentation_Master_Email_Send_Message" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v18.1, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>










<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">


    <br />
    <br />
    <br />

    <asp:UpdatePanel ID="UpdatePanel44" runat="server">
        <ContentTemplate>
            <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="ASPxGridView1"></dx:ASPxGridViewExporter>

            <clientsideevents callbackcomplete="function(s, e) { lp.Hide(); }" />
            <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" Theme="Material" runat="server" ClientInstanceName="lp" Modal="true" Text="Keep Calm! Getting data shortly... :)">
            </dx:ASPxLoadingPanel>
        </ContentTemplate>
    </asp:UpdatePanel>









    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <div class="row well" style="margin: auto">
                <div class="col-md-3">
                    <label>From Date</label>

                    <dx:ASPxDateEdit ID="txtfromdate" runat="server" EditFormat="Custom" Width="250px" EditFormatString="dd/MM/yy"></dx:ASPxDateEdit>

                </div>

                <div class="col-md-3">
                    <label>To Date</label>

                    <dx:ASPxDateEdit ID="txttodate" runat="server" EditFormat="Custom" Width="250px" EditFormatString="dd/MM/yy"></dx:ASPxDateEdit>

                </div>


                <div class="col-md-1">
                    <label></label>
                    <label></label>
                    <dx:ASPxButton ID="CmdSearch" runat="server" AutoPostBack="False" Text="Search" ClientInstanceName="CmdSearch" CssClass="btn btn-info" OnClick="CmdSearch_Click">
                        <ClientSideEvents Click="function(s, e) { lp.Show();}" />
                    </dx:ASPxButton>


                </div>

                <div class="col-md-2">
                    <asp:RadioButton ID="rddefault" runat="server" GroupName="quota" Text="Fifo" CssClass="radio" Visible="False" />
                </div>


            </div>

        </ContentTemplate>
    </asp:UpdatePanel>





    <br />
    <div class="col-sm-12">
        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
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
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPdfExport" />
                <asp:PostBackTrigger ControlID="btnXlsExport" />
                <asp:PostBackTrigger ControlID="btnRtfExport" />
                <asp:PostBackTrigger ControlID="btnXlsxExport" />
                <asp:PostBackTrigger ControlID="btnCsvExport" />

            </Triggers>
        </asp:UpdatePanel>
    </div>
    <br />
    <br />
    <div class="col-sm-12">
        <dx:ContentControl ID="ContentControl2" runat="server">
            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                <ContentTemplate>
                    <dx:ASPxGridView Settings-ShowGroupPanel="true" SettingsBehavior-AllowDragDrop="true" SettingsBehavior-AllowGroup="true" OnDataBinding="ASPxGridView1_DataBinding" SettingsBehavior-AllowSort="true" ID="ASPxGridView1" runat="server" align="left"
                        AutoGenerateColumns="False" EnablePagingCallbackAnimation="True"
                        KeyFieldName="QueuedMessageID" Width="100%" EnableTheming="True" Theme="Office2010Black">
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="Size" SummaryType="Sum" />
                        </TotalSummary>
                        <GroupSummary>
                            <dx:ASPxSummaryItem SummaryType="Count" />
                        </GroupSummary>
                        <Columns>



                            <dx:GridViewDataTextColumn Caption="Module" FieldName="Perpose" Width="200px"
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



                            <dx:GridViewDataTextColumn Caption="Mobile" FieldName="To" Width="100px"
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
                            <dx:GridViewDataTextColumn Caption="Display Name" FieldName="ToName" Width="400px"
                                ShowInCustomizationForm="True" VisibleIndex="3">
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



                            <dx:GridViewDataDateColumn Caption="Send Time" FieldName="SendTime" Width="200px"
                                ShowInCustomizationForm="True" VisibleIndex="4">
                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm">
                                </PropertiesDateEdit>
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
                            </dx:GridViewDataDateColumn>



                            <dx:GridViewDataDateColumn Caption="Expiry Date" FieldName="ExpiryDate" Width="100px"
                                ShowInCustomizationForm="True" VisibleIndex="5">
                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                                </PropertiesDateEdit>
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
                            </dx:GridViewDataDateColumn>



                            <dx:GridViewDataTextColumn Settings-AllowDragDrop="True" Caption="View Full SMS" ReadOnly="True" Width="100px"
                                VisibleIndex="10">
                                <Settings AllowDragDrop="True" />
                                <DataItemTemplate>
                                    <asp:LinkButton ID="CmdEdit" EnableViewState="false" runat="server"
                                        CommandArgument='<%# Eval("QueuedMessageID") %>' OnCommand="CmdDelete_Command"
                                        Text='SMS Text'></asp:LinkButton>
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
        </dx:ContentControl>

    </div>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>

            <dx:ASPxPopupControl runat="server" CloseAction="CloseButton" Modal="true" AllowDragging="True" ID="ASPxPopupControl1" Theme="Metropolis" PopupElementID="CmdSubmit" ClientInstanceName="popup" Width="600px" Height="300px" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" HeaderText="Message Body">
                <ContentCollection>

                    <dx:PopupControlContentControl runat="server">

                        <asp:TextBox ID="txtMessageBody" Width="100%" Height="100%" TextMode="MultiLine" Enabled="false" runat="server"></asp:TextBox>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
