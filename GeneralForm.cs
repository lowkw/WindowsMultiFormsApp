using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Low, Kok Wei (M214391)
namespace WindowsMultiFormsApp
{
    public partial class GeneralForm : Form
    {
        /*
         * 4.1.	Create a Dictionary data structure with a TKey of type integer and 
         *      a TValue of type string, name the new structure “MasterFile”.
         */
        Dictionary<int, string> masterFile = new Dictionary<int, string>();
        public GeneralForm()
        {
            InitializeComponent();
        }
        
        private void GeneralForm_Load(object sender, EventArgs e)
        {
            ReadStaffsFromFile();
        }
        /*
         * 4.2.	Create a method that will read the data from the .csv file into 
         *      the Dictionary data structure when the form loads.
         */
        private void ReadStaffsFromFile()
        {
            if (File.Exists(@"../../Resources/MalinStaffNamesV2.csv"))
            {
                string[] staffInfo = File.ReadAllLines(@"../../Resources/MalinStaffNamesV2.csv");
                string line;
                string[] parts;
                for (int x = 0; x < staffInfo.Length; x++)
                {                    
                    line = staffInfo[x];
                    parts = line.Split(',');
                    masterFile.Add(int.Parse(parts[0]), parts[1]);
                }
                DisplayAllStaffs();
            }
            else
                MessageBox.Show("File did not load");
        }
        /* 4.3.	Create a method to display the Dictionary data into a 
         *      non-selectable listbox (ie read only).
         */
        private void DisplayAllStaffs()
        {
            listBoxAllStaffs.Items.Clear();
            foreach (var staff in masterFile)
                listBoxAllStaffs.Items.Add(staff.Key + "\t" + staff.Value);
        }

        /*
         * 4.9.	Create a method that will open the Admin Form when the Alt + A keys 
         *      are pressed. Ensure the General Form sends the currently selected Staff ID 
         *      and Staff Name to the Admin Form for Update and Delete purposes and 
         *      is opened as modal. Create modified logic to open the Admin Form to 
         *      Create a new user when a new Staff Name is entered into the textbox.
         */
        private void GeneralForm_KeyDown(object sender, KeyEventArgs e)
        {          
            if (e.Alt && e.KeyCode == Keys.A)
            {
                using (Form adminForm = new AdminForm())
                    adminForm.ShowDialog(this);
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            listBoxName.Items.Clear();            
            
            Dictionary<int, string>.ValueCollection valueColl = masterFile.Values;
            foreach (string s in valueColl)
            {
                if (s.Contains(textBoxName.Text))
                {
                    listBoxName.Items.Add(s);
                }
            }                                    
        }
    }
}
