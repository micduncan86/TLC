<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="TLC.Teams.Add" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmAdd" runat="server" method="post" action="api/team">
        <div class="form-group">
            <p>
            <label for="TeamName">Team Name:</label>
            <input type="text" id="TeamName" runat="server" name="TeamName" class="form-control" placeholder="Team Name" />
             </p>
            <p>
            <label for="GroupNumber">Group Number:</label>
            <input type="text" id="GroupNumber" runat="server" name="GroupNumber" class="form-control" placeholder="Group Number" />
             </p>
            <p>
                <label for="TeamLeader">Team Leader:</label>
                <asp:DropDownList ID="TeamLeaderId"  runat="server" class="form-control"></asp:DropDownList>                
            </p>
        </div>
    </form>
</body>
</html>
