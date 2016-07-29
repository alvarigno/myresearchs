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
	public partial class Form1Ex : Form
	{
		WatcherEx fileWatcher = null;

		public Form1Ex()
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
					fileWatcher.EventChangedAttribute     += new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedCreationTime  += new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedDirectoryName += new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedFileName      += new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedLastAccess    += new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedLastWrite     += new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedSecurity      += new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedSize          += new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventCreated              += new WatcherExEventHandler(fileWatcher_EventCreated);
					fileWatcher.EventDeleted              += new WatcherExEventHandler(fileWatcher_EventDeleted);
					fileWatcher.EventDisposed             += new WatcherExEventHandler(fileWatcher_EventDisposed);
					fileWatcher.EventError                += new WatcherExEventHandler(fileWatcher_EventError);
					fileWatcher.EventRenamed              += new WatcherExEventHandler(fileWatcher_EventRenamed);
					fileWatcher.EventPathAvailability     += new WatcherExEventHandler(fileWatcher_EventPathAvailability);
				}
				else
				{
					fileWatcher.EventChangedAttribute     -= new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedCreationTime  -= new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedDirectoryName -= new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedFileName      -= new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedLastAccess    -= new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedLastWrite     -= new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedSecurity      -= new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventChangedSize          -= new WatcherExEventHandler(fileWatcher_EventChanged);
					fileWatcher.EventCreated              -= new WatcherExEventHandler(fileWatcher_EventCreated);
					fileWatcher.EventDeleted              -= new WatcherExEventHandler(fileWatcher_EventDeleted);
					fileWatcher.EventDisposed             -= new WatcherExEventHandler(fileWatcher_EventDisposed);
					fileWatcher.EventError                -= new WatcherExEventHandler(fileWatcher_EventError);
					fileWatcher.EventRenamed              -= new WatcherExEventHandler(fileWatcher_EventRenamed);
					fileWatcher.EventPathAvailability     += new WatcherExEventHandler(fileWatcher_EventPathAvailability);
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
				info.MonitorPathInterval = 250;
				fileWatcher            = new WatcherEx(info);
				ManageEventHandlers(true);
				result = true;
			}
			else
			{
				MessageBox.Show("The folder (or file) specified does not exist.\nPlease specify a valid folder or filename.");
			}
			return result;
		}
		#endregion Helper Methods

		#region ListView Update Delegate Methods and Definitions 
		private delegate void DelegateCreateListViewItem(string eventName, string filterName, string fileName);

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Common ListViewItem creation for all events.  This method checks to see if 
		/// invoke is required, and makes the appropriate call to the 
		/// InvokedCreateListViewItem() method.
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="filterName"></param>
		/// <param name="fileName"></param>
		void CreateListViewItem(string eventName, string filterName, string fileName)
		{
			if (this.listView1.InvokeRequired)
			{
		        DelegateCreateListViewItem method = new DelegateCreateListViewItem(InvokedCreateListViewItem);
				Invoke(method, new object[3]{eventName, filterName, fileName} );
			}
			else
			{
				InvokedCreateListViewItem(eventName, filterName, fileName);
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// The method actually used to update the listview. 
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="filterName"></param>
		/// <param name="fileName"></param>
		private void InvokedCreateListViewItem(string eventName, string filterName, string fileName)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text = DateTime.Now.ToString("HH:mm:ss");
			lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, eventName)); 
			lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, filterName)); 
			lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, fileName));
			//this.listView1.BeginUpdate();
			this.listView1.Items.Add(lvi);
			this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
			//this.listView1.EndUpdate();
		}
		#endregion ListView Update Delegate Methods and Definitions 

		#region Watcher Events
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Watcher object detects a file rename event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileWatcher_EventRenamed(object sender, WatcherExEventArgs e)
		{
			CreateListViewItem("Renamed", "N/A", (e.Arguments == null) ? "Null argument object": ((RenamedEventArgs)(e.Arguments)).FullPath);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Watcher object detects an error event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileWatcher_EventError(object sender, WatcherExEventArgs e)
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
		void fileWatcher_EventDisposed(object sender, WatcherExEventArgs e)
		{
			CreateListViewItem("Disposed", "N/A", (e.Arguments == null) ? "Null argument object": ((EventArgs)(e.Arguments)).ToString());
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Watcher object detects a file deleted event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileWatcher_EventDeleted(object sender, WatcherExEventArgs e)
		{
			CreateListViewItem("Deleted", "N/A", (e.Arguments == null) ? "Null argument object": ((FileSystemEventArgs)(e.Arguments)).FullPath);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Watcher object detects a file created event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileWatcher_EventCreated(object sender, WatcherExEventArgs e)
		{
			CreateListViewItem("Created", "N/A", (e.Arguments == null) ? "Null argument object": ((FileSystemEventArgs)(e.Arguments)).FullPath);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the Watcher object detects a folder/file change event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileWatcher_EventChanged(object sender, WatcherExEventArgs e)
		{
			CreateListViewItem("Change", e.Filter.ToString(), (e.Arguments == null) ? "Null argument object": ((FileSystemEventArgs)(e.Arguments)).FullPath);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the availability of the folder has changed.  If it becomes 
		/// "disconnected", the internal watchers are disabled. When it becomes connected 
		/// again, the internal watchers are restored.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileWatcher_EventPathAvailability(object sender, WatcherExEventArgs e)
		{
			string eventName  = "Availability";
			string filterName = "N/A";
			string status   = "";
			if (e.Arguments == null) 
			{
				status = "Null argument object";
			}
			else
			{
				status = (((PathAvailablitiyEventArgs)(e.Arguments)).PathIsAvailable) ? "Connected" 
				                                                                      : "Disconnected";
			}
		    CreateListViewItem(eventName, filterName, status);
		}

		#endregion Watcher Events

		#region Form Events
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the form is closing. Disposes the Watcher object
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1Ex_FormClosing(object sender, FormClosingEventArgs e)
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

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the user clicks the Clear List button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonClearList_Click(object sender, EventArgs e)
		{
			this.listView1.Items.Clear();
		}
		#endregion Form Events

	}
}
