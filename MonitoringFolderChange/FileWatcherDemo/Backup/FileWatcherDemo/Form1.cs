using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FileWatcher;

namespace FileWatcherDemo
{
	public partial class Form1 : Form
	{
		Watcher fileWatcher = null;

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor
		/// </summary>
		public Form1()
		{
			InitializeComponent();

			this.textBoxFolderToWatch.Text = @"d:\tempo";
			this.buttonStart.Enabled = true;
			this.buttonStop.Enabled = false;
		}

		#region Helper Methods
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Toggles the Watcher event handlers on/off
		/// </summary>
		/// <param name="add"></param>
		private void ManageEventHandlers(bool add)
		{
			// For the purposes of this demo, I funneled all of the change events into 
			// one handler to keep the codebase smaller. You could certainly have an 
			// individual handler for each type of change event if you desire.
			if (fileWatcher != null)
			{
				if (add)
				{
					fileWatcher.EventChangedAttribute     += new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedCreationTime  += new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedDirectoryName += new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedFileName      += new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedLastAccess    += new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedLastWrite     += new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedSecurity      += new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedSize          += new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventCreated              += new WatcherEventHandler(fileWatcher_EventCreated);
					fileWatcher.EventDeleted              += new WatcherEventHandler(fileWatcher_EventDeleted);
					fileWatcher.EventDisposed             += new WatcherEventHandler(fileWatcher_EventDisposed);
					fileWatcher.EventError                += new WatcherEventHandler(fileWatcher_EventError);
					fileWatcher.EventRenamed              += new WatcherEventHandler(fileWatcher_EventRenamed);
				}
				else
				{
					fileWatcher.EventChangedAttribute     -= new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedCreationTime  -= new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedDirectoryName -= new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedFileName      -= new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedLastAccess    -= new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedLastWrite     -= new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedSecurity      -= new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedSize          -= new WatcherEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventCreated              -= new WatcherEventHandler(fileWatcher_EventCreated);
					fileWatcher.EventDeleted              -= new WatcherEventHandler(fileWatcher_EventDeleted);
					fileWatcher.EventDisposed             -= new WatcherEventHandler(fileWatcher_EventDisposed);
					fileWatcher.EventError                -= new WatcherEventHandler(fileWatcher_EventError);
					fileWatcher.EventRenamed              -= new WatcherEventHandler(fileWatcher_EventRenamed);
				}
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the Watcher object. In the interest of providing a complete 
		/// demonstration, all change events are monitored. This will unlikely be a 
		/// real-world requirement in most cases.
		/// </summary>
		private bool InitWatcher()
		{
			bool result = false;
			if (Directory.Exists(this.textBoxFolderToWatch.Text) || File.Exists(this.textBoxFolderToWatch.Text))
			{
				WatcherInfo info = new WatcherInfo();
				info.ChangesFilters = NotifyFilters.Attributes    | 
				                      NotifyFilters.CreationTime  | 
				                      NotifyFilters.DirectoryName | 
				                      NotifyFilters.FileName      | 
				                      NotifyFilters.LastAccess    | 
				                      NotifyFilters.LastWrite     | 
				                      NotifyFilters.Security      | 
				                      NotifyFilters.Size;

				info.IncludeSubFolders = this.checkBoxIncludeSubfolders.Checked;
				info.WatchesFilters    = WatcherChangeTypes.All;
				info.WatchForDisposed  = true;
				info.WatchForError     = false;
				info.WatchPath         = this.textBoxFolderToWatch.Text;
				info.BufferKBytes      = 8;
				fileWatcher            = new Watcher(info);
				ManageEventHandlers(true);
				result = true;
			}
			else
			{
				MessageBox.Show("The folder (or file) specified does not exist.\nPlease specify a valid folder or filename.");
				this.buttonStart.Enabled = false;
			}
			return result;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Common ListViewItem creation for all events.
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="filterName"></param>
		/// <param name="fileName"></param>
		void CreateListViewItem(string eventName, string filterName, string fileName)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text = DateTime.Now.ToString("HH:mm");
			lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, eventName)); 
			lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, filterName)); 
			lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, fileName));
			this.listView1.Items.Add(lvi);
		}
		#endregion Helper Methods

		#region Watcher Events
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Watcher object detects a file rename event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileWatcher_EventRenamed(object sender, WatcherEventArgs e)
		{
			CreateListViewItem("Renamed", "N/A", (e.Arguments == null) ? "Null argument object": ((RenamedEventArgs)(e.Arguments)).FullPath);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Watcher object detects an error event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileWatcher_EventError(object sender, WatcherEventArgs e)
		{
			CreateListViewItem("Error", "N/A", (e.Arguments == null) ? "Null argument object": ((EventArgs)(e.Arguments)).ToString());
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Watcher object disposes one of the internal FileSystemWatcher 
		/// objects.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileWatcher_EventDisposed(object sender, WatcherEventArgs e)
		{
			CreateListViewItem("Disposed", "N/A", (e.Arguments == null) ? "Null argument object": ((EventArgs)(e.Arguments)).ToString());
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Watcher object detects a file deleted event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileWatcher_EventDeleted(object sender, WatcherEventArgs e)
		{
			CreateListViewItem("Deleted", "N/A", (e.Arguments == null) ? "Null argument object": ((FileSystemEventArgs)(e.Arguments)).FullPath);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Watcher object detects a file created event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileWatcher_EventCreated(object sender, WatcherEventArgs e)
		{
			CreateListViewItem("Created", "N/A", (e.Arguments == null) ? "Null argument object": ((FileSystemEventArgs)(e.Arguments)).FullPath);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Watcher object detects a folder/file change event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileWatcher_EventChanged(object sender, WatcherEventArgs e)
		{
			CreateListViewItem("Change", e.Filter.ToString(), (e.Arguments == null) ? "Null argument object": ((FileSystemEventArgs)(e.Arguments)).FullPath);
		}
		#endregion Watcher Events

		#region Form Events
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the form is closing. Disposes the Watcher object
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (fileWatcher != null)
			{
				ManageEventHandlers(false);
				fileWatcher.Dispose();
				fileWatcher = null;
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the user clicks the Start button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonStart_Click(object sender, EventArgs e)
		{
			if (fileWatcher != null)
			{
				fileWatcher.Dispose();
				fileWatcher = null;
			}
			if (InitWatcher())
			{
				this.fileWatcher.Start();
				this.buttonStart.Enabled = false;
				this.buttonStop.Enabled = true;
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the user clicks the Stop button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonStop_Click(object sender, EventArgs e)
		{
			if (fileWatcher != null)
			{
				ManageEventHandlers(false);
				this.fileWatcher.Stop();
				this.buttonStart.Enabled = true;
				this.buttonStop.Enabled = false;
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the user clicks the Done button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonDone_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the user clicks the Browse button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonBrowse_Click(object sender, EventArgs e)
		{
			string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 
			if (string.IsNullOrEmpty(appDataFolder))
			{
				MessageBox.Show("Could not determine the name of the ApplicationData folder\non this machine. Terminating browse attempt.");
				return;
			}
			if (string.IsNullOrEmpty(this.textBoxFolderToWatch.Text))
			{
				this.textBoxFolderToWatch.Text = appDataFolder; 
			}
			// determine the foler from which we start browsing
			string startingDir = this.textBoxFolderToWatch.Text;
			// if the specified folder doesn't exist
			if (!Directory.Exists(startingDir))
			{
				MessageBox.Show("Specified folder does not exist. Starting with\nthe ApplicationData folder instead.");
				// set our starting folder to the user's application data folder
				startingDir = appDataFolder;
			}

			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.SelectedPath = startingDir;
			DialogResult result = dlg.ShowDialog();
			if (result == DialogResult.OK)
			{
				if (!string.IsNullOrEmpty(dlg.SelectedPath))
				{
					this.textBoxFolderToWatch.Text = dlg.SelectedPath;
				}
			}
		}
		#endregion Form Events

		//--------------------------------------------------------------------------------
		private void textBoxFolderToWatch_TextChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.textBoxFolderToWatch.Text))
			{
				this.buttonStart.Enabled = true;
			}
		}


	}
}
