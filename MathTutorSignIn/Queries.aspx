<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Queries.aspx.cs" Inherits="MathTutorSignIn.Queries" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Retrieving Sign-in Sheet Data and Other Information</h1>
            </hgroup>

        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <asp:Label ID="ErrorSuccess" runat="server" Text="" Font-Size="Medium" Visible="false" ></asp:Label><br />
    <asp:Table ID="OutputTable" runat="server" Visible="false" GridLines="Both"></asp:Table>
                
    <br />
    <br />    
    <br /> Note: Allow a minute for the queries to be displayed after choosing what to view.<br />

    <asp:Button ID="GetWholeSignInButton" runat="server" Text="Get entire sign-in sheet" />
    <br />
    <asp:Button ID="GetWeeklySignInButton" runat="server" Text="Get sign-in sheet for week:" />
    <asp:DropDownList ID="WeekNumDropDownList" runat="server" Height="37px" Style="margin-left: 0px">
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
    <br />
    <br />
    <h3>For Professors:</h3>
    <asp:LinkButton ID="SortByClassLevel" runat="server" Text="Sort by Class Level"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />
    <asp:LinkButton ID="SortByCRN_NumTimesPerStudent" runat="server" Text="Sort by CRN and get number of visits per student"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />
    <asp:LinkButton ID="SortByCRN_NumTimesPerClass" runat="server" Text="Sort by CRN and get number of visits for each class"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />
    <br />
    <asp:LinkButton ID="SortByProfessor" runat="server" Text="Sort by Professor"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />
    <asp:LinkButton ID="SortByProfessor_Totals" runat="server" Text="Sort by Professor and get number of visits for each class"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />

    <h3>Other useful information from sign-in sheet:</h3>
    <asp:LinkButton ID="SortByLevel" runat="server" Text="How many students came in for each class level?"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />
    <br />
    <asp:LinkButton ID="SortByStudent" runat="server" Text="Sort by Student Name"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />
    <br />
    <asp:LinkButton ID="PerDay" runat="server" Text="How many students came each day?"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />
    <asp:LinkButton ID="PerWeek" runat="server" Text="How many students came in each week?"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />
    <asp:LinkButton ID="PerHour" runat="server" Text="How many students came in each hour of the day?"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />
    <br />
    <asp:LinkButton ID="AvgPerDay" runat="server" Text="What is the average number of students per day?"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />
    <asp:LinkButton ID="AvgPerWeek" runat="server" Text="What is the average number of students per week?"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />
    <br />
    <asp:LinkButton ID="NonMathClasses" runat="server" Text="How many students did not come in for any of the 
        specified math classes (they came for a different class or another reason)?"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>
    <br />
    <br />
    <h3>For Math Secretary:</h3>
    <asp:LinkButton ID="AllClassesQuery" runat="server" Text="See All Classes Currently in the Database"
        CommandName="RunQuery" Font-Size="Medium"></asp:LinkButton>

    <br />
    <br />
    <h3>FOR THE BEGINNING OF THE TERM:</h3>
    After last term's class information has been cleared, the information for the next term's courses must be entered.
    <br />
    <asp:Button ID="InsertNewInfoButton" runat="server" Text="Add Courses for Current Term" />
    <br />
    <br />
    <br />
    <i>WARNING: This button DELETES all information and starts fresh. This is only to be done at the beginning of each term<br />
    by the math secretary once all of the previous term&#39;s information has been saved.</i><br />
    <asp:Button ID="ResetButton" runat="server" Text="Reset Sign-in Sheet for New Term" Font-Size="Small" />
    <br />
    <asp:Label ID="WarningLabel" runat="server" BorderStyle="Solid" ForeColor="Black" Text="Are you sure you want to delete ALL information?" Visible="False"></asp:Label>
    <br />
    <asp:Button ID="YesButton" runat="server" Text="Yes" Visible="False" />
    <asp:Button ID="NoButton" runat="server" Text="No" Visible="False" />
    
</asp:Content>
