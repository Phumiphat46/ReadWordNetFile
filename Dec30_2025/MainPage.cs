using System.Data;

namespace Dec30_2025
{
    public partial class MainPage : Form
    {

        List<LittleWordNetEntry> dataList = new List<LittleWordNetEntry>(); // field
        List<Word_LexID_Pointer> childDataList = new List<Word_LexID_Pointer>();
        //List<Pointer> pointersCountList = new List<Pointer>();
        List<Pointer> pointersList = new List<Pointer>();

        public MainPage() // constructor
        {
            InitializeComponent(); // build ui
            // prototypedisplayNounDataGridView();
            //List<LittleWordNetEntry> list = new List<LittleWordNetEntry>();
            //List<WordNetEntry> wordNetEntries = new List<WordNetEntry>();
            displayNounDataGridViewUsingLittleWordNetEntryClass();
            //showDataindataList();

            // showDatainchildDataList();
        }

        // folder for selecting
        private void btBrowse_Click(object sender, EventArgs e)
        {

            OpenFileDialog fileBrowser = new OpenFileDialog
            {
                InitialDirectory = @"C:\Users\phumi\OneDrive\Desktop\Sem2\ReadWordNetFile", // กำหนด default Directory ให้กับ select folder page
                Title = "Browse CSV Files", // กำหนด header title ให้กับ windows file browser
            };

            if (fileBrowser.ShowDialog() == DialogResult.OK) // ถ้า user เลือกไฟล์และกดปุ่ม OK
            {
                tbFilePath.Text = fileBrowser.FileName; // เอา path ของไฟล์ที่เลือกมาแสดงใน TextBox tbPath
            }
        }

        /* ดึงข้อมูลจาก data.noun มาแสดงใน DataGridView โดยที่ใน datagridview มี column ดังนี้
         * 1. ลำดับที่ 2. SynsetOffset 3. LexFilenum
         * ซึ่งจะแสดงข้อมูลได้มากสุด 10 บรรทัดต่อหน้า และข้าม 29 บรรทัดแรก
         * โดยจะทดลองโดยใช้ class LittleWordNetEntry
         */
        private void displayNounDataGridViewUsingLittleWordNetEntryClass()
        {
            string filePath = @"C:\Users\phumi\OneDrive\Desktop\Sem2\ReadWordNetFile\data.noun";
            // อ่านไฟล์ทีละบรรทัด
            var lines = File.ReadLines(filePath);
            // สร้าง List เพื่อเก็บ WordNetEntry objects

            int counter = 1;
            foreach (string line in lines.Skip(29)) // ข้าม 29 บรรทัดแรก
            {

                if (line.StartsWith(" "))
                    continue; // skip lines that start with a space


                LittleWordNetEntry entries = new LittleWordNetEntry(counter, line); // สร้าง object ของ class โดยมีการส่งค่าผ่าน constructor
                Word_LexID_Pointer wordAndLexID = new Word_LexID_Pointer(entries); // สร้าง object ของ class 
                Pointer pointer = new Pointer(entries);

                dataGridView1.Rows.Add(entries.EntryNo, entries.SynsetOffset, entries.LexFilenum, entries.SSType, entries.WCnt, entries.Pcnt, entries.Gloss);
                counter++;

                dataList.Add(entries);
                childDataList.Add(wordAndLexID);
                //pointersCountList.Add(pointerCount);
                pointersList.Add(pointer);


                if (counter >= 11)
                {
                    break; // แสดงผลได้มากสุด 10 บรรทัด
                }
                
                

            }
        }

        private void showDataindataList()
        {
            foreach (var entry in dataList)
            {
                MessageBox.Show(entry.FullLine);
            }

        }

        private void showDatainchildDataList()
        {
            foreach (var entry in childDataList)
            {
                // 1. รวมรายชื่อคำศัพท์เข้าด้วยกัน คั่นด้วยเครื่องหมายคอมม่า
                string wordsJoined = string.Join(", ", entry.Words);

                // 2. รวม LexID เข้าด้วยกัน (ต้องแปลง int เป็น string ก่อนในตัว)
                string lexIdsJoined = string.Join(", ", entry.LexID);

                // 3. แสดงผลใน MessageBox
                string message = $"Offset: {entry.SynsetOffset}\n" +
                                 $"Words: {wordsJoined}\n" +
                                 $"LexIDs: {lexIdsJoined}";

                MessageBox.Show(message, "Word Details");
            }
        }

        private void MainPage_Load(object sender, EventArgs e)
        {
            MessageBox.Show("MainPage_Load is Working!"); // ถ้าอันนี้ไม่เด้ง แปลว่า Event ยังไม่เชื่อมต่อ

            // 1. สร้าง column แบบ click ได้ เอาไว้ดู Word และ Lex ID
            DataGridViewButtonColumn btnView = new DataGridViewButtonColumn();
            btnView.HeaderText = "View Word(s) and Lex ID(s)";      // หัวข้อคอลัมน์
            btnView.Name = "viewChildData";            // ชื่อคอลัมน์
            btnView.Text = "View";            // ข้อความบนปุ่ม
            btnView.UseColumnTextForButtonValue = true; // ให้ทุกแถวแสดงคำว่า "View" เหมือนกันหมด
            dataGridView1.Columns.Add(btnView);

            // clickable Column that used for watching Pointers
            DataGridViewButtonColumn btnViewPointer = new DataGridViewButtonColumn();
            btnViewPointer.HeaderText = "View Pointer (s) ";
            btnViewPointer.Name = "viewPointer";
            btnViewPointer.Text = "View";
            btnViewPointer.UseColumnTextForButtonValue = true; // ให้ทุกแถวแสดงคำว่า "View" เหมือนกันหมด
            dataGridView1.Columns.Add(btnViewPointer);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. ตรวจสอบว่าคลิกที่คอลัมน์ปุ่ม "viewChildData" ใช่หรือไม่ (และไม่ใช่หัวตาราง)
            if (dataGridView1.Columns[e.ColumnIndex].Name == "viewChildData" && e.RowIndex >= 0)
            {
                // 2. ดึงข้อมูลจาก List ตาม Index ของแถวที่ถูกคลิก
                // (เพราะลำดับใน dataGridView1 กับ childDataList ตรงกัน)
                Word_LexID_Pointer selectedData = childDataList[e.RowIndex];

                // 3. เปิด ChildForm โดยส่งข้อมูลที่เลือกไปตัวเดียว
                ChildForm2 f2 = new ChildForm2(selectedData);
                f2.ShowDialog(); // ใช้ ShowDialog เพื่อให้จัดการทีละหน้า
            }

            // go to PointerForm
            if (dataGridView1.Columns[e.ColumnIndex].Name == "viewPointer" && e.RowIndex >= 0)
            {
                MessageBox.Show("This is Form 3");
                Pointer selectedData = pointersList[e.RowIndex];
                PointerForm f3 = new PointerForm(selectedData);
                f3.ShowDialog();
            }
        }


        /* Old 
         
         // ดึงข้อมูลจาก data.noun มาแสดงใน DataGridView โดยที่ใน datagridview มี 3 column คือ 1. ลำดับที่ 2. SynsetOffset 3. ข้อมูลส่วนที่เหลือ
        // โดยจะแสดงข้อมูลได้มากสุด 10 บรรทัดต่อหน้า และข้าม 29 บรรทัดแรก
        // ซึ่งจะทดลองโดยไม่มีการใช้ class
        private void prototypedisplayNounDataGridView()
        {
            string filePath = @"C:\Users\phumi\OneDrive\Desktop\Sem2\ReadWordNetFile\data.noun";
            // อ่านไฟล์ทีละบรรทัด
            var lines = File.ReadLines(filePath);
            // แสดงข้อมูลได้มากสุด 10 บรรทัด

            int skippedLines = 0;
            int displayedLines = 0;

            int lineIndex = 0;
            foreach (string line in lines)
            {
                // ✅ ข้าม 29 บรรทัดแรก
                if (skippedLines < 29)
                {
                    skippedLines++;
                    continue;
                }

                // ✅ จำกัดให้แสดงผล 10 บรรทัดต่อหน้า
                if (displayedLines >= 10)
                {
                    break;
                }
                // แยกข้อมูลเบื้องต้นด้วย Substring
                string synsetOffset = line.Substring(0, 8);
                string remainingData = line.Substring(9); // ข้อมูลส่วนที่เหลือหลังจาก SynsetOffset
                // เพิ่มข้อมูลลงใน DataGridView
                dataGridView1.Rows.Add(lineIndex + 1, synsetOffset, remainingData);
                displayedLines++;
                lineIndex++;
            }
        }*/
    }
}


