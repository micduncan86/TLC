<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="TLC.Members.INdex" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="display: inline;">Members</h3>
    <div style="padding: 5px; min-height: 45px;">
        <div class="dropdown pull-right">
            <button class="btn btn-sm btn-info dropdown-toggle" type="button" data-toggle="dropdown">
                I want to <span class="caret"></span>
            </button>
            <ul class="dropdown-menu" role="menu">
                <li role="presentation">
                    <asp:LinkButton ID="lnkAdd" runat="server" OnClick="lnkAdd_Click">
                            <span class="glyphicon glyphicon-user"></span> Add New Member
                    </asp:LinkButton>
                </li>
                <li role="presentation" class="divider"></li>
                <li role="presentation">
                    <a href="../export/members.aspx?type=xls" target="_blank">
                        <span class="glyphicon glyphicon-file"></span>Export To Excel
                    </a>
                </li>
            </ul>
        </div>
        <div class="input-group input-group-sm pull-right">
            <input type="text" id="txtsearch" runat="server" class="form-control" placeholder="Search for...">
            <span class="input-group-btn" style="width: unset;">
                <asp:LinkButton ID="lnkSearch" runat="server" CssClass="btn btn-sm btn-secondary" OnClick="lnkSearch_Click">
                                <span class="glyphicon glyphicon-search"></span>
                </asp:LinkButton>
            </span>

        </div>

    </div>
    <div>
        <div class="row">
            <div class="col-md-12">
                <asp:ListView ID="lstMembers" runat="server" DataKeyNames="MemberId" GroupPlaceholderID="phGroup" ItemPlaceholderID="phItem" OnItemCommand="lstMembers_ItemCommand" OnItemDataBound="lstMembers_ItemDataBound1">
                    <EmptyDataTemplate>
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <table class="table table-striped table-hover" style="margin-bottom: unset;">
                                    <thead>
                                        <tr>
                                            <th style="border-bottom: unset; border-right: 1px solid silver; min-width: 100px; width: 100px;"></th>
                                            <th class="col-md-2 col-mobile-only" style="border-bottom: unset; border-right: 1px solid silver;">Name</th>
                                            <th class="col-md-3 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Name</th>
                                            <th class="col-md-3 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Email</th>
                                            <th class="col-md-3 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Team</th>
                                            <th class="col-md-3 col-mobile" style="border-bottom: unset;">Info</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                            <div class="panel-body" style="min-height:400px; max-height:400px; overflow:auto;">
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
                                            <th style="border-bottom: unset; border-right: 1px solid silver; min-width: 100px; width: 100px;"></th>                
                                            <th class="col-md-2 col-mobile-only" style="border-bottom: unset; border-right: 1px solid silver;">Name</th>
                                            <th class="col-md-3 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Name</th>
                                            <th class="col-md-3 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Email</th>
                                            <th class="col-md-3 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Team</th>
                                            <th class="col-md-3 col-mobile" style="border-bottom: unset;">Info</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                            <div class="panel-body" style="min-height:400px; max-height:400px; overflow:auto;">
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
                        <td style="width: 90px;">
                            <div class="dropdown">
                                <button class="btn btn-sm btn-info dropdown-toggle" type="button" data-toggle="dropdown">
                                    Actions
                                <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" role="menu">
                                    <li role="presentation" id="liAssign" runat="server" visible="false">
                                        <asp:LinkButton ID="lnkAddToTeam" runat="server" CommandName="Assign">
                                        <span class="glyphicon glyphicon-ok btn btn-xs btn-info"></span> 
                                         Assign to Team
                                        </asp:LinkButton>
                                    </li>
                                    <li role="presentation">
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit">
                                        <span class="glyphicon glyphicon-pencil btn btn-xs btn-info"></span>
                                         Edit
                                        </asp:LinkButton>

                                    </li>
                                    <li role="presentation">
                                        <asp:LinkButton ID="lnkCopy" runat="server" CommandName="Copy">
                                        <span class="glyphicon glyphicon-share btn btn-xs btn-info"></span>
                                         Copy
                                        </asp:LinkButton>
                                    </li>
                                    <li role="presentation" class="divider"></li>
                                    <% if (HttpContext.Current.User.IsInRole("Administrater"))
                                        { %>
                                    <li role="presentation" id="liDelete" runat="server">
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" OnClientClick="javascript: return confirm('Are you sure you want to delete this member?');">
                                        <span class="glyphicon glyphicon-trash btn btn-xs btn-danger"></span>
                                         Delete
                                        </asp:LinkButton>
                                    </li>
                                    <%} %>
                                </ul>
                            </div>
                        </td>
                        <td class="col-md-2 col-mobile-only">
                            <h4><%# Eval("FullName") %></h4>
                            <a href="mailto:<%# Eval("Email") %>" class="btn btn-xs btn-info">
                                <span class="glyphicon glyphicon-envelope"></span>
                                <%# Eval("Email") %></a>
                            <div class="form-group">
                                <%# Eval("Address") %>
                                <p><%# Eval("City") %>, <%# Eval("State") %> <%# Eval("ZipCode") %></p>
                                <p><%# Eval("Phone") %></p>
                            </div>
                            <a href="../home.aspx?TeamId=<%# Eval("Team.TeamId") %>" class="btn btn-xs btn-warning"><%# Eval("Team.TeamName") %></a>
                        </td>
                        <td class="col-md-3 col-mobile">
                            <%# Eval("FullName") %>                    
                        </td>
                        <td class="col-md-3 col-mobile">                          
                            <a href="mailto:<%# Eval("Email") %>" class="btn btn-xs btn-info" style="<%# Eval("Email") == "" ? "display:none;": "" %>" >
                                <span class="glyphicon glyphicon-envelope"></span>
                                <%# Eval("Email") %></a>                 
                        </td>
                        <td class="col-md-3 col-mobile">
                            <a href="../home.aspx?TeamId=<%# Eval("Team.TeamId") %>" class="btn btn-xs btn-warning"><%# Eval("Team.TeamName") %></a>
                        </td>
                        <td class="col-md-3 col-mobile">
                            <%# Eval("Address") %>
                            <p><%# Eval("City") %>, <%# Eval("State") %> <%# Eval("ZipCode") %></p>
                            <p><%# Eval("Phone") %></p>
                        </td>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>       
    </div>

    <div id="mdlMembers" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">
                        <asp:Literal ID="ltrModalTitle" runat="server">Team Members</asp:Literal></h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="pnlNewTeam" runat="server" Visible="False">
                        <div>
                            <div class="form-group">
                                <asp:TextBox runat="server" ID="txtNewMemberName" CssClass="form-control" placeholder="Name" Style=""></asp:TextBox>
                            </div>
                            <div class="form-group form-inline">
                                <asp:TextBox runat="server" ID="txtNewMemberPhone" CssClass="form-control" placeholder="Phone" Width="140"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtNewMemberEmail" CssClass="form-control" placeholder="Email"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <asp:TextBox runat="server" ID="txtNewMemberAddress" CssClass="form-control" placeholder="Address" Style=""></asp:TextBox>
                            </div>
                            <div class="form-group form-inline">
                                <asp:TextBox runat="server" ID="txtNewMemberCity" CssClass="form-control" placeholder="City" Width="250"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtNewMemberState" CssClass="form-control" placeholder="State" Width="50"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtNewMemberZipCode" CssClass="form-control" placeholder="Postal Code" Width="115"></asp:TextBox>
                            </div>
                            <div class="form-group" id="divteamselection" runat="server">
                                <label>Team Selection:</label>
                                <asp:DropDownList ID="ddlTeam" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton runat="server" ID="lnkAddUpdateMember" CommandName="New" CssClass="btn btn-sm btn-success" OnClick="lnkAddUpdateMember_Click">
                        <span class="glyphicon glyphicon-plus"></span>
                        Add Team
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfShowModal" ClientIDMode="Static" runat="server" />
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#hdfShowModal").val() == "1") {
                $("#mdlMembers").modal("show");
            }
        });
    </script>
</asp:Content>

