using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dec30_2025
{
    public class Pointer
    {
        public string SynsetOffset { get; set; }
        public string FullLine { get; set; }
        public int PCnt { get; set; }

        public List<string> PointersData { get; set; }

        public Pointer(LittleWordNetEntry entry)
        {
            this.SynsetOffset = entry.SynsetOffset;
            this.FullLine = entry.FullLine;
            this.PCnt = entry.Pcnt;

            // ดึง pointer fields มาเก็บใน PointersData
            this.PointersData = ExtractPointers(this.FullLine, this.PCnt);
        }

        private List<string> ExtractPointers(string line, int p_cnt)
        {
            List<string> pointers = new List<string>();

            // ตัดส่วน gloss ออก
            string[] parts = line.Split('|');
            string beforeGloss = parts[0].Trim();

            // แยกด้วยช่องว่าง
            string[] tokens = beforeGloss.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // w_cnt อยู่ที่ index 3
            int w_cnt = int.Parse(tokens[3]);

            // ตำแหน่งของ p_cnt = 4 + (w_cnt * 2)
            int p_cntIndex = 4 + (w_cnt * 2);

            // pointer fields เริ่มหลัง p_cnt
            int startIndex = p_cntIndex + 1;

            for (int i = 0; i < p_cnt; i++)
            {
                // แต่ละ pointer มี 4 field
                string pointer = string.Join(" ", tokens, startIndex + (i * 4), 4);
                pointers.Add(pointer);
            }

            return pointers;
        }

    }
}
        /*
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

    }
}
        */
