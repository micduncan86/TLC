<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddToTeam.aspx.cs" Inherits="TLC.Members.AddToTeam" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-8">
            <h2>Member Search</h2>
        </div>
        <div class="col-md-4" style="margin-top:25px;">
            <div class="input-group input-group-sm pull-right">
                <input type="text" id="txtsearch" runat="server" class="form-control" placeholder="Search for...">
                <span class="input-group-btn" style="width: unset;">
                    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="btn btn-sm btn-secondary" OnClick="lnkSearch_Click">
                                <span class="glyphicon glyphicon-search"></span>
                    </asp:LinkButton>
                </span>

            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Search Results
                </div>
                <div class="panel-body" style="max-height: 500px; overflow-x: hidden; overflow-y: auto;">
                    <asp:ListView ID="lstMembers" runat="server" DataKeyNames="MemberId" GroupPlaceholderID="phGroup" ItemPlaceholderID="phItem">
                        <LayoutTemplate>
                            <table class="table table-striped table-hover">
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th class="col-xs-3">Name</th>
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
                            <td>
                                <asp:CheckBox ID="chkAdd" runat="server" />
                            </td>
                            <td class="col-xs-3"><%# Eval("FullName") %></td>
                            <td class="col-xs-4 col-mobile"><%# Eval("Email") %></td>
                            <td class="col-xs-3 col-mobile"><%# Eval("Phone") %></td>
                            <td class="col-xs-2 col-mobile"><%# Eval("Address") %></td>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-sm btn-success" OnClick="btnAdd_Click" Text="Add Members To Team" />
            <a href='<%: "../home.aspx" + Request.Url.Query %>' onclick=" var url = $(this).attr('href');app.ShowLoading(function(){window.location = url;}); return false;" class="btn btn-sm btn-primary">Back</a>
        </div>
    </div>
</asp:Content>
