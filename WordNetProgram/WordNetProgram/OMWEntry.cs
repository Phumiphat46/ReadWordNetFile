using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordNetProgram
{
    public class OMWEntry
    {
        // C# properties can't have hyphens or spaces, so we use underscores.
        // We will rename the UI column headers dynamically below.
        public int Line_Number { get; set; }
        public string Synset_offset_pos { get; set; }
        public string Thai_word { get; set; }
    }
}
