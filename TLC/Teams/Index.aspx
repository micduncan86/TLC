<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="TLC.Teams.Index" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Teams        
        <asp:LinkButton ID="lnkAdd" runat="server" CssClass="btn btn-sm btn-success" Style="float: right;" OnClick="lnkAdd_Click">
            <span class="glyphicon glyphicon-plus"></span>
            Add New Team
        </asp:LinkButton>
    </h3>
    <p>
        <asp:GridView ID="grdTeams" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed" RowStyle-CssClass="row" HeaderStyle-CssClass="row" BorderWidth="0" OnRowCommand="grdTeams_RowCommand" DataKeyNames="TeamId"  ShowHeader="false">
            <Columns>
                <asp:TemplateField ItemStyle-Width="90px">
                    <ItemTemplate>
                        <div class="dropdown">
                            <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                Actions <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">
                                <li role="presentation">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' CommandName="Edit">Edit
                                    </asp:LinkButton>
                                </li>
                                <li role="presentation">
                                    <a href='<%# "Members.aspx?Id=" + ((TLC.Data.Team)DataBinder.GetDataItem(Container)).TeamId %>' >Members</a>  
                                </li>
                                <li role="presentation">
                                    <a href='<%# "Events.aspx?Id=" + ((TLC.Data.Team)DataBinder.GetDataItem(Container)).TeamId %>' >Events</a>  
                                </li>
                                <li role="presentation" class="divider"></li>
                                <li role="presentation">
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' CommandName="Delete" OnClientClick="javascript: return confirm('Are you sure you want to delete this team?');">Delete
                                    </asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="col-md-12" HeaderText="Name">
                    <ItemTemplate>
                        <h4>
                            <asp:Literal ID="ltrName" runat="server" Text='<%# Eval("TeamName") %>'></asp:Literal></h4>
                            <div class="dropdown">
                                <button class="btn btn-info dropdown-toggle" type="button" data-toggle="dropdown">
                                    Info <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" role="menu" style="padding:5px;">
                                    <li role="presentation">
                                        <label>Team Leader:</label>
                                        <asp:Literal ID="Literal2" runat="server" Text='<%# Eval("TeamLeader.FullName") %>'></asp:Literal>
                                    </li>
                                    <li role="presentation">
                                        <label>Group Number:</label>
                                        <asp:Literal ID="Literal3" runat="server" Text='<%# Eval("TeamNumber") %>'></asp:Literal>
                                    </li>
                                    <li role="presentation">    
                                        <label>Members:</label>
                                        <asp:LinkButton ID="lnkMembers" runat="server" style="display:inline-block;" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' CommandName="LoadMembers">
                                <span class="badge">
                                    <span class="glyphicon glyphicon-user"></span> <%# Eval("Members.Count") %>
                                </span>
                                        </asp:LinkButton>
                                    </li>
                                </ul>
                            </div>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Text='<%# Eval("TeamName") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>



            </Columns>
        </asp:GridView>
    </p>

    <div id="mdlMembers" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;
                    </button>
                    <h4 class="modal-title">
                        <asp:Literal ID="ltrModalTitle" runat="server">Team Members</asp:Literal></h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="pnlMembers" runat="server">
                        <asp:ListView ID="lstMembers" runat="server" GroupPlaceholderID="grpContent" ItemPlaceholderID="itmContent">
                            <EmptyDataTemplate>
                                No Members
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <ul class="list-group">
                                    <asp:PlaceHolder ID="grpContent" runat="server"></asp:PlaceHolder>
                                </ul>
                            </LayoutTemplate>
                            <GroupTemplate>
                                <li class="list-group-item">
                                    <asp:PlaceHolder ID="itmContent" runat="server"></asp:PlaceHolder>
                                </li>
                            </GroupTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrMember" runat="server" Text='<%# Eval("FullName") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                    <asp:Panel ID="pnlNewTeam" runat="server" Visible="False">
                        <p>
                            <label>Team Name:</label>
                            <asp:TextBox runat="server" ID="txtNewTeamName" CssClass="form-control"></asp:TextBox>
                        </p>
                        <p>
                            <label>Group Number:</label>
                            <asp:TextBox runat="server" ID="txtNewTeamGroupNumber" CssClass="form-control"></asp:TextBox>
                        </p>
                        <p>
                            <label>Team Leader:</label>
                            <asp:DropDownList ID="ddlNewTeamLeader" runat="server" CssClass="form-control"></asp:DropDownList>
                        </p>
                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <asp:HyperLink ID="lnkManageMembers" runat="server" CssClass="btn btn-sm btn-success">
                        <span class="glyphicon glyphicon-circle"></span>
                        Manage Members
                    </asp:HyperLink>
                    <asp:LinkButton runat="server" ID="lnkAddNewTeam" CommandName="New" CssClass="btn btn-sm btn-success" Visible="False" OnClick="lnkAddNewTeam_OnClick">
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
