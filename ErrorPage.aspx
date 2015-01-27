<%@ Page Title="" Language="C#" MasterPageFile="~/MasterError.master" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderhead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="Server">
    <div id="HeadDiv" runat="server" class="content_resize" style="-moz-border-radius: 50px; -webkit-border-radius: 50px; border-radius: 50px;">
        <div style="float: left; width: 1000px;">
            <div style="float: left; width: 200px; height:auto;">
                <table>
                    <tr>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div style="float: left; width: 600px; height:auto;" class="content">
                <table>
                    <tr>
                        <td>
                            <asp:Image ID="ImageError" runat="server" ImageUrl="~/images/404-florel-creativem-f.jpg" Width="600px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelError" runat="server" Text="" Font-Bold="True" Font-Size="Medium" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="LBHomePage" runat="server" PostBackUrl="~/HomePage.aspx" Font-Bold="True" Font-Size="Medium" ForeColor="#0033CC">Back To Home Page</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="float: left; width: 200px; height:auto;">
                <table>
                    <tr>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

