<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="DRFEntry.aspx.cs" Inherits="Transaction_DRFEntry" %>

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

        function CalcTotHours(fieldName) {
            var InTime = document.getElementById("<%=TxtInTime.ClientID %>").value;
            var OutTime = document.getElementById("<%=TxtOutTime.ClientID %>").value;
            var TotHours = InTime - OutTime;
            document.getElementById("<%=TxtTotWrkHours.Text%>").value = TotHours;
        }

        function validate() {

            var str = "";
            if (document.getElementById("<%=DDLEmplName.ClientID%>").value == "0") {
                str = str + "Employee Is Required. \n";
            }
            if (document.getElementById("<%=TxtDRFDate.ClientID%>").value == "") {
                str = str + "DRF Date Is Required. \n";
            }
            if (document.getElementById("<%=TxtInTime.ClientID%>").value == "") {
                str = str + "In Time Is Required. \n";
            }
            if (document.getElementById("<%=TxtOutTime.ClientID%>").value == "") {
                str = str + "Out Time Is Required. \n";
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
                                        <tr id="EmpSearchRow" runat="server" style="text-align: left;">
                                            <th style="text-align: right;">Employee</th>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="DDLEmpView" runat="server" Width="300px"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="BtnView" runat="server" Text="View All" CssClass="buttonglossy" Height="25px" Width="77px" OnClick="BtnView_Click" />
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <td colspan="3">
                                                <asp:GridView ID="GridDRF" runat="server"
                                                    AutoGenerateColumns="False"
                                                    DataKeyNames="Id"
                                                    OnPageIndexChanging="GridDRF_PageIndexChanging"
                                                    CssClass="mGrid"
                                                    PageSize="15"
                                                    EmptyDataText="No Data Available">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnEdit" runat="server" OnClick="btnDRFSelect_Click" AlternateText="Edit" CommandName="Select" ImageUrl="~/images/Edit.jpg" ToolTip="Edit" Height="20px" Width="20px" />
                                                                <asp:ImageButton ID="imgbtnDelete" runat="server" OnClick="btnDRFDelete_Click" AlternateText="Delete" ImageUrl="~/images/Delete.png" ToolTip="Delete" Height="20px" Width="20px" />
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
                                                        <asp:TemplateField HeaderText="DRF Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdDrfDate" runat="server" Width="100px" Text='<%# Bind("DRFDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdDrfDate" runat="server" Width="100px" BorderStyle="None" Text='<%# Bind("DRFDate") %>'></asp:TextBox>
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
                                            <td style="text-align: left;">
                                                <asp:HiddenField ID="HidFldId" runat="server" />
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="HidFldUID" runat="server" />
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: right; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Employee Name</th>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="DDLEmplName" runat="server" Enabled="false" Width="331px" Height="16px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: right; color: #FF0000;">*</th>
                                            <th style="text-align: left;">DRF Date</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtDRFDate" runat="server" Width="100px"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnCalendar2" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar2" PopupPosition="BottomRight" TargetControlID="TxtDRFDate" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: right; color: #FF0000;">*</th>
                                            <th style="text-align: left;">In Time</th>
                                            <td style="text-align: left; vertical-align: top;">
                                                <asp:TextBox ID="TxtInTime" runat="server" Width="100px" OnTextChanged="TxtInTime_TextChanged" AutoPostBack="true"></asp:TextBox><%--TextMode="Time" onblur="CalcTotHours(this);--%>
                                            </td>

                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: right; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Out Time</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtOutTime" runat="server" Width="100px" OnTextChanged="TxtOutTime_TextChanged" AutoPostBack="true"></asp:TextBox><%--TextMode="Time" onblur="CalcTotHours(this);"--%>
                                            </td>

                                        </tr>
                                        <tr>
                                            <th></th>
                                            <th style="text-align: left;">Total Working Hours</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtTotWrkHours" runat="server" Width="100px" Enabled="false" BackColor="White"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Remark</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtRemark" runat="server" Width="500px" TextMode="MultiLine" Height="55px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="text-align: left;">
                                            <td colspan="3" style="text-align: left;">
                                                <asp:Panel ID="ModulePanel" runat="server" ScrollBars="Auto">

                                                    <asp:GridView ID="GridWork" runat="server"
                                                        DataKeyNames="Id,DRFID"
                                                        AutoGenerateColumns="false" HeaderStyle-BackColor="#61A6F8"
                                                        ShowFooter="true" HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="White"
                                                        OnRowCancelingEdit="GridWork_RowCancelingEdit"
                                                        OnRowEditing="GridWork_RowEditing"
                                                        OnRowUpdating="GridWork_RowUpdating"
                                                        OnRowCommand="GridWork_RowCommand"
                                                        OnRowDeleting="GridWork_RowDeleting"
                                                        CssClass="ModGridview"
                                                        PageSize="5" OnRowDataBound="GridWork_RowDataBound">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                                <EditItemTemplate>
                                                                    <asp:Label ID="LblEditId" runat="server" Text='<%#Eval("Id") %>' />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblItmId" runat="server" Text='<%#Eval("Id") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="LblFtrId" runat="server" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="DRF Id" Visible="false">
                                                                <EditItemTemplate>
                                                                    <asp:Label ID="LblEditDRFId" runat="server" Text='<%#Eval("DRFId") %>' />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblItmDRFId" runat="server" Text='<%#Eval("DRFId") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="LblFtrDRFId" runat="server" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Start Time">
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="TxtEditStartTime" runat="server" Width="80px" Text='<%#Eval("StartTime") %>' />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblItmStartTime" runat="server" Text='<%#Eval("StartTime") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="TxtFtrStartTime" runat="server" Width="80px" />
                                                                    <asp:RequiredFieldValidator ID="RFvStartTime" runat="server" ControlToValidate="TxtFtrStartTime" Text="*" ForeColor="red" ValidationGroup="validaiton" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="End Time">
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="TxtEditEndTime" runat="server" Width="80px" Text='<%#Eval("EndTime") %>' />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblItmEndTime" runat="server" Text='<%#Eval("EndTime") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="TxtFtrEndTime" runat="server" Width="80px" />
                                                                    <asp:RequiredFieldValidator ID="RFvEndTime" runat="server" ControlToValidate="TxtFtrEndTime" Text="*" ForeColor="red" ValidationGroup="validaiton" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Total Time">
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="TxtEditTotTime" runat="server" Width="80px" Text='<%#Eval("TotTime") %>' />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblItmTotTime" runat="server" Text='<%#Eval("TotTime") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="TxtFtrTotTime" runat="server" Width="80px" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Project Id" Visible="false">
                                                                <EditItemTemplate>
                                                                    <asp:Label ID="LblEditPrjId" runat="server" Text='<%#Eval("PrjId") %>' />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblItmPrjId" runat="server" Text='<%#Eval("PrjId") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="TxtFtrPrjId" runat="server" Width="100px" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Project Name">
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="DDlEditPrjName" runat="server" Height="22px" Width="150px"
                                                                        AutoPostBack="true"
                                                                        OnSelectedIndexChanged="DDlEditPrjName_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblItmPrjName" runat="server" Text='<%#Eval("PrjName") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="DDlFtrPrjName" runat="server" Height="22px" Width="150px"
                                                                        AutoPostBack="true"
                                                                        OnSelectedIndexChanged="DDlFtrPrjName_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RFvPrjName" runat="server" ControlToValidate="DDlFtrPrjName" Text="*" ForeColor="red" ValidationGroup="validaiton" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Module Id" Visible="false">
                                                                <EditItemTemplate>
                                                                    <asp:Label ID="LblEditModId" runat="server" Text='<%#Eval("ModId") %>' />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblItmModId" runat="server" Text='<%#Eval("ModId") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="TxtFtrModId" runat="server" Width="100px" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Module Name">
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="DDlEditModName" runat="server" Height="22px" Width="150px">
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblItmModName" runat="server" Text='<%#Eval("ModName") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="DDlFtrModName" runat="server" Height="22px" Width="150px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RFvModName" runat="server" ControlToValidate="DDlFtrModName" Text="*" ForeColor="red" ValidationGroup="validaiton" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Work Description">
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="TxtEditWorkDesc" runat="server" Width="100px" Text='<%#Eval("WorkDesc") %>' />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblItmWorkDesc" runat="server" Text='<%#Eval("WorkDesc") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="TxtFtrWorkDesc" runat="server" Width="100px" />
                                                                    <asp:RequiredFieldValidator ID="RFvWorkDesc" runat="server" ControlToValidate="TxtFtrWorkDesc" Text="*" ForeColor="red" ValidationGroup="validaiton" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Work Status">
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="DDlEditWorkStat" runat="server" Height="26px" Width="90px">
                                                                        <asp:ListItem Value="0">---Select---</asp:ListItem>
                                                                        <asp:ListItem Value="D">Done</asp:ListItem>
                                                                        <asp:ListItem Value="P">Pending</asp:ListItem>
                                                                        <asp:ListItem Value="C">Cancel</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblItmWorkStat" runat="server" Text='<%#Eval("WorkStatus") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="DDlFtrWorkStat" runat="server" Height="26px" Width="90px">
                                                                        <asp:ListItem Value="0">---Select---</asp:ListItem>
                                                                        <asp:ListItem Value="D">Done</asp:ListItem>
                                                                        <asp:ListItem Value="P">Pending</asp:ListItem>
                                                                        <asp:ListItem Value="C">Cancel</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RFvWorkStat" runat="server" ControlToValidate="DDlFtrWorkStat" Text="*" ForeColor="red" ValidationGroup="validaiton" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Remark">
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="TxtEditRemark" runat="server" Width="100px" Text='<%#Eval("Remark") %>' />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblItmRemark" runat="server" Text='<%#Eval("Remark") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="TxtFtrRemark" runat="server" Width="100px" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <EditItemTemplate>
                                                                    <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update" AlternateText="Select" ImageUrl="~/images/update.jpg" ToolTip="Update" Height="20px" Width="20px" />
                                                                    <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" AlternateText="Cancel" ImageUrl="~/images/cancel.jpg" ToolTip="Cancel" Height="20px" Width="20px" />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit" AlternateText="Edit" ImageUrl="~/images/Edit.jpg" ToolTip="Edit" Height="20px" Width="20px" />
                                                                    <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete" AlternateText="Delete"
                                                                        ImageUrl="~/images/Delete-js.png" ToolTip="Delete" Height="20px" Width="20px"
                                                                        OnClientClick="return confirm('Are you sure you want to delete this entry ?');" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:ImageButton ID="imgbtnAdd" runat="server" CommandName="AddNew" AlternateText="Add New" ImageUrl="~/images/AddNewitem.jpg" Width="30px" Height="30px" ToolTip="Add New Entry" ValidationGroup="validaiton" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                        </Columns>

                                                        <HeaderStyle BackColor="#61A6F8" Font-Bold="True" ForeColor="White" />

                                                    </asp:GridView>


                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: center;">Total Time</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtTotTime" runat="server" Width="150px" Enabled="false" BackColor="White"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="BtnRow" style="vertical-align: top">
                                            <th></th>
                                            <th></th>
                                            <td style="text-align: left;">
                                                <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="button black" Height="25px" Width="77px" OnClick="BtnSave_Click" OnClientClick="return validate();" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" CssClass="button black" Height="25px" Width="77px" OnClick="BtnNew_Click" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="button black" Height="25px" Width="77px" OnClick="BtnCancel_Click" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnDelete" runat="server" Text="Delete" CssClass="button black" Height="25px" Width="77px" OnClick="BtnDelete_Click" OnClientClick="Confirm()" />
                                            </td>
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
                    </Triggers>--%>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</asp:Content>

