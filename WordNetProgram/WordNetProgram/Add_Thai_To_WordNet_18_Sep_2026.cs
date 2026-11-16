using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics; // Added to use Process.Start for File Explorer

namespace WordNetProgram
{
    public partial class Add_Thai_To_WordNet_18_Sep_2026 : Form
    {
        // Track the path of the most recently merged file
        private string lastMergedFilePath = "";

        // Store all short file names from the selected folder so we can filter them dynamically
        private List<string> allFolderFiles = new List<string>();



        public Add_Thai_To_WordNet_18_Sep_2026()
        {
            InitializeComponent();
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            string basePath = AppContext.BaseDirectory;
            MessageBox.Show("The .exe is running from:\n" + basePath, "EXE Location - Base Directory", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            // 1. Define the fixed input path dynamically using AppContext.BaseDirectory
            string fixedDataPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Data");

            // 2. Ensure the directory actually exists before trying to read it
            if (!System.IO.Directory.Exists(fixedDataPath))
            {
                MessageBox.Show($"The expected Data folder was not found at:\n{fixedDataPath}", "Folder Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tbFolderPath.Text = fixedDataPath;
            string[] files = System.IO.Directory.GetFiles(fixedDataPath);

            allFolderFiles.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();

            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";

            foreach (string file in files)
            {
                string shortFileName = System.IO.Path.GetFileName(file);
                string lowerFileName = shortFileName.ToLower();

                // Save all file names in memory for dynamic filtering later
                allFolderFiles.Add(shortFileName);

                // Populate comboBox1 with primary WordNet data files
                if (lowerFileName.EndsWith("data.noun") ||
                    lowerFileName.EndsWith("data.verb") ||
                    lowerFileName.EndsWith("data.adj") ||
                    lowerFileName.EndsWith("data.adv"))
                {
                    comboBox1.Items.Add(shortFileName);
                }
            }

            MessageBox.Show("Files successfully loaded from the fixed Data folder.", "Files Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear existing candidate items in comboBox 2, 3, and 4
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();

            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;

            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";

            if (comboBox1.SelectedItem != null)
            {
                string selectedFileName = comboBox1.SelectedItem.ToString();

                // 1. If data.noun is chosen:
                //    - comboBox2 shows ONLY "omw_noun.tab"
                //    - comboBox3 is enabled and shows ONLY "dhanon_thai_noun.tab"
                //    - comboBox4 shows ONLY "gemini_noun.tab"
                if (selectedFileName.EndsWith("data.noun", StringComparison.OrdinalIgnoreCase))
                {
                    comboBox3.Enabled = true;
                    FilterAndPopulateComboBox(comboBox2, "omw_noun.tab");
                    FilterAndPopulateComboBox(comboBox3, "dhanon_thai_noun.tab");
                    FilterAndPopulateComboBox(comboBox4, "gemini_noun.tab");
                }
                // 2. If data.verb is chosen:
                else if (selectedFileName.EndsWith("data.verb", StringComparison.OrdinalIgnoreCase))
                {
                    comboBox3.Enabled = false;
                    FilterAndPopulateComboBox(comboBox2, "omw_verb.tab");
                    FilterAndPopulateComboBox(comboBox4, "gemini_verb.tab");
                }
                // 3. If data.adj is chosen:
                else if (selectedFileName.EndsWith("data.adj", StringComparison.OrdinalIgnoreCase))
                {
                    comboBox3.Enabled = false;
                    FilterAndPopulateComboBox(comboBox2, "omw_adj.tab");
                    FilterAndPopulateComboBox(comboBox4, "gemini_adj.tab");
                }
                // 4. If data.adv is chosen:
                else if (selectedFileName.EndsWith("data.adv", StringComparison.OrdinalIgnoreCase))
                {
                    comboBox3.Enabled = false;
                    FilterAndPopulateComboBox(comboBox2, "omw_adv.tab");
                    FilterAndPopulateComboBox(comboBox4, "gemini_adv.tab");
                }
            }
        }

        // Helper method that adds ONLY the matching file(s) as candidates to a ComboBox
        private void FilterAndPopulateComboBox(ComboBox comboBox, string targetSuffix)
        {
            foreach (string fileName in allFolderFiles)
            {
                if (fileName.EndsWith(targetSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    comboBox.Items.Add(fileName);
                }
            }
        }

        private void bTLoadFile_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                MessageBox.Show("Please browse for a folder and select a file from the list first.", "Input Verification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedFileName = comboBox1.SelectedItem.ToString();
            string fullPathToLoad = System.IO.Path.Combine(tbFolderPath.Text, selectedFileName);

            if (!System.IO.File.Exists(fullPathToLoad))
            {
                MessageBox.Show($"Data file missing on local disk target paths:\n{fullPathToLoad}", "File Engine Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadAndParseWordNet(fullPathToLoad);
        }

        private void LoadAndParseWordNet(string filePath)
        {
            var entries = new List<WordNetEntry>();
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(" "))
                    continue;

                var mainParts = line.Split(new[] { '|' }, 2);
                if (mainParts.Length < 2) continue;

                string dataPart = mainParts[0].Trim();
                string glossPart = mainParts[1].Trim();

                var tokens = dataPart.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length < 6) continue;

                try
                {
                    var entry = new WordNetEntry
                    {
                        Synset_offset = tokens[0],
                        lex_filenum = tokens[1],
                        ss_type = tokens[2],
                        w_cnt = tokens[3],
                        gloss = glossPart
                    };

                    int wordCount = Convert.ToInt32(tokens[3], 16);
                    int pCntIndex = 4 + (wordCount * 2);
                    entry.word_lex_id = string.Join(" ", tokens.Skip(4).Take(wordCount * 2));

                    if (pCntIndex < tokens.Length)
                    {
                        entry.p_cnt = tokens[pCntIndex];
                        int ptrCount = Convert.ToInt32(tokens[pCntIndex]); // p_cnt is decimal
                        int ptrTokensToTake = ptrCount * 4; // each pointer is 4 blocks

                        entry.ptr = string.Join(" ", tokens.Skip(pCntIndex + 1).Take(ptrTokensToTake));

                        int framesCntIndex = pCntIndex + 1 + ptrTokensToTake;

                        // Verb specific logic: If there are tokens remaining before the | separator, they are frames
                        if (framesCntIndex < tokens.Length)
                        {
                            entry.frames_cnt = tokens[framesCntIndex];
                            entry.frames = string.Join(" ", tokens.Skip(framesCntIndex + 1));
                        }
                    }

                    entries.Add(entry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing line: {line}. Reason: {ex.Message}");
                }
            }

            dgvWordNet.DataSource = entries;

            // Format dynamic headers
            if (dgvWordNet.Columns.Contains("word_lex_id")) dgvWordNet.Columns["word_lex_id"].HeaderText = "word lex_id";
            if (dgvWordNet.Columns.Contains("ptr")) dgvWordNet.Columns["ptr"].HeaderText = "[ptr...]";
            if (dgvWordNet.Columns.Contains("gloss")) dgvWordNet.Columns["gloss"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void bTLoadOMW_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null || string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                MessageBox.Show("Please browse for a folder and select a file from ComboBox 2 first.", "Input Verification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedFileName = comboBox2.SelectedItem.ToString();
            string fullPathToLoad = System.IO.Path.Combine(tbFolderPath.Text, selectedFileName);

            if (!System.IO.File.Exists(fullPathToLoad))
            {
                MessageBox.Show($"Data file missing on local disk target paths:\n{fullPathToLoad}", "File Engine Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var omwEntries = new List<OMWEntry>();
            string[] lines = File.ReadAllLines(fullPathToLoad, Encoding.UTF8);
            int currentLine = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("("))
                {
                    currentLine++;
                    continue;
                }

                var parts = line.Split('\t');
                if (parts.Length >= 3)
                {
                    omwEntries.Add(new OMWEntry
                    {
                        Line_Number = currentLine,
                        Synset_offset_pos = parts[0].Trim(),
                        Thai_word = parts[2].Trim()
                    });
                }
                currentLine++;
            }

            dGVOMW.DataSource = omwEntries;

            if (dGVOMW.Columns.Contains("Line_Number"))
            {
                dGVOMW.Columns["Line_Number"].HeaderText = "Line";
                dGVOMW.Columns["Line_Number"].DisplayIndex = 0;
                dGVOMW.Columns["Line_Number"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            if (dGVOMW.Columns.Contains("Synset_offset_pos")) dGVOMW.Columns["Synset_offset_pos"].HeaderText = "Synset_offset-pos";
            if (dGVOMW.Columns.Contains("Thai_word")) dGVOMW.Columns["Thai_word"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dGVMergedWordNet.SelectedTab = tabPage2;
        }

        private void bTLoadDhanon_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem == null || string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                MessageBox.Show("Please browse for a folder and select a file from ComboBox 3 first.", "Input Verification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedFileName = comboBox3.SelectedItem.ToString();
            string fullPathToLoad = System.IO.Path.Combine(tbFolderPath.Text, selectedFileName);

            if (!System.IO.File.Exists(fullPathToLoad))
            {
                MessageBox.Show($"Data file missing on local disk target paths:\n{fullPathToLoad}", "File Engine Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var dhanonEntries = new List<DhanonEntry>();
            string[] lines = File.ReadAllLines(fullPathToLoad, Encoding.UTF8);
            int currentLine = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("("))
                {
                    currentLine++;
                    continue;
                }

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3)
                {
                    dhanonEntries.Add(new DhanonEntry
                    {
                        Line_Number = currentLine,
                        Synset_offset_pos = parts[0].Trim(),
                        Thai_word = parts[2].Trim()
                    });
                }
                currentLine++;
            }

            dGVDhanon.DataSource = dhanonEntries;

            if (dGVDhanon.Columns.Contains("Line_Number"))
            {
                dGVDhanon.Columns["Line_Number"].HeaderText = "Line";
                dGVDhanon.Columns["Line_Number"].DisplayIndex = 0;
                dGVDhanon.Columns["Line_Number"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            if (dGVDhanon.Columns.Contains("Synset_offset_pos")) dGVDhanon.Columns["Synset_offset_pos"].HeaderText = "Synset_offset-pos";
            if (dGVDhanon.Columns.Contains("Thai_word")) dGVDhanon.Columns["Thai_word"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dGVMergedWordNet.SelectedTab = tabPage3;
        }

        private void bTLoadGemini_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem == null || string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                MessageBox.Show("Please browse for a folder and select a file from ComboBox 4 first.", "Input Verification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedFileName = comboBox4.SelectedItem.ToString();
            string fullPathToLoad = System.IO.Path.Combine(tbFolderPath.Text, selectedFileName);

            if (!System.IO.File.Exists(fullPathToLoad))
            {
                MessageBox.Show($"Data file missing on local disk target paths:\n{fullPathToLoad}", "File Engine Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var geminiEntries = new List<GeminiEntry>();
            string[] lines = File.ReadAllLines(fullPathToLoad, Encoding.UTF8);
            int currentLine = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("("))
                {
                    currentLine++;
                    continue;
                }

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3)
                {
                    geminiEntries.Add(new GeminiEntry
                    {
                        Line_Number = currentLine,
                        Synset_offset_pos = parts[0].Trim(),
                        Thai_word = parts[2].Trim()
                    });
                }
                currentLine++;
            }

            dGVGemini.DataSource = geminiEntries;

            if (dGVGemini.Columns.Contains("Line_Number"))
            {
                dGVGemini.Columns["Line_Number"].HeaderText = "Line";
                dGVGemini.Columns["Line_Number"].DisplayIndex = 0;
                dGVGemini.Columns["Line_Number"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            if (dGVGemini.Columns.Contains("Synset_offset_pos")) dGVGemini.Columns["Synset_offset_pos"].HeaderText = "Synset_offset-pos";
            if (dGVGemini.Columns.Contains("Thai_word")) dGVGemini.Columns["Thai_word"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dGVMergedWordNet.SelectedTab = tabPage4;
        }

        private Dictionary<string, List<string>> LoadTabTranslations(string filePath)
        {
            var dict = new Dictionary<string, List<string>>();
            if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
                return dict;

            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("(")) continue;

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3)
                {
                    // Extract the 8-digit offset by removing the "-n", "-a", or "-v" suffix
                    string offset = parts[0].Length >= 8 ? parts[0].Substring(0, 8) : parts[0].Split('-')[0];
                    string thaiWord = string.Join(" ", parts.Skip(2)).Trim();

                    if (!dict.ContainsKey(offset))
                    {
                        dict[offset] = new List<string>();
                    }
                    if (!dict[offset].Contains(thaiWord))
                    {
                        dict[offset].Add(thaiWord);
                    }
                }
            }
            return dict;
        }

        private void bTAddThaiToWordNet_Click(object sender, EventArgs e)
        {
            // 1. Verify folder and primary files are selected (Gemini in comboBox4 is optional)
            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null ||
                string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                MessageBox.Show("Please ensure a folder and the necessary input files (at least Data and OMW) are selected.", "Input Verification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string folderPath = tbFolderPath.Text;
            string primaryDataPath = Path.Combine(folderPath, comboBox1.SelectedItem.ToString());
            string omwPath = Path.Combine(folderPath, comboBox2.SelectedItem.ToString());

            // Only try to read Dhanon if it is enabled (meaning a noun file is selected)
            string dhanonPath = "";
            if (comboBox3.Enabled && comboBox3.SelectedItem != null)
            {
                dhanonPath = Path.Combine(folderPath, comboBox3.SelectedItem.ToString());
            }

            // Only try to read Gemini if a file is actually selected in comboBox4
            string geminiPath = "";
            if (comboBox4.SelectedItem != null)
            {
                geminiPath = Path.Combine(folderPath, comboBox4.SelectedItem.ToString());
            }

            // 2. Load the dictionaries into memory
            var omwDict = LoadTabTranslations(omwPath);
            var dhanonDict = LoadTabTranslations(dhanonPath);
            var geminiDict = LoadTabTranslations(geminiPath);

            // 3. Read the original data file
            string[] dataLines = File.ReadAllLines(primaryDataPath, Encoding.UTF8);
            List<string> outputLines = new List<string>();

            foreach (string line in dataLines)
            {
                // Keep header or empty lines untouched
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("  "))
                {
                    outputLines.Add(line);
                    continue;
                }

                // Extract the Synset_offset (the first 8 characters)
                string offset = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                string extension = "";

                // 4. Apply the hierarchical fallback logic
                if (omwDict.ContainsKey(offset) && omwDict[offset].Count > 0)
                {
                    extension = $"\tTH: {omwDict[offset].Count}\t{string.Join(", ", omwDict[offset])}\tTH_Source: OMW";
                }
                else if (dhanonDict.ContainsKey(offset) && dhanonDict[offset].Count > 0)
                {
                    extension = $"\tTH: {dhanonDict[offset].Count}\t{string.Join(", ", dhanonDict[offset])}\tTH_Source: Dhanon";
                }
                else if (geminiDict.ContainsKey(offset) && geminiDict[offset].Count > 0)
                {
                    extension = $"\tTH: {geminiDict[offset].Count}\t{string.Join(", ", geminiDict[offset])}\tTH_Source: Gemini";
                }
                else
                {
                    // If no Thai word exists in any dictionary, append TH: 0
                    extension = "\tTH: 0";
                }

                outputLines.Add(line + extension);
            }

            // 5. Duplicate and write to a new file in the merged_data folder
            string mergedDataFolder = Path.Combine(folderPath, "merged_data");

            // Automatically create the merged_data folder if it does not exist
            if (!Directory.Exists(mergedDataFolder))
            {
                Directory.CreateDirectory(mergedDataFolder);
            }

            string newFileName = "Merged_Thai_" + Path.GetFileName(primaryDataPath);
            string fullOutputPath = Path.Combine(mergedDataFolder, newFileName);

            File.WriteAllLines(fullOutputPath, outputLines, new UTF8Encoding(false));

            // Save the path so btMergeRevealInFileExplorer knows which file to highlight
            lastMergedFilePath = fullOutputPath;

            MessageBox.Show($"File successfully duplicated and merged!\nSaved as: {newFileName}\nLocation: {mergedDataFolder}", "Merge Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 6. Automatically Load into the Tab 5 Grid
            LoadMergedDataToDataGridView(fullOutputPath);
        }

        private void LoadMergedDataToDataGridView(string filePath)
        {
            var mergedEntries = new List<MergedWordNetEntry>();
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            int currentLine = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("  "))
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
                    string thSource = ""; // Renamed from thOrigin

                    // Extract the appended Thai extension
                    int thIndex = rightSide.IndexOf("TH:");
                    if (thIndex >= 0)
                    {
                        glossPart = rightSide.Substring(0, thIndex).Trim();
                        string thSection = rightSide.Substring(thIndex);

                        var pieces = thSection.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pieces.Length >= 2) thCount = pieces[1];

                        // Look for TH_Source:
                        int sourceIndex = thSection.IndexOf("TH_Source:"); // Renamed from originIndex
                        if (sourceIndex >= 0)
                        {
                            // "TH_Source:" is 10 characters long, so + 10 safely captures the value
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
                        TH_Source = thSource // Renamed to TH_Source
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

                        // Verb Specific handling: Capture extra frame parameters
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

            dGVAddedThai.DataSource = mergedEntries;

            // Header explicit bindings
            if (dGVAddedThai.Columns.Contains("word_lex_id")) dGVAddedThai.Columns["word_lex_id"].HeaderText = "word lex_id";
            if (dGVAddedThai.Columns.Contains("ptr")) dGVAddedThai.Columns["ptr"].HeaderText = "[ptr...]";
            if (dGVAddedThai.Columns.Contains("gloss")) dGVAddedThai.Columns["gloss"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dGVAddedThai.Columns.Contains("Thai_Words")) dGVAddedThai.Columns["Thai_Words"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dGVMergedWordNet.SelectedTab = tabPage5;
        }

        // Event handler for the new btMergeRevealInFileExplorer button
        private void btMergedRevealInFileExplorer_Click(object sender, EventArgs e)
        {
            string targetFilePath = lastMergedFilePath;

            // Fallback: If they haven't clicked Merge in this session yet, check if a merged file already exists for the selected ComboBox1 item
            if (string.IsNullOrEmpty(targetFilePath) && comboBox1.SelectedItem != null && !string.IsNullOrWhiteSpace(tbFolderPath.Text))
            {
                string expectedFileName = "Merged_Thai_" + comboBox1.SelectedItem.ToString();
                string mergedDataFolder = Path.Combine(tbFolderPath.Text, "merged_data");
                targetFilePath = Path.Combine(mergedDataFolder, expectedFileName);
            }

            // 1. If the merged file exists, open File Explorer and highlight the specific file
            if (!string.IsNullOrEmpty(targetFilePath) && File.Exists(targetFilePath))
            {
                Process.Start("explorer.exe", $"/select,\"{targetFilePath}\"");
            }
            // 2. Otherwise, if the merged_data folder exists, open that folder directly
            else if (!string.IsNullOrWhiteSpace(tbFolderPath.Text) && Directory.Exists(Path.Combine(tbFolderPath.Text, "merged_data")))
            {
                Process.Start("explorer.exe", $"\"{Path.Combine(tbFolderPath.Text, "merged_data")}\"");
            }
            // 3. If neither exists yet, alert the user
            else
            {
                MessageBox.Show("No merged file or merged_data folder found. Please merge a file first.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}