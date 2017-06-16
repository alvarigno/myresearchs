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
using MaterialSkin;
using MaterialSkin.Controls;

namespace UpLoadFile_WF
{
    public partial class Form1 : MaterialForm
    {
        private readonly MaterialSkinManager materialSkinManager;
        PictureBox pb;
        int countbutton = 0;
        public static string item;
        public static List<string> listadoimg = new List<string>();

        public Form1()
        {
            InitializeComponent();

            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // Set the file dialog to filter for graphics files.
            this.openFileDialog1.Filter ="Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF| All files (*.*)|*.*";

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

                        pb.Height = 100;
                        pb.Width = 100;
                        pb.Image = loadedImage;
                        pb.Tag = file;
                        pb.Name = "cajaimg";
                        pb.Visible = true;

                        pb.MouseDown += new MouseEventHandler(pbox_MouseDown);
                        pb.DragOver += new DragEventHandler(pbox_DragOver);
                        pb.AllowDrop = true;
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;                        

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
                int myIndex = p.Controls.GetChildIndex((sender as PictureBox));


                //Dragged to control to location of next picturebox
                PictureBox q = (PictureBox)e.Data.GetData(typeof(PictureBox));
                p.Controls.SetChildIndex(q, myIndex);

            }
        }
        void pbox_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            DoDragDrop(sender, DragDropEffects.All);

            FlowLayoutPanel p = (FlowLayoutPanel)(sender as PictureBox).Parent;
            int myIndex = p.Controls.GetChildIndex((sender as PictureBox));

            if (e.Button == System.Windows.Forms.MouseButtons.Right) {

                List<Control> listControls = new List<Control>();

                // ContextMenu mnuContextMenu = new ContextMenu();
                // MenuItem mnuItemNew = new MenuItem();
                // MenuItem mnuItemOpen = new MenuItem();
                // mnuItemNew.Text = "&New";
                // mnuItemOpen.Text = "&Open";
                // mnuContextMenu.MenuItems.Add(mnuItemNew);
                // mnuContextMenu.MenuItems.Add(mnuItemOpen);
                // MenuItem mnuItemOpenWith = new MenuItem();
                // mnuItemOpenWith.Text = "Open &With...";
                // mnuItemOpen.MenuItems.Add(mnuItemOpenWith);
                // mnuContextMenu.MenuItems.Add("&Close");
                // pb.ContextMenu = mnuContextMenu;

               // MessageBox.Show("Botón derecho elimina la selección");

                 foreach (Control control in p.Controls)
                 {
                     listControls.Add(control);
                     
                 }
                
                 foreach (Control control in listControls)
                 {
                     flowLayoutPanel.Controls.Remove(control);
                 }
                 
                 listControls.RemoveAt(myIndex);
                
                 foreach (Control control in listControls)
                 {
                    flowLayoutPanel.Controls.Add(control);
                
                 }
                 listControls.Clear();

            }


        }

        public void button2_Click(object sender, EventArgs e)
        {
            string listado = "";
            countbutton = 0;
            
            if (pb != null) { 

                Control[] ctrls = flowLayoutPanel.Controls.Find(pb.Name, true);

                foreach (Control c in ChildControls(flowLayoutPanel))
                {
                    string imagePath = (string)c.Tag;
                    listadoimg.Add(imagePath + countbutton);
                    countbutton = countbutton + 1;
                }

                foreach (string Txt in listadoimg)
                {

                    listado = listado + Txt+",";
                    
                }
                if (listado != "") { listado = listado.Remove(listado.Length - 1); }

                MessageBox.Show("listado: "+ listadoimg+" - "+listado);
                listadoimg.Clear();

            }
        }

        private IEnumerable<Control> ChildControls(Control parent)
        {
            List<Control> controls = new List<Control>();
            controls.Add(parent);
            foreach (Control ctrl in parent.Controls)
            {
                controls.AddRange(ChildControls(ctrl));
            }
            return controls;
        }


        private void flowLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
