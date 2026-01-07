using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dec30_2025
{
    // เป็น Class ที่เอาไว้เก็บข้อมูล Word, LexID, Pointer ซึ่งเป็นข้อมูลที่สามารถมีได้หลายตัว
    // เป็น Child Class
    public class Word_LexID_Pointer
    {
        // Properties
        // EntryNo 
        public string SynsetOffset { get; set; }
        public string FullLine { get; set; }

        public int WCnt { get; set; }

        public List<string> Words { get; set; }

        public List<int> LexID { get; set; }

        public Word_LexID_Pointer(LittleWordNetEntry entry) // constructor
        {
            this.SynsetOffset = entry.SynsetOffset;
            this.FullLine = entry.FullLine;
            this.WCnt = entry.WCnt; // ?????

            this.Words = new List<string>();
            this.LexID = new List<int>();

            string dataPart = entry.FullLine.Split('|')[0].Trim();
            string[] parts = dataPart.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // we got parts = ["00006269", "03", "n", "01", "life", "0", "002", "@", "00004258", "n", "0000", "~", "07993776", "n", "0000"]

            int wordStartIndex = 4;
            for (int i = 0; i < this.WCnt; i++)
            {
                // index ของคำศัพท์
                int wIdx = wordStartIndex + (i * 2);
                // index ของ lex_id (อยู่ถัดจากคำศัพท์ 1 ตำแหน่ง)
                int lIdx = wIdx + 1;

                string word = parts[wIdx];
                int lexId = Convert.ToInt32(parts[lIdx]);

                this.Words.Add(word);
                this.LexID.Add(lexId);
            }
    }
    }
    
}
