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

    public partial class ClassLevel : System.Web.UI.Page
    {

        /**
         * The drop down list of class levels is filled when the page is loaded.
         **/
        protected void Page_Load(object sender, EventArgs e)
        {
            ClassSubmitButton.Click += new EventHandler(this.SaveData_OnClick);

            //this ensures that the list is only filled the first time the page is opened
            if (!IsPostBack)
            {
                LoadClassLevels();
            }
        }

        /*
         * Loads the class levels into the drop down list.
         */
        private void LoadClassLevels()
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

                    //The sql statement to get the list for the dropdown menu. Note: The table you are getting has a very distinct name
                    String sql = "SELECT Distinct ClassLevel FROM [dbo].[Class$];";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlCon);

                    adapter.Fill(levels);

                    ddlClassLev.Enabled = true;
                    ddlClassLev.DataSource = levels;

                    //The data text field has to have the EXACT name of the column
                    ddlClassLev.DataTextField = "ClassLevel";
                    ddlClassLev.DataValueField = "ClassLevel";
                    ddlClassLev.DataBind();
                }
                //If there is an error, they are notified and sent back to the default page.
                catch (NullReferenceException ex)
                {
                    ToBeginning.Text = "There was an error (not all the fields were entered)." + ToBeginning.Text;
                    ToBeginning.Visible = true;
                }
                catch (SqlException sqlex)
                {
                    ToBeginning.Text = "There was an error accessing the database." + ToBeginning.Text;
                    ToBeginning.Visible = true; 
                }
                catch (Exception e)
                {
                    ToBeginning.Text = "There was an error." + ToBeginning.Text;
                    ToBeginning.Visible = true;
                }
                finally
                {
                    sqlCon.Close();
                }

            }

            // Add the initial item - you can add this even if the options from the
            // db were not successfully loaded
            ddlClassLev.Items.Insert(0, new ListItem("<Select Class Level>", "0"));
            ddlClassLev.Items.Insert(1, new ListItem("Other", "1"));
            ddlClassLev.Items.Insert(2, new ListItem("Placement Test", "2"));

        }


        /*
         * When the button is clicked, save the entered information to be entered into the DB later,
         * then send the user to the next page (the professors page), unless they are not at the tutoring center
         * for a class: then their information is saved and they are done.
         * If nothing is selected from the drop down menu or if the fields are blank, they cannot continue.
         */
        private void SaveData_OnClick(Object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            try
            {
                String strFName = FNameTextBox.Text.ToString().Trim();
                string strLName = LNameTextBox.Text.ToString().Trim();
                string classLev = ddlClassLev.SelectedItem.ToString();

                if (strFName.Length < 1 || strLName.Length < 1 || classLev == "<Select Class Level>")
                {
                    throw new FormatException();
                }

                String firstLetter = strFName.Substring(0, 1).ToUpper();
                strFName = firstLetter + strFName.Substring(1, strFName.Length - 1);
                String firstLetter2 = strLName.Substring(0, 1).ToUpper();
                strLName = firstLetter2 + strLName.Substring(1, strLName.Length - 1);

                //in this situation, the student does not need to provide information about their class, 
                //so their information is saved to the DB and they are transferred to the done page
                if (classLev.Equals("Other") || classLev.Equals("Placement Test"))
                {
                    string connection = "Persist Security Info=False;User ID=MATH-TUTOR;Password=Math42;Initial Catalog=SignIn;Data Source=MATH-TUTOR\\MATH";
                    SqlConnection sqlCon = new SqlConnection(connection);
                    using (sqlCon)
                    {
                        try
                        {
                            sqlCon.Open();

                            //Finds the highest SignInNum that's been used (it won't allow nulls to be entered)
                            String sql = @"SELECT MAX(SignInNum), MAX(Week) FROM SignIn$;";
                            SqlCommand myCommand3 = new SqlCommand(sql, sqlCon);
                            SqlDataReader reader = myCommand3.ExecuteReader();
                            reader.Read();
                            // Add one to the highest current primary key, if this isn't the first entry
                            int nextKey = 1;
                            if (!reader.IsDBNull(0))
                            {
                                nextKey = reader.GetInt32(0) + 1;
                            }
                            // As long as this isn't the first entry in the database, if the Week variable is empty just use the highest one in the database.
                            // During the day when the application times out, the information will be able to be saved this way
                            if (Application["Week"] == null && !reader.IsDBNull(1))
                            {
                                Application["Week"] = reader.GetInt32(1);
                            }
                            reader.Close();

                            //Insert info into SignIn table
                            String time = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                            SqlCommand myCommand2 = new SqlCommand("INSERT INTO SignIn$ (Week, Day, Hour, Minute, Vnum, CRN, SignInNum)  VALUES (@param1, @param2,@param3, @param4,@param5, @param6, @param7)", sqlCon);
                            myCommand2.Parameters.AddWithValue("@param1", Application["Week"]);
                            myCommand2.Parameters.AddWithValue("@param2", getDate(time));
                            myCommand2.Parameters.AddWithValue("@param3", getHour(time));
                            myCommand2.Parameters.AddWithValue("@param4", getMin(time));
                            myCommand2.Parameters.AddWithValue("@param5", Application["VNum"]);
                            if (classLev.Equals("Other"))
                            {
                                myCommand2.Parameters.AddWithValue("@param6", "Other");
                            }
                            else if (classLev.Equals("Placement Test")) 
                            {
                                myCommand2.Parameters.AddWithValue("@param6", "Placement Test");
                            }
                            myCommand2.Parameters.AddWithValue("@param7", nextKey);
                            myCommand2.CommandType = CommandType.Text;
                            myCommand2.ExecuteNonQuery();

                            //Insert student info into student table
                            SqlCommand myCommand = new SqlCommand("INSERT INTO Student$ (Vnum, FirstName,LastName)  VALUES (@param1, @param2,@param3)", sqlCon);
                            myCommand.Parameters.AddWithValue("@param1", Application["VNum"]);
                            myCommand.Parameters.AddWithValue("@param2", strFName);
                            myCommand.Parameters.AddWithValue("@param3", strLName);
                            myCommand.CommandType = CommandType.Text;
                            myCommand.ExecuteNonQuery();
                        }
                        //If there is an error, they are notified and sent back to the default page.
                        catch (NullReferenceException ex)
                        {
                            ToBeginning.Text = "There was an error (not all the fields were entered)." + ToBeginning.Text;
                            ToBeginning.Visible = true;
                        }
                        catch (SqlException sqlex)
                        {
                            ToBeginning.Text = "There was an error accessing the database." + ToBeginning.Text;
                            ToBeginning.Visible = true; 
                        }
                        catch (Exception exc)
                        {
                            ToBeginning.Text = "There was an error." + ToBeginning.Text;
                            ToBeginning.Visible = true;
                        }
                        finally
                        {
                            sqlCon.Close();
                        }
                    }
                    Response.Redirect("DonePage.aspx");
                }
                else
                {
                    Application["FirstName"] = strFName;
                    Application["LastName"] = strLName;
                    Application["ClassLevel"] = classLev;
                    Response.Redirect("ChooseProf.aspx");
                }

            }
            catch (FormatException ex)
            {
                invalidInputMessage.Visible = true;
            }
        }

        /*
         * Parses the time String to get the date.
         */
        String getDate(String s)
        {
            char[] arr = new char[s.Length];
            arr[0] = s[4];
            arr[1] = s[5];
            arr[2] = '/';
            arr[3] = s[6];
            arr[4] = s[7];
            arr[5] = '/';
            arr[6] = s[0];
            arr[7] = s[1];
            arr[8] = s[2];
            arr[9] = s[3];
            return new String(arr);
        }

        /*
         * Parses the time String to get the hour.
         */
        String getHour(String s)
        {
            char[] arr = new char[s.Length];
            arr[0] = s[8];
            arr[1] = s[9];

            return new String(arr);
        }

        /*
         * Parses the time String to get the minute.
         */
        String getMin(String s)
        {

            char[] arr = new char[s.Length];
            arr[0] = s[10];
            arr[1] = s[11];

            return new String(arr);
        }

    }
}