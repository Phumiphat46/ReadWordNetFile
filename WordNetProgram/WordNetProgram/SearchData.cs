using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordNetProgram
{
    public partial class SearchData : Form
    {
        private readonly string dataFolderPath = @"C:\Users\phumi\OneDrive\Desktop\Inputs_of_Merge_Program\all_Merged_Thai";

        // Flag to prevent recursive event firing between cbAll and individual checkboxes
        private bool isUpdatingAll = false;

        public SearchData()
        {
            InitializeComponent();
            SetupDataGridView();
            SetupCheckboxEvents();
        }

        private void SetupCheckboxEvents()
        {
            // Attach CheckedChanged handler to all individual POS checkboxes
            cbNoun.CheckedChanged += cbPos_CheckedChanged;
            cbVerb.CheckedChanged += cbPos_CheckedChanged;
            cbAdj.CheckedChanged += cbPos_CheckedChanged;
            cbAdv.CheckedChanged += cbPos_CheckedChanged;
        }

        private void cbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (isUpdatingAll) return;

            isUpdatingAll = true;
            bool isChecked = cbAll.Checked;
            cbNoun.Checked = isChecked;
            cbVerb.Checked = isChecked;
            cbAdj.Checked = isChecked;
            cbAdv.Checked = isChecked;
            isUpdatingAll = false;
        }

        private void cbPos_CheckedChanged(object sender, EventArgs e)
        {
            if (isUpdatingAll) return;

            isUpdatingAll = true;

            // If any individual POS is unchecked, cbAll must be unchecked
            // If all individual POS checkboxes are checked, cbAll turns checked
            if (cbNoun.Checked && cbVerb.Checked && cbAdj.Checked && cbAdv.Checked)
            {
                cbAll.Checked = true;
            }
            else
            {
                cbAll.Checked = false;
            }

            isUpdatingAll = false;
        }

        private void SetupDataGridView()
        {
            dgvDisplayData.AutoGenerateColumns = false;

            // Map the DataGridView columns to the MergedEntry properties
            if (dgvDisplayData.Columns.Contains("line")) dgvDisplayData.Columns["line"].DataPropertyName = "line";
            if (dgvDisplayData.Columns.Contains("synset_offset")) dgvDisplayData.Columns["synset_offset"].DataPropertyName = "synset_offset";
            if (dgvDisplayData.Columns.Contains("ss_type")) dgvDisplayData.Columns["ss_type"].DataPropertyName = "ss_type";
            if (dgvDisplayData.Columns.Contains("en_cnt")) dgvDisplayData.Columns["en_cnt"].DataPropertyName = "en_cnt";
            if (dgvDisplayData.Columns.Contains("en_words_and_lex_id")) dgvDisplayData.Columns["en_words_and_lex_id"].DataPropertyName = "en_words_and_lex_id";
            if (dgvDisplayData.Columns.Contains("p_cnt")) dgvDisplayData.Columns["p_cnt"].DataPropertyName = "p_cnt";
            if (dgvDisplayData.Columns.Contains("gloss")) dgvDisplayData.Columns["gloss"].DataPropertyName = "gloss";
            if (dgvDisplayData.Columns.Contains("th_cnt")) dgvDisplayData.Columns["th_cnt"].DataPropertyName = "th_cnt";
            if (dgvDisplayData.Columns.Contains("thai_words")) dgvDisplayData.Columns["thai_words"].DataPropertyName = "thai_words";
            if (dgvDisplayData.Columns.Contains("th_src")) dgvDisplayData.Columns["th_src"].DataPropertyName = "th_src";

            DataGridViewButtonColumn viewBtnCol = new DataGridViewButtonColumn();
            viewBtnCol.Name = "view";
            viewBtnCol.HeaderText = "View";
            viewBtnCol.Text = "View";
            viewBtnCol.UseColumnTextForButtonValue = true;
            dgvDisplayData.Columns.Add(viewBtnCol);

            // Wire the click event for the view button
            dgvDisplayData.CellContentClick -= dgvDisplayData_CellContentClick;
            dgvDisplayData.CellContentClick += dgvDisplayData_CellContentClick;
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = tbSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a word or synset_offset to search.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Determine which files to load based on POS checkboxes
            List<string> targetFiles = new List<string>();
            if (cbNoun.Checked) targetFiles.Add("Merged_Thai_data.noun");
            if (cbVerb.Checked) targetFiles.Add("Merged_Thai_data.verb");
            if (cbAdj.Checked) targetFiles.Add("Merged_Thai_data.adj");
            if (cbAdv.Checked) targetFiles.Add("Merged_Thai_data.adv");

            if (targetFiles.Count == 0)
            {
                MessageBox.Show("Please select at least one POS (Noun, Verb, etc.) to search in.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<MergedEntry> searchResults = new List<MergedEntry>();

            // 2. Load and Search
            foreach (string fileName in targetFiles)
            {
                string fullPath = Path.Combine(dataFolderPath, fileName);
                if (!File.Exists(fullPath)) continue;

                string[] lines = File.ReadAllLines(fullPath, Encoding.UTF8);
                int lineNumber = 1;

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("  ") || line.StartsWith("("))
                    {
                        lineNumber++;
                        continue;
                    }

                    MergedEntry entry = ParseLine(line, lineNumber);
                    if (entry != null)
                    {
                        // 3. Apply Search Filters
                        bool isMatch = false;

                        // Check specific fields if checkboxes are selected
                        if (cbSynsetOffset.Checked && entry.synset_offset.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0) isMatch = true;
                        if (cbEngWord.Checked && entry.en_words_and_lex_id.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0) isMatch = true;
                        if (cbThaiWord.Checked && entry.thai_words.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0) isMatch = true;
                        if (cbGloss.Checked && entry.gloss.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0) isMatch = true;
                        if (cbThaiSource.Checked && entry.th_src.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0) isMatch = true;

                        // If no specific field is checked, default to checking all fields
                        if (!cbSynsetOffset.Checked && !cbEngWord.Checked && !cbThaiWord.Checked && !cbGloss.Checked && !cbThaiSource.Checked && !isMatch)
                        {
                            if (entry.synset_offset.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                entry.en_words_and_lex_id.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                entry.thai_words.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                entry.gloss.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                entry.th_src.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                isMatch = true;
                            }
                        }

                        if (isMatch)
                        {
                            searchResults.Add(entry);
                        }
                    }
                    lineNumber++;
                }
            }

            // 4. Display Results
            dgvDisplayData.DataSource = null;
            dgvDisplayData.DataSource = searchResults;

            // Update the result count label
            lbResultCount.Text = $"Search Result: {searchResults.Count}";

            if (searchResults.Count == 0)
            {
                MessageBox.Show("No matches found for the given search criteria.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private MergedEntry ParseLine(string line, int lineNumber)
        {
            try
            {
                var mainParts = line.Split(new[] { '|' }, 2);
                if (mainParts.Length < 2) return null;

                string dataPart = mainParts[0].Trim();
                string rightSide = mainParts[1].Trim();

                var tokens = dataPart.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length < 6) return null;

                var entry = new MergedEntry
                {
                    line = lineNumber,
                    synset_offset = tokens[0],
                    lex_filenum = tokens[1],
                    ss_type = tokens[2],
                    en_cnt = tokens[3]
                };

                int wordCount = Convert.ToInt32(tokens[3], 16);
                int pCntIndex = 4 + (wordCount * 2);
                entry.en_words_and_lex_id = string.Join(" ", tokens.Skip(4).Take(wordCount * 2));

                if (pCntIndex < tokens.Length)
                {
                    entry.p_cnt = tokens[pCntIndex];
                    int ptrCount = 0;
                    int.TryParse(tokens[pCntIndex], out ptrCount);
                    int ptrTokensToTake = ptrCount * 4;
                    entry.ptr = string.Join(" ", tokens.Skip(pCntIndex + 1).Take(ptrTokensToTake));
                }

                // Parse Gloss and Thai extensions
                int thIndex = rightSide.IndexOf("TH:");
                if (thIndex >= 0)
                {
                    entry.gloss = rightSide.Substring(0, thIndex).Trim();
                    string thSection = rightSide.Substring(thIndex);

                    var pieces = thSection.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (pieces.Length >= 2) entry.th_cnt = pieces[1];

                    int originIndex = thSection.IndexOf("TH_Source:");
                    if (originIndex == -1) originIndex = thSection.IndexOf("TH_Origin:"); // Fallback support

                    if (originIndex >= 0)
                    {
                        entry.th_src = thSection.Substring(originIndex + 10).Trim();
                        int startWords = thSection.IndexOf(entry.th_cnt) + entry.th_cnt.Length;
                        entry.thai_words = thSection.Substring(startWords, originIndex - startWords).Trim().Trim(',');
                    }
                }
                else
                {
                    entry.gloss = rightSide;
                    entry.th_cnt = "0";
                    entry.thai_words = "";
                    entry.th_src = "";
                }

                return entry;
            }
            catch
            {
                return null;
            }
        }

        private void dgvDisplayData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 5. Open new form on View button click
            if (e.RowIndex >= 0 && dgvDisplayData.Columns[e.ColumnIndex].Name.Equals("view", StringComparison.OrdinalIgnoreCase))
            {
                MergedEntry selectedEntry = (MergedEntry)dgvDisplayData.Rows[e.RowIndex].DataBoundItem;
                if (selectedEntry != null)
                {
                    string formattedEnWords = FormatEnglishWords(selectedEntry.en_words_and_lex_id);

                    string displayContent =
                        $"synset_offset: {selectedEntry.synset_offset}\n" +
                        $"lex_filenum: {selectedEntry.lex_filenum}\n" +
                        $"ss_type: {selectedEntry.ss_type}\n" +
                        $"en_cnt: {selectedEntry.en_cnt}\n" +
                        $"en_words_and_lex_id: {formattedEnWords}\n" +
                        $"p_cnt: {selectedEntry.p_cnt}\n" +
                        $"ptr: {selectedEntry.ptr}\n" +
                        $"gloss: {selectedEntry.gloss}\n" +
                        $"th_cnt: {selectedEntry.th_cnt}\n" +
                        $"th_words: {selectedEntry.thai_words}\n" +
                        $"th_src: {selectedEntry.th_src}";

                    using (ViewRecordForm viewForm = new ViewRecordForm(displayContent))
                    {
                        viewForm.ShowDialog();
                    }
                }
            }
        }

        private string FormatEnglishWords(string enWordsAndLexId)
        {
            if (string.IsNullOrWhiteSpace(enWordsAndLexId)) return "";

            var tokens = enWordsAndLexId.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> pairedWords = new List<string>();

            for (int i = 0; i < tokens.Length; i += 2)
            {
                if (i + 1 < tokens.Length)
                {
                    pairedWords.Add($"{tokens[i]} {tokens[i + 1]}");
                }
                else
                {
                    pairedWords.Add(tokens[i]);
                }
            }

            return string.Join(", ", pairedWords);
        }
    }
}