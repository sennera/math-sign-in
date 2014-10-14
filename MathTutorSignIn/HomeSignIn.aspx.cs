using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MathTutorSignIn
{
    public partial class HomeSignIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SubmitButton.Click += new EventHandler(this.CheckVNum);
        }

        /*
         * When the button is clicked, check if the V# is in the system yet. If not,
         * proceed to the next page. If so, the student is sent to a page to verify who they are. 
         */
        void CheckVNum(Object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            //Establish a database connection
            SqlConnection myCon = new SqlConnection("Persist Security Info=False;User ID=MATH-TUTOR;Password=Math42;Initial Catalog=SignIn;Data Source=MATH-TUTOR\\MATH");
                
            try
            {
                String strVNum = VNumTextBox.Text.ToString();
                int VNum = Convert.ToInt32(strVNum);

                //the V# must be 8 numbers 
                if (strVNum.Length != 8)
                {
                    throw new FormatException();
                }

                Application["VNum"] = strVNum;

                myCon.Open();

                //Get the list of Vnumbers using a query
                SqlCommand command = new SqlCommand("SELECT Vnum FROM SignIn$", myCon);
                SqlDataReader reader = command.ExecuteReader();

                //Checks each vnumber and goes to the appropriate page
                while (reader.Read())
                {
                    string vNumber = reader.GetString(0);
                    if (vNumber.Equals( strVNum ))
                    {
                        reader.Close();
                        myCon.Close(); 
                        Response.Redirect("InSystem.aspx");
                    }
                }
                reader.Close();
                myCon.Close();
                Server.Transfer("ClassLevel.aspx");
            }
            catch (SqlException sqlEx)
            {
                failedSQLMessage.Visible = true;
            }
            catch (FormatException ex)
            {
                invalidInputMessage.Visible = true;
            }
            catch (OverflowException ex2)
            {
                invalidInputMessage.Visible = true;
            }
            catch (Exception e1)
            {
                errorMessage.Visible = true;
            }
            finally
            {
                myCon.Close();
            }
        }
    }
}