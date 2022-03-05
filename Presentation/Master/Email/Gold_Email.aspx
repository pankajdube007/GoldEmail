<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Gold_Email.aspx.cs" Inherits="Gold_Email" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v18.1, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v18.1, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v18.1, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script src="<%=ResolveUrl("~/Scripts/jquery-3.1.1.min.js")%>"></script>
    <script src="<%=ResolveUrl("~/Scripts/jquery-ui.min.js")%>"></script>
    <style type="text/css">
        .lbItem {
            width: 200px;
        }

        /* like SelectedItem style */
        .ui-draggable-dragging {
            background-color: #A0A0A0;
            color: White;
        }

        /* small glowing effect */
        .hover {
            -webkit-box-shadow: 0 0 15px #ff0000;
            -moz-box-shadow: 0 0 15px #ff0000;
            box-shadow: 0 0 15px #ff0000;
        }
    </style>

    <style type="text/css">
        .ui-datepicker {
            font-size: 8pt !important;
        }

        .tabsB {
            width: 100%;
        }
    </style>

    <script type="text/javascript">
        function InitalizejQuery() {
            $('.lbItem').draggable(
                {
                    helper: 'clone',
                    //cancel: '.lbItem',

                    drag: function (event, ui) {

                        $('[id$=hfFlag]').val('1');
                    }
                }
            );
            $('.listBoxRight').droppable(
                {
                    activeClass: "hover",
                    drop: function (ev, ui) {

                        if ($('[id$=hfFlag]').val() == '1') {
                            var txt = $(ui.draggable).text();
                            var a = "<";
                            var b = "%";
                            var c = ">";
                            var d = b + txt + b;
                            if ($(ui.draggable).parents(".listBoxRight").length != 0 && ($(this)).hasClass("listBoxRight"))
                                return;
                            lbChosen.ExecuteCommand(ASPxClientCommandConsts.PASTEHTML_COMMAND, d);
                            $('[id$=hfFlag]').val('0')
                        }
                    }
                }
            );

            $('textarea').droppable(
                {
                    activeClass: "hover",
                    drop: function (ev, ui) {
                   
                        if ($('[id$=hfFlag]').val() == '1') {
                            var txt = $(ui.draggable).text();
                            var a = "<";
                            var b = "%";
                            var c = ">";
                            var d = b + txt + b;
                            $('textarea').val($('textarea').val() + d);
                            //if ($(ui.draggable).parents(".listBoxRight").length != 0 && ($(this)).hasClass("listBoxRight"))
                            //    return;
                            //lbChosen.ExecuteCommand(ASPxClientCommandConsts.PASTEHTML_COMMAND, d);
                            $('[id$=hfFlag]').val('0')
                        }
                    }
                }
            );

        }


    </script>

    <script type="text/javascript">

        function ShowPopup() {
            debugger
            ASPxClientPopupControlInstance.Show();
            return false;
        }
    </script>



    <script type="text/javascript">
        var textSeparator = ";";
        function OnListBoxSelectionChanged(listBox, args) {
            if (args.index == 0)
                args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
            UpdateSelectAllItemState();
            UpdateText();
        }
        function UpdateSelectAllItemState() {
            IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
        }
        function IsAllSelected() {
            var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
            return checkListBox.GetSelectedItems().length == selectedDataItemCount;
        }
        function UpdateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            checkComboBox.SetText(GetSelectedItemsText(selectedItems));
        }
        function SynchronizeListBoxValues(dropDown, args) {
            checkListBox.UnselectAll();
            var texts = dropDown.GetText().split(textSeparator);
            var values = GetValuesByTexts(texts);
            checkListBox.SelectValues(values);
            UpdateSelectAllItemState();
            UpdateText(); // for remove non-existing texts
        }
        function GetSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                if (items[i].index != 0)
                    texts.push(items[i].text);
            return texts.join(textSeparator);
        }
        function GetValuesByTexts(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }
    </script>

    <script type="text/javascript">
        var command;
        function OnBeginCallback(s, e) {
            command = e.command;
        }
        function OnEndCallback(s, e) {
            if (command == 'COLUMNMOVE')
                lst.PerformCallback();
        }

        function btnUp_Click(s, e) {
            var item = lst.GetSelectedItem();
            if (item == null)
                return;
            if (item.index > 0) {
                lst.RemoveItem(item.index);
                lst.InsertItem(item.index - 1, item.text, item.value)
                lst.SetSelectedIndex(item.index - 1);
                grid.PerformCallback(s.GetText());
            }
        }
        function btnDown_Click(s, e) {
            var item = lst.GetSelectedItem();
            if (item == null)
                return;
            if (item.index < lst.GetItemCount() - 1) {
                lst.RemoveItem(item.index);
                lst.InsertItem(item.index + 1, item.text, item.value)
                lst.SetSelectedIndex(item.index + 1);
                grid.PerformCallback(s.GetText());
            }
        }
    </script>
    <script type="text/javascript">
        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">


    <div class="wrapper row-offcanvas row-offcanvas-left">
        <div class="content">




            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                <ContentTemplate>
                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"
                        GridViewID="ASPxGridView1">
                    </dx:ASPxGridViewExporter>

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

                                                        <dx:ASPxLabel runat="server" ID="Attachment" Visible="false"></dx:ASPxLabel>
                                                        <dx:ASPxLabel runat="server" ID="lblLink" Visible="false"></dx:ASPxLabel>
                                                        <dx:ASPxLabel runat="server" ID="lblheadererror" Visible="false" Style="color: red"></dx:ASPxLabel>
                                                        <br />
                                                        <dx:ASPxLabel runat="server" Text="Select HEADER" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                        <dx:ASPxListBox runat="server" ID="ls1" Style="height: 500px; width: 300px;" CssClass="list-group-item-heading" Theme="Office2010Black" AutoPostBack="true" OnSelectedIndexChanged="click_Passdata"></dx:ASPxListBox>

                                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:mycon %>" SelectCommand="SELECT [Name], [MessageTemplateID] FROM [Gold_MessageTemplate]"></asp:SqlDataSource>
                                                        <dx:ASPxPopupControl runat="server" Modal="True" AllowDragging="True" ID="hederpopup" PopupElementID="CmdSubmit" ClientInstanceName="popupControl" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                                                            <ContentCollection>
                                                                <dx:PopupControlContentControl runat="server">
                                                                    <dx:ASPxLabel runat="server" ID="lblheadererror1" Visible="false" Style="color: red"></dx:ASPxLabel>
                                                                    <asp:TextBox runat="server" ID="txtheaderpopup" TextMode="MultiLine" Height="90px" Width="400px" CssClass="form-control">
                                                                    </asp:TextBox>

                                                                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" ><%--OnUnload="UpdatePanel1_Unload"
                                                                        <ContentTemplate>--%>
                                                                    <asp:Button ID="ASPxButton2" runat="server" Text="Add" CssClass="btn-success" OnClick="click_headerpopupadd"></asp:Button>
                                                                    <%--<dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Add" CssClass="btn-success" OnClick="click_headerpopupadd">
                                                                            </dx:ASPxButton>--%>
                                                                    <asp:Button runat="server" Text="Reset" OnClick="click_headerpopupreset"></asp:Button>

                                                                </dx:PopupControlContentControl>
                                                            </ContentCollection>
                                                        </dx:ASPxPopupControl>
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


                                                    <div class="col-md-4" style="height: 100%; margin-top: 25px" id="dvdatabind">
                                                        <dx:ASPxGlobalEvents ID="ASPxGlobalEvents1" runat="server">
                                                        </dx:ASPxGlobalEvents>
                                                        <asp:HiddenField ID="hdid" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hfFlag" runat="server" Value="0" />
                                                        <div class="col-md-12">
                                                            <dx:ASPxLabel runat="server" Text="HEADER" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>

                                                            <br />
                                                            <%-- <asp:TextBox ID="txtheader" runat="server" TextMode="MultiLine" Height="30px" Enabled="false" Width="555px" Theme="Office2010Black"> </asp:TextBox>--%>
                                                            <dx:ASPxTextBox ID="txtheader" runat="server" TextMode="MultiLine" Enabled="false" Width="555px" Theme="Office2010Black"></dx:ASPxTextBox>
                                                        </div>



                                                        <div class="col-md-6" style="margin-top: 15px">

                                                            <dx:ASPxLabel runat="server" Text="FROM EMAIL" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                            <br />
                                                            <%--<dx:ASPxComboBox runat="server" ClientInstanceName="chkExecItem" ValueType="System.String" ID="cbemail" Width="250px" Enabled="false" SelectionMode="CheckColumn"></dx:ASPxComboBox>--%>
                                                            <%--<dx:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="250px" runat="server" AnimationType="None" AutoPostBack="false">
                                                                <DropDownWindowStyle BackColor="#EDEDED" />
                                                                <DropDownWindowTemplate>
                                                                    <dx:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn"  AutoPostBack="false"
                                                                        runat="server">
                                                                        <Border BorderStyle="None" />
                                                                        <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                                                        <Items>
                                                                            <dx:ListEditItem Text="(Select all)"  Value="0" />
                                                                            <dx:ListEditItem Text="Chrome" Value="1" />
                                                                            <dx:ListEditItem Text="Firefox" Value="2" />
                                                                            <dx:ListEditItem Text="IE" Value="3" />
                                                                            <dx:ListEditItem Text="Opera" Value="4" />
                                                                            <dx:ListEditItem Text="Safari" Value="5" />
                                                                        </Items>
                                                                        <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" />
                                                                    </dx:ASPxListBox>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="padding: 2px">
                                                                                <dx:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                                                    <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                                                </dx:ASPxButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </DropDownWindowTemplate>
                                                                <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" />
                                                            </dx:ASPxDropDownEdit>--%>
                                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                <ContentTemplate>


                                                                    <dx:ASPxListBox ID="chkemail" ClientInstanceName="chkemail" JS-Filter="True" runat="server" Style="padding: 0px 0px !important;"
                                                                        Width="250" CssClass="form-control js-filter-class  form-control" ValueType="System.String" OnSelectedIndexChanged="Check_Index" AutoPostBack="true"
                                                                        TabIndex="1" SelectionMode="CheckColumn" Height="110" Theme="Office2010Black">
                                                                        <CaptionSettings Position="Top" />
                                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){

                                                                                           if(e.index == 0 && e.isSelected == true)
                                                                                           {
                                                                                               chkemail.SelectAll();
                                                                                           }

                                                                                           if(e.index == 0 && e.isSelected == false)
                                                                                           {
                                                                                               chkemail.UnselectAll();
                                                                                           }
                                                                           }" />

                                                                    </dx:ASPxListBox>
                                                                    <dx:ASPxCallback ID="chkemailCall" runat="server" ClientInstanceName="chkemailCall">
                                                                        <ClientSideEvents CallbackComplete="function(s, e) { lp.Hide(); }" />
                                                                    </dx:ASPxCallback>

                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-md-6" style="margin-top: 15px">
                                                            <dx:ASPxLabel runat="server" Text="Select Sequence Sending" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                            <dx:ASPxListBox ID="ASPxListBox1" runat="server" ClientInstanceName="lst" Theme="Office2010Black"
                                                                ValueType="System.Int32" Width="250px" Height="130px"
                                                                OnCallback="ASPxListBox1_Callback" OnInit="ASPxListBox1_Init" SelectedIndex="0">
                                                            </dx:ASPxListBox>
                                                            <dx:ASPxButton ID="btnUp" runat="server" Text="UP" AutoPostBack="False" Width="50px" Theme="Office2010Black">
                                                                <ClientSideEvents Click="btnUp_Click" />
                                                            </dx:ASPxButton>
                                                            <dx:ASPxButton ID="btnDown" runat="server" Text="DOWN" AutoPostBack="False" Width="50px" Theme="Office2010Black">
                                                                <ClientSideEvents Click="btnDown_Click" />
                                                            </dx:ASPxButton>
                                                        </div>








                                                        <div class="col-md-6" style="margin-top: 15px;">
                                                            <dx:ASPxLabel runat="server" Text="Start Date" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                            <%--<dx:ASPxTimeEdit ID="edittime" runat="server" EditFormat="Custom" EditFormatString="H:mm" Enabled="false" Theme="Office2010Black"></dx:ASPxTimeEdit>--%>

                                                            <dx:ASPxDateEdit ID="edittime" runat="server" EditFormat="Custom" Width="200" EditFormatString="dd-MM-yyyy H:mm" Enabled="false" Theme="Office2010Black">
                                                                <TimeSectionProperties Visible="true">
                                                                    <TimeEditProperties EditFormatString="H:mm" />
                                                                </TimeSectionProperties>
                                                            </dx:ASPxDateEdit>
                                                        </div>

                                                        <div class="col-md-6" style="margin-top: 15px">
                                                            <dx:ASPxLabel runat="server" Text="Expiry Date" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                            <dx:ASPxDateEdit ID="editdate" runat="server" Enabled="false" Theme="Office2010Black" EditFormatString="dd-MM-yyyy"></dx:ASPxDateEdit>
                                                        </div>






                                                        <div class="col-md-6" style="margin-top: 15px">
                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                <ContentTemplate>
                                                                    <dx:ASPxLabel runat="server" Text="Active" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                                    <dx:ASPxCheckBox runat="server" ID="ckactive" Enabled="false" Style="height: 26px; width: 26px" Theme="Office2010Black"></dx:ASPxCheckBox>


                                                                    <dx:ASPxLabel runat="server" Text="Attachment" Style="font-weight: bold; font-size: 12px; margin-left: 35px"></dx:ASPxLabel>
                                                                    <dx:ASPxCheckBox runat="server" ID="ckattachment" Style="height: 26px; width: 26px" OnCheckedChanged="Check_Attachment" AutoPostBack="true" Enabled="false" Theme="Office2010Black"></dx:ASPxCheckBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-md-6" style="margin-top: 15px">
                                                            <dx:ASPxLabel runat="server" Text="Enter Interval (In days Only): " Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                                <ContentTemplate>
                                                                    <dx:ASPxTextBox ID="txtinterval" onKeypress="return onlyNumbers(event);" runat="server" Style="width: 60px" Text="1" Enabled="false" Width="555px" Theme="Office2010Black" MaxLength="3"></dx:ASPxTextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-md-12">

                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:FileUpload ID="fileUpload1" runat="server" Visible="false" />
                                                                </ContentTemplate>

                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <br />
                                                        <div class="col-md-12">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                <ContentTemplate>
                                                                    <dx:ASPxButton ID="btnsave" runat="server" AutoPostBack="false" OnClick="click_save" Text="Save" Visible="false" Style="background-color: green; margin-top: 10px" CssClass="btn-success">
                                                                    </dx:ASPxButton>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="btnsave" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>




                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="col-md-12">

                                                            <dx:ASPxLabel runat="server" Text="SUBJECT" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                            <br />
                                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                                <ContentTemplate>
                                                                    <%-- <dx:ASPxHtmlEditor ID="txtsubject" runat="server" Enabled="false"  CssClass="listBoxRight1" ClientInstanceName="lbChosen1" Height="200px" Width="900px"></dx:ASPxHtmlEditor>--%>
                                                                    <asp:TextBox ID="txtsubject" runat="server" TextMode="MultiLine" Enabled="false" Height="70px" Width="555px" Theme="Office2010Black"> </asp:TextBox>
                                                                </ContentTemplate>

                                                            </asp:UpdatePanel>
                                                            <%--<dx:ASPxTextBox ID="txtsubject" runat="server" TextMode="MultiLine" Enabled="false" Width="555px" Theme="Office2010Black"></dx:ASPxTextBox>--%>
                                                            <%-- <asp:TextBox ID="txtsubject" runat="server" TextMode="MultiLine" Enabled="false" Height="30px" Width="555px" Theme="Office2010Black"> </asp:TextBox>--%>
                                                        </div>

                                                        <div class="col-md-6" style="margin-top: 15px">
                                                            <dx:ASPxLabel runat="server" Text="BCC" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                            <%--<dx:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit2" Width="250px" runat="server" AnimationType="None" AutoPostBack="false">
                                                                <DropDownWindowStyle BackColor="#EDEDED" />
                                                                <DropDownWindowTemplate>
                                                                    <dx:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" AutoPostBack="false"
                                                                        runat="server">
                                                                        <Border BorderStyle="None" />
                                                                        <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                                        <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" />
                                                                    </dx:ASPxListBox>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="padding: 4px">
                                                                                <dx:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                                                    <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                                                </dx:ASPxButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </DropDownWindowTemplate>
                                                                <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" />
                                                            </dx:ASPxDropDownEdit>--%>
                                                            <%--  <asp:TextBox ID="txtbcc" runat="server" TextMode="MultiLine" Enabled="false" Height="30px" Width="250px"> </asp:TextBox>--%>
                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                <ContentTemplate>
                                                                    <%--<dx:ASPxListBox ID="chkbcc" ClientInstanceName="chkbcc" JS-Filter="True" runat="server" Style="padding: 0px 0px !important;"
                                                                        Width="250" CssClass="form-control js-filter-class" ValueType="System.String"
                                                                        TabIndex="1" SelectionMode="CheckColumn" Height="110">
                                                                        <CaptionSettings Position="Top" />
                                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){

                                                                                           if(e.index == 0 && e.isSelected == true)
                                                                                           {
                                                                                               chkbcc.SelectAll();
                                                                                           }

                                                                                           if(e.index == 0 && e.isSelected == false)
                                                                                           {
                                                                                               chkbcc.UnselectAll();
                                                                                           }
                                                                           }" />
                                                                    </dx:ASPxListBox>
                                                                    <dx:ASPxCallback ID="chkbccCall" runat="server" ClientInstanceName="chkbccCall">
                                                                        <ClientSideEvents CallbackComplete="function(s, e) { lp.Hide(); }" />
                                                                    </dx:ASPxCallback>--%>
                                                                    <dx:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="210px" runat="server" AnimationType="None" Theme="Office2010Black" Enabled="false">
                                                                        <DropDownWindowStyle BackColor="#EDEDED" />
                                                                        <DropDownWindowTemplate>
                                                                            <dx:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" Theme="SoftOrange"
                                                                                runat="server">
                                                                                <Border BorderStyle="None" />
                                                                                <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                                                                <%--  <Items>
                                                                            <dx:ListEditItem Text="(Select all)" />
                                                                            <dx:ListEditItem Text="Chrome" Value="1" />
                                                                            <dx:ListEditItem Text="Firefox" Value="2" />
                                                                            <dx:ListEditItem Text="IE" Value="3" />
                                                                            <dx:ListEditItem Text="Opera" Value="4" />
                                                                            <dx:ListEditItem Text="Safari" Value="5" />
                                                                        </Items>--%>
                                                                                <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" />
                                                                            </dx:ASPxListBox>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="padding: 4px">
                                                                                        <dx:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close" Style="float: right" Theme="Office2010Black">
                                                                                            <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                                                        </dx:ASPxButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </DropDownWindowTemplate>
                                                                        <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" />
                                                                    </dx:ASPxDropDownEdit>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>


                                                        </div>
                                                        <div class="col-md-6" style="margin-top: 15px">
                                                            <dx:ASPxLabel runat="server" Text="Select Tokan" Style="font-weight: bold; font-size: 12px" ClientInstanceName="lbAvailable"></dx:ASPxLabel>
                                                            <br />
                                                            <dx:ASPxComboBox runat="server" ValueType="System.String" ID="cbtokan" Width="250px" Enabled="false" Theme="Office2010Black">
                                                                <ClientSideEvents GotFocus="function (s, e) { InitalizejQuery(); }" />
                                                                <ItemStyle CssClass="lbItem  txt" />
                                                            </dx:ASPxComboBox>
                                                        </div>


                                                        <div class="col-md-12" style="margin-top: 15px">
                                                            <dx:ASPxLabel runat="server" Text="BODY" Style="font-weight: bold; font-size: 12px; width: 100%"></dx:ASPxLabel>
                                                            <br />
                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                <ContentTemplate>
                                                                    <dx:ASPxHtmlEditor ID="txtbody" runat="server" Enabled="false" CssClass="listBoxRight" ClientInstanceName="lbChosen" Height="700px" Width="900px"></dx:ASPxHtmlEditor>
                                                                </ContentTemplate>

                                                            </asp:UpdatePanel>
                                                        </div>

                                                        <div class="col-md-12" style="margin-top: 15px">
                                                            <dx:ASPxLabel runat="server" Text="SIGNATURE" Style="font-weight: bold; font-size: 12px; width: 100%"></dx:ASPxLabel>
                                                            <br />
                                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                                <ContentTemplate>
                                                                    <dx:ASPxHtmlEditor ID="txtsign" runat="server" Enabled="false" CssClass="listBoxRight" ClientInstanceName="lbsign" Height="200px" Width="900px"></dx:ASPxHtmlEditor>
                                                                </ContentTemplate>

                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
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


                                                                                <div class="row" style="width: 100%">
                                                                                    <div class="col-md-12">
                                                                                    </div>

                                                                                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" Width="100%" AutoGenerateColumns="False" Theme="Office2010Black" KeyFieldName="MessageTemplateLocalizedID" OnDataBinding="ASPxGridView1_DataBinding" Style="margin-left: 25px;" EnableTheming="True">
                                                                                        <Settings ShowFilterRow="True" />
                                                                                        <SettingsCommandButton>
                                                                                            <ShowAdaptiveDetailButton ButtonType="Image">
                                                                                            </ShowAdaptiveDetailButton>
                                                                                            <HideAdaptiveDetailButton ButtonType="Image">
                                                                                            </HideAdaptiveDetailButton>
                                                                                        </SettingsCommandButton>
                                                                                        <Columns>

                                                                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="MessageTemplateLocalizedID" VisibleIndex="1" Width="250px">
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

                                                                                            <dx:GridViewDataTextColumn Caption="Template Name" FieldName="Name" VisibleIndex="1" Width="250px">
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

                                                                                            <dx:GridViewDataTextColumn Caption="BCC Email" FieldName="BCCEmailAddresses" VisibleIndex="2" Width="250px">
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

                                                                                            <dx:GridViewDataTextColumn Caption="Body" FieldName="Body" VisibleIndex="3" Width="500px" Visible="false">

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




                                                                                            <dx:GridViewDataTextColumn Caption="Sending Time" FieldName="EmailTime" VisibleIndex="4" Width="250px">
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

                                                                                            <dx:GridViewDataTextColumn Caption="Last Date" FieldName="EmailLastDate" VisibleIndex="4" Width="250px">
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

                                                                                            <dx:GridViewDataTextColumn VisibleIndex="6" Caption="Active" Width="50px">
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
                                                                                                <DataItemTemplate>
                                                                                                    <dx:ASPxCheckBox ID="chbox" runat="server" Enabled="false" Checked='<%# Eval("isactive").ToString() == "True" ? true : false  %>'>
                                                                                                    </dx:ASPxCheckBox>
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

