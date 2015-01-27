<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="~/Contactus.aspx.cs" Inherits="Contactus" %>

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
            if (document.getElementById("<%=TxtName.ClientID%>").value == "") {
                str = str + "Employee Name Is Required. \n";
            }
            if (document.getElementById("<%=TxtEMailId.ClientID%>").value == "") {
                str = str + "EMail Id Is Required. \n";
            }
            if (document.getElementById("<%=TxtMobNo.ClientID%>").value == "") {
                str = str + "Mob.No Is Required. \n";
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
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script type="text/javascript">
        var map;
        function initialize() {
            var latlng = new google.maps.LatLng(21.170240, 72.831061);
            var myOptions = {
                zoom: 8,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById("map"), myOptions);
            var marker = new google.maps.Marker
            (
                {
                    position: new google.maps.LatLng(21.170240, 72.831061),
                    map: map,
                    title: 'Click me'
                }
            );
            var infowindow = new google.maps.InfoWindow({
                content: 'Location info:<br/>Country Name:<br/>LatLng:'
            });
            google.maps.event.addListener(marker, 'click', function () {
                // Calling the open method of the infoWindow 
                infowindow.open(map, marker);
            });
        }
        window.onload = initialize;
    </script>
    <div id="HeadDiv" runat="server" class="content_resize" style="-moz-border-radius: 50px; -webkit-border-radius: 50px; border-radius: 50px;">
        <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" />
        <asp:SiteMapPath ID="SiteMap1" runat="server"></asp:SiteMapPath>

        <div class="article">
            <h2>Contact us</h2>

        </div>
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="LblMsg" runat="server" Text="..." Font-Bold="True" Font-Size="Small" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div style="float: left; width: 1000px;">
            <div style="float: left; width: 500px;">
                <table id="tblContact">
                    <tr>
                        <td>
                            <asp:HiddenField ID="HidFldId" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="HidFldUID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Name (required)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtName" runat="server" Width="325px" Style="margin-bottom: 0px"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td>Email Address (required)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtEMailId" runat="server" Width="261px" onblur="EMailValidation(this);"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td>Website
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtWebSite" runat="server" Width="325px" Style="margin-bottom: 0px"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td>Mobile Number (required)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtMobNo" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Your Message
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtMsg" runat="server" Height="150px" Width="425px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trContact" runat="server">
                        <td id="tdContact" runat="server">
                            Contact Status <br />
                            <asp:DropDownList ID="DdlContactStat" runat="server" Height="22px" Width="200px">
                                <asp:ListItem Value="">---Select Status---</asp:ListItem>
                                <asp:ListItem Value="P">Pending</asp:ListItem>
                                <asp:ListItem Value="C">Complete</asp:ListItem>                                
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="BtnSubmit" runat="server" Text="Submit" CssClass="button black"
                                Font-Underline="False" Height="32px" Width="79px" OnClientClick="return validation();" OnClick="BtnSubmit_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="float: left; width: 500px;">
                <div style="width: 500px;">
                    <table>
                        <tr>
                            <td>
                                <p>
                                    <u><b>Address :</b></u><br />
                                    5/788,Vania Sheri, Mahidhar Pura,Surat (395003),<br />
                                    Surat (395003),<br />
                                    Gujarat (India).<br /><br />
                                    <u><b>Mobile No. :</b></u><br />
                                    1234567898, (0261) 55555<br />
                                </p>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="map" style="width: 300px;">
                </div>
            </div>
        </div>

    </div>
</asp:Content>

