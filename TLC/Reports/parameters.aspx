<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="parameters.aspx.cs" Inherits="TLC.Reports.parameters" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3>Report Parameters</h3>
                </div>
                <div class="panel-body">
                    <div class="form-group form-inline">
                        <label>Report Selection:</label>
                        <asp:DropDownList ID="ddlReports" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlReports_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div id="divTeams" runat="server" class="form-group form-inline" visible="false">
                        <label>Team Selection:</label>
                        <asp:DropDownList ID="ddlTeams" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTeams_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div id="divMembers" runat="server" class="form-group form-inline" visible="false">
                        <label>Member Selection:</label>
                        <asp:DropDownList ID="ddlMembers" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div id="divDateRange" runat="server" class="form-group form-inline" visible="false">
                        <label>Dates:</label>
                        <label>From:</label>
                        <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control datepicker" data-date-format="mm/dd/yyyy" placeholder="From Date"></asp:TextBox>
                        <label>To:</label>
                        <asp:TextBox ID="txtTo" runat="server" CssClass="form-control datepicker" data-date-format="mm/dd/yyyy" placeholder="To Date"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:LinkButton ID="btnShow" runat="server" CssClass="btn btn-md btn-info" OnClick="btnShow_Click">
                            <span>Show Report</span>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function ShowReport(rptName) {
            $(document).ready(function () {
                //window.open(url, "_blank");
                var n = app.GlobalDialog.init("Report View", function () {
                    $(".modal-dialog").width($(window).width() * .9);
                    $(".modal-content").height($(window).height() * .9);
                    var container = n.find(".modal-body");
                    var url = "../reports/view.aspx?Report=" + rptName;
                    var pdfObject = $("<object>").css("width", "100%")
                        .css("border", "none")
                        .attr("data", url)
                        .attr("type", "application/pdf")
                        .height($(".modal-content").height() - 105)
                        .appendTo(container);
                    $("<embed>")
                        .attr("src", url)
                        .attr("type", "application/pdf")
                    .appendTo(pdfObject);     
                });
            });
        }
        $(document).ready(function () {
            $(".datepicker").datepicker({
                format: "mm/dd/yyyy",
                orientation: "auto bottom"
            });
        });
    </script>
</asp:Content>
