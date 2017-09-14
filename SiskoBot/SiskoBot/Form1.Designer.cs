namespace SiskoBot
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Btn_RunClick = new System.Windows.Forms.Button();
            this.Btn_Stop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(259, 20);
            this.textBox1.TabIndex = 0;
            // 
            // Btn_RunClick
            // 
            this.Btn_RunClick.Location = new System.Drawing.Point(12, 38);
            this.Btn_RunClick.Name = "Btn_RunClick";
            this.Btn_RunClick.Size = new System.Drawing.Size(75, 23);
            this.Btn_RunClick.TabIndex = 1;
            this.Btn_RunClick.Text = "Pognaley";
            this.Btn_RunClick.UseVisualStyleBackColor = true;
            this.Btn_RunClick.Click += new System.EventHandler(this.BtnRunClick);
            // 
            // Btn_Stop
            // 
            this.Btn_Stop.Location = new System.Drawing.Point(94, 38);
            this.Btn_Stop.Name = "Btn_Stop";
            this.Btn_Stop.Size = new System.Drawing.Size(75, 23);
            this.Btn_Stop.TabIndex = 2;
            this.Btn_Stop.Text = "Stop";
            this.Btn_Stop.UseVisualStyleBackColor = true;
            this.Btn_Stop.Click += new System.EventHandler(this.BtnStopClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 78);
            this.Controls.Add(this.Btn_Stop);
            this.Controls.Add(this.Btn_RunClick);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Btn_RunClick;
        private System.Windows.Forms.Button Btn_Stop;
    }
}

