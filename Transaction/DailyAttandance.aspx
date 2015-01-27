<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="DailyAttandance.aspx.cs" Inherits="Transaction_DailyAttandance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderhead" runat="Server">

    <style type="text/css">
        .ModGridview {
            font-family: Verdana;
            font-size: 10pt;
            font-weight: normal;
            color: black;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }
    </style>

    <script type="text/javascript">
        function closepopup() {
            $find('ModalPopupExtender1').hide();
        }

        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do You Want To Delete This Entry ? ")) {
                confirm_value.value = "Yes";
            }
            else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

        function validate() {
            var str = "";
            if (document.getElementById("<%=ddlEmployee.ClientID%>").value == "0") {
                str = str + "Employee Name Is Required. \n";
            }
            if (document.getElementById("<%=TxtDate.ClientID%>").value == "") {
                str = str + "Date Is Required. \n";
            }
            if (document.getElementById("<%=TxtInTime.ClientID%>").value == "") {
                str = str + "In Time Is Required. \n";
            }
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
            <div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Menu ID="MyMenu" Width="250px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
                            OnMenuItemClick="MyMenu_MenuItemClick" RenderingMode="Table" BorderStyle="None">
                            <Items>
                                <asp:MenuItem ImageUrl="~/images/ViewEnable.jpg" Value="0" Text=" "></asp:MenuItem>
                                <asp:MenuItem ImageUrl="~/images/NewOrEditDisable.jpg" Text=" " Value="1"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                        <div style="border: 1px solid #C0C0C0;">
                            <asp:MultiView ID="MyMultiView" runat="server" ActiveViewIndex="0">
                                <asp:View ID="TabView" runat="server">
                                    <table>
                                        <tr id="EmpSearchRow" runat="server">
                                            <th style="text-align: right;">Employee</th>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="DDLEmpView" runat="server" Width="300px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
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
                                            <td>
                                                <asp:Button ID="BtnView" runat="server" Text="View" CssClass="buttonglossy" Height="25px" OnClick="BtnView_Click" />
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <td colspan="5">
                                                <asp:GridView ID="GridInOut" runat="server"
                                                    AutoGenerateColumns="False"
                                                    AllowPaging="true"
                                                    PageSize="25"
                                                    DataKeyNames="Id"
                                                    OnPageIndexChanging="GridInOut_PageIndexChanging"
                                                    CssClass="mGrid"
                                                    EmptyDataText="No Data Available">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnEdit" runat="server" OnClick="btnInOutSelect_Click" CommandName="Select" AlternateText="Edit" ImageUrl="~/images/Edit.jpg" ToolTip="Edit" Height="20px" Width="20px" />
                                                                <asp:ImageButton ID="imgbtnDelete" runat="server" OnClick="btnInOutDelete_Click" AlternateText="Delete" ImageUrl="~/images/Delete.png" ToolTip="Delete" Height="20px" Width="20px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Id" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Employee Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdEmpName" runat="server" Width="200px" Text='<%# Bind("EmpName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdEmpName" runat="server" Width="200px" BorderStyle="None" Text='<%# Bind("EmpName") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdDate" runat="server" Width="200px" Text='<%# Bind("WorkDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdDate" runat="server" Width="200px" BorderStyle="None" Text='<%# Bind("WorkDate") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="In Time">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdInTime" runat="server" Width="100px" Text='<%# Bind("InTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdInTime" runat="server" Width="100px" BorderStyle="None" Text='<%# Bind("InTime") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OutTime">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdOutTime" runat="server" Width="100px" Text='<%# Bind("OutTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdOutTime" runat="server" Width="100px" BorderStyle="None" Text='<%# Bind("OutTime") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="TotTime">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdTotTime" runat="server" Width="100px" Text='<%# Bind("TotTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdTotTime" runat="server" Width="100px" BorderStyle="None" Text='<%# Bind("TotTime") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
                                                    CancelControlID="btnno" BackgroundCssClass="modalBackground">
                                                </asp:ModalPopupExtender>
                                                <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="130px" Width="400px" Style="display: none;">
                                                    <table style="border: Solid 2px #D46900; width: 100%; height: 100%">
                                                        <tr style="background-image: url('../images/header-js.gif')">
                                                            <td style="height: 10%; color: White; font-weight: bold; padding: 3px; font-size: larger; font-family: Calibri; text-align: left;">Confirm Box
                                                            </td>
                                                            <td style="color: White; font-weight: bold; padding: 3px; font-size: larger; text-align: right;">
                                                                <a href="javascript:void(0)" onclick="closepopup()">
                                                                    <img src="../images/Close-js.gif" style="border: 0px; text-align: right;" /></a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="text-align: left; padding: 5px; font-family: Calibri">
                                                                <asp:Label ID="lblUser" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2"></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td style="text-align: right; padding-right: 15px">
                                                                <asp:ImageButton ID="btnYes" OnClick="btnYes_Click" runat="server" ImageUrl="~/images/btnyes-js.jpg" />
                                                                <asp:ImageButton ID="btnNo" runat="server" ImageUrl="~/images/btnNo-js.jpg" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="TabNewEdit" runat="server">
                                    <table>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:HiddenField ID="HidFldId" runat="server" />
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="HidFldUID" runat="server" />
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <td colspan="5" style="text-align: left;">
                                                <asp:Label ID="LblMsg" runat="server" Text="..." Font-Bold="True" Font-Size="Small" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Employee</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:DropDownList ID="ddlEmployee" runat="server" Width="300px" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Office Time From</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtOffFTime" runat="server" Width="100px" Enabled="false" BackColor="White"></asp:TextBox>
                                            </td>
                                            <th style="text-align: right;">To</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtOffTTime" runat="server" Width="100px" Enabled="false" BackColor="White"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Office Hours</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtOffHours" runat="server" Width="100px" Enabled="false" BackColor="White"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Date</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtDate" runat="server" Width="175px" AutoPostBack="true" OnTextChanged="TxtDate_TextChanged"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnCalendar2" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar2" PopupPosition="BottomRight" TargetControlID="TxtDate" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Days</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtDays" runat="server" Width="100px" Enabled="false" BackColor="White"></asp:TextBox>
                                            </td>
                                            <th style="text-align: right;">Month</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtMonth" runat="server" Width="100px" Enabled="false" BackColor="White"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">In Time</th>
                                            <td style="text-align: left; vertical-align: top;">
                                                <%--<asp:TextBox ID="TxtInTime" runat="server" Width="100px" TextMode="Time" onblur="CalcTotHours(this);"></asp:TextBox>--%>
                                                <asp:TextBox ID="TxtInTime" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="TxtInTime_TextChanged"></asp:TextBox>
                                            </td>
                                            <th style="text-align: right;">Out Time</th>
                                            <td style="text-align: left;">
                                                <%--<asp:TextBox ID="TxtOutTime" runat="server" Width="100px" TextMode="Time" onblur="CalcTotHours(this);"></asp:TextBox>--%>
                                                <asp:TextBox ID="TxtOutTime" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="TxtOutTime_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Total Working Hours</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtTotWrkHours" runat="server" Width="100px" Enabled="false" BackColor="White"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Over Time</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtOverTime" runat="server" Width="100px" Enabled="false" BackColor="White"></asp:TextBox>
                                            </td>
                                            <th style="text-align: right;">Less Time</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtLessTime" runat="server" Width="100px" Enabled="false" BackColor="White"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Reason</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtReason" runat="server" Width="283px" TextMode="MultiLine" Height="55px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Remark</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtRemark" runat="server" Width="283px" TextMode="MultiLine" Height="55px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="BtnRow" style="vertical-align: top">
                                            <td colspan="5" style="text-align: center;">
                                                <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="button black" Height="25px" Width="77px" OnClick="BtnSave_Click" OnClientClick="return validate();" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" CssClass="button black" Height="25px" Width="77px" OnClick="BtnNew_Click" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="button black" Height="25px" Width="77px" OnClick="BtnCancel_Click" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnDelete" runat="server" Text="Delete" CssClass="button black" Height="25px" Width="77px" OnClick="BtnDelete_Click" OnClientClick="Confirm()" /></td>
                                        </tr>
                                        <tr runat="server" id="trNavigation" style="vertical-align: top">                                            
                                            <th colspan="2"></th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:ImageButton ID="ImgBtnFirst" runat="server" Height="40px" Width="50px" AlternateText="<" ToolTip="First" ImageAlign="Middle" OnClick="ImgBtnFirst_Click" ImageUrl="~/images/FirstG.png" BorderColor="#00CC00" BorderStyle="Double" />                                                
                                                &nbsp;&nbsp;<asp:ImageButton ID="ImgBtnPrev" runat="server" Height="40px" Width="50px" AlternateText="<<" ToolTip="Prev" ImageAlign="Middle" OnClick="ImgBtnPrev_Click" ImageUrl="~/images/PrevG.png" BorderColor="#00CC00" BorderStyle="Double" />
                                                &nbsp;&nbsp;<asp:ImageButton ID="ImgBtnNext" runat="server" Height="40px" Width="50px" AlternateText=">>" ToolTip="Next" ImageAlign="Middle" OnClick="ImgBtnNext_Click" ImageUrl="~/images/NextG.png" BorderColor="#00CC00" BorderStyle="Double" />
                                                &nbsp;&nbsp;<asp:ImageButton ID="ImgBtnLast" runat="server" Height="40px" Width="50px" AlternateText=">" ToolTip="Last" ImageAlign="Middle" OnClick="ImgBtnLast_Click" ImageUrl="~/images/LastG.png" BorderColor="#00CC00" BorderStyle="Double" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                            </asp:MultiView>
                        </div>
                    </ContentTemplate>
                    <%-- <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnNew" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnDelete" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlEmployee" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="TxtDate" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="TxtInTime" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="TxtOutTime" EventName="TextChanged" />
                    </Triggers>--%>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

