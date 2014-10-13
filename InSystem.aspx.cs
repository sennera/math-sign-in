using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MathTutorSignIn
{
    public partial class InSystem : System.Web.UI.Page
    {

        /**
         * The table is filled with the student's information based on the V# they entered on the previous page.
         **/
        protected void Page_Load(object sender, EventArgs e)
        {
            FillNameTable(NameTable);
            Yes.Click += new EventHandler(this.inTheSystem);
        }

        /*
         * Loads the student info into the table.
         */
        void FillNameTable(Table tableToBeFilled)
        {
            try
            {
                //Establish a database connection
                SqlConnection myCon = new SqlConnection("Persist Security Info=False;User ID=MATH-TUTOR;Password=Math42;Initial Catalog=SignIn;Data Source=MATH-TUTOR\\MATH");
                myCon.Open();

                //Get the list of Vnumbers using a query
                string query = "SELECT Vnum as 'V#', FirstName as 'First Name', LastName as 'Last Name' FROM [dbo].[Student$] WHERE VNum = " + Application["VNum"] + ";";
                SqlCommand command = new SqlCommand(query, myCon);
                SqlDataReader reader = command.ExecuteReader();

                //Checks each vnumber and goes to the appropriate page
                while (reader.Read())
                {
                    TableRow newRow = new TableRow();

                    for (int i = 0; i <= 2; i++)
                    {
                        TableCell newCell = new TableCell();
                        newCell.Controls.Add(new LiteralControl(reader.GetString(i)));
                        newRow.Cells.Add(newCell);
                    }
                    tableToBeFilled.Rows.Add(newRow);
                }

                reader.Close();
                myCon.Close();
            }
            //If there is an error, they are notified and sent back to the default page.
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
                
        }

        /*
         * When the button is clicked, the student's information is saved to the DB, 
         * then the user is sent to the done page.
         */
        void inTheSystem(Object sender, EventArgs e)
        {
            SqlConnection myCon = new SqlConnection("Persist Security Info=False;User ID=MATH-TUTOR;Password=Math42;Initial Catalog=SignIn;Data Source=MATH-TUTOR\\MATH");
            try
            {
                myCon.Open();

                string query = "SELECT Distinct CRN FROM [dbo].[SignIn$] WHERE VNum = " + Application["VNum"] + ";";
                SqlCommand command = new SqlCommand(query, myCon);
                SqlDataReader CRNReader = command.ExecuteReader();
                CRNReader.Read();
                String CRN = CRNReader.GetString(0);
                CRNReader.Close();

                //Finds the highest SignInNum that's been used (it won't allow nulls to be entered)
                String sql = @"SELECT MAX(SignInNum), MAX(Week) FROM SignIn$;";
                SqlCommand myCommand3 = new SqlCommand(sql, myCon);
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
            
                string insertQuery = "INSERT INTO SignIn$ (Week, Day, Hour, Minute, Vnum, CRN, SignInNum) VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7)";
                SqlCommand Command2 = new SqlCommand(insertQuery, myCon);
                String time = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                Command2.Parameters.AddWithValue("@param1", Application["Week"]);
                Command2.Parameters.AddWithValue("@param2", getDate(time));
                Command2.Parameters.AddWithValue("@param3", getHour(time));
                Command2.Parameters.AddWithValue("@param4", getMin(time));
                Command2.Parameters.AddWithValue("@param5", Application["VNum"]);
                Command2.Parameters.AddWithValue("@param6", CRN);
                Command2.Parameters.AddWithValue("@param7", nextKey);
                Command2.ExecuteNonQuery();     
                
                Response.Redirect("DonePage.aspx");
            }
                        
            //If there is an error, they are notified and sent back to the home sign in page.
            catch (SqlException sqlex)
            {
                ToBeginning.Text = "There was an error accessing the database." + ToBeginning.Text;
                ToBeginning.Visible = true; 
            }
            catch (Exception ex)
            {
                ToBeginning.Text = "There was an error." + ToBeginning.Text;
                ToBeginning.Visible = true;
            }
            finally
            {
                myCon.Close();
            }

        }
        // invalid operation exception needed here


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