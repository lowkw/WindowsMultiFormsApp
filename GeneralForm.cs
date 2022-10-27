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
        bool listBoxNameIDSelected = false;
        /*
         * 4.1.	Create a Dictionary data structure with a TKey of type integer and 
         *      a TValue of type string, name the new structure “MasterFile”.
         */
        public static Dictionary<int, string> masterFile = new Dictionary<int, string>();
        //public static SortedDictionary<int, string> masterFile = new SortedDictionary<int, string>();
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
                string[] parts;
                /*                
                string[] staffInfo = File.ReadAllLines(@"../../Resources/MalinStaffNamesV2.csv");
                string line;
                for (int x = 0; x < staffInfo.Length; x++)
                {                    
                    line = staffInfo[x];
                    parts = line.Split(',');
                    masterFile.Add(int.Parse(parts[0]), parts[1]);
                }
                */                
                foreach (string line in File.ReadLines(@"../../Resources/MalinStaffNamesV2.csv"))
                {
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
         * 4.4.	Create a method to filter the Staff Name data from the Dictionary into
         *      a second filtered and selectable listbox. This method must use a textbox input 
         *      and update as each character is entered. The listbox must reflect the filtered 
         *      data in real time.
         */
        private void textBoxName_TextChanged(object sender, EventArgs e)
        {            
            listBoxNameID.Items.Clear();
            if (!String.IsNullOrEmpty(textBoxName.Text))
            {
                foreach (KeyValuePair<int, string> kvp in masterFile)
                {
                    if (kvp.Value.Contains(textBoxName.Text))
                    {
                        listBoxNameID.Items.Add(kvp.Key + "\t" + kvp.Value);
                    }
                }
            }
        }
        /*
         * 4.5.	Create a method to filter the Staff ID data from the Dictionary into 
         *      the second filtered and selectable list box. This method must use a 
         *      textbox input and update as each number is entered. The listbox must 
         *      reflect the filtered data in real time.
         */
        private void textBoxID_TextChanged(object sender, EventArgs e)
        {
            
            listBoxNameID.Items.Clear();
            if (!String.IsNullOrEmpty(textBoxID.Text))
            {
                foreach (KeyValuePair<int, string> kvp in masterFile)
                {
                    if (kvp.Key.ToString().Contains(textBoxID.Text))
                    {
                        listBoxNameID.Items.Add(kvp.Key + "\t" + kvp.Value);
                    }
                }
            }
        }
        /*
         * 4.8.	Create a method for the filtered and selectable listbox which will 
         *      populate the two textboxes when a staff record is selected.
         */
        private void listBoxNameID_KeyDown(object sender, KeyEventArgs e)
        {
            listBoxNameIDSelected = false;
            if (listBoxNameID.Items.Count > 0 && e.KeyCode == Keys.Enter && listBoxNameID.SelectedIndex != -1)
            {
                string[] subs = listBoxNameID.SelectedItem.ToString().Split('\t');
                textBoxName.Text = subs[1];
                textBoxID.Text = subs[0];
                listBoxNameIDSelected = true;
                listBoxNameID.SelectedIndex = 0;
                textBoxName.Focus();
            }
        }
        private void GeneralForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.N)
                ClearNameTextbox();
            if (e.Alt && e.KeyCode == Keys.I)
                ClearIDTextbox();
            if (e.Alt && e.KeyCode == Keys.A)
                OpenAdminForm();
            if (e.Alt && e.KeyCode == Keys.L)
                this.Close();
        }
        /*
         * 4.6.	Create a method for the Staff Name textbox which will clear the contents 
         *      and place the focus into the Staff Name textbox. Utilise a keyboard shortcut.
         */
        private void ClearNameTextbox()
        {
            textBoxName.Clear();
            textBoxName.Focus();            
        }
        /*
         * 4.7. Create a method for the Staff ID textbox which will clear the contents 
         *      and place the focus into the textbox.Utilise a keyboard shortcut.
         */
        private void ClearIDTextbox()
        {
            textBoxID.Clear();
            textBoxID.Focus();
        }

        /*
         * 4.9.	Create a method that will open the Admin Form when the Alt + A keys 
         *      are pressed. Ensure the General Form sends the currently selected Staff ID 
         *      and Staff Name to the Admin Form for Update and Delete purposes and 
         *      is opened as modal. Create modified logic to open the Admin Form to 
         *      Create a new user when a new Staff Name is entered into the textbox.
         */
        private void OpenAdminForm()
        {
            if (String.IsNullOrEmpty(textBoxName.Text) && 
                (!listBoxNameIDSelected || listBoxNameID.SelectedIndex==-1))
            {
                MessageBox.Show("Please select a current staff record or enter the new staff's name.");
            }
            else if (!String.IsNullOrEmpty(textBoxName.Text) && textBoxName.Text.ToString().Any(c => char.IsDigit(c)))
            {
                MessageBox.Show("Please enter the alphabet letters only to staff name.");
                textBoxName.Focus();
            }
            else
            {
                if (!listBoxNameIDSelected || listBoxNameID.SelectedIndex == -1)
                {
                    // to prevent update and delete operations
                    textBoxID.Clear();
                }
                listBoxNameID.Items.Clear();
                using (Form adminForm = new AdminForm(textBoxName.Text, textBoxID.Text))
                    adminForm.ShowDialog(this);
                textBoxID.Text = AdminForm.staffID.ToString();
                textBoxName.Clear();
                listBoxNameID.Items.Clear();
                //show staff record that has been created or updated
                if (AdminForm.staffID > 0)
                {
                    listBoxNameID.Items.Add(textBoxID.Text + "\t" + masterFile[AdminForm.staffID]);                    
                }
                else
                    textBoxID.Clear();
                textBoxID.Focus();
                //load csv file that has been changed
                if (AdminForm.staffID != -1)
                    ReadFromUpdatedFile();
            }
        }
        private void ReadFromUpdatedFile()
        {
            if (File.Exists(@"../../Resources/MalinStaffNamesV2.csv"))
            {
                listBoxAllStaffs.Items.Clear();
                string[] parts;
                /*                
                string[] staffInfo = File.ReadAllLines(@"../../Resources/MalinStaffNamesV2.csv");
                string line;                                
                for (int x = 0; x < staffInfo.Length; x++)
                {
                    line = staffInfo[x];
                    parts = line.Split(',');                    
                    listBoxAllStaffs.Items.Add(parts[0] + "\t" + parts[1]);
                }
                */                
                foreach (string line in File.ReadLines(@"../../Resources/MalinStaffNamesV2.csv"))
                {
                    parts = line.Split(',');                    
                    listBoxAllStaffs.Items.Add(parts[0] + "\t" + parts[1]);
                }                
            }
            else
                MessageBox.Show("File did not load");
        }        
    }
}
