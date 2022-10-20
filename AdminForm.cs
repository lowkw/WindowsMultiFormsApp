using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsMultiFormsApp
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.C)
            {
                toolStripStatusLabel1.Text = "Staff record created";
            }
            if (e.Alt && e.KeyCode == Keys.U)
            {
                toolStripStatusLabel1.Text = "Staff name updated";
            }
            if (e.Alt && e.KeyCode == Keys.D)
            {
                toolStripStatusLabel1.Text = "Staff record deleted";
            }
            /*
             * 5.7.	Create a method that will close the Admin Form when 
             *      the Alt + L keys are pressed
             */
            if (e.Alt && e.KeyCode == Keys.L)
            {
                this.Dispose();
            }
        }
    }
}
