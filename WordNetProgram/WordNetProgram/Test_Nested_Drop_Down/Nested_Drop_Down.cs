using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WordNetProgram.Test_Nested_Drop_Down
{
    public partial class Nested_Drop_Down : Form
    {
        private TextBox tbAncestorCount;
        private Button btnEnter;
        private FlowLayoutPanel sectionsContainer;
        private Panel pnlApply;

        // Tracks each dynamically created ancestor and descendant section
        private readonly List<DropdownSection> dropdownSections = new List<DropdownSection>();

        // Safety cap to prevent freezing the UI if a huge number is entered
        private const int MaxAncestors = 20;

        public Nested_Drop_Down()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Collapsible Dropdown";
            this.Size = new Size(430, 520);
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5f);

            // Top bar for entering the number of nested dropdowns
            Panel topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                Padding = new Padding(12, 10, 12, 8),
                BackColor = Color.FromArgb(248, 248, 248)
            };

            Label lblCount = new Label
            {
                Text = "Number of Ancestors:",
                AutoSize = true,
                Location = new Point(12, 15),
                ForeColor = Color.FromArgb(50, 50, 50)
            };

            tbAncestorCount = new TextBox
            {
                Location = new Point(200, 12),
                Width = 55,
                Text = "1"
            };
            tbAncestorCount.KeyPress += TbAncestorCount_KeyPress;
            tbAncestorCount.KeyDown += TbAncestorCount_KeyDown;

            btnEnter = new Button
            {
                Text = "Enter",
                Location = new Point(262, 10),
                Size = new Size(65, 27),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEnter.Click += BtnEnter_Click;

            Label lblHint = new Label
            {
                Text = $"(0–{MaxAncestors})",
                AutoSize = true,
                Location = new Point(334, 15),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8.5f)
            };

            Panel topDivider = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = Color.FromArgb(225, 225, 225)
            };

            topBar.Controls.AddRange(new Control[] { lblCount, tbAncestorCount, btnEnter, lblHint, topDivider });

            // Main scrollable vertical container
            FlowLayoutPanel mainFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            // Container that holds all generated sections
            sectionsContainer = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            mainFlow.Controls.Add(sectionsContainer);

            // Shared Apply Button at the bottom
            pnlApply = new Panel
            {
                Size = new Size(360, 48),
                Padding = new Padding(24, 10, 10, 0)
            };
            Button btnApply = new Button
            {
                Text = "Apply",
                Location = new Point(24, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnApply.Click += BtnApply_Click;
            pnlApply.Controls.Add(btnApply);
            mainFlow.Controls.Add(pnlApply);

            // Add mainFlow first, then topBar so DockStyle.Fill respects DockStyle.Top
            this.Controls.Add(mainFlow);
            this.Controls.Add(topBar);

            // Initial render for default value "1"
            GenerateDropdowns(1);
        }

        // Allow only digits and control keys (e.g., Backspace)
        private void TbAncestorCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Also trigger Enter button when pressing the Enter key inside the TextBox
        private void TbAncestorCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnEnter_Click(sender, EventArgs.Empty);
            }
        }

        private void BtnEnter_Click(object sender, EventArgs e)
        {
            if (int.TryParse(tbAncestorCount.Text.Trim(), out int count) && count >= 0 && count <= MaxAncestors)
            {
                GenerateDropdowns(count);
            }
            else
            {
                MessageBox.Show(
                    $"Please enter a valid number between 0 and {MaxAncestors}.",
                    "Invalid Input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                tbAncestorCount.Focus();
                tbAncestorCount.SelectAll();
            }
        }

        private void GenerateDropdowns(int count)
        {
            sectionsContainer.SuspendLayout();

            // Dispose existing popups and controls cleanly
            foreach (var section in dropdownSections)
            {
                section.Dispose();
            }
            dropdownSections.Clear();

            while (sectionsContainer.Controls.Count > 0)
            {
                Control ctrl = sectionsContainer.Controls[0];
                sectionsContainer.Controls.RemoveAt(0);
                ctrl.Dispose();
            }

            if (count > 0)
            {
                // 1. Ancestors in descending order: Nth Ancestor -> ... -> 1st Ancestor
                for (int i = count; i >= 1; i--)
                {
                    string title = $"{GetOrdinal(i)} Ancestor";
                    DropdownSection section = CreateDropdownSection(title);
                    dropdownSections.Add(section);

                    sectionsContainer.Controls.Add(section.HeaderPanel);
                    sectionsContainer.Controls.Add(section.ContentPanel);
                }

                // Visual gap between Ancestors and Descendants
                Panel groupSpacer = new Panel
                {
                    Size = new Size(360, 16),
                    BackColor = Color.White,
                    Margin = new Padding(0)
                };
                sectionsContainer.Controls.Add(groupSpacer);

                // 2. Descendants in ascending order: 1st Descendant -> ... -> Nth Descendant
                for (int i = 1; i <= count; i++)
                {
                    string title = $"{GetOrdinal(i)} Descendant";
                    DropdownSection section = CreateDropdownSection(title);
                    dropdownSections.Add(section);

                    sectionsContainer.Controls.Add(section.HeaderPanel);
                    sectionsContainer.Controls.Add(section.ContentPanel);
                }
            }

            pnlApply.Visible = count > 0;
            sectionsContainer.ResumeLayout(true);
        }

        private DropdownSection CreateDropdownSection(string title)
        {
            var section = new DropdownSection { Title = title };

            // Header
            section.HeaderPanel = CreateHeaderRow(title, 0, out Label chevron);
            section.ChevronLabel = chevron;

            // Content container
            section.ContentPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 6),
                Padding = new Padding(0)
            };

            section.HeaderPanel.Click += (s, e) => ToggleSection(section.ContentPanel, section.ChevronLabel);

            // Row 1: Sibling TextBox
            section.ContentPanel.Controls.Add(
                CreateTextBoxRow("Sibling:", out CheckBox chkSibling, out TextBox txtSibling));
            section.ChkSibling = chkSibling;
            section.TxtSibling = txtSibling;

            // Row 2: Depth TextBox
            section.ContentPanel.Controls.Add(
                CreateTextBoxRow("Depth:", out CheckBox chkDepth, out TextBox txtDepth));
            section.ChkDepth = chkDepth;
            section.TxtDepth = txtDepth;

            // Row 3: 1-20 Multi-Select Dropdown
            section.ContentPanel.Controls.Add(CreateRangeDropdownRow(section));

            return section;
        }

        private Panel CreateHeaderRow(string title, int indentLeft, out Label chevronLabel)
        {
            Panel row = new Panel
            {
                Size = new Size(360, 38),
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
                ForeColor = Color.FromArgb(50, 50, 50)
            };

            Label chevron = new Label
            {
                Text = "▲",
                AutoSize = true,
                Location = new Point(335, 10),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8f)
            };
            chevronLabel = chevron;

            Panel divider = new Panel
            {
                Height = 1,
                Width = 360 - indentLeft,
                Location = new Point(indentLeft, 37),
                BackColor = Color.FromArgb(230, 230, 230)
            };

            EventHandler forwardClick = (s, e) => OnRowClick(row);
            lblIcon.Click += forwardClick;
            lblTitle.Click += forwardClick;
            chevron.Click += forwardClick;

            row.Controls.AddRange(new Control[] { lblIcon, lblTitle, chevron, divider });
            return row;
        }

        private void OnRowClick(Panel row)
        {
            row.GetType()
               .GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
               ?.Invoke(row, new object[] { EventArgs.Empty });
        }

        private void ToggleSection(Control contentPanel, Label chevron)
        {
            contentPanel.Visible = !contentPanel.Visible;
            chevron.Text = contentPanel.Visible ? "▲" : "▼";
        }

        private Panel CreateTextBoxRow(string labelText, out CheckBox checkBox, out TextBox textBox)
        {
            Panel row = new Panel
            {
                Size = new Size(360, 40),
                BackColor = Color.White,
                Margin = new Padding(0)
            };

            CheckBox cb = new CheckBox
            {
                Text = labelText,
                Location = new Point(24, 10),
                Size = new Size(85, 22),
                Checked = true
            };

            TextBox tb = new TextBox
            {
                Location = new Point(116, 8),
                Width = 190
            };

            cb.CheckedChanged += (s, e) => tb.Enabled = cb.Checked;

            checkBox = cb;
            textBox = tb;

            row.Controls.Add(cb);
            row.Controls.Add(tb);
            return row;
        }

        private Panel CreateRangeDropdownRow(DropdownSection section)
        {
            Panel row = new Panel
            {
                Size = new Size(360, 42),
                BackColor = Color.White,
                Margin = new Padding(0)
            };

            section.ChkRange = new CheckBox
            {
                Text = "Select:",
                Location = new Point(24, 10),
                Size = new Size(85, 22),
                Checked = true
            };

            section.BtnRangeDropdown = new Button
            {
                Text = "Choose 1–20 ▼",
                Location = new Point(116, 6),
                Size = new Size(190, 28),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Standard
            };

            section.ChkRange.CheckedChanged += (s, e) =>
                section.BtnRangeDropdown.Enabled = section.ChkRange.Checked;

            Panel popupPanel = new Panel
            {
                Size = new Size(190, 220),
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
                Size = new Size(182, 184),
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

        private void BtnApply_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var section in dropdownSections)
            {
                string sibling = section.ChkSibling.Checked ? section.TxtSibling.Text : "(Disabled)";
                string depth = section.ChkDepth.Checked ? section.TxtDepth.Text : "(Disabled)";

                List<string> selectedNums = new List<string>();
                if (section.ChkRange.Checked)
                {
                    foreach (var item in section.ClbNumbers.CheckedItems)
                        selectedNums.Add(item.ToString());
                }

                string rangeText = section.ChkRange.Checked
                    ? (selectedNums.Any() ? string.Join(", ", selectedNums) : "None")
                    : "(Disabled)";

                sb.AppendLine($"[{section.Title}]");
                sb.AppendLine($"  Sibling: {sibling}");
                sb.AppendLine($"  Depth: {depth}");
                sb.AppendLine($"  Selected Numbers: {rangeText}");
                sb.AppendLine();
            }

            MessageBox.Show(
                sb.ToString().TrimEnd(),
                "Applied Values",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
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
            public Panel HeaderPanel { get; set; }
            public Panel ContentPanel { get; set; }
            public Label ChevronLabel { get; set; }

            public CheckBox ChkSibling { get; set; }
            public TextBox TxtSibling { get; set; }

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
    }
}