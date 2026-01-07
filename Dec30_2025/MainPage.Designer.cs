namespace Dec30_2025
{
    partial class MainPage
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
            btBrowse = new Button();
            label1 = new Label();
            tbFilePath = new TextBox();
            dataGridView1 = new DataGridView();
            btChildForm = new Button();
            lbChildForm = new Label();
            number = new DataGridViewTextBoxColumn();
            synsetOffset = new DataGridViewTextBoxColumn();
            data = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // btBrowse
            // 
            btBrowse.Location = new Point(562, 53);
            btBrowse.Name = "btBrowse";
            btBrowse.Size = new Size(94, 29);
            btBrowse.TabIndex = 7;
            btBrowse.Text = "Browse File";
            btBrowse.UseVisualStyleBackColor = true;
            btBrowse.Click += btBrowse_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(39, 56);
            label1.Name = "label1";
            label1.Size = new Size(64, 20);
            label1.TabIndex = 6;
            label1.Text = "File Path";
            // 
            // tbFilePath
            // 
            tbFilePath.Location = new Point(109, 53);
            tbFilePath.Name = "tbFilePath";
            tbFilePath.Size = new Size(421, 27);
            tbFilePath.TabIndex = 5;
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { number, synsetOffset, data, Column1, Column2, Column3, Column4 });
            dataGridView1.Location = new Point(12, 261);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1104, 366);
            dataGridView1.TabIndex = 8;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // btChildForm
            // 
            btChildForm.Location = new Point(135, 109);
            btChildForm.Name = "btChildForm";
            btChildForm.Size = new Size(94, 29);
            btChildForm.TabIndex = 9;
            btChildForm.Text = "Go";
            btChildForm.UseVisualStyleBackColor = true;
            // 
            // lbChildForm
            // 
            lbChildForm.AutoSize = true;
            lbChildForm.Location = new Point(39, 113);
            lbChildForm.Name = "lbChildForm";
            lbChildForm.Size = new Size(80, 20);
            lbChildForm.TabIndex = 10;
            lbChildForm.Text = "ChildForm:";
            // 
            // number
            // 
            number.HeaderText = "index";
            number.MinimumWidth = 6;
            number.Name = "number";
            number.Width = 125;
            // 
            // synsetOffset
            // 
            synsetOffset.HeaderText = "Synset Offset";
            synsetOffset.MinimumWidth = 6;
            synsetOffset.Name = "synsetOffset";
            synsetOffset.Width = 125;
            // 
            // data
            // 
            data.HeaderText = "Data";
            data.MinimumWidth = 6;
            data.Name = "data";
            data.Width = 125;
            // 
            // Column1
            // 
            Column1.HeaderText = "SS_Type";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.Width = 125;
            // 
            // Column2
            // 
            Column2.HeaderText = "W_cnt";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            Column2.Width = 125;
            // 
            // Column3
            // 
            Column3.HeaderText = "P_Cnt";
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            Column3.Width = 125;
            // 
            // Column4
            // 
            Column4.HeaderText = "Gloss";
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            Column4.Width = 125;
            // 
            // MainPage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1128, 660);
            Controls.Add(lbChildForm);
            Controls.Add(btChildForm);
            Controls.Add(dataGridView1);
            Controls.Add(btBrowse);
            Controls.Add(label1);
            Controls.Add(tbFilePath);
            Name = "MainPage";
            Text = "Main Page";
            Load += MainPage_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btBrowse;
        private Label label1;
        private TextBox tbFilePath;
        private DataGridView dataGridView1;
        private Button btChildForm;
        private Label lbChildForm;
        private DataGridViewButtonColumn view;
        private DataGridViewTextBoxColumn number;
        private DataGridViewTextBoxColumn synsetOffset;
        private DataGridViewTextBoxColumn data;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
    }
}
