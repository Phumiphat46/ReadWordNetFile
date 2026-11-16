namespace WordNetProgram
{
    partial class SearchData
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
            tbSearch = new System.Windows.Forms.TextBox();
            btSearch = new System.Windows.Forms.Button();
            dgvDisplayData = new System.Windows.Forms.DataGridView();
            line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            synset_offset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ss_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            en_cnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            en_words_and_lex_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            p_cnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            gloss = new System.Windows.Forms.DataGridViewTextBoxColumn();
            th_cnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            thai_words = new System.Windows.Forms.DataGridViewTextBoxColumn();
            th_src = new System.Windows.Forms.DataGridViewTextBoxColumn();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            cbSynsetOffset = new System.Windows.Forms.CheckBox();
            cbGloss = new System.Windows.Forms.CheckBox();
            cbThaiWord = new System.Windows.Forms.CheckBox();
            cbEngWord = new System.Windows.Forms.CheckBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            cbAll = new System.Windows.Forms.CheckBox();
            cbAdv = new System.Windows.Forms.CheckBox();
            cbVerb = new System.Windows.Forms.CheckBox();
            cbAdj = new System.Windows.Forms.CheckBox();
            cbNoun = new System.Windows.Forms.CheckBox();
            lbResultCount = new System.Windows.Forms.Label();
            cbThaiSource = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)dgvDisplayData).BeginInit();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // tbSearch
            // 
            tbSearch.Location = new System.Drawing.Point(191, 49);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new System.Drawing.Size(353, 27);
            tbSearch.TabIndex = 0;
            // 
            // btSearch
            // 
            btSearch.Location = new System.Drawing.Point(560, 49);
            btSearch.Name = "btSearch";
            btSearch.Size = new System.Drawing.Size(94, 29);
            btSearch.TabIndex = 1;
            btSearch.Text = "Search";
            btSearch.UseVisualStyleBackColor = true;
            btSearch.Click += btSearch_Click;
            // 
            // dgvDisplayData
            // 
            dgvDisplayData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvDisplayData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDisplayData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { line, synset_offset, ss_type, en_cnt, en_words_and_lex_id, p_cnt, gloss, th_cnt, thai_words, th_src });
            dgvDisplayData.Location = new System.Drawing.Point(191, 134);
            dgvDisplayData.Name = "dgvDisplayData";
            dgvDisplayData.RowHeadersWidth = 51;
            dgvDisplayData.Size = new System.Drawing.Size(1453, 435);
            dgvDisplayData.TabIndex = 2;
            // 
            // line
            // 
            line.HeaderText = "line";
            line.MinimumWidth = 6;
            line.Name = "line";
            line.Width = 125;
            // 
            // synset_offset
            // 
            synset_offset.HeaderText = "synset_offset";
            synset_offset.MinimumWidth = 6;
            synset_offset.Name = "synset_offset";
            synset_offset.Width = 125;
            // 
            // ss_type
            // 
            ss_type.HeaderText = "ss_type";
            ss_type.MinimumWidth = 6;
            ss_type.Name = "ss_type";
            ss_type.Width = 125;
            // 
            // en_cnt
            // 
            en_cnt.HeaderText = "en_cnt";
            en_cnt.MinimumWidth = 6;
            en_cnt.Name = "en_cnt";
            en_cnt.Width = 125;
            // 
            // en_words_and_lex_id
            // 
            en_words_and_lex_id.HeaderText = "en_words & lex_id";
            en_words_and_lex_id.MinimumWidth = 6;
            en_words_and_lex_id.Name = "en_words_and_lex_id";
            en_words_and_lex_id.Width = 125;
            // 
            // p_cnt
            // 
            p_cnt.HeaderText = "p_cnt";
            p_cnt.MinimumWidth = 6;
            p_cnt.Name = "p_cnt";
            p_cnt.Width = 125;
            // 
            // gloss
            // 
            gloss.HeaderText = "gloss";
            gloss.MinimumWidth = 6;
            gloss.Name = "gloss";
            gloss.Width = 125;
            // 
            // th_cnt
            // 
            th_cnt.HeaderText = "th_cnt";
            th_cnt.MinimumWidth = 6;
            th_cnt.Name = "th_cnt";
            th_cnt.Width = 125;
            // 
            // thai_words
            // 
            thai_words.HeaderText = "thai_words";
            thai_words.MinimumWidth = 6;
            thai_words.Name = "thai_words";
            thai_words.Width = 125;
            // 
            // th_src
            // 
            th_src.HeaderText = "th_src";
            th_src.MinimumWidth = 6;
            th_src.Name = "th_src";
            th_src.Width = 125;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Location = new System.Drawing.Point(12, 108);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(151, 421);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filter";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(cbThaiSource);
            groupBox3.Controls.Add(cbSynsetOffset);
            groupBox3.Controls.Add(cbGloss);
            groupBox3.Controls.Add(cbThaiWord);
            groupBox3.Controls.Add(cbEngWord);
            groupBox3.Location = new System.Drawing.Point(6, 205);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(140, 173);
            groupBox3.TabIndex = 7;
            groupBox3.TabStop = false;
            groupBox3.Text = "Field";
            // 
            // cbSynsetOffset
            // 
            cbSynsetOffset.AutoSize = true;
            cbSynsetOffset.Location = new System.Drawing.Point(6, 26);
            cbSynsetOffset.Name = "cbSynsetOffset";
            cbSynsetOffset.Size = new System.Drawing.Size(117, 24);
            cbSynsetOffset.TabIndex = 6;
            cbSynsetOffset.Text = "Synset Offset";
            cbSynsetOffset.UseVisualStyleBackColor = true;
            // 
            // cbGloss
            // 
            cbGloss.AutoSize = true;
            cbGloss.Location = new System.Drawing.Point(6, 97);
            cbGloss.Name = "cbGloss";
            cbGloss.Size = new System.Drawing.Size(66, 24);
            cbGloss.TabIndex = 5;
            cbGloss.Text = "Gloss";
            cbGloss.UseVisualStyleBackColor = true;
            // 
            // cbThaiWord
            // 
            cbThaiWord.AutoSize = true;
            cbThaiWord.Location = new System.Drawing.Point(6, 73);
            cbThaiWord.Name = "cbThaiWord";
            cbThaiWord.Size = new System.Drawing.Size(105, 24);
            cbThaiWord.TabIndex = 4;
            cbThaiWord.Text = "Thai Words";
            cbThaiWord.UseVisualStyleBackColor = true;
            // 
            // cbEngWord
            // 
            cbEngWord.AutoSize = true;
            cbEngWord.Location = new System.Drawing.Point(6, 50);
            cbEngWord.Name = "cbEngWord";
            cbEngWord.Size = new System.Drawing.Size(102, 24);
            cbEngWord.TabIndex = 3;
            cbEngWord.Text = "Eng Words";
            cbEngWord.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cbAll);
            groupBox2.Controls.Add(cbAdv);
            groupBox2.Controls.Add(cbVerb);
            groupBox2.Controls.Add(cbAdj);
            groupBox2.Controls.Add(cbNoun);
            groupBox2.Location = new System.Drawing.Point(6, 26);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(127, 173);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Part-Of-Speech";
            // 
            // cbAll
            // 
            cbAll.AutoSize = true;
            cbAll.Location = new System.Drawing.Point(5, 25);
            cbAll.Name = "cbAll";
            cbAll.Size = new System.Drawing.Size(49, 24);
            cbAll.TabIndex = 7;
            cbAll.Text = "All";
            cbAll.UseVisualStyleBackColor = true;
            cbAll.CheckedChanged += cbAll_CheckedChanged;
            // 
            // cbAdv
            // 
            cbAdv.AutoSize = true;
            cbAdv.Location = new System.Drawing.Point(5, 123);
            cbAdv.Name = "cbAdv";
            cbAdv.Size = new System.Drawing.Size(79, 24);
            cbAdv.TabIndex = 6;
            cbAdv.Text = "Adverb";
            cbAdv.UseVisualStyleBackColor = true;
            // 
            // cbVerb
            // 
            cbVerb.AutoSize = true;
            cbVerb.Location = new System.Drawing.Point(5, 97);
            cbVerb.Name = "cbVerb";
            cbVerb.Size = new System.Drawing.Size(61, 24);
            cbVerb.TabIndex = 5;
            cbVerb.Text = "Verb";
            cbVerb.UseVisualStyleBackColor = true;
            // 
            // cbAdj
            // 
            cbAdj.AutoSize = true;
            cbAdj.Location = new System.Drawing.Point(5, 71);
            cbAdj.Name = "cbAdj";
            cbAdj.Size = new System.Drawing.Size(93, 24);
            cbAdj.TabIndex = 4;
            cbAdj.Text = "Adjective";
            cbAdj.UseVisualStyleBackColor = true;
            // 
            // cbNoun
            // 
            cbNoun.AutoSize = true;
            cbNoun.Location = new System.Drawing.Point(5, 48);
            cbNoun.Name = "cbNoun";
            cbNoun.Size = new System.Drawing.Size(67, 24);
            cbNoun.TabIndex = 3;
            cbNoun.Text = "Noun";
            cbNoun.UseVisualStyleBackColor = true;
            // 
            // lbResultCount
            // 
            lbResultCount.AutoSize = true;
            lbResultCount.Location = new System.Drawing.Point(191, 108);
            lbResultCount.Name = "lbResultCount";
            lbResultCount.Size = new System.Drawing.Size(91, 20);
            lbResultCount.TabIndex = 4;
            lbResultCount.Text = "search result";
            // 
            // cbThaiSource
            // 
            cbThaiSource.AutoSize = true;
            cbThaiSource.Location = new System.Drawing.Point(6, 121);
            cbThaiSource.Name = "cbThaiSource";
            cbThaiSource.Size = new System.Drawing.Size(108, 24);
            cbThaiSource.TabIndex = 7;
            cbThaiSource.Text = "Thai Source";
            cbThaiSource.UseVisualStyleBackColor = true;
            // 
            // SearchData
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1722, 581);
            Controls.Add(lbResultCount);
            Controls.Add(groupBox1);
            Controls.Add(dgvDisplayData);
            Controls.Add(btSearch);
            Controls.Add(tbSearch);
            Name = "SearchData";
            Text = "SearchData";
            ((System.ComponentModel.ISupportInitialize)dgvDisplayData).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.DataGridView dgvDisplayData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbNoun;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbGloss;
        private System.Windows.Forms.CheckBox cbThaiWord;
        private System.Windows.Forms.CheckBox cbEngWord;
        private System.Windows.Forms.CheckBox cbAdv;
        private System.Windows.Forms.CheckBox cbVerb;
        private System.Windows.Forms.CheckBox cbAdj;
        private System.Windows.Forms.CheckBox cbAll;
        private System.Windows.Forms.Label lbResultCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn line;
        private System.Windows.Forms.DataGridViewTextBoxColumn synset_offset;
        private System.Windows.Forms.DataGridViewTextBoxColumn ss_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn en_cnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn en_words_and_lex_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn p_cnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn gloss;
        private System.Windows.Forms.DataGridViewTextBoxColumn th_cnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn thai_words;
        private System.Windows.Forms.DataGridViewTextBoxColumn th_src;
        private System.Windows.Forms.CheckBox cbSynsetOffset;
        private System.Windows.Forms.CheckBox cbThaiSource;
    }
}