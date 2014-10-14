using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MathTutorSignIn
{
    public partial class Queries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetWholeSignInButton.Click += new EventHandler(this.GetSignInSheet);
            GetWeeklySignInButton.Click += new EventHandler(this.GetOneWeekSheet);
            SortByClassLevel.Click += new EventHandler(this.DoSortByClassLevel);
            SortByCRN_NumTimesPerStudent.Click += new EventHandler(this.DoSortByCRN_NumTimesPerStudent);
            SortByCRN_NumTimesPerClass.Click += new EventHandler(this.DoSortByCRN_NumTimesPerClass);
            SortByProfessor.Click += new EventHandler(this.DoSortByProfessor);
            SortByProfessor_Totals.Click += new EventHandler(this.DoSortByProfessor_Totals);
            SortByLevel.Click += new EventHandler(this.DoSortByLevel);
            SortByStudent.Click += new EventHandler(this.DoSortByStudent);
            PerDay.Click += new EventHandler(this.DoPerDay);
            PerWeek.Click += new EventHandler(this.DoPerWeek);
            PerHour.Click += new EventHandler(this.DoPerHour);
            AvgPerDay.Click += new EventHandler(this.DoAvgPerDay);
            AvgPerWeek.Click += new EventHandler(this.DoAvgPerWeek);
            NonMathClasses.Click += new EventHandler(this.DoNonMathClasses);
            AllClassesQuery.Click += new EventHandler(this.DoAllClassesQuery);
            InsertNewInfoButton.Click += new EventHandler(this.ToNewInfoPage);
            ResetButton.Click += new EventHandler(this.showCaution);
            YesButton.Click += new EventHandler(this.clear);
            NoButton.Click += new EventHandler(this.clickedNo);
        }

        //Shows the entire sign in sheet
        void GetSignInSheet(Object sender, EventArgs e)
        {
            String sql = @"SELECT SignInNum, Week, Day, Hour, Minute, SignIn$.Vnum, LastName, FirstName,
                Class$.CRN, ClassLevel, Professor, Days, StartTime, EndTime
                FROM [dbo].[SignIn$]
                JOIN [dbo].[Class$] ON Class$.CRN = SignIn$.CRN
                JOIN [dbo].[Student$] ON Student$.Vnum = SignIn$.Vnum;";
            RunQuery(sql);
        }

        //Shows the entire sign in sheet for one week
        void GetOneWeekSheet(Object sender, EventArgs e)
        {
            String week = WeekNumDropDownList.SelectedItem.ToString();
            if(!week.Equals("-"))
            {
                String sql = @"SELECT SignInNum, Week, Day, Hour, Minute, SignIn$.Vnum, LastName, FirstName,
                Class$.CRN, ClassLevel, Professor, Days, StartTime, EndTime
                FROM [dbo].[SignIn$]
                JOIN [dbo].[Class$] ON Class$.CRN = SignIn$.CRN
                JOIN [dbo].[Student$] ON Student$.Vnum = SignIn$.Vnum
                WHERE Week = " + week + ";";
                RunQuery(sql); 
            }
        }

        // Sorts the Sign-in page by CRN
        void DoSortByClassLevel(Object sender, EventArgs e)
        {
            String sql = @"SELECT Class$.CRN, ClassLevel, Professor, Days, StartTime, EndTime, 
				Week, Day, Hour, Minute, SignIn$.Vnum, LastName, FirstName
                FROM [dbo].[SignIn$]
                JOIN [dbo].[Class$] ON Class$.CRN = SignIn$.CRN
                JOIN [dbo].[Student$] ON Student$.Vnum = SignIn$.Vnum
				ORDER BY Class$.ClassLevel;";
            RunQuery(sql);
        }
        
        // Sorts the Sign-in page by CRN, giving all information except that each student 
        // will only be shown once, along with how many times they came in 
        void DoSortByCRN_NumTimesPerStudent(Object sender, EventArgs e)
        {
            String sql = @"SELECT Class$.CRN, Class$.ClassLevel, Class$.Professor, Class$.Days, Class$.StartTime, Class$.EndTime,
				SignIn$.Vnum, Student$.LastName, Student$.FirstName, COUNT(SignIn$.Vnum) AS TimesCameIn
                FROM SignIn$ 
				JOIN Class$ ON SignIn$.CRN = Class$.CRN 
				JOIN Student$ ON SignIn$.VNum = Student$.VNum
                GROUP BY Class$.CRN, Class$.Professor, Class$.ClassLevel, Class$.Days, Class$.StartTime, Class$.EndTime, 
					SignIn$.Vnum, Student$.FirstName, Student$.LastName
                ORDER BY Class$.CRN;";
            RunQuery(sql);
        }

        // Sorts the Sign-in page by CRN. For each class, it counts up how many visits have been made total, how many 
        // students have come, and the average number of visits per student 
        void DoSortByCRN_NumTimesPerClass(Object sender, EventArgs e)
        {
            String sql = @"SELECT CRN, ClassLevel, Professor, Days, StartTime, EndTime, 
	                TotalVisits, TotalStudents, (TotalVisits / TotalStudents) AS AverageVisitsPerStudent
                FROM (
	                SELECT Class$.CRN, Class$.ClassLevel, Class$.Professor, Class$.Days, Class$.StartTime, Class$.EndTime, 
	                COUNT(*) AS TotalVisits, COUNT(DISTINCT Student$.Vnum) AS TotalStudents
                    FROM SignIn$ 
				    JOIN Class$ ON SignIn$.CRN = Class$.CRN 
				    JOIN Student$ ON SignIn$.VNum = Student$.VNum
                    GROUP BY Class$.CRN, Class$.Professor, Class$.ClassLevel, 
                        Class$.Days, Class$.StartTime, Class$.EndTime) AS Totals
				ORDER BY CRN;";
            RunQuery(sql);
        }

        // Sorts the Sign-in page by professor and counts up the number of times each student came in 
        void DoSortByProfessor(Object sender, EventArgs e)
        {
            String sql = @"SELECT Class$.CRN, Class$.ClassLevel, Class$.Professor, Class$.Days, Class$.StartTime, Class$.EndTime,
				SignIn$.Vnum, Student$.LastName, Student$.FirstName, COUNT(SignIn$.Vnum) AS TimesCameIn
                FROM SignIn$ 
				JOIN Class$ ON SignIn$.CRN = Class$.CRN 
				JOIN Student$ ON SignIn$.VNum = Student$.VNum
                GROUP BY Class$.CRN, Class$.Professor, Class$.ClassLevel, Class$.Days, Class$.StartTime, Class$.EndTime, 
					SignIn$.Vnum, Student$.FirstName, Student$.LastName
                ORDER BY Class$.Professor, Class$.ClassLevel;";
            RunQuery(sql);
        }

        // Sorts the Sign-in page by professor and counts up the number of times each student came in 
        void DoSortByProfessor_Totals(Object sender, EventArgs e)
        {
            String sql = @"SELECT CRN, ClassLevel, Professor, Days, StartTime, EndTime, 
	                TotalVisits, TotalStudents, (TotalVisits / TotalStudents) AS AverageVisitsPerStudent
                FROM (
	                SELECT Class$.CRN, Class$.ClassLevel, Class$.Professor, Class$.Days, Class$.StartTime, Class$.EndTime, 
	                COUNT(*) AS TotalVisits, COUNT(DISTINCT Student$.Vnum) AS TotalStudents
                    FROM SignIn$ 
				    JOIN Class$ ON SignIn$.CRN = Class$.CRN 
				    JOIN Student$ ON SignIn$.VNum = Student$.VNum
                    GROUP BY Class$.CRN, Class$.Professor, Class$.ClassLevel, 
                        Class$.Days, Class$.StartTime, Class$.EndTime) AS Totals
				ORDER BY Professor, CRN;";
            RunQuery(sql);
        }
        
        // Sorts the Sign-in page by class level
        void DoSortByLevel(Object sender, EventArgs e)
        {
            String sql = @"SELECT CLASS$.ClassLevel, COUNT(*) AS NumStudents 
                FROM SignIn$ JOIN Class$ ON SignIn$.CRN = Class$.CRN 
                GROUP BY CLASS$.ClassLevel ORDER BY CLASS$.ClassLevel;";
            RunQuery(sql);
        }   

        /** 
         * Sorts the sign in sheet by Student (in order alphabetically by last name).
         * Fields listed: V#, First Name, Last Name, SignIn#, Week, Date, Time, CRN
         **/
        void DoSortByStudent(Object sender, EventArgs e)
        {
            String sql = @"SELECT SignIn$.Vnum, Student$.LastName, Student$.FirstName,  
	                        SignIn$.Week, SignIn$.Day, SignIn$.Hour, SignIn$.Minute, SignIn$.CRN
                        FROM SignIn$ JOIN Student$ ON SignIn$.VNum = Student$.VNum
                        GROUP BY Student$.LastName, SignIn$.Vnum, Student$.FirstName, SignIn$.CRN, SignIn$.Week, 
	                        SignIn$.Day, SignIn$.Hour, SignIn$.Minute
                        ORDER BY Student$.LastName, Student$.FirstName, SignIn$.Vnum, SignIn$.CRN;";
            RunQuery(sql);
        }

        void DoPerDay(Object sender, EventArgs e)
        {
            String sql = @"SELECT Day, COUNT(Distinct VNum) AS NumStudents 
                FROM SignIn$ GROUP BY Day ORDER BY COUNT(Distinct VNum) DESC;";
            RunQuery(sql);
        }
        
        void DoPerWeek(Object sender, EventArgs e)
        {
            String sql = @"SELECT Week, COUNT(*) AS 'Number of Students'
            FROM SignIn$
            GROUP BY Week
            ORDER BY Week;";
            RunQuery(sql);
        }

        void DoPerHour(Object sender, EventArgs e)
        {
            String sql = @"SELECT Hour, COUNT(*) As 'Number of Students'
            FROM SignIn$ 
            GROUP BY Hour
            ORDER BY COUNT(*) DESC;";
            RunQuery(sql);
        }
        

        void DoAvgPerDay(Object sender, EventArgs e)
        {
            String sql = @"SELECT COUNT(*) / COUNT(DISTINCT(Day)) As 'Daily Average Students'
            FROM SignIn$;";
            RunQuery(sql);

        }

        void DoAvgPerWeek(Object sender, EventArgs e)
        {
            String sql = @"SELECT COUNT(*) / COUNT(DISTINCT(Week)) As 'Weekly Average Students'
            FROM SignIn$;";
            RunQuery(sql);
        }

        // Gets the number of students who came in for a non-math class or another reason
        void DoNonMathClasses(Object sender, EventArgs e)
        {
            String sql = @"SELECT COUNT(*) As 'Number of Students'
            FROM SignIn$ 
            WHERE CRN = 'Other';";
            RunQuery(sql);
        }

        // Gets all information for the classes entered into the database
        void DoAllClassesQuery(Object sender, EventArgs e)
        {
            String sql = @"SELECT * FROM [dbo].[Class$] ORDER BY Class$.ClassLevel ASC;";
            RunQuery(sql);
        }

        //Run a query and show the output in the output table.
        void RunQuery(String sqlStatement)
        {
            OutputTable.Rows.Clear();
            ErrorSuccess.Text = "Loading results...";
            ErrorSuccess.Visible = true;

            string connection = "Persist Security Info=False;User ID=MATH-TUTOR;Password=Math42;Initial Catalog=SignIn;Data Source=MATH-TUTOR\\MATH";
            try
            {
                SqlConnection sqlCon = new SqlConnection(connection);
                sqlCon.Open();

                SqlCommand command = new SqlCommand(sqlStatement, sqlCon);
                SqlDataReader reader = command.ExecuteReader();

                TableRow nameRow = new TableRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string colName = reader.GetName(i);
                    nameRow.Cells.Add(makeCell(colName));
                }
                OutputTable.Rows.Add(nameRow);

                while (reader.Read())
                {
                    TableRow newRow = new TableRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        TableCell newCell = new TableCell();

                        try
                        {
                            newCell.Controls.Add(new LiteralControl(Convert.ToString(reader.GetDouble(i))));
                        }
                        catch (InvalidCastException exc)
                        {
                            try
                            {
                                newCell.Controls.Add(new LiteralControl(Convert.ToString(reader.GetInt32(i))));
                            }
                            catch (InvalidCastException e)
                            {
                                newCell.Controls.Add(new LiteralControl(reader.GetString(i)));
                            }

                        }
                        newRow.Cells.Add(newCell);
                    }
                    OutputTable.Rows.Add(newRow);
                }
                reader.Close();
                sqlCon.Close();
                OutputTable.Visible = true;
                ErrorSuccess.Text = "Your results were successfully found!";
            }
            catch (Exception e)
            {
                ErrorSuccess.Text = "There was an error running the query. Check the database and the connection.";
            }

        }

        // Makes a cell with the given text
        TableCell makeCell(string name)
        {
            TableCell newCell = new TableCell();
            newCell.Text = name;
            return newCell;
        }

        void ToNewInfoPage(Object sender, EventArgs e)
        {
            Server.Transfer("NewTermEntry.aspx");
        }

        //Shows the warning message and buttons
        void showCaution(Object sender, EventArgs e)
        {
            WarningLabel.Visible = true;
            YesButton.Visible = true;
            NoButton.Visible = true;
        }

        //Clears all the information from tables
        void clear(Object sender, EventArgs e)
        {
            clearTable("Class$");
            clearTable("Student$");
            clearTable("SignIn$");
            hideCaution();
        }

        //Clears the table that is input
        void clearTable(String tableName) 
        {
            string connection = "Persist Security Info=False;User ID=MATH-TUTOR;Password=Math42;Initial Catalog=SignIn;Data Source=MATH-TUTOR\\MATH";
            SqlConnection sqlCon = new SqlConnection(connection);
            String sql = "DELETE FROM " + tableName + ";";
            sqlCon.Open();
            SqlCommand command = new SqlCommand(sql, sqlCon);
            command.ExecuteNonQuery();
            sqlCon.Close();
        }

        //Hides the caution button and message for deleting of the tables
        void hideCaution()
        {
            WarningLabel.Visible = false;
            YesButton.Visible = false;
            NoButton.Visible = false;
        }

        void clickedNo(Object sender, EventArgs e)
        {
            hideCaution();
        }
    }
}