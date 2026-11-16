using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordNetProgram
{
    public class MergedWordNetEntry
    {
        public int Line { get; set; }
        public string Synset_offset { get; set; }
        public string lex_filenum { get; set; }
        public string ss_type { get; set; }
        public string w_cnt { get; set; }
        public string word_lex_id { get; set; }
        public string p_cnt { get; set; }
        public string ptr { get; set; }
        public string frames_cnt { get; set; }
        public string frames { get; set; }
        public string gloss { get; set; }
        public string Number_of_Thai_Word { get; set; }
        public string Thai_Words { get; set; }
        public string TH_Source { get; set; }
    }
}
