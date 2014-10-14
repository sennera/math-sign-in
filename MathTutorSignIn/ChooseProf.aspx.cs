using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace MathTutorSignIn
{
    public partial class ChooseProf : System.Web.UI.Page
    {

        String ErrorText = " Inform a tutor or click here to start over.";

        /**
         * The drop down list of professors is filled when the page is loaded.
         **/
        protected void Page_Load(object sender, EventArgs e)
        {
            ProfSubmitButton.Click += new EventHandler(this.SaveData_OnClick);

            //this ensures that the list is only filled the first time the page is opened
            if (!IsPostBack)
            {
                LoadProfessors();
            }

        }

        /*
         * Loads the professors for the entered class level into the drop down list.
         * Note: The Class Level should have been entered and saved in order for 
         * this information to be retrieved.
         */
        private void LoadProfessors()
        {
            DataTable levels = new DataTable();

            //Establish a connection to the database
            string connection = "Persist Security Info=False;User ID=MATH-TUTOR;Password=Math42;Initial Catalog=SignIn;Data Source=MATH-TUTOR\\MATH";
            SqlConnection sqlCon = new SqlConnection(connection);
            using (sqlCon)
            {
                try
                {
                    sqlCon.Open();

                    String enteredClassLev = Application["ClassLevel"].ToString();
                    //The sql statement to get the list for the dropdown menu. 
                    //Note: The table you are getting has a very distinct name.
                    String sql = "SELECT Distinct Professor FROM [dbo].[Class$] WHERE ClassLevel = " + enteredClassLev + ";";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlCon);

                    adapter.Fill(levels);

                    ddlProf.Enabled = true;
                    ddlProf.DataSource = levels;

                    //The data text field has to have the EXACT name of the column
                    ddlProf.DataTextField = "Professor";
                    ddlProf.DataValueField = "Professor";
                    ddlProf.DataBind();
                }
                //If there is an error, they are notified and sent back to the default page.
                catch (SqlException sqlEx)
                {
                    ToBeginning.Text = "There was an error accessing the database." + ErrorText;
                    ToBeginning.Visible = true; 
                }
                catch (Exception ex)
                {
                    ToBeginning.Text = "There was an error." + ErrorText;
                    ToBeginning.Visible = true;
                }
                finally
                {
                    sqlCon.Close();
                }

            }

            // Add the initial item - you can add this even if the options from the
            // database were not successfully loaded
            ddlProf.Items.Insert(0, new ListItem("<Select Your Professor>", "0"));

        }

        /*
         * When the button is clicked, save the entered information to be entered into the DB later,
         * then send the user to the next page, where they will enter the class time. 
         * If nothing is selected from the drop down menu, they cannot continue.
         */
        private void SaveData_OnClick(Object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            try
            {
                
                string prof = ddlProf.SelectedItem.ToString();

                if (prof == "<Select Your Professor>")
                {
                    throw new FormatException();
                }
                else
                {
                    Application["Professor"] = prof;
                    Server.Transfer("ChooseTime.aspx");
                }

            }
            catch (Exception ex)
            {
                invalidInputMessage.Visible = true;
            }
        }

    }
}