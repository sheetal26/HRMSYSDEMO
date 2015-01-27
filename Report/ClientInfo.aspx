<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="ClientInfo.aspx.cs" Inherits="Report_ClientInfo" %>

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
                            <td style="text-align: right;">Client Name</td>
                            <td>
                                <asp:TextBox ID="TxtClientName" runat="server" Width="200px" Style="margin-bottom: 0px"></asp:TextBox>
                            </td>
                            <td style="text-align: right;">Firm Name</td>
                            <td>
                                <asp:TextBox ID="TxtFirmName" runat="server" Width="200px" Style="margin-bottom: 0px"></asp:TextBox>
                            </td>
                            <td style="text-align: right;">EMail Id</td>
                            <td>
                                <asp:TextBox ID="TxtEMailId" runat="server" Width="261px" onblur="EMailValidation(this);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">County</td>
                            <td>
                                <asp:DropDownList ID="ddlCountry" runat="server" Width="200px"
                                    OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right;">State</td>
                            <td>
                                <asp:DropDownList ID="ddlState" runat="server" Width="200px"
                                    OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right;">City</td>
                            <td>
                                <asp:DropDownList ID="ddlCity" runat="server" Width="200px" AutoPostBack="True"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">DOJ From</td>
                            <td>
                                <asp:TextBox ID="TxtFDOJ" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar2" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar2" PopupPosition="BottomRight" TargetControlID="TxtFDOJ" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align: right;">To Date</td>
                            <td>
                                <asp:TextBox ID="TxtTDOJ" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar3" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar3" PopupPosition="BottomRight" TargetControlID="TxtTDOJ" Enabled="True">
                                </asp:CalendarExtender>
                            </td>                            
                            <td colspan="2" style="text-align: right;">
                                
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">Left Date From</td>
                            <td>
                                <asp:TextBox ID="TxtFLeftDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar5" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar5" PopupPosition="BottomRight" TargetControlID="TxtFLeftDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align: right;">To Date</td>
                            <td>
                                <asp:TextBox ID="TxtTLeftDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar6" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar6" PopupPosition="BottomRight" TargetControlID="TxtTLeftDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td colspan="2" style="text-align: center;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="BtnShow" runat="server" Text="Show Data (RV)" CssClass="button green"
                                                OnClick="BtnShow_Click" Height="25px" Width="115px" />
                                        </td>                                        
                                        <td>
                                            <asp:Button ID="BtnPrintRV" runat="server" Text="Print Data RV" CssClass="button black"
                                                Height="25px" Width="115px" OnClientClick="doPrint();" />
                                        </td>
                                        <td>
                                            <asp:Button ID="BtnClear" runat="server" Text="Clear All" CssClass="button black"
                                                OnClick="BtnClear_Click" Height="25px" Width="115px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="700px" Width="1000px"
                                    BorderStyle="Solid" BorderWidth="1px">
                                </rsweb:ReportViewer>
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


