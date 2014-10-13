using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MathTutorSignIn
{
    public partial class NewTermEntry : System.Web.UI.Page
    {

        // The column names of the values to be entered into the table.
        protected static String[] COLUMN_NAMES = new String[] { "CRN", "Class Level", "Start Time", "End Time", "Days", "Instructor" };
        protected static int NUMBER_OF_FIELDS = 6;
        protected static List<String[]> INPUT_DATA = new List<String[]>();
        protected static int CRN = 0;
        protected static int CLASS_LEVEL = 1;
        protected static int START_TIME = 2;
        protected static int END_TIME = 3;
        protected static int DAYS = 4;
        protected static int INSTRUCTOR = 5;

        protected void Page_Load(object sender, EventArgs e)
        {
            DoneButton1.Click += new EventHandler(this.LeavePage);
            DoneButton2.Click += new EventHandler(this.LeavePage);
            SaveButton.Click += new EventHandler(this.SaveData_OnClick);
            FinalSaveButton.Click += new EventHandler(this.FinalSaveData_OnClick);
            DeleteButton.Click += new EventHandler(this.DeleteData_OnClick);
            ClearFieldsButton.Click += new EventHandler(this.ClearFields_OnClick);
        }

        //Goes back to the query page
        void LeavePage(Object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Server.Transfer("Queries.aspx");
        }

        // Makes a cell with the given text
        TableCell makeCell(string name)
        {
            TableCell newCell = new TableCell();
            newCell.Text = name;
            return newCell;
        }

        // Puts the data in the text box into a preview table to ask the user if that's what they want to do.
        // The data must be tab-delimited and must have the correct columns.
        void SaveData_OnClick(Object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            NumColsWarning.Visible = false;

            PreviewTable.Rows.Clear();

            // Make the row that contains the column names
            TableRow columnNameRow = new TableRow();
            for (int i = 0; i < COLUMN_NAMES.Length; i++)
            {
                columnNameRow.Cells.Add(makeCell(COLUMN_NAMES[i]));
            }
            PreviewTable.Rows.Add(columnNameRow);

            string[] newLineDelim = new String[] { "\n" };
            string[] classes = InputData.Text.Split(newLineDelim, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (String classRow in classes)
            {
                string[] tabDelim = new String[] { "\t" };
                string[] fields = classRow.Split(tabDelim, System.StringSplitOptions.RemoveEmptyEntries);

                // if the wrong number of fields is provided, do not continue with this row
                if (fields.Length != NUMBER_OF_FIELDS && fields.Length != 0)
                {
                    NumColsWarning.Visible = true; 
                    break;
                }
                // if the row starts with "CRN" rather than a value, the input was wrong -- skip this row
                else if (!fields[0].Equals("CRN")) 
                {
                    TableRow fieldsRow = new TableRow();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        fieldsRow.Cells.Add(makeCell(fields[i]));
                    }
                    PreviewTable.Rows.Add(fieldsRow);
                    INPUT_DATA.Add(fields);
                }
            }

            //at the end, only show these items if there is data to show (the first row is the columns row)
            if (PreviewTable.Rows.Count > 1)
            {
                PreviewTable.Visible = true;
                FinalSaveButton.Visible = true;
                ClearFieldsButton.Visible = true;
                PreviewLabel.Visible = true;
                SaveButton.Visible = false;
            }
            // otherwise make sure they're still hidden
            else
            {
                resetVisibility();
            }
        }

        //Saves the data in the text box when the user confirms that the data looks right
        void FinalSaveData_OnClick(Object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            bool success = true;

            string connection = "Persist Security Info=False;User ID=MATH-TUTOR;Password=Math42;Initial Catalog=SignIn;Data Source=MATH-TUTOR\\MATH";
            SqlConnection sqlCon = new SqlConnection(connection);

            try
            {
                sqlCon.Open();

                for (int i = 0; i < INPUT_DATA.Count; i++ )
                {
                    string crn = INPUT_DATA[i][CRN].Trim();
                    System.Diagnostics.Debug.WriteLine("count " + INPUT_DATA.Count);
                    System.Diagnostics.Debug.WriteLine(crn);
                    string classLevel = INPUT_DATA[i][CLASS_LEVEL].Trim();
                    string startTime = INPUT_DATA[i][START_TIME].Trim();
                    string endTime = INPUT_DATA[i][END_TIME].Trim();
                    string days = INPUT_DATA[i][DAYS].Trim();
                    string instructor = INPUT_DATA[i][INSTRUCTOR].Trim();

                    try
                    {
                        if (crn.Equals("") || classLevel.Equals("") || instructor.Equals("") || days.Equals("") || startTime.Equals("") || endTime.Equals(""))
                        {
                            throw new FormatException("None of the fields may be empty. Please re-enter the data.");
                        }
                        Convert.ToInt32(crn);
                        if (crn.Length != 5)
                        {
                            throw new FormatException("The CRN must be of length 5. Please re-enter the data.");
                        }
                        if (classLevel.Length > 3)
                        {
                            throw new FormatException("The class level may not be greater than length 3. Please re-enter the data.");
                        }
                        Convert.ToInt32(classLevel);
                        if (!(startTime.Length == 4 || startTime.Length == 3))
                        {
                            throw new FormatException("Please enter a valid time into the start time.");
                        }
                        Convert.ToInt32(startTime);
                        if (!(endTime.Length == 4 || endTime.Length == 3))
                        {
                            throw new FormatException("Please enter a valid time into the end time.");
                        }
                        Convert.ToInt32(endTime);
                        System.Diagnostics.Debug.WriteLine( endTime.GetType().ToString());
                        string sql = @"INSERT INTO Class$ (CRN, ClassLevel, Professor, Days, StartTime, EndTime) 
                        VALUES (@param1, @param2, @param3, @param4, @param5, @param6)";
                        SqlCommand command = new SqlCommand(sql, sqlCon);
                        command.Parameters.AddWithValue("@param1", crn);
                        command.Parameters.AddWithValue("@param2", classLevel);
                        command.Parameters.AddWithValue("@param3", instructor);
                        command.Parameters.AddWithValue("@param4", days);
                        command.Parameters.AddWithValue("@param5", startTime);
                        command.Parameters.AddWithValue("@param6", endTime);
                        command.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("success");
                    }
                    catch (FormatException ex)
                    {
                        SomeError.Text = SomeError.Text + string.Format(ex.Message + " This entry was not entered: {0}, {1}, {2}, {3}, {4}, {5}. ", crn, classLevel, startTime, endTime, days, instructor);
                        SomeError.Visible = true;
                        success = false;
                    }
                    catch (SqlException sqlExc)
                    {
                        DuplicateKeyError.Text = DuplicateKeyError.Text + string.Format(" This entry was not entered: {0}, {1}, {2}, {3}, {4}, {5}. ", crn, classLevel, startTime, endTime, days, instructor);
                        DuplicateKeyError.Visible = true;
                        success = false;
                    }
                    catch (Exception exc)
                    {
                        SomeError.Text = SomeError.Text + string.Format(" There was some error. This entry was not entered: {0}, {1}, {2}, {3}, {4}, {5}. ", crn, classLevel, startTime, endTime, days, instructor);
                        SomeError.Visible = true;
                        success = false;
                    }
                }
            }
            catch (Exception e2)
            {
                SomeError.Text = SomeError.Text + string.Format(" There was some error with the database connection.");
            }
            finally
            {
                sqlCon.Close();
            }

            if (success == true)
            {
                resetVisibility();
                emptyCells();
                SomeError.Text = "Success! All the entries have been stored.";
                SomeError.Visible = true;
            }
        }

        //Deletes the row with the input CRN in the database
        void DeleteData_OnClick(Object sender, EventArgs e)
        {
            String crn = CRNText.Text;

            try
            {
                string connection = "Persist Security Info=False;User ID=MATH-TUTOR;Password=Math42;Initial Catalog=SignIn;Data Source=MATH-TUTOR\\MATH";
                SqlConnection sqlCon = new SqlConnection(connection);
                sqlCon.Open();

                String sql = @"DELETE FROM Class$ 
                    WHERE CRN = " + crn + ";";
                SqlCommand command = new SqlCommand(sql, sqlCon);
                command.ExecuteNonQuery();
                sqlCon.Close();
                emptyCells();
                DeleteLabel.Text = "This CRN has been deleted from the database: " + crn;
                DeleteLabel.Visible = true;
            }
            catch (SqlException sqlDelEx)
            {
                DeleteLabel.Text = "There was an error with the database. This CRN may not have been deleted from the database: " + crn;
                DeleteLabel.Visible = true;
            }
            catch (Exception delEx)
            {
                DeleteLabel.Text = "There was an error. This CRN may not have been deleted from the database: " + crn;
                DeleteLabel.Visible = true;
            }

        }

        //Empties all the text boxes
        void emptyCells()
        {
            InputData.Text = "";
            INPUT_DATA.Clear();
        }

        // Reset visible buttons on page
        void resetVisibility()
        {
            PreviewTable.Visible = false;
            FinalSaveButton.Visible = false;
            ClearFieldsButton.Visible = false;
            PreviewLabel.Visible = false;
            SomeError.Visible = false;
            DuplicateKeyError.Visible = false;
            SaveButton.Visible = true;
            DeleteLabel.Visible = false;
        }

        // Clears the input text box and resets visibility of buttons and labels
        void ClearFields_OnClick(Object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            resetVisibility();
            NumColsWarning.Visible = false;
            emptyCells();
        }


    }
}