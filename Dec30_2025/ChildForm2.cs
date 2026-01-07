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
    public partial class ChildForm2 : Form
    {
        // เปลี่ยนจากเก็บ List เป็นเก็บแค่ก้อนเดียว
        private Word_LexID_Pointer _singleData;

        public ChildForm2(Word_LexID_Pointer data)
        {
            InitializeComponent();
            this._singleData = data;
            DisplaySpecificData();
        }

        private void DisplaySpecificData()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "index";
            dataGridView1.Columns[1].Name = "w_cnt";
            dataGridView1.Columns[2].Name = "wordandlexid";
            // รวม Words และ LexID เข้าด้วยกันเป็นคู่ๆ
            List<string> combinedList = new List<string>();
            for (int i = 0; i < _singleData.Words.Count; i++)
            {
               combinedList.Add($"{_singleData.Words[i]} ({_singleData.LexID[i]})");
                
            }
            // string wordsAndLexIDs = string.Join(", ", combinedList);

            /* old 
            // แสดงผลเฉพาะข้อมูลของแถวที่กดมา
            dataGridView1.Rows.Add(_singleData.SynsetOffset, _singleData.WCnt, wordsAndLexIDs);
            */


            int index = 1;
            foreach (string word in combinedList) 
            { 
                dataGridView1.Rows.Add(index, word);
                index++;
            }
        }
    }
}
