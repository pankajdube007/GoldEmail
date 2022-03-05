<%@ Page Title="" Language="C#" MasterPageFile="~/WithoutHeader.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>



<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .modalBackground {
            /*background-color: Gray;*/
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .btn-space {
            margin-right: 10px;
        }

        .View {
            font-size: 15px;
            margin-bottom: 20px;
            color: #3A7FBA;
        }
    </style>
    </asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
              <asp:HiddenField runat="server" id="hip" Value=""> </asp:HiddenField>
              <asp:HiddenField runat="server" id="hhost" Value=""> </asp:HiddenField>
           
        </ContentTemplate>   
    </asp:UpdatePanel>   
    <img src="blurred-bg-3.jpg" class="login-img wow fadeIn animated" alt="" style="visibility: visible; animation-name: fadeIn;margin-top:-130px">
    <div>
        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />                                                          
        <asp:ModalPopupExtender ID="mpeImage" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup" RepositionMode="RepositionOnWindowResize" DropShadow="true" BackgroundCssClass="modalBackground"></asp:ModalPopupExtender>
        <asp:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID="pnlpopup" Corners="All" Radius="25" />        
        <asp:RoundedCornersExtender ID="RoundedCornersExtender2" runat="server" TargetControlID="loginheader" Corners="Top" Radius="25" />
        <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="380px" Width="500px" Style="display: none">                                                                   
            <div class="col-md-12">
            <section id="loginForm">
                <div class="form-horizontal">
                    <div id="loginheader" runat="server" class="row bg-gradient-9" style="background-color:darkcyan">
                        <br />
                    <h4 class="text-center">This Is Member Area</h4>
                      <br />
                        </div>
                  <br /><br />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-md-3 control-label">User name</asp:Label>
                        <div class="col-md-9">
                            <asp:TextBox runat="server" ID="UserName" CssClass="form-control" placeholder="Email" required="required"  />
                            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" CssClass="text-danger" ErrorMessage="The user name field is required." />--%>

                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-3 control-label">Password</asp:Label>
                        <div class="col-md-9">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" placeholder="Password"  required="required"/>
                            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />--%>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-3 col-md-9">
                            <div class="checkbox">
                                <asp:CheckBox runat="server" ID="RememberMe" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-3 col-md-9">
                            <asp:Button ID="btnLogIn" runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-success" />
                        </div>
                    </div>
                </div>
                <p>
                    <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register</asp:HyperLink>
                    if you don't have a local account. <asp:Label runat="server" ID="litinfo" Visible="true"></asp:Label>
                </p>
            </section>
        </div>    
        </asp:Panel>
    </div>

</asp:Content>

