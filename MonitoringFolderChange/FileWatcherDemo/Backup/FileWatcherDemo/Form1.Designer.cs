namespace FileWatcherDemo
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnTime = new System.Windows.Forms.ColumnHeader();
			this.columnEvent = new System.Windows.Forms.ColumnHeader();
			this.columnFilter = new System.Windows.Forms.ColumnHeader();
			this.columnFileName = new System.Windows.Forms.ColumnHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxFolderToWatch = new System.Windows.Forms.TextBox();
			this.buttonStart = new System.Windows.Forms.Button();
			this.buttonStop = new System.Windows.Forms.Button();
			this.buttonDone = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonBrowse = new System.Windows.Forms.Button();
			this.checkBoxIncludeSubfolders = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTime,
            this.columnEvent,
            this.columnFilter,
            this.columnFileName});
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.Location = new System.Drawing.Point(7, 59);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(609, 507);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// columnTime
			// 
			this.columnTime.Text = "Time";
			this.columnTime.Width = 50;
			// 
			// columnEvent
			// 
			this.columnEvent.Text = "Event";
			this.columnEvent.Width = 80;
			// 
			// columnFilter
			// 
			this.columnFilter.Text = "Filter";
			this.columnFilter.Width = 80;
			// 
			// columnFileName
			// 
			this.columnFileName.Text = "File Name";
			this.columnFileName.Width = 439;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Folder to watch";
			// 
			// textBoxFolderToWatch
			// 
			this.textBoxFolderToWatch.Location = new System.Drawing.Point(90, 8);
			this.textBoxFolderToWatch.Name = "textBoxFolderToWatch";
			this.textBoxFolderToWatch.Size = new System.Drawing.Size(376, 20);
			this.textBoxFolderToWatch.TabIndex = 2;
			this.textBoxFolderToWatch.TextChanged += new System.EventHandler(this.textBoxFolderToWatch_TextChanged);
			// 
			// buttonStart
			// 
			this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonStart.Location = new System.Drawing.Point(191, 572);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(75, 23);
			this.buttonStart.TabIndex = 3;
			this.buttonStart.Text = "Start";
			this.buttonStart.UseVisualStyleBackColor = true;
			this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
			// 
			// buttonStop
			// 
			this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonStop.Location = new System.Drawing.Point(276, 572);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new System.Drawing.Size(75, 23);
			this.buttonStop.TabIndex = 4;
			this.buttonStop.Text = "Stop";
			this.buttonStop.UseVisualStyleBackColor = true;
			this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
			// 
			// buttonDone
			// 
			this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonDone.Location = new System.Drawing.Point(361, 572);
			this.buttonDone.Name = "buttonDone";
			this.buttonDone.Size = new System.Drawing.Size(75, 23);
			this.buttonDone.TabIndex = 5;
			this.buttonDone.Text = "Done";
			this.buttonDone.UseVisualStyleBackColor = true;
			this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(143, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Detected File System Events";
			// 
			// buttonBrowse
			// 
			this.buttonBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonBrowse.Location = new System.Drawing.Point(468, 6);
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.Size = new System.Drawing.Size(33, 23);
			this.buttonBrowse.TabIndex = 7;
			this.buttonBrowse.Text = "...";
			this.buttonBrowse.UseVisualStyleBackColor = true;
			this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
			// 
			// checkBoxIncludeSubfolders
			// 
			this.checkBoxIncludeSubfolders.AutoSize = true;
			this.checkBoxIncludeSubfolders.Location = new System.Drawing.Point(512, 11);
			this.checkBoxIncludeSubfolders.Name = "checkBoxIncludeSubfolders";
			this.checkBoxIncludeSubfolders.Size = new System.Drawing.Size(112, 17);
			this.checkBoxIncludeSubfolders.TabIndex = 8;
			this.checkBoxIncludeSubfolders.Text = "Include subfolders";
			this.checkBoxIncludeSubfolders.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 602);
			this.Controls.Add(this.checkBoxIncludeSubfolders);
			this.Controls.Add(this.buttonBrowse);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.buttonDone);
			this.Controls.Add(this.buttonStop);
			this.Controls.Add(this.buttonStart);
			this.Controls.Add(this.textBoxFolderToWatch);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listView1);
			this.Name = "Form1";
			this.Text = "File Watcher Demo";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnTime;
		private System.Windows.Forms.ColumnHeader columnEvent;
		private System.Windows.Forms.ColumnHeader columnFilter;
		private System.Windows.Forms.ColumnHeader columnFileName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxFolderToWatch;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.Button buttonDone;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.CheckBox checkBoxIncludeSubfolders;
	}
}

