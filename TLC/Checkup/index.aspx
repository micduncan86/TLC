<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="TLC.Checkup.index" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Check Up History</h3>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="form-group form-inline">
                        <asp:Label id="lblTeams" runat="server" Visible="false">Teams:</asp:Label>
                    <asp:DropDownList ID="ddlTeams" runat="server" CssClass="form-control" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlTeams_SelectedIndexChanged" Visible="false">
                    </asp:DropDownList>
                    <label>Members:</label>
                    <asp:DropDownList ID="ddlMembers" runat="server" CssClass="form-control" Width="150px"></asp:DropDownList>
                    <asp:Button ID="btnLoadCheckUps" runat="server" OnClick="btnLoadCheckUps_Click" Text="Load" CssClass="btn btn-sm btn-info" />
                    </div>                    
                </div>
                <div class="panel-body">
                    <asp:GridView ID="grdCheckUps" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover">
                        <Columns>
                            <asp:BoundField DataField="Member.FullName" HeaderText="Member" Visible="false" />
                            <asp:BoundField DataField="CheckUpDate" HeaderText="Date" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Method" HeaderText="Method" />
                            <asp:BoundField DataField="Outcome" HeaderText="Outcome" />
                            <asp:TemplateField HeaderText="Last Modified">
                                <ItemTemplate>
                                    <label title='<%#  Eval("ModifiedDate") %>'><%# Eval("ModifiedBy") %></label>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>
                            <strong>No Check Ups Found.</strong>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
            <a href="../Home.aspx" class="btn btn-sm btn-success">Back</a>
        </div>
    </div>
</asp:Content>
