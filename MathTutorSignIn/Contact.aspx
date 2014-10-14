<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="MathTutorSignIn.Contact" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
        <h2>Problems with the online sign-in page? Here's who to contact.</h2>
    </hgroup>

    <section class="contact">
        <header>
            <h3>Phone:</h3>
        </header>
        <p>
            <span class="label">WOU University Computing Services (ask for Michael Ellis):</span>
            <span>503-838-8925</span>
        </p>
    </section>

    <section class="contact">
        <header>
            <h3>Email:</h3>
        </header>
        <p>
            <span class="label">WOU IT:</span>
            <span><a href="mailto:ucshelpdesk@wou.edu">ucshelpdesk@wou.edu</a></span>
        </p>
        <p>
            <span class="label">April:</span>
            <span><a href="mailto:asenner11@wou.edu">asenner11@wou.edu</a></span>
        </p>
    </section>

</asp:Content>