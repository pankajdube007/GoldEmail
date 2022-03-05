<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="Presentation_Dashboard_DashBoard" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" Runat="Server">

    </asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
    <br />
    <br />
    <br />
    <div class="col-sm-12">
        <asp:UpdatePanel ID="mUP" runat="server">
        <ContentTemplate>
            <asp:Repeater ID="rptClient" runat="server">
                <HeaderTemplate>
                    <table class="table">
                        <tr>
                            <th >
                                User Host Address
                            </th>
                            <th >
                                User Host Name
                            </th>
                            <th >
                                Machine Name
                            </th>
                            <th >
                                OS
                            </th>
                            <th >
                                Browser
                            </th>
                            <th >
                                Browser Version
                            </th>
                            <th >
                                Cookies Enable
                            </th>
                            <th >
                                Is Mobile Device
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label ID="lblUserHostAddress" runat="server" Text='<%# Eval("UserHostAddress") %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblUserHostName" runat="server" Text='<%# Eval("UserHostName") %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblMachineName" runat="server" Text='<%# Eval("MachineName") %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblPlatform" runat="server" Text='<%# Eval("Platform") %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblBrowser" runat="server" Text='<%# Eval("Browser") %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblVersion" runat="server" Text='<%# Eval("Version") %>' />
                        </td>
                         <td>
                            <asp:Label ID="lblCookies" runat="server" Text='<%# Eval("Cookies") %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblIsMobileDevice" runat="server" Text='<%# Eval("IsMobileDevice") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    
</asp:Content>

