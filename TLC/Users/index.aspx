<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="TLC.Users.index" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Users
            <asp:LinkButton ID="btnAddNew" runat="server" CssClass="btn btn-sm btn-success pull-right" OnClick="btnAddNew_Click">
                            <span class="glyphicon glyphicon-user"></span> Add New User
    </asp:LinkButton>
        </h3>
    <div class="row">
        <div class="col-md-12">
            <asp:ListView ID="lstUsers" runat="server" DataKeyNames="UserId" GroupPlaceholderID="phGroup" ItemPlaceholderID="phItem" OnItemDataBound="lstUsers_ItemDataBound">
                <EmptyDataTemplate>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <table class="table table-striped table-hover" style="margin-bottom: unset;">
                                <thead>
                                    <tr>
                                        <th class="col-xs-2" style="border-bottom: unset; border-right: 1px solid silver;"></th>
                                        <th class="col-xs-2" style="border-bottom: unset; border-right: 1px solid silver;">Email</th>
                                        <th class="col-xs-3 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Username</th>
                                        <th class="col-xs-2 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Role</th>
                                        <th class="col-xs-3 col-mobile" style="border-bottom: unset;">Assigned Team</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                        <div class="panel-body">
                            <strong>No Users</strong>
                        </div>
                    </div>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <table class="table table-striped table-hover" style="margin-bottom: unset;">
                                <thead>
                                    <tr>
                                        <th class="col-xs-2" style="border-bottom: unset; border-right: 1px solid silver;"></th>
                                        <th class="col-xs-2" style="border-bottom: unset; border-right: 1px solid silver;">Email</th>
                                        <th class="col-xs-3 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Username</th>
                                        <th class="col-xs-2 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Role</th>

                                    </tr>
                                </thead>
                            </table>
                        </div>
                        <div class="panel-body">
                            <table class="table table-striped table-hover">
                                <tbody>
                                    <asp:PlaceHolder ID="phGroup" runat="server"></asp:PlaceHolder>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </LayoutTemplate>
                <GroupTemplate>
                    <tr>
                        <asp:PlaceHolder ID="phItem" runat="server"></asp:PlaceHolder>
                    </tr>
                </GroupTemplate>
                <ItemTemplate>
                    <td class="col-xs-2">
                        <div class="dropdown">
                            <button class="btn btn-sm btn-info dropdown-toggle" type="button" data-toggle="dropdown">
                                Actions <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">
                                <li role="presentation">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditUser" CommandArgument='<%# Eval("UserId") %>' OnCommand="lnk_Command">
                                    <span class="glyphicon glyphicon-pencil btn btn-xs btn-info"></span>
                                     Edit
                                    </asp:LinkButton>
                                </li>
                                <li role="presentation">
                                    <a title="Change Password" href="ChangePassword.aspx?UserId=<%# Eval("UserId") %>">
                                        <span class="glyphicon glyphicon-flash btn btn-xs btn-warning"></span>
                                        Change Password
                                    </a>
                                </li>
                                <li role="presentation">
                                    <asp:LinkButton ID="lnkRemove" runat="server" CommandName="Delete" CommandArgument='<%# Eval("UserId") %>' OnCommand="lnk_Command" OnClientClick="return confirm('Are you sure you want to remove this user?');">
                            <span class="glyphicon glyphicon-remove btn btn-xs btn-danger"></span>
                                     Delete
                                    </asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                    </td>
                    <td class="col-xs-2"><%# Eval("Email") %></td>
                    <td class="col-xs-3 col-mobile"><%# Eval("UserName") %></td>
                    <td class="col-xs-2 col-mobile"><%# Eval("UserRole") %></td>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
    <div id="modalNewUser" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">
                        <asp:Literal ID="modalTitle" runat="server">User</asp:Literal>
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hdnUserId" runat="server" />
                    <div>
                        <div class="form-group">
                            <label>Email (login):</label>
                            <asp:TextBox runat="server" ID="txtEmail" TextMode="Email" CssClass="form-control" placeholder="Email" Style="max-width: none; width: 425px;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqEmail" Enabled="false" runat="server" ControlToValidate="txtEmail" CssClass="text-danger" ErrorMessage="The email field is required." />
                            <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="form-control" placeholder="Password" Style="max-width: none; width: 425px;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqPassword" Enabled="false" runat="server" ControlToValidate="txtPassword" CssClass="text-danger" ErrorMessage="The password field is required." />
                        </div>
                        <div class="form-group">
                            <label>User Name:</label>
                            <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control" placeholder="UserName" Style="max-width: none; width: 425px;"></asp:TextBox>
                        </div>

                        <div class="form-group form-inline">
                            <asp:CheckBox ID="chkIsAdmin" runat="server" CssClass="" Text=" Is Admin?" />
                        </div>
                        <div class="form-group">
                            <label>Team Selection:</label>
                            <asp:DropDownList ID="ddlTeam" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Label ID="lblError" runat="server" CssClass="btn btn-danger pull-left" Visible="false"></asp:Label>
                    <asp:LinkButton ID="lnkAddUser" runat="server" CssClass="btn btn-sm btn-success" OnClick="lnkAddUser_Click">
                        <span class="glyphicon glyphicon-user"></span>
                        Add User
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfShowModal" ClientIDMode="Static" runat="server" />
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#hdfShowModal").val() == "1") {
                $("#modalNewUser").modal("show");
                $("#modalNewUser").on("hidden.bs.modal", function () {
                    ValidatorEnable(document.getElementById("MainContent_reqEmail"), false);
                    ValidatorEnable(document.getElementById("MainContent_reqPassword"), false);
                });
            }
        });
    </script>
</asp:Content>
