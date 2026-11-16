using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordNetProgram
{
    public partial class _141126_Part_Holonym_Part_Meronym_of_Noun : Form
    {
        // private string pythonScript = @"C:\Users\phumi\OneDrive\Desktop\Add_Thai_To_WordNet_V5(2_Sep_2026)\5.Test_Gen_Graph_14-Sep-2026\16-Oct-2026_Holonym_Meronym_WinForm\holonym_meronym_backend5.py";
        private string pythonScript = System.IO.Path.Combine(AppContext.BaseDirectory, "holonym_meronym_backend5.py");
        //private string jsonFile = @"C:\Users\phumi\OneDrive\Desktop\Inputs_of_Merge_Program\all_Merged_Thai\Merged_Thai_Noun.json";
        //private string jsonFile = @"C:\Users\phumi\OneDrive\Desktop\Inputs_of_Merge_Program\No_Gemini_Merged_Thai_Noun.json";
        private string jsonFile = System.IO.Path.Combine(AppContext.BaseDirectory, "Data", "merged_data", "merged_JSON", "Merged_Thai_Noun.json");

        private string currentSvgPath = string.Empty;
        private string currentHtmlFilePath = string.Empty;
        private WebBrowser svgViewer = null;
        private double zoomFactor = 1.0;
        private bool isGenerating = false; // Prevents double-execution race conditions

        // Data structure to hold each displayed synset's button label and details
        private class SynsetEntityInfo
        {
            public string Offset { get; set; }
            public string ButtonText { get; set; }
            public string EnglishWords { get; set; }
            public string ThaiWords { get; set; }
            public string Source { get; set; }
            public string Gloss { get; set; }
            public string Holonyms { get; set; }
            public string Meronyms { get; set; }
        }

        private List<SynsetEntityInfo> displayedSynsets = new List<SynsetEntityInfo>();
        private Panel pnlEntityButtons = null;
        private Panel pnlEntityDetails = null;
        private RichTextBox rtbEntityDetails = null;

        public _141126_Part_Holonym_Part_Meronym_of_Noun()
        {
            InitializeComponent();
            SetupEventHandlers();

            // Fill the container (panel1 / splitContainer1) so the embedded WebBrowser uses the entire viewing area
            if (pbGraph.Parent is ScrollableControl parentContainer)
            {
                parentContainer.AutoScroll = true;
            }
            pbGraph.Dock = DockStyle.Fill;

            // Initialize the split layout inside tpEntities (Buttons List on top, Details Panel at bottom)
            InitializeEntitiesTabLayout();

            // Enable pressing 'F' for Full Screen
            this.KeyPreview = true;
            this.KeyDown += _141126_Part_Holonym_Part_Meronym_of_Noun_KeyDown;
        }

        private void InitializeEntitiesTabLayout()
        {
            if (pnlEntityButtons != null) return;

            tpEntities.Controls.Clear();

            // Bottom panel: Details view
            pnlEntityDetails = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 350,
                Padding = new Padding(8),
                BackColor = Color.FromArgb(245, 247, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblDetailsHeader = new Label
            {
                Text = "Synset Details (Click a blue synset link or double-click a button above to regenerate):",
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Tahoma", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            rtbEntityDetails = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Tahoma", 9.5f),
                DetectUrls = false, // Turn off http:// requirement
                Text = "Select any synset button above to inspect its full definition and lexical links."
            };

            // Custom hyperlink hover (Hand cursor) and click handling without "http://"
            rtbEntityDetails.MouseMove += RtbEntityDetails_MouseMove;
            rtbEntityDetails.MouseDown += RtbEntityDetails_MouseDown;

            pnlEntityDetails.Controls.Add(rtbEntityDetails);
            pnlEntityDetails.Controls.Add(lblDetailsHeader);

            // Top/Main panel: Scrollable container for synset buttons
            pnlEntityButtons = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(6)
            };

            tpEntities.Controls.Add(pnlEntityButtons);
            tpEntities.Controls.Add(pnlEntityDetails);
        }

        // Helper: Checks if the mouse is hovering over the top "Offset: 02084071" or any "02083346-สุนัข" link in Holonyms/Meronyms
        private string GetOffsetAtCursor(Point mouseLocation)
        {
            if (string.IsNullOrEmpty(rtbEntityDetails.Text)) return null;

            int charIndex = rtbEntityDetails.GetCharIndexFromPosition(mouseLocation);

            // Ensure the mouse is actually near the character and not in empty white space
            Point charPos = rtbEntityDetails.GetPositionFromCharIndex(charIndex);
            if (Math.Abs(mouseLocation.X - charPos.X) > 20 || Math.Abs(mouseLocation.Y - charPos.Y) > 20)
                return null;

            // 1. Check if hovering over the main "Offset: 02084071" at the top
            Match offsetMatch = Regex.Match(rtbEntityDetails.Text, @"^Offset:\s*(\d{8})");
            if (offsetMatch.Success)
            {
                int start = offsetMatch.Groups[1].Index;
                int end = start + offsetMatch.Groups[1].Length;
                if (charIndex >= start && charIndex <= end)
                {
                    return offsetMatch.Groups[1].Value;
                }
            }

            // 2. Check if hovering over any "02083346-สุนัข" link in Holonyms/Meronyms
            int holoStartIndex = rtbEntityDetails.Text.IndexOf("Holonyms (Wholes):");
            if (holoStartIndex != -1 && charIndex >= holoStartIndex)
            {
                MatchCollection matches = Regex.Matches(rtbEntityDetails.Text.Substring(holoStartIndex), @"(\d{8})-[^\s,\)]+");
                foreach (Match m in matches)
                {
                    int start = holoStartIndex + m.Index;
                    int end = start + m.Length;
                    if (charIndex >= start && charIndex <= end)
                    {
                        return m.Groups[1].Value; // Returns the 8-digit Offset
                    }
                }
            }

            return null;
        }

        private void RtbEntityDetails_MouseMove(object sender, MouseEventArgs e)
        {
            string offset = GetOffsetAtCursor(e.Location);
            rtbEntityDetails.Cursor = (offset != null) ? Cursors.Hand : Cursors.IBeam;
        }

        private async void RtbEntityDetails_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string offset = GetOffsetAtCursor(e.Location);
                if (!string.IsNullOrEmpty(offset))
                {
                    await TriggerGraphForOffsetAsync(offset);
                }
            }
        }

        private async Task TriggerGraphForOffsetAsync(string targetOffset)
        {
            if (isGenerating) return;

            tbSynsetWord.Text = targetOffset;
            isGenerating = true;
            btGenGraph.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                await RunPythonBackendAsync(targetOffset);
            }
            finally
            {
                btGenGraph.Enabled = true;
                this.Cursor = Cursors.Default;
                isGenerating = false;
            }
        }

        private void SetupEventHandlers()
        {
            // Remove existing handlers first to prevent double-subscription if Designer already wired them
            btGenGraph.Click -= BtGenGraph_Click;
            btGenGraph.Click += BtGenGraph_Click;

            btRevealPicInFolder.Click -= BtRevealPicInFolder_Click;
            btRevealPicInFolder.Click += BtRevealPicInFolder_Click;

            btZoomIn.Click -= BtZoomIn_Click;
            btZoomIn.Click += BtZoomIn_Click;

            btZoomOut.Click -= BtZoomOut_Click;
            btZoomOut.Click += BtZoomOut_Click;

            tabControl1.DrawItem -= tabControl1_DrawItem;
            tabControl1.DrawItem += tabControl1_DrawItem;
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            TabControl tabCtrl = (TabControl)sender;
            TabPage tabPage = tabCtrl.TabPages[e.Index];
            Rectangle tabBounds = tabCtrl.GetTabRect(e.Index);

            bool isSelected = (e.State == DrawItemState.Selected);
            Brush bgBrush = isSelected ? Brushes.White : SystemBrushes.Control;

            g.FillRectangle(bgBrush, tabBounds);
            g.DrawRectangle(SystemPens.ControlDark, tabBounds.X, tabBounds.Y, tabBounds.Width, tabBounds.Height);

            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            g.TranslateTransform(tabBounds.Left + (tabBounds.Width / 2f), tabBounds.Top + (tabBounds.Height / 2f));
            g.RotateTransform(90);

            RectangleF rotatedBounds = new RectangleF(-tabBounds.Height / 2f, -tabBounds.Width / 2f, tabBounds.Height, tabBounds.Width);

            Brush textBrush = isSelected ? Brushes.Black : Brushes.DimGray;
            Font textFont = isSelected ? new Font(e.Font, FontStyle.Bold) : e.Font;

            g.DrawString(tabPage.Text, textFont, textBrush, rotatedBounds, format);
            g.ResetTransform();
        }

        private void _141126_Part_Holonym_Part_Meronym_of_Noun_KeyDown(object sender, KeyEventArgs e)
        {
            // Trigger full screen when pressing 'F' if the user is not typing in a TextBox
            if (e.KeyCode == Keys.F && !(this.ActiveControl is TextBox))
            {
                OpenGraphFullScreen();
                e.Handled = true;
            }
        }

        private void InitializeSvgViewer()
        {
            if (svgViewer == null)
            {
                svgViewer = new WebBrowser
                {
                    Dock = DockStyle.Fill,
                    ScrollBarsEnabled = true,
                    ScriptErrorsSuppressed = true
                };

                // Allow pressing 'F' for Full Screen even when the WebBrowser control has focus
                svgViewer.PreviewKeyDown += (s, args) =>
                {
                    if (args.KeyCode == Keys.F)
                    {
                        OpenGraphFullScreen();
                    }
                };

                pbGraph.Controls.Add(svgViewer);
            }
        }

        private async void BtGenGraph_Click(object sender, EventArgs e)
        {
            // Block duplicate clicks or double-wired event calls immediately
            if (isGenerating) return;

            string query = tbSynsetWord.Text.Trim();
            if (string.IsNullOrEmpty(query))
            {
                MessageBox.Show("Please enter an English word, Thai word, or Synset Offset.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isGenerating = true;
            btGenGraph.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                await RunPythonBackendAsync(query);
            }
            finally
            {
                btGenGraph.Enabled = true;
                this.Cursor = Cursors.Default;
                isGenerating = false;
            }
        }

        private async Task RunPythonBackendAsync(string query)
        {
            // No longer clamps values > 10
            string GetArg(string flag, TextBox tb)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    return $" {flag} 0";
                }

                if (int.TryParse(tb.Text.Trim(), out int val))
                {
                    return $" {flag} {val}";
                }

                return $" {flag} 0";
            }

            string arguments = $"\"{pythonScript}\" --query \"{query}\" --json \"{jsonFile}\"" +
                               $"{GetArg("--p", tbMeronym)}" +
                               $"{GetArg("--ip", tb1stIndirectMeronym)}" +
                               $"{GetArg("--cp", tbCoMeronym)}" +
                               $"{GetArg("--icp", tbMeronymOf1stIndirectMeronym)}";

            try
            {
                string scriptDirectory = Path.GetDirectoryName(pythonScript);

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8
                };

                string output = "";
                string errors = "";

                await Task.Run(() =>
                {
                    using (Process process = Process.Start(startInfo))
                    {
                        var outputTask = process.StandardOutput.ReadToEndAsync();
                        var errorTask = process.StandardError.ReadToEndAsync();
                        process.WaitForExit();

                        output = outputTask.Result;
                        errors = errorTask.Result;
                    }
                });

                if (output.Contains("POLYSEMY_START"))
                {
                    string selectedOffset = ShowPolysemyDialog(output);
                    if (!string.IsNullOrEmpty(selectedOffset))
                    {
                        tbSynsetWord.Text = selectedOffset;
                        await RunPythonBackendAsync(selectedOffset);
                    }
                }
                else if (output.Contains("SUCCESS:") || output.Contains("OUTPUT_IMAGE:"))
                {
                    // 1. Collect all displayed synsets and their details from Python output
                    displayedSynsets.Clear();
                    if (output.Contains("SYNSETS_START"))
                    {
                        string[] lines = output.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        bool captureSynsets = false;

                        foreach (string line in lines)
                        {
                            if (line == "SYNSETS_END") captureSynsets = false;
                            if (captureSynsets)
                            {
                                string[] fields = line.Split('|');
                                if (fields.Length >= 8)
                                {
                                    displayedSynsets.Add(new SynsetEntityInfo
                                    {
                                        Offset = fields[0].Trim(),
                                        ButtonText = fields[1].Trim(),
                                        EnglishWords = fields[2].Trim(),
                                        ThaiWords = fields[3].Trim(),
                                        Source = fields[4].Trim(),
                                        Gloss = fields[5].Trim(),
                                        Holonyms = fields[6].Trim(),
                                        Meronyms = fields[7].Trim()
                                    });
                                }
                            }
                            if (line == "SYNSETS_START") captureSynsets = true;
                        }
                    }

                    // 2. Populate buttons and details panel in tpEntities
                    PopulateEntityButtons(displayedSynsets);

                    // 3. Render SVG graph
                    string tag = output.Contains("SUCCESS:") ? "SUCCESS:" : "OUTPUT_IMAGE:";
                    string[] parts = output.Split(new[] { tag }, StringSplitOptions.None);
                    string returnedPath = parts[1].Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();

                    if (!Path.IsPathRooted(returnedPath))
                    {
                        returnedPath = Path.Combine(scriptDirectory, returnedPath);
                    }

                    currentSvgPath = Path.ChangeExtension(returnedPath, ".svg");
                    DisplaySvgInForm(currentSvgPath);
                }
                else
                {
                    MessageBox.Show($"Error running script:\n{errors}\n{output}", "Python Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not launch Python. Ensure Python is in your PATH.\n\n" + ex.Message, "Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateEntityButtons(List<SynsetEntityInfo> synsets)
        {
            InitializeEntitiesTabLayout();
            pnlEntityButtons.Controls.Clear();

            int topPosition = 8;
            int buttonHeight = 38;
            int spacing = 6;
            int buttonWidth = Math.Max(250, pnlEntityButtons.ClientSize.Width - 25);

            for (int i = 0; i < synsets.Count; i++)
            {
                SynsetEntityInfo info = synsets[i];

                Button btn = new Button
                {
                    Text = info.ButtonText,
                    Tag = info,
                    Location = new Point(8, topPosition),
                    Size = new Size(buttonWidth, buttonHeight),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Tahoma", 9.5f, i == 0 ? FontStyle.Bold : FontStyle.Regular),
                    // Root node button matches the green (#86f8a6) used in the Holonym-Meronym graph
                    BackColor = i == 0 ? Color.FromArgb(134, 248, 166) : Color.FromArgb(249, 249, 249),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderColor = Color.LightGray;

                // Single-click: Show full details in the bottom Details Panel
                btn.Click += (s, e) =>
                {
                    if (s is Button clickedBtn && clickedBtn.Tag is SynsetEntityInfo selectedInfo)
                    {
                        ShowSynsetDetails(selectedInfo, clickedBtn);
                    }
                };

                // Double-click: Immediately regenerate the tree for this synset
                btn.MouseDoubleClick += async (s, e) =>
                {
                    if (s is Button clickedBtn && clickedBtn.Tag is SynsetEntityInfo selectedInfo)
                    {
                        ShowSynsetDetails(selectedInfo, clickedBtn);
                        await TriggerGraphForOffsetAsync(selectedInfo.Offset);
                    }
                };

                pnlEntityButtons.Controls.Add(btn);
                topPosition += buttonHeight + spacing;

                // Automatically display the target (root) synset's details initially
                if (i == 0)
                {
                    ShowSynsetDetails(info, btn);
                }
            }
        }

        private void ShowSynsetDetails(SynsetEntityInfo info, Button activeBtn)
        {
            // Highlight the currently selected button border
            foreach (Control ctrl in pnlEntityButtons.Controls)
            {
                if (ctrl is Button b)
                {
                    b.FlatAppearance.BorderSize = (b == activeBtn) ? 2 : 1;
                    b.FlatAppearance.BorderColor = (b == activeBtn) ? Color.FromArgb(46, 139, 87) : Color.LightGray;
                }
            }

            rtbEntityDetails.Text =
                $"Offset: {info.Offset}\n" +
                $"Thai Words: {info.ThaiWords}   |   Source: {info.Source}\n" +
                $"English Words: {info.EnglishWords}\n" +
                $"Gloss: {info.Gloss}\n" +
                $"Holonyms (Wholes): {info.Holonyms}\n" +
                $"Meronyms (Parts): {info.Meronyms}";

            // Reset all text formatting to standard black
            rtbEntityDetails.SelectAll();
            rtbEntityDetails.SelectionColor = Color.Black;
            rtbEntityDetails.SelectionFont = new Font("Tahoma", 9.5f, FontStyle.Regular);

            // 1. Style the top "Offset: 02084071" value as a blue underlined hyperlink
            Match offsetMatch = Regex.Match(rtbEntityDetails.Text, @"^Offset:\s*(\d{8})");
            if (offsetMatch.Success)
            {
                rtbEntityDetails.Select(offsetMatch.Groups[1].Index, offsetMatch.Groups[1].Length);
                rtbEntityDetails.SelectionColor = Color.FromArgb(0, 102, 204); // Link Blue
                rtbEntityDetails.SelectionFont = new Font("Tahoma", 9.5f, FontStyle.Underline);
            }

            // 2. Style every "02083346-สุนัข" pattern in Holonyms/Meronyms as a blue underlined hyperlink
            int holoStartIndex = rtbEntityDetails.Text.IndexOf("Holonyms (Wholes):");
            if (holoStartIndex != -1)
            {
                MatchCollection matches = Regex.Matches(rtbEntityDetails.Text.Substring(holoStartIndex), @"\d{8}-[^\s,\)]+");
                foreach (Match m in matches)
                {
                    rtbEntityDetails.Select(holoStartIndex + m.Index, m.Length);
                    rtbEntityDetails.SelectionColor = Color.FromArgb(0, 102, 204); // Link Blue
                    rtbEntityDetails.SelectionFont = new Font("Tahoma", 9.5f, FontStyle.Underline);
                }
            }

            rtbEntityDetails.SelectionLength = 0;
        }

        private void DisplaySvgInForm(string svgPath)
        {
            if (!File.Exists(svgPath))
            {
                MessageBox.Show($"SVG file not found:\n{svgPath}", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            InitializeSvgViewer();

            try
            {
                string svgContent = File.ReadAllText(svgPath, Encoding.UTF8);
                string htmlFilePath = Path.ChangeExtension(svgPath, ".html");

                // Wrap the SVG in an HTML layout that centers it horizontally and vertically
                string htmlContent = $@"<!DOCTYPE html>
<html>
<head>
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <meta charset=""utf-8"" />
    <style>
        html, body {{
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background-color: #FFFFFF;
        }}
        table {{
            width: 100%;
            height: 100%;
            border-collapse: collapse;
        }}
        td {{
            text-align: center;
            vertical-align: middle;
        }}
        svg {{
            display: inline-block;
        }}
    </style>
</head>
<body>
    <table>
        <tr>
            <td>
                {svgContent}
            </td>
        </tr>
    </table>
</body>
</html>";

                File.WriteAllText(htmlFilePath, htmlContent, new UTF8Encoding(false));
                currentHtmlFilePath = htmlFilePath;
                zoomFactor = 1.0;

                svgViewer.Visible = true;
                svgViewer.Navigate(new Uri(htmlFilePath));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to display SVG in form.\n{ex.Message}", "Render Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ShowPolysemyDialog(string output)
        {
            List<string[]> options = new List<string[]>();
            string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            bool inPolysemy = false;

            foreach (string line in lines)
            {
                if (line == "POLYSEMY_START") { inPolysemy = true; continue; }
                if (line == "POLYSEMY_END") { inPolysemy = false; break; }

                if (inPolysemy)
                {
                    string[] parts = line.Split(new string[] { "||" }, StringSplitOptions.None);
                    if (parts.Length >= 4) options.Add(parts);
                }
            }

            if (options.Count == 0) return null;

            using (Form prompt = new Form()
            {
                Width = 850,
                Height = 400,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = $"Multiple meanings found for '{tbSynsetWord.Text}'. Please select one:",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false
            })
            {
                ListBox listBox = new ListBox() { Left = 10, Top = 10, Width = 810, Height = 280, Font = new Font("Tahoma", 10), HorizontalScrollbar = true };

                for (int i = 0; i < options.Count; i++)
                {
                    var opt = options[i];
                    string thaiPart = string.IsNullOrEmpty(opt[2]) ? "" : $" | Thai Words: {opt[2]}";
                    listBox.Items.Add($"[{i + 1}] Offset: {opt[0]} | Words: {opt[1]}{thaiPart} | Gloss: {opt[3]}");
                }

                Button confirmation = new Button() { Text = "Select", Left = 375, Width = 100, Top = 310, DialogResult = DialogResult.OK };
                prompt.Controls.Add(listBox);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                if (listBox.Items.Count > 0) listBox.SelectedIndex = 0;

                if (prompt.ShowDialog() == DialogResult.OK && listBox.SelectedIndex >= 0)
                {
                    return options[listBox.SelectedIndex][0];
                }
            }

            return null;
        }

        private void ApplyZoom()
        {
            if (svgViewer != null && svgViewer.Document != null && svgViewer.Document.Body != null)
            {
                svgViewer.Document.Body.Style = $"zoom: {Math.Round(zoomFactor * 100)}%;";
            }
        }

        private void BtZoomIn_Click(object sender, EventArgs e)
        {
            if (svgViewer != null && svgViewer.Visible)
            {
                zoomFactor *= 1.2;
                ApplyZoom();
            }
        }

        private void BtZoomOut_Click(object sender, EventArgs e)
        {
            if (svgViewer != null && svgViewer.Visible)
            {
                zoomFactor /= 1.2;
                ApplyZoom();
            }
        }

        private void BtRevealPicInFolder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentSvgPath) && File.Exists(currentSvgPath))
            {
                Process.Start("explorer.exe", $"/select,\"{currentSvgPath}\"");
            }
            else
            {
                MessageBox.Show("No graph has been generated yet.", "Folder Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ==========================================
        // FULL SCREEN OVERLAY LOGIC (Press 'F')
        // ==========================================
        private void OpenGraphFullScreen()
        {
            if (string.IsNullOrEmpty(currentHtmlFilePath) || !File.Exists(currentHtmlFilePath) || svgViewer == null || !svgViewer.Visible)
            {
                MessageBox.Show("ยังไม่มีกราฟให้แสดงผลแบบเต็มจอ กรุณาสร้างกราฟก่อนครับ", "No Graph Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (Form fullScreenForm = new Form())
            {
                fullScreenForm.FormBorderStyle = FormBorderStyle.None;
                fullScreenForm.WindowState = FormWindowState.Maximized;
                fullScreenForm.BackColor = Color.White;
                fullScreenForm.KeyPreview = true;
                fullScreenForm.StartPosition = FormStartPosition.CenterScreen;

                Button btExitFullScreen = new Button
                {
                    Text = "✖ Exit Full Screen (Esc / F)",
                    Size = new Size(190, 36),
                    Location = new Point(Screen.PrimaryScreen.Bounds.Width - 210, 20),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    BackColor = Color.FromArgb(45, 45, 45),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Tahoma", 9.5f, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btExitFullScreen.FlatAppearance.BorderSize = 0;
                btExitFullScreen.Click += (s, args) => fullScreenForm.Close();

                WebBrowser fullScreenBrowser = new WebBrowser
                {
                    Dock = DockStyle.Fill,
                    ScrollBarsEnabled = true,
                    ScriptErrorsSuppressed = true,
                    IsWebBrowserContextMenuEnabled = false
                };

                fullScreenBrowser.PreviewKeyDown += (s, args) =>
                {
                    if (args.KeyCode == Keys.Escape || args.KeyCode == Keys.F)
                    {
                        fullScreenForm.Close();
                    }
                };

                fullScreenForm.KeyDown += (s, args) =>
                {
                    if (args.KeyCode == Keys.Escape || args.KeyCode == Keys.F)
                    {
                        fullScreenForm.Close();
                    }
                };

                fullScreenForm.Controls.Add(btExitFullScreen);
                fullScreenForm.Controls.Add(fullScreenBrowser);
                btExitFullScreen.BringToFront();

                fullScreenBrowser.Navigate(new Uri(currentHtmlFilePath));
                fullScreenForm.ShowDialog(this);
            }
        }
    }
}