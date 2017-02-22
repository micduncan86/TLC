<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="TLC.Members.INdex"  MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Members        
        <asp:LinkButton ID="lnkAdd" runat="server" CssClass="btn btn-sm btn-success" Style="float: right;" OnClick="lnkAdd_Click" >
            <span class="glyphicon glyphicon-plus"></span>
            Add New Member
        </asp:LinkButton>
    </h3>
    <p>
        <asp:GridView ID="grdMembers" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed" RowStyle-CssClass="row" HeaderStyle-CssClass="row" BorderWidth="0"  DataKeyNames="MemberId" OnRowCommand="grdMembers_RowCommand" ShowHeader="false">
            <EmptyDataTemplate>
                No Members.
            </EmptyDataTemplate>
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
                                    <asp:LinkButton ID="lnkCopy" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' CommandName="Copy">Copy
                                    </asp:LinkButton>
                                </li>                                
                                <li role="presentation" class="divider"></li>
                                <li role="presentation">
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' CommandName="Delete" OnClientClick="javascript: return confirm('Are you sure you want to delete this member?');">Delete
                                    </asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="col-md-12" HeaderText="Name">
                    <ItemTemplate>
                        <h4><%# Eval("FullName") %> (<%# Eval("Phone") %>)</h4>
                        <a href="mailto:<%# Eval("Email") %>" class="btn btn-xs btn-info">
                            <span class="glyphicon glyphicon-envelope"></span>
                            <%# Eval("Email") %></a>
                       
                            <div class="form-group">
                                <%# Eval("Address") %>
                                <p><%# Eval("City") %>, <%# Eval("State") %> <%# Eval("ZipCode") %></p>
                            </div>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Text='<%# Eval("FullName") %>'></asp:TextBox>
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
                    <asp:Panel ID="pnlNewTeam" runat="server" Visible="False">
                        <div>
                        <div class="form-group">                             
                            <asp:TextBox runat="server" ID="txtNewMemberName" CssClass="form-control" placeholder="Name" style="max-width:none; width:425px;"></asp:TextBox>                            
                        </div>
                        <div class="form-group form-inline">   
                            <asp:TextBox runat="server" ID="txtNewMemberPhone" CssClass="form-control" placeholder="Phone" Width="140"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtNewMemberEmail" CssClass="form-control" placeholder="Email"></asp:TextBox>
                        </div>
                        
                        <div class="form-group">   
                            <asp:TextBox runat="server" ID="txtNewMemberAddress" CssClass="form-control" placeholder="Address" style="max-width:none; width:425px;"></asp:TextBox>
                        </div>
                        <div class="form-group form-inline">  
                            <asp:TextBox runat="server" ID="txtNewMemberCity" CssClass="form-control" placeholder="City" Width="250"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtNewMemberState" CssClass="form-control" placeholder="State" Width="50"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtNewMemberZipCode" CssClass="form-control" placeholder="Postal Code" Width="115"></asp:TextBox>
                        </div>
                            <div class="form-group">
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

