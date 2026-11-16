using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordNetProgram
{
    public class WordNetJsonEntry
    {
        public string Offset { get; set; }
        public string LexFileNum { get; set; }
        public string SynsetType { get; set; }
        public List<string> Words { get; set; } = new List<string>();
        public string Gloss { get; set; }
        public string ThaiWordsCount { get; set; }
        public List<string> ThaiWords { get; set; } = new List<string>();
        public string ThaiWordsSource { get; set; }

        public List<WordNetPointer> Antonyms { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> Hypernyms { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> Hyponyms { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> InstanceHypernyms { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> InstanceHyponyms { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> HolonymsMember { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> HolonymsSubstance { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> HolonymsPart { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> MeronymsMember { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> MeronymsSubstance { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> MeronymsPart { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> Attributes { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> DerivationallyRelated { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> DomainTopic { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> DomainRegion { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> DomainUsage { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> MemberDomainTopic { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> MemberDomainRegion { get; set; } = new List<WordNetPointer>();
        public List<WordNetPointer> MemberDomainUsage { get; set; } = new List<WordNetPointer>();
    }
}
