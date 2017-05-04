<%@ Page Title="Check Up" Language="C#" AutoEventWireup="true" CodeBehind="add.aspx.cs" Inherits="TLC.Checkup.add" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">    
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%# Page.Title = "Check Up" %>
    <h3>Add Check Up</h3>
    <div class="row">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="panel panel-default">
                <div class="panel-heading">
                </div>
                <div class="panel-body">
                    <div>
                        <div class="form-group ">
                            <label>Team:</label>
                            <asp:DropDownList ID="ddlTeam" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group ">
                            <label>Member:</label>
                            <asp:DropDownList ID="ddlMember" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group ">
                            <label>Date:</label>
                            <asp:TextBox runat="server" ID="txtCheckUpDate" CssClass="form-control datepicker" placeholder="Check Up Date"></asp:TextBox> 
                            <asp:RequiredFieldValidator ID="reqDate" runat="server" CssClass="danger" ControlToValidate="txtCheckUpDate" ErrorMessage="Check Up Date is required."></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID ="valIsDate" runat="server" CssClass="danger" ControlToValidate ="txtCheckUpDate" Operator="DataTypeCheck" Type="Date" ErrorMessage="Enter a Valid Date"></asp:CompareValidator>
                        </div>
                        <div class="form-group ">
                            <label>Method:</label>
                            <asp:DropDownList ID="ddlMethod" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label>OutCome:</label>
                            <asp:TextBox runat="server" ID="txtOutCome" CssClass="form-control" placeholder="OutCome" TextMode="MultiLine"></asp:TextBox> 
                        </div>
                        <div class="form-group">
                            <asp:CheckBox ID="chkActionRequired" runat="server" ClientIDMode="Static" Text="Requires FollowUp" />
                            <asp:TextBox runat="server" ID="txtFollowUpAction" ClientIDMode="Static" CssClass="form-control" placeholder="Action" TextMode="MultiLine" style="display:none;"></asp:TextBox>
                            
                        </div>                        
                    </div>
                    <asp:LinkButton ID="lnkAddCheckUp" runat="server" CssClass="btn btn-sm btn-success" OnClick="lnkAddCheckUp_Click">
                        <span class="glyphicon glyphicon-check"></span> Add Check Up
                    </asp:LinkButton>
                    <a href="../Home.aspx" class="btn bnt-sm btn-info" onclick=" var url = $(this).attr('href');app.ShowLoading(function(){window.location = url;}); return false;">Back</a>
                </div>
            </div>
        </div>
    </div>
    
    <asp:HiddenField ID="hdfShowModal" ClientIDMode="Static" runat="server" />
    <script type="text/javascript">
        $(document).ready(function () {
            $(".datepicker").datepicker();

            $("#chkActionRequired").click(function () {
                $("#txtFollowUpAction").toggle(this.checked);
            })
            if ($("#hdfShowModal").val() == "1") {
                $("#modalCheckUp").modal("show");
            }
        });
    </script>
</asp:Content>
