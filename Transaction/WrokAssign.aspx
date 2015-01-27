<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="WrokAssign.aspx.cs" Inherits="Transaction_WrokAssign" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderhead" runat="Server">
    <style type="text/css">
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

        function CalcDueDays(fieldName) {
            if (fieldName.value == "") return;
            var dateArr = document.getElementById("<%=TxtAssignDate.ClientID %>").value;
            var parts = dateArr.split("/");
            //date constructor in (Year,Month,Date) format
            //-1 in Month because in Date Constructor the month Offset start from 0
            var BillDate = new Date(parseInt(parts[2]), parseInt(parts[1] - 1), parseInt(parts[0]));
            var dueDate = BillDate;
            dueDate.setDate(BillDate.getDate() + parseInt(fieldName.value));
            dueDate = dueDate.getDate() + "/" + (dueDate.getMonth() + 1) + "/" + dueDate.getFullYear();
            document.getElementById("<%=TxtDueDate.ClientID %>").value = dueDate;
        }

        function validate() {
            var str = "";
            if (document.getElementById("<%=DDLPrjLed.ClientID%>").value == "0") {
                str = str + "Select Project Ledger. \n";
            }
            if (document.getElementById("<%=DDLPrjName.ClientID%>").value == "0") {
                str = str + "Select Project Name. \n";
            }
            if (document.getElementById("<%=DDLPrjModule.ClientID%>").value == "0") {
                str = str + "Select Project Module. \n";
            }
            if (document.getElementById("<%=ddlEmployee.ClientID%>").value == "0") {
                str = str + "Select Employee Name. \n";
            }

            if (document.getElementById("<%=TxtAssignDate.ClientID%>").value == "") {
                str = str + "Assign Date Is Blank, Enter Valid Date. \n";
            }
            if (document.getElementById("<%=TxtDueDays.ClientID%>").value == "") {
                str = str + "Due Days Is Zero, Enter Valid Days. \n";
            }
            if (document.getElementById("<%=TxtWrkDetails1.ClientID%>").value == "") {
                str = str + "Enter Working Details. \n";
            }
            if (document.getElementById("<%=ddlWrkStatus.ClientID%>").value == "0") {
                str = str + "Select Work Status. \n";
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
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="LblMsg" runat="server" Text="..." Font-Bold="True" Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
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
                                        <tr>
                                            <td>
                                                <asp:Button ID="BtnView" runat="server" Text="View All" CssClass="button black" Height="25px" Width="77px" OnClick="BtnView_Click" />
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <td>
                                                <%--AutoGenerateDeleteButton="True" AutoGenerateSelectButton="True"
                                                    OnRowDeleting="GridWork_RowDeleting"
                                                    OnSelectedIndexChanged="GridWork_SelectedIndexChanged"--%>
                                                <asp:GridView ID="GridWork" runat="server"
                                                    AutoGenerateColumns="False"
                                                    DataKeyNames="Id" PageSize="15"
                                                    OnPageIndexChanging="GridWork_PageIndexChanging"
                                                    CssClass="mGrid"
                                                    EmptyDataText="No Data Available">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnEdit" runat="server" OnClick="btnWorkSelect_Click" AlternateText="Edit" CommandName="Select" ImageUrl="~/images/Edit.jpg" ToolTip="Edit" Height="20px" Width="20px" />
                                                                <asp:ImageButton ID="imgbtnDelete" runat="server" OnClick="btnWorkDelete_Click" AlternateText="Delete" ImageUrl="~/images/Delete.png" ToolTip="Delete" Height="20px" Width="20px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Id" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Prj.Led.Id" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdPrjLedId" runat="server" Text='<%# Bind("PrjLedId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Prj.Id" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdPrjId" runat="server" Text='<%# Bind("PrjId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Prj.Mod.Id" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdPrjModId" runat="server" Text='<%# Bind("PrjModId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Emp.Id" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdEmpId" runat="server" Text='<%# Bind("EmpId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Project Led.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdPrjLed" runat="server" Text='<%# Bind("ProjectLed") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Project Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdPrjName" runat="server" Text='<%# Bind("ProjectName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Module Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdModName" runat="server" Text='<%# Bind("ModuleName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="EmpName">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdEmpName" runat="server" Text='<%# Bind("EmpName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CompleteDate">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdCompleteDate" runat="server" Text='<%# Bind("CompleteDate") %>'></asp:Label>
                                                            </ItemTemplate>
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
                                            <td style="text-align: left;">
                                                <asp:HiddenField ID="HidFldId" runat="server" />
                                            </td>
                                            <td colspan="4">
                                                <asp:HiddenField ID="HidFldUID" runat="server" />
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Projct Ledger</th>
                                            <td colspan="4" style="text-align: left;">
                                                <asp:DropDownList ID="DDLPrjLed" runat="server" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Project Name</th>
                                            <td colspan="4" style="text-align: left;">
                                                <asp:DropDownList ID="DDLPrjName" runat="server" Width="300px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="DDLPrjName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Project Module</th>
                                            <td colspan="4" style="text-align: left;">
                                                <asp:DropDownList ID="DDLPrjModule" runat="server" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Client Name</th>
                                            <td colspan="4" style="text-align: left;">
                                                <asp:TextBox ID="TxtClientName" runat="server" Width="300px" Style="margin-bottom: 0px" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Employee</th>
                                            <td colspan="4" style="text-align: left;">
                                                <asp:DropDownList ID="ddlEmployee" runat="server" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Assign Date</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtAssignDate" runat="server" Width="100px" onblur="CalcDueDays(this);"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnCalendar1" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar1" PopupPosition="BottomLeft" TargetControlID="TxtAssignDate" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <th style="text-align: right;">Assign Time</th>
                                            <td colspan="2" style="text-align: left; vertical-align: top;">
                                                <asp:TextBox ID="TxtAssignTime" runat="server" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Due Days</th>
                                            <td style="text-align: left; vertical-align: top;">
                                                <asp:TextBox ID="TxtDueDays" runat="server" Width="100px" onblur="CalcDueDays(this);"></asp:TextBox>
                                            </td>
                                            <th style="text-align: right;">Due Date</th>
                                            <td colspan="2" style="text-align: left;">
                                                <asp:TextBox ID="TxtDueDate" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                <%--<asp:ImageButton ID="imgbtnCalendar2" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar2" PopupPosition="BottomLeft" TargetControlID="TxtDueDate" Enabled="True">
                                                </asp:CalendarExtender>--%>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">1. Work Details</th>
                                            <td colspan="4" style="text-align: left; vertical-align: top;">
                                                <asp:TextBox ID="TxtWrkDetails1" runat="server" Width="500px" Height="35px"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;"></th>
                                            <th style="text-align: left;">2. Work Details</th>
                                            <td colspan="4" style="text-align: left; vertical-align: top;">
                                                <asp:TextBox ID="TxtWrkDetails2" runat="server" Width="500px" Height="35px"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;"></th>
                                            <th style="text-align: left;">3. Work Details</th>
                                            <td colspan="4" style="text-align: left; vertical-align: top;">
                                                <asp:TextBox ID="TxtWrkDetails3" runat="server" Width="500px" Height="35px"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Remark</th>
                                            <td colspan="4" style="text-align: left;">
                                                <asp:TextBox ID="TxtRemark" runat="server" Width="500px" TextMode="MultiLine" Height="35px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Working Status</th>
                                            <td colspan="4" style="text-align: left;">
                                                <asp:DropDownList ID="ddlWrkStatus" runat="server" Width="150px">
                                                    <asp:ListItem Value="0">---Select Status---</asp:ListItem>
                                                    <asp:ListItem Value="D">Done</asp:ListItem>
                                                    <asp:ListItem Value="P">Pending</asp:ListItem>
                                                    <asp:ListItem Value="C">Cancel</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>


                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Complete Date</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtCompleteDate" runat="server" Width="100px"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnCalendar3" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar3" PopupPosition="BottomRight" TargetControlID="TxtCompleteDate" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <th style="text-align: right;">Complete Time</th>
                                            <td style="text-align: left; vertical-align: top;">
                                                <asp:TextBox ID="TxtCompleteTime" runat="server" Width="100px"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:Button ID="BtnUpdate" runat="server" Text="Update" CssClass="button black" Width="77px" OnClick="BtnUpdate_Click" />
                                            </td>
                                        </tr>

                                        <tr runat="server" id="BtnRow" style="vertical-align: top">
                                            <td colspan="6" style="text-align: center;">
                                                <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="button black" Height="25px" Width="77px" OnClick="BtnSave_Click" OnClientClick="return validate();" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" CssClass="button black" Height="25px" Width="77px" OnClick="BtnNew_Click" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="button black" Height="25px" Width="77px" OnClick="BtnCancel_Click" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnDelete" runat="server" Text="Delete" CssClass="button black" Height="25px" Width="77px" OnClick="BtnDelete_Click" OnClientClick="Confirm()" /></td>
                                        </tr>

                                        <tr runat="server" id="trNavigation" style="vertical-align: top">
                                            <th colspan="2"></th>
                                            <td colspan="4" style="text-align: left;">
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
                    <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnNew" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnDelete" EventName="Click" />
                    </Triggers>--%>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</asp:Content>

