using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace UpLoadFile_WF
{
    public partial class Form1 : Form
    {
        PictureBox pb;
        int myIndex;
         public static string item;
        public static List<string> listadoimg = new List<string>();

        public Form1()
        {
            InitializeComponent();

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // Set the file dialog to filter for graphics files.
            this.openFileDialog1.Filter =
                "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|" +
                "All files (*.*)|*.*";

            // Allow the user to select multiple images.
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "My Image Browser";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {

                    
                    // Create a PictureBox.
                    try
                    {
                        pb = new PictureBox();
                        Image loadedImage = Image.FromFile(file);
                        //pb.Height = loadedImage.Height;
                        //pb.Width = loadedImage.Width;

                       ///////////////////////////////////mini image///////////////////////////////////////
                       //Image.GetThumbnailImageAbort myCallback =
                       //new Image.GetThumbnailImageAbort(ThumbnailCallback);
                       //Bitmap myBitmap = new Bitmap(loadedImage);
                       //Image myThumbnail = myBitmap.GetThumbnailImage(100, 100, myCallback, IntPtr.Zero);
                       ///////////////////////////////////Fin mini image///////////////////////////////////////

                        pb.Height = 100;
                        pb.Width = 100;
                        pb.Image = loadedImage;
                        pb.Tag = file;
                        pb.Name = "cajaimg";

                        pb.MouseDown += new MouseEventHandler(pbox_MouseDown);
                        pb.DragOver += new DragEventHandler(pbox_DragOver);
                        pb.AllowDrop = true;
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;

                        System.Windows.Forms.Button borrar = new System.Windows.Forms.Button();
                        borrar.Parent = pb;
                        borrar.Click += this.Borrar_Click;
                        borrar.Location = new Point(1, 2);      // or whatever you want!!
                        pb.Location = new Point(3, 4);

                        listadoimg.Add(file);
                        flowLayoutPanel.Controls.Add(pb);

                    }
                    catch (SecurityException ex)
                    {
                        // The user lacks appropriate permissions to read files, discover paths, etc.
                        MessageBox.Show("Security error. Please contact your administrator for details.\n\n" +
                            "Error message: " + ex.Message + "\n\n" +
                            "Details (send to Support):\n\n" + ex.StackTrace
                        );
                    }
                    catch (Exception ex)
                    {
                        // Could not load the image - probably related to Windows file system permissions.
                        MessageBox.Show("Cannot display the image: " + file.Substring(file.LastIndexOf('\\'))
                            + ". You may not have permission to read the file, or " +
                            "it may be corrupt.\n\nReported error: " + ex.Message);
                    }
                }
            }

            //Random R = new Random();
            //
            //    panel1.AutoScroll = true;
            //    pictureBox1.Parent = panel1;
            //    pictureBox1.Location = Point.Empty;
            //    pictureBox1.Image = new Bitmap(3000, 500);
            //    pictureBox1.ClientSize = pictureBox1.Image.Size;
            //
            //    var imgFiles = openFileDialog1.FileNames;
            //    foreach (string file in imgFiles)
            //    {
            //        using (Graphics G = Graphics.FromImage(pictureBox1.Image))
            //        using (Bitmap bmp = new Bitmap(file))
            //        {
            //            if (bmp.Size.Width < 4000)
            //            {
            //                for (int i = 0; i < 10; i++)
            //                    G.DrawImage(bmp, R.Next(100), R.Next(100));
            //            }
            //        }
            //    }
            

        }

        void Borrar_Click(object sender, EventArgs e)
        {

            if (flowLayoutPanel.Controls.Contains(pb))
            {
                this.pb.Click -= new System.EventHandler(this.Borrar_Click);
                flowLayoutPanel.Controls.Remove(pb);
                pb.Dispose();
            }
            //throw new NotImplementedException();
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// ////////////////drag and drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Pb_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Hola"+ e.GetType());
            //throw new NotImplementedException();
        }

        public void pbox_DragOver(object sender, DragEventArgs e)
        {
            base.OnDragOver(e);
            // is another dragable
            if (e.Data.GetData(typeof(PictureBox)) != null)
            {
                FlowLayoutPanel p = (FlowLayoutPanel)(sender as PictureBox).Parent;
                //Current Position             
                myIndex = p.Controls.GetChildIndex((sender as PictureBox));


                //Dragged to control to location of next picturebox
                PictureBox q = (PictureBox)e.Data.GetData(typeof(PictureBox));
                p.Controls.SetChildIndex(q, myIndex);

                //item = listadoimg[myIndex];
                //myIndex = p.Controls.GetChildIndex((sender as PictureBox));
                //                listadoimg.RemoveAt(myIndex);
                //
                //                listadoimg.Insert(myIndex, item);

                //listadoimg.Insert(myIndex, q.Image.ToString());
                //listadoimg.RemoveAt(myIndex);
            }
        }
        void pbox_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            DoDragDrop(sender, DragDropEffects.All);


        }

        public void button2_Click(object sender, EventArgs e)
        {

            //  var idx = listadoimg.FindIndex(x => x == Form1.item);
            //  var item = listadoimg[idx];
            //  listadoimg.Remove(item);
            //  listadoimg.Insert(myIndex, item);

            listadoimg.Clear();
            if (pb != null) { 
                Control[] ctrls = flowLayoutPanel.Controls.Find(pb.Name, true);
                foreach (PictureBox c in ctrls)
                {
                    string imagePath = (string)c.Tag;
                    listadoimg.Add(imagePath);

                }
                //MessageBox.Show("listado: "+ listadoimg);
            }
        }


}
}
