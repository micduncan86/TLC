<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TLC.Home" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-10" style="margin-top: 20px; margin-bottom: 10px;">
            <label style="font-size: 24px;">Welcome <%: Context.User.Identity.GetUserName()  %></label>
        </div>
        <div class="col-md-2" style="margin-top: 20px; margin-bottom: 10px;">
            <asp:DropDownList ID="ddlTeams" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlTeams_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:Literal ID="ltrTeamName" runat="server"></asp:Literal>
                    <span class="badge" style="float: right;"><span>
                        <asp:Literal ID="ltrMemberCount" runat="server"></asp:Literal></span><span class="glyphicon glyphicon-user"></span></span>
                </div>
                <div class="panel-body" style="min-height: 125px; max-height: 125px;">
                    <p>
                        Team Number:
                        <asp:Label ID="lblTeamNumber" runat="server"></asp:Label>
                    </p>
                    Co Leader:
                    <asp:Label ID="lblCoLeader" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <div class="col-md-8">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Events   
                    <span class="badge">
                        <asp:Literal ID="ltrEventCount" runat="server"></asp:Literal></span>
                    <div class="pull-right" style="margin-top: -5px;">
                        <div class="dropdown">
                            <button class="btn btn-sm btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                Actions <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu dropdown-menu-right" role="menu">
                                <li role="presentation">
                                    <asp:LinkButton ID="lnkAddEvent" runat="server" CommandName="Checkup">Add New Event
                                    </asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="panel-body" style="min-height: 125px; max-height: 125px;">
                    <asp:ListView ID="lstEvents" runat="server" DataKeyNames="EventId" GroupPlaceholderID="phGroup" ItemPlaceholderID="phItem">
                        <LayoutTemplate>
                            <div style="overflow-x: hidden; overflow-y: scroll; max-height: 107px;">
                                <asp:PlaceHolder ID="phGroup" runat="server"></asp:PlaceHolder>
                            </div>
                        </LayoutTemplate>
                        <GroupTemplate>
                            <ul class="list-group">
                                <li class="list-group-item">
                                    <asp:PlaceHolder ID="phItem" runat="server"></asp:PlaceHolder>
                                </li>
                            </ul>
                        </GroupTemplate>
                        <EmptyDataTemplate>
                            <strong>No Data</strong>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <p>
                                <label><%# Eval("Title") %></label>
                                <button type="button" class="btn btn-xs btn-info">
                                    <span class="glyphicon glyphicon-pencil"></span>
                                </button>
                                <button type="button" class="btn btn-xs btn-info">
                                    <span class="glyphicon glyphicon-trash"></span>
                                </button>
                                <label class="btn btn-xs btn-primary" style="float: right;">Status:<%# Eval("Status") %></label></p>
                            <pre><%# Eval("Description") %></pre>

                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Members</strong>
                    <div class="pull-right" style="margin-top: -5px;">
                        <div class="dropdown">
                            <button class="btn btn-sm btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                Actions <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu dropdown-menu-right" role="menu">
                                <li role="presentation">
                                    <asp:LinkButton ID="lnkAddMember" runat="server" OnClick="lnkAddMember_Click">
                                        Add Member to Team
                                    </asp:LinkButton>                                  
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div style="min-height: 300px; max-height: 300px; overflow-x: hidden; overflow-y: auto;">
                        <asp:ListView ID="lstMembers" runat="server" DataKeyNames="MemberId" GroupPlaceholderID="phGroup" ItemPlaceholderID="phItem" OnItemDataBound="lstMembers_ItemDataBound" OnItemCommand="lstMembers_ItemCommand">
                            <LayoutTemplate>
                                <table class="table table-striped table-hover">
                                    <thead>
                                        <tr>
                                            <th class="col-xs-1"></th>
                                            <th class="col-xs-2">Name</th>
                                            <th class="col-xs-4 col-mobile">Email</th>
                                            <th class="col-xs-2 col-mobile">Phone</th>
                                            <th class="col-xs-3 col-mobile">Address</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phGroup" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </LayoutTemplate>
                            <GroupTemplate>
                                <tr>
                                    <asp:PlaceHolder ID="phItem" runat="server"></asp:PlaceHolder>
                                </tr>
                            </GroupTemplate>
                            <EmptyDataTemplate>
                                <strong>No Data</strong>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <td class="col-xs-1">
                                    <div class="dropdown">
                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                            Actions <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu" role="menu">
                                            <li role="presentation">
                                                <asp:LinkButton ID="lnkCheckUp" runat="server" CommandName="Checkup">Add Check Up
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lnkHistory" runat="server" CommandName="Copy">View History
                                                </asp:LinkButton>
                                            </li>
                                            <li role="presentation" class="divider"></li>
                                            <li role="presentation">
                                                <asp:LinkButton ID="lnkRemoveMember" runat="server" CommandArgument='<%# Eval("MemberId") %>' CommandName="Delete">Remove From Team
                                                </asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </td>
                                <td class="col-xs-2"><%# Eval("FullName") %></td>
                                <td class="col-xs-4 col-mobile"><%# Eval("Email") %></td>
                                <td class="col-xs-3 col-mobile"><%# Eval("Phone") %></td>
                                <td class="col-xs-2 col-mobile"><%# Eval("Address") %></td>

                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </div>
        </div>
    </div>   
</asp:Content>
