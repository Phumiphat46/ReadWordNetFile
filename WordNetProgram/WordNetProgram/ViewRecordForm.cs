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
    public partial class ViewRecordForm : Form
    {
        private RichTextBox rtbDetails;
        public ViewRecordForm(string content)
        {
            this.Text = "Synset Details";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            rtbDetails = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 11f),
                ReadOnly = true,
                Text = content,
                Padding = new Padding(10)
            };

            this.Controls.Add(rtbDetails);
        }
    }
}
