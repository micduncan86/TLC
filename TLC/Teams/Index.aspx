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
        <asp:GridView ID="grdTeams" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed" RowStyle-CssClass="row" HeaderStyle-CssClass="row" BorderWidth="0" OnRowCommand="grdTeams_RowCommand" DataKeyNames="TeamId" OnRowDataBound="grdTeams_RowDataBound" OnRowUpdating="grdTeams_RowUpdating">
            <Columns>
                <asp:BoundField DataField="TeamId" HeaderText="Team ID" HeaderStyle-CssClass="col-md-1" ControlStyle-CssClass="form-control" ReadOnly="true" />
                <asp:BoundField DataField="GroupNumber" HeaderText="Group #" HeaderStyle-CssClass="col-md-1" ControlStyle-CssClass="form-control" />
                <asp:TemplateField HeaderStyle-CssClass="col-md-2" HeaderText="Team Leader">
                    <ItemTemplate>
                        <asp:Literal ID="ltrTeamLeader" runat="server" Text='<%# Eval("TeamLeader.FullName") %>'></asp:Literal>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlLeader" runat="server" CssClass="form-control"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="col-md-7" HeaderText="Name">
                    <ItemTemplate>
                        <asp:Literal ID="ltrName" runat="server" Text='<%# Eval("Name") %>'></asp:Literal>
                        <p>
                            <asp:LinkButton ID="lnkMembers" runat="server" CssClass="btn btn-sm btn-default" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' CommandName="LoadMembers">
                                 <span class="glyphicon glyphicon-user"></span>
                                <span class="badge">
                                    <%# Eval("Members.Count") %>
                                </span>
                            </asp:LinkButton>
                        </p>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Text='<%# Eval("Name") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="col-md-1">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' CommandName="Edit" CssClass="btn btn-sm btn-primary">
                        <span class="glyphicon glyphicon-pencil"></span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' CommandName="Delete" CssClass="btn btn-sm btn-warning">
                        <span class="glyphicon glyphicon-trash" onclick="javascript: return confirm('Are you sure you want to delete this team?');"></span>
                        </asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' CommandName="Update" CssClass="btn btn-sm btn-success">
                        <span class="glyphicon glyphicon-ok"></span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lnkCancel" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' CommandName="Cancel" CssClass="btn btn-sm btn-danger">
                        <span class="glyphicon glyphicon-remove"></span>
                        </asp:LinkButton>
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
                    <h4 class="modal-title">Team Members</h4>
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
                </div>
                <div class="modal-footer">
                    <a class="btn btn-sm btn-success" href="../Members/index.aspx">
                        <span class="glyphicon glyphicon-circle"></span>
                        Manage Members
                    </a>
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
