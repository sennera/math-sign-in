<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MathTutorSignIn._Default" %>

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
    <table>
        <tr>
            <td class="auto-style1">

                <h2>Are you a math tutor? </h2>

                <h4>Week #
                    <asp:DropDownList ID="ddlWeekNum" runat="server">
                        <asp:ListItem>-</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </h4>

                <h5>Password</h5>
                <asp:TextBox ID="TutorPassword" runat="server" AutoCompleteType="Disabled" TextMode="Password"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="SubmitButton" runat="server" Height="36px" Text="Go to Sign In Sheet" Width="200px" />
                <br />
                <asp:Label ID="invalidWeekEntry" Visible="false" runat="server" Text="You must enter the week number to proceed to the sign in page."></asp:Label>
                <br />
                <asp:Label runat="server" ID="invalidPasswordMessage" Visible="false" Text="That is not the correct tutor password."></asp:Label>
            </td>

            <td>
                <br />
                <br />
                <h3>Are you a math faculty? </h3>
                <h5>Password</h5>

                <asp:TextBox ID="FacultyPassword" runat="server" TextMode="Password"></asp:TextBox>

                <br />
                <asp:Button ID="ToQueryPage" runat="server" Height="36px" Text="View Sign-in Sheet" Width="200px" />
                <br />
                <asp:Label runat="server" ID="invalidPassword2" Visible="false" Text="That is not the correct faculty password."></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style1 {
            width: 413px;
        }
    </style>
</asp:Content>

