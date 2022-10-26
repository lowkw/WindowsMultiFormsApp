using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
// Low, Kok Wei (M214391)
namespace WindowsMultiFormsApp
{
    public partial class AdminForm : Form
    {
        Dictionary<int, string> masterFile;
        public AdminForm()
        {
            InitializeComponent();            
        }
        /*
         * 5.2.	Create a method that will receive the Staff ID from the general form 
         *      and then populate textboxes with the related data.
         */
        public AdminForm(string strAdminName, string strAdminID, string strAdminNewName)
        {
            InitializeComponent();
            textBoxAdminName.Text = strAdminName;
            textBoxAdminID.Text = strAdminID;
            textBoxAdminNewName.Text = strAdminNewName;
            masterFile = GeneralForm.masterFile;
        }

        private void AdminForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.C)
                CreateStaffRec();
            if (e.Alt && e.KeyCode == Keys.U)
                UpdateStaffRec();
            if (e.Alt && e.KeyCode == Keys.D)
                DeleteStaffRec();
            if (e.Alt && e.KeyCode == Keys.L)
                CloseAdminForm();            
        }
        /*
         * 5.3.	Create a method that will create a new Staff ID and input the staff name 
         *      from the related text box. The Staff ID must be unique starting with 
         *      77xxxxxxx while the staff name may be duplicated. The new staff member 
         *      must be added to the Dictionary data structure.
         */
        private void CreateStaffRec()
        {
            if (!String.IsNullOrEmpty(textBoxAdminNewName.Text))
            {
                bool addNewStaff = false;
                while (!addNewStaff)
                {
                    Random r = new Random();
                    try
                    {
                        masterFile.Add(r.Next(770000000,780000000), textBoxAdminNewName.Text);
                        addNewStaff = true;
                    }
                    catch { }
                }
                toolStripStatusLabel1.Text = "New staff record created";
            } else
                toolStripStatusLabel1.Text = "New staff record not created because staff name is blanks";
        }
        /*
         * 5.4.	Create a method that will Update the name of the current Staff ID.
         */
        private void UpdateStaffRec()
        {
            if (!String.IsNullOrEmpty(textBoxAdminID.Text)) {
                masterFile[Int32.Parse(textBoxAdminID.Text)] = textBoxAdminName.Text;
                toolStripStatusLabel1.Text = "Staff name updated"; 
            } else
                toolStripStatusLabel1.Text = "Current staff name not updated because staff ID is blanks";
        }
        /*
         * 5.5.	Create a method that will Remove the current Staff ID and clear the text boxes.
         */
        private void DeleteStaffRec()
        {
            if (!String.IsNullOrEmpty(textBoxAdminID.Text)) {
                masterFile.Remove(Int32.Parse(textBoxAdminID.Text));
                textBoxAdminID.Clear();
                textBoxAdminName.Clear();
                toolStripStatusLabel1.Text = "Staff record deleted";
            } else
                toolStripStatusLabel1.Text = "Current staff record not deleted because staff ID is blanks";
        }
        /*
         * 5.7.	Create a method that will close the Admin Form when 
         *      the Alt + L keys are pressed
         */
        private void CloseAdminForm()
        {
            WriteStaffsToFile();
            this.Dispose();            
        }
        /*
         * 5.6.	Create a method that will save changes to the csv file, this method 
         *      should be called before the Admin Form closes.
         */
        private void WriteStaffsToFile()
        {
            string filePath = @"../../Resources/MalinStaffNamesV2.csv";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                List<string> strList = new List<string>();
                foreach (KeyValuePair<int, string> kvp in masterFile)
                {
                    strList.Add(kvp.Key.ToString() + "," + kvp.Value);
                }
                File.AppendAllLines(filePath, strList);
            }
        }
    }
}
