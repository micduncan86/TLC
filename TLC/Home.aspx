<%@ Page Title="Home" Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TLC.Home" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-10" style="margin-bottom: 10px;">
            <label style="font-size: 24px;">Welcome <%: Context.User.Identity.GetUserName()  %></label>
        </div>
        <div class="col-md-2" style="margin-top: 20px; margin-bottom: 10px;">
            <asp:ListView ID="lstTeams" runat="server" GroupPlaceholderID="phGroup" ItemPlaceholderID="phItem" DataKeyNames="TeamId">
                <LayoutTemplate>
                    <div class="dropdown">
                        <button class="btn btn-sm btn-info dropdown-toggle" style="width:100%;" type="button" data-toggle="dropdown">
                           Select a Team<span class="caret"></span>
                        </button>                        
                        <ul class="dropdown-menu dropdown-menu-right" role="menu">
                            <li role="presentation" class="alert-info">
                                <a href="Teams/index.aspx?Add=1"><span class="glyphicon glyphicon-plus"></span> Add New Team</span> </a>
                            </li>
                            <asp:PlaceHolder ID="phGroup" runat="server"></asp:PlaceHolder>
                        </ul>
                    </div>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <div class="dropdown">
                        <button class="btn btn-sm btn-info dropdown-toggle" style="width:100%;" type="button" data-toggle="dropdown">
                           Select a Team<span class="caret"></span>
                        </button>                        
                        <ul class="dropdown-menu dropdown-menu-right" role="menu">
                            <li role="presentation">
                                <a href="Teams/index.aspx">Add New Team</a>
                            </li>
                        </ul>
                    </div>
                </EmptyDataTemplate>
                <GroupTemplate>
                    <li role="presentation">
                        <asp:PlaceHolder ID="phItem" runat="server"></asp:PlaceHolder>
                    </li>
                </GroupTemplate>
                <ItemTemplate>                    
                    <a href="home.aspx?TeamId=<%# Eval("TeamId") %>"><%# Eval("TeamName") %></a>                    
                </ItemTemplate>
            </asp:ListView>            
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <div class="panel panel-default">
                <input type="hidden" id="hdnTeamId" runat="server" />
                <div class="panel-heading">
                    <asp:TextBox ID="txtTeamName" runat="server" CssClass="form-control" Style="display: inline; font-size: 12px; height: 25px; width: 200px;"></asp:TextBox>
                    <asp:LinkButton ID="lnkUpdateTeamInfo" runat="server" CssClass="btn btn-xs btn-success pull-right" OnClick="lnkUpdateTeamInfo_Click">
                        Update
                    </asp:LinkButton>
                </div>
                <div class="panel-body" style="min-height: 125px; max-height: 125px;">
                    <p>
                        Team Number:
                        <asp:TextBox ID="txtTeamNumber" runat="server" CssClass="form-control" Style="display: inline; width: 100px;"></asp:TextBox>
                    </p>
                    <p>
                        Team Leader:
                    <asp:Label ID="lblTeamLeader" runat="server"></asp:Label>
                    </p>
                    <p>
                        Co Leader:
                    <asp:Label ID="lblCoLeader" runat="server"></asp:Label>
                    </p>

                    <p>
                    </p>
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
                            <button class="btn btn-sm btn-info dropdown-toggle" type="button" data-toggle="dropdown">
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
                            <div style="overflow-x: hidden; overflow-y: auto; height: 105px;">
                                <table class="table table-striped table-hover">
                                    <thead>
                                        <tr>
                                            <th class="col-xs-1"></th>
                                            <th class="col-xs-2">Title</th>
                                            <th class="col-xs-2 col-mobile">Date</th>
                                            <th class="col-xs-7 col-mobile">Description</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phGroup" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
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
                            <td class="col-xs-1"></td>
                            <td class="col-xs-2"><%# Eval("Title") %></td>
                            <td class="col-xs-2 col-mobile"><%# DateTime.Parse(Eval("EventDate").ToString()).ToShortDateString() %></td>
                            <td class="col-xs-7 col-mobile"><%# Eval("Description") %></td>
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
                    <span class="badge"><span>
                        <asp:Literal ID="ltrMemberCount" runat="server"></asp:Literal></span><span class="glyphicon glyphicon-user"></span></span>
                    <div class="pull-right" style="margin-top: -5px;">
                        <div class="dropdown">
                            <button class="btn btn-sm btn-info dropdown-toggle" type="button" data-toggle="dropdown">
                                Actions <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu dropdown-menu-right" role="menu">
                                <li role="presentation">
                                    
                                    <a href="members/index.aspx?TeamId=<%  Response.Write(string.Format("{0}", hdnTeamId.Value)); %>"><span class="glyphicon glyphicon-plus"></span> Add Member to Team</a>                                </li>
                                <li role="presentation">
                                    <a href="Checkup/index.aspx?TeamId=<%  Response.Write(string.Format("{0}", hdnTeamId.Value)); %>"><span class="glyphicon glyphicon-search"></span> View All Check Ups</a>
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
                                            <th class="col-xs-2 col-mobile">Last CheckUp</th>
                                            <th class="col-xs-3 col-mobile">Outcome</th>
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
                                                <a href='<%# Page.ResolveUrl("~/checkup/index.aspx?AddCheckup=1&MemberId=" + Eval("MemberId")) %>'><span class="glyphicon glyphicon-plus"></span> Add Check Up</a>
                                                <a href='<%# Page.ResolveUrl("~/checkup/index.aspx?MemberId=" + Eval("MemberId")) %>'><span class="glyphicon glyphicon-check"></span> Check Up History</a>
                                            </li>
                                            <li role="presentation" class="divider"></li>
                                            <li role="presentation">
                                                <asp:LinkButton ID="lnkRemoveMember" runat="server" CommandArgument='<%# Eval("MemberId") %>' CommandName="Delete"><span class="glyphicon glyphicon-trash"></span> Remove From Team
                                                </asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </td>
                                <td class="col-xs-2"><%# Eval("FullName") %></td>
                                <td class="col-xs-4 col-mobile"><%# Eval("Email") %></td>
                                <td class="col-xs-3 col-mobile"><%# Eval("LatestCheckUpDate") %></td>
                                <td class="col-xs-2 col-mobile"><%# Eval("LatestCheckUpOutCome") %></td>

                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
