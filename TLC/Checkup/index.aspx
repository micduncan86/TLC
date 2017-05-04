<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="TLC.Checkup.index" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Check Up History
        <a class="btn btn-sm btn-info" Style="float: right; margin: 0 5px;" href="../home.aspx?TeamId=<% Response.Write(Request.Params.Get("TeamId")); %>" onclick=" var url = $(this).attr('href');app.ShowLoading(function(){window.location = url;}); return false;">
            <span class="glyphicon glyphicon-menu-left"></span>
            Back to Team
        </a>
    </h3>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="form-group form-inline">
                        <asp:Label id="lblTeams" runat="server" Visible="false">Teams:</asp:Label>
                    <asp:DropDownList ID="ddlTeams" runat="server" CssClass="form-control" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlTeams_SelectedIndexChanged" Visible="false">
                    </asp:DropDownList>
                    <label>Members:</label>
                    <asp:DropDownList ID="ddlMembers" ClientIDMode="Static" runat="server" CssClass="form-control" Width="150px"></asp:DropDownList>
                    <asp:Button ID="btnLoadCheckUps" runat="server" OnClick="btnLoadCheckUps_Click" Text="Load" CssClass="btn btn-sm btn-info" />
                        <asp:Button ID="btnAddCheckUp" ClientIDMode="Static" runat="server" Text="Add Check Up" CssClass="btn btn-sm btn-success" OnClick="btnAddCheckUp_Click" />
                    </div>                    
                </div>
                <div class="panel-body" style="min-height:400px; max-height:400px; overflow:auto;">
                    <asp:GridView ID="grdCheckUps" runat="server" DataKeyNames="CheckUpId" AutoGenerateColumns="false" CssClass="table table-striped table-hover" OnRowCommand="grdCheckUps_RowCommand" OnRowDataBound="grdCheckUps_RowDataBound">
                        <Columns>   
                            <asp:ButtonField CommandName="Edit" ControlStyle-CssClass="btn btn-xs btn-default" Text="<span class='glyphicon glyphicon-pencil'></span>" />                     
                            <asp:TemplateField Visible="false" HeaderText="Member">
                                <ItemTemplate>
                                    <asp:Label ID="lblMember" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CheckUpDate" HeaderText="Date" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Method" HeaderText="Method" />
                            <asp:BoundField DataField="Outcome" HeaderText="Outcome" HeaderStyle-CssClass="col-mobile" ItemStyle-CssClass="col-mobile" />
                            <asp:TemplateField  HeaderText="Follow Up?" HeaderStyle-CssClass="col-mobile" ItemStyle-CssClass="col-mobile">
                                <ItemTemplate>
                                    <label title='<%#  Eval("Actions") %>' style='cursor:<%# Convert.ToBoolean(Eval("RequiresAction")) ? "help" : "pointer" %>'><%# Convert.ToBoolean(Eval("RequiresAction")) ? "Yes" : "No" %></label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Modified" HeaderStyle-CssClass="col-mobile" ItemStyle-CssClass="col-mobile">
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
        </div>
    </div>
    <div id="modalCheckUp" class="modal fade" role="dialog">
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
                    <asp:Panel ID="pnlNewCheckUp" runat="server">
                        <div>
                            <input type="hidden" id="txtTeamId" runat="server" />
                            <input type="hidden" id="txtMemberId" runat="server" />
                            <div class="form-group">
                                <label>Check Up Date:</label>
                                <asp:TextBox runat="server" ID="txtCheckUpDate" CssClass="form-control datepicker" placeholder="Date" Width="140"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Method:</label>
                                <asp:DropDownList ID="ddlMethod" runat="server" CssClass="form-control" Width="140px"></asp:DropDownList>
                            </div>

                            <div class="form-group">
                                <label>Outcome:</label>
                                <asp:TextBox runat="server" ID="txtOutcome" Rows="5" CssClass="form-control" placeholder="Outcome" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div class="form-group form-inline">
                                <asp:CheckBox ID="chkActionRequired" ClientIDMode="Static" runat="server" Text="Is Follow Up actions required?" />
                            </div>
                            <div id="followupActions" class="form-group " style="display:none;">
                                <label>Follow Up Actions:</label>
                                <asp:TextBox runat="server" ID="txtActions" Rows="5" CssClass="form-control" placeholder="Actions" TextMode="MultiLine" Style=""></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton runat="server" ID="lnkAddCheckUp" CommandName="New" CssClass="btn btn-sm btn-success" OnClick="lnkAddCheckUp_Click" >
                        <span class="glyphicon glyphicon-plus"></span>
                        Add Check Up
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfShowModal" ClientIDMode="Static" runat="server" />
    <script type="text/javascript">
        $(document).ready(function () {
            $(".datepicker").datepicker();

            $("#chkActionRequired").click(function () {
                $("#followupActions").toggle(this.checked);
            })
            if ($("#hdfShowModal").val() == "1") {
                $("#modalCheckUp").modal("show");
                if ($("#chkActionRequired").is(":checked")) {
                    $("#MainContent_txtActions").show();
                }
            }
            $("#ddlMembers").change(function () {
                $("#btnAddCheckUp").toggle($(this).val() != "0");  
            });
        });
    </script>
</asp:Content>
