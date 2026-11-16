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
    public partial class _131126_Parent_Child_Of_Verb : Form
    {
        private string currentSvgPath = string.Empty;
        private string currentHtmlFilePath = string.Empty;
        private WebBrowser svgViewer = null;
        private double zoomFactor = 1.0;

        // Data structure to hold each displayed synset's button label and details
        private class SynsetEntityInfo
        {
            public string Offset { get; set; }
            public string ButtonText { get; set; }
            public string EnglishWords { get; set; }
            public string ThaiWords { get; set; }
            public string Source { get; set; }
            public string Gloss { get; set; }
            public string Hypernyms { get; set; }
            public string Hyponyms { get; set; }
        }

        private List<SynsetEntityInfo> displayedSynsets = new List<SynsetEntityInfo>();
        private Panel pnlEntityButtons = null;
        private Panel pnlEntityDetails = null;
        private RichTextBox rtbEntityDetails = null;

        //private string pythonScript = @"C:\Users\phumi\OneDrive\Desktop\Add_Thai_To_WordNet_V5(2_Sep_2026)\5.Test_Gen_Graph_14-Sep-2026\11-Oct-2026_WinForm2\backend4-1.py";
        private string pythonScript = System.IO.Path.Combine(AppContext.BaseDirectory, "parent_child_backend5.py");
        // private string jsonFile = @"C:\Users\phumi\OneDrive\Desktop\Add_Thai_To_WordNet_V5(2_Sep_2026)\5.Test_Gen_Graph_14-Sep-2026\11-Oct-2026_WinForm2\Merged_Thai_Verb.json";
        private string jsonFile = System.IO.Path.Combine(AppContext.BaseDirectory, "Data", "merged_data", "merged_JSON", "Merged_Thai_Verb.json");

        public _131126_Parent_Child_Of_Verb()
        {             
            InitializeComponent();
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
            this.KeyDown += _111126_Parent_Child_Of_Noun_KeyDown;
        }

        private void InitializeEntitiesTabLayout()
        {
            if (pnlEntityButtons != null) return;

            tpEntities.Controls.Clear();

            // Bottom panel: Details view (without the Regenerate button)
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

        // Helper: Checks if the mouse is hovering over the top "Offset: 02084071" or any "02083346-สุนัข" link
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

            // 2. Check if hovering over any "02083346-สุนัข" link in Hypernyms/Hyponyms
            int hyperStartIndex = rtbEntityDetails.Text.IndexOf("Hypernyms (Parents):");
            if (hyperStartIndex != -1 && charIndex >= hyperStartIndex)
            {
                MatchCollection matches = Regex.Matches(rtbEntityDetails.Text.Substring(hyperStartIndex), @"(\d{8})-[^\s,\)]+");
                foreach (Match m in matches)
                {
                    int start = hyperStartIndex + m.Index;
                    int end = start + m.Length;
                    if (charIndex >= start && charIndex <= end)
                    {
                        return m.Groups[1].Value; // Returns the 8-digit Offset (e.g., "02083346")
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
            tbSynsetWord.Text = targetOffset;
            btGenGraph.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            await RunPythonBackendAsync(targetOffset);

            btGenGraph.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void _111126_Parent_Child_Of_Noun_KeyDown(object sender, KeyEventArgs e)
        {
            // Trigger full screen when pressing 'F' if the user is not typing in a TextBox
            if (e.KeyCode == Keys.F && !(this.ActiveControl is TextBox))
            {
                OpenGraphFullScreen();
                e.Handled = true;
            }
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

        private async void btGenGraph_Click(object sender, EventArgs e)
        {
            string query = tbSynsetWord.Text.Trim();
            if (string.IsNullOrEmpty(query))
            {
                MessageBox.Show("Please enter an English word, Thai word, or Offset.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Temporarily disable button to prevent double-clicks
            btGenGraph.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            // Run asynchronously so the C# form stays responsive
            await RunPythonBackendAsync(query);

            btGenGraph.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private async Task RunPythonBackendAsync(string query)
        {
            string GetLimit(TextBox tb)
            {
                if (string.IsNullOrWhiteSpace(tb.Text)) return "0";

                if (int.TryParse(tb.Text.Trim(), out int val))
                {
                    return val.ToString();
                }
                return "0";
            }

            string args = $"\"{jsonFile}\" \"{query}\" " +
                          $"{GetLimit(tbMaxChild)} " +
                          $"{GetLimit(tbMaxGrandChild)} " +
                          $"{GetLimit(tbMaxGreatGrandChild)} " +
                          $"{GetLimit(tbMaxSibling)} " +
                          $"{GetLimit(tbMaxNephew)} " +
                          $"{GetLimit(tbMaxUncleAunt)} " +
                          $"{GetLimit(tbMaxCousin)} " +
                          $"{GetLimit(tbMaxGreatUncleGreatAunt)} " +
                          $"{GetLimit(tbMaxFirstCousin)} ";

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = $"\"{pythonScript}\" {args}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8
                };

                string output = "";
                string error = "";

                // Run Python on a background thread so the C# UI never freezes
                await Task.Run(() =>
                {
                    using (Process process = Process.Start(startInfo))
                    {
                        var outputTask = process.StandardOutput.ReadToEndAsync();
                        var errorTask = process.StandardError.ReadToEndAsync();
                        process.WaitForExit();

                        output = outputTask.Result;
                        error = errorTask.Result;
                    }
                });

                if (output.Contains("POLYSEMY_START"))
                {
                    List<string> options = new List<string>();
                    string[] lines = output.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    bool capture = false;

                    foreach (string line in lines)
                    {
                        if (line == "POLYSEMY_END") capture = false;
                        if (capture) options.Add(line);
                        if (line == "POLYSEMY_START") capture = true;
                    }

                    string chosenOffset = ShowPolysemyDialog(options, query);
                    if (!string.IsNullOrEmpty(chosenOffset))
                    {
                        tbSynsetWord.Text = chosenOffset;
                        await RunPythonBackendAsync(chosenOffset);
                    }
                }
                else if (output.Contains("SUCCESS:"))
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
                                        Hypernyms = fields[6].Trim(),
                                        Hyponyms = fields[7].Trim()
                                    });
                                }
                            }
                            if (line == "SYNSETS_START") captureSynsets = true;
                        }
                    }

                    // 2. Populate buttons and details panel in tpEntities
                    PopulateEntityButtons(displayedSynsets);

                    // 3. Render SVG graph
                    string[] parts = output.Split(new[] { "SUCCESS:" }, StringSplitOptions.None);
                    string returnedPath = parts[1].Trim();

                    currentSvgPath = Path.ChangeExtension(returnedPath, ".svg");
                    DisplaySvgInForm(currentSvgPath);
                }
                else
                {
                    MessageBox.Show($"Python Error:\n{output}\n{error}", "Execution Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not start Python.\nMake sure Python is installed and in your environment PATH.\n\nError: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    BackColor = i == 0 ? Color.FromArgb(173, 216, 230) : Color.FromArgb(249, 249, 249),
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
                    b.FlatAppearance.BorderColor = (b == activeBtn) ? Color.FromArgb(0, 102, 204) : Color.LightGray;
                }
            }

            rtbEntityDetails.Text =
                $"Offset: {info.Offset}\n" +
                $"Thai Words: {info.ThaiWords}   |   Source: {info.Source}\n" +
                $"English Words: {info.EnglishWords}\n" +
                $"Gloss: {info.Gloss}\n" +
                $"Hypernyms (Parents): {info.Hypernyms}\n" +
                $"Hyponyms (Children): {info.Hyponyms}";

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

            // 2. Style every "02083346-สุนัข" pattern in Hypernyms/Hyponyms as a blue underlined hyperlink
            int hyperStartIndex = rtbEntityDetails.Text.IndexOf("Hypernyms (Parents):");
            if (hyperStartIndex != -1)
            {
                MatchCollection matches = Regex.Matches(rtbEntityDetails.Text.Substring(hyperStartIndex), @"\d{8}-[^\s,\)]+");
                foreach (Match m in matches)
                {
                    rtbEntityDetails.Select(hyperStartIndex + m.Index, m.Length);
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

        private string ShowPolysemyDialog(List<string> options, string originalQuery)
        {
            using (Form prompt = new Form())
            {
                prompt.Width = 800;
                prompt.Height = 400;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.Text = $"Multiple meanings found for '{originalQuery}'";
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.MaximizeBox = false;

                Label lbl = new Label()
                {
                    Left = 20,
                    Top = 10,
                    Width = 740,
                    Text = "Please select the specific meaning you want to explore:"
                };

                ListBox listBox = new ListBox()
                {
                    Left = 20,
                    Top = 40,
                    Width = 740,
                    Height = 260,
                    HorizontalScrollbar = true
                };

                foreach (string opt in options)
                {
                    string[] parts = opt.Split('|');
                    if (parts.Length >= 4)
                    {
                        listBox.Items.Add($"[{parts[0]}] {parts[1]} | Thai: {parts[2]} | {parts[3]}");
                    }
                }

                Button confirmation = new Button()
                {
                    Text = "Select",
                    Left = 660,
                    Width = 100,
                    Top = 315,
                    DialogResult = DialogResult.OK
                };

                prompt.Controls.Add(lbl);
                prompt.Controls.Add(listBox);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                if (prompt.ShowDialog() == DialogResult.OK && listBox.SelectedIndex != -1)
                {
                    string selectedItem = options[listBox.SelectedIndex];
                    return selectedItem.Split('|')[0];
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

        private void btZoomIn_Click(object sender, EventArgs e)
        {
            if (svgViewer != null && svgViewer.Visible)
            {
                zoomFactor *= 1.2;
                ApplyZoom();
            }
        }

        private void btZoomOut_Click(object sender, EventArgs e)
        {
            if (svgViewer != null && svgViewer.Visible)
            {
                zoomFactor /= 1.2;
                ApplyZoom();
            }
        }

        private void btRevealPicInFolder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentSvgPath) && File.Exists(currentSvgPath))
            {
                Process.Start("explorer.exe", $"/select,\"{currentSvgPath}\"");
            }
            else
            {
                MessageBox.Show("No graph has been generated yet.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
