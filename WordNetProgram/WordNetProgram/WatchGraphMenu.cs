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
    public partial class WatchGraphMenu : Form
    {
        public WatchGraphMenu()
        {
            InitializeComponent();
        }

        private void btNounParentChild_Click(object sender, EventArgs e)
        {
            // var nounParentChildForm = new _171026_Parent_Child_Of_Noun(); // old code
            var nounParentChildForm = new _111126_Parent_Child_Of_Noun();
            nounParentChildForm.ShowDialog();
        }

        private void btNounHolonym_Meronym_Click(object sender, EventArgs e)
        {
            // var nounHolonymMeronymForm = new _171026_Holonym_Meronym_Of_Noun(); old
            var nounHolonymMeronymForm = new _141126_Part_Holonym_Part_Meronym_of_Noun();
            nounHolonymMeronymForm.ShowDialog();
        }

        private void btVerbParentChild_Click(object sender, EventArgs e)
        {
            // var verbParentChildForm = new _171026_Parent_Child_Of_Verb();
            var verbParentChildForm = new _131126_Parent_Child_Of_Verb();
            verbParentChildForm.ShowDialog();
        }
    }
}

