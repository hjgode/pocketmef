using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.ComponentModel;
using System.ComponentModel.Composition;

using MEFdemo1.AppContracts;

namespace AppPlugin1
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IAppPlugin))]
    public partial class UserForm1 : Form, IAppPlugin
    {
        string _sReturn = "";
        public string sReturnData
        {
            get { return _sReturn; }
        }
        public string sAppText
        {
            get { return "AppPlugin1"; }
        }
        public Bitmap appBitmap
        {
            get
            {
                //string AppPath;
                //AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                //if (!AppPath.EndsWith(@"\"))
                //    AppPath += @"\";
                //Uri uri = new Uri(AppPath);
                //AppPath = uri.AbsolutePath;
                //AppPath = AppPath.Replace("/", "\\");
                //Bitmap bmp = new Bitmap(AppPath + "bernd_klein.jpg");

                System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream("AppPlugin1.bernd_klein.jpg");
                Bitmap bmp = new Bitmap(s);
                
                return bmp;
            }
        }
        public UserForm1()
        {
            InitializeComponent();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void UserForm1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // Up
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                // Down
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Left
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Right
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
            }

        }

    }
}