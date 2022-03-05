<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Gold_Message.aspx.cs" Inherits="Presentation_Master_Email_Gold_Message" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v18.1, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">

  

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
         function onlyNumbers(event)
      {
             var charCode = (event.which) ? event.which : event.keyCode;
             if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;    
         return true;
      }
   </script>
    <script  type="text/javascript">
        function CharacterCount() {
            debugger;
            //$("#ctl00_MainContent_ASPxPageControl1_lblcount").text($("#ctl00_MainContent_ASPxPageControl1_memobody_I").val().length);
            $("[id$=lblcount]").text($('textarea').val()    .length);

        
      }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <dx:ASPxGlobalEvents ID="ASPxGlobalEvents1" runat="server">
  
    </dx:ASPxGlobalEvents>
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
                                                    <div class="col-md-2"></div>
                                                    <div class="col-md-2" style="height: 100%;">

                                                        <dx:ASPxLabel runat="server" ID="lblId" Visible="false"></dx:ASPxLabel>
                                                        <dx:ASPxLabel runat="server" ID="lblLink" Visible="false"></dx:ASPxLabel>
                                                        <dx:ASPxLabel runat="server" ID="lblheadererror" Visible="false" Style="color: red"></dx:ASPxLabel>
                                                        <br />
                                                        <dx:ASPxLabel runat="server" Text="Select HEADER" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                        <dx:ASPxListBox runat="server" ID="ls1" Style="height: 500px; width: 300px;" Theme="Office2010Black" AutoPostBack="true" 
                                                            OnSelectedIndexChanged="Click_PassData">

                                                        </dx:ASPxListBox>









                                                    </div>


                                                    <div class="col-md-4" style="height: 100%; margin-top: 25px" id="dvdatabind">

                                                        <asp:HiddenField ID="hdid" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hfFlag" runat="server" Value="0" />
                                                        <div class="col-md-12">
                                                            <dx:ASPxLabel runat="server" Text="HEADER" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>

                                                            <br />
                                                            <%-- <asp:TextBox ID="txtheader" runat="server" TextMode="MultiLine" Height="30px" Enabled="false" Width="555px" Theme="Office2010Black"> </asp:TextBox>--%>
                                                            <dx:ASPxTextBox ID="txtheader" runat="server" TextMode="MultiLine" Enabled="false" Width="555px" Theme="Office2010Black"></dx:ASPxTextBox>
                                                        </div>










                                                        <div class="col-md-6" style="margin-top: 15px;">
                                                            <dx:ASPxLabel runat="server" Text="Start Date" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>


                                                             <dx:ASPxDateEdit ID="edittime" runat="server" EditFormat="Custom"  Width="250" EditFormatString="dd-MM-yyyy H:mm" Enabled="false" Theme="Office2010Black">
                                                                <TimeSectionProperties Visible="true">
                                                                    <TimeEditProperties EditFormatString="H:mm" />
                                                                </TimeSectionProperties>
                                                            </dx:ASPxDateEdit>
                                                        </div>

                                                        <div class="col-md-6" style="margin-top: 15px">
                                                            <dx:ASPxLabel runat="server" Text="Expiry Date" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                            <dx:ASPxDateEdit ID="editdate" runat="server" Enabled="false" Width="250px" Theme="Office2010Black" EditFormatString="dd-MM-yyyy"></dx:ASPxDateEdit>

                                                        </div>



                                                        <div class="col-md-6" style="margin-top: 15px">
                                                            <dx:ASPxLabel runat="server" Text="Select Tokan" Style="font-weight: bold; font-size: 12px" ClientInstanceName="lbAvailable"></dx:ASPxLabel>
                                                            <br />
                                                            <dx:ASPxComboBox runat="server" ValueType="System.String" ID="cbtokan" Width="250px" Enabled="false" Theme="Office2010Black">
                                                                <ClientSideEvents GotFocus="function (s, e) { InitalizejQuery(); }" />
                                                                <ItemStyle CssClass="lbItem txt" />
                                                            </dx:ASPxComboBox>
                                                        </div>


                                                        <div class="col-md-6" style="margin-top: 15px">

                                                            <dx:ASPxLabel runat="server" Text="Active" Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                            <br />
                                                            <dx:ASPxCheckBox runat="server" ID="ckactive" Style="height: 26px; width: 26px" Theme="Office2010Black" Enabled="false"></dx:ASPxCheckBox>



                                                        </div>

                                                          <div class="col-md-12" style="margin-top: 15px">
                                                             <dx:ASPxLabel runat="server" Text="Enter Interval (In Days Only) " Style="font-weight: bold; font-size: 12px"></dx:ASPxLabel>
                                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                                <ContentTemplate>
                                                         <dx:ASPxTextBox ID="txtinterval" runat="server" onKeypress="return onlyNumbers(event);" style="width:60px" Text="1" Enabled="false" Width="555px" Theme="Office2010Black" MaxLength="3"></dx:ASPxTextBox>
                                                              </ContentTemplate>
                                                                </asp:UpdatePanel> 
                                                        </div>
                                                        <div class="col-md-6" style="margin-top: 15px">
                                                            <dx:ASPxLabel runat="server" Text="BODY" Style="font-weight: bold; font-size: 12px; width: 100%"></dx:ASPxLabel>
                                                            <br />
                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                <ContentTemplate>
                                                                    <dx:ASPxMemo ID="lbChosen" onpaste="CharacterCount()" onkeyup="CharacterCount()" runat="server" Height="215px" 
                                                                        Width="300px" Enabled="false"
                                                                         CssClass="listBoxRight" ClientInstanceName="lbChosen"></dx:ASPxMemo>
                                                                </ContentTemplate>

                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-md-6" style="margin-top: 15px">
                                                            <dx:ASPxLabel runat="server" Text="Your Text Count:" ID="ASPxLabel1" Style="font-weight: bold; font-size: 12px; width: 100%" ></dx:ASPxLabel>
                                                            <br />
                                                            <dx:ASPxLabel runat="server" Text="0" ID="lblcount" Style="color:forestgreen"></dx:ASPxLabel>

                                                        </div>
                                                        <br />
                                                        <div class="col-md-12">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                <ContentTemplate>
                                                                    <dx:ASPxButton ID="btnsave" runat="server" Text="Save" Visible="false" Style="background-color: green; margin-top: 10px" CssClass="btn-success" OnClick="Click_SaveData">
                                                                    </dx:ASPxButton>
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

                                                                                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" Theme="Office2010Black"  Style="margin-left: 25px;" EnableTheming="True" KeyFieldName="MessageTemplateLocalizedID" OnDataBinding="ASPxGridView1_DataBinding">
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

                                                                                                 <dx:GridViewDataTextColumn Caption="Body" FieldName="Body" VisibleIndex="3" Width="500px">

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

                                                                                            
                                                                                            <dx:GridViewDataTextColumn Caption="Sending Time" FieldName="MessageTime" VisibleIndex="4" Width="250px">
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

                                                                                            <dx:GridViewDataTextColumn Caption="Last Date" FieldName="LastMessageDate" VisibleIndex="4" Width="250px">
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

