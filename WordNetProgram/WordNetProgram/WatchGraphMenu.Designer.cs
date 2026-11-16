namespace WordNetProgram
{
    partial class WatchGraphMenu
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
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            btNounParentChild = new System.Windows.Forms.Button();
            gbNoun = new System.Windows.Forms.GroupBox();
            btNounHolonym_Meronym = new System.Windows.Forms.Button();
            gbVerb = new System.Windows.Forms.GroupBox();
            btVerbParentChild = new System.Windows.Forms.Button();
            gbNoun.SuspendLayout();
            gbVerb.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(12, 23);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(243, 35);
            label1.TabIndex = 0;
            label1.Text = "Watch Graph Menu";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            label2.Location = new System.Drawing.Point(44, 68);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(361, 20);
            label2.TabIndex = 1;
            label2.Text = "Watch Relationship of a synset as a graph visualization";
            // 
            // btNounParentChild
            // 
            btNounParentChild.BackColor = System.Drawing.Color.White;
            btNounParentChild.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btNounParentChild.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btNounParentChild.ForeColor = System.Drawing.Color.Orange;
            btNounParentChild.Location = new System.Drawing.Point(16, 26);
            btNounParentChild.Name = "btNounParentChild";
            btNounParentChild.Size = new System.Drawing.Size(580, 53);
            btNounParentChild.TabIndex = 2;
            btNounParentChild.Text = "1. Hypernym-Hyponym (3 Tier Bidirectional Tree)";
            btNounParentChild.UseVisualStyleBackColor = false;
            btNounParentChild.Click += btNounParentChild_Click;
            // 
            // gbNoun
            // 
            gbNoun.Controls.Add(btNounHolonym_Meronym);
            gbNoun.Controls.Add(btNounParentChild);
            gbNoun.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            gbNoun.ForeColor = System.Drawing.Color.Orange;
            gbNoun.Location = new System.Drawing.Point(44, 117);
            gbNoun.Name = "gbNoun";
            gbNoun.Size = new System.Drawing.Size(621, 176);
            gbNoun.TabIndex = 3;
            gbNoun.TabStop = false;
            gbNoun.Text = "Noun Graph";
            // 
            // btNounHolonym_Meronym
            // 
            btNounHolonym_Meronym.BackColor = System.Drawing.Color.White;
            btNounHolonym_Meronym.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btNounHolonym_Meronym.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btNounHolonym_Meronym.ForeColor = System.Drawing.Color.Orange;
            btNounHolonym_Meronym.Location = new System.Drawing.Point(16, 95);
            btNounHolonym_Meronym.Name = "btNounHolonym_Meronym";
            btNounHolonym_Meronym.Size = new System.Drawing.Size(580, 53);
            btNounHolonym_Meronym.TabIndex = 3;
            btNounHolonym_Meronym.Text = "2. Part-Of (Holonym-Meronym)";
            btNounHolonym_Meronym.UseVisualStyleBackColor = false;
            btNounHolonym_Meronym.Click += btNounHolonym_Meronym_Click;
            // 
            // gbVerb
            // 
            gbVerb.Controls.Add(btVerbParentChild);
            gbVerb.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            gbVerb.ForeColor = System.Drawing.Color.RoyalBlue;
            gbVerb.Location = new System.Drawing.Point(44, 342);
            gbVerb.Name = "gbVerb";
            gbVerb.Size = new System.Drawing.Size(621, 94);
            gbVerb.TabIndex = 4;
            gbVerb.TabStop = false;
            gbVerb.Text = "Verb Graph";
            // 
            // btVerbParentChild
            // 
            btVerbParentChild.BackColor = System.Drawing.Color.White;
            btVerbParentChild.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btVerbParentChild.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btVerbParentChild.ForeColor = System.Drawing.Color.RoyalBlue;
            btVerbParentChild.Location = new System.Drawing.Point(16, 26);
            btVerbParentChild.Name = "btVerbParentChild";
            btVerbParentChild.Size = new System.Drawing.Size(580, 53);
            btVerbParentChild.TabIndex = 2;
            btVerbParentChild.Text = "1. Hypernym-Hyponym (3 Tier Bidirectional Tree)";
            btVerbParentChild.UseVisualStyleBackColor = false;
            btVerbParentChild.Click += btVerbParentChild_Click;
            // 
            // WatchGraphMenu
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 582);
            Controls.Add(gbVerb);
            Controls.Add(gbNoun);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "WatchGraphMenu";
            Text = "WatchGraphMenu";
            gbNoun.ResumeLayout(false);
            gbVerb.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btNounParentChild;
        private System.Windows.Forms.GroupBox gbNoun;
        private System.Windows.Forms.GroupBox gbVerb;
        private System.Windows.Forms.Button btVerbParentChild;
        private System.Windows.Forms.Button btNounHolonym_Meronym;
    }
}