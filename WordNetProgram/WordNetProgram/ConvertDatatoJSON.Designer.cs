namespace WordNetProgram
{
    partial class ConvertDatatoJSON
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
            btBrowse = new Button();
            tbFolderPath = new TextBox();
            label1 = new Label();
            btLoadMergedVerb = new Button();
            label7 = new Label();
            cbbMergedVerb = new ComboBox();
            label5 = new Label();
            btLoadMergedAdj = new Button();
            cbbMergedAdj = new ComboBox();
            btLoadMergedNoun = new Button();
            label3 = new Label();
            cbbMergedNoun = new ComboBox();
            btLoadMergedAdv = new Button();
            cbbMergedAdv = new ComboBox();
            label2 = new Label();
            tcPOS = new TabControl();
            tpNoun = new TabPage();
            tcNoun = new TabControl();
            tpMergedNoun = new TabPage();
            dgvMergedNoun = new DataGridView();
            tpMergedNounJson = new TabPage();
            btNounRevealInFileExplorer = new Button();
            btNounViewOnNotepad = new Button();
            dgvMergedNounJSON = new DataGridView();
            tpAdj = new TabPage();
            tcAdj = new TabControl();
            tpMergedAdj = new TabPage();
            dgvMergedAdj = new DataGridView();
            tpMergedAdjJson = new TabPage();
            btAdjRevealInFileExplorer = new Button();
            btAdjViewOnNotepad = new Button();
            dgvMergedAdjJson = new DataGridView();
            tpVerb = new TabPage();
            tcVerb = new TabControl();
            tpMergedVerb = new TabPage();
            dgvMergedVerb = new DataGridView();
            tpMergedVerbJson = new TabPage();
            btVerbRevealInFileExplorer = new Button();
            btVerbViewOnNotepad = new Button();
            dgvMergedVerbJson = new DataGridView();
            tpAdv = new TabPage();
            tcAdv = new TabControl();
            tpMergedAdv = new TabPage();
            tpMergedAdvJSON = new TabPage();
            btAdvRevealInFileExplorer = new Button();
            btAdvViewOnNotepad = new Button();
            dgvMergedAdvJson = new DataGridView();
            cbNoun = new CheckBox();
            cbAdj = new CheckBox();
            cbVerb = new CheckBox();
            cbAdv = new CheckBox();
            cbSelectAll = new CheckBox();
            btToJson = new Button();
            dgvMergedAdv = new DataGridView();
            tcPOS.SuspendLayout();
            tpNoun.SuspendLayout();
            tcNoun.SuspendLayout();
            tpMergedNoun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMergedNoun).BeginInit();
            tpMergedNounJson.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMergedNounJSON).BeginInit();
            tpAdj.SuspendLayout();
            tcAdj.SuspendLayout();
            tpMergedAdj.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMergedAdj).BeginInit();
            tpMergedAdjJson.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMergedAdjJson).BeginInit();
            tpVerb.SuspendLayout();
            tcVerb.SuspendLayout();
            tpMergedVerb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMergedVerb).BeginInit();
            tpMergedVerbJson.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMergedVerbJson).BeginInit();
            tpAdv.SuspendLayout();
            tcAdv.SuspendLayout();
            tpMergedAdv.SuspendLayout();
            tpMergedAdvJSON.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMergedAdvJson).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMergedAdv).BeginInit();
            SuspendLayout();
            // 
            // btBrowse
            // 
            btBrowse.BackColor = SystemColors.Control;
            btBrowse.Location = new Point(524, 24);
            btBrowse.Name = "btBrowse";
            btBrowse.Size = new Size(94, 29);
            btBrowse.TabIndex = 5;
            btBrowse.Text = "Browse";
            btBrowse.UseVisualStyleBackColor = false;
            btBrowse.Click += btBrowse_Click;
            // 
            // tbFolderPath
            // 
            tbFolderPath.Location = new Point(83, 24);
            tbFolderPath.Name = "tbFolderPath";
            tbFolderPath.Size = new Size(435, 27);
            tbFolderPath.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 27);
            label1.Name = "label1";
            label1.Size = new Size(51, 20);
            label1.TabIndex = 3;
            label1.Text = "Folder";
            // 
            // btLoadMergedVerb
            // 
            btLoadMergedVerb.BackColor = SystemColors.Control;
            btLoadMergedVerb.Location = new Point(524, 175);
            btLoadMergedVerb.Name = "btLoadMergedVerb";
            btLoadMergedVerb.Size = new Size(94, 29);
            btLoadMergedVerb.TabIndex = 28;
            btLoadMergedVerb.Text = "Load";
            btLoadMergedVerb.UseVisualStyleBackColor = false;
            btLoadMergedVerb.Click += btLoadMergedVerb_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(26, 136);
            label7.Name = "label7";
            label7.Size = new Size(85, 20);
            label7.TabIndex = 27;
            label7.Text = "merged.adj";
            // 
            // cbbMergedVerb
            // 
            cbbMergedVerb.FormattingEnabled = true;
            cbbMergedVerb.Location = new Point(164, 176);
            cbbMergedVerb.Name = "cbbMergedVerb";
            cbbMergedVerb.Size = new Size(354, 28);
            cbbMergedVerb.TabIndex = 26;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(26, 184);
            label5.Name = "label5";
            label5.Size = new Size(93, 20);
            label5.TabIndex = 25;
            label5.Text = "merged.verb";
            // 
            // btLoadMergedAdj
            // 
            btLoadMergedAdj.BackColor = SystemColors.Control;
            btLoadMergedAdj.Location = new Point(524, 127);
            btLoadMergedAdj.Name = "btLoadMergedAdj";
            btLoadMergedAdj.Size = new Size(94, 29);
            btLoadMergedAdj.TabIndex = 24;
            btLoadMergedAdj.Text = "Load";
            btLoadMergedAdj.UseVisualStyleBackColor = false;
            btLoadMergedAdj.Click += btLoadMergedAdj_Click;
            // 
            // cbbMergedAdj
            // 
            cbbMergedAdj.FormattingEnabled = true;
            cbbMergedAdj.Location = new Point(164, 128);
            cbbMergedAdj.Name = "cbbMergedAdj";
            cbbMergedAdj.Size = new Size(354, 28);
            cbbMergedAdj.TabIndex = 23;
            // 
            // btLoadMergedNoun
            // 
            btLoadMergedNoun.BackColor = SystemColors.Control;
            btLoadMergedNoun.Location = new Point(524, 81);
            btLoadMergedNoun.Name = "btLoadMergedNoun";
            btLoadMergedNoun.Size = new Size(94, 29);
            btLoadMergedNoun.TabIndex = 22;
            btLoadMergedNoun.Text = "Load";
            btLoadMergedNoun.UseVisualStyleBackColor = false;
            btLoadMergedNoun.Click += btLoadMergedNoun_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(26, 85);
            label3.Name = "label3";
            label3.Size = new Size(97, 20);
            label3.TabIndex = 21;
            label3.Text = "merged.noun";
            // 
            // cbbMergedNoun
            // 
            cbbMergedNoun.FormattingEnabled = true;
            cbbMergedNoun.Location = new Point(164, 82);
            cbbMergedNoun.Name = "cbbMergedNoun";
            cbbMergedNoun.Size = new Size(354, 28);
            cbbMergedNoun.TabIndex = 20;
            // 
            // btLoadMergedAdv
            // 
            btLoadMergedAdv.BackColor = SystemColors.Control;
            btLoadMergedAdv.Location = new Point(524, 220);
            btLoadMergedAdv.Name = "btLoadMergedAdv";
            btLoadMergedAdv.Size = new Size(94, 29);
            btLoadMergedAdv.TabIndex = 31;
            btLoadMergedAdv.Text = "Load";
            btLoadMergedAdv.UseVisualStyleBackColor = false;
            btLoadMergedAdv.Click += btLoadMergedAdv_Click;
            // 
            // cbbMergedAdv
            // 
            cbbMergedAdv.FormattingEnabled = true;
            cbbMergedAdv.Location = new Point(164, 221);
            cbbMergedAdv.Name = "cbbMergedAdv";
            cbbMergedAdv.Size = new Size(354, 28);
            cbbMergedAdv.TabIndex = 30;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 229);
            label2.Name = "label2";
            label2.Size = new Size(88, 20);
            label2.TabIndex = 29;
            label2.Text = "merged.adv";
            // 
            // tcPOS
            // 
            tcPOS.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tcPOS.Controls.Add(tpNoun);
            tcPOS.Controls.Add(tpAdj);
            tcPOS.Controls.Add(tpVerb);
            tcPOS.Controls.Add(tpAdv);
            tcPOS.Location = new Point(26, 264);
            tcPOS.Name = "tcPOS";
            tcPOS.SelectedIndex = 0;
            tcPOS.Size = new Size(1266, 443);
            tcPOS.TabIndex = 32;
            // 
            // tpNoun
            // 
            tpNoun.Controls.Add(tcNoun);
            tpNoun.Location = new Point(4, 29);
            tpNoun.Name = "tpNoun";
            tpNoun.Padding = new Padding(3);
            tpNoun.Size = new Size(1258, 410);
            tpNoun.TabIndex = 0;
            tpNoun.Text = "Noun";
            tpNoun.UseVisualStyleBackColor = true;
            // 
            // tcNoun
            // 
            tcNoun.Controls.Add(tpMergedNoun);
            tcNoun.Controls.Add(tpMergedNounJson);
            tcNoun.Location = new Point(3, 6);
            tcNoun.Name = "tcNoun";
            tcNoun.SelectedIndex = 0;
            tcNoun.Size = new Size(1255, 398);
            tcNoun.TabIndex = 33;
            // 
            // tpMergedNoun
            // 
            tpMergedNoun.Controls.Add(dgvMergedNoun);
            tpMergedNoun.Location = new Point(4, 29);
            tpMergedNoun.Name = "tpMergedNoun";
            tpMergedNoun.Padding = new Padding(3);
            tpMergedNoun.Size = new Size(1247, 365);
            tpMergedNoun.TabIndex = 0;
            tpMergedNoun.Text = "merged.noun";
            tpMergedNoun.UseVisualStyleBackColor = true;
            // 
            // dgvMergedNoun
            // 
            dgvMergedNoun.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMergedNoun.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMergedNoun.Location = new Point(3, 0);
            dgvMergedNoun.Name = "dgvMergedNoun";
            dgvMergedNoun.ReadOnly = true;
            dgvMergedNoun.RowHeadersWidth = 51;
            dgvMergedNoun.Size = new Size(1238, 362);
            dgvMergedNoun.TabIndex = 1;
            // 
            // tpMergedNounJson
            // 
            tpMergedNounJson.Controls.Add(btNounRevealInFileExplorer);
            tpMergedNounJson.Controls.Add(btNounViewOnNotepad);
            tpMergedNounJson.Controls.Add(dgvMergedNounJSON);
            tpMergedNounJson.Location = new Point(4, 29);
            tpMergedNounJson.Name = "tpMergedNounJson";
            tpMergedNounJson.Padding = new Padding(3);
            tpMergedNounJson.Size = new Size(1247, 365);
            tpMergedNounJson.TabIndex = 1;
            tpMergedNounJson.Text = "merged_noun.json";
            tpMergedNounJson.UseVisualStyleBackColor = true;
            // 
            // btNounRevealInFileExplorer
            // 
            btNounRevealInFileExplorer.Location = new Point(191, 6);
            btNounRevealInFileExplorer.Name = "btNounRevealInFileExplorer";
            btNounRevealInFileExplorer.Size = new Size(163, 29);
            btNounRevealInFileExplorer.TabIndex = 38;
            btNounRevealInFileExplorer.Text = "Reveal in File Explorer";
            btNounRevealInFileExplorer.UseVisualStyleBackColor = true;
            btNounRevealInFileExplorer.Click += btNounRevealInFileExplorer_Click;
            // 
            // btNounViewOnNotepad
            // 
            btNounViewOnNotepad.Location = new Point(10, 6);
            btNounViewOnNotepad.Name = "btNounViewOnNotepad";
            btNounViewOnNotepad.Size = new Size(163, 29);
            btNounViewOnNotepad.TabIndex = 37;
            btNounViewOnNotepad.Text = "View on Notepad";
            btNounViewOnNotepad.UseVisualStyleBackColor = true;
            btNounViewOnNotepad.Click += btNounViewOnNotepad_Click;
            // 
            // dgvMergedNounJSON
            // 
            dgvMergedNounJSON.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMergedNounJSON.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMergedNounJSON.Location = new Point(3, 41);
            dgvMergedNounJSON.Name = "dgvMergedNounJSON";
            dgvMergedNounJSON.ReadOnly = true;
            dgvMergedNounJSON.RowHeadersWidth = 51;
            dgvMergedNounJSON.Size = new Size(1238, 298);
            dgvMergedNounJSON.TabIndex = 36;
            // 
            // tpAdj
            // 
            tpAdj.Controls.Add(tcAdj);
            tpAdj.Location = new Point(4, 29);
            tpAdj.Name = "tpAdj";
            tpAdj.Padding = new Padding(3);
            tpAdj.Size = new Size(1258, 410);
            tpAdj.TabIndex = 1;
            tpAdj.Text = "Adj";
            tpAdj.UseVisualStyleBackColor = true;
            // 
            // tcAdj
            // 
            tcAdj.Controls.Add(tpMergedAdj);
            tcAdj.Controls.Add(tpMergedAdjJson);
            tcAdj.Location = new Point(1, 3);
            tcAdj.Name = "tcAdj";
            tcAdj.SelectedIndex = 0;
            tcAdj.Size = new Size(1251, 401);
            tcAdj.TabIndex = 34;
            // 
            // tpMergedAdj
            // 
            tpMergedAdj.Controls.Add(dgvMergedAdj);
            tpMergedAdj.Location = new Point(4, 29);
            tpMergedAdj.Name = "tpMergedAdj";
            tpMergedAdj.Padding = new Padding(3);
            tpMergedAdj.Size = new Size(1243, 368);
            tpMergedAdj.TabIndex = 0;
            tpMergedAdj.Text = "merged.adj";
            tpMergedAdj.UseVisualStyleBackColor = true;
            // 
            // dgvMergedAdj
            // 
            dgvMergedAdj.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMergedAdj.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMergedAdj.Location = new Point(4, 3);
            dgvMergedAdj.Name = "dgvMergedAdj";
            dgvMergedAdj.ReadOnly = true;
            dgvMergedAdj.RowHeadersWidth = 51;
            dgvMergedAdj.Size = new Size(1236, 359);
            dgvMergedAdj.TabIndex = 2;
            // 
            // tpMergedAdjJson
            // 
            tpMergedAdjJson.Controls.Add(btAdjRevealInFileExplorer);
            tpMergedAdjJson.Controls.Add(btAdjViewOnNotepad);
            tpMergedAdjJson.Controls.Add(dgvMergedAdjJson);
            tpMergedAdjJson.Location = new Point(4, 29);
            tpMergedAdjJson.Name = "tpMergedAdjJson";
            tpMergedAdjJson.Padding = new Padding(3);
            tpMergedAdjJson.Size = new Size(1243, 368);
            tpMergedAdjJson.TabIndex = 1;
            tpMergedAdjJson.Text = "merged_adj.json";
            tpMergedAdjJson.UseVisualStyleBackColor = true;
            // 
            // btAdjRevealInFileExplorer
            // 
            btAdjRevealInFileExplorer.Location = new Point(172, 6);
            btAdjRevealInFileExplorer.Name = "btAdjRevealInFileExplorer";
            btAdjRevealInFileExplorer.Size = new Size(163, 29);
            btAdjRevealInFileExplorer.TabIndex = 39;
            btAdjRevealInFileExplorer.Text = "Reveal in File Explorer";
            btAdjRevealInFileExplorer.UseVisualStyleBackColor = true;
            btAdjRevealInFileExplorer.Click += btAdjRevealInFileExplorer_Click;
            // 
            // btAdjViewOnNotepad
            // 
            btAdjViewOnNotepad.Location = new Point(3, 6);
            btAdjViewOnNotepad.Name = "btAdjViewOnNotepad";
            btAdjViewOnNotepad.Size = new Size(163, 29);
            btAdjViewOnNotepad.TabIndex = 38;
            btAdjViewOnNotepad.Text = "View on Notepad";
            btAdjViewOnNotepad.UseVisualStyleBackColor = true;
            btAdjViewOnNotepad.Click += btAdjViewOnNotepad_Click;
            // 
            // dgvMergedAdjJson
            // 
            dgvMergedAdjJson.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMergedAdjJson.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMergedAdjJson.Location = new Point(3, 39);
            dgvMergedAdjJson.Name = "dgvMergedAdjJson";
            dgvMergedAdjJson.ReadOnly = true;
            dgvMergedAdjJson.RowHeadersWidth = 51;
            dgvMergedAdjJson.Size = new Size(1217, 326);
            dgvMergedAdjJson.TabIndex = 3;
            // 
            // tpVerb
            // 
            tpVerb.Controls.Add(tcVerb);
            tpVerb.Location = new Point(4, 29);
            tpVerb.Name = "tpVerb";
            tpVerb.Size = new Size(1258, 410);
            tpVerb.TabIndex = 2;
            tpVerb.Text = "Verb";
            tpVerb.UseVisualStyleBackColor = true;
            // 
            // tcVerb
            // 
            tcVerb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tcVerb.Controls.Add(tpMergedVerb);
            tcVerb.Controls.Add(tpMergedVerbJson);
            tcVerb.Location = new Point(1, 3);
            tcVerb.Name = "tcVerb";
            tcVerb.SelectedIndex = 0;
            tcVerb.Size = new Size(1257, 404);
            tcVerb.TabIndex = 34;
            // 
            // tpMergedVerb
            // 
            tpMergedVerb.Controls.Add(dgvMergedVerb);
            tpMergedVerb.Location = new Point(4, 29);
            tpMergedVerb.Name = "tpMergedVerb";
            tpMergedVerb.Padding = new Padding(3);
            tpMergedVerb.Size = new Size(1249, 371);
            tpMergedVerb.TabIndex = 0;
            tpMergedVerb.Text = "merged.verb";
            tpMergedVerb.UseVisualStyleBackColor = true;
            // 
            // dgvMergedVerb
            // 
            dgvMergedVerb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMergedVerb.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMergedVerb.Location = new Point(2, 6);
            dgvMergedVerb.Name = "dgvMergedVerb";
            dgvMergedVerb.ReadOnly = true;
            dgvMergedVerb.RowHeadersWidth = 51;
            dgvMergedVerb.Size = new Size(1244, 362);
            dgvMergedVerb.TabIndex = 2;
            // 
            // tpMergedVerbJson
            // 
            tpMergedVerbJson.Controls.Add(btVerbRevealInFileExplorer);
            tpMergedVerbJson.Controls.Add(btVerbViewOnNotepad);
            tpMergedVerbJson.Controls.Add(dgvMergedVerbJson);
            tpMergedVerbJson.Location = new Point(4, 29);
            tpMergedVerbJson.Name = "tpMergedVerbJson";
            tpMergedVerbJson.Padding = new Padding(3);
            tpMergedVerbJson.Size = new Size(1249, 371);
            tpMergedVerbJson.TabIndex = 1;
            tpMergedVerbJson.Text = "merged_verb.json";
            tpMergedVerbJson.UseVisualStyleBackColor = true;
            // 
            // btVerbRevealInFileExplorer
            // 
            btVerbRevealInFileExplorer.Location = new Point(175, 6);
            btVerbRevealInFileExplorer.Name = "btVerbRevealInFileExplorer";
            btVerbRevealInFileExplorer.Size = new Size(163, 29);
            btVerbRevealInFileExplorer.TabIndex = 41;
            btVerbRevealInFileExplorer.Text = "Reveal in File Explorer";
            btVerbRevealInFileExplorer.UseVisualStyleBackColor = true;
            btVerbRevealInFileExplorer.Click += btVerbRevealInFileExplorer_Click;
            // 
            // btVerbViewOnNotepad
            // 
            btVerbViewOnNotepad.Location = new Point(6, 6);
            btVerbViewOnNotepad.Name = "btVerbViewOnNotepad";
            btVerbViewOnNotepad.Size = new Size(163, 29);
            btVerbViewOnNotepad.TabIndex = 40;
            btVerbViewOnNotepad.Text = "View on Notepad";
            btVerbViewOnNotepad.UseVisualStyleBackColor = true;
            btVerbViewOnNotepad.Click += btVerbViewOnNotepad_Click;
            // 
            // dgvMergedVerbJson
            // 
            dgvMergedVerbJson.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMergedVerbJson.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMergedVerbJson.Location = new Point(2, 41);
            dgvMergedVerbJson.Name = "dgvMergedVerbJson";
            dgvMergedVerbJson.ReadOnly = true;
            dgvMergedVerbJson.RowHeadersWidth = 51;
            dgvMergedVerbJson.Size = new Size(1244, 326);
            dgvMergedVerbJson.TabIndex = 3;
            // 
            // tpAdv
            // 
            tpAdv.Controls.Add(tcAdv);
            tpAdv.Location = new Point(4, 29);
            tpAdv.Name = "tpAdv";
            tpAdv.Size = new Size(1258, 410);
            tpAdv.TabIndex = 3;
            tpAdv.Text = "Adv";
            tpAdv.UseVisualStyleBackColor = true;
            // 
            // tcAdv
            // 
            tcAdv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tcAdv.Controls.Add(tpMergedAdv);
            tcAdv.Controls.Add(tpMergedAdvJSON);
            tcAdv.Location = new Point(1, 3);
            tcAdv.Name = "tcAdv";
            tcAdv.SelectedIndex = 0;
            tcAdv.Size = new Size(1254, 404);
            tcAdv.TabIndex = 34;
            // 
            // tpMergedAdv
            // 
            tpMergedAdv.Controls.Add(dgvMergedAdv);
            tpMergedAdv.Location = new Point(4, 29);
            tpMergedAdv.Name = "tpMergedAdv";
            tpMergedAdv.Padding = new Padding(3);
            tpMergedAdv.Size = new Size(1246, 371);
            tpMergedAdv.TabIndex = 0;
            tpMergedAdv.Text = "merged.adv";
            tpMergedAdv.UseVisualStyleBackColor = true;
            // 
            // tpMergedAdvJSON
            // 
            tpMergedAdvJSON.Controls.Add(btAdvRevealInFileExplorer);
            tpMergedAdvJSON.Controls.Add(btAdvViewOnNotepad);
            tpMergedAdvJSON.Controls.Add(dgvMergedAdvJson);
            tpMergedAdvJSON.Location = new Point(4, 29);
            tpMergedAdvJSON.Name = "tpMergedAdvJSON";
            tpMergedAdvJSON.Padding = new Padding(3);
            tpMergedAdvJSON.Size = new Size(1246, 371);
            tpMergedAdvJSON.TabIndex = 1;
            tpMergedAdvJSON.Text = "merged_adv.json";
            tpMergedAdvJSON.UseVisualStyleBackColor = true;
            // 
            // btAdvRevealInFileExplorer
            // 
            btAdvRevealInFileExplorer.Location = new Point(175, 6);
            btAdvRevealInFileExplorer.Name = "btAdvRevealInFileExplorer";
            btAdvRevealInFileExplorer.Size = new Size(163, 29);
            btAdvRevealInFileExplorer.TabIndex = 41;
            btAdvRevealInFileExplorer.Text = "Reveal in File Explorer";
            btAdvRevealInFileExplorer.UseVisualStyleBackColor = true;
            btAdvRevealInFileExplorer.Click += btAdvRevealInFileExplorer_Click;
            // 
            // btAdvViewOnNotepad
            // 
            btAdvViewOnNotepad.Location = new Point(6, 6);
            btAdvViewOnNotepad.Name = "btAdvViewOnNotepad";
            btAdvViewOnNotepad.Size = new Size(163, 29);
            btAdvViewOnNotepad.TabIndex = 40;
            btAdvViewOnNotepad.Text = "View on Notepad";
            btAdvViewOnNotepad.UseVisualStyleBackColor = true;
            btAdvViewOnNotepad.Click += btAdvViewOnNotepad_Click;
            // 
            // dgvMergedAdvJson
            // 
            dgvMergedAdvJson.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMergedAdvJson.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMergedAdvJson.Location = new Point(2, 41);
            dgvMergedAdvJson.Name = "dgvMergedAdvJson";
            dgvMergedAdvJson.ReadOnly = true;
            dgvMergedAdvJson.RowHeadersWidth = 51;
            dgvMergedAdvJson.Size = new Size(1241, 333);
            dgvMergedAdvJson.TabIndex = 3;
            // 
            // cbNoun
            // 
            cbNoun.AutoSize = true;
            cbNoun.Location = new Point(624, 86);
            cbNoun.Name = "cbNoun";
            cbNoun.Size = new Size(67, 24);
            cbNoun.TabIndex = 34;
            cbNoun.Text = "Noun";
            cbNoun.UseVisualStyleBackColor = true;
            cbNoun.CheckedChanged += cbNoun_CheckedChanged;
            // 
            // cbAdj
            // 
            cbAdj.AutoSize = true;
            cbAdj.Location = new Point(624, 128);
            cbAdj.Name = "cbAdj";
            cbAdj.Size = new Size(54, 24);
            cbAdj.TabIndex = 35;
            cbAdj.Text = "Adj";
            cbAdj.UseVisualStyleBackColor = true;
            // 
            // cbVerb
            // 
            cbVerb.AutoSize = true;
            cbVerb.Location = new Point(624, 178);
            cbVerb.Name = "cbVerb";
            cbVerb.Size = new Size(61, 24);
            cbVerb.TabIndex = 36;
            cbVerb.Text = "Verb";
            cbVerb.UseVisualStyleBackColor = true;
            // 
            // cbAdv
            // 
            cbAdv.AutoSize = true;
            cbAdv.Location = new Point(624, 220);
            cbAdv.Name = "cbAdv";
            cbAdv.Size = new Size(57, 24);
            cbAdv.TabIndex = 37;
            cbAdv.Text = "Adv";
            cbAdv.UseVisualStyleBackColor = true;
            // 
            // cbSelectAll
            // 
            cbSelectAll.AutoSize = true;
            cbSelectAll.Location = new Point(624, 56);
            cbSelectAll.Name = "cbSelectAll";
            cbSelectAll.Size = new Size(93, 24);
            cbSelectAll.TabIndex = 38;
            cbSelectAll.Text = "Select All";
            cbSelectAll.UseVisualStyleBackColor = true;
            cbSelectAll.CheckedChanged += cbSelectAll_CheckedChanged;
            // 
            // btToJson
            // 
            btToJson.BackColor = SystemColors.Control;
            btToJson.Location = new Point(732, 131);
            btToJson.Name = "btToJson";
            btToJson.Size = new Size(133, 63);
            btToJson.TabIndex = 33;
            btToJson.Text = "Convert to JSON";
            btToJson.UseVisualStyleBackColor = false;
            btToJson.Click += btToJson_Click;
            // 
            // dgvMergedAdv
            // 
            dgvMergedAdv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvMergedAdv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMergedAdv.Location = new Point(0, 0);
            dgvMergedAdv.Name = "dgvMergedAdv";
            dgvMergedAdv.ReadOnly = true;
            dgvMergedAdv.RowHeadersWidth = 51;
            dgvMergedAdv.Size = new Size(1240, 362);
            dgvMergedAdv.TabIndex = 2;
            // 
            // ConvertDatatoJSON
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1423, 719);
            Controls.Add(cbSelectAll);
            Controls.Add(cbAdv);
            Controls.Add(cbVerb);
            Controls.Add(cbAdj);
            Controls.Add(cbNoun);
            Controls.Add(btToJson);
            Controls.Add(tcPOS);
            Controls.Add(btLoadMergedAdv);
            Controls.Add(cbbMergedAdv);
            Controls.Add(label2);
            Controls.Add(btLoadMergedVerb);
            Controls.Add(label7);
            Controls.Add(cbbMergedVerb);
            Controls.Add(label5);
            Controls.Add(btLoadMergedAdj);
            Controls.Add(cbbMergedAdj);
            Controls.Add(btLoadMergedNoun);
            Controls.Add(label3);
            Controls.Add(cbbMergedNoun);
            Controls.Add(btBrowse);
            Controls.Add(tbFolderPath);
            Controls.Add(label1);
            Name = "ConvertDatatoJSON";
            Text = "ConvertDatatoJSON";
            tcPOS.ResumeLayout(false);
            tpNoun.ResumeLayout(false);
            tcNoun.ResumeLayout(false);
            tpMergedNoun.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMergedNoun).EndInit();
            tpMergedNounJson.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMergedNounJSON).EndInit();
            tpAdj.ResumeLayout(false);
            tcAdj.ResumeLayout(false);
            tpMergedAdj.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMergedAdj).EndInit();
            tpMergedAdjJson.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMergedAdjJson).EndInit();
            tpVerb.ResumeLayout(false);
            tcVerb.ResumeLayout(false);
            tpMergedVerb.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMergedVerb).EndInit();
            tpMergedVerbJson.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMergedVerbJson).EndInit();
            tpAdv.ResumeLayout(false);
            tcAdv.ResumeLayout(false);
            tpMergedAdv.ResumeLayout(false);
            tpMergedAdvJSON.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMergedAdvJson).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMergedAdv).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btBrowseClick;
        private System.Windows.Forms.TextBox tbFolderPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btLoadMergedVerb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbbMergedVerb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btLoadMergedAdj;
        private System.Windows.Forms.ComboBox cbbMergedAdj;
        private System.Windows.Forms.Button bTLoadOMW;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbbMergedNoun;
        private System.Windows.Forms.Button btLoadMergedAdv;
        private System.Windows.Forms.ComboBox cbbMergedAdv;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl dGVMergedWordNet;
        private System.Windows.Forms.TabPage tpNoun;
        private System.Windows.Forms.TabPage tpAdj;
        private System.Windows.Forms.TabPage tpVerb;
        private System.Windows.Forms.TabPage tpAdv;
        private System.Windows.Forms.TabControl tcNoun;
        private System.Windows.Forms.TabPage tpMergedNoun;
        private System.Windows.Forms.TabPage tpMergedNounJson;
        private System.Windows.Forms.TabControl tcAdj;
        private System.Windows.Forms.TabPage tpMergedAdj;
        private System.Windows.Forms.TabControl tcVerb;
        private System.Windows.Forms.TabPage tpMergedVerb;
        private System.Windows.Forms.TabPage tpMergedVerbJson;
        private System.Windows.Forms.TabControl tcAdv;
        private System.Windows.Forms.TabPage tpMergedAdv;
        private System.Windows.Forms.TabPage tpMergedAdvJSON;
        private System.Windows.Forms.Button btLoadMergedNoun;
        private System.Windows.Forms.TabControl tcPOS;
        private System.Windows.Forms.DataGridView dgvMergedAdj;
        private System.Windows.Forms.DataGridView dgvMergedVerb;
        private System.Windows.Forms.CheckBox cbNoun;
        private System.Windows.Forms.CheckBox cbAdj;
        private System.Windows.Forms.CheckBox cbVerb;
        private System.Windows.Forms.CheckBox cbAdv;
        private System.Windows.Forms.CheckBox cbSelectAll;
        private System.Windows.Forms.DataGridView dgvMergedNounJSON;
        private System.Windows.Forms.Button btNounViewOnNotepad;
        private System.Windows.Forms.Button btToJson;
        private System.Windows.Forms.DataGridView dgvMergedVerbJson;
        private System.Windows.Forms.DataGridView dgvMergedAdvJson;
        private System.Windows.Forms.TabPage tpMergedAdjJson;
        private System.Windows.Forms.DataGridView dgvMergedAdjJson;
        private System.Windows.Forms.Button btAdjViewOnNotepad;
        private System.Windows.Forms.Button btAdvViewOnNotepad;
        private System.Windows.Forms.Button btBrowse;
        private System.Windows.Forms.Button btVerbViewOnNotepad;
        private System.Windows.Forms.Button btNounRevealInFileExplorer;
        private System.Windows.Forms.Button btAdjRevealInFileExplorer;
        private System.Windows.Forms.Button btVerbRevealInFileExplorer;
        private System.Windows.Forms.Button btAdvRevealInFileExplorer;
        private DataGridView dgvMergedNoun;
        private DataGridView dgvMergedAdv;
    }
}