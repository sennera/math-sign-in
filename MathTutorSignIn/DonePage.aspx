<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="DonePage.aspx.cs" Inherits="MathTutorSignIn.DonePage" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Done!</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>Thank you, you're all done! Good luck with your math endeavors!</h3>
    <br />
    <asp:Button ID="ToBeginning" runat="server" PostBackUrl="~/HomeSignIn.aspx" Height="36px" Text="Back to Home"
        Width="277px" Enabled="True" />
</asp:Content>
