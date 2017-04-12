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
                        <asp:DropDownList ID="ddlReports" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>  
                    <div class="form-group">
                        <button id="btnShow" class="btn btn-md btn-info">
                            <span>Show Report</span>
                        </button>
                    </div>                  
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnShow").on("click", function (e) {
                var n = app.GlobalDialog.init("Report View", function () {
                    var url = "../reports/view.aspx?Report=" + $("#MainContent_ddlReports").val();
                    $(".modal-dialog").width($(window).width() * .9);
                    $(".modal-content").height($(window).height() * .9);
                    var container = n.find(".modal-body");
                    var iframe = $("<iframe>").css("width", "100%").css("border","none")
                    .attr("src", url).appendTo(container);
                    iframe.height($(".modal-content").height() - 105);
                });
                e.preventDefault();
            });
        });
    </script>
</asp:Content>
