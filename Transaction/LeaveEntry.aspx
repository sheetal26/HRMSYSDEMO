<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="LeaveEntry.aspx.cs" Inherits="Transaction_LeaveEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderhead" runat="Server">
   
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

            if (document.getElementById("<%=ddlEmployee.ClientID%>").value == "") {
                str = str + "Employee Is Required. \n";
            }
            if (document.getElementById("<%=TxtDate.ClientID%>").value == "") {
                str = str + "Application Date Is Required. \n";
            }
            if (document.getElementById("<%=TxtFromDate.ClientID%>").value == "") {
                str = str + "From Date Is Required. \n";
            }
            if (document.getElementById("<%=TxtTotDays.ClientID%>").value == "0") {
                str = str + "Total Days Is Zero. \n";
            }
            if (document.getElementById("<%=TxtReason.ClientID%>").value == "") {
                str = str + "Reason Is Required. \n";
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
                                        <tr>
                                            <td>
                                                <asp:Button ID="BtnView" runat="server" Text="View All" CssClass="button black" Height="25px" Width="77px" OnClick="BtnView_Click" />
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <td>
                                                <asp:GridView ID="GridLeave" runat="server"
                                                    AutoGenerateColumns="False" PageSize="15"
                                                    DataKeyNames="Id"
                                                    OnPageIndexChanging="GridLeave_PageIndexChanging"
                                                    CssClass="mGrid"
                                                    EmptyDataText="No Data Available">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnEdit" runat="server" OnClick="btnLeaveSelect_Click" CommandName="Select" AlternateText="Edit" ImageUrl="~/images/Edit.jpg" ToolTip="Edit" Height="20px" Width="20px" />
                                                                <asp:ImageButton ID="imgbtnDelete" runat="server" OnClick="btnLeaveDelete_Click" AlternateText="Delete" ImageUrl="~/images/Delete.png" ToolTip="Delete" Height="20px" Width="20px" />
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
                                                                <asp:Label ID="LblGrdEName" runat="server" Width="200px" Text='<%# Bind("EmpName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdEName" runat="server" Width="200px" BorderStyle="None" Text='<%# Bind("EmpName") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Application Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdAppDate" runat="server" Width="150px" Text='<%# Bind("Application_Date") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdAppDate" runat="server" Width="150px" BorderStyle="None" Text='<%# Bind("Application_Date") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="From Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdFromDate" runat="server" Width="150px" Text='<%# Bind("FromDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdFromDate" runat="server" Width="150px" BorderStyle="None" Text='<%# Bind("FromDate") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdToDate" runat="server" Width="150px" Text='<%# Bind("ToDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdToDate" runat="server" Width="150px" BorderStyle="None" Text='<%# Bind("ToDate") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Tot Days">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdTotDays" runat="server" Width="150px" Text='<%# Bind("TotalDays") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdTotDays" runat="server" Width="200px" BorderStyle="None" Text='<%# Bind("TotalDays") %>'></asp:TextBox>
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
                                            <td colspan="2">
                                                <asp:HiddenField ID="HidFldUID" runat="server" />
                                            </td>
                                        </tr>

                                        <tr style="vertical-align: top">
                                            <td colspan="6" style="text-align: left;">
                                                <asp:Label ID="LblMsg" runat="server" Text="..." Font-Bold="True" Font-Size="Small" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Employee</th>
                                            <td colspan="4" style="text-align: left;">
                                                <asp:DropDownList ID="ddlEmployee" runat="server" Width="300px"
                                                    data-bvalidator="required" data-bvalidator-msg="Please Select Employee">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: right; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Application Date</th>
                                            <td colspan="4" style="text-align: left;">
                                                <asp:TextBox ID="TxtDate" runat="server" Width="175px" AutoPostBack="true"
                                                    data-bvalidator="required" data-bvalidator-msg="Enter Application Date In dd/mm/yyyy Format"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnCalendar1" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar1" PopupPosition="BottomRight" TargetControlID="TxtDate" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">From Date</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtFromDate" runat="server" Width="100px" Enabled="true" BackColor="White"
                                                    data-bvalidator="required" data-bvalidator-msg="Enter From Date In dd/mm/yyyy Format"
                                                    OnTextChanged="TxtFromDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnCalendar2" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar2" PopupPosition="BottomRight" TargetControlID="TxtFromDate" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <th style="text-align: right; color: #FF0000;">*</th>
                                            <th style="text-align: left;">To Date</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtToDate" runat="server" Width="100px" Enabled="true" BackColor="White"
                                                    data-bvalidator="required" data-bvalidator-msg="Enter To Date In dd/mm/yyyy Format"
                                                    OnTextChanged="TxtToDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnCalendar3" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar3" PopupPosition="BottomRight" TargetControlID="TxtToDate" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Total Days</th>
                                            <td colspan="4" style="text-align: left;">
                                                <asp:TextBox ID="TxtTotDays" runat="server" Width="100px" Enabled="false" BackColor="White"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th></th>
                                            <th style="text-align: left;">Type</th>
                                            <td colspan="4">
                                                <asp:DropDownList ID="DDLLeaveType" runat="server" Width="200px" Height="22px">
                                                    <asp:ListItem Value="">Select Type</asp:ListItem>
                                                    <asp:ListItem Value="H">Half Day</asp:ListItem>
                                                    <asp:ListItem Value="F">Full Day</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Reason</th>
                                            <td colspan="4" style="text-align: left;">
                                                <asp:TextBox ID="TxtReason" runat="server" Width="283px" TextMode="MultiLine" Height="55px"
                                                    data-bvalidator="required" data-bvalidator-msg="Enter Valid Reason">  
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th></th>
                                            <th style="text-align: left;">Status</th>
                                            <td colspan="4">
                                                <asp:DropDownList ID="DDLStatus" runat="server" Width="200px" Height="22px">
                                                    <asp:ListItem Value="">Select Status</asp:ListItem>
                                                    <asp:ListItem Value="A">Approve</asp:ListItem>
                                                    <asp:ListItem Value="U">UnApproved</asp:ListItem>
                                                    <asp:ListItem Value="C">Cancel</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Remark</th>
                                            <td colspan="4" style="text-align: left;">
                                                <asp:TextBox ID="TxtRemark" runat="server" Width="283px" TextMode="MultiLine" Height="55px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th></th>
                                            <td colspan="4" runat="server" id="BtnRow" style="text-align: center;">
                                                <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="button black" Height="25px" Width="77px" OnClick="BtnSave_Click" OnClientClick="return validate();" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" CssClass="button black" Height="25px" Width="77px" OnClick="BtnNew_Click" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="button black" Height="25px" Width="77px" OnClick="BtnCancel_Click" />
                                                &nbsp;&nbsp;<asp:Button ID="BtnDelete" runat="server" Text="Delete" CssClass="button black" Height="25px" Width="77px" OnClick="BtnDelete_Click" OnClientClick="Confirm()" />
                                                <%--&nbsp;&nbsp;<input type="reset" value="Reset" class="button black" style="height:25px; width:77px;">
                                                --%>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trNavigation" style="vertical-align: top">
                                            <th colspan="2"></th>
                                            <td colspan="4" style="text-align: center;">
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
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnNew" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnDelete" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>

</asp:Content>

