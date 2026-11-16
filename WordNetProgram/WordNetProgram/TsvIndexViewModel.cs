using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiNLPToolR2.Kawpun2.Real_Program.Final_Stage_Merge_Thai_and_Eng_Word.Last_Time_is_Now_18_Sep_2026_._4Sep2026
{
    public class TsvIndexViewModel
    {
        public int Order_of_Synset { get; set; }
        public string Synset_offset { get; set; }
        public string SS_Type { get; set; }
        public string Eng_words { get; set; }
        public string Thai_words { get; set; }
        public string Gloss { get; set; }
        public string TH_Source { get; set; }

        // Hidden properties used for the FileStream seek operation
        public long StartByte { get; set; }
        public long EndByte { get; set; }
    }
}
