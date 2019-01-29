namespace filtry_graficzne_csharp
{
    partial class wielkosc_maski
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbWspolczynnikQ = new System.Windows.Forms.TextBox();
            this.btWykonaj = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "q = ";
            // 
            // tbWspolczynnikQ
            // 
            this.tbWspolczynnikQ.Location = new System.Drawing.Point(44, 10);
            this.tbWspolczynnikQ.Name = "tbWspolczynnikQ";
            this.tbWspolczynnikQ.Size = new System.Drawing.Size(100, 20);
            this.tbWspolczynnikQ.TabIndex = 1;
            // 
            // btWykonaj
            // 
            this.btWykonaj.Location = new System.Drawing.Point(69, 36);
            this.btWykonaj.Name = "btWykonaj";
            this.btWykonaj.Size = new System.Drawing.Size(75, 23);
            this.btWykonaj.TabIndex = 2;
            this.btWykonaj.Text = "wykonaj";
            this.btWykonaj.UseVisualStyleBackColor = true;
            this.btWykonaj.Click += new System.EventHandler(this.btWykonaj_Click);
            // 
            // wielkosc_maski
            // 
            this.AcceptButton = this.btWykonaj;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(167, 66);
            this.ControlBox = false;
            this.Controls.Add(this.btWykonaj);
            this.Controls.Add(this.tbWspolczynnikQ);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "wielkosc_maski";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Podaj wielkość";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbWspolczynnikQ;
        private System.Windows.Forms.Button btWykonaj;
    }
}