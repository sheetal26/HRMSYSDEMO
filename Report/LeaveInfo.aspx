<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="LeaveInfo.aspx.cs" Inherits="Report_LeaveInfo" %>

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
                            <td style="text-align: right;">App From Date</td>
                            <td>
                                <asp:TextBox ID="TxtAppFDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar3" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar3" PopupPosition="BottomRight" TargetControlID="TxtAppFDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align: right;">To Date</td>
                            <td>
                                <asp:TextBox ID="TxtAppTDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar4" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar4" PopupPosition="BottomRight" TargetControlID="TxtAppTDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                        </tr>                       
                        <tr>
                            <td style="text-align: right;">Leave Type</td>
                            <td>
                                <asp:DropDownList ID="DDLLeaveType" runat="server" Width="200px" Height="22px">
                                    <asp:ListItem Value="0">---Select Type---</asp:ListItem>
                                    <asp:ListItem Value="H">Half Day</asp:ListItem>
                                    <asp:ListItem Value="F">Full Day</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right;">Leave Status</td>
                            <td>
                                <asp:DropDownList ID="DDLStatus" runat="server" Width="200px" Height="22px">
                                    <asp:ListItem Value="0">---Select Status---</asp:ListItem>
                                    <asp:ListItem Value="A">Approve</asp:ListItem>
                                    <asp:ListItem Value="U">UnApproved</asp:ListItem>
                                    <asp:ListItem Value="C">Cancel</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;">
                                <asp:Button ID="BtnShow" runat="server" Text="Show Data (RV)" CssClass="button green" OnClick="BtnShow_Click" Height="25px" Width="125px" />
                            </td> 
                            <td style="text-align: left;">
                                <asp:Button ID="BtnPrintRV" runat="server" Text="Print Data RV" CssClass="button black" Height="25px" Width="125px" OnClientClick="doPrint();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="600px" Width="1000px" BorderStyle="Solid" BorderWidth="1px"></rsweb:ReportViewer>
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

