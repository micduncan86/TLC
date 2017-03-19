<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="TLC.Email.test" MasterPageFile="~/Site.Master" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <input type="email" placeholder="To Address" id="txtToEmail" runat="server" />
    <input type="text" placeholder="Subject" id="txtSubject" runat="server" />
    <textarea id="txtBody" placeholder="Body" runat="server"></textarea>
    <asp:Button ID="btnSend" runat="server"  Text="Send" OnClick="btnSend_Click"/>




</asp:Content>
    