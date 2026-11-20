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
    public partial class modified_111126_Parent_Child_Of_Noun : Form
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

        // ==========================================
        // NEW FILTER TAB (tpNewFilter) CONTROLS
        // ==========================================
        private FlowLayoutPanel pnlNewFilterMainFlow = null;
        private FlowLayoutPanel pnlNewFilterSections = null;
        private Panel pnlNewFilterApply = null;
        private readonly List<DropdownSection> newFilterDropdownSections = new List<DropdownSection>();
        private int currentNewHierarchyCount = 0;
        private const int MaxNewHierarchy = 20;

        // Define paths (Use backend4.py if you want SVG-only generation without the 10-node limit)
        //private string pythonScript = @"C:\Users\phumi\OneDrive\Desktop\Add_Thai_To_WordNet_V5(2_Sep_2026)\5.Test_Gen_Graph_14-Sep-2026\11-Oct-2026_WinForm2\backend5.py";
        // default backend path private string pythonScript = System.IO.Path.Combine(AppContext.BaseDirectory, "parent_child_backend5.py");
        private string pythonScript = @"C:\Users\phumi\OneDrive\Desktop\Sem2\ReadWordNetFile\WordNetProgram\WordNetProgram\can_enter_hierarchy_number_parent_child_backend5.py";
        //private string jsonFile = @"C:\Users\phumi\OneDrive\Desktop\Inputs_of_Merge_Program\No_Gemini_Merged_Thai_Noun.json";
        //private string jsonFile = @"C:\Users\phumi\OneDrive\Desktop\Add_Thai_To_WordNet_V5(2_Sep_2026)\5.Test_Gen_Graph_14-Sep-2026\11-Oct-2026_WinForm2\Test_Case_Mammal_to_Offspring_of_Working_dog.noun.json";
        private string jsonFile = @"C:\Users\phumi\OneDrive\Desktop\Sem2\ReadWordNetFile\WordNetProgram\WordNetProgram\Test_Case_Whole_to_Watch_dog.json";
        //private string jsonFile = System.IO.Path.Combine(AppContext.BaseDirectory, "Data", "merged_data", "merged_JSON", "Merged_Thai_Noun.json");

        public modified_111126_Parent_Child_Of_Noun()
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

            // Initialize the dynamic nested dropdown area inside tpNewFilter below tbNewHierarchy
            InitializeNewFilterTabLayout();

            // Enable pressing 'F' for Full Screen
            this.KeyPreview = true;
            this.KeyDown += modified_111126_Parent_Child_Of_Noun_KeyDown;
            this.FormClosed += (s, e) => DisposeNewFilterDropdowns();
        }

        // ==========================================
        // NEW FILTER TAB (tpNewFilter) LOGIC
        // ==========================================
        private void InitializeNewFilterTabLayout()
        {
            if (pnlNewFilterMainFlow != null) return;

            // Wire validation and Enter-key handling to tbNewHierarchy
            tbNewHierarchy.KeyPress += TbNewHierarchy_KeyPress;
            tbNewHierarchy.KeyDown += TbNewHierarchy_KeyDown;

            // Position the scrollable container below any existing Designer controls in tpNewFilter
            int topOffset = 10;
            foreach (Control ctrl in tpNewFilter.Controls)
            {
                if (ctrl.Bottom + 10 > topOffset)
                {
                    topOffset = ctrl.Bottom + 10;
                }
            }

            Panel topDivider = new Panel
            {
                Location = new Point(0, topOffset - 2),
                Size = new Size(tpNewFilter.ClientSize.Width, 1),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.FromArgb(225, 225, 225)
            };

            pnlNewFilterMainFlow = new FlowLayoutPanel
            {
                Location = new Point(0, topOffset),
                Size = new Size(tpNewFilter.ClientSize.Width, Math.Max(120, tpNewFilter.ClientSize.Height - topOffset)),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(8),
                BackColor = Color.White
            };

            pnlNewFilterSections = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            pnlNewFilterMainFlow.Controls.Add(pnlNewFilterSections);

            pnlNewFilterApply = new Panel
            {
                Size = new Size(340, 48),
                Padding = new Padding(16, 10, 10, 0),
                Visible = false
            };

            Button btnApplyNewFilter = new Button
            {
                Text = "Apply",
                Location = new Point(16, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnApplyNewFilter.Click += BtnApplyNewFilter_Click;
            pnlNewFilterApply.Controls.Add(btnApplyNewFilter);
            pnlNewFilterMainFlow.Controls.Add(pnlNewFilterApply);

            tpNewFilter.Controls.Add(topDivider);
            tpNewFilter.Controls.Add(pnlNewFilterMainFlow);
        }

        // Allow only digits and control keys (e.g., Backspace) in tbNewHierarchy
        private void TbNewHierarchy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Pressing Enter in tbNewHierarchy creates the nested dropdowns AND generates the graph if a word is entered
        private async void TbNewHierarchy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (!TrySyncNewFilterDropdownsFromInput(out _, forceRegenerate: true))
                    return;

                string query = tbSynsetWord.Text.Trim();
                if (!string.IsNullOrEmpty(query))
                {
                    btGenGraph.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;

                    await RunPythonBackendAsync(query);

                    btGenGraph.Enabled = true;
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private bool TrySyncNewFilterDropdownsFromInput(out int? newHierarchyLevel, bool forceRegenerate = false)
        {
            InitializeNewFilterTabLayout();
            string rawText = tbNewHierarchy.Text.Trim();

            if (string.IsNullOrEmpty(rawText))
            {
                if (currentNewHierarchyCount != 0)
                {
                    GenerateNewFilterDropdowns(0);
                }
                newHierarchyLevel = null;
                return true;
            }

            if (int.TryParse(rawText, out int count) && count >= 0 && count <= MaxNewHierarchy)
            {
                if (forceRegenerate || count != currentNewHierarchyCount)
                {
                    GenerateNewFilterDropdowns(count);
                }
                newHierarchyLevel = count;
                return true;
            }

            MessageBox.Show(
                $"Please enter a valid number between 0 and {MaxNewHierarchy} in New Hierarchy.",
                "Invalid Input",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            tbNewHierarchy.Focus();
            tbNewHierarchy.SelectAll();

            newHierarchyLevel = null;
            return false;
        }

        private void DisposeNewFilterDropdowns()
        {
            foreach (var section in newFilterDropdownSections)
            {
                section.Dispose();
            }
            newFilterDropdownSections.Clear();
        }

        private void GenerateNewFilterDropdowns(int count)
        {
            InitializeNewFilterTabLayout();
            pnlNewFilterSections.SuspendLayout();

            DisposeNewFilterDropdowns();

            while (pnlNewFilterSections.Controls.Count > 0)
            {
                Control ctrl = pnlNewFilterSections.Controls[0];
                pnlNewFilterSections.Controls.RemoveAt(0);
                ctrl.Dispose();
            }

            currentNewHierarchyCount = count;

            if (count > 0)
            {
                int sectionWidth = Math.Max(320, pnlNewFilterMainFlow.ClientSize.Width - 28);

                // 1. Ancestors in descending order: Nth Ancestor -> ... -> 1st Ancestor
                for (int i = count; i >= 1; i--)
                {
                    string title = $"{GetOrdinal(i)} Ancestor";
                    DropdownSection section = CreateDropdownSection(title, true, i, sectionWidth);
                    newFilterDropdownSections.Add(section);

                    pnlNewFilterSections.Controls.Add(section.HeaderPanel);
                    pnlNewFilterSections.Controls.Add(section.ContentPanel);
                }

                // Visual gap between Ancestors and Descendants
                Panel groupSpacer = new Panel
                {
                    Size = new Size(sectionWidth, 16),
                    BackColor = Color.White,
                    Margin = new Padding(0)
                };
                pnlNewFilterSections.Controls.Add(groupSpacer);

                // 2. Descendants in ascending order: 1st Descendant -> ... -> Nth Descendant
                for (int i = 1; i <= count; i++)
                {
                    string title = $"{GetOrdinal(i)} Descendant";
                    DropdownSection section = CreateDropdownSection(title, false, i, sectionWidth);
                    newFilterDropdownSections.Add(section);

                    pnlNewFilterSections.Controls.Add(section.HeaderPanel);
                    pnlNewFilterSections.Controls.Add(section.ContentPanel);
                }
            }

            pnlNewFilterApply.Visible = count > 0;
            pnlNewFilterSections.ResumeLayout(true);
        }

        private DropdownSection CreateDropdownSection(string title, bool isAncestor, int level, int rowWidth)
        {
            var section = new DropdownSection
            {
                Title = title,
                IsAncestor = isAncestor,
                Level = level
            };

            // Content container
            section.ContentPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 6),
                Padding = new Padding(0)
            };

            // Header
            section.HeaderPanel = CreateHeaderRow(title, 0, rowWidth, out Label chevron, () =>
            {
                ToggleSection(section.ContentPanel, section.ChevronLabel);
            });
            section.ChevronLabel = chevron;

            // Row 1: Other Children TextBox
            section.ContentPanel.Controls.Add(
                CreateTextBoxRow("Other Children:", rowWidth, out CheckBox chkOtherChildren, out TextBox txtOtherChildren));
            section.ChkOtherChildren = chkOtherChildren;
            section.TxtOtherChildren = txtOtherChildren;

            // Row 2: Depth TextBox
            section.ContentPanel.Controls.Add(
                CreateTextBoxRow("Depth:", rowWidth, out CheckBox chkDepth, out TextBox txtDepth));
            section.ChkDepth = chkDepth;
            section.TxtDepth = txtDepth;

            // Row 3: 1-20 Multi-Select Dropdown
            section.ContentPanel.Controls.Add(CreateRangeDropdownRow(section, rowWidth));

            return section;
        }

        private Panel CreateHeaderRow(string title, int indentLeft, int rowWidth, out Label chevronLabel, Action onClick)
        {
            Panel row = new Panel
            {
                Size = new Size(rowWidth, 38),
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };

            Label lblIcon = new Label
            {
                Text = "−",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(18, 18),
                Location = new Point(indentLeft + 6, 10)
            };

            Label lblTitle = new Label
            {
                Text = title,
                AutoSize = true,
                Location = new Point(indentLeft + 32, 9),
                ForeColor = Color.FromArgb(50, 50, 50),
                Font = new Font("Segoe UI", 9.5f)
            };

            Label chevron = new Label
            {
                Text = "▲",
                AutoSize = true,
                Location = new Point(rowWidth - 24, 10),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8f)
            };
            chevronLabel = chevron;

            Panel divider = new Panel
            {
                Height = 1,
                Width = rowWidth - indentLeft,
                Location = new Point(indentLeft, 37),
                BackColor = Color.FromArgb(230, 230, 230)
            };

            EventHandler clickHandler = (s, e) => onClick?.Invoke();
            row.Click += clickHandler;
            lblIcon.Click += clickHandler;
            lblTitle.Click += clickHandler;
            chevron.Click += clickHandler;

            row.Controls.AddRange(new Control[] { lblIcon, lblTitle, chevron, divider });
            return row;
        }

        private void ToggleSection(Control contentPanel, Label chevron)
        {
            contentPanel.Visible = !contentPanel.Visible;
            chevron.Text = contentPanel.Visible ? "▲" : "▼";
        }

        private int GetFilterLabelColumnWidth(Font font)
        {
            // Measures the exact rendered pixel width at the current Windows DPI
            // plus room for the checkbox square and padding
            int textWidth = TextRenderer.MeasureText("Other Children:", font).Width;
            return textWidth + 32;
        }

        private Panel CreateTextBoxRow(string labelText, int rowWidth, out CheckBox checkBox, out TextBox textBox)
        {
            Font rowFont = new Font("Segoe UI", 9f);
            int labelWidth = GetFilterLabelColumnWidth(rowFont);
            int inputLeft = 20 + labelWidth + 6;
            int inputWidth = Math.Max(100, rowWidth - inputLeft - 15);

            Panel row = new Panel
            {
                Size = new Size(rowWidth, 40),
                BackColor = Color.White,
                Margin = new Padding(0)
            };

            CheckBox cb = new CheckBox
            {
                Text = labelText,
                AutoSize = true,
                Location = new Point(20, 10),
                Checked = true,
                Font = rowFont
            };

            TextBox tb = new TextBox
            {
                Location = new Point(inputLeft, 8),
                Width = inputWidth,
                Font = rowFont
            };

            cb.CheckedChanged += (s, e) => tb.Enabled = cb.Checked;

            checkBox = cb;
            textBox = tb;

            row.Controls.Add(cb);
            row.Controls.Add(tb);
            return row;
        }

        private Panel CreateRangeDropdownRow(DropdownSection section, int rowWidth)
        {
            Font rowFont = new Font("Segoe UI", 9f);
            int labelWidth = GetFilterLabelColumnWidth(rowFont);
            int inputLeft = 20 + labelWidth + 6;
            int inputWidth = Math.Max(100, rowWidth - inputLeft - 15);

            Panel row = new Panel
            {
                Size = new Size(rowWidth, 42),
                BackColor = Color.White,
                Margin = new Padding(0)
            };

            section.ChkRange = new CheckBox
            {
                Text = "Select:",
                AutoSize = true,
                Location = new Point(20, 10),
                Checked = true,
                Font = rowFont
            };

            section.BtnRangeDropdown = new Button
            {
                Text = "Choose 1–20 ▼",
                Location = new Point(inputLeft, 6),
                Size = new Size(inputWidth, 28),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Standard,
                Font = rowFont
            };

            section.ChkRange.CheckedChanged += (s, e) =>
                section.BtnRangeDropdown.Enabled = section.ChkRange.Checked;

            Panel popupPanel = new Panel
            {
                Size = new Size(inputWidth, 220),
                BackColor = Color.White
            };

            section.ChkSelectAll = new CheckBox
            {
                Text = "Select / Unselect All",
                Location = new Point(8, 6),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };

            section.ClbNumbers = new CheckedListBox
            {
                Location = new Point(4, 30),
                Size = new Size(inputWidth - 8, 184),
                CheckOnClick = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            for (int i = 1; i <= 20; i++)
            {
                section.ClbNumbers.Items.Add(i.ToString());
            }

            section.ChkSelectAll.CheckedChanged += (s, e) =>
            {
                if (section.IsUpdatingChecks) return;

                section.IsUpdatingChecks = true;
                for (int i = 0; i < section.ClbNumbers.Items.Count; i++)
                {
                    section.ClbNumbers.SetItemChecked(i, section.ChkSelectAll.Checked);
                }
                section.IsUpdatingChecks = false;

                section.UpdateRangeButtonText();
            };

            section.ClbNumbers.ItemCheck += (s, e) =>
            {
                if (section.IsUpdatingChecks) return;

                this.BeginInvoke((Action)(() =>
                {
                    section.IsUpdatingChecks = true;
                    section.ChkSelectAll.Checked =
                        (section.ClbNumbers.CheckedItems.Count == section.ClbNumbers.Items.Count);
                    section.IsUpdatingChecks = false;

                    section.UpdateRangeButtonText();
                }));
            };

            popupPanel.Controls.Add(section.ChkSelectAll);
            popupPanel.Controls.Add(section.ClbNumbers);

            ToolStripControlHost host = new ToolStripControlHost(popupPanel)
            {
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                AutoSize = false,
                Size = popupPanel.Size
            };

            section.RangePopup = new ToolStripDropDown
            {
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                AutoClose = true
            };
            section.RangePopup.Items.Add(host);

            section.BtnRangeDropdown.Click += (s, e) =>
            {
                section.RangePopup.Show(
                    section.BtnRangeDropdown,
                    new Point(0, section.BtnRangeDropdown.Height));
            };

            row.Controls.Add(section.ChkRange);
            row.Controls.Add(section.BtnRangeDropdown);
            return row;
        }

        private string BuildNewFilterConfigBase64(int hierarchyLevel)
        {
            string FormatSectionJson(DropdownSection section)
            {
                bool showOther = section.ChkOtherChildren.Checked;
                string otherChildrenVal = "null"; // Blank = unlimited (show all siblings/other children)
                if (showOther && int.TryParse(section.TxtOtherChildren.Text.Trim(), out int parsedOther) && parsedOther >= 0)
                {
                    otherChildrenVal = parsedOther.ToString();
                }

                int depthVal = 0; // Blank or unchecked = 0 (do not expand lateral branches downward)
                if (section.ChkDepth.Checked && int.TryParse(section.TxtDepth.Text.Trim(), out int parsedDepth) && parsedDepth >= 0)
                {
                    depthVal = parsedDepth;
                }

                return $"\"{section.Level}\":{{\"show_other_children\":{(showOther ? "true" : "false")},\"other_children\":{otherChildrenVal},\"depth\":{depthVal}}}";
            }

            var ancEntries = newFilterDropdownSections
                .Where(s => s.IsAncestor)
                .Select(FormatSectionJson);

            var descEntries = newFilterDropdownSections
                .Where(s => !s.IsAncestor)
                .Select(FormatSectionJson);

            string json = $"{{\"hierarchy\":{hierarchyLevel}," +
                          $"\"ancestors\":{{{string.Join(",", ancEntries)}}}," +
                          $"\"descendants\":{{{string.Join(",", descEntries)}}}}}";

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

        private async void BtnApplyNewFilter_Click(object sender, EventArgs e)
        {
            string query = tbSynsetWord.Text.Trim();
            if (string.IsNullOrEmpty(query))
            {
                MessageBox.Show("Please enter an English word, Thai word, or Offset first.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TrySyncNewFilterDropdownsFromInput(out _))
                return;

            btGenGraph.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            await RunPythonBackendAsync(query);

            btGenGraph.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private static string GetOrdinal(int number)
        {
            int mod100 = number % 100;
            if (mod100 >= 11 && mod100 <= 13)
                return $"{number}th";

            switch (number % 10)
            {
                case 1: return $"{number}st";
                case 2: return $"{number}nd";
                case 3: return $"{number}rd";
                default: return $"{number}th";
            }
        }

        private class DropdownSection : IDisposable
        {
            public string Title { get; set; }
            public bool IsAncestor { get; set; }
            public int Level { get; set; }

            public Panel HeaderPanel { get; set; }
            public Panel ContentPanel { get; set; }
            public Label ChevronLabel { get; set; }

            public CheckBox ChkOtherChildren { get; set; }
            public TextBox TxtOtherChildren { get; set; }

            public CheckBox ChkDepth { get; set; }
            public TextBox TxtDepth { get; set; }

            public CheckBox ChkRange { get; set; }
            public Button BtnRangeDropdown { get; set; }
            public ToolStripDropDown RangePopup { get; set; }
            public CheckBox ChkSelectAll { get; set; }
            public CheckedListBox ClbNumbers { get; set; }
            public bool IsUpdatingChecks { get; set; }

            public void UpdateRangeButtonText()
            {
                int count = ClbNumbers.CheckedItems.Count;
                if (count == 0)
                    BtnRangeDropdown.Text = "Choose 1–20 ▼";
                else if (count == ClbNumbers.Items.Count)
                    BtnRangeDropdown.Text = "All Selected (1–20) ▼";
                else
                    BtnRangeDropdown.Text = $"{count} Selected ▼";
            }

            public void Dispose()
            {
                RangePopup?.Dispose();
                HeaderPanel?.Dispose();
                ContentPanel?.Dispose();
            }
        }

        // ==========================================
        // ENTITIES TAB (tpEntities) LOGIC
        // ==========================================
        private void InitializeEntitiesTabLayout()
        {
            if (pnlEntityButtons != null) return;

            tpEntities.Controls.Clear();

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
                DetectUrls = false,
                Text = "Select any synset button above to inspect its full definition and lexical links."
            };

            rtbEntityDetails.MouseMove += RtbEntityDetails_MouseMove;
            rtbEntityDetails.MouseDown += RtbEntityDetails_MouseDown;

            pnlEntityDetails.Controls.Add(rtbEntityDetails);
            pnlEntityDetails.Controls.Add(lblDetailsHeader);

            pnlEntityButtons = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(6)
            };

            tpEntities.Controls.Add(pnlEntityButtons);
            tpEntities.Controls.Add(pnlEntityDetails);
        }

        private string GetOffsetAtCursor(Point mouseLocation)
        {
            if (string.IsNullOrEmpty(rtbEntityDetails.Text)) return null;

            int charIndex = rtbEntityDetails.GetCharIndexFromPosition(mouseLocation);

            Point charPos = rtbEntityDetails.GetPositionFromCharIndex(charIndex);
            if (Math.Abs(mouseLocation.X - charPos.X) > 20 || Math.Abs(mouseLocation.Y - charPos.Y) > 20)
                return null;

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
                        return m.Groups[1].Value;
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
            if (!TrySyncNewFilterDropdownsFromInput(out _))
                return;

            tbSynsetWord.Text = targetOffset;
            btGenGraph.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            await RunPythonBackendAsync(targetOffset);

            btGenGraph.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void modified_111126_Parent_Child_Of_Noun_KeyDown(object sender, KeyEventArgs e)
        {
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

            // Build or update nested dropdowns in tpNewFilter according to tbNewHierarchy
            if (!TrySyncNewFilterDropdownsFromInput(out _))
                return;

            btGenGraph.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            await RunPythonBackendAsync(query);

            btGenGraph.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private async Task RunPythonBackendAsync(string query)
        {
            int newHVal = -1;
            bool hasNewHierarchyInput = !string.IsNullOrWhiteSpace(tbNewHierarchy.Text) &&
                                        int.TryParse(tbNewHierarchy.Text.Trim(), out newHVal) &&
                                        newHVal >= 0;

            int oldHVal = -1;
            bool hasOldHierarchyInput = !string.IsNullOrWhiteSpace(tbHierarchy.Text) &&
                                        int.TryParse(tbHierarchy.Text.Trim(), out oldHVal) &&
                                        oldHVal > 0;

            string GetLimit(TextBox tb, string defaultIfEmpty = "0")
            {
                if (string.IsNullOrWhiteSpace(tb.Text)) return defaultIfEmpty;

                if (int.TryParse(tb.Text.Trim(), out int val))
                {
                    return val.ToString();
                }
                return defaultIfEmpty;
            }

            string args;
            if (hasNewHierarchyInput)
            {
                string configBase64 = BuildNewFilterConfigBase64(newHVal);
                args = $"\"{jsonFile}\" \"{query}\" --new-filter {configBase64}";
            }
            else
            {
                string defaultDownwardLimit = hasOldHierarchyInput ? "-1" : "0";

                args = $"\"{jsonFile}\" \"{query}\" " +
                       $"{GetLimit(tbMaxChild, defaultDownwardLimit)} " +
                       $"{GetLimit(tbMaxGrandChild, defaultDownwardLimit)} " +
                       $"{GetLimit(tbMaxGreatGrandChild, defaultDownwardLimit)} " +
                       $"{GetLimit(tbMaxSibling, "0")} " +
                       $"{GetLimit(tbMaxNephew, "0")} " +
                       $"{GetLimit(tbMaxUncleAunt, "0")} " +
                       $"{GetLimit(tbMaxCousin, "0")} " +
                       $"{GetLimit(tbMaxGreatUncleGreatAunt, "0")} " +
                       $"{GetLimit(tbMaxFirstCousin, "0")} " +
                       $"{GetLimit(tbHierarchy, "-1")}";
            }

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

                    PopulateEntityButtons(displayedSynsets);

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
                    BackColor = i == 0 ? Color.FromArgb(255, 224, 178) : Color.FromArgb(249, 249, 249),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderColor = Color.LightGray;

                btn.Click += (s, e) =>
                {
                    if (s is Button clickedBtn && clickedBtn.Tag is SynsetEntityInfo selectedInfo)
                    {
                        ShowSynsetDetails(selectedInfo, clickedBtn);
                    }
                };

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

                if (i == 0)
                {
                    ShowSynsetDetails(info, btn);
                }
            }
        }

        private void ShowSynsetDetails(SynsetEntityInfo info, Button activeBtn)
        {
            foreach (Control ctrl in pnlEntityButtons.Controls)
            {
                if (ctrl is Button b)
                {
                    b.FlatAppearance.BorderSize = (b == activeBtn) ? 2 : 1;
                    b.FlatAppearance.BorderColor = (b == activeBtn) ? Color.FromArgb(230, 126, 34) : Color.LightGray;
                }
            }

            rtbEntityDetails.Text =
                $"Offset: {info.Offset}\n" +
                $"Thai Words: {info.ThaiWords}   |   Source: {info.Source}\n" +
                $"English Words: {info.EnglishWords}\n" +
                $"Gloss: {info.Gloss}\n" +
                $"Hypernyms (Parents): {info.Hypernyms}\n" +
                $"Hyponyms (Children): {info.Hyponyms}";

            rtbEntityDetails.SelectAll();
            rtbEntityDetails.SelectionColor = Color.Black;
            rtbEntityDetails.SelectionFont = new Font("Tahoma", 9.5f, FontStyle.Regular);

            Match offsetMatch = Regex.Match(rtbEntityDetails.Text, @"^Offset:\s*(\d{8})");
            if (offsetMatch.Success)
            {
                rtbEntityDetails.Select(offsetMatch.Groups[1].Index, offsetMatch.Groups[1].Length);
                rtbEntityDetails.SelectionColor = Color.FromArgb(0, 102, 204);
                rtbEntityDetails.SelectionFont = new Font("Tahoma", 9.5f, FontStyle.Underline);
            }

            int hyperStartIndex = rtbEntityDetails.Text.IndexOf("Hypernyms (Parents):");
            if (hyperStartIndex != -1)
            {
                MatchCollection matches = Regex.Matches(rtbEntityDetails.Text.Substring(hyperStartIndex), @"\d{8}-[^\s,\)]+");
                foreach (Match m in matches)
                {
                    rtbEntityDetails.Select(hyperStartIndex + m.Index, m.Length);
                    rtbEntityDetails.SelectionColor = Color.FromArgb(0, 102, 204);
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