<%@ Page Title="Log in" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TLC.Account.Login" Async="true" %>

<!DOCTYPE html>
<html>
<head>
    <title>Log In</title>
    <link rel="stylesheet" href="../Content/bootstrap.min.css" />
    <link rel="stylesheet" href="../Content/Site.css" />    
</head>
<body style="padding: 0;">
    <form id="form1" runat="server">
        <div class="tlcloginLogo">
        </div>
        <div style="margin: 0 auto; max-width: 600px;">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Log In
                        </div>
                        <div class="panel-body">
                            <section id="loginForm">
                                <div class="form-horizontal">
                                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                                        <p class="text-danger">
                                            <asp:Literal runat="server" ID="FailureText" />
                                        </p>
                                    </asp:PlaceHolder>
                                    <div class="form-group">
                                        <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
                                        <div class="col-md-10">
                                            <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                                            <asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="Email"
                                                CssClass="text-danger" ErrorMessage="The email field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
                                        <div class="col-md-10">
                                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-offset-2 col-md-10">
                                            <asp:Button runat="server" ID="btnSignIn" Text="Log in" CssClass="btn btn-default" />
                                            <asp:Button runat="server" OnClick="LogIn" ID="btnLogin" Text="Log in" CssClass="btn btn-default" style="display:none;" />
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="../Scripts/bootstrap.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {  
            var Dialog = function (Header, afterOpen) {
                $(".modal.in").modal("hide");
                var div = $("#bootModal").length == 0 ? $("<div>") : $("#bootModal");

                div.attr("id", "bootModal").attr("role", "dialog").addClass("modal fade").attr("title", Header).empty();

                var divDialog = $("<div>").addClass("modal-dialog").appendTo(div);
                var divContent = $("<div>").addClass("modal-content").appendTo(divDialog);
                var divHeader = $("<div>").addClass("modal-header").appendTo(divContent);
                var divBody = $("<div>").addClass("modal-body").appendTo(divContent);
                var divFooter = $("<div>").addClass("modal-footer").appendTo(divContent);

                var btnX = $("<button>").attr("type", "button").addClass("close").attr("data-dismiss", "modal").html("&times;").appendTo(divHeader);
                var title = $("<h4>").html(div.attr("title")).appendTo(divHeader);

                //var btnClose = $("<button>").attr("type", "button").addClass("btn btn-default").attr("data-dismiss", "modal").html("Close").appendTo(divFooter);



                div.modal("show");
                //app.gDialog = div;

                if (afterOpen) {
                    div.off("shown.bs.modal").on('shown.bs.modal', function (e) {
                        afterOpen();
                    });
                }
                return div;
            };
            var ShowLoading = function (onOpen) {
                var n = Dialog("Loading...", function () {
                    if (onOpen) {
                        setTimeout(function () { onOpen(); }, 350);
                    }
                });
                $("<img>").attr("src", "../images/load_indicator.gif").appendTo(n.find(".modal-body"));
                $("<label>").attr("id","lblLoading").css("padding", "0 5px").html("Logging In....").appendTo(n.find(".modal-body"));
                n.find(".modal-dialog").width(200);
                n.find(".modal-header").hide();
                n.find(".modal-footer").hide();
                var nTop = (($(window).height() + $(window).scrollTop()) * .5);
                n.css({
                    "position": "absolute",
                    "top": nTop + "px",
                    "margin-top": "-125px"
                });
                if (window.matchMedia("all and (max-width: 479px)").matches) {
                    n.css({
                        "margin-left": "-75px",
                        "left": "50%"
                    });
                }
            };
            $("#btnSignIn").on("click", function (e) {
                e.preventDefault();
                if (Page_ClientValidate()) {
                    ShowLoading(function () {
                        setTimeout(function () {
                            $("#bootModal").find("#lblLoading").html("Loading settings...");
                        }, 50);
                        $("#btnLogin").trigger("click");
                    });
                }               
            });
        });       
    </script>
</body>
</html>
