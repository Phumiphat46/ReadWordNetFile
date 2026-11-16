using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordNetProgram
{
// ใช้กับ SearchData.cs
    public class MergedEntry
    {
        public int line { get; set; }
        public string synset_offset { get; set; }
        public string lex_filenum { get; set; }
        public string ss_type { get; set; }
        public string en_cnt { get; set; }
        public string en_words_and_lex_id { get; set; }
        public string p_cnt { get; set; }
        public string ptr { get; set; }
        public string gloss { get; set; }
        public string th_cnt { get; set; }
        public string thai_words { get; set; }
        public string th_src { get; set; }
    }
}
