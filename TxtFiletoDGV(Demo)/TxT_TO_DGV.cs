using System.Data;

namespace TxtFiletoDGV
{
    public partial class TxT_TO_DGV : Form
    {
        public TxT_TO_DGV()
        {
            InitializeComponent();
        }

        DataTable table = new DataTable(); // สร้าง obj เป็นตารางขึ้นมา
        private void Form1_Load(object sender, EventArgs e)
        {
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("First Name", typeof(string));
            table.Columns.Add("Last Name", typeof(string));
            table.Columns.Add("Age", typeof(int));

            dataGridView1.DataSource = table; // กำหนดให้ property DataSource ของ dataGridView1 เอามาจาก table
        }

        private void button_Import_Click(object sender, EventArgs e) // system.IO กับ System.Windows.Form มีส่วนร่วมเป็นอย่างมากกับการอ่านข้อมูล txt file
        {
            string[] lines = File.ReadAllLines(@"C:\Users\phumi\OneDrive\Desktop\Sem1\Project\Sem2\ReadWordNetFile\table.txt");
            string[] values;
            for (int i = 0; i < lines.Length; i++)
            {
                values = lines[i].ToString().Split('/'); // original data in a row: / fn1   / ln1   /17 -> after this line execute: {"fn1", "ln1", "17"}
                string[] row = new string[values.Length]; // สร้าง Array เพื่อเก็บค่าของ row ๆ นึง

                for (int j = 0; j < values.Length; j++)
                {
                    row[j] = values[j].Trim();
                }
                table.Rows.Add(row);
            }
        }
    }
}
