using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dec30_2025
{
    // เป็น Class ที่เอาไว้ใช้ในการทดลอง สำหรับทดลองเก็บข้อมูลแค่ 3 ตัว คือ EntryNo, SynsetOffset, LexFilenum
    public class LittleWordNetEntry
    {
        // Properties
        public int EntryNo { get; set; }
        public string SynsetOffset { get; set; }
        public string LexFilenum { get; set; }

        public string SSType { get; set; }

        public int WCnt { get; set; }

        public int Pcnt { get; set; }

        public string Gloss { get; set; }

        public string FullLine { get; set; }


        // Constructor
        public LittleWordNetEntry(int no, string line)
        {
            this.EntryNo = no;
            this.FullLine = line;
            this.SynsetOffset = line.Substring(0, 8);
            this.LexFilenum = line.Substring(9, 2);
            this.SSType = line.Substring(12, 1);
            this.WCnt = Convert.ToInt32(line.Substring(14, 2), 16);
            this.Pcnt = ExtractPCnt(line);

            this.Gloss = line.Split('|')[1].Trim();

        }


        private int ExtractPCnt(string line)
        {
            // ตัดส่วน gloss ออก
            string[] parts = line.Split('|');
            string beforeGloss = parts[0].Trim();

            // แยกด้วยช่องว่าง
            string[] tokens = beforeGloss.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // w_cnt อยู่ที่ index 3
            int w_cnt = int.Parse(tokens[3]);

            // ตำแหน่งของ p_cnt = 4 + (w_cnt * 2)
            int p_cntIndex = 4 + (w_cnt * 2);

            return int.Parse(tokens[p_cntIndex]);
        }

        public override string ToString()
        {
            return $"{SynsetOffset}, {LexFilenum}, {SSType}, {WCnt}, {Gloss}";
        }
    }
}
