<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Events.aspx.cs" Inherits="TLC.Teams.Events" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" href="../Content/bootstrap-datepicker.css" />

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Events        
        <asp:LinkButton ID="lnkAdd" runat="server" CssClass="btn btn-sm btn-success" Style="float: right;" OnClick="lnkAdd_Click">
            <span class="glyphicon glyphicon-plus"></span>
            Add New Event
        </asp:LinkButton>
    </h3>
    <div>
        <div class="row">
            <div class="col-md-12">
                <asp:GridView ID="grdEvents" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" RowStyle-CssClass="row" HeaderStyle-CssClass="row" BorderWidth="0" DataKeyNames="EventId" ShowHeader="false" OnRowCommand="grdEvents_RowCommand">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="90px">
                            <ItemTemplate>
                                <div class="dropdown">
                                    <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                        Actions <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu" role="menu">
                                        <li role="presentation">
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' CommandName="Edit">
                                        <span class="glyphicon glyphicon-pencil btn btn-xs btn-info"></span>
                                        Edit
                                            </asp:LinkButton>
                                        </li>
                                        <li role="presentation" class="divider"></li>
                                        <li role="presentation">
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' CommandName="Delete" OnClientClick="javascript: return confirm('Are you sure you want to delete this team?');">
                                        <span class="glyphicon glyphicon-trash btn btn-xs btn-danger"></span>
                                        Delete
                                            </asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <h4><%# Eval("Title") %></h4>
                                <div class="form-group">
                                    <%# Eval("EventDate") %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <EmptyDataTemplate>
                        No Members
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>

    <div id="mdlMembers" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;
                    </button>
                    <h4 class="modal-title">
                        <asp:Literal ID="modalTitle" runat="server">Event</asp:Literal>
                    </h4>
                </div>
                <div class="modal-body">
                    <div>
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtEventTitle" CssClass="form-control" placeholder="Title" Style="max-width: none; width: 425px;"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <div class="input-group date" data-provider="datepicker">
                                <asp:TextBox ID="txtEventDate" runat="server" CssClass="form-control datepicker" data-date-format="mm/dd/yyyy" placeholder="Event Date"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group form-inline">
                            <asp:CheckBox ID="chkIsComplete" runat="server" CssClass="" Text=" Is Event Complete?" />
                        </div>
                        <div class="form-group">
                            <label>Team Selection:</label>
                            <asp:DropDownList ID="ddlTeam" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtEventNotes" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="5" placeholder="Notes:"></asp:TextBox>
                        </div>
                        "
                    </div>

                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lnkAddEvent" runat="server" CssClass="btn btn-sm btn-success" OnClick="lnkAddEvent_Click">
                        <span class="glyphicon glyphicon-circle"></span>
                        Add Event
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
                $("#mdlMembers").on("shown.bs.modal", function () {
                    $(".datepicker").datepicker({
                        format: "mm/dd/yyyy",
                        startDate: "-1d"
                    });
                });
            }
        });
    </script>
    <script src="../Scripts/bootstrap-datepicker.js"></script>
</asp:Content>
