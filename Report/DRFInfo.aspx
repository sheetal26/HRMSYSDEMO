<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="DRFInfo.aspx.cs" Inherits="Report_DRFInfo" %>

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
                                <asp:DropDownList ID="ddlEmployee" runat="server" Width="200px">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right;">DRF From Date</td>
                            <td>
                                <asp:TextBox ID="TxtFDrfDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar1" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar1" PopupPosition="BottomRight" TargetControlID="TxtFDrfDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align: right;">To Date</td>
                            <td>
                                <asp:TextBox ID="TxtTDrfDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar2" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar2" PopupPosition="BottomRight" TargetControlID="TxtTDrfDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">Project Name</td>
                            <td>
                                 <asp:DropDownList ID="DDLPrjName" runat="server" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="DDLPrjName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right;">Module Name</td>
                            <td>
                                 <asp:DropDownList ID="DDLPrjModule" runat="server" Width="200px" Height="22px">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right;">Work Status</td>
                            <td>
                                <asp:DropDownList ID="DDlWorkStat" runat="server" Height="26px" Width="125px">
                                    <asp:ListItem Value="0">---Select---</asp:ListItem>
                                    <asp:ListItem Value="D">Done</asp:ListItem>
                                    <asp:ListItem Value="P">Pending</asp:ListItem>
                                    <asp:ListItem Value="C">Cancel</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>      
                            <td style="text-align: right;"></td>
                            <td></td>                      
                            <td style="text-align: right;"></td>
                            <td></td>
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

