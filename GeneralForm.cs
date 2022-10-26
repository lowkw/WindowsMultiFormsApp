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
        public static Dictionary<int, string> masterFile = new Dictionary<int, string>();
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
        private void textBoxName_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxNewName.Clear();
        }
        private void textBoxID_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxNewName.Clear();
        }
        private void textBoxNewName_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxName.Clear();
            textBoxID.Clear();
            listBoxNameID.Items.Clear();
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
        private void listBoxNameID_SelectedIndexChanged(object sender, EventArgs e)
        {            
            string[] subs= listBoxNameID.Text.ToString().Split('\t');
            textBoxName.Text = subs[1];
            textBoxID.Text = subs[0];
            textBoxNewName.Clear();
            listBoxNameID.SelectedIndex = 0;
        }                
        private void GeneralForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.N)
                ClearNameTextbox();
            if (e.Alt && e.KeyCode == Keys.I)
                ClearIDTextbox();
            if (e.Alt && e.KeyCode == Keys.A)
                OpenAdminForm();
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
            if ((String.IsNullOrEmpty(textBoxName.Text) || String.IsNullOrEmpty(textBoxID.Text) ||
                    listBoxNameID.SelectedIndex == -1) && String.IsNullOrEmpty(textBoxNewName.Text))
            {
                MessageBox.Show("Please select a current staff record or enter the new staff's name.");
            }
            else if (!String.IsNullOrEmpty(textBoxNewName.Text) && textBoxNewName.Text.ToString().Any(c => char.IsDigit(c)))
            {
                MessageBox.Show("Please enter the alphabet letters only to new staff name.");
                textBoxNewName.Focus();
            }
            else
            {
                listBoxNameID.Items.Clear();
                using (Form adminForm = new AdminForm(textBoxName.Text, textBoxID.Text, textBoxNewName.Text))
                    adminForm.ShowDialog(this);
            }
        }
    }
}
