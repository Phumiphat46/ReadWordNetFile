using System.Data;

namespace ReadWordNetFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataTable table = new DataTable(); // create table obj
        private void buttonFilesPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            DialogResult result = folder.ShowDialog(); // โชว์หน้า file explorer ขึ้นมา
            if (result == DialogResult.OK)
            {
                filesPathTextBox.Text = folder.SelectedPath; // ให้ text box แสดง Path
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            // เคลียร์ column เผื่อมีของเก่าค้าง
            table.Columns.Clear();

            // ✅ ใช้แค่ 2 คอลัมน์ตามที่ต้องการ
            table.Columns.Add("synset_offset", typeof(string));  // col 1
            table.Columns.Add("details", typeof(string));        // col 2 = lex_filenum ... gloss

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = table; // กำหนดให้ DataGridView ใช้ DataTable นี้
        }

        private void buttonNounPath_Click(object sender, EventArgs e)
        {
            //OpenFileDialog fileDialog = new OpenFileDialog(); // สร้าง obj ตัว browse file ขึ้นมา
            //                                                  // fileDialog.Multiselect = true; // กำหนดให้ Property Multiselect ของ fileDialog เป็น true (user สามารถเลือกหลาย ๆ ไฟล์พร้อมกันได้)
            //fileDialog.Filter = "Noun files| data.noun";
            //DialogResult result = fileDialog.ShowDialog();
            //if (result == DialogResult.OK)
            //{

            //    string path = fileDialog.FileName;
            //    string fileName = fileDialog.SafeFileName;
            //    nounPathTextBox.Text = fileName;
            //}

            string path = @"C:\Users\phumi\OneDrive\Desktop\Sem1\Project\Sem2\ReadWordNetFile\data.noun";

            if (!File.Exists(path))
            {
                MessageBox.Show("File not found: " + path);
                return;
            }

            // ล้างข้อมูลเก่าใน table แต่ไม่ลบ columns
            table.Rows.Clear();

            string[] lines = File.ReadAllLines(path);

            // ✅ ข้ามบรรทัดที่ 1-29 (index 0-28)
            int startIndex = 29; // บรรทัดที่ 30 (0-based index)

            int maxRows = 10;    // ต้องการแค่ 10 บรรทัดแรกของ "ข้อมูลจริง"
            int addedCount = 0;

            for (int i = startIndex; i < lines.Length && addedCount < maxRows; i++)
            {
                string line = lines[i];

                // กันกรณีบรรทัดว่างหรือมีแต่ space
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // ตัด space ด้านหน้า (บางไฟล์อาจมี)
                string trimmed = line.TrimStart();

                // หา space แรก เพื่อตัด synset_offset ออกมา
                int firstSpaceIndex = trimmed.IndexOf(' ');
                if (firstSpaceIndex == -1)
                {
                    // รูปแบบไม่ถูกต้อง ข้ามไป
                    continue;
                }

                // col 1: synset_offset
                string synsetOffset = trimmed.Substring(0, firstSpaceIndex);

                // col 2: ส่วนที่เหลือทั้งหมด (lex_filenum ... gloss)
                string rest = trimmed.Substring(firstSpaceIndex + 1).Trim();

                // ❗ ตรงนี้สำคัญ: เพิ่ม row โดยส่งค่าให้ตรงกับจำนวน column = 2 ค่าเท่านั้น
                table.Rows.Add(synsetOffset, rest);

                addedCount++;
            }

            MessageBox.Show($"Imported {addedCount} rows from data.noun (skipped first 29 lines).");
        }
    }
}


