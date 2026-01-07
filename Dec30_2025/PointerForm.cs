using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dec30_2025
{
    public partial class PointerForm : Form
    {
        private Pointer _singleData;

        public PointerForm(Pointer data)
        {
            InitializeComponent();
            this._singleData = data;
            DisplaySpecificData();
        }

        private void DisplaySpecificData()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // ตั้งชื่อคอลัมน์
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "index";
            dataGridView1.Columns[1].Name = "pointer";

            // แสดงผล pointer ทีละตัว
            int index = 1;
            foreach (string pointer in _singleData.PointersData)
            {
                dataGridView1.Rows.Add(index, pointer);
                index++;
            }
        }
    }
}


