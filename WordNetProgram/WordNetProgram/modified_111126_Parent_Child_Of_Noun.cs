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

        // Data structure for each item inside the Synset multi-select dropdown
        private class SynsetDropdownItem
        {
            public string Offset { get; set; }
            public string DisplayText { get; set; }
            public override string ToString() => DisplayText;
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

        private class SavedDropdownState
        {
            public bool IsExpanded { get; set; } = true;

            // Ancestor state
            public bool OtherChildrenChecked { get; set; } = true;
            public string OtherChildrenText { get; set; } = "0";
            public bool DepthChecked { get; set; } = true;
            public string DepthText { get; set; } = "";
            public bool ApplyDepthChecked { get; set; } = true;
            public List<SynsetDropdownItem> SynsetItems { get; set; } = new List<SynsetDropdownItem>();
            public HashSet<string> CheckedOffsets { get; set; } = new HashSet<string>();

            // Descendant state
            public bool NumberOfDescendantChecked { get; set; } = true;
            public string NumberOfDescendantText { get; set; } = "";
        }

        private readonly Dictionary<(bool IsAncestor, int Level), SavedDropdownState> savedFilterStates = new Dictionary<(bool IsAncestor, int Level), SavedDropdownState>();

        // Define paths (Use backend4.py if you want SVG-only generation without the 10-node limit)
        //private string pythonScript = @"C:\Users\phumi\OneDrive\Desktop\Add_Thai_To_WordNet_V5(2_Sep_2026)\5.Test_Gen_Graph_14-Sep-2026\11-Oct-2026_WinForm2\backend5.py";
        // default backend path private string pythonScript = System.IO.Path.Combine(AppContext.BaseDirectory, "parent_child_backend5.py");
        private string pythonScript = @"C:\Users\phumi\OneDrive\Desktop\Sem2\ReadWordNetFile\WordNetProgram\WordNetProgram\can_enter_hierarchy_number_parent_child_backend5.py";
        //private string jsonFile = @"C:\Users\phumi\OneDrive\Desktop\Inputs_of_Merge_Program\No_Gemini_Merged_Thai_Noun.json";
        //private string jsonFile = @"C:\Users\phumi\OneDrive\Desktop\Add_Thai_To_WordNet_V5(2_Sep_2026)\5.Test_Gen_Graph_14-Sep-2026\11-Oct-2026_WinForm2\Test_Case_Mammal_to_Offspring_of_Working_dog.noun.json";
        //private string jsonFile = @"C:\Users\phumi\OneDrive\Desktop\Sem2\ReadWordNetFile\WordNetProgram\WordNetProgram\Test_Case_Whole_to_Watch_dog.json";
        private string jsonFile = System.IO.Path.Combine(AppContext.BaseDirectory, "Data", "merged_data", "merged_JSON", "Merged_Thai_Noun.json");

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

            // Default to 0 when tbNewHierarchy is left blank
            if (string.IsNullOrEmpty(rawText))
            {
                if (currentNewHierarchyCount != 0)
                {
                    GenerateNewFilterDropdowns(0);
                }
                newHierarchyLevel = 0;
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

        private void SaveCurrentNewFilterStates()
        {
            foreach (var section in newFilterDropdownSections)
            {
                var key = (section.IsAncestor, section.Level);
                var state = new SavedDropdownState
                {
                    IsExpanded = section.ContentPanel?.Visible ?? true
                };

                if (section.IsAncestor)
                {
                    state.OtherChildrenChecked = section.ChkOtherChildren.Checked;
                    state.OtherChildrenText = section.TxtOtherChildren.Text;
                    state.DepthChecked = section.ChkDepth.Checked;
                    state.DepthText = section.TxtDepth.Text;
                    state.ApplyDepthChecked = section.ChkSynsetToApplyDepth.Checked;

                    state.SynsetItems = section.ClbSynsetsToApplyDepth.Items
                        .OfType<SynsetDropdownItem>()
                        .Select(x => new SynsetDropdownItem { Offset = x.Offset, DisplayText = x.DisplayText })
                        .ToList();

                    state.CheckedOffsets = new HashSet<string>(
                        section.ClbSynsetsToApplyDepth.CheckedItems
                            .OfType<SynsetDropdownItem>()
                            .Select(x => x.Offset)
                    );
                }
                else
                {
                    state.NumberOfDescendantChecked = section.ChkNumberOfDescendant.Checked;
                    state.NumberOfDescendantText = section.TbNumberOfDescendant.Text;
                }

                savedFilterStates[key] = state;
            }
        }

        private void RestoreSectionState(DropdownSection section)
        {
            var key = (section.IsAncestor, section.Level);
            if (!savedFilterStates.TryGetValue(key, out var state)) return;

            section.ContentPanel.Visible = state.IsExpanded;
            section.ChevronLabel.Text = state.IsExpanded ? "▲" : "▼";

            if (section.IsAncestor)
            {
                section.ChkOtherChildren.Checked = state.OtherChildrenChecked;
                section.TxtOtherChildren.Text = state.OtherChildrenText;

                section.ChkDepth.Checked = state.DepthChecked;
                section.TxtDepth.Text = state.DepthText;

                section.ChkSynsetToApplyDepth.Checked = state.ApplyDepthChecked;

                section.IsUpdatingChecks = true;
                section.ClbSynsetsToApplyDepth.Items.Clear();
                foreach (var item in state.SynsetItems)
                {
                    bool isChecked = state.CheckedOffsets.Contains(item.Offset);
                    section.ClbSynsetsToApplyDepth.Items.Add(item, isChecked);
                }
                section.ChkSelectAll.Checked = (section.ClbSynsetsToApplyDepth.Items.Count > 0 &&
                                                section.ClbSynsetsToApplyDepth.CheckedItems.Count == section.ClbSynsetsToApplyDepth.Items.Count);
                section.IsUpdatingChecks = false;
                section.UpdateSynsetToApplyDepthButtonText();
            }
            else
            {
                section.ChkNumberOfDescendant.Checked = state.NumberOfDescendantChecked;
                section.TbNumberOfDescendant.Text = state.NumberOfDescendantText;
            }
        }

        private void GenerateNewFilterDropdowns(int count)
        {
            InitializeNewFilterTabLayout();
            pnlNewFilterSections.SuspendLayout();

            // Save existing inputs before disposing controls
            SaveCurrentNewFilterStates();

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
                    RestoreSectionState(section);
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
                    RestoreSectionState(section);
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

            if (isAncestor)
            {
                // Row 1: Other Children TextBox (pre-filled with "0")
                section.ContentPanel.Controls.Add(
                    CreateTextBoxRow("Other Children:", rowWidth, out CheckBox chkOtherChildren, out TextBox txtOtherChildren));
                txtOtherChildren.Text = "0";
                section.ChkOtherChildren = chkOtherChildren;
                section.TxtOtherChildren = txtOtherChildren;

                // Row 2: Depth TextBox
                section.ContentPanel.Controls.Add(
                    CreateTextBoxRow("Depth:", rowWidth, out CheckBox chkDepth, out TextBox txtDepth));
                section.ChkDepth = chkDepth;
                section.TxtDepth = txtDepth;

                // Row 3: Synset to Apply Depth Multi-Select Dropdown
                section.ContentPanel.Controls.Add(CreateSynsetToApplyDepthDropdownRow(section, rowWidth));
            }
            else
            {
                // Descendant Row: Number of Descendant TextBox
                section.ContentPanel.Controls.Add(
                    CreateTextBoxRow(
                        "Number of Descendant:",
                        rowWidth,
                        out CheckBox chkNumberOfDescendant,
                        out TextBox tbNumberOfDescendant,
                        widthSampleText: "Number of Descendant:"));
                section.ChkNumberOfDescendant = chkNumberOfDescendant;
                section.TbNumberOfDescendant = tbNumberOfDescendant;
            }

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

        private int GetFilterLabelColumnWidth(Font font, string sampleText = "Other Children:")
        {
            int textWidth = TextRenderer.MeasureText(sampleText, font).Width;
            return textWidth + 32;
        }

        private Panel CreateTextBoxRow(
    string labelText,
    int rowWidth,
    out CheckBox checkBox,
    out TextBox textBox,
    string widthSampleText = "Other Children:")
        {
            Font rowFont = new Font("Segoe UI", 9f);
            int labelWidth = GetFilterLabelColumnWidth(rowFont, widthSampleText);
            int inputLeft = 20 + labelWidth + 6;
            int inputWidth = Math.Max(80, rowWidth - inputLeft - 15);

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

        private Panel CreateSynsetToApplyDepthDropdownRow(DropdownSection section, int rowWidth)
        {
            Font rowFont = new Font("Segoe UI", 9f);
            int labelWidth = GetFilterLabelColumnWidth(rowFont);
            int inputLeft = 20 + labelWidth + 6;
            int inputWidth = Math.Max(100, rowWidth - inputLeft - 15);
            int popupWidth = Math.Max(inputWidth, 340);

            Panel row = new Panel
            {
                Size = new Size(rowWidth, 42),
                BackColor = Color.White,
                Margin = new Padding(0)
            };

            section.ChkSynsetToApplyDepth = new CheckBox
            {
                Text = "Apply Depth:",
                AutoSize = true,
                Location = new Point(20, 10),
                Checked = true,
                Font = rowFont
            };

            section.BtnSynsetToApplyDepth = new Button
            {
                Text = "Choose Synset to Apply Depth ▼",
                Location = new Point(inputLeft, 6),
                Size = new Size(inputWidth, 28),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Standard,
                Font = rowFont
            };

            section.ChkSynsetToApplyDepth.CheckedChanged += (s, e) =>
                section.BtnSynsetToApplyDepth.Enabled = section.ChkSynsetToApplyDepth.Checked;

            Panel popupPanel = new Panel
            {
                Size = new Size(popupWidth, 220),
                BackColor = Color.White
            };

            section.ChkSelectAll = new CheckBox
            {
                Text = "Select / Unselect All",
                Location = new Point(8, 6),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };

            section.ClbSynsetsToApplyDepth = new CheckedListBox
            {
                Location = new Point(4, 30),
                Size = new Size(popupWidth - 8, 184),
                CheckOnClick = true,
                HorizontalScrollbar = true,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Tahoma", 9f)
            };

            section.ChkSelectAll.CheckedChanged += (s, e) =>
            {
                if (section.IsUpdatingChecks) return;

                section.IsUpdatingChecks = true;
                for (int i = 0; i < section.ClbSynsetsToApplyDepth.Items.Count; i++)
                {
                    section.ClbSynsetsToApplyDepth.SetItemChecked(i, section.ChkSelectAll.Checked);
                }
                section.IsUpdatingChecks = false;

                section.UpdateSynsetToApplyDepthButtonText();
            };

            section.ClbSynsetsToApplyDepth.ItemCheck += (s, e) =>
            {
                if (section.IsUpdatingChecks) return;

                this.BeginInvoke((Action)(() =>
                {
                    section.IsUpdatingChecks = true;
                    section.ChkSelectAll.Checked =
                        (section.ClbSynsetsToApplyDepth.Items.Count > 0 &&
                         section.ClbSynsetsToApplyDepth.CheckedItems.Count == section.ClbSynsetsToApplyDepth.Items.Count);
                    section.IsUpdatingChecks = false;

                    section.UpdateSynsetToApplyDepthButtonText();
                }));
            };

            popupPanel.Controls.Add(section.ChkSelectAll);
            popupPanel.Controls.Add(section.ClbSynsetsToApplyDepth);

            ToolStripControlHost host = new ToolStripControlHost(popupPanel)
            {
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                AutoSize = false,
                Size = popupPanel.Size
            };

            section.SynsetToApplyDepthPopup = new ToolStripDropDown
            {
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                AutoClose = true
            };
            section.SynsetToApplyDepthPopup.Items.Add(host);

            section.BtnSynsetToApplyDepth.Click += (s, e) =>
            {
                section.SynsetToApplyDepthPopup.Show(
                    section.BtnSynsetToApplyDepth,
                    new Point(0, section.BtnSynsetToApplyDepth.Height));
            };

            row.Controls.Add(section.ChkSynsetToApplyDepth);
            row.Controls.Add(section.BtnSynsetToApplyDepth);
            return row;
        }

        private string BuildNewFilterConfigBase64(int hierarchyLevel)
        {
            string FormatAncestorJson(DropdownSection section)
            {
                bool showOther = section.ChkOtherChildren.Checked;
                string otherChildrenVal = "null"; // Blank = unlimited
                if (showOther && int.TryParse(section.TxtOtherChildren.Text.Trim(), out int parsedOther) && parsedOther >= 0)
                {
                    otherChildrenVal = parsedOther.ToString();
                }

                int depthVal = 0; // Blank or unchecked = 0
                if (section.ChkDepth.Checked && int.TryParse(section.TxtDepth.Text.Trim(), out int parsedDepth) && parsedDepth >= 0)
                {
                    depthVal = parsedDepth;
                }

                var depthTargetOffsets = new List<string>();
                if (section.ChkSynsetToApplyDepth.Checked)
                {
                    foreach (var item in section.ClbSynsetsToApplyDepth.CheckedItems)
                    {
                        if (item is SynsetDropdownItem synItem)
                        {
                            depthTargetOffsets.Add($"\"{synItem.Offset}\"");
                        }
                    }
                }
                string depthTargetsJson = $"[{string.Join(",", depthTargetOffsets)}]";

                return $"\"{section.Level}\":{{\"show_other_children\":{(showOther ? "true" : "false")},\"other_children\":{otherChildrenVal},\"depth\":{depthVal},\"depth_target_offsets\":{depthTargetsJson}}}";
            }

            string FormatDescendantJson(DropdownSection section)
            {
                bool showDescendants = section.ChkNumberOfDescendant.Checked;
                string numDescVal = "null"; // Blank = unlimited
                if (showDescendants && int.TryParse(section.TbNumberOfDescendant.Text.Trim(), out int parsedNum) && parsedNum >= 0)
                {
                    numDescVal = parsedNum.ToString();
                }

                return $"\"{section.Level}\":{{\"show_descendants\":{(showDescendants ? "true" : "false")},\"number_of_descendant\":{numDescVal}}}";
            }

            var ancEntries = newFilterDropdownSections
                .Where(s => s.IsAncestor)
                .Select(FormatAncestorJson);

            var descEntries = newFilterDropdownSections
                .Where(s => !s.IsAncestor)
                .Select(FormatDescendantJson);

            string json = $"{{\"hierarchy\":{hierarchyLevel}," +
                          $"\"ancestors\":{{{string.Join(",", ancEntries)}}}," +
                          $"\"descendants\":{{{string.Join(",", descEntries)}}}}}";

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

        private void PopulateNewFilterDropdownOptions(string pythonOutput)
        {
            var optionsBySection = new Dictionary<(bool IsAncestor, int Level), List<SynsetDropdownItem>>();

            if (pythonOutput.Contains("DROPDOWN_OPTIONS_START"))
            {
                string[] lines = pythonOutput.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                bool capture = false;

                foreach (string line in lines)
                {
                    if (line == "DROPDOWN_OPTIONS_END") capture = false;
                    if (capture)
                    {
                        string[] fields = line.Split('|');
                        if (fields.Length >= 4 && int.TryParse(fields[1], out int level))
                        {
                            bool isAncestor = fields[0].Trim().Equals("ANC", StringComparison.OrdinalIgnoreCase);
                            var key = (isAncestor, level);

                            if (!optionsBySection.ContainsKey(key))
                                optionsBySection[key] = new List<SynsetDropdownItem>();

                            optionsBySection[key].Add(new SynsetDropdownItem
                            {
                                Offset = fields[2].Trim(),
                                DisplayText = fields[3].Trim()
                            });
                        }
                    }
                    if (line == "DROPDOWN_OPTIONS_START") capture = true;
                }
            }

            foreach (var section in newFilterDropdownSections)
            {
                if (!section.IsAncestor) continue;

                var key = (section.IsAncestor, section.Level);
                var items = optionsBySection.TryGetValue(key, out var list)
                    ? list
                    : new List<SynsetDropdownItem>();

                section.PopulateSynsetOptions(items);
            }
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

            // Ancestor controls
            public CheckBox ChkOtherChildren { get; set; }
            public TextBox TxtOtherChildren { get; set; }

            public CheckBox ChkDepth { get; set; }
            public TextBox TxtDepth { get; set; }

            public CheckBox ChkSynsetToApplyDepth { get; set; }
            public Button BtnSynsetToApplyDepth { get; set; }
            public ToolStripDropDown SynsetToApplyDepthPopup { get; set; }
            public CheckBox ChkSelectAll { get; set; }
            public CheckedListBox ClbSynsetsToApplyDepth { get; set; }
            public bool IsUpdatingChecks { get; set; }

            // Descendant controls
            public CheckBox ChkNumberOfDescendant { get; set; }
            public TextBox TbNumberOfDescendant { get; set; }

            public void UpdateSynsetToApplyDepthButtonText()
            {
                if (ClbSynsetsToApplyDepth == null || BtnSynsetToApplyDepth == null) return;

                int count = ClbSynsetsToApplyDepth.CheckedItems.Count;
                if (count == 0)
                    BtnSynsetToApplyDepth.Text = "Choose Synset to Apply Depth ▼";
                else if (count == ClbSynsetsToApplyDepth.Items.Count)
                    BtnSynsetToApplyDepth.Text = "All Selected ▼";
                else
                    BtnSynsetToApplyDepth.Text = $"{count} Selected ▼";
            }

            public void PopulateSynsetOptions(List<SynsetDropdownItem> items)
            {
                if (ClbSynsetsToApplyDepth == null || ChkSelectAll == null) return;

                var previouslyChecked = new HashSet<string>(
                    ClbSynsetsToApplyDepth.CheckedItems
                        .OfType<SynsetDropdownItem>()
                        .Select(x => x.Offset)
                );

                IsUpdatingChecks = true;
                ClbSynsetsToApplyDepth.Items.Clear();

                foreach (var item in items)
                {
                    bool isChecked = previouslyChecked.Contains(item.Offset);
                    ClbSynsetsToApplyDepth.Items.Add(item, isChecked);
                }

                ChkSelectAll.Checked = (ClbSynsetsToApplyDepth.Items.Count > 0 &&
                                        ClbSynsetsToApplyDepth.CheckedItems.Count == ClbSynsetsToApplyDepth.Items.Count);
                IsUpdatingChecks = false;

                UpdateSynsetToApplyDepthButtonText();
            }

            public void Dispose()
            {
                SynsetToApplyDepthPopup?.Dispose();
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
            // Default to 0 when tbNewHierarchy is left blank
            int newHVal = 0;
            if (!string.IsNullOrWhiteSpace(tbNewHierarchy.Text) &&
                int.TryParse(tbNewHierarchy.Text.Trim(), out int parsedNewH) &&
                parsedNewH >= 0)
            {
                newHVal = parsedNewH;
            }

            string configBase64 = BuildNewFilterConfigBase64(newHVal);
            string args = $"\"{jsonFile}\" \"{query}\" --new-filter {configBase64}";

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
                    PopulateNewFilterDropdownOptions(output);

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