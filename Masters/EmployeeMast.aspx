<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="EmployeeMast.aspx.cs" Inherits="Masters_EmployeeMast" EnableEventValidation="false" %>

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

    <script type="text/javascript" src="../js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.19.custom.min.js"></script>

    <link type="text/css" href="../css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />

    <%--<script type="text/javascript" src="../bValidator-0.73/jquery.bvalidator-yc.js"></script>
    <script type="text/javascript" src="../bValidator-0.73/jquery.bvalidator.js"></script>
    <link href="../bValidator-0.73/bvalidator.css" rel="stylesheet" />--%>

    <script type="text/javascript">

        $(document).ready(function () {
            //    $.ajax({
            //        async: true,
            //        cache: false,
            //        type: "POST",
            //        contentType: "application/json; charset=utf-8",
            //        url: "EmployeeMast.aspx/BindCountry",
            //        data: "{}",
            //        dataType: "json",
            //        success: function (data) {
            //          $("#<%= ddlCountry.ClientID %>").append($("<option></option>").val("").html("---Select Country---"));
           //         $("#<%= ddlState.ClientID %>").append($("<option></option>").val("").html("---Select State---"));
           //           $("#<%= ddlCity.ClientID %>").append($("<option></option>").val("").html("---Select City---"));
           //          $.each(data.d, function (key, value) {
           //             $("#<%= ddlCountry.ClientID %>").append($("<option></option>").val(value.CountryId).html(value.CountryName));
           //        });
           //    },
           //    error: function (result) {
           //        alert("Error");
           //    }
           //});

           //$.ajax({
           //    async: true,
           //    cache: false,
           //    type: "POST",
           //    dataType: "json",
           //    contentType: "application/json; charset=utf-8",
           //    url: "EmployeeMast.aspx/BindDepartment",
           //    data: "{}",
           //    success: function (data) {
           //         $("#<%= DDLDeptName.ClientID %>").append($("<option></option>").val("").html("---Select Department---"));
           //         $("#<%= DDLDesignation.ClientID %>").append($("<option></option>").val("").html("---Select Designation---"));
           //          $.each(data.d,
           //             function (key, value) {
           //                $("#<%= DDLDeptName.ClientID %>").append($("<option></option>").val(value.strDeptId).html(value.strDeptName));
           //            }
           //        )
           //    },
           //    error: function (result) {
           //        alert("Error");
           //    }
           //})

           //$('#FrmMastHome').bValidator();

           $("#<%= TxtDOB.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
           $("#<%= TxtDOJ.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
           $("#<%= TxtLeftDate.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

       });

       //$(function () {
       //    $("#<%= DDLDeptName.ClientID %>").change(function () {
        //var IntDeptId = $(this).val();

        //$.ajax({
        //    async: true,
        //    cache: false,
        //    type: "POST",
        //    contentType: "application/json; charset=utf-8",
        //    datatype: "json",
        //    url: "EmployeeMast.aspx/BindDesignation",
        //    data: "{IntDeptId:" + IntDeptId + "}",
        //    success: function (data) {
        //       $("#<%= DDLDesignation.ClientID %>").empty();
        //       $("#<%= DDLDesignation.ClientID %>").append($("<option></option>").val("").html("---Select Designation---"));
        //       $.each(data.d, function (key, value) {
        //          $("#<%= DDLDesignation.ClientID %>").append($("<option></option>").val(value.strDesigId).html(value.strDesigName))
        //       })
        //  },
        //        error: function (result) {
        //            alert("Error");
        //        }
        //    })
        //})
        //})

        //    $(function () {
        //       $("#<%= ddlCountry.ClientID %>").change(function () {
        //var IntCountryId = $(this).val();

        //$.ajax({
        //    async: true,
        //    cache: false,
        //    type: "POST",
        //    contentType: "application/json; charset=utf-8",
        //    url: "EmployeeMast.aspx/BindState",
        //    data: '{IntCountryId:' + IntCountryId + '}',
        //    dataType: "json",
        //    success: function (data) {
        //                 $("#<%= ddlState.ClientID %>").empty();
        //              $("#<%= ddlCity.ClientID %>").empty();
        //             $("#<%= ddlState.ClientID %>").append($("<option></option>").val("").html("---Select State---"));
        //            $("#<%= ddlCity.ClientID %>").append($("<option></option>").val("").html("---Select City---"));
        //            $.each(data.d, function (key, value) {
        //              $("#<%= ddlState.ClientID %>").append($("<option></option>").val(value.strStateId).html(value.strState));
        //            });
        //        },
        //        error: function (result) {
        //            alert("Error");
        //        }
        //    });
        //    })
        //})

        //   $(function () {
        //       $("#<%= ddlState.ClientID %>").change(function () {

        //var IntStateId = $(this).val();

        //$.ajax({
        //    async: true,
        //    cache: false,
        //    type: "POST",
        //    contentType: "application/json; charset=utf-8",
        //    url: "EmployeeMast.aspx/BindCity",
        //    data: '{IntStateId:' + IntStateId + '}',
        //    dataType: "json",
        //    success: function (data) {
        //                 $("#<%= ddlCity.ClientID %>").empty();
        //            $("#<%= ddlCity.ClientID %>").append($("<option></option>").val("").html("---Select City---"));
        //          $.each(data.d, function (key, value) {
        //             $("#<%= ddlCity.ClientID %>").append($("<option></option>").val(value.strCityId).html(value.strCity));
        //            });
        //        },
        //        error: function (result) {
        //            alert("Error");
        //        }
        //    });

        //    })
        //})

        //****

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

        function EMailValidation(fieldName) {
            var str = "";
            if (fieldName.value != "") {
                var em = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
                if (!fieldName.value.match(em)) {
                    alert("Enter EMail In Correct Format !!!");
                    document.getElementById("<%=TxtEMailId.ClientID%>").focus();
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }

            function validate() {
                var str = "";
                if (document.getElementById("<%=TxtEmpName.ClientID%>").value == "") {
                    str = str + "Employee Name Is Required. \n";
                }
                if (document.getElementById("<%=DDLEmpGroup.ClientID%>").value == "0") {
                    str = str + "Employee Group Is Required. \n";
                }
                if (document.getElementById("<%=TxtFJobTime.ClientID%>").value == "") {
                    str = str + "From Time Is Required. \n";
                }
                if (document.getElementById("<%=TxtTJobTime.ClientID%>").value == "") {
                    str = str + "To Time Is Required. \n";
                }
                var basicsal = parseFloat(document.getElementById("<%=TxtBasicSal.ClientID%>").value);
                if (isNaN(basicsal) == true) {
                    str = str + "Basic Rate Is Required. \n";
                }
                if (document.getElementById("<%=TxtDOJ.ClientID%>").value == "") {
                str = str + "Date Of Join Is Required. \n";
            }
            if (document.getElementById("<%=TxtDOB.ClientID%>").value == "") {
                    str = str + "DOB Is Required. \n";
                }
                if (document.getElementById("<%=TxtEMailId.ClientID%>").value == "") {
                    str = str + "EMail Id Is Required. \n";
                }
                if (document.getElementById("<%=TxtMobNo.ClientID%>").value == "") {
                    str = str + "Mob.No Is Required. \n";
                }
                if (document.getElementById("<%=TxtPassword.ClientID%>").value == "") {
                    str = str + "Password Is Required. \n";
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

    <%--ClientScriptManager.RegisterForEventValidation--%>

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
                                                <asp:Label ID="Label1" runat="server" Text="Search"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtFEmpName" runat="server" ToolTip="Search Employee" Width="200px" AutoPostBack="True" OnTextChanged="TxtFEmpName_TextChanged"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtFEmpGrp" runat="server" ToolTip="Search Emp.Group" Width="190px" AutoPostBack="True" OnTextChanged="TxtFEmpGrp_TextChanged"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtFEMailId" runat="server" ToolTip="Search Emp.EMailId" Width="150px" AutoPostBack="True" OnTextChanged="TxtFEMailId_TextChanged"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtFMobNo" runat="server" ToolTip="Search Emp.MobNo" Width="150px" AutoPostBack="True" OnTextChanged="TxtFMobNo_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                                            </td>
                                            <td colspan="4">
                                                <asp:Button ID="BtnView" runat="server" Text="View All" CssClass="button black" Height="25px" Width="77px" OnClick="BtnView_Click" />
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <td colspan="5">
                                                <asp:GridView ID="GridEmp" runat="server"
                                                    AutoGenerateColumns="False" PageSize="15"
                                                    DataKeyNames="Id"
                                                    OnPageIndexChanging="GridEmp_PageIndexChanging"
                                                    CssClass="mGrid"
                                                    EmptyDataText="No Data Available">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnEdit" runat="server" OnClick="btnEmpSelect_Click" CommandName="Select" AlternateText="Edit" ImageUrl="~/images/Edit.jpg" ToolTip="Edit" Height="20px" Width="20px" />
                                                                <asp:ImageButton ID="imgbtnDelete" runat="server" OnClick="btnEmpDelete_Click" AlternateText="Delete" ImageUrl="~/images/Delete.png" ToolTip="Delete" Height="20px" Width="20px" />
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
                                                        <asp:TemplateField HeaderText="Employee Group">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrdEGrp" runat="server" Width="200px" Text='<%# Bind("EmpGroup") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtGrdEGrp" runat="server" Width="200px" BorderStyle="None" Text='<%# Bind("EmpGroup") %>'></asp:TextBox>
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
                                                <%-- <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
                                                    CancelControlID="btnno" BackgroundCssClass="modalBackground">
                                                </asp:ModalPopupExtender>--%>
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
                                            <th style="text-align: left;">Employee Name</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtEmpName" runat="server" Width="325px" Style="margin-bottom: 0px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Department</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:DropDownList ID="DDLDeptName" runat="server" Width="300px" AutoPostBack="True" OnSelectedIndexChanged="DDLDeptName_SelectedIndexChanged">                                                    
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Designation</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:DropDownList ID="DDLDesignation" runat="server" Width="300px">                                                  
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Employee Group</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:DropDownList ID="DDLEmpGroup" runat="server" Height="22px" Width="200px">
                                                    <asp:ListItem Value="">---Select Group---</asp:ListItem>
                                                    <asp:ListItem>Administrator</asp:ListItem>
                                                    <asp:ListItem>Dept. Head</asp:ListItem>
                                                    <asp:ListItem>Ledger</asp:ListItem>
                                                    <asp:ListItem>Senior</asp:ListItem>
                                                    <asp:ListItem>Junior</asp:ListItem>
                                                    <asp:ListItem>Fresher</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Office Time From</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtFJobTime" runat="server" Width="80px" OnTextChanged="TxtFJobTime_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                            <th style="text-align: right;">To</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtTJobTime" runat="server" Width="80px" OnTextChanged="TxtTJobTime_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th></th>
                                            <th style="text-align: left;">Total Hours</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtTotHours" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                            </td>
                                            <th style="text-align: right;">Basic Rate</th>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtBasicSal" runat="server" Width="150px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Date of Join</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtDOJ" runat="server" Width="100px"></asp:TextBox>
                                                <%--<asp:ImageButton ID="imgbtnCalendar2" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar2" PopupPosition="BottomRight" TargetControlID="TxtDOJ" Enabled="True">
                                                </asp:CalendarExtender>--%>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Date of Brith</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtDOB" runat="server" Width="100px"></asp:TextBox>
                                                <%--<asp:ImageButton ID="imgbtnCalendar" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar" PopupPosition="BottomRight" TargetControlID="TxtDOB" Enabled="True">
                                                </asp:CalendarExtender>--%>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Gender</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:RadioButtonList ID="rblGender" runat="server" ValidationGroup="Gender" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="M" Text="Male"></asp:ListItem>
                                                    <asp:ListItem Value="F" Text="Female"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">EMail Id</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtEMailId" runat="server" Width="261px" onblur="EMailValidation(this);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="text-align: left; color: #FF0000;">*</th>
                                            <th style="text-align: left;">Password</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Country</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:DropDownList ID="ddlCountry" runat="server" Width="300px" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="True">
                                                   
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">State</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:DropDownList ID="ddlState" runat="server" Width="300px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="True">
                                              
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">City</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:DropDownList ID="ddlCity" runat="server" Width="300px">                                                   
                                                </asp:DropDownList>
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
                                            <th style="text-align: left;">Blood groups</th>
                                            <td colspan="3" style="text-align: left;">
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
                                        <tr style="vertical-align: top">
                                            <th></th>
                                            <th style="text-align: left;">Left Date</th>
                                            <td colspan="3" style="text-align: left;">
                                                <asp:TextBox ID="TxtLeftDate" runat="server" Width="175px"></asp:TextBox>
                                                <%-- <asp:ImageButton ID="imgbtnCalendar3" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="imgbtnCalendar3" PopupPosition="BottomRight" TargetControlID="TxtLeftDate" Enabled="True">
                                                </asp:CalendarExtender>--%>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <td colspan="5" style="text-align: center;">
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
                    <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnNew" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnDelete" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="TxtFEmpName" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="TxtFEmpGrp" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="TxtFEMailId" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="TxtFMobNo" EventName="TextChanged" />
                    </Triggers>--%>
                </asp:UpdatePanel>
            </div>


        </div>
    </div>

</asp:Content>

