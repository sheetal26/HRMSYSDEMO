<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="AssWrkInfo.aspx.cs" Inherits="Report_AssWrkInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderhead" runat="Server">
    <script type="text/javascript">
       // function CallPrint() {
          //  var prtContent = document.getElementById('<%=divPrint.ClientID %>');
         //   var WinPrint = window.open('', '', 'letf=0,top=0,toolbar=0,scrollbars=0,status=0');
         //   WinPrint.document.write(prtContent.innerHTML);
         //   WinPrint.document.close();
        //    WinPrint.focus();
        //    WinPrint.print();
        //    WinPrint.close();
        //}

        //For GridView
        function printGrid() {
            var gridData = document.getElementById('<%= GridWrokList.ClientID %>');
            var windowUrl = 'about:blank';

            //set print document name for gridview
            var uniqueName = new Date();
            var windowName = 'Print_' + uniqueName.getTime();

            var prtWindow = window.open(windowUrl, windowName,
            'left=100,top=100,right=100,bottom=100,width=700,height=500');
            prtWindow.document.write('<html><head></head>');
            prtWindow.document.write('<body style="background:none !important">');
            prtWindow.document.write(gridData.outerHTML);
            prtWindow.document.write('</body></html>');
            prtWindow.document.close();
            prtWindow.focus();
            prtWindow.print();
            prtWindow.close();
        }

        function doPrint() {
            var prtContent = document.getElementById('<%= ReportViewer1.ClientID %>');
            prtContent.border = 0; //set no border here
            var WinPrint = window.open('', '', 'left=150,top=100,width=1000,height=1000,toolbar=0,scrollbars=1,status=0,resizable=1');
            WinPrint.document.write(prtContent.outerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
        }

        ////For Report Viewer
        //// Print function (require the reportviewer client ID)
        //function printReport(report_ID) {
        //    var rv1 = $('#' + report_ID);
        //    var iDoc = rv1.parents('html');

        //    // Reading the report styles
        //    var styles = iDoc.find("head style[id$='ReportControl_styles']").html();
        //    if ((styles == undefined) || (styles == '')) {
        //        iDoc.find('head script').each(function () {
        //            var cnt = $(this).html();
        //            var p1 = cnt.indexOf('ReportStyles":"');
        //            if (p1 > 0) {
        //                p1 += 15;
        //                var p2 = cnt.indexOf('"', p1);
        //                styles = cnt.substr(p1, p2 - p1);
        //            }
        //        });
        //    }
        //    if (styles == '') { alert("Cannot generate styles, Displaying without styles.."); }
        //    styles = '<style type="text/css">' + styles + "</style>";

        //    // Reading the report html
        //    var table = rv1.find("div[id$='_oReportDiv']");
        //    if (table == undefined) {
        //        alert("Report source not found.");
        //        return;
        //    }

        //    // Generating a copy of the report in a new window
        //    var docType = '<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/loose.dtd">';
        //    var docCnt = styles + table.parent().html();
        //    var docHead = '<head><title>Printing ...</title><style>body{margin:5;padding:0;}</style></head>';
        //    var winAttr = "location=yes, statusbar=no, directories=no, menubar=no, titlebar=no, toolbar=no, dependent=no, width=720, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
        //    var newWin = window.open("", "_blank", winAttr);
        //    writeDoc = newWin.document;
        //    writeDoc.open();
        //    writeDoc.write(docType + '<html>' + docHead + '<body onload="window.print();">' + docCnt + '</body></html>');
        //    writeDoc.close();

        //    // The print event will fire as soon as the window loads
        //    newWin.focus();
        //    // uncomment to autoclose the preview window when printing is confirmed or canceled.
        //    // newWin.close();
        //};

        //// Linking the print function to the print button        
        //function doPrint() {
        //    $('#BtnPrintRV').click(function () {
        //        printReport('ReportViewer1');
        //    });
        //}
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="Server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <%-- <asp:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID="HeadDiv" Radius="50" Corners="All">
    </asp:RoundedCornersExtender>--%>

    <div id="HeadDiv" runat="server" class="content_resize" style="-moz-border-radius: 50px; -webkit-border-radius: 50px; border-radius: 50px;">
        <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" />
        <asp:SiteMapPath ID="SiteMap1" runat="server"></asp:SiteMapPath>
        <div class="content">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>Projct Ledger</td>
                            <td>
                                <asp:DropDownList ID="DDLPrjLed" runat="server" Width="250px">
                                </asp:DropDownList>
                            </td>
                            <td>Employee</td>
                            <td>
                                <asp:DropDownList ID="ddlEmployee" runat="server" Width="250px">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: center;">
                                <asp:Button ID="BtnShow" runat="server" Text="Show Data (RV)" CssClass="button green" OnClick="BtnShow_Click" Height="25px" Width="125px" />
                            </td>
                        </tr>
                        <tr>
                            <td>Project Name</td>
                            <td>
                                <asp:DropDownList ID="DDLPrjName" runat="server" Width="250px" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDLPrjName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>Project Module</td>
                            <td>
                                <asp:DropDownList ID="DDLPrjModule" runat="server" Width="250px">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: center;">
                                <%--<asp:Button ID="BtnShowWS" runat="server" Text="Show Data (WS)" CssClass="button yellow" OnClick="BtnShowWS_Click" Height="25px" Width="125px" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td>Assign Date</td>
                            <td>
                                <asp:TextBox ID="TxtAssignDate" runat="server" Width="100px" onblur="CalcDueDays(this);"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar1" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar1" PopupPosition="BottomLeft" TargetControlID="TxtAssignDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td>Working Status</td>
                            <td>
                                <asp:DropDownList ID="ddlWrkStatus" runat="server" Width="150px">
                                    <asp:ListItem Value="0">---Select Status---</asp:ListItem>
                                    <asp:ListItem>Done</asp:ListItem>
                                    <asp:ListItem>Pending</asp:ListItem>
                                    <asp:ListItem>Cancel</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: center;">
                                <%--<asp:Button ID="BtnPrintWS" runat="server" Text="Print Data" CssClass="button black" OnClientClick="javascript:printGrid();" Height="25px" Width="125px" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td>Due Date</td>
                            <td colspan="3">
                                <asp:TextBox ID="TxtDueDate" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCalendar2" runat="server" ImageUrl="~/images/date_picker1.gif" Height="20px" Width="20px" />
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="imgbtnCalendar2" PopupPosition="BottomLeft" TargetControlID="TxtDueDate" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td style="text-align: center;">
                                <asp:Button ID="BtnPrintRV" runat="server" Text="Print Data RV" CssClass="button black" Height="25px" Width="125px" OnClientClick="doPrint();"  />
                            </td>
                        </tr>
                         <tr>
                            <td colspan="5">
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1000px" BorderStyle="Solid" BorderWidth="1px"></rsweb:ReportViewer>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div id="divPrint" runat="server">
                                    <asp:GridView ID="GridWrokList" runat="server" CssClass="EU_DataTable"
                                        EmptyDataText="No Data Available">
                                        <%--<RowStyle Wrap="False" />--%>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="BtnShowWS" EventName="Click" />--%>
                    <asp:AsyncPostBackTrigger ControlID="BtnShow" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

