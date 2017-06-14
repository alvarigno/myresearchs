namespace publicacionCA_DM_CefSharp
{
    partial class frmPrincipal
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.chileautosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demotoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copiarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.publicarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chileautosToolStripMenuItem,
            this.demotoresToolStripMenuItem,
            this.copiarToolStripMenuItem,
            this.publicarToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(599, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // chileautosToolStripMenuItem
            // 
            this.chileautosToolStripMenuItem.Name = "chileautosToolStripMenuItem";
            this.chileautosToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.chileautosToolStripMenuItem.Text = "Chileautos";
            this.chileautosToolStripMenuItem.Click += new System.EventHandler(this.chileautosToolStripMenuItem_Click);
            // 
            // demotoresToolStripMenuItem
            // 
            this.demotoresToolStripMenuItem.Name = "demotoresToolStripMenuItem";
            this.demotoresToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.demotoresToolStripMenuItem.Text = "Demotores";
            this.demotoresToolStripMenuItem.Click += new System.EventHandler(this.demotoresToolStripMenuItem_Click);
            // copiarToolStripMenuItem
            // 
            this.copiarToolStripMenuItem.BackColor = System.Drawing.Color.Teal;
            this.copiarToolStripMenuItem.CheckOnClick = true;
            this.copiarToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Window;
            this.copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
            this.copiarToolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.copiarToolStripMenuItem.Text = "Copiar";
            this.copiarToolStripMenuItem.Click += new System.EventHandler(this.copiarToolStripMenuItem_Click);
            // 
            // publicarToolStripMenuItem
            // 
            this.publicarToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.publicarToolStripMenuItem.ForeColor = System.Drawing.Color.MediumBlue;
            this.publicarToolStripMenuItem.Name = "publicarToolStripMenuItem";
            this.publicarToolStripMenuItem.Size = new System.Drawing.Size(85, 25);
            this.publicarToolStripMenuItem.Text = "Publicar";
            this.publicarToolStripMenuItem.Click += new System.EventHandler(this.publicarToolStripMenuItem_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 353);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmPrincipal";
            this.Text = "Publicación simultánea Chileautos y Demotores";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem chileautosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem demotoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copiarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem publicarToolStripMenuItem;
    }
}

