<%@ Page Title="ChooseClassLevel" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClassLevel.aspx.cs" Inherits="MathTutorSignIn.ClassLevel" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Your Information</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <ol class="round">
        <li class="one">
            <h5>First Name</h5>
            <asp:TextBox ID="FNameTextBox" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
        </li>
        <li class="two">
            <h5>Last Name</h5>
            <asp:TextBox ID="LNameTextBox" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
        </li>
        <li class="three">
            <h5>Class Level</h5>
            <asp:DropDownList ID="ddlClassLev" runat="server" Height="25px" Width="300px"
                AppendDataBoundItems="true">
            </asp:DropDownList>
            <br />
            <br />
            <asp:Button ID="ClassSubmitButton" runat="server" Height="36px" Text="Submit"
                Width="164px" Enabled="True" />

            <asp:Button ID="BackButton" runat="server" PostBackUrl="~/HomeSignIn.aspx" Height="31px" Text="Back" ForeColor="Gray" />
            <br />
            <asp:Label runat="server" ID="invalidInputMessage" Visible="false" Text="You must enter values for all fields."></asp:Label>
            <asp:Button ID="ToBeginning" runat="server" PostBackUrl="~/HomeSignIn.aspx" Visible="false" Height="36px" Style="font-size:120%; font-weight:bold" Enabled="True" />
    </ol>
</asp:Content>
