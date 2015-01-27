<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="MthSalaryInfo.aspx.cs" Inherits="Report_MthSalaryInfo" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderhead" runat="Server">
    <script type="text/javascript">
        function doPrint() {
            var prtContent = document.getElementById('<%= ReportViewer1.ClientID %>');
            prtContent.border = 0; //set no border here
            var WinPrint = window.open('', '', 'left=150,top=100,width=1000,height=1000,toolbar=0,scrollbars=1,status=0,resizable=1');
            WinPrint.document.write(prtContent.outerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
        }

        function validate() {
            var str = "";
            if (document.getElementById("<%=ddlyear.ClientID%>").value == "0") {
                str = str + "Select Valid Year. \n";
            }
            //if (document.getElementById("<%=ddlmonth.ClientID%>").value == "0") {
            //    str = str + "Select Valid Month. \n";
            //}
            if (str == "") {
                return (true);
            }
            else {
                alert(str);
                return (false);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <div id="HeadDiv" runat="server" class="content_resize" style="-moz-border-radius: 50px; -webkit-border-radius: 50px; border-radius: 50px;">
        <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" />
        <asp:SiteMapPath ID="SiteMap1" runat="server"></asp:SiteMapPath>
        <div class="content">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table>
                        <tr style="vertical-align: top">
                            <td colspan="5" style="text-align: left;">
                                <asp:Label ID="LblMsg" runat="server" Text="..." Font-Bold="True" Font-Size="Small" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th style="text-align: right;">Year</th>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlyear" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <th style="text-align: right;">Month</th>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlmonth" runat="server" Width="200px">
                                    <asp:ListItem Value="0" Text="---Select Month---"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="JANUARY"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="FEBRUARY"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="MARCH"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="APRIL"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="MAY"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="JUNE"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="JULY"></asp:ListItem>
                                    <asp:ListItem Value="8" Text="AUGUST"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="SEPTEMBER"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="OCTOBER"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="NOVEMBER"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="DECEMBER"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: center;">
                                <asp:Button ID="BtnShow" runat="server" Text="Show Data (RV)" CssClass="button green" OnClick="BtnShow_Click" 
                                    OnClientClick="return validate();" Height="25px" Width="125px" />
                            </td>
                        </tr>
                        <tr>
                            <th style="text-align: right;">Employee</th>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlEmployee" runat="server" Width="300px" AutoPostBack="True"></asp:DropDownList>
                            </td>
                            <td style="text-align: right;">
                                Employee Group
                            </td> 
                            <td style="text-align: left;">
                                 <asp:DropDownList ID="DDLEmpGroup" runat="server" Height="22px" Width="200px">
                                    <asp:ListItem Value="0">---Select Group---</asp:ListItem>
                                    <asp:ListItem>Administrator</asp:ListItem>
                                    <asp:ListItem>Dept. Head</asp:ListItem>
                                    <asp:ListItem>Ledger</asp:ListItem>
                                    <asp:ListItem>Senior</asp:ListItem>
                                    <asp:ListItem>Junior</asp:ListItem>
                                    <asp:ListItem>Fresher</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: center;">
                                <asp:Button ID="BtnPrintRV" runat="server" Text="Print Data RV" CssClass="button black" Height="25px" Width="125px" OnClientClick="doPrint();" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="5">
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1000px" Height="600px" BorderStyle="Solid" BorderWidth="1px">
                                </rsweb:ReportViewer>
                            </td>
                        </tr>                       
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnShow" EventName="Click" />                        
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>

