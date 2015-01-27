<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="UserMast.aspx.cs" Inherits="Masters_UserMast" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderhead" runat="Server">
      <script type="text/javascript">

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


              if (document.getElementById("<%=TxtUserId.ClientID%>").value == "") {
                  str = str + "User Id Is Blank, Enter Valid User Id. \n";
              }
              if (document.getElementById("<%=TxtUserName.ClientID%>").value == "") {
                  str = str + "User Name Is Blank, Enter Valid User Name. \n";
              }
              if (document.getElementById("<%=TxtLoginName.ClientID%>").value == "") {
                  str = str + "Login Name Is Blank, Enter Valid Login Name. \n";
              }
              if (document.getElementById("<%=DDLUserGroup.ClientID%>").value == "0") {
                  str = str + "Select User Group. \n";
              }

              if (document.getElementById("<%=TxtDOB.ClientID%>").value == "") {
                  str = str + "DOB Is Blank, Enter Valid Brith Date. \n";
              }
              if (document.getElementById("<%=TxtEMailId.ClientID%>").value == "") {
                  str = str + "EMail Id Is Blank, Enter Valid EMail Id. \n";
              }
              if (document.getElementById("<%=TxtMobNo.ClientID%>").value == "") {
                  str = str + "Mob.No Is Blank, Enter Valid Mob.No. \n";
              }
              if (document.getElementById("<%=TxtPassword.ClientID%>").value == "") {
                  str = str + "Password Is Blank, Enter Valid Password. \n";
              }
              if (document.getElementById("<%=TxtQuestion.ClientID%>").value == "") {
                  str = str + "Question Is Blank, Enter Valid Question. \n";
              }
              if (document.getElementById("<%=TxtAnswer.ClientID%>").value == "") {
                  str = str + "Answer Is Blank, Enter Valid Answer. \n";
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
    <%--  <asp:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID="HeadDiv" Radius="50" Corners="All">
    </asp:RoundedCornersExtender>--%>
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
                                    <table style="text-align: center;">
                                    <tr style="text-align: left;">
                                        <td>
                                            <asp:Button ID="BtnView" runat="server" Text="View All" CssClass="button black" Height="25px" Width="77px" OnClick="BtnView_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="GridUser" runat="server"
                                                AutoGenerateColumns="False" AutoGenerateDeleteButton="True" AutoGenerateSelectButton="True" DataKeyNames="Id" OnPageIndexChanging="GridUser_PageIndexChanging"
                                                OnRowDeleting="GridUser_RowDeleting"
                                                OnSelectedIndexChanged="GridUser_SelectedIndexChanged"
                                                CssClass="mGrid">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Id" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblGrdId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="User Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblGrdUId" runat="server" Width="100px" Text='<%# Bind("UID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TxtGrdUId" runat="server" Width="100px" BorderStyle="None" Text='<%# Bind("UId") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="User Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblGrdUName" runat="server" Width="200px" Text='<%# Bind("UserName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TxtGrdUName" runat="server" Width="200px" BorderStyle="None" Text='<%# Bind("UserName") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Login Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblGrdLoginName" runat="server" Width="200px" Text='<%# Bind("LoginName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TxtGrdLoginName" runat="server" Width="200px" BorderStyle="None" Text='<%# Bind("LoginName") %>'></asp:TextBox>
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
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                </asp:View>
                                <asp:View ID="TabNewEdit" runat="server">
                                    <table style="text-align: center;">
                                    <tr>
                                        <td colspan="2" style="text-align: left;">
                                            <asp:HiddenField ID="HidFldId" runat="server" />
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="HidFldUID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left; color: #FF0000;">*</th>
                                        <th style="text-align: left;">User Id</th>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="TxtUserId" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left; color: #FF0000;">*</th>
                                        <th style="text-align: left;">User Name</th>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="TxtUserName" runat="server" Width="325px" Style="margin-bottom: 0px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left; color: #FF0000;">*</th>
                                        <th style="text-align: left;">Login Name</th>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="TxtLoginName" runat="server" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left; color: #FF0000;">*</th>
                                        <th style="text-align: left;">User Group</th>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="DDLUserGroup" runat="server" Height="22px" Width="250px"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left; color: #FF0000;">*</th>
                                        <th style="text-align: left;">Date of Brith</th>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="TxtDOB" runat="server"></asp:TextBox>
                                            <asp:ImageButton ID="imgbtnCalendar" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                PopupButtonID="imgbtnCalendar" PopupPosition="BottomRight" TargetControlID="TxtDOB" Enabled="True">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th style="text-align: left;">Gender</th>
                                        <td style="text-align: left;">
                                            <asp:RadioButtonList ID="rblGender" runat="server" ValidationGroup="Gender" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="M" Text="Male"></asp:ListItem>
                                                <asp:ListItem Value="F" Text="Female"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left; color: #FF0000;">*</th>
                                        <th style="text-align: left;">EMail Id</th>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="TxtEMailId" runat="server" Width="261px"  onblur="EMailValidation(this);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left; color: #FF0000;">*</th>
                                        <th style="text-align: left;">Password</th>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th style="text-align: left;">Country</th>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlCountry" runat="server" Width="300px" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th style="text-align: left;">State</th>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlState" runat="server" Width="300px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th style="text-align: left;">City</th>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlCity" runat="server" Width="300px"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left; color: #FF0000;">*</th>
                                        <th style="text-align: left;">Mobile Number</th>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="TxtMobNo" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th style="text-align: left;">Phone Number</th>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="TxtPhone" runat="server" Width="225px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left; color: #FF0000;">*</th>
                                        <th style="text-align: left;">Question</th>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="TxtQuestion" runat="server" Width="300px" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left; color: #FF0000;">*</th>
                                        <th style="text-align: left;">Answer</th>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="TxtAnswer" runat="server" Width="300px" Height="53px" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: center;">
                                            <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="button black" Height="25px" Width="77px" OnClick="BtnSave_Click"  OnClientClick="return validate();"  />
                                            &nbsp;&nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" CssClass="button black" Height="25px" Width="77px" OnClick="BtnNew_Click" />
                                            &nbsp;&nbsp;<asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="button black" Height="25px" Width="77px" OnClick="BtnCancel_Click" />
                                            &nbsp;&nbsp;<asp:Button ID="BtnDelete" runat="server" Text="Delete" CssClass="button black" Height="25px" Width="77px" OnClick="BtnDelete_Click" /></td>
                                    </tr>
                                    <tr runat="server" id="trNavigation" style="vertical-align: top">
                                     
                                        <td colspan="3" style="text-align: center;">
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
                </asp:UpdatePanel>
            </div>

        </div>
    </div>

</asp:Content>

