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
    public partial class ChooseTime : System.Web.UI.Page
    {

        String ErrorText = " Inform a tutor or click here to start over.";

        /**
         * The drop down list of class times is filled when the page is loaded.
         **/
        protected void Page_Load(object sender, EventArgs e)
        {
            
            TimeSubmitButton.Click += new EventHandler(this.SaveData_OnClick);

            //this ensures that the list is only filled the first time the page is opened
            if (!IsPostBack)
            {
                LoadClassTimes();
            }

        }

        /*
         * Loads the class times for the entered class level and professor into the drop down list.
         * Note: The Class Level and Professor should have been entered and saved in order for 
         * this information to be retrieved.
         */
        private void LoadClassTimes()
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

                    String enteredProf = Application["Professor"].ToString();
                    String enteredClassLev = Application["ClassLevel"].ToString();
                    //The sql statement to get the list for the dropdown menu. Note: The table you are getting has a very distinct name
                    String sql = @"SELECT Distinct Days, StartTime, EndTime FROM [dbo].[Class$] 
                        WHERE Professor = '" + enteredProf + "' AND ClassLevel = " + enteredClassLev + ";";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlCon);

                    adapter.Fill(levels);

                    ddlTime.Enabled = true;
                    levels.Columns.Add("Time", typeof(string), "Days + ' ' + StartTime + '-' + EndTime");
                    ddlTime.DataSource = levels;

                    //The data text field has to have the EXACT name of the column
                    ddlTime.DataTextField = "Time";
                    ddlTime.DataValueField = "Time";
                    ddlTime.DataBind();
                }

                //If there is an error, they are notified and sent back to the default page.
                catch (NullReferenceException ex)
                {
                    ToBeginning.Text = "There was an error (not all the fields were entered)." + ErrorText;
                    ToBeginning.Visible = true;
                }
                catch (SqlException sqlEx)
                {
                    ToBeginning.Text = "There was an error accessing the database." + ErrorText;
                    ToBeginning.Visible = true; 
                }
                catch (Exception exc)
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
            // db were not successfully loaded
            ddlTime.Items.Insert(0, new ListItem("<Select Your Class Time>", "0"));

        }

        /*
         * When the button is clicked, save the entered information to be entered into the DB later,
         * then send the user to the next page (the done page). 
         * If nothing is selected from the drop down menu, they cannot continue.
         */
        private void SaveData_OnClick(Object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            try
            {

                string classTime = ddlTime.SelectedItem.ToString();
                string[] classTime2 = classTime.Split(' ', '-');

                if (classTime == "<Select Your Class Time>")
                {
                    throw new FormatException();
                }
                else
                {
                    string connection = "Persist Security Info=False;User ID=MATH-TUTOR;Password=Math42;Initial Catalog=SignIn;Data Source=MATH-TUTOR\\MATH";
                    SqlConnection sqlCon = new SqlConnection(connection);
                    using (sqlCon)
                    {
                        try
                        {
                            sqlCon.Open();

                            //Finds the CRN the student is in based on the entered info.
                            String sql = @"SELECT CRN FROM Class$ WHERE Professor = '" + 
                                Application["Professor"] + "' AND ClassLevel = '" + Application["ClassLevel"] + 
                                "' AND Days = '" + classTime2[0] + "' AND StartTime = '" + classTime2[1] + "'AND EndTime ='" + 
                                classTime2[2] + "';";
                            SqlCommand myCommand2 = new SqlCommand(sql, sqlCon);
                            SqlDataReader CRNReader = myCommand2.ExecuteReader();
                            CRNReader.Read();
                            String CRN = CRNReader.GetString(0);
                            CRNReader.Close();

                            //Finds the highest SignInNum that's been used (it won't allow nulls to be entered)
                            String maxSql = @"SELECT MAX(SignInNum), MAX(Week) FROM SignIn$;";
                            SqlCommand myCommand4 = new SqlCommand(maxSql, sqlCon);
                            SqlDataReader reader = myCommand4.ExecuteReader();
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
                            //Since this is the one that's most likely to fail, this should be before inserting the student, 
                            //so that the student will only be entered once they're entered in the SignIn table
                            SqlCommand myCommand3 = new SqlCommand("INSERT INTO [dbo].[SignIn$] (Week, Day, Hour, Minute, Vnum, CRN, SignInNum)  VALUES (@param1, @param2,@param3, @param4,@param5, @param6, @param7)", sqlCon);
                            String time = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                            myCommand3.Parameters.AddWithValue("@param1", Application["Week"]);
                            myCommand3.Parameters.AddWithValue("@param2", getDate( time ));
                            myCommand3.Parameters.AddWithValue("@param3", getHour( time ));
                            myCommand3.Parameters.AddWithValue("@param4", getMin(time));
                            myCommand3.Parameters.AddWithValue("@param5", Application["VNum"]);
                            myCommand3.Parameters.AddWithValue("@param6", CRN);
                            myCommand3.Parameters.AddWithValue("@param7", nextKey);
                            myCommand3.ExecuteNonQuery();

                            //Insert student info into student table
                            SqlCommand myCommand = new SqlCommand("INSERT INTO [dbo].[Student$] (Vnum, FirstName,LastName)  VALUES (@param1, @param2,@param3)", sqlCon);
                            myCommand.Parameters.AddWithValue("@param1", Application["VNum"]);
                            myCommand.Parameters.AddWithValue("@param2", Application["FirstName"]);
                            myCommand.Parameters.AddWithValue("@param3", Application["LastName"]);
                            myCommand.CommandType = CommandType.Text;
                            myCommand.ExecuteNonQuery();

                            Application["Time"] = classTime;
                            Response.Redirect("DonePage.aspx");
                        }
                        
                        //If there is an error, they are notified and sent back to the home sign in page.
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
                    
                }

            }
            catch (FormatException ex)
            {
                invalidInputMessage.Visible = true;
            }
            catch (Exception e1)
            {
                ToBeginning.Text = "There was an error." + ErrorText;
                ToBeginning.Visible = true;
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