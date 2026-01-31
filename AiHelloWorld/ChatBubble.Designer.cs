namespace AiHelloWorld
{
    partial class ChatBubble
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.lblText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblText
			// 
			this.lblText.AutoSize = true;
			this.lblText.Location = new System.Drawing.Point(10, 10);
			this.lblText.Margin = new System.Windows.Forms.Padding(10);
			this.lblText.Name = "lblText";
			this.lblText.Padding = new System.Windows.Forms.Padding(10);
			this.lblText.Size = new System.Drawing.Size(55, 33);
			this.lblText.TabIndex = 0;
			this.lblText.Text = "label1";
			// 
			// ChatBubble
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblText);
			this.Name = "ChatBubble";
			this.Size = new System.Drawing.Size(272, 50);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblText;
    }
}
