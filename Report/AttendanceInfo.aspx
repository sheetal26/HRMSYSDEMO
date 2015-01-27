<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="AttendanceInfo.aspx.cs" Inherits="Report_AttendanceInfo" %>

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
                        <tr>
                            <td style="text-align: right;">Employee</td>
                            <td>
                                <asp:DropDownList ID="ddlEmployee" runat="server" Width="250px">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right;">Year</td>
                            <td>
                                <asp:DropDownList ID="ddlyear" runat="server" Width="150px" OnSelectedIndexChanged="ddlyear_SelectedIndexChanged" 
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right;">Month</td>
                            <td>
                                <asp:DropDownList ID="ddlmonth" runat="server" Width="200px" OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged" 
                                    AutoPostBack="true">                                    
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
                            <tr>
                                <td style="text-align: right;">From Date</td>
                                <td>
                                    <asp:TextBox ID="TxtFDate" runat="server" Width="100px"></asp:TextBox>
                                    <asp:ImageButton ID="imgbtnCalendar1" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                        PopupButtonID="imgbtnCalendar1" PopupPosition="BottomRight" TargetControlID="TxtFDate" Enabled="True">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="text-align: right;">To Date</td>
                                <td>
                                    <asp:TextBox ID="TxtTDate" runat="server" Width="100px"></asp:TextBox>
                                    <asp:ImageButton ID="imgbtnCalendar2" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                        PopupButtonID="imgbtnCalendar2" PopupPosition="BottomRight" TargetControlID="TxtTDate" Enabled="True">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Button ID="BtnShow" runat="server" Text="Show Data (RV)" CssClass="button green" OnClick="BtnShow_Click" Height="25px" Width="125px" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button ID="BtnPrintRV" runat="server" Text="Print Data RV" CssClass="button black" Height="25px" Width="125px" OnClientClick="doPrint();" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="700px" Width="1000px" BorderStyle="Solid" BorderWidth="1px"></rsweb:ReportViewer>
                                </td>
                            </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnShow" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="BtnPrintRV" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

