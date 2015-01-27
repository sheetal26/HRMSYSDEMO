<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="ClientMast.aspx.cs" Inherits="Masters_ClientMast" %>

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

        function closepopup() {
            $find('ModalPopupExtender1').hide();
        }

        function EMailValidation(fieldName) {
            var str = "";
            if (fieldName.value != "") {
                var em = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
                if (!fieldName.value.match(em)) {
                    alert("Enter EMail In Correct Format !!!");
                    return false;
                }
                else {
                    return true;
                }
            }
        }

        function validate() {
            var str = "";
            if (document.getElementById("<%=TxtClientName.ClientID%>").value == "") {
                str = str + "Client Name Is Blank, Enter Valid Name. \n";
            }
            if (document.getElementById("<%=TxtFirmName.ClientID%>").value == "") {
                str = str + "Firm Name Is Blank, Enter Valid Name. \n";
            }
            if (document.getElementById("<%=TxtDOJ.ClientID%>").value == "") {
                str = str + "Date Of Join Is Blank, Enter Valid Join Date. \n";
            }
            if (document.getElementById("<%=TxtEMailId.ClientID%>").value == "") {
                str = str + "EMail Id Is Blank, Enter Valid EMail Id. \n";
            }
            if (document.getElementById("<%=TxtMobNo.ClientID%>").value == "") {
                str = str + "Mob.No Is Blank, Enter Valid Mob.No. \n";
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
                                                <%-- OnRowDeleting="GridClient_RowDeleting" AutoGenerateDeleteButton="True" AutoGenerateSelectButton="True" OnSelectedIndexChanged="GridClient_SelectedIndexChanged"--%>
                                                <asp:GridView ID="GridClient" runat="server" PageSize="15"
                                                    AutoGenerateColumns="False" DataKeyNames="Id"
                                                    OnPageIndexChanging="GridClient_PageIndexChanging"
                                                    CssClass="mGrid"
                                                    EmptyDataText="No Data Available">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <%-- <EditItemTemplate>
                                                                <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" runat="server" ImageUrl="~/images/update.jpg" ToolTip="Update" Height="20px" Width="20px" />
                                                                <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/cancel.jpg" ToolTip="Cancel" Height="20px" Width="20px" />
                                                            </EditItemTemplate>--%>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnEdit" CommandName="Select" OnClick="btnClientSelect_Click" AlternateText="Edit" runat="server" ImageUrl="~/images/Edit.jpg" ToolTip="Edit" Height="20px" Width="20px" />
                                                                <asp:ImageButton ID="imgbtnDelete" OnClick="btnClientDelete_Click" AlternateText="Delete" runat="server" ImageUrl="~/images/Delete.png" ToolTip="Delete" Height="20px" Width="20px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Id" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Client Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdCName" runat="server" Width="200px" Text='<%# Bind("ClientName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdCName" runat="server" Width="200px" BorderStyle="None" Text='<%# Bind("ClientName") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Firm Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdFirmName" runat="server" Width="200px" Text='<%# Bind("FirmName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdFirmName" runat="server" Width="200px" BorderStyle="None" Text='<%# Bind("FirmName") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="EMail Id">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdEMailId" runat="server" Width="150px" Text='<%# Bind("EMailId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdEMailId" runat="server" Width="150px" BorderStyle="None" Text='<%# Bind("EMailId") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mobile No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdMobNo" runat="server" Width="150px" Text='<%# Bind("MobNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdMobNo" runat="server" Width="200px" BorderStyle="None" Text='<%# Bind("MobNo") %>'></asp:TextBox>
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
                                            <td colspan="4" style="text-align: left;">
                                                <asp:HiddenField ID="HidFldId" runat="server" />
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="HidFldUID" runat="server" />
                                            </td>
                                        </tr>

                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Client Name</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtClientName" runat="server" Width="325px" Style="margin-bottom: 0px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Firm Name</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtFirmName" runat="server" Width="325px" Style="margin-bottom: 0px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Date of Join</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtDOJ" runat="server" Width="100px"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnCalendar2" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar2" PopupPosition="BottomRight" TargetControlID="TxtDOJ" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">EMail Id</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtEMailId" runat="server" Width="261px" onblur="EMailValidation(this);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Country</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:DropDownList ID="ddlCountry" runat="server" Width="300px" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">State</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:DropDownList ID="ddlState" runat="server" Width="300px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">City</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:DropDownList ID="ddlCity" runat="server" Width="300px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Address 1</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtAddress1" runat="server" Width="425px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Pin Code</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtPinCode" runat="server" Width="145px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Mobile Number</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtMobNo" runat="server" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Phone Number</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtPhone" runat="server" Width="225px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Left Date</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtLeftDate" runat="server" Width="175px"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnCalendar3" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar3" PopupPosition="BottomRight" TargetControlID="TxtLeftDate" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <td colspan="5" style="text-align: center;">
                                                <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="button black" Height="25px" Width="77px" OnClick="BtnSave_Click"  OnClientClick="return validate();" />
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
