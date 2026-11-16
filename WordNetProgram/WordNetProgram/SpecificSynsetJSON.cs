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
    public partial class SpecificSynsetJSON : Form
    {
        public SpecificSynsetJSON(string jsonContent)
        {
            InitializeComponent();

            // Optional: Disable WordWrap so the JSON formatting isn't ruined by narrow windows
            rtbJSON.WordWrap = false;

            // Display the data
            rtbJSON.Text = jsonContent;
        }
    }
}
