<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ChooseProf.aspx.cs" Inherits="MathTutorSignIn.ChooseProf" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Your Professor</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:SignInConnectionString %>"
        SelectCommand="SELECT Distinct Professor
                        From Class;"></asp:SqlDataSource>--%>
    &nbsp;
    <asp:DropDownList ID="ddlProf" runat="server" Height="25px" Width="300px"
        AppendDataBoundItems="true">
    </asp:DropDownList>
    <br />
    <br />

    <asp:Button ID="ProfSubmitButton" runat="server" Height="36px" Text="Submit"
        Width="277px" Enabled="True" />
    <asp:Button ID="BackButton" runat="server" PostBackUrl="~/ClassLevel.aspx" Height="31px" Text="Back" ForeColor="Gray" />
    <br />
    <asp:Label runat="server" ID="invalidInputMessage" Visible="false" Text="You must select a value to continue."></asp:Label>
    <asp:Button ID="ToBeginning" runat="server" PostBackUrl="~/HomeSignIn.aspx" Visible="false" Height="36px" Text=" Inform a tutor or click here to start over."
        Style="font-size:120%; font-weight:bold" Enabled="True" />
</asp:Content>
