<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="MthAttSalGen.aspx.cs" Inherits="Transaction_MthAttSalGen" %>

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

        function validate()
        {
            var str = "";
            if (document.getElementById("<%=ddlyear.ClientID%>").value == "0") {
                str = str + "Select Valid Year. \n";
            }
            if (document.getElementById("<%=ddlmonth.ClientID%>").value == "0") {
                str = str + "Select Valid Month. \n";
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
                        <div style="border: 1px solid #C0C0C0;">

                            <table>
                                <tr style="vertical-align: top">
                                    <th></th>
                                    <td colspan="4" style="text-align: left;">
                                        <asp:HiddenField ID="HidFldId" runat="server" />
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="HidFldUID" runat="server" />
                                    </td>
                                </tr>
                                <tr style="vertical-align: top">
                                    <td colspan="6" style="text-align: left;">
                                        <asp:Label ID="LblMsg" runat="server" Text="..." Font-Bold="True" Font-Size="Small" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="vertical-align: top">
                                    <th style="text-align: right; color: #FF0000;">*</th>
                                    <th style="text-align: left;">Year</th>
                                    <td style="text-align: left;">
                                        <%--<asp:TextBox ID="TxtYear" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="TxtYear_TextChanged"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddlyear" runat="server" OnSelectedIndexChanged="ddlyear_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>  
                                    <th style="text-align: right; color: #FF0000;">*</th>
                                    <th style="text-align: left;">Month</th>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlmonth" runat="server" OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged" AutoPostBack="true">
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
                                </tr>
                            
                                <tr style="vertical-align: top">
                                    <th style="text-align: left; color: #FF0000;"></th>
                                    <th style="text-align: left;">From Date</th>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="TxtFDate" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <th style="text-align: left; color: #FF0000;"></th>
                                    <th style="text-align: left;">To</th>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="TxtTDate" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="vertical-align: top">
                                    <th style="text-align: right; color: #FF0000;">*</th>
                                    <th style="text-align: left;">Employee</th>
                                    <td colspan="4" style="text-align: left;">
                                        <asp:DropDownList ID="ddlEmployee" runat="server" Width="300px" AutoPostBack="True"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="vertical-align: top">
                                    <th></th>
                                    <th style="text-align: left;">Remark</th>
                                    <td colspan="4" style="text-align: left;">
                                        <asp:TextBox ID="TxtRemark" runat="server" Width="296px" TextMode="MultiLine" Height="55px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr runat="server" id="BtnRow" style="vertical-align: top">
                                    <th colspan="2"></th>
                                    <td colspan="4" style="text-align: left;">
                                        <asp:Button ID="BtnGenSal" runat="server" Text="Generate" CssClass="buttonglossy buttonglossy-green" Height="25px" Width="77px" OnClick="BtnGenSal_Click" OnClientClick="return validate();" />
                                        &nbsp;&nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonglossy buttonglossy-green"  Height="25px" Width="77px" OnClick="BtnNew_Click" />
                                        &nbsp;&nbsp;<asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="buttonglossy buttonglossy-red"  Height="25px" Width="77px" OnClick="BtnCancel_Click" />
                                        &nbsp;&nbsp;<asp:Button ID="BtnView" runat="server" Text="View" CssClass="buttonglossy"  Height="25px" OnClick="BtnView_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" style="text-align: left;">
                                        <%--   <div style="border: 1px solid #C0C0C0;">--%>
                                        <asp:GridView ID="GridSal" runat="server"
                                            AutoGenerateColumns="False"
                                            DataKeyNames="TransactionId"
                                            OnPageIndexChanging="GridSal_PageIndexChanging"
                                            CssClass="mGrid"
                                            PageSize="10"
                                            EmptyDataText="No Data Available">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnEdit" CommandName="Select" OnClick="btnSalSelect_Click" runat="server"
                                                            ImageUrl="~/images/Edit.jpg" ToolTip="Select" Height="20px" Width="20px" Visible="false" />
                                                        <asp:ImageButton ID="imgbtnDelete" OnClick="btnSalDelete_Click" AlternateText="Edit" runat="server"
                                                            ImageUrl="~/images/Delete.png" ToolTip="Delete" Height="20px" Width="20px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="TransactionId" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdTranId" runat="server" Text='<%# Bind("TransactionId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Employee Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdEmpId" runat="server" Width="15px" Text='<%# Bind("EmpId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdEmpId" runat="server" Width="15px" BorderStyle="None" Text='<%# Bind("EmpId") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Employee Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdEmpName" runat="server" Width="150px" Text='<%# Bind("EmpName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdEmpName" runat="server" Width="150px" BorderStyle="None" Text='<%# Bind("EmpName") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Present">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdYear" runat="server" Width="30px" Text='<%# Bind("Present") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdPresent" runat="server" Width="30px" BorderStyle="None" Text='<%# Bind("Present") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Absent">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdAbsent" runat="server" Width="30px" Text='<%# Bind("Absent") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdAbsent" runat="server" Width="30px" BorderStyle="None" Text='<%# Bind("Absent") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Work Hours">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdWorkHours" runat="server" Width="40px" Text='<%# Bind("WorkHours") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdWorkHours" runat="server" Width="40px" BorderStyle="None" Text='<%# Bind("WorkHours") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Over/Less Time">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdExtraOrCut" runat="server" Width="40px" Text='<%# Bind("ExtraOrCut") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdExtraOrCut" runat="server" Width="40px" BorderStyle="None" Text='<%# Bind("ExtraOrCut") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Basic Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdBasicRate" runat="server" Width="70px" Text='<%# Bind("BasicRate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdBasicRate" runat="server" Width="70px" BorderStyle="None" Text='<%# Bind("BasicRate") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Per.Day Sal.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdPerDaySal" runat="server" Width="70px" Text='<%# Bind("PerDaySal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdPerDaySal" runat="server" Width="70px" BorderStyle="None" Text='<%# Bind("PerDaySal") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PerHourSal">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdPerHourSal" runat="server" Width="70px" Text='<%# Bind("PerHourSal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdPerHourSal" runat="server" Width="70px" BorderStyle="None" Text='<%# Bind("PerHourSal") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Cut Sal.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdCutSal" runat="server" Width="70px" Text='<%# Bind("CutSal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdCutSal" runat="server" Width="70px" BorderStyle="None" Text='<%# Bind("CutSal") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Give Sal.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdGiveSal" runat="server" Width="70px" Text='<%# Bind("GiveSal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdGiveSal" runat="server" Width="70px" BorderStyle="None" Text='<%# Bind("GiveSal") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Work Sal.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdWorkSal" runat="server" Width="70px" Text='<%# Bind("WorkSal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdWorkSal" runat="server" Width="70px" BorderStyle="None" Text='<%# Bind("WorkSal") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Over/Less Sal.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrdExtraOrCutSal" runat="server" Width="70px" Text='<%# Bind("ExtraOrCutSal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TxtGrdExtraOrCutSal" runat="server" Width="70px" BorderStyle="None" Text='<%# Bind("ExtraOrCutSal") %>'></asp:TextBox>
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
                                        <%--</div>--%>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnGenSal" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnNew" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnView" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlyear" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlmonth" EventName="SelectedIndexChanged" />                        
                        <asp:AsyncPostBackTrigger ControlID="GridSal" EventName="PageIndexChanging" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

