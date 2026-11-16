using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordNetProgram
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void btAddThaiToWordNet_Click(object sender, EventArgs e)
        {
            var addThaiForm = new Add_Thai_To_WordNet_18_Sep_2026();
            addThaiForm.ShowDialog();
        }

        private void btConvertToJson_Click(object sender, EventArgs e)
        {
            var convertForm = new ConvertDatatoJSON();
            convertForm.ShowDialog();
        }

        private void btWatchGraph_Click(object sender, EventArgs e)
        {
            var watchGraphForm = new WatchGraphMenu();
            watchGraphForm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btSearchData_Click(object sender, EventArgs e)
        {
            var searchDataForm = new SearchData();
            searchDataForm.ShowDialog();
        }
    }
}
