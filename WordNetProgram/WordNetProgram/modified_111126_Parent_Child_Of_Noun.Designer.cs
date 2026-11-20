namespace WordNetProgram
{
    partial class modified_111126_Parent_Child_Of_Noun
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
            panel1 = new Panel();
            btZoomOut = new Button();
            label3 = new Label();
            label2 = new Label();
            btZoomIn = new Button();
            label1 = new Label();
            btGenGraph = new Button();
            btRevealPicInFolder = new Button();
            tbSynsetWord = new TextBox();
            splitContainer1 = new SplitContainer();
            pbGraph = new PictureBox();
            tabControl1 = new TabControl();
            tpEntities = new TabPage();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            label22 = new Label();
            label21 = new Label();
            label20 = new Label();
            label19 = new Label();
            label24 = new Label();
            label18 = new Label();
            tpFilter = new TabPage();
            label25 = new Label();
            tbHierarchy = new TextBox();
            label23 = new Label();
            label17 = new Label();
            tbFirstCousinOnceRemoved = new Label();
            tbMaxFirstCousin = new TextBox();
            label12 = new Label();
            tbMaxGreatUncleGreatAunt = new TextBox();
            label16 = new Label();
            label15 = new Label();
            label8 = new Label();
            tbMaxUncleAunt = new TextBox();
            label9 = new Label();
            tbMaxCousin = new TextBox();
            label10 = new Label();
            tbMaxNephew = new TextBox();
            label11 = new Label();
            tbMaxSibling = new TextBox();
            label14 = new Label();
            label13 = new Label();
            label4 = new Label();
            tbMaxGreatGrandChild = new TextBox();
            label6 = new Label();
            tbMaxGrandChild = new TextBox();
            label7 = new Label();
            tbMaxChild = new TextBox();
            label5 = new Label();
            tpNewFilter = new TabPage();
            label26 = new Label();
            tbNewHierarchy = new TextBox();
            label27 = new Label();
            label28 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbGraph).BeginInit();
            tabControl1.SuspendLayout();
            tpEntities.SuspendLayout();
            tpFilter.SuspendLayout();
            tpNewFilter.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Orange;
            panel1.Controls.Add(btZoomOut);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(btZoomIn);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btGenGraph);
            panel1.Controls.Add(btRevealPicInFolder);
            panel1.Controls.Add(tbSynsetWord);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1306, 125);
            panel1.TabIndex = 0;
            // 
            // btZoomOut
            // 
            btZoomOut.Location = new Point(1071, 55);
            btZoomOut.Name = "btZoomOut";
            btZoomOut.Size = new Size(105, 29);
            btZoomOut.TabIndex = 19;
            btZoomOut.Text = "Zoom Out (-)";
            btZoomOut.UseVisualStyleBackColor = true;
            btZoomOut.Click += btZoomOut_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label3.ForeColor = SystemColors.ActiveCaptionText;
            label3.Location = new Point(20, 66);
            label3.Name = "label3";
            label3.Size = new Size(65, 23);
            label3.TabIndex = 9;
            label3.Text = "- Noun";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(16, 43);
            label2.Name = "label2";
            label2.Size = new Size(122, 23);
            label2.TabIndex = 8;
            label2.Text = "(Parent-Child)";
            // 
            // btZoomIn
            // 
            btZoomIn.Location = new Point(960, 55);
            btZoomIn.Name = "btZoomIn";
            btZoomIn.Size = new Size(105, 29);
            btZoomIn.TabIndex = 18;
            btZoomIn.Text = "Zoom In (+)";
            btZoomIn.UseVisualStyleBackColor = true;
            btZoomIn.Click += btZoomIn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(17, 20);
            label1.Name = "label1";
            label1.Size = new Size(179, 23);
            label1.TabIndex = 7;
            label1.Text = "Hypernym-Hyponym";
            // 
            // btGenGraph
            // 
            btGenGraph.Location = new Point(584, 46);
            btGenGraph.Name = "btGenGraph";
            btGenGraph.Size = new Size(155, 32);
            btGenGraph.TabIndex = 6;
            btGenGraph.Text = "Generate Graph";
            btGenGraph.UseVisualStyleBackColor = true;
            btGenGraph.Click += btGenGraph_Click;
            // 
            // btRevealPicInFolder
            // 
            btRevealPicInFolder.Location = new Point(960, 20);
            btRevealPicInFolder.Name = "btRevealPicInFolder";
            btRevealPicInFolder.Size = new Size(216, 29);
            btRevealPicInFolder.TabIndex = 17;
            btRevealPicInFolder.Text = "Reveal in File Explorer";
            btRevealPicInFolder.UseVisualStyleBackColor = true;
            btRevealPicInFolder.Click += btRevealPicInFolder_Click;
            // 
            // tbSynsetWord
            // 
            tbSynsetWord.Font = new Font("Segoe UI", 11F);
            tbSynsetWord.Location = new Point(250, 46);
            tbSynsetWord.Name = "tbSynsetWord";
            tbSynsetWord.Size = new Size(300, 32);
            tbSynsetWord.TabIndex = 5;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.Location = new Point(0, 125);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(pbGraph);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabControl1);
            splitContainer1.Size = new Size(1306, 868);
            splitContainer1.SplitterDistance = 926;
            splitContainer1.TabIndex = 1;
            // 
            // pbGraph
            // 
            pbGraph.BackColor = Color.Gray;
            pbGraph.Location = new Point(31, 101);
            pbGraph.Name = "pbGraph";
            pbGraph.Size = new Size(861, 603);
            pbGraph.SizeMode = PictureBoxSizeMode.StretchImage;
            pbGraph.TabIndex = 1;
            pbGraph.TabStop = false;
            // 
            // tabControl1
            // 
            tabControl1.Alignment = TabAlignment.Left;
            tabControl1.Controls.Add(tpEntities);
            tabControl1.Controls.Add(tpFilter);
            tabControl1.Controls.Add(tpNewFilter);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(376, 868);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.TabIndex = 0;
            tabControl1.DrawItem += tabControl1_DrawItem;
            // 
            // tpEntities
            // 
            tpEntities.Controls.Add(button4);
            tpEntities.Controls.Add(button3);
            tpEntities.Controls.Add(button2);
            tpEntities.Controls.Add(label22);
            tpEntities.Controls.Add(label21);
            tpEntities.Controls.Add(label20);
            tpEntities.Controls.Add(label19);
            tpEntities.Controls.Add(label24);
            tpEntities.Controls.Add(label18);
            tpEntities.Location = new Point(30, 4);
            tpEntities.Name = "tpEntities";
            tpEntities.Padding = new Padding(3);
            tpEntities.Size = new Size(342, 860);
            tpEntities.TabIndex = 0;
            tpEntities.Text = "Entities";
            tpEntities.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.FlatStyle = FlatStyle.Flat;
            button4.Location = new Point(59, 329);
            button4.Name = "button4";
            button4.Size = new Size(216, 29);
            button4.TabIndex = 61;
            button4.Text = "button4";
            button4.TextAlign = ContentAlignment.MiddleLeft;
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.FlatStyle = FlatStyle.Flat;
            button3.Location = new Point(59, 294);
            button3.Name = "button3";
            button3.Size = new Size(216, 29);
            button3.TabIndex = 60;
            button3.Text = "button3";
            button3.TextAlign = ContentAlignment.MiddleLeft;
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.FlatStyle = FlatStyle.Flat;
            button2.Location = new Point(59, 259);
            button2.Name = "button2";
            button2.Size = new Size(216, 29);
            button2.TabIndex = 59;
            button2.Text = "button2";
            button2.TextAlign = ContentAlignment.MiddleLeft;
            button2.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Font = new Font("Segoe UI", 11F);
            label22.Location = new Point(59, 222);
            label22.Name = "label22";
            label22.Size = new Size(78, 25);
            label22.TabIndex = 57;
            label22.Text = "Related:";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Font = new Font("Segoe UI", 11F);
            label21.Location = new Point(59, 185);
            label21.Name = "label21";
            label21.Size = new Size(61, 25);
            label21.TabIndex = 56;
            label21.Text = "Gloss:";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Font = new Font("Segoe UI", 11F);
            label20.Location = new Point(57, 149);
            label20.Name = "label20";
            label20.Size = new Size(107, 25);
            label20.TabIndex = 55;
            label20.Text = "Eng Words:";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Font = new Font("Segoe UI", 11F);
            label19.Location = new Point(55, 109);
            label19.Name = "label19";
            label19.Size = new Size(111, 25);
            label19.TabIndex = 54;
            label19.Text = "Thai Words:";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Font = new Font("Segoe UI", 13F);
            label24.Location = new Point(45, 63);
            label24.Name = "label24";
            label24.Size = new Size(145, 30);
            label24.TabIndex = 53;
            label24.Text = "Synest Offset:";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label18.ForeColor = SystemColors.WindowText;
            label18.Location = new Point(36, 25);
            label18.Name = "label18";
            label18.Size = new Size(98, 32);
            label18.TabIndex = 52;
            label18.Text = "Entities";
            // 
            // tpFilter
            // 
            tpFilter.AutoScroll = true;
            tpFilter.Controls.Add(label25);
            tpFilter.Controls.Add(tbHierarchy);
            tpFilter.Controls.Add(label23);
            tpFilter.Controls.Add(label17);
            tpFilter.Controls.Add(tbFirstCousinOnceRemoved);
            tpFilter.Controls.Add(tbMaxFirstCousin);
            tpFilter.Controls.Add(label12);
            tpFilter.Controls.Add(tbMaxGreatUncleGreatAunt);
            tpFilter.Controls.Add(label16);
            tpFilter.Controls.Add(label15);
            tpFilter.Controls.Add(label8);
            tpFilter.Controls.Add(tbMaxUncleAunt);
            tpFilter.Controls.Add(label9);
            tpFilter.Controls.Add(tbMaxCousin);
            tpFilter.Controls.Add(label10);
            tpFilter.Controls.Add(tbMaxNephew);
            tpFilter.Controls.Add(label11);
            tpFilter.Controls.Add(tbMaxSibling);
            tpFilter.Controls.Add(label14);
            tpFilter.Controls.Add(label13);
            tpFilter.Controls.Add(label4);
            tpFilter.Controls.Add(tbMaxGreatGrandChild);
            tpFilter.Controls.Add(label6);
            tpFilter.Controls.Add(tbMaxGrandChild);
            tpFilter.Controls.Add(label7);
            tpFilter.Controls.Add(tbMaxChild);
            tpFilter.Controls.Add(label5);
            tpFilter.Location = new Point(30, 4);
            tpFilter.Name = "tpFilter";
            tpFilter.Padding = new Padding(3);
            tpFilter.Size = new Size(342, 860);
            tpFilter.TabIndex = 1;
            tpFilter.Text = "Filter";
            tpFilter.UseVisualStyleBackColor = true;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(40, 90);
            label25.Name = "label25";
            label25.Size = new Size(235, 20);
            label25.TabIndex = 77;
            label25.Text = "(both Upper and Lower Hierarchy)";
            // 
            // tbHierarchy
            // 
            tbHierarchy.Location = new Point(41, 121);
            tbHierarchy.Name = "tbHierarchy";
            tbHierarchy.Size = new Size(239, 27);
            tbHierarchy.TabIndex = 76;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(42, 70);
            label23.Name = "label23";
            label23.Size = new Size(151, 20);
            label23.TabIndex = 75;
            label23.Text = "Number of Hierarchy:";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(37, 778);
            label17.Name = "label17";
            label17.Size = new Size(259, 20);
            label17.TabIndex = 74;
            label17.Text = "(per Great-Uncle/Great-Aunt of Input)";
            // 
            // tbFirstCousinOnceRemoved
            // 
            tbFirstCousinOnceRemoved.AutoSize = true;
            tbFirstCousinOnceRemoved.Location = new Point(38, 755);
            tbFirstCousinOnceRemoved.Name = "tbFirstCousinOnceRemoved";
            tbFirstCousinOnceRemoved.Size = new Size(224, 20);
            tbFirstCousinOnceRemoved.TabIndex = 73;
            tbFirstCousinOnceRemoved.Text = "Max First Cousin Once Removed:";
            // 
            // tbMaxFirstCousin
            // 
            tbMaxFirstCousin.Location = new Point(38, 804);
            tbMaxFirstCousin.Name = "tbMaxFirstCousin";
            tbMaxFirstCousin.Size = new Size(192, 27);
            tbMaxFirstCousin.TabIndex = 72;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(38, 693);
            label12.Name = "label12";
            label12.Size = new Size(214, 20);
            label12.TabIndex = 71;
            label12.Text = "Max Great-Uncles/Great-Aunts:";
            // 
            // tbMaxGreatUncleGreatAunt
            // 
            tbMaxGreatUncleGreatAunt.Location = new Point(38, 718);
            tbMaxGreatUncleGreatAunt.Name = "tbMaxGreatUncleGreatAunt";
            tbMaxGreatUncleGreatAunt.Size = new Size(192, 27);
            tbMaxGreatUncleGreatAunt.TabIndex = 70;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(38, 623);
            label16.Name = "label16";
            label16.Size = new Size(175, 20);
            label16.TabIndex = 69;
            label16.Text = "(per Uncle/Aunt of input)";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(40, 485);
            label15.Name = "label15";
            label15.Size = new Size(147, 20);
            label15.TabIndex = 68;
            label15.Text = "(per Sibling of input)";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(41, 545);
            label8.Name = "label8";
            label8.Size = new Size(130, 20);
            label8.TabIndex = 67;
            label8.Text = "Max Uncles/Aunts:";
            // 
            // tbMaxUncleAunt
            // 
            tbMaxUncleAunt.Location = new Point(41, 568);
            tbMaxUncleAunt.Name = "tbMaxUncleAunt";
            tbMaxUncleAunt.Size = new Size(192, 27);
            tbMaxUncleAunt.TabIndex = 66;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(41, 603);
            label9.Name = "label9";
            label9.Size = new Size(94, 20);
            label9.TabIndex = 65;
            label9.Text = "Max Cousins:";
            // 
            // tbMaxCousin
            // 
            tbMaxCousin.Location = new Point(41, 649);
            tbMaxCousin.Name = "tbMaxCousin";
            tbMaxCousin.Size = new Size(192, 27);
            tbMaxCousin.TabIndex = 64;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(41, 465);
            label10.Name = "label10";
            label10.Size = new Size(105, 20);
            label10.TabIndex = 63;
            label10.Text = "Max Nephews:";
            // 
            // tbMaxNephew
            // 
            tbMaxNephew.Location = new Point(41, 508);
            tbMaxNephew.Name = "tbMaxNephew";
            tbMaxNephew.Size = new Size(192, 27);
            tbMaxNephew.TabIndex = 62;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(41, 405);
            label11.Name = "label11";
            label11.Size = new Size(152, 20);
            label11.TabIndex = 61;
            label11.Text = "Max Siblings of Input:";
            // 
            // tbMaxSibling
            // 
            tbMaxSibling.Location = new Point(41, 428);
            tbMaxSibling.Name = "tbMaxSibling";
            tbMaxSibling.Size = new Size(192, 27);
            tbMaxSibling.TabIndex = 60;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(40, 338);
            label14.Name = "label14";
            label14.Size = new Size(175, 20);
            label14.TabIndex = 59;
            label14.Text = "(per GrandChild of Input)";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(40, 251);
            label13.Name = "label13";
            label13.Size = new Size(135, 20);
            label13.TabIndex = 58;
            label13.Text = "(per Child of Input)";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(41, 318);
            label4.Name = "label4";
            label4.Size = new Size(181, 20);
            label4.TabIndex = 57;
            label4.Text = "Max Great-GrandChildren:";
            // 
            // tbMaxGreatGrandChild
            // 
            tbMaxGreatGrandChild.Location = new Point(41, 366);
            tbMaxGreatGrandChild.Name = "tbMaxGreatGrandChild";
            tbMaxGreatGrandChild.Size = new Size(192, 27);
            tbMaxGreatGrandChild.TabIndex = 56;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(41, 231);
            label6.Name = "label6";
            label6.Size = new Size(139, 20);
            label6.TabIndex = 55;
            label6.Text = "Max GrandChildren:";
            // 
            // tbMaxGrandChild
            // 
            tbMaxGrandChild.Location = new Point(41, 277);
            tbMaxGrandChild.Name = "tbMaxGrandChild";
            tbMaxGrandChild.Size = new Size(192, 27);
            tbMaxGrandChild.TabIndex = 54;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(41, 166);
            label7.Name = "label7";
            label7.Size = new Size(99, 20);
            label7.TabIndex = 53;
            label7.Text = "Max Children:";
            // 
            // tbMaxChild
            // 
            tbMaxChild.Location = new Point(41, 189);
            tbMaxChild.Name = "tbMaxChild";
            tbMaxChild.Size = new Size(192, 27);
            tbMaxChild.TabIndex = 52;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label5.ForeColor = SystemColors.WindowText;
            label5.Location = new Point(35, 21);
            label5.Name = "label5";
            label5.Size = new Size(72, 32);
            label5.TabIndex = 51;
            label5.Text = "Filter";
            // 
            // tpNewFilter
            // 
            tpNewFilter.Controls.Add(label26);
            tpNewFilter.Controls.Add(tbNewHierarchy);
            tpNewFilter.Controls.Add(label27);
            tpNewFilter.Controls.Add(label28);
            tpNewFilter.Location = new Point(30, 4);
            tpNewFilter.Name = "tpNewFilter";
            tpNewFilter.Size = new Size(342, 860);
            tpNewFilter.TabIndex = 2;
            tpNewFilter.Text = "New Filter";
            tpNewFilter.UseVisualStyleBackColor = true;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(40, 90);
            label26.Name = "label26";
            label26.Size = new Size(235, 20);
            label26.TabIndex = 81;
            label26.Text = "(both Upper and Lower Hierarchy)";
            // 
            // tbNewHierarchy
            // 
            tbNewHierarchy.Location = new Point(41, 121);
            tbNewHierarchy.Name = "tbNewHierarchy";
            tbNewHierarchy.Size = new Size(239, 27);
            tbNewHierarchy.TabIndex = 80;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(42, 70);
            label27.Name = "label27";
            label27.Size = new Size(151, 20);
            label27.TabIndex = 79;
            label27.Text = "Number of Hierarchy:";
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label28.ForeColor = SystemColors.WindowText;
            label28.Location = new Point(35, 21);
            label28.Name = "label28";
            label28.Size = new Size(72, 32);
            label28.TabIndex = 78;
            label28.Text = "Filter";
            // 
            // modified_111126_Parent_Child_Of_Noun
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1306, 993);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            Name = "modified_111126_Parent_Child_Of_Noun";
            Text = "_111126_Parent_Child_Of_Noun";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbGraph).EndInit();
            tabControl1.ResumeLayout(false);
            tpEntities.ResumeLayout(false);
            tpEntities.PerformLayout();
            tpFilter.ResumeLayout(false);
            tpFilter.PerformLayout();
            tpNewFilter.ResumeLayout(false);
            tpNewFilter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btGenGraph;
        private System.Windows.Forms.TextBox tbSynsetWord;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpEntities;
        private System.Windows.Forms.TabPage tpFilter;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label tbFirstCousinOnceRemoved;
        private System.Windows.Forms.TextBox tbMaxFirstCousin;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbMaxGreatUncleGreatAunt;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbMaxUncleAunt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbMaxCousin;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbMaxNephew;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbMaxSibling;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbMaxGreatGrandChild;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbMaxGrandChild;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbMaxChild;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.PictureBox pbGraph;
        private System.Windows.Forms.Button btZoomOut;
        private System.Windows.Forms.Button btZoomIn;
        private System.Windows.Forms.Button btRevealPicInFolder;
        private TextBox tbHierarchy;
        private Label label23;
        private Label label25;
        private TabPage tpNewFilter;
        private Label label26;
        private TextBox tbNewHierarchy;
        private Label label27;
        private Label label28;
    }
}