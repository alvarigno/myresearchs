using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsBrowserApp
{
    public partial class Form1 : Form
    {

        public static string data ="";
        public static object doc1;
        public Form1()
        {
            InitializeComponent();
            this.webBrowser1.ObjectForScripting = new MyScript();
            this.webBrowser1.DocumentCompleted+= new WebBrowserDocumentCompletedEventHandler(browser_DocumentCompleted);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.chileautos.cl/publicar/paso1");
            //textBox2.Text = webBrowser1.Document.GetElementsByTagName("label")[0].OuterHtml;
        }

        [ComVisible(true)]
        public class MyScript
        {
            public void CallServerSideCode()
            {
                var doc1 = ((Form1)Application.OpenForms[0]).webBrowser1.Document;
                //data = doc1.GetElementsByTagName("HTML")[0].OuterHtml;
                

            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            textBox1.Text = webBrowser1.Url.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            if (webBrowser1.CanGoBack)
            {
                webBrowser1.GoBack();
            }
            else
            {
                MessageBox.Show("You cannot go back");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            webBrowser1.Navigate(textBox1.Text);

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            TabPage tabpage = new TabPage();
            tabpage.Text = "New File";
            tabControl1.Controls.Add(tabpage);
            WebBrowser webbrowser = new WebBrowser();
            webbrowser.Parent = tabpage;
            webbrowser.Dock = DockStyle.Fill;
            webbrowser.Navigate("www.google.com");

        }

        private void button4_Click(object sender, EventArgs e)
        {

            HtmlElement elem;

            if (webBrowser1.Document != null)
            {
                CodeForm cf = new CodeForm();
                HtmlElementCollection elems = webBrowser1.Document.GetElementsByTagName("HTML");
                if (elems.Count == 1)
                {
                    elem = elems[0];
                    cf.Code = elem.OuterHtml;
                    cf.Show();
                }
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }



        private void browser_DocumentCompleted(Object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.webBrowser1.Document.Body.MouseDown += new HtmlElementEventHandler(Body_MouseDown);
        }

        private void Body_MouseDown(Object sender, HtmlElementEventArgs e)
        {
            switch (e.MouseButtonsPressed)
            {
                case MouseButtons.Left:
                    //HtmlElement elem = webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);
                    //
                    //String nameStr = elem.GetAttribute("value");
                    //if (nameStr != null && nameStr.Length != 0)
                    //{
                    //    String contentStr = elem.GetAttribute("value");
                    //    MessageBox.Show("Document: " + webBrowser1.Url.ToString() + "\nDescription: " + contentStr);
                    //    elem = null;
                    //}

                    //switch (e.MouseButtonsPressed)
                    //{
                    //    case MouseButtons.Left:
                    //        HtmlElement element = this._browser.Document.GetElementFromPoint(e.ClientMousePosition);
                    //        if (element != null && "submit".Equals(element.GetAttribute("type"), StringComparison.OrdinalIgnoreCase)
                    //        {
                    //        }
                    //        break;
                    //}

                    HtmlElement element = this.webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);
                    // if (element != null && "input".Equals(element.GetAttribute("value"), StringComparison.OrdinalIgnoreCase))
                    if (element != null)
                    {
                        textBox2.Text = element.GetAttribute("value");;
                    }
                    break;
            }

           // if (webBrowser1.Document != null)
           // {
           //     HtmlElementCollection elems = webBrowser1.Document.GetElementsByTagName("input");
           //     foreach (HtmlElement elem in elems)
           //     {
           //         String nameStr = elem.GetAttribute("value");
           //         if (nameStr != null && nameStr.Length != 0)
           //         {
           //             String contentStr = elem.GetAttribute("value");
           //             MessageBox.Show("Document: " + webBrowser1.Url.ToString() + "\nDescription: " + contentStr);
           //         }
           //     }
           // }

        }

    }
}
