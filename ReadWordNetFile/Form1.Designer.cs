namespace ReadWordNetFile
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            label2 = new Label();
            filesPathTextBox = new TextBox();
            nounPathTextBox = new TextBox();
            buttonFilesPath = new Button();
            textBox3 = new TextBox();
            label3 = new Label();
            textBox4 = new TextBox();
            label4 = new Label();
            textBox5 = new TextBox();
            label5 = new Label();
            buttonNounPath = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            dataGridView1 = new DataGridView();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
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
            fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(116, 26);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(58, 66);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 1;
            label1.Text = "Files Path";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(58, 113);
            label2.Name = "label2";
            label2.Size = new Size(45, 20);
            label2.TabIndex = 2;
            label2.Text = "Noun";
            // 
            // filesPathTextBox
            // 
            filesPathTextBox.Location = new Point(166, 66);
            filesPathTextBox.Name = "filesPathTextBox";
            filesPathTextBox.Size = new Size(335, 27);
            filesPathTextBox.TabIndex = 3;
            // 
            // nounPathTextBox
            // 
            nounPathTextBox.Location = new Point(166, 110);
            nounPathTextBox.Name = "nounPathTextBox";
            nounPathTextBox.Size = new Size(335, 27);
            nounPathTextBox.TabIndex = 4;
            // 
            // buttonFilesPath
            // 
            buttonFilesPath.Location = new Point(518, 66);
            buttonFilesPath.Name = "buttonFilesPath";
            buttonFilesPath.Size = new Size(94, 29);
            buttonFilesPath.TabIndex = 5;
            buttonFilesPath.Text = ".......";
            buttonFilesPath.UseVisualStyleBackColor = true;
            buttonFilesPath.Click += buttonFilesPath_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(166, 161);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(335, 27);
            textBox3.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(58, 164);
            label3.Name = "label3";
            label3.Size = new Size(39, 20);
            label3.TabIndex = 6;
            label3.Text = "Verb";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(166, 213);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(335, 27);
            textBox4.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(58, 216);
            label4.Name = "label4";
            label4.Size = new Size(71, 20);
            label4.TabIndex = 8;
            label4.Text = "Adjective";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(166, 259);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(335, 27);
            textBox5.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(58, 262);
            label5.Name = "label5";
            label5.Size = new Size(57, 20);
            label5.TabIndex = 10;
            label5.Text = "Adverb";
            // 
            // buttonNounPath
            // 
            buttonNounPath.Location = new Point(518, 110);
            buttonNounPath.Name = "buttonNounPath";
            buttonNounPath.Size = new Size(94, 29);
            buttonNounPath.TabIndex = 12;
            buttonNounPath.Text = ".......";
            buttonNounPath.UseVisualStyleBackColor = true;
            buttonNounPath.Click += buttonNounPath_Click;
            // 
            // button3
            // 
            button3.Location = new Point(518, 164);
            button3.Name = "button3";
            button3.Size = new Size(94, 29);
            button3.TabIndex = 13;
            button3.Text = ".......";
            button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(518, 213);
            button4.Name = "button4";
            button4.Size = new Size(94, 29);
            button4.TabIndex = 14;
            button4.Text = ".......";
            button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new Point(518, 253);
            button5.Name = "button5";
            button5.Size = new Size(94, 29);
            button5.TabIndex = 15;
            button5.Text = ".......";
            button5.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(43, 327);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(701, 270);
            dataGridView1.TabIndex = 16;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 609);
            Controls.Add(dataGridView1);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(buttonNounPath);
            Controls.Add(textBox5);
            Controls.Add(label5);
            Controls.Add(textBox4);
            Controls.Add(label4);
            Controls.Add(textBox3);
            Controls.Add(label3);
            Controls.Add(buttonFilesPath);
            Controls.Add(nounPathTextBox);
            Controls.Add(filesPathTextBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private Label label1;
        private Label label2;
        private TextBox filesPathTextBox;
        private TextBox nounPathTextBox;
        private Button buttonFilesPath;
        private TextBox textBox3;
        private Label label3;
        private TextBox textBox4;
        private Label label4;
        private TextBox textBox5;
        private Label label5;
        private Button buttonNounPath;
        private Button button3;
        private Button button4;
        private Button button5;
        private DataGridView dataGridView1;
    }
}
