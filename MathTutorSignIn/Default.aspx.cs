using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MathTutorSignIn
{
    public partial class _Default : Page
    {
        static String TUTOR_PASSWORD = "Math42";
        static String FACULTY_PASSWORD = "Math42";


        protected void Page_Load(object sender, EventArgs e)
        {
            SubmitButton.Click += new EventHandler(this.CheckTutorPassword_OnClick);
            ToQueryPage.Click += new EventHandler(this.CheckFacultyPassword_OnClick);
        }

        /*
         * If the Tutor button is clicked, the password is checked. If correct, the user
         * if directed to the home sign in page. If incorrect, or if the week number is not selected,
         * they cannot continue.
         */
        private void CheckTutorPassword_OnClick(Object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            string weekNum = ddlWeekNum.SelectedItem.ToString();
            if (weekNum.Equals("") || weekNum.Equals("-"))
            {
                invalidWeekEntry.Visible = true;
                invalidPasswordMessage.Visible = false;
                return;
            }
            else
            {
                invalidWeekEntry.Visible = false;
            }

            string input = TutorPassword.Text.ToString();
            if (input.Equals(TUTOR_PASSWORD))
            {
                Application["Week"] = weekNum;
                Server.Transfer("HomeSignIn.aspx");
            }
            else
            {
                invalidPasswordMessage.Visible = true;            
            }
        }

        /*
         * If the Faculty button is clicked, the password is checked. If correct, the user
         * if directed to the faculty page. If incorrect, they cannot continue.
         */
        private void CheckFacultyPassword_OnClick(Object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            string input = FacultyPassword.Text.ToString();
            if (input.Equals(FACULTY_PASSWORD))
            {
                Response.Redirect("Queries.aspx");
            }
            else
            {
                invalidPassword2.Visible = true;
            }
        }
    }
}