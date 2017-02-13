<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Members.aspx.cs" Inherits="TLC.Teams.Members" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>
        <label style="max-width: 200px; text-overflow:clip; white-space:nowrap; overflow:hidden;"><asp:Literal ID="ltrHeader" runat="server"></asp:Literal></label>
        <asp:LinkButton ID="lnkAdd" runat="server" CssClass="btn btn-sm btn-success" Style="float: right;" OnClick="lnkAdd_Click">
            <span class="glyphicon glyphicon-plus"></span>
            Add New
        </asp:LinkButton>
    </h3>
    <hr />
    <p>
        <asp:GridView ID="grdMembers" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed" RowStyle-CssClass="row" HeaderStyle-CssClass="row" BorderWidth="0" DataKeyNames="TeamMemberId" OnRowCommand="grdMembers_RowCommand" ShowHeader="false">
            <Columns>
                             <asp:CommandField ShowDeleteButton="true" ControlStyle-CssClass="btn btn-sm btn-danger" DeleteText="<span class='glyphicon glyphicon-remove'></span>" />
                <asp:TemplateField ItemStyle-CssClass="col-md-12">
                    <ItemTemplate>
                        <p><%# Eval("FullName") %></p>
                    <p>
                        <strong>P:</strong><%# Eval("Phone") %>
                        <strong>E:</strong><%# Eval("Email") %>
                    </p>
                    </ItemTemplate>                    
                </asp:TemplateField>
<%--                <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                <asp:BoundField DataField="Phone" HeaderText="Phone" />
                <asp:BoundField DataField="Email" HeaderText="Email" />--%>
   
            </Columns>
            <EmptyDataTemplate>
                No Members
            </EmptyDataTemplate>
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
                    <asp:Panel ID="pnlNewMembers" runat="server" Style="max-height: 350px; overflow-y: scroll;">
                        <asp:ListView ID="lstMembers" runat="server" GroupPlaceholderID="grpContent" ItemPlaceholderID="itmContent" DataKeyNames="TeamMemberId">
                            <EmptyDataTemplate>
                                No Members
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <ul class="list-group">
                                    <asp:PlaceHolder ID="grpContent" runat="server"></asp:PlaceHolder>
                                </ul>
                            </LayoutTemplate>
                            <GroupTemplate>
                                <li class="list-group-item" style="min-height: 90px;">
                                    <asp:PlaceHolder ID="itmContent" runat="server"></asp:PlaceHolder>
                                </li>
                            </GroupTemplate>
                            <ItemTemplate>
                                <div style="float: left; padding-right: 15px;">
                                    <asp:CheckBox ID="chkAdd" runat="server" />
                                </div>
                                <div style="padding-left:25px;">
                                    <asp:Literal ID="ltrMember" runat="server" Text='<%# Eval("FullName") %>'></asp:Literal>
                                    <p>
                                        <strong>P:</strong><asp:Literal ID="ltrEmail" runat="server" Text='<%# Eval("Phone") %>'></asp:Literal>
                                        <strong>E:</strong><asp:Literal ID="Literal1" runat="server" Text='<%# Eval("Email") %>'></asp:Literal>
                                    </p>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>                        
                    </asp:Panel>
                    <div class="list-group-item">
                        <h3>Add New Member</h3>
                            <asp:TextBox ID="txtnewMemberName" runat="server" placeholder="First Name" CssClass="form-control"></asp:TextBox>       
                            <input ID="txtnewMemberPhone" runat="server" placeholder="Phone" class="form-control" type="tel" />
                            <input ID="txtnewMemberEmail" runat="server" placeholder="Email" class="form-control" type="email" />
                        </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lnkAddMember" runat="server" CssClass="btn btn-sm btn-success" OnClick="lnkAddMember_Click">
                        <span class="glyphicon glyphicon-circle"></span>
                        Add Team Member
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
