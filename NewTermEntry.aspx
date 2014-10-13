<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewTermEntry.aspx.cs" Inherits="MathTutorSignIn.NewTermEntry" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Enter Information About This Term&#39;s Math Classes</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h4>Add Class Info</h4>
    <h5>This is only to be done at the beginning of the term. </h5>
    The order of the columns should be: <br />
    <b>CRN | Class Level | Start Time | End Time| Days | Instructor</b>
    <br />
    This data should be tab-delimited. 
    <br /><br />
    <b>Instructions:</b><br />
    1. Copy and paste class data from WOU RealTime Availability website to Notepad, which is in Accessories (to strip the formatting). <br />
    2. Copy and paste that data into Excel (Ctrl-A, Ctrl-C). <br />
    3. Remove the unneccesary fields so that it matches the order and columns above. <br /> 
    4. For classes that span multiple rows, make it so they are only on one row. (If it has M, W, and TF split up, put them all on the same line.)
    5. Copy and paste the data from Excel into the text box below.  <br /> 
    <br />

    <asp:TextBox ID="InputData" runat="server" TextMode="multiline" Height="120px"></asp:TextBox> <br /> 
    <asp:Label ID="NumColsWarning" runat="server" Text="The number of fields is incorrect for at least one of your entries. Please make sure to enter the fields specified above." Visible="False"></asp:Label>
    <asp:Label ID="SomeError" runat="server" Text="Sorry, there was an error. Your data has not been entered." Visible="False"></asp:Label>
    <asp:Label ID="DuplicateKeyError" runat="server" Text="There was an error with the query. There may have been a duplicated CRN added." Visible="False"></asp:Label>
    <br />
    <asp:Label ID="PreviewLabel" runat="server" Text="Preview your data to input: " Visible="False"></asp:Label><br /> 
    <asp:Table ID="PreviewTable" runat="server" Visible="false" GridLines="Both" BorderColor="Black"></asp:Table> <br /> 

    <asp:Button ID="FinalSaveButton" runat="server" Text="Looks good, save it!" Visible="false"/>
    <asp:Button ID="ClearFieldsButton" runat="server" Text="Redo" Visible="false" />
    <asp:Button ID="SaveButton" runat="server" Text="Save" />
    <asp:Button ID="DoneButton1" runat="server" Text="Done" />
    <br />
    <br />

    <h4>Delete A Class From Database</h4>
    Enter the CRN you would like to delete. Data cannot be recovered and must be re-entered if deleted.<br /><br />
    <asp:Label ID="Label2" runat="server" Text="CRN" Font-Size="Medium" ></asp:Label>
    <asp:TextBox ID="CRNText" runat="server" ></asp:TextBox> <br /> 

    <asp:Label ID="DeleteLabel" runat="server" Text="" Visible="False"></asp:Label><br /> 
    <asp:Button ID="DeleteButton" runat="server" Text="Delete" Height="39px" Font-Size="Small" Width="77px" />
    <asp:Button ID="DoneButton2" runat="server" Text="Done" Height="39px" Font-Size="Small" Width="75px" />
</asp:Content>
