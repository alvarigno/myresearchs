//------------------------------------------------------------------------------
// File    : MailPop3.cs 
// Version : 1.10
// Date    : 17. November 2002
// Author  : Bruno Podetti
// Email   : Podetti@gmx.net
//
// Sample how to use the MailPop3 class
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Mail;

namespace Sample
{
  /// <summary>
  /// Summary description for eMailForm.
  /// </summary>
  public class eMailForm : System.Windows.Forms.Form
  {
    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.TextBox textBoxServer;
    private System.Windows.Forms.TextBox textBoxPort;
    private System.Windows.Forms.TextBox textBoxUsername;
    private System.Windows.Forms.TextBox textBoxPassword;
    private System.Windows.Forms.ColumnHeader columnHeaderFrom;
    private System.Windows.Forms.ColumnHeader columnHeaderTo;
    private System.Windows.Forms.ColumnHeader columnHeaderDate;
    private System.Windows.Forms.ColumnHeader columnHeaderSubject;
    private System.Windows.Forms.Label labelUsername;
    private System.Windows.Forms.Label labelServer;
    private System.Windows.Forms.Label labelPassword;
    private System.Windows.Forms.Label labelPort;
    private System.Windows.Forms.Button buttonRead;
    private System.Windows.Forms.TextBox textBoxError;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public eMailForm()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      //
      // TODO: Add any constructor code after InitializeComponent call
      //
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() 
    {
      Application.Run(new eMailForm());
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if(components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.listView1 = new System.Windows.Forms.ListView();
      this.columnHeaderFrom = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderTo = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderDate = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderSubject = new System.Windows.Forms.ColumnHeader();
      this.labelUsername = new System.Windows.Forms.Label();
      this.labelServer = new System.Windows.Forms.Label();
      this.labelPassword = new System.Windows.Forms.Label();
      this.labelPort = new System.Windows.Forms.Label();
      this.textBoxServer = new System.Windows.Forms.TextBox();
      this.textBoxPort = new System.Windows.Forms.TextBox();
      this.textBoxUsername = new System.Windows.Forms.TextBox();
      this.textBoxPassword = new System.Windows.Forms.TextBox();
      this.buttonRead = new System.Windows.Forms.Button();
      this.textBoxError = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // listView1
      // 
      this.listView1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right);
      this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                this.columnHeaderFrom,
                                                                                this.columnHeaderTo,
                                                                                this.columnHeaderDate,
                                                                                this.columnHeaderSubject});
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.HideSelection = false;
      this.listView1.Location = new System.Drawing.Point(8, 64);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(608, 200);
      this.listView1.TabIndex = 9;
      this.listView1.View = System.Windows.Forms.View.Details;
      // 
      // columnHeaderFrom
      // 
      this.columnHeaderFrom.Text = "From";
      this.columnHeaderFrom.Width = 100;
      // 
      // columnHeaderTo
      // 
      this.columnHeaderTo.Text = "To";
      this.columnHeaderTo.Width = 100;
      // 
      // columnHeaderDate
      // 
      this.columnHeaderDate.Text = "Date";
      this.columnHeaderDate.Width = 100;
      // 
      // columnHeaderSubject
      // 
      this.columnHeaderSubject.Text = "Subject";
      this.columnHeaderSubject.Width = 300;
      // 
      // labelUsername
      // 
      this.labelUsername.Location = new System.Drawing.Point(264, 10);
      this.labelUsername.Name = "labelUsername";
      this.labelUsername.Size = new System.Drawing.Size(64, 16);
      this.labelUsername.TabIndex = 5;
      this.labelUsername.Text = "Username:";
      // 
      // labelServer
      // 
      this.labelServer.Location = new System.Drawing.Point(8, 10);
      this.labelServer.Name = "labelServer";
      this.labelServer.Size = new System.Drawing.Size(80, 16);
      this.labelServer.TabIndex = 1;
      this.labelServer.Text = "Pop3 Server:";
      // 
      // labelPassword
      // 
      this.labelPassword.Location = new System.Drawing.Point(264, 32);
      this.labelPassword.Name = "labelPassword";
      this.labelPassword.Size = new System.Drawing.Size(64, 16);
      this.labelPassword.TabIndex = 7;
      this.labelPassword.Text = "Password:";
      // 
      // labelPort
      // 
      this.labelPort.Location = new System.Drawing.Point(8, 32);
      this.labelPort.Name = "labelPort";
      this.labelPort.Size = new System.Drawing.Size(80, 16);
      this.labelPort.TabIndex = 3;
      this.labelPort.Text = "Port:";
      // 
      // textBoxServer
      // 
      this.textBoxServer.Location = new System.Drawing.Point(96, 8);
      this.textBoxServer.Name = "textBoxServer";
      this.textBoxServer.Size = new System.Drawing.Size(152, 20);
      this.textBoxServer.TabIndex = 2;
      this.textBoxServer.Text = "";
      // 
      // textBoxPort
      // 
      this.textBoxPort.Location = new System.Drawing.Point(96, 32);
      this.textBoxPort.Name = "textBoxPort";
      this.textBoxPort.Size = new System.Drawing.Size(152, 20);
      this.textBoxPort.TabIndex = 4;
      this.textBoxPort.Text = "110";
      // 
      // textBoxUsername
      // 
      this.textBoxUsername.Location = new System.Drawing.Point(328, 8);
      this.textBoxUsername.Name = "textBoxUsername";
      this.textBoxUsername.Size = new System.Drawing.Size(152, 20);
      this.textBoxUsername.TabIndex = 6;
      this.textBoxUsername.Text = "";
      // 
      // textBoxPassword
      // 
      this.textBoxPassword.Location = new System.Drawing.Point(328, 32);
      this.textBoxPassword.Name = "textBoxPassword";
      this.textBoxPassword.PasswordChar = '*';
      this.textBoxPassword.Size = new System.Drawing.Size(152, 20);
      this.textBoxPassword.TabIndex = 8;
      this.textBoxPassword.Text = "";
      // 
      // buttonRead
      // 
      this.buttonRead.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
      this.buttonRead.Location = new System.Drawing.Point(8, 288);
      this.buttonRead.Name = "buttonRead";
      this.buttonRead.TabIndex = 10;
      this.buttonRead.Text = "Read";
      this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
      // 
      // textBoxError
      // 
      this.textBoxError.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right);
      this.textBoxError.Location = new System.Drawing.Point(96, 272);
      this.textBoxError.Multiline = true;
      this.textBoxError.Name = "textBoxError";
      this.textBoxError.ReadOnly = true;
      this.textBoxError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxError.Size = new System.Drawing.Size(520, 56);
      this.textBoxError.TabIndex = 12;
      this.textBoxError.Text = "Enter connection settings and press Read";
      // 
      // eMailForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(624, 333);
      this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                  this.textBoxError,
                                                                  this.buttonRead,
                                                                  this.textBoxServer,
                                                                  this.labelUsername,
                                                                  this.listView1,
                                                                  this.labelServer,
                                                                  this.labelPassword,
                                                                  this.labelPort,
                                                                  this.textBoxPort,
                                                                  this.textBoxUsername,
                                                                  this.textBoxPassword});
      this.Name = "eMailForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "eMailForm";
      this.Load += new System.EventHandler(this.eMailForm_Load);
      this.ResumeLayout(false);

    }
    #endregion

    private void eMailForm_Load(object sender, System.EventArgs e)
    {
    }

    private void buttonRead_Click(object sender, System.EventArgs e)
    {
      // Create MailPop3 connection
      MailPOP3 pop3Connection = new MailPOP3();

      string err = pop3Connection.DoConnect(textBoxServer.Text,
                                            int.Parse(textBoxPort.Text),
                                            textBoxUsername.Text,
                                            textBoxPassword.Text);

      if(err==null || err.Length==0)
        return;

      // Show the connection state
      textBoxError.Text = err;

      if(!err.StartsWith("+OK"))
      {
        pop3Connection.Quit();
        return;
      }

      // check if we have mail:
      MailHeader[] myHeader = pop3Connection.GetMails(false); 
      if(myHeader!=null)
      {
        listView1.Items.Clear();

        for(int n=0;n<myHeader.Length;n++)
        {
          if(myHeader[n]!=null)
          {
            ListViewItem lvItem = listView1.Items.Add(myHeader[n].from); 
            lvItem.SubItems.Add(myHeader[n].to);
            lvItem.SubItems.Add(myHeader[n].date);
            lvItem.SubItems.Add(myHeader[n].subject);
          }
        }
      }
      pop3Connection.Quit();
    }
  }
}
