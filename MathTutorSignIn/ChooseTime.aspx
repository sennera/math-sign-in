<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ChooseTime.aspx.cs" Inherits="MathTutorSignIn.ChooseTime" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Your Class Time</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    &nbsp;
    <asp:DropDownList ID="ddlTime" runat="server" Height="25px" Width="300px"
        AppendDataBoundItems="true">
    </asp:DropDownList>
    <br />
    <br />
    <asp:Button ID="TimeSubmitButton" runat="server" Height="36px" Text="Submit"
        Width="277px" Enabled="True" />
    <asp:Button ID="BackButton" runat="server" PostBackUrl="~/ChooseProf.aspx" Height="31px" Text="Back" ForeColor="Gray" />
    <br />
    <asp:Label runat="server" ID="invalidInputMessage" Visible="false" Text="You must select a value to continue."></asp:Label>
    <asp:Button ID="ToBeginning" runat="server" PostBackUrl="~/HomeSignIn.aspx" Visible="false" Height="36px"
        Style="font-size:120%; font-weight:bold" Enabled="True" />
</asp:Content>
