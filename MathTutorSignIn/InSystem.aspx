<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InSystem.aspx.cs" Inherits="MathTutorSignIn.InSystem" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>You've already been here...</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>Is this information correct?</h3>
    Your name:
    <asp:Table ID="NameTable" runat="server" GridLines="Both" Height="32px">
    </asp:Table>
    <h3>
        <asp:Button ID="Yes" runat="server" Height="36px" Text="Yes" Width="100px" />
        <asp:Button ID="No" runat="server" PostBackUrl="~/ClassLevel.aspx" Height="36px" Text="No"
            Width="100px" />
        <asp:Button ID="Back" runat="server" PostBackUrl="~/HomeSignIn.aspx" Height="36px" Text="Back"
            Width="100px" />
        <asp:Button ID="ToBeginning" runat="server" PostBackUrl="~/HomeSignIn.aspx" Visible="false" Height="36px" 
            Style="font-size:120%; font-weight:bold" Enabled="True" />
    </h3>
    &nbsp;
</asp:Content>
