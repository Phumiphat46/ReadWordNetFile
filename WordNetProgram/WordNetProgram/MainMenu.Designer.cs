namespace WordNetProgram
{
    partial class MainMenu
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
            btConvertToJson = new Button();
            btWatchGraph = new Button();
            btSearchData = new Button();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            addThaiWordToolStripMenuItem = new ToolStripMenuItem();
            convertTheMergedToJSONToolStripMenuItem = new ToolStripMenuItem();
            watchRelationshipGraphToolStripMenuItem = new ToolStripMenuItem();
            searchToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            gbApplication = new GroupBox();
            button1 = new Button();
            btAddThaiToWordNet = new Button();
            Title = new Label();
            menuStrip1.SuspendLayout();
            gbApplication.SuspendLayout();
            SuspendLayout();
            // 
            // btConvertToJson
            // 
            btConvertToJson.Location = new Point(136, 114);
            btConvertToJson.Name = "btConvertToJson";
            btConvertToJson.Size = new Size(288, 29);
            btConvertToJson.TabIndex = 0;
            btConvertToJson.Text = "2. Convert Data to JSON";
            btConvertToJson.UseVisualStyleBackColor = true;
            btConvertToJson.Click += btConvertToJson_Click;
            // 
            // btWatchGraph
            // 
            btWatchGraph.Location = new Point(136, 149);
            btWatchGraph.Name = "btWatchGraph";
            btWatchGraph.Size = new Size(288, 29);
            btWatchGraph.TabIndex = 1;
            btWatchGraph.Text = "3. Watch Relationship Graph";
            btWatchGraph.UseVisualStyleBackColor = true;
            btWatchGraph.Click += btWatchGraph_Click;
            // 
            // btSearchData
            // 
            btSearchData.Location = new Point(136, 184);
            btSearchData.Name = "btSearchData";
            btSearchData.Size = new Size(288, 29);
            btSearchData.TabIndex = 2;
            btSearchData.Text = "4. Search Data";
            btSearchData.UseVisualStyleBackColor = true;
            btSearchData.Click += btSearchData_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(867, 28);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addThaiWordToolStripMenuItem, convertTheMergedToJSONToolStripMenuItem, watchRelationshipGraphToolStripMenuItem, searchToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(100, 24);
            fileToolStripMenuItem.Text = "Application";
            // 
            // addThaiWordToolStripMenuItem
            // 
            addThaiWordToolStripMenuItem.Name = "addThaiWordToolStripMenuItem";
            addThaiWordToolStripMenuItem.Size = new Size(299, 26);
            addThaiWordToolStripMenuItem.Text = "1. Add Thai Word";
            addThaiWordToolStripMenuItem.Click += btAddThaiToWordNet_Click;
            // 
            // convertTheMergedToJSONToolStripMenuItem
            // 
            convertTheMergedToJSONToolStripMenuItem.Name = "convertTheMergedToJSONToolStripMenuItem";
            convertTheMergedToJSONToolStripMenuItem.Size = new Size(299, 26);
            convertTheMergedToJSONToolStripMenuItem.Text = "2. Convert The Merged to JSON";
            convertTheMergedToJSONToolStripMenuItem.Click += btConvertToJson_Click;
            // 
            // watchRelationshipGraphToolStripMenuItem
            // 
            watchRelationshipGraphToolStripMenuItem.Name = "watchRelationshipGraphToolStripMenuItem";
            watchRelationshipGraphToolStripMenuItem.Size = new Size(299, 26);
            watchRelationshipGraphToolStripMenuItem.Text = "3. Watch Relationship Graph";
            watchRelationshipGraphToolStripMenuItem.Click += btWatchGraph_Click;
            // 
            // searchToolStripMenuItem
            // 
            searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            searchToolStripMenuItem.Size = new Size(299, 26);
            searchToolStripMenuItem.Text = "4. Search";
            searchToolStripMenuItem.Click += btSearchData_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(299, 26);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(64, 24);
            aboutToolStripMenuItem.Text = "About";
            // 
            // gbApplication
            // 
            gbApplication.Controls.Add(button1);
            gbApplication.Controls.Add(btAddThaiToWordNet);
            gbApplication.Controls.Add(btSearchData);
            gbApplication.Controls.Add(btConvertToJson);
            gbApplication.Controls.Add(btWatchGraph);
            gbApplication.Location = new Point(138, 144);
            gbApplication.Name = "gbApplication";
            gbApplication.Size = new Size(577, 293);
            gbApplication.TabIndex = 5;
            gbApplication.TabStop = false;
            gbApplication.Text = "Application";
            // 
            // button1
            // 
            button1.Location = new Point(136, 219);
            button1.Name = "button1";
            button1.Size = new Size(288, 29);
            button1.TabIndex = 8;
            button1.Text = "5. Thai Sentence Analyzer";
            button1.UseVisualStyleBackColor = true;
            // 
            // btAddThaiToWordNet
            // 
            btAddThaiToWordNet.Location = new Point(136, 79);
            btAddThaiToWordNet.Name = "btAddThaiToWordNet";
            btAddThaiToWordNet.Size = new Size(288, 29);
            btAddThaiToWordNet.TabIndex = 7;
            btAddThaiToWordNet.Text = "1. Add Thai to WordNet";
            btAddThaiToWordNet.UseVisualStyleBackColor = true;
            btAddThaiToWordNet.Click += btAddThaiToWordNet_Click;
            // 
            // Title
            // 
            Title.AutoSize = true;
            Title.Font = new Font("Sans Serif Collection", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Title.Location = new Point(125, 89);
            Title.Name = "Title";
            Title.Size = new Size(301, 78);
            Title.TabIndex = 6;
            Title.Text = "Taxonomy of Thai Word";
            // 
            // MainMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(867, 483);
            Controls.Add(gbApplication);
            Controls.Add(menuStrip1);
            Controls.Add(Title);
            MainMenuStrip = menuStrip1;
            Name = "MainMenu";
            Text = "MainMenu";
            Load += MainMenu_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            gbApplication.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btConvertToJson;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btSearchData;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox gbApplication;
        private System.Windows.Forms.ToolStripMenuItem addThaiWordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertTheMergedToJSONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button btAddThaiToWordNet;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btWatchGraph;
        private System.Windows.Forms.ToolStripMenuItem watchRelationshipGraphToolStripMenuItem;
        private System.Windows.Forms.Button button1;
    }
}