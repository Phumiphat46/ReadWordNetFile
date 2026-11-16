namespace WordNetProgram
{
    partial class Add_Thai_To_WordNet_18_Sep_2026
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
            label1 = new Label();
            tbFolderPath = new TextBox();
            btBrowseClick = new Button();
            comboBox1 = new ComboBox();
            label2 = new Label();
            button2 = new Button();
            comboBox2 = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            bTLoadOMW = new Button();
            bTLoadDhanon = new Button();
            comboBox3 = new ComboBox();
            label5 = new Label();
            bTLoadGemini = new Button();
            label7 = new Label();
            comboBox4 = new ComboBox();
            dGVMergedWordNet = new TabControl();
            tabPage1 = new TabPage();
            dgvWordNet = new DataGridView();
            tabPage2 = new TabPage();
            dGVOMW = new DataGridView();
            tabPage3 = new TabPage();
            dGVDhanon = new DataGridView();
            tabPage4 = new TabPage();
            dGVGemini = new DataGridView();
            tabPage5 = new TabPage();
            btMergedRevealInFileExplorer = new Button();
            dGVAddedThai = new DataGridView();
            bTAddThaiToWordNet = new Button();
            label6 = new Label();
            label8 = new Label();
            dGVMergedWordNet.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvWordNet).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dGVOMW).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dGVDhanon).BeginInit();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dGVGemini).BeginInit();
            tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dGVAddedThai).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 23);
            label1.Name = "label1";
            label1.Size = new Size(51, 20);
            label1.TabIndex = 0;
            label1.Text = "Folder";
            // 
            // tbFolderPath
            // 
            tbFolderPath.Location = new Point(69, 20);
            tbFolderPath.Name = "tbFolderPath";
            tbFolderPath.Size = new Size(435, 27);
            tbFolderPath.TabIndex = 1;
            // 
            // btBrowseClick
            // 
            btBrowseClick.Location = new Point(510, 20);
            btBrowseClick.Name = "btBrowseClick";
            btBrowseClick.Size = new Size(94, 29);
            btBrowseClick.TabIndex = 2;
            btBrowseClick.Text = "Load";
            btBrowseClick.UseVisualStyleBackColor = true;
            btBrowseClick.Click += btBrowse_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(150, 65);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(354, 28);
            comboBox1.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 67);
            label2.Name = "label2";
            label2.Size = new Size(132, 20);
            label2.TabIndex = 4;
            label2.Text = "WordNet Data File";
            // 
            // button2
            // 
            button2.Location = new Point(510, 63);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 5;
            button2.Text = "Load";
            button2.UseVisualStyleBackColor = true;
            button2.Click += bTLoadFile_Click;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(150, 127);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(354, 28);
            comboBox2.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 130);
            label3.Name = "label3";
            label3.Size = new Size(88, 20);
            label3.TabIndex = 7;
            label3.Text = "OMW's Thai";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 95);
            label4.Name = "label4";
            label4.Size = new Size(181, 20);
            label4.TabIndex = 8;
            label4.Text = "(e.g. data.noun, data.verb)";
            // 
            // bTLoadOMW
            // 
            bTLoadOMW.Location = new Point(510, 126);
            bTLoadOMW.Name = "bTLoadOMW";
            bTLoadOMW.Size = new Size(94, 29);
            bTLoadOMW.TabIndex = 9;
            bTLoadOMW.Text = "Load";
            bTLoadOMW.UseVisualStyleBackColor = true;
            bTLoadOMW.Click += bTLoadOMW_Click;
            // 
            // bTLoadDhanon
            // 
            bTLoadDhanon.Location = new Point(510, 176);
            bTLoadDhanon.Name = "bTLoadDhanon";
            bTLoadDhanon.Size = new Size(94, 29);
            bTLoadDhanon.TabIndex = 11;
            bTLoadDhanon.Text = "Load";
            bTLoadDhanon.UseVisualStyleBackColor = true;
            bTLoadDhanon.Click += bTLoadDhanon_Click;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(150, 177);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(354, 28);
            comboBox3.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 241);
            label5.Name = "label5";
            label5.Size = new Size(117, 20);
            label5.TabIndex = 12;
            label5.Text = "Gemini (tab file)";
            // 
            // bTLoadGemini
            // 
            bTLoadGemini.Location = new Point(510, 232);
            bTLoadGemini.Name = "bTLoadGemini";
            bTLoadGemini.Size = new Size(94, 29);
            bTLoadGemini.TabIndex = 15;
            bTLoadGemini.Text = "Load";
            bTLoadGemini.UseVisualStyleBackColor = true;
            bTLoadGemini.Click += bTLoadGemini_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 185);
            label7.Name = "label7";
            label7.Size = new Size(110, 20);
            label7.TabIndex = 14;
            label7.Text = "Dhanon's Noun";
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(150, 233);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(354, 28);
            comboBox4.TabIndex = 13;
            // 
            // dGVMergedWordNet
            // 
            dGVMergedWordNet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dGVMergedWordNet.Controls.Add(tabPage1);
            dGVMergedWordNet.Controls.Add(tabPage2);
            dGVMergedWordNet.Controls.Add(tabPage3);
            dGVMergedWordNet.Controls.Add(tabPage4);
            dGVMergedWordNet.Controls.Add(tabPage5);
            dGVMergedWordNet.Location = new Point(12, 293);
            dGVMergedWordNet.Name = "dGVMergedWordNet";
            dGVMergedWordNet.SelectedIndex = 0;
            dGVMergedWordNet.Size = new Size(910, 431);
            dGVMergedWordNet.TabIndex = 16;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dgvWordNet);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(902, 398);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "WordNet Data";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvWordNet
            // 
            dgvWordNet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvWordNet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvWordNet.Location = new Point(0, 0);
            dgvWordNet.Name = "dgvWordNet";
            dgvWordNet.ReadOnly = true;
            dgvWordNet.RowHeadersWidth = 51;
            dgvWordNet.Size = new Size(880, 376);
            dgvWordNet.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(dGVOMW);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(902, 398);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "OMW's Thai";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // dGVOMW
            // 
            dGVOMW.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dGVOMW.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dGVOMW.Location = new Point(11, 11);
            dGVOMW.Name = "dGVOMW";
            dGVOMW.ReadOnly = true;
            dGVOMW.RowHeadersWidth = 51;
            dGVOMW.Size = new Size(880, 376);
            dGVOMW.TabIndex = 1;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(dGVDhanon);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(902, 398);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Dhanon's Noun";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // dGVDhanon
            // 
            dGVDhanon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dGVDhanon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dGVDhanon.Location = new Point(11, 11);
            dGVDhanon.Name = "dGVDhanon";
            dGVDhanon.ReadOnly = true;
            dGVDhanon.RowHeadersWidth = 51;
            dGVDhanon.Size = new Size(880, 376);
            dGVDhanon.TabIndex = 2;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(dGVGemini);
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(902, 398);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Gemini's Thai";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // dGVGemini
            // 
            dGVGemini.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dGVGemini.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dGVGemini.Location = new Point(11, 11);
            dGVGemini.Name = "dGVGemini";
            dGVGemini.ReadOnly = true;
            dGVGemini.RowHeadersWidth = 51;
            dGVGemini.Size = new Size(880, 376);
            dGVGemini.TabIndex = 3;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(btMergedRevealInFileExplorer);
            tabPage5.Controls.Add(dGVAddedThai);
            tabPage5.Location = new Point(4, 29);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(902, 398);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Merged WordNet";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // btMergedRevealInFileExplorer
            // 
            btMergedRevealInFileExplorer.Location = new Point(11, 3);
            btMergedRevealInFileExplorer.Name = "btMergedRevealInFileExplorer";
            btMergedRevealInFileExplorer.Size = new Size(163, 29);
            btMergedRevealInFileExplorer.TabIndex = 39;
            btMergedRevealInFileExplorer.Text = "Reveal in File Explorer";
            btMergedRevealInFileExplorer.UseVisualStyleBackColor = true;
            btMergedRevealInFileExplorer.Click += btMergedRevealInFileExplorer_Click;
            // 
            // dGVAddedThai
            // 
            dGVAddedThai.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dGVAddedThai.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dGVAddedThai.Location = new Point(11, 34);
            dGVAddedThai.Name = "dGVAddedThai";
            dGVAddedThai.ReadOnly = true;
            dGVAddedThai.RowHeadersWidth = 51;
            dGVAddedThai.Size = new Size(880, 353);
            dGVAddedThai.TabIndex = 4;
            // 
            // bTAddThaiToWordNet
            // 
            bTAddThaiToWordNet.Location = new Point(627, 63);
            bTAddThaiToWordNet.Name = "bTAddThaiToWordNet";
            bTAddThaiToWordNet.Size = new Size(240, 29);
            bTAddThaiToWordNet.TabIndex = 17;
            bTAddThaiToWordNet.Text = "Add Thai Word To WordNet";
            bTAddThaiToWordNet.UseVisualStyleBackColor = true;
            bTAddThaiToWordNet.Click += bTAddThaiToWordNet_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 154);
            label6.Name = "label6";
            label6.Size = new Size(66, 20);
            label6.TabIndex = 18;
            label6.Text = "(tab file)";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(16, 205);
            label8.Name = "label8";
            label8.Size = new Size(66, 20);
            label8.TabIndex = 19;
            label8.Text = "(tab file)";
            // 
            // Add_Thai_To_WordNet_18_Sep_2026
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(934, 736);
            Controls.Add(bTAddThaiToWordNet);
            Controls.Add(dGVMergedWordNet);
            Controls.Add(bTLoadGemini);
            Controls.Add(label7);
            Controls.Add(comboBox4);
            Controls.Add(label5);
            Controls.Add(bTLoadDhanon);
            Controls.Add(comboBox3);
            Controls.Add(bTLoadOMW);
            Controls.Add(label3);
            Controls.Add(comboBox2);
            Controls.Add(button2);
            Controls.Add(label2);
            Controls.Add(comboBox1);
            Controls.Add(btBrowseClick);
            Controls.Add(tbFolderPath);
            Controls.Add(label1);
            Controls.Add(label4);
            Controls.Add(label6);
            Controls.Add(label8);
            Name = "Add_Thai_To_WordNet_18_Sep_2026";
            Text = "Add_Thai_To_WordNet";
            dGVMergedWordNet.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvWordNet).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dGVOMW).EndInit();
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dGVDhanon).EndInit();
            tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dGVGemini).EndInit();
            tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dGVAddedThai).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbFolderPath;
        private System.Windows.Forms.Button btBrowseClick;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bTLoadOMW;
        private System.Windows.Forms.Button bTLoadDhanon;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bTLoadGemini;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.TabControl dGVMergedWordNet;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button bTAddThaiToWordNet;
        private System.Windows.Forms.DataGridView dgvWordNet;
        private System.Windows.Forms.DataGridView dGVOMW;
        private System.Windows.Forms.DataGridView dGVDhanon;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dGVGemini;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.DataGridView dGVAddedThai;
        private System.Windows.Forms.Button btMergedRevealInFileExplorer;
    }
}