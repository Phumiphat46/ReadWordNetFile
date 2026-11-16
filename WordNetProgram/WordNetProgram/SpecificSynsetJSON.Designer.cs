namespace WordNetProgram
{
    partial class SpecificSynsetJSON
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
            rtbJSON = new System.Windows.Forms.RichTextBox();
            SuspendLayout();
            // 
            // rtbJSON
            // 
            rtbJSON.Location = new System.Drawing.Point(75, 81);
            rtbJSON.Name = "rtbJSON";
            rtbJSON.Size = new System.Drawing.Size(623, 341);
            rtbJSON.TabIndex = 0;
            rtbJSON.Text = "";
            // 
            // SpecificSynsetJSON
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(rtbJSON);
            Name = "SpecificSynsetJSON";
            Text = "SpecificSynsetJSON";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbJSON;
    }
}