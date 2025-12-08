namespace WordnetToDGV
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            filePathTextBox = new TextBox();
            filesPathButton = new Button();
            nounPathButton = new Button();
            nounPathTextBox = new TextBox();
            label2 = new Label();
            verbPathButton = new Button();
            verbPathTextBox = new TextBox();
            label3 = new Label();
            adjectivePathButton = new Button();
            adjectivePathTextBox = new TextBox();
            label4 = new Label();
            adverbPathButton = new Button();
            adverbPathTextBox = new TextBox();
            label5 = new Label();
            dataGridView1 = new DataGridView();
            tabControl1 = new TabControl();
            nounTabPage = new TabPage();
            verbTabPage = new TabPage();
            dataGridView2 = new DataGridView();
            adjectiveTabPage = new TabPage();
            dataGridView3 = new DataGridView();
            adverbTabPage = new TabPage();
            dataGridView4 = new DataGridView();
            nounPathComboBox = new ComboBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabControl1.SuspendLayout();
            nounTabPage.SuspendLayout();
            verbTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            adjectiveTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            adverbTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(116, 26);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(55, 24);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(67, 76);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 1;
            label1.Text = "Files Path";
            // 
            // filePathTextBox
            // 
            filePathTextBox.Location = new Point(142, 73);
            filePathTextBox.Name = "filePathTextBox";
            filePathTextBox.Size = new Size(422, 27);
            filePathTextBox.TabIndex = 2;
            // 
            // filesPathButton
            // 
            filesPathButton.Location = new Point(579, 73);
            filesPathButton.Name = "filesPathButton";
            filesPathButton.Size = new Size(94, 29);
            filesPathButton.TabIndex = 3;
            filesPathButton.Text = "Select Path";
            filesPathButton.UseVisualStyleBackColor = true;
            filesPathButton.Click += filesPathButton_Click;
            // 
            // nounPathButton
            // 
            nounPathButton.Location = new Point(579, 115);
            nounPathButton.Name = "nounPathButton";
            nounPathButton.Size = new Size(139, 29);
            nounPathButton.TabIndex = 6;
            nounPathButton.Text = "Import data.noun";
            nounPathButton.UseVisualStyleBackColor = true;
            nounPathButton.Click += nounPathButton_Click;
            // 
            // nounPathTextBox
            // 
            nounPathTextBox.Location = new Point(142, 115);
            nounPathTextBox.Name = "nounPathTextBox";
            nounPathTextBox.Size = new Size(422, 27);
            nounPathTextBox.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(67, 118);
            label2.Name = "label2";
            label2.Size = new Size(75, 20);
            label2.TabIndex = 4;
            label2.Text = "data.noun";
            // 
            // verbPathButton
            // 
            verbPathButton.Location = new Point(579, 160);
            verbPathButton.Name = "verbPathButton";
            verbPathButton.Size = new Size(139, 29);
            verbPathButton.TabIndex = 9;
            verbPathButton.Text = "Import data.verb";
            verbPathButton.UseVisualStyleBackColor = true;
            verbPathButton.Click += verbPathButton_Click;
            // 
            // verbPathTextBox
            // 
            verbPathTextBox.Location = new Point(142, 160);
            verbPathTextBox.Name = "verbPathTextBox";
            verbPathTextBox.Size = new Size(422, 27);
            verbPathTextBox.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(67, 163);
            label3.Name = "label3";
            label3.Size = new Size(71, 20);
            label3.TabIndex = 7;
            label3.Text = "data.verb";
            // 
            // adjectivePathButton
            // 
            adjectivePathButton.Location = new Point(579, 204);
            adjectivePathButton.Name = "adjectivePathButton";
            adjectivePathButton.Size = new Size(139, 29);
            adjectivePathButton.TabIndex = 12;
            adjectivePathButton.Text = "Import data.adj";
            adjectivePathButton.UseVisualStyleBackColor = true;
            adjectivePathButton.Click += adjectivePathButton_Click;
            // 
            // adjectivePathTextBox
            // 
            adjectivePathTextBox.Location = new Point(142, 204);
            adjectivePathTextBox.Name = "adjectivePathTextBox";
            adjectivePathTextBox.Size = new Size(422, 27);
            adjectivePathTextBox.TabIndex = 11;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(67, 207);
            label4.Name = "label4";
            label4.Size = new Size(63, 20);
            label4.TabIndex = 10;
            label4.Text = "data.adj";
            // 
            // adverbPathButton
            // 
            adverbPathButton.Location = new Point(579, 247);
            adverbPathButton.Name = "adverbPathButton";
            adverbPathButton.Size = new Size(139, 29);
            adverbPathButton.TabIndex = 15;
            adverbPathButton.Text = "Import data.adv";
            adverbPathButton.UseVisualStyleBackColor = true;
            adverbPathButton.Click += adverbPathButton_Click;
            // 
            // adverbPathTextBox
            // 
            adverbPathTextBox.Location = new Point(142, 247);
            adverbPathTextBox.Name = "adverbPathTextBox";
            adverbPathTextBox.Size = new Size(422, 27);
            adverbPathTextBox.TabIndex = 14;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(67, 250);
            label5.Name = "label5";
            label5.Size = new Size(66, 20);
            label5.TabIndex = 13;
            label5.Text = "data.adv";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(6, 46);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(756, 263);
            dataGridView1.TabIndex = 16;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(nounTabPage);
            tabControl1.Controls.Add(verbTabPage);
            tabControl1.Controls.Add(adjectiveTabPage);
            tabControl1.Controls.Add(adverbTabPage);
            tabControl1.Location = new Point(12, 314);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(776, 349);
            tabControl1.TabIndex = 17;
            // 
            // nounTabPage
            // 
            nounTabPage.Controls.Add(dataGridView1);
            nounTabPage.Location = new Point(4, 29);
            nounTabPage.Name = "nounTabPage";
            nounTabPage.Padding = new Padding(3);
            nounTabPage.Size = new Size(768, 316);
            nounTabPage.TabIndex = 0;
            nounTabPage.Text = "Noun";
            nounTabPage.UseVisualStyleBackColor = true;
            // 
            // verbTabPage
            // 
            verbTabPage.Controls.Add(dataGridView2);
            verbTabPage.Location = new Point(4, 29);
            verbTabPage.Name = "verbTabPage";
            verbTabPage.Padding = new Padding(3);
            verbTabPage.Size = new Size(768, 316);
            verbTabPage.TabIndex = 1;
            verbTabPage.Text = "Verb";
            verbTabPage.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(6, 46);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(756, 263);
            dataGridView2.TabIndex = 17;
            // 
            // adjectiveTabPage
            // 
            adjectiveTabPage.Controls.Add(dataGridView3);
            adjectiveTabPage.Location = new Point(4, 29);
            adjectiveTabPage.Name = "adjectiveTabPage";
            adjectiveTabPage.Size = new Size(768, 316);
            adjectiveTabPage.TabIndex = 2;
            adjectiveTabPage.Text = "Adjective";
            adjectiveTabPage.UseVisualStyleBackColor = true;
            // 
            // dataGridView3
            // 
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Location = new Point(6, 46);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.Size = new Size(756, 263);
            dataGridView3.TabIndex = 18;
            // 
            // adverbTabPage
            // 
            adverbTabPage.Controls.Add(dataGridView4);
            adverbTabPage.Location = new Point(4, 29);
            adverbTabPage.Name = "adverbTabPage";
            adverbTabPage.Size = new Size(768, 316);
            adverbTabPage.TabIndex = 3;
            adverbTabPage.Text = "Adverb";
            adverbTabPage.UseVisualStyleBackColor = true;
            // 
            // dataGridView4
            // 
            dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView4.Location = new Point(6, 46);
            dataGridView4.Name = "dataGridView4";
            dataGridView4.RowHeadersWidth = 51;
            dataGridView4.Size = new Size(756, 263);
            dataGridView4.TabIndex = 18;
            // 
            // nounPathComboBox
            // 
            nounPathComboBox.FormattingEnabled = true;
            nounPathComboBox.Location = new Point(142, 280);
            nounPathComboBox.Name = "nounPathComboBox";
            nounPathComboBox.Size = new Size(422, 28);
            nounPathComboBox.TabIndex = 18;
            nounPathComboBox.SelectedIndexChanged += nounPathComboBox_SelectedIndexChanged;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 664);
            Controls.Add(nounPathComboBox);
            Controls.Add(tabControl1);
            Controls.Add(adverbPathButton);
            Controls.Add(adverbPathTextBox);
            Controls.Add(label5);
            Controls.Add(adjectivePathButton);
            Controls.Add(adjectivePathTextBox);
            Controls.Add(label4);
            Controls.Add(verbPathButton);
            Controls.Add(verbPathTextBox);
            Controls.Add(label3);
            Controls.Add(nounPathButton);
            Controls.Add(nounPathTextBox);
            Controls.Add(label2);
            Controls.Add(filesPathButton);
            Controls.Add(filePathTextBox);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabControl1.ResumeLayout(false);
            nounTabPage.ResumeLayout(false);
            verbTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            adjectiveTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            adverbTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView4).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private Label label1;
        private TextBox filePathTextBox;
        private Button filesPathButton;
        private Button nounPathButton;
        private TextBox nounPathTextBox;
        private Label label2;
        private Button verbPathButton;
        private TextBox verbPathTextBox;
        private Label label3;
        private Button adjectivePathButton;
        private TextBox adjectivePathTextBox;
        private Label label4;
        private Button adverbPathButton;
        private TextBox adverbPathTextBox;
        private Label label5;
        private DataGridView dataGridView1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage nounTabPage;
        private TabPage verbTabPage;
        private TabPage adjectiveTabPage;
        private TabPage adverbTabPage;
        private DataGridView dataGridView2;
        private DataGridView dataGridView3;
        private DataGridView dataGridView4;
        private ComboBox comboBox1;
        private ComboBox nounPathComboBox;
    }
}