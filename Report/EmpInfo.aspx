<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="EmpInfo.aspx.cs" Inherits="Report_EmpInfo" %>

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
                            <td style="text-align:right;">Employee</td>
                            <td>
                                <asp:DropDownList ID="ddlEmployee" runat="server" Width="200px">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align:right;">Employee Group</td>
                            <td>
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
                            <td style="text-align:right;">Blood Group
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBloodGrp" runat="server" Width="150px">
                                    <asp:ListItem Value="0">---Select Group---</asp:ListItem>
                                    <asp:ListItem>A +</asp:ListItem>
                                    <asp:ListItem>A -</asp:ListItem>
                                    <asp:ListItem>B +</asp:ListItem>
                                    <asp:ListItem>B -</asp:ListItem>
                                    <asp:ListItem>AB +</asp:ListItem>
                                    <asp:ListItem>AB -</asp:ListItem>
                                    <asp:ListItem>O +</asp:ListItem>
                                    <asp:ListItem>O -</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">Department
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLDeptName" runat="server" Width="200px" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDLDeptName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align:right;">Designation
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLDesignation" runat="server" Width="200px"></asp:DropDownList>
                            </td>
                            <td style="text-align:right;">Gender
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblGender" runat="server" Width="160px" ValidationGroup="Gender" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Value="A" Text="All"></asp:ListItem>
                                    <asp:ListItem Value="M" Text="Male"></asp:ListItem>
                                    <asp:ListItem Value="F" Text="Female"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">Date of Join From
                            </td>
                            <td>
                                <asp:TextBox ID="TxtFDOJ" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar1" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar1" PopupPosition="BottomRight" TargetControlID="TxtFDOJ" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align: right;">To Date
                            </td>
                            <td>
                                <asp:TextBox ID="TxtTDOJ" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar2" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar2" PopupPosition="BottomRight" TargetControlID="TxtTDOJ" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align:right;">Country
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCountry" runat="server" Width="200px" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">Date of Brith From
                            </td>
                            <td>
                                <asp:TextBox ID="TxtFDOB" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar3" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar3" PopupPosition="BottomRight" TargetControlID="TxtFDOB" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align: right;">To Date
                            </td>
                            <td>
                                <asp:TextBox ID="TxtTDOB" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar4" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar4" PopupPosition="BottomRight" TargetControlID="TxtTDOB" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align:right;">State
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlState" runat="server" Width="200px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">Left Date From
                            </td>
                            <td>
                                <asp:TextBox ID="TxtFLeftDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar5" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar5" PopupPosition="BottomRight" TargetControlID="TxtFLeftDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align: right;">To Date
                            </td>
                            <td>
                                <asp:TextBox ID="TxtTLeftDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar6" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar6" PopupPosition="BottomRight" TargetControlID="TxtTLeftDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align:right;">City
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCity" runat="server" Width="200px" AutoPostBack="True"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td style="text-align:right;">
                                <asp:Button ID="BtnShow" runat="server" Text="Show Data (RV)" CssClass="button green" OnClick="BtnShow_Click" Height="25px" Width="125px" />
                            </td>
                            <td style="text-align:center;">
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

