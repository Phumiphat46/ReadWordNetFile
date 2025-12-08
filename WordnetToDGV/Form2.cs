using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordnetToDGV
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        DataTable table = new DataTable(); // สร้าง obj เป็นตารางขึ้นมา
        private void Form2_Load(object sender, EventArgs e)
        {
            // เปลี่ยนการกำหนดคอลัมน์ให้เหลือ 2 คอลัมน์ตามต้องการ
            table.Columns.Add("Synset_Offset", typeof(string)); // Col 1: Synset_offset (เป็น String เพราะมี 0 นำหน้า)
            table.Columns.Add("Data_Body", typeof(string));     // Col 2: ข้อมูลส่วนที่เหลือทั้งหมด

            dataGridView1.DataSource = table; // กำหนดให้ property DataSource ของ dataGridView1 เอามาจาก table
            dataGridView2.DataSource = table; // กำหนดให้ property DataSource ของ dataGridView1 เอามาจาก table
            dataGridView3.DataSource = table; // กำหนดให้ property DataSource ของ dataGridView1 เอามาจาก table
            dataGridView4.DataSource = table; // กำหนดให้ property DataSource ของ dataGridView1 เอามาจาก table

            /* ---------------------------------------------------------------------------------------------------- */
            nounPathComboBox.Items.Clear();

            // 2. เพิ่มรายการทวีปที่คุณต้องการ
            nounPathComboBox.Items.Add("1 - Asia");
            nounPathComboBox.Items.Add("2 - Africa");
            nounPathComboBox.Items.Add("3 - Europe");
            nounPathComboBox.Items.Add("4 - North America");
            nounPathComboBox.Items.Add("5 - South America");
            nounPathComboBox.Items.Add("6 - Oceania"); // แก้ไขจาก "6. - Oceania" เป็น "6 - Oceania" เพื่อความสม่ำเสมอ

            // 3. กำหนดให้รายการแรกถูกเลือกโดยอัตโนมัติเมื่อฟอร์มโหลด (ไม่บังคับ)
            nounPathComboBox.SelectedIndex = 0;
        }

        private void nounPathButton_Click(object sender, EventArgs e)
        {
            table.Rows.Clear();
            // 1. อ่านไฟล์แบบสตรีม, ข้าม 29 บรรทัดแรก (ที่เป็นคำอธิบาย)
            // และจำกัดการอ่านข้อมูลจริงเพียง 10 บรรทัดถัดไปเท่านั้น
            var lines = File.ReadLines(@"C:\Users\phumi\OneDrive\Desktop\Sem1\Project\Sem2\ReadWordNetFile\data.noun")
                            .Skip(29) // ข้าม 29 บรรทัดแรกที่เป็น Header/License
                            .Take(10) // <-- เพิ่ม: จำกัดการอ่านแค่ 10 บรรทัดข้อมูลแรก
                            .ToArray(); // แปลงเป็น Array เพื่อให้วนลูปได้ง่าย

            // 2. วนลูปเพื่อประมวลผลแต่ละบรรทัดข้อมูล (Synset)
            foreach (string line in lines)
            {
                // โค้ดส่วนนี้ยังคงใช้ Substring เพื่อแยก Synset_offset และ Data_Body ตามเดิม
                if (line.Length >= 8)
                {
                    // Col 1: synset_offset (8 ตัวอักษรแรก)
                    string synsetOffset = line.Substring(0, 8).Trim();

                    // Col 2: ข้อมูลส่วนที่เหลือ (ตั้งแต่ตำแหน่งที่ 8 จนจบ)
                    string dataBody = line.Substring(8).Trim();

                    // 3. สร้างแถวใหม่และเพิ่มเข้าตาราง
                    string[] row = new string[] { synsetOffset, dataBody };

                    table.Rows.Add(row);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void filesPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            DialogResult result = folder.ShowDialog();
            if (result == DialogResult.OK)
            {
                filePathTextBox.Text = folder.SelectedPath;
            }

            nounPathTextBox.Text = "data.noun";
        }

        private void verbPathButton_Click(object sender, EventArgs e)
        {
            table.Rows.Clear();
            // 1. อ่านไฟล์แบบสตรีม, ข้าม 29 บรรทัดแรก (ที่เป็นคำอธิบาย)
            // และจำกัดการอ่านข้อมูลจริงเพียง 10 บรรทัดถัดไปเท่านั้น
            var lines = File.ReadLines(@"C:\Users\phumi\OneDrive\Desktop\Sem1\Project\Sem2\ReadWordNetFile\data.verb")
                            .Skip(29) // ข้าม 29 บรรทัดแรกที่เป็น Header/License
                            .Take(10) // <-- เพิ่ม: จำกัดการอ่านแค่ 10 บรรทัดข้อมูลแรก
                            .ToArray(); // แปลงเป็น Array เพื่อให้วนลูปได้ง่าย

            // 2. วนลูปเพื่อประมวลผลแต่ละบรรทัดข้อมูล (Synset)
            foreach (string line in lines)
            {
                // โค้ดส่วนนี้ยังคงใช้ Substring เพื่อแยก Synset_offset และ Data_Body ตามเดิม
                if (line.Length >= 8)
                {
                    // Col 1: synset_offset (8 ตัวอักษรแรก)
                    string synsetOffset = line.Substring(0, 8).Trim();

                    // Col 2: ข้อมูลส่วนที่เหลือ (ตั้งแต่ตำแหน่งที่ 8 จนจบ)
                    string dataBody = line.Substring(8).Trim();

                    // 3. สร้างแถวใหม่และเพิ่มเข้าตาราง
                    string[] row = new string[] { synsetOffset, dataBody };

                    table.Rows.Add(row);
                }
            }
        }

        private void adjectivePathButton_Click(object sender, EventArgs e)
        {
            table.Rows.Clear();
            // 1. อ่านไฟล์แบบสตรีม, ข้าม 29 บรรทัดแรก (ที่เป็นคำอธิบาย)
            // และจำกัดการอ่านข้อมูลจริงเพียง 10 บรรทัดถัดไปเท่านั้น
            var lines = File.ReadLines(@"C:\Users\phumi\OneDrive\Desktop\Sem1\Project\Sem2\ReadWordNetFile\data.adj")
                            .Skip(29) // ข้าม 29 บรรทัดแรกที่เป็น Header/License
                            .Take(10) // <-- เพิ่ม: จำกัดการอ่านแค่ 10 บรรทัดข้อมูลแรก
                            .ToArray(); // แปลงเป็น Array เพื่อให้วนลูปได้ง่าย

            // 2. วนลูปเพื่อประมวลผลแต่ละบรรทัดข้อมูล (Synset)
            foreach (string line in lines)
            {
                // โค้ดส่วนนี้ยังคงใช้ Substring เพื่อแยก Synset_offset และ Data_Body ตามเดิม
                if (line.Length >= 8)
                {
                    // Col 1: synset_offset (8 ตัวอักษรแรก)
                    string synsetOffset = line.Substring(0, 8).Trim();

                    // Col 2: ข้อมูลส่วนที่เหลือ (ตั้งแต่ตำแหน่งที่ 8 จนจบ)
                    string dataBody = line.Substring(8).Trim();

                    // 3. สร้างแถวใหม่และเพิ่มเข้าตาราง
                    string[] row = new string[] { synsetOffset, dataBody };

                    table.Rows.Add(row);
                }
            }
        }

        private void adverbPathButton_Click(object sender, EventArgs e)
        {
            table.Rows.Clear();
            // 1. อ่านไฟล์แบบสตรีม, ข้าม 29 บรรทัดแรก (ที่เป็นคำอธิบาย)
            // และจำกัดการอ่านข้อมูลจริงเพียง 10 บรรทัดถัดไปเท่านั้น
            var lines = File.ReadLines(@"C:\Users\phumi\OneDrive\Desktop\Sem1\Project\Sem2\ReadWordNetFile\data.adv")
                            .Skip(29) // ข้าม 29 บรรทัดแรกที่เป็น Header/License
                            .Take(10) // <-- เพิ่ม: จำกัดการอ่านแค่ 10 บรรทัดข้อมูลแรก
                            .ToArray(); // แปลงเป็น Array เพื่อให้วนลูปได้ง่าย

            // 2. วนลูปเพื่อประมวลผลแต่ละบรรทัดข้อมูล (Synset)
            foreach (string line in lines)
            {
                // โค้ดส่วนนี้ยังคงใช้ Substring เพื่อแยก Synset_offset และ Data_Body ตามเดิม
                if (line.Length >= 8)
                {
                    // Col 1: synset_offset (8 ตัวอักษรแรก)
                    string synsetOffset = line.Substring(0, 8).Trim();

                    // Col 2: ข้อมูลส่วนที่เหลือ (ตั้งแต่ตำแหน่งที่ 8 จนจบ)
                    string dataBody = line.Substring(8).Trim();

                    // 3. สร้างแถวใหม่และเพิ่มเข้าตาราง
                    string[] row = new string[] { synsetOffset, dataBody };

                    table.Rows.Add(row);
                }
            }
        }

        private void nounPathComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
