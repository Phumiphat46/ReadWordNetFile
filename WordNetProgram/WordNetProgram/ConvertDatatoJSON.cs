using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThaiNLPToolR2.Kawpun2.Real_Program.Final_Stage_Merge_Thai_and_Eng_Word.Last_Time_is_Now_18_Sep_2026_._4Sep2026;

namespace WordNetProgram
{
    public partial class ConvertDatatoJSON : Form
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public ConvertDatatoJSON()
        {
            InitializeComponent();
            SetupJsonDataGridView();
        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            // 1. Define the fixed input path dynamically using AppContext.BaseDirectory
            string fixedDataPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Data", "merged_data");

            // 2. Ensure the directory actually exists before trying to read it
            if (!System.IO.Directory.Exists(fixedDataPath))
            {
                MessageBox.Show($"The expected merged_data folder was not found at:\n{fixedDataPath}", "Folder Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tbFolderPath.Text = fixedDataPath;

            string[] files = System.IO.Directory.GetFiles(fixedDataPath);

            cbbMergedNoun.Items.Clear();
            cbbMergedAdj.Items.Clear();
            cbbMergedVerb.Items.Clear();
            cbbMergedAdv.Items.Clear();

            foreach (string file in files)
            {
                string shortFileName = System.IO.Path.GetFileName(file);
                string lowerFileName = shortFileName.ToLower();

                if (lowerFileName.EndsWith("merged_thai_data.noun"))
                {
                    cbbMergedNoun.Items.Add(shortFileName);
                }
                if (lowerFileName.EndsWith("merged_thai_data.adj"))
                {
                    cbbMergedAdj.Items.Add(shortFileName);
                }
                if (lowerFileName.EndsWith("merged_thai_data.verb"))
                {
                    cbbMergedVerb.Items.Add(shortFileName);
                }
                if (lowerFileName.EndsWith("merged_thai_data.adv"))
                {
                    cbbMergedAdv.Items.Add(shortFileName);
                }
            }
            
            MessageBox.Show("Files successfully loaded from the merged_data folder.", "Files Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ==========================================
        // ORIGINAL NOUN PARSING LOGIC
        // ==========================================
        private void btLoadMergedNoun_Click(object sender, EventArgs e)
        {
            if (cbbMergedNoun.SelectedItem == null || string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                MessageBox.Show("Please select a Merged Noun file from the dropdown first.", "Input Verification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedFileName = cbbMergedNoun.SelectedItem.ToString();
            string fullPathToLoad = System.IO.Path.Combine(tbFolderPath.Text, selectedFileName);

            if (!System.IO.File.Exists(fullPathToLoad))
            {
                MessageBox.Show($"File missing:\n{fullPathToLoad}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadMergedNounToDataGridView(fullPathToLoad);

            if (tcPOS.TabPages.Contains(tpNoun)) tcPOS.SelectedTab = tpNoun;
            if (tcNoun.TabPages.Contains(tpMergedNoun)) tcNoun.SelectedTab = tpMergedNoun;
        }

        private void LoadMergedNounToDataGridView(string filePath)
        {
            var mergedEntries = new List<MergedWordNetEntry>();
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            int currentLine = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("  ") || line.StartsWith("("))
                {
                    currentLine++;
                    continue;
                }

                try
                {
                    var coreSplit = line.Split(new[] { '|' }, 2);
                    if (coreSplit.Length < 2) continue;

                    string dataPart = coreSplit[0].Trim();
                    string rightSide = coreSplit[1].Trim();

                    string glossPart = rightSide;
                    string thCount = "";
                    string thaiWords = "";
                    string thSource = "";

                    int thIndex = rightSide.IndexOf("TH:");
                    if (thIndex >= 0)
                    {
                        glossPart = rightSide.Substring(0, thIndex).Trim();
                        string thSection = rightSide.Substring(thIndex);

                        var pieces = thSection.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pieces.Length >= 2) thCount = pieces[1];

                        int sourceIndex = thSection.IndexOf("TH_Source:");
                        if (sourceIndex >= 0)
                        {
                            thSource = thSection.Substring(sourceIndex + 10).Trim();
                            int startWords = thSection.IndexOf(thCount) + thCount.Length;
                            thaiWords = thSection.Substring(startWords, sourceIndex - startWords).Trim();
                        }
                    }

                    var tokens = dataPart.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length < 6) continue;

                    var entry = new MergedWordNetEntry
                    {
                        Line = currentLine,
                        Synset_offset = tokens[0],
                        lex_filenum = tokens[1],
                        ss_type = tokens[2],
                        w_cnt = tokens[3],
                        gloss = glossPart,
                        Number_of_Thai_Word = thCount,
                        Thai_Words = thaiWords,
                        TH_Source = thSource
                    };

                    int wordCount = Convert.ToInt32(tokens[3], 16);
                    int pCntIndex = 4 + (wordCount * 2);
                    entry.word_lex_id = string.Join(" ", tokens.Skip(4).Take(wordCount * 2));

                    if (pCntIndex < tokens.Length)
                    {
                        entry.p_cnt = tokens[pCntIndex];
                        int ptrCount = Convert.ToInt32(tokens[pCntIndex]);
                        int ptrTokensToTake = ptrCount * 4;

                        entry.ptr = string.Join(" ", tokens.Skip(pCntIndex + 1).Take(ptrTokensToTake));

                        int framesCntIndex = pCntIndex + 1 + ptrTokensToTake;
                        if (framesCntIndex < tokens.Length)
                        {
                            entry.frames_cnt = tokens[framesCntIndex];
                            entry.frames = string.Join(" ", tokens.Skip(framesCntIndex + 1));
                        }
                    }

                    mergedEntries.Add(entry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing merged line: {line}. Reason: {ex.Message}");
                }

                currentLine++;
            }

            dgvMergedNoun.DataSource = mergedEntries;

            if (dgvMergedNoun.Columns.Contains("Line"))
            {
                dgvMergedNoun.Columns["Line"].DisplayIndex = 0;
                dgvMergedNoun.Columns["Line"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            if (dgvMergedNoun.Columns.Contains("word_lex_id")) dgvMergedNoun.Columns["word_lex_id"].HeaderText = "word lex_id";
            if (dgvMergedNoun.Columns.Contains("ptr")) dgvMergedNoun.Columns["ptr"].HeaderText = "[ptr...]";
            if (dgvMergedNoun.Columns.Contains("gloss")) dgvMergedNoun.Columns["gloss"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dgvMergedNoun.Columns.Contains("Thai_Words")) dgvMergedNoun.Columns["Thai_Words"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        // ==========================================
        // ORIGINAL ADJECTIVE PARSING LOGIC
        // ==========================================
        private void btLoadMergedAdj_Click(object sender, EventArgs e)
        {
            if (cbbMergedAdj.SelectedItem == null || string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                MessageBox.Show("Please select a Merged Adjective file from the dropdown first.", "Input Verification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedFileName = cbbMergedAdj.SelectedItem.ToString();
            string fullPathToLoad = System.IO.Path.Combine(tbFolderPath.Text, selectedFileName);

            if (!System.IO.File.Exists(fullPathToLoad))
            {
                MessageBox.Show($"File missing:\n{fullPathToLoad}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadMergedAdjToDataGridView(fullPathToLoad);

            if (tcPOS.TabPages.Contains(tpAdj)) tcPOS.SelectedTab = tpAdj;
            if (tcAdj.TabPages.Contains(tpMergedAdj)) tcAdj.SelectedTab = tpMergedAdj;
        }

        private void LoadMergedAdjToDataGridView(string filePath)
        {
            var mergedEntries = new List<MergedWordNetEntry>();
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            int currentLine = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("  ") || line.StartsWith("("))
                {
                    currentLine++;
                    continue;
                }

                try
                {
                    var coreSplit = line.Split(new[] { '|' }, 2);
                    if (coreSplit.Length < 2) continue;

                    string dataPart = coreSplit[0].Trim();
                    string rightSide = coreSplit[1].Trim();

                    string glossPart = rightSide;
                    string thCount = "";
                    string thaiWords = "";
                    string thSource = "";

                    int thIndex = rightSide.IndexOf("TH:");
                    if (thIndex >= 0)
                    {
                        glossPart = rightSide.Substring(0, thIndex).Trim();
                        string thSection = rightSide.Substring(thIndex);

                        var pieces = thSection.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pieces.Length >= 2) thCount = pieces[1];

                        int sourceIndex = thSection.IndexOf("TH_Source:");
                        if (sourceIndex >= 0)
                        {
                            thSource = thSection.Substring(sourceIndex + 10).Trim();
                            int startWords = thSection.IndexOf(thCount) + thCount.Length;
                            thaiWords = thSection.Substring(startWords, sourceIndex - startWords).Trim();
                        }
                    }

                    var tokens = dataPart.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length < 6) continue;

                    var entry = new MergedWordNetEntry
                    {
                        Line = currentLine,
                        Synset_offset = tokens[0],
                        lex_filenum = tokens[1],
                        ss_type = tokens[2],
                        w_cnt = tokens[3],
                        gloss = glossPart,
                        Number_of_Thai_Word = thCount,
                        Thai_Words = thaiWords,
                        TH_Source = thSource
                    };

                    int wordCount = Convert.ToInt32(tokens[3], 16);
                    int pCntIndex = 4 + (wordCount * 2);
                    entry.word_lex_id = string.Join(" ", tokens.Skip(4).Take(wordCount * 2));

                    if (pCntIndex < tokens.Length)
                    {
                        entry.p_cnt = tokens[pCntIndex];
                        int ptrCount = Convert.ToInt32(tokens[pCntIndex]);
                        int ptrTokensToTake = ptrCount * 4;

                        entry.ptr = string.Join(" ", tokens.Skip(pCntIndex + 1).Take(ptrTokensToTake));

                        int framesCntIndex = pCntIndex + 1 + ptrTokensToTake;
                        if (framesCntIndex < tokens.Length)
                        {
                            entry.frames_cnt = tokens[framesCntIndex];
                            entry.frames = string.Join(" ", tokens.Skip(framesCntIndex + 1));
                        }
                    }

                    mergedEntries.Add(entry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing merged line: {line}. Reason: {ex.Message}");
                }

                currentLine++;
            }

            dgvMergedAdj.DataSource = mergedEntries;

            if (dgvMergedAdj.Columns.Contains("Line"))
            {
                dgvMergedAdj.Columns["Line"].DisplayIndex = 0;
                dgvMergedAdj.Columns["Line"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            if (dgvMergedAdj.Columns.Contains("word_lex_id")) dgvMergedAdj.Columns["word_lex_id"].HeaderText = "word lex_id";
            if (dgvMergedAdj.Columns.Contains("ptr")) dgvMergedAdj.Columns["ptr"].HeaderText = "[ptr...]";
            if (dgvMergedAdj.Columns.Contains("gloss")) dgvMergedAdj.Columns["gloss"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dgvMergedAdj.Columns.Contains("Thai_Words")) dgvMergedAdj.Columns["Thai_Words"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        // ==========================================
        // ORIGINAL ADVERB PARSING LOGIC
        // ==========================================
        private void btLoadMergedAdv_Click(object sender, EventArgs e)
        {
            if (cbbMergedAdv.SelectedItem == null || string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                MessageBox.Show("Please select a Merged Adverb file from the dropdown first.", "Input Verification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedFileName = cbbMergedAdv.SelectedItem.ToString();
            string fullPathToLoad = System.IO.Path.Combine(tbFolderPath.Text, selectedFileName);

            if (!System.IO.File.Exists(fullPathToLoad))
            {
                MessageBox.Show($"File missing:\n{fullPathToLoad}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadMergedAdvToDataGridView(fullPathToLoad);

            if (tcPOS.TabPages.Contains(tpAdv)) tcPOS.SelectedTab = tpAdv;
            if (tcAdv.TabPages.Contains(tpMergedAdv)) tcAdv.SelectedTab = tpMergedAdv;
        }

        private void LoadMergedAdvToDataGridView(string filePath)
        {
            var mergedEntries = new List<MergedWordNetEntry>();
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            int currentLine = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("  ") || line.StartsWith("("))
                {
                    currentLine++;
                    continue;
                }

                try
                {
                    var coreSplit = line.Split(new[] { '|' }, 2);
                    if (coreSplit.Length < 2) continue;

                    string dataPart = coreSplit[0].Trim();
                    string rightSide = coreSplit[1].Trim();

                    string glossPart = rightSide;
                    string thCount = "";
                    string thaiWords = "";
                    string thSource = "";

                    int thIndex = rightSide.IndexOf("TH:");
                    if (thIndex >= 0)
                    {
                        glossPart = rightSide.Substring(0, thIndex).Trim();
                        string thSection = rightSide.Substring(thIndex);

                        var pieces = thSection.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pieces.Length >= 2) thCount = pieces[1];

                        int sourceIndex = thSection.IndexOf("TH_Source:");
                        if (sourceIndex >= 0)
                        {
                            thSource = thSection.Substring(sourceIndex + 10).Trim();
                            int startWords = thSection.IndexOf(thCount) + thCount.Length;
                            thaiWords = thSection.Substring(startWords, sourceIndex - startWords).Trim();
                        }
                    }

                    var tokens = dataPart.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length < 6) continue;

                    var entry = new MergedWordNetEntry
                    {
                        Line = currentLine,
                        Synset_offset = tokens[0],
                        lex_filenum = tokens[1],
                        ss_type = tokens[2],
                        w_cnt = tokens[3],
                        gloss = glossPart,
                        Number_of_Thai_Word = thCount,
                        Thai_Words = thaiWords,
                        TH_Source = thSource
                    };

                    int wordCount = Convert.ToInt32(tokens[3], 16);
                    int pCntIndex = 4 + (wordCount * 2);
                    entry.word_lex_id = string.Join(" ", tokens.Skip(4).Take(wordCount * 2));

                    if (pCntIndex < tokens.Length)
                    {
                        entry.p_cnt = tokens[pCntIndex];
                        int ptrCount = Convert.ToInt32(tokens[pCntIndex]);
                        int ptrTokensToTake = ptrCount * 4;

                        entry.ptr = string.Join(" ", tokens.Skip(pCntIndex + 1).Take(ptrTokensToTake));

                        int framesCntIndex = pCntIndex + 1 + ptrTokensToTake;
                        if (framesCntIndex < tokens.Length)
                        {
                            entry.frames_cnt = tokens[framesCntIndex];
                            entry.frames = string.Join(" ", tokens.Skip(framesCntIndex + 1));
                        }
                    }

                    mergedEntries.Add(entry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing merged line: {line}. Reason: {ex.Message}");
                }

                currentLine++;
            }

            dgvMergedAdv.DataSource = mergedEntries;

            if (dgvMergedAdv.Columns.Contains("Line"))
            {
                dgvMergedAdv.Columns["Line"].DisplayIndex = 0;
                dgvMergedAdv.Columns["Line"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            if (dgvMergedAdv.Columns.Contains("word_lex_id")) dgvMergedAdv.Columns["word_lex_id"].HeaderText = "word lex_id";
            if (dgvMergedAdv.Columns.Contains("ptr")) dgvMergedAdv.Columns["ptr"].HeaderText = "[ptr...]";
            if (dgvMergedAdv.Columns.Contains("gloss")) dgvMergedAdv.Columns["gloss"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dgvMergedAdv.Columns.Contains("Thai_Words")) dgvMergedAdv.Columns["Thai_Words"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        // ==========================================
        // ORIGINAL VERB PARSING LOGIC
        // ==========================================
        private void btLoadMergedVerb_Click(object sender, EventArgs e)
        {
            if (cbbMergedVerb.SelectedItem == null || string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                MessageBox.Show("Please select a Merged Verb file from the dropdown first.", "Input Verification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedFileName = cbbMergedVerb.SelectedItem.ToString();
            string fullPathToLoad = System.IO.Path.Combine(tbFolderPath.Text, selectedFileName);

            if (!System.IO.File.Exists(fullPathToLoad))
            {
                MessageBox.Show($"File missing:\n{fullPathToLoad}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadMergedVerbToDataGridView(fullPathToLoad);

            if (tcPOS.TabPages.Contains(tpVerb)) tcPOS.SelectedTab = tpVerb;
            if (tcVerb.TabPages.Contains(tpMergedVerb)) tcVerb.SelectedTab = tpMergedVerb;
        }

        private void LoadMergedVerbToDataGridView(string filePath)
        {
            var mergedEntries = new List<MergedWordNetEntry>();
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            int currentLine = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("  ") || line.StartsWith("("))
                {
                    currentLine++;
                    continue;
                }

                try
                {
                    var coreSplit = line.Split(new[] { '|' }, 2);
                    if (coreSplit.Length < 2) continue;

                    string dataPart = coreSplit[0].Trim();
                    string rightSide = coreSplit[1].Trim();

                    string glossPart = rightSide;
                    string thCount = "";
                    string thaiWords = "";
                    string thSource = "";

                    int thIndex = rightSide.IndexOf("TH:");
                    if (thIndex >= 0)
                    {
                        glossPart = rightSide.Substring(0, thIndex).Trim();
                        string thSection = rightSide.Substring(thIndex);

                        var pieces = thSection.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pieces.Length >= 2) thCount = pieces[1];

                        int sourceIndex = thSection.IndexOf("TH_Source:");
                        if (sourceIndex >= 0)
                        {
                            thSource = thSection.Substring(sourceIndex + 10).Trim();
                            int startWords = thSection.IndexOf(thCount) + thCount.Length;
                            thaiWords = thSection.Substring(startWords, sourceIndex - startWords).Trim();
                        }
                    }

                    var tokens = dataPart.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length < 6) continue;

                    var entry = new MergedWordNetEntry
                    {
                        Line = currentLine,
                        Synset_offset = tokens[0],
                        lex_filenum = tokens[1],
                        ss_type = tokens[2],
                        w_cnt = tokens[3],
                        gloss = glossPart,
                        Number_of_Thai_Word = thCount,
                        Thai_Words = thaiWords,
                        TH_Source = thSource
                    };

                    int wordCount = Convert.ToInt32(tokens[3], 16);
                    int pCntIndex = 4 + (wordCount * 2);
                    entry.word_lex_id = string.Join(" ", tokens.Skip(4).Take(wordCount * 2));

                    if (pCntIndex < tokens.Length)
                    {
                        entry.p_cnt = tokens[pCntIndex];
                        int ptrCount = Convert.ToInt32(tokens[pCntIndex]);
                        int ptrTokensToTake = ptrCount * 4;

                        entry.ptr = string.Join(" ", tokens.Skip(pCntIndex + 1).Take(ptrTokensToTake));

                        int framesCntIndex = pCntIndex + 1 + ptrTokensToTake;
                        if (framesCntIndex < tokens.Length)
                        {
                            entry.frames_cnt = tokens[framesCntIndex];
                            entry.frames = string.Join(" ", tokens.Skip(framesCntIndex + 1));
                        }
                    }

                    mergedEntries.Add(entry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing merged line: {line}. Reason: {ex.Message}");
                }

                currentLine++;
            }

            dgvMergedVerb.DataSource = mergedEntries;

            if (dgvMergedVerb.Columns.Contains("Line"))
            {
                dgvMergedVerb.Columns["Line"].DisplayIndex = 0;
                dgvMergedVerb.Columns["Line"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            if (dgvMergedVerb.Columns.Contains("word_lex_id")) dgvMergedVerb.Columns["word_lex_id"].HeaderText = "word lex_id";
            if (dgvMergedVerb.Columns.Contains("ptr")) dgvMergedVerb.Columns["ptr"].HeaderText = "[ptr...]";
            if (dgvMergedVerb.Columns.Contains("gloss")) dgvMergedVerb.Columns["gloss"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dgvMergedVerb.Columns.Contains("Thai_Words")) dgvMergedVerb.Columns["Thai_Words"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        // ==========================================
        // CHECKBOX LOGIC
        // ==========================================
        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = cbSelectAll.Checked;
            cbNoun.Checked = isChecked;
            cbAdj.Checked = isChecked;
            cbVerb.Checked = isChecked;
            cbAdv.Checked = isChecked;
        }

        private void cbNoun_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNoun.Checked)
            {
                if (string.IsNullOrWhiteSpace(tbFolderPath.Text) || cbbMergedNoun.SelectedItem == null)
                {
                    MessageBox.Show("Please browse the folder and choose a file in the Merged Noun dropdown first.", "Input Verification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbNoun.Checked = false;
                }
            }
        }

        // ==========================================
        // JSON & TSV CREATION LOGIC
        // ==========================================
        private void btToJson_Click(object sender, EventArgs e)
        {
            if (!cbNoun.Checked && !cbAdj.Checked && !cbVerb.Checked && !cbAdv.Checked && !cbSelectAll.Checked)
            {
                MessageBox.Show("Please select at least one file type to convert.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Define the fixed output directory dynamically
                string jsonOutputFolder = Path.Combine(AppContext.BaseDirectory, "Data", "merged_data", "merged_JSON");

                // Create the directory if it doesn't exist
                if (!Directory.Exists(jsonOutputFolder))
                {
                    Directory.CreateDirectory(jsonOutputFolder);
                }

                if (cbNoun.Checked || cbSelectAll.Checked)
                {
                    if (cbbMergedNoun.SelectedItem != null)
                    {
                        string inputPath = Path.Combine(tbFolderPath.Text, cbbMergedNoun.SelectedItem.ToString());
                        string outputJsonPath = Path.Combine(jsonOutputFolder, "Merged_Thai_Noun.json");
                        string outputTsvPath = Path.Combine(jsonOutputFolder, "Index_Merged_Thai_Noun.tsv");

                        ConvertAndIndexFile(inputPath, outputJsonPath, outputTsvPath);

                        if (tcPOS.TabPages.Contains(tpNoun)) tcPOS.SelectedTab = tpNoun;
                        if (tcNoun.TabPages.Contains(tpMergedNounJson)) tcNoun.SelectedTab = tpMergedNounJson;

                        SetupJsonDataGridView();
                        LoadTsvToGrid(outputTsvPath);
                    }
                }

                if (cbAdj.Checked || cbSelectAll.Checked)
                {
                    if (cbbMergedAdj.SelectedItem != null)
                    {
                        string inputPath = Path.Combine(tbFolderPath.Text, cbbMergedAdj.SelectedItem.ToString());
                        string outputJsonPath = Path.Combine(jsonOutputFolder, "Merged_Thai_Adj.json");
                        string outputTsvPath = Path.Combine(jsonOutputFolder, "Index_Merged_Thai_Adj.tsv");

                        ConvertAndIndexFile(inputPath, outputJsonPath, outputTsvPath);

                        if (tcPOS.TabPages.Contains(tpAdj)) tcPOS.SelectedTab = tpAdj;
                        if (tcAdj.TabPages.Contains(tpMergedAdjJson)) tcAdj.SelectedTab = tpMergedAdjJson;

                        SetupAdjJsonDataGridView();
                        LoadAdjTsvToGrid(outputTsvPath);
                    }
                }
                if (cbVerb.Checked || cbSelectAll.Checked)
                {
                    if (cbbMergedVerb.SelectedItem != null)
                    {
                        string inputPath = Path.Combine(tbFolderPath.Text, cbbMergedVerb.SelectedItem.ToString());
                        string outputJsonPath = Path.Combine(jsonOutputFolder, "Merged_Thai_Verb.json");
                        string outputTsvPath = Path.Combine(jsonOutputFolder, "Index_Merged_Thai_Verb.tsv");

                        ConvertAndIndexFile(inputPath, outputJsonPath, outputTsvPath);

                        if (tcPOS.TabPages.Contains(tpVerb)) tcPOS.SelectedTab = tpVerb;
                        if (tcVerb.TabPages.Contains(tpMergedVerbJson)) tcVerb.SelectedTab = tpMergedVerbJson;

                        SetupVerbJsonDataGridView();
                        LoadVerbTsvToGrid(outputTsvPath);
                    }
                }
                if (cbAdv.Checked || cbSelectAll.Checked)
                {
                    if (cbbMergedAdv.SelectedItem != null)
                    {
                        string inputPath = Path.Combine(tbFolderPath.Text, cbbMergedAdv.SelectedItem.ToString());
                        string outputJsonPath = Path.Combine(jsonOutputFolder, "Merged_Thai_Adv.json");
                        string outputTsvPath = Path.Combine(jsonOutputFolder, "Index_Merged_Thai_Adv.tsv");

                        ConvertAndIndexFile(inputPath, outputJsonPath, outputTsvPath);

                        if (tcPOS.TabPages.Contains(tpAdv)) tcPOS.SelectedTab = tpAdv;
                        if (tcAdv.TabPages.Contains(tpMergedAdvJSON)) tcAdv.SelectedTab = tpMergedAdvJSON;

                        SetupAdvJsonDataGridView();
                        LoadAdvTsvToGrid(outputTsvPath);
                    }
                }

                MessageBox.Show($"JSON Conversion successfully completed!\nSaved to: {jsonOutputFolder}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during conversion: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConvertAndIndexFile(string inputFilePath, string outputJsonPath, string outputTsvPath)
        {
            var jsonEntries = new List<WordNetJsonEntry>();
            string[] lines = File.ReadAllLines(inputFilePath, Encoding.UTF8);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("  ") || line.StartsWith("(")) continue;

                try
                {
                    var coreSplit = line.Split(new[] { '|' }, 2);
                    if (coreSplit.Length < 2) continue;

                    string dataPart = coreSplit[0].Trim();
                    string rightSide = coreSplit[1].Trim();

                    var entry = new WordNetJsonEntry();
                    var tokens = dataPart.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (tokens.Length < 6) continue;

                    entry.Offset = tokens[0];
                    entry.LexFileNum = tokens[1];
                    entry.SynsetType = tokens[2];

                    int wordCount = Convert.ToInt32(tokens[3], 16);
                    int currentIdx = 4;

                    for (int i = 0; i < wordCount; i++)
                    {
                        entry.Words.Add(tokens[currentIdx]);
                        currentIdx += 2;
                    }

                    if (currentIdx < tokens.Length)
                    {
                        int pointerCount = Convert.ToInt32(tokens[currentIdx]);
                        currentIdx++;

                        for (int i = 0; i < pointerCount; i++)
                        {
                            if (currentIdx + 3 < tokens.Length)
                            {
                                string symbol = tokens[currentIdx];
                                var ptr = new WordNetPointer
                                {
                                    Target = tokens[currentIdx + 1],
                                    POS = tokens[currentIdx + 2],
                                    SourceTarget = tokens[currentIdx + 3]
                                };
                                AddPointerToEntry(entry, symbol, ptr);
                                currentIdx += 4;
                            }
                        }
                    }

                    int thIndex = rightSide.IndexOf("TH:");
                    if (thIndex >= 0)
                    {
                        entry.Gloss = rightSide.Substring(0, thIndex).Trim().Trim('"');

                        int sourceIndex = rightSide.IndexOf("TH_Source:");
                        if (sourceIndex >= 0)
                        {
                            entry.ThaiWordsSource = rightSide.Substring(sourceIndex + 10).Trim();

                            string thSection = rightSide.Substring(thIndex + 3, sourceIndex - (thIndex + 3)).Trim();
                            int firstSpace = thSection.IndexOfAny(new[] { ' ', '\t' });

                            if (firstSpace >= 0)
                            {
                                entry.ThaiWordsCount = thSection.Substring(0, firstSpace).Trim();
                                string rawWords = thSection.Substring(firstSpace).Trim();
                                entry.ThaiWords = rawWords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                          .Select(w => w.Trim())
                                                          .ToList();
                            }
                        }
                    }
                    else
                    {
                        entry.Gloss = rightSide.Trim().Trim('"');
                    }

                    jsonEntries.Add(entry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error mapping line: {ex.Message}");
                }
            }

            // Write JSON and exact TSV Byte Offsets simultaneously
            List<string> tsvLines = new List<string>
            {
                "Synset_offset\tstart_byte_offset_of_the_synset\tend_byte_offset_of_the_synset\tSynsetType\tEnglish_words\tThai_words\tGloss\tThai_sources"
            };

            using (FileStream fs = new FileStream(outputJsonPath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                long currentBytePosition = 0;

                byte[] bracketOpen = Encoding.UTF8.GetBytes("[\r\n");
                fs.Write(bracketOpen, 0, bracketOpen.Length);
                currentBytePosition += bracketOpen.Length;

                for (int i = 0; i < jsonEntries.Count; i++)
                {
                    var entry = jsonEntries[i];
                    string jsonString = JsonSerializer.Serialize(entry, _jsonOptions);

                    // Indent the JSON string visually to match standard serialization formatting
                    jsonString = "  " + jsonString.Replace("\n", "\n  ");

                    byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);

                    long startOffset = currentBytePosition;
                    fs.Write(jsonBytes, 0, jsonBytes.Length);
                    currentBytePosition += jsonBytes.Length;
                    long endOffset = currentBytePosition;

                    string engWordsStr = string.Join(", ", entry.Words);
                    string thaiWordsStr = entry.ThaiWords != null ? string.Join(", ", entry.ThaiWords) : "";

                    if (entry.ThaiWords != null && entry.ThaiWords.Count > 1)
                    {
                        thaiWordsStr = $"\"{thaiWordsStr}\"";
                    }

                    tsvLines.Add($"{entry.Offset}\t{startOffset}\t{endOffset}\t{entry.SynsetType}\t{engWordsStr}\t{thaiWordsStr}\t{entry.Gloss}\t{entry.ThaiWordsSource ?? ""}");

                    if (i < jsonEntries.Count - 1)
                    {
                        byte[] commaNewline = Encoding.UTF8.GetBytes(",\r\n");
                        fs.Write(commaNewline, 0, commaNewline.Length);
                        currentBytePosition += commaNewline.Length;
                    }
                    else
                    {
                        byte[] newline = Encoding.UTF8.GetBytes("\r\n");
                        fs.Write(newline, 0, newline.Length);
                        currentBytePosition += newline.Length;
                    }
                }

                byte[] bracketClose = Encoding.UTF8.GetBytes("]");
                fs.Write(bracketClose, 0, bracketClose.Length);
            }

            File.WriteAllLines(outputTsvPath, tsvLines, Encoding.UTF8);
        }

        private void AddPointerToEntry(WordNetJsonEntry entry, string symbol, WordNetPointer ptr)
        {
            switch (symbol)
            {
                case "!": entry.Antonyms.Add(ptr); break;
                case "@": entry.Hypernyms.Add(ptr); break;
                case "~": entry.Hyponyms.Add(ptr); break;
                case "@i": entry.InstanceHypernyms.Add(ptr); break;
                case "~i": entry.InstanceHyponyms.Add(ptr); break;
                case "#m": entry.HolonymsMember.Add(ptr); break;
                case "#s": entry.HolonymsSubstance.Add(ptr); break;
                case "#p": entry.HolonymsPart.Add(ptr); break;
                case "%m": entry.MeronymsMember.Add(ptr); break;
                case "%s": entry.MeronymsSubstance.Add(ptr); break;
                case "%p": entry.MeronymsPart.Add(ptr); break;
                case "=": entry.Attributes.Add(ptr); break;
                case "+": entry.DerivationallyRelated.Add(ptr); break;
                case ";c": entry.DomainTopic.Add(ptr); break;
                case ";r": entry.DomainRegion.Add(ptr); break;
                case ";u": entry.DomainUsage.Add(ptr); break;
                case "-c": entry.MemberDomainTopic.Add(ptr); break;
                case "-r": entry.MemberDomainRegion.Add(ptr); break;
                case "-u": entry.MemberDomainUsage.Add(ptr); break;
            }
        }

        // ==========================================
        // DGV JSON VIEWER (INDEX/BYTE SEEK LOGIC)
        // ==========================================
        private void btViewOnDGV_Click(object sender, EventArgs e)
        {
            if (!cbNoun.Checked || cbbMergedNoun.SelectedItem == null)
            {
                MessageBox.Show("Please select the JSON file and check the cbNoun checkbox.", "Requirement Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jsonOutputFolder = Path.Combine(AppContext.BaseDirectory, "Data", "merged_data", "merged_JSON");
            string tsvPath = Path.Combine(jsonOutputFolder, "Index_Merged_Thai_Noun.tsv");

            if (!File.Exists(tsvPath))
            {
                MessageBox.Show($"TSV Index file not found:\n{tsvPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tcPOS.TabPages.Contains(tpNoun)) tcPOS.SelectedTab = tpNoun;
            if (tcNoun.TabPages.Contains(tpMergedNounJson)) tcNoun.SelectedTab = tpMergedNounJson;

            SetupJsonDataGridView();
            LoadTsvToGrid(tsvPath);
        }

        private void SetupJsonDataGridView()
        {
            dgvMergedNounJSON.AutoGenerateColumns = false;
            dgvMergedNounJSON.Columns.Clear();
            dgvMergedNounJSON.AllowUserToAddRows = false;

            dgvMergedNounJSON.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Order_of_Synset", HeaderText = "Order_of_Synset", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMergedNounJSON.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Synset_offset", HeaderText = "Synset_offset", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMergedNounJSON.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SS_Type", HeaderText = "SS_Type", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMergedNounJSON.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Eng_words", HeaderText = "Eng_words" });
            dgvMergedNounJSON.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Thai_words", HeaderText = "Thai_words" });
            dgvMergedNounJSON.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Gloss", HeaderText = "Gloss", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvMergedNounJSON.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TH_Source", HeaderText = "TH_Source" });

            DataGridViewButtonColumn viewBtnCol = new DataGridViewButtonColumn
            {
                Name = "ViewColumn",
                HeaderText = "View",
                Text = "View",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            dgvMergedNounJSON.Columns.Add(viewBtnCol);

            dgvMergedNounJSON.CellContentClick -= dgvMergedNounJSON_CellContentClick;
            dgvMergedNounJSON.CellContentClick += dgvMergedNounJSON_CellContentClick;
        }

        private void LoadTsvToGrid(string tsvPath)
        {
            var viewModels = new List<TsvIndexViewModel>();
            string[] lines = File.ReadAllLines(tsvPath, Encoding.UTF8);

            int order = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("Synset_offset"))
                    continue;

                string[] parts = line.Split('\t');

                if (parts.Length >= 8)
                {
                    viewModels.Add(new TsvIndexViewModel
                    {
                        Order_of_Synset = order++,
                        Synset_offset = parts[0].Trim(),
                        StartByte = long.Parse(parts[1].Trim()),
                        EndByte = long.Parse(parts[2].Trim()),
                        SS_Type = parts[3].Trim(),
                        Eng_words = parts[4].Trim(),
                        Thai_words = parts[5].Trim(),
                        Gloss = parts[6].Trim(),
                        TH_Source = parts[7].Trim()
                    });
                }
            }

            dgvMergedNounJSON.DataSource = viewModels;
        }

        private void dgvMergedNounJSON_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMergedNounJSON.Columns[e.ColumnIndex].Name == "ViewColumn")
            {
                var selectedItem = (TsvIndexViewModel)dgvMergedNounJSON.Rows[e.RowIndex].DataBoundItem;

                string jsonOutputFolder = Path.Combine(AppContext.BaseDirectory, "Data", "merged_data", "merged_JSON");
                string jsonPath = Path.Combine(jsonOutputFolder, "Merged_Thai_Noun.json"); // CHANGE Noun to Adj/Verb/Adv for other grids

                if (!File.Exists(jsonPath))
                {
                    MessageBox.Show($"{Path.GetFileName(jsonPath)} missing from the selected directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    string jsonChunk = "";
                    int bytesToRead = (int)(selectedItem.EndByte - selectedItem.StartByte);

                    using (FileStream fs = new FileStream(jsonPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        fs.Seek(selectedItem.StartByte, SeekOrigin.Begin);
                        byte[] buffer = new byte[bytesToRead];
                        int bytesRead = fs.Read(buffer, 0, bytesToRead);
                        jsonChunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    }

                    SpecificSynsetJSON viewForm = new SpecificSynsetJSON(jsonChunk.Trim());
                    viewForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to seek JSON bytes: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btNounViewOnNotepad_Click(object sender, EventArgs e)
        {
            string jsonOutputFolder = Path.Combine(AppContext.BaseDirectory, "Data", "merged_data", "merged_JSON");
            string jsonPath = Path.Combine(jsonOutputFolder, "Merged_Thai_Noun.json"); // CHANGE Noun to Adj/Verb/Adv for other buttons

            if (File.Exists(jsonPath))
            {
                try
                {
                    Process.Start("notepad.exe", jsonPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open Notepad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"{Path.GetFileName(jsonPath)} does not exist. Convert the data first.", "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SetupAdjJsonDataGridView()
        {
            dgvMergedAdjJson.AutoGenerateColumns = false;
            dgvMergedAdjJson.Columns.Clear();
            dgvMergedAdjJson.AllowUserToAddRows = false;

            dgvMergedAdjJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Order_of_Synset", HeaderText = "Order_of_Synset", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMergedAdjJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Synset_offset", HeaderText = "Synset_offset", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMergedAdjJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SS_Type", HeaderText = "SS_Type", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMergedAdjJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Eng_words", HeaderText = "Eng_words" });
            dgvMergedAdjJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Thai_words", HeaderText = "Thai_words" });
            dgvMergedAdjJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Gloss", HeaderText = "Gloss", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvMergedAdjJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TH_Source", HeaderText = "TH_Source" });

            DataGridViewButtonColumn viewBtnCol = new DataGridViewButtonColumn
            {
                Name = "ViewColumn",
                HeaderText = "View",
                Text = "View",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            dgvMergedAdjJson.Columns.Add(viewBtnCol);

            dgvMergedAdjJson.CellContentClick -= dgvMergedAdjJson_CellContentClick;
            dgvMergedAdjJson.CellContentClick += dgvMergedAdjJson_CellContentClick;
        }

        private void LoadAdjTsvToGrid(string tsvPath)
        {
            var viewModels = new List<TsvIndexViewModel>();
            string[] lines = File.ReadAllLines(tsvPath, Encoding.UTF8);
            int order = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("Synset_offset"))
                    continue;

                string[] parts = line.Split('\t');
                if (parts.Length >= 8)
                {
                    viewModels.Add(new TsvIndexViewModel
                    {
                        Order_of_Synset = order++,
                        Synset_offset = parts[0].Trim(),
                        StartByte = long.Parse(parts[1].Trim()),
                        EndByte = long.Parse(parts[2].Trim()),
                        SS_Type = parts[3].Trim(),
                        Eng_words = parts[4].Trim(),
                        Thai_words = parts[5].Trim(),
                        Gloss = parts[6].Trim(),
                        TH_Source = parts[7].Trim()
                    });
                }
            }
            dgvMergedAdjJson.DataSource = viewModels;
        }

        private void dgvMergedAdjJson_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMergedAdjJson.Columns[e.ColumnIndex].Name == "ViewColumn")
            {
                var selectedItem = (TsvIndexViewModel)dgvMergedAdjJson.Rows[e.RowIndex].DataBoundItem;
                string jsonPath = Path.Combine(tbFolderPath.Text, "Merged_Thai_Adj.json");

                if (!File.Exists(jsonPath))
                {
                    MessageBox.Show("Merged_Thai_Adj.json missing from the selected directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    string jsonChunk = "";
                    int bytesToRead = (int)(selectedItem.EndByte - selectedItem.StartByte);

                    using (FileStream fs = new FileStream(jsonPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        fs.Seek(selectedItem.StartByte, SeekOrigin.Begin);
                        byte[] buffer = new byte[bytesToRead];
                        int bytesRead = fs.Read(buffer, 0, bytesToRead);
                        jsonChunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    }

                    SpecificSynsetJSON viewForm = new SpecificSynsetJSON(jsonChunk.Trim());
                    viewForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to seek JSON bytes: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btAdjViewOnNotepad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                MessageBox.Show("Please select the target folder first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jsonPath = Path.Combine(tbFolderPath.Text, "Merged_Thai_Adj.json");

            if (File.Exists(jsonPath))
            {
                try
                {
                    Process.Start("notepad.exe", jsonPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open Notepad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Merged_Thai_Adj.json does not exist. Convert the data first.", "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void SetupVerbJsonDataGridView()
        {
            dgvMergedVerbJson.AutoGenerateColumns = false;
            dgvMergedVerbJson.Columns.Clear();
            dgvMergedVerbJson.AllowUserToAddRows = false;

            dgvMergedVerbJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Order_of_Synset", HeaderText = "Order_of_Synset", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMergedVerbJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Synset_offset", HeaderText = "Synset_offset", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMergedVerbJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SS_Type", HeaderText = "SS_Type", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMergedVerbJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Eng_words", HeaderText = "Eng_words" });
            dgvMergedVerbJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Thai_words", HeaderText = "Thai_words" });
            dgvMergedVerbJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Gloss", HeaderText = "Gloss", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvMergedVerbJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TH_Source", HeaderText = "TH_Source" });

            DataGridViewButtonColumn viewBtnCol = new DataGridViewButtonColumn
            {
                Name = "ViewColumn",
                HeaderText = "View",
                Text = "View",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            dgvMergedVerbJson.Columns.Add(viewBtnCol);

            dgvMergedVerbJson.CellContentClick -= dgvMergedVerbJson_CellContentClick;
            dgvMergedVerbJson.CellContentClick += dgvMergedVerbJson_CellContentClick;
        }

        private void LoadVerbTsvToGrid(string tsvPath)
        {
            var viewModels = new List<TsvIndexViewModel>();
            string[] lines = File.ReadAllLines(tsvPath, Encoding.UTF8);
            int order = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("Synset_offset"))
                    continue;

                string[] parts = line.Split('\t');
                if (parts.Length >= 8)
                {
                    viewModels.Add(new TsvIndexViewModel
                    {
                        Order_of_Synset = order++,
                        Synset_offset = parts[0].Trim(),
                        StartByte = long.Parse(parts[1].Trim()),
                        EndByte = long.Parse(parts[2].Trim()),
                        SS_Type = parts[3].Trim(),
                        Eng_words = parts[4].Trim(),
                        Thai_words = parts[5].Trim(),
                        Gloss = parts[6].Trim(),
                        TH_Source = parts[7].Trim()
                    });
                }
            }
            dgvMergedVerbJson.DataSource = viewModels;
        }
        private void dgvMergedVerbJson_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMergedVerbJson.Columns[e.ColumnIndex].Name == "ViewColumn")
            {
                var selectedItem = (TsvIndexViewModel)dgvMergedVerbJson.Rows[e.RowIndex].DataBoundItem;
                string jsonPath = Path.Combine(tbFolderPath.Text, "Merged_Thai_Verb.json");

                if (!File.Exists(jsonPath))
                {
                    MessageBox.Show("Merged_Thai_Verb.json missing from the selected directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    string jsonChunk = "";
                    int bytesToRead = (int)(selectedItem.EndByte - selectedItem.StartByte);

                    using (FileStream fs = new FileStream(jsonPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        fs.Seek(selectedItem.StartByte, SeekOrigin.Begin);
                        byte[] buffer = new byte[bytesToRead];
                        int bytesRead = fs.Read(buffer, 0, bytesToRead);
                        jsonChunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    }

                    SpecificSynsetJSON viewForm = new SpecificSynsetJSON(jsonChunk.Trim());
                    viewForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to seek JSON bytes: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btVerbViewOnNotepad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                MessageBox.Show("Please select the target folder first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jsonPath = Path.Combine(tbFolderPath.Text, "Merged_Thai_Verb.json");

            if (File.Exists(jsonPath))
            {
                try
                {
                    Process.Start("notepad.exe", jsonPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open Notepad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Merged_Thai_Verb.json does not exist. Convert the data first.", "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void SetupAdvJsonDataGridView()
        {
            dgvMergedAdvJson.AutoGenerateColumns = false;
            dgvMergedAdvJson.Columns.Clear();
            dgvMergedAdvJson.AllowUserToAddRows = false;

            dgvMergedAdvJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Order_of_Synset", HeaderText = "Order_of_Synset", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMergedAdvJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Synset_offset", HeaderText = "Synset_offset", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMergedAdvJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SS_Type", HeaderText = "SS_Type", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMergedAdvJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Eng_words", HeaderText = "Eng_words" });
            dgvMergedAdvJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Thai_words", HeaderText = "Thai_words" });
            dgvMergedAdvJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Gloss", HeaderText = "Gloss", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvMergedAdvJson.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TH_Source", HeaderText = "TH_Source" });

            DataGridViewButtonColumn viewBtnCol = new DataGridViewButtonColumn
            {
                Name = "ViewColumn",
                HeaderText = "View",
                Text = "View",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            dgvMergedAdvJson.Columns.Add(viewBtnCol);

            dgvMergedAdvJson.CellContentClick -= dgvMergedAdvJson_CellContentClick;
            dgvMergedAdvJson.CellContentClick += dgvMergedAdvJson_CellContentClick;
        }

        private void LoadAdvTsvToGrid(string tsvPath)
        {
            var viewModels = new List<TsvIndexViewModel>();
            string[] lines = File.ReadAllLines(tsvPath, Encoding.UTF8);
            int order = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("Synset_offset"))
                    continue;

                string[] parts = line.Split('\t');
                if (parts.Length >= 8)
                {
                    viewModels.Add(new TsvIndexViewModel
                    {
                        Order_of_Synset = order++,
                        Synset_offset = parts[0].Trim(),
                        StartByte = long.Parse(parts[1].Trim()),
                        EndByte = long.Parse(parts[2].Trim()),
                        SS_Type = parts[3].Trim(),
                        Eng_words = parts[4].Trim(),
                        Thai_words = parts[5].Trim(),
                        Gloss = parts[6].Trim(),
                        TH_Source = parts[7].Trim()
                    });
                }
            }
            dgvMergedAdvJson.DataSource = viewModels;
        }
        private void dgvMergedAdvJson_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMergedAdvJson.Columns[e.ColumnIndex].Name == "ViewColumn")
            {
                var selectedItem = (TsvIndexViewModel)dgvMergedAdvJson.Rows[e.RowIndex].DataBoundItem;
                string jsonPath = Path.Combine(tbFolderPath.Text, "Merged_Thai_Adv.json");

                if (!File.Exists(jsonPath))
                {
                    MessageBox.Show("Merged_Thai_Adv.json missing from the selected directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    string jsonChunk = "";
                    int bytesToRead = (int)(selectedItem.EndByte - selectedItem.StartByte);

                    using (FileStream fs = new FileStream(jsonPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        fs.Seek(selectedItem.StartByte, SeekOrigin.Begin);
                        byte[] buffer = new byte[bytesToRead];
                        int bytesRead = fs.Read(buffer, 0, bytesToRead);
                        jsonChunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    }

                    SpecificSynsetJSON viewForm = new SpecificSynsetJSON(jsonChunk.Trim());
                    viewForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to seek JSON bytes: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btAdvViewOnNotepad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                MessageBox.Show("Please select the target folder first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jsonPath = Path.Combine(tbFolderPath.Text, "Merged_Thai_Adv.json");

            if (File.Exists(jsonPath))
            {
                try
                {
                    Process.Start("notepad.exe", jsonPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open Notepad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Merged_Thai_Adv.json does not exist. Convert the data first.", "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RevealJsonInExplorer(string jsonFileName)
        {
            string jsonOutputFolder = Path.Combine(AppContext.BaseDirectory, "Data", "merged_data", "merged_JSON");
            string jsonPath = Path.Combine(jsonOutputFolder, jsonFileName);

            if (File.Exists(jsonPath))
            {
                try
                {
                    // Opens File Explorer and highlights the specific JSON file
                    Process.Start("explorer.exe", $"/select,\"{jsonPath}\"");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open File Explorer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"{jsonFileName} does not exist. Convert the data first.", "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btNounRevealInFileExplorer_Click(object sender, EventArgs e)
        {
            RevealJsonInExplorer("Merged_Thai_Noun.json");
        }

        private void btAdjRevealInFileExplorer_Click(object sender, EventArgs e)
        {
            RevealJsonInExplorer("Merged_Thai_Adj.json");
        }

        private void btVerbRevealInFileExplorer_Click(object sender, EventArgs e)
        {
            RevealJsonInExplorer("Merged_Thai_Verb.json");
        }

        private void btAdvRevealInFileExplorer_Click(object sender, EventArgs e)
        {
            RevealJsonInExplorer("Merged_Thai_Adv.json");
        }
    }
}