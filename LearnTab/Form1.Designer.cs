namespace LearnTab
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            label1 = new Label();
            tabPage2 = new TabPage();
            label2 = new Label();
            tabPage3 = new TabPage();
            label3 = new Label();
            tabPage4 = new TabPage();
            label4 = new Label();
            addTabButton = new Button();
            removeTabButton = new Button();
            removeAllTabButton = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Location = new Point(12, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(655, 447);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(647, 414);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 24);
            label1.Name = "label1";
            label1.Size = new Size(80, 20);
            label1.TabIndex = 0;
            label1.Text = "Tab Page 1";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label2);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(647, 414);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 3);
            label2.Name = "label2";
            label2.Size = new Size(82, 20);
            label2.TabIndex = 0;
            label2.Text = "Tab page 2";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(label3);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(647, 414);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 0);
            label3.Name = "label3";
            label3.Size = new Size(82, 20);
            label3.TabIndex = 1;
            label3.Text = "Tab page 3";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(label4);
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(647, 414);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "tabPage4";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 9);
            label4.Name = "label4";
            label4.Size = new Size(82, 20);
            label4.TabIndex = 2;
            label4.Text = "Tab page 4";
            // 
            // addTabButton
            // 
            addTabButton.Location = new Point(16, 456);
            addTabButton.Name = "addTabButton";
            addTabButton.Size = new Size(158, 29);
            addTabButton.TabIndex = 1;
            addTabButton.Text = "Add New Tab";
            addTabButton.UseVisualStyleBackColor = true;
            addTabButton.Click += addTabButton_Click;
            // 
            // removeTabButton
            // 
            removeTabButton.Location = new Point(180, 456);
            removeTabButton.Name = "removeTabButton";
            removeTabButton.Size = new Size(161, 29);
            removeTabButton.TabIndex = 2;
            removeTabButton.Text = "Remove Tab";
            removeTabButton.UseVisualStyleBackColor = true;
            removeTabButton.Click += removeTabButton_Click;
            // 
            // removeAllTabButton
            // 
            removeAllTabButton.Location = new Point(16, 491);
            removeAllTabButton.Name = "removeAllTabButton";
            removeAllTabButton.Size = new Size(158, 29);
            removeAllTabButton.TabIndex = 3;
            removeAllTabButton.Text = "Remove All";
            removeAllTabButton.UseVisualStyleBackColor = true;
            removeAllTabButton.Click += removeAllTabButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(672, 594);
            Controls.Add(removeAllTabButton);
            Controls.Add(removeTabButton);
            Controls.Add(addTabButton);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Form1";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button addTabButton;
        private Button removeTabButton;
        private Button removeAllTabButton;
    }
}
