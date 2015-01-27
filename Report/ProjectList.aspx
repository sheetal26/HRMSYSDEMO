<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="ProjectList.aspx.cs" Inherits="Report_ProjectList" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderhead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <%--<asp:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID="HeadDiv" Radius="50" Corners="All">
    </asp:RoundedCornersExtender>--%>
    <div id="HeadDiv" runat="server" class="content_resize"  style="-moz-border-radius: 50px; -webkit-border-radius: 50px;border-radius: 50px;">
        <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" />
        <asp:SiteMapPath ID="SiteMap1" runat="server"></asp:SiteMapPath>
        <div class="content"> <%-- class="article"--%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>Project Name</td>
                            <td>
                                <asp:DropDownList ID="DDLPrjName" runat="server" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="DDLPrjName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>Project Start Date</td>
                            <td>
                                <asp:TextBox ID="TxtStartDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar1" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar1" PopupPosition="BottomRight" TargetControlID="TxtStartDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align: center;">
                                <asp:Button ID="BtnShow" runat="server" Text="Show Data (RV)" CssClass="button green" OnClick="BtnShow_Click" Height="25px" />
                            </td>
                        </tr>
                        <tr>
                            <td>Project Status</td>
                            <td>
                                <asp:DropDownList ID="ddlPrjStatus" runat="server" Width="150px">
                                    <asp:ListItem Value="0">---Select Status---</asp:ListItem>
                                    <asp:ListItem Value="D">Done</asp:ListItem>
                                    <asp:ListItem Value="P">Pending</asp:ListItem>
                                </asp:DropDownList>
                            </td>

                            <td>Project End Date</td>
                            <td>
                                <asp:TextBox ID="TxtEndDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar2" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar2" PopupPosition="BottomRight" TargetControlID="TxtEndDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align: center;">
                                <%--<asp:Button ID="BtnShowCR" runat="server" Text="Show Data (CR)" CssClass="button blue" Height="25px" OnClick="BtnShowCR_Click" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td>Module Name</td>
                            <td>
                                <asp:DropDownList ID="DDLPrjModule" runat="server" Width="250px" Height="16px">
                                </asp:DropDownList>
                            </td>
                            <td>Module Start Date</td>
                            <td>
                                <asp:TextBox ID="TxtModStartDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar3" PopupPosition="BottomRight" TargetControlID="TxtModStartDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align: center;">
                                <%--<asp:Button ID="BtnShowWS" runat="server" Text="Show Data (WS)" CssClass="button yellow" Height="25px" OnClick="BtnShowWS_Click" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td>Module Status</td>
                            <td>
                                <asp:DropDownList ID="ddlModStatus" runat="server" Width="150px">
                                    <asp:ListItem Value="0">---Select Status---</asp:ListItem>
                                    <asp:ListItem Value="D">Done</asp:ListItem>
                                    <asp:ListItem Value="P">Pending</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Module End Date</td>
                            <td>
                                <asp:TextBox ID="TxtModEndDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar4" PopupPosition="BottomRight" TargetControlID="TxtModEndDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align: center;">
                                <%--<asp:Button ID="BtnShowsQE" runat="server" Text="Show Data (QE)" CssClass="button purple" Height="25px" OnClick="BtnShowQE_Click" />--%>
                            </td>
                        </tr>
                       <%-- <tr>
                            <th colspan="5">
                                <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="HRMSysLinQDataContext" EntityTypeName="" OrderBy="ProjectName, Client_Mast, StartDate" TableName="Project_Modules"></asp:LinqDataSource>
                            </th>
                        </tr>
                        <tr>
                            <th colspan="5">

                                <asp:QueryExtender ID="QueryExtender1" runat="server">
                                </asp:QueryExtender>

                            </th>
                        </tr>--%>
                        <tr>
                            <td colspan="5">
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="700px" Width="1000px" BorderStyle="Solid" BorderWidth="1px">
                                </rsweb:ReportViewer>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:GridView ID="GridPrjList" runat="server" CssClass="mGrid"></asp:GridView>
                            </td>
                        </tr>                        
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnShow" EventName="Click" />
                    <%--<asp:AsyncPostBackTrigger ControlID="BtnShowCR" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="BtnShowWS" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="BtnShowsQE" EventName="Click" />--%>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>


</asp:Content>

