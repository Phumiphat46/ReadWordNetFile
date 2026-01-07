using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dec30_2025
{
    /* อาจารย์บอกให้สร้าง class นี้เพื่อเก็บข้อมูลของแต่ละ entry ในไฟล์ WordNet
    โดยที่ให้มีเฉพาะ properties ที่สามารถมีข้อมูลได้มากที่สุด 1 ตัว
    ได้แก่
    1. EntryNo (ลำดับที่)
    2. SynsetOffset (8 ตัวแรก)
    3. LexFilenum (ตัวที่ 10-11)
    4. SsType (ตัวที่ 13)
    5. WCnt (ตัวที่ 15-16)
    6. PCnt (ตัวที่ตามหลังคำศัพท์)
    และ 7. Gloss (ความหมาย)

    โดยที่ Word, LexId และ Pointer ซึ่งเป็นข้อมูลที่สามารถมีได้หลายตัว จะไม่ถูกเก็บใน class นี้
     */
    internal class UnusedWordNetEntry
    {
        // Properties
        public int EntryNo { get; set; }
        public string SynsetOffset { get; set; }
        public string LexFilenum { get; set; }
        public string SsType { get; set; }
        public int WCnt { get; set; }
        public int PCnt { get; set; }
        public string Gloss { get; set; }
        public UnusedWordNetEntry()
        {
        }


    }
}
