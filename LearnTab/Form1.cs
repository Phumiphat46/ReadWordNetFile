namespace LearnTab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // tabControl1.SelectTab("tabPage2"); // กำหนดให้ default tab เป็น tabPage2
            tabControl1.SelectedIndex = 2; // กำหนดให้ default tab เป็น tabPage 3 (เพราะ tabPage 1 คือ index 0)
        }

        private void removeTabButton_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab); // ลบ current tab (tab ที่ user กำลังเปิดใช้อยู่)
        }

        private void addTabButton_Click(object sender, EventArgs e) // method สร้าง tab ขึ้นมาใหม่
        {
            TabPage tpage = new TabPage("Tab Page ");
            Label l1 = new Label();
            l1.Text = "tab 4";
            tpage.Controls.Add(l1);
            tabControl1.TabPages.Add(tpage);

        }

        private void removeAllTabButton_Click(object sender, EventArgs e) // method ลบ tab ทั้งหมดออกไป
        {
            tabControl1.TabPages.Clear(); 
        }
    }
}
// per se