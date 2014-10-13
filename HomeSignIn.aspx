<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HomeSignIn.aspx.cs" Inherits="MathTutorSignIn.HomeSignIn" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Welcome to the Math Center!</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>After you sign in, you can get to work!</h3>
    <h5>Your V#</h5>
    <asp:Label runat="server" ID="VLabel" Visible="true" Text="V"></asp:Label>
    <asp:TextBox ID="VNumTextBox" runat="server" AutoCompleteType="Disabled"></asp:TextBox>

    <br />
    <asp:Button ID="SubmitButton" runat="server" Height="36px" Text="Submit" Width="277px" />
    <br />
    <asp:Label runat="server" ID="invalidInputMessage" Visible="false" Text="This is not a valid V#. Please enter 8 digits (don't include the V)."></asp:Label>
    <asp:Label runat="server" ID="failedSQLMessage" Visible="false" Text="There was an error accessing the database."></asp:Label>
    <asp:Label runat="server" ID="errorMessage" Visible="false" Text="An error occurred."></asp:Label>
</asp:Content>
