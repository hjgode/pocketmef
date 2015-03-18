using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Diagnostics;

using MEFdemo1.AppContracts;

namespace MEFdemo1
{
    public partial class MainForm : Form
    {
        //we will import all available plugins
        [ImportMany(typeof(IAppPlugin))]
        private IEnumerable<IAppPlugin> plugins;
        private int iPluginCount = 0;
        
        DirectoryCatalog catalog;
        CompositionContainer container;

        public MainForm()
        {
            InitializeComponent();
            try
            {
                catalog = new DirectoryCatalog(".", "MEFdemo1.Plugins.*.dll");
                container = new CompositionContainer(catalog);
                container.ComposeParts(this);
#if DEBUG
                //some diagnostics...
                //see http://mef.codeplex.com/wikipage?title=Debugging%20and%20Diagnostics&referringTitle=Guide
                // using Samples\.... as in MEF preview 7 and 8
                CompositionInfo ci = new CompositionInfo(catalog, container);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.IO.TextWriter tw = new System.IO.StreamWriter(ms);
                CompositionInfoTextFormatter.Write(ci, tw);
                System.Diagnostics.Debug.WriteLine(Encoding.UTF8.GetString(ms.GetBuffer(), 0, ms.GetBuffer().Length));
                foreach (PartDefinitionInfo pi in ci.PartDefinitions)
                {
                    iPluginCount++;
                    System.Diagnostics.Debug.WriteLine("isRejected: " + pi.IsRejected.ToString());
                    System.Diagnostics.Debug.WriteLine("partInfo: " + pi.PartDefinition.ToString());
                }

                tw.Close();
#else
                foreach (PartDefinitionInfo pi in ci.PartDefinitions)
                {
                    iPluginCount++;
                }
#endif
                drawPlugins();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Plugins loaded: " + ex.Message);
            }
        }

        int iOffsetX = 10; int iOffsetY = 10;
        int iSizeX = 80; int iSizeY = 80;
        PictureBox[] pbList;
        private void drawPlugins()
        {
            int iRow = 1, iCol = 1;

            if (iPluginCount > 0)
            {
                pbList = new PictureBox[iPluginCount];
                int iIdx=0;
                this.SuspendLayout();
                foreach (IAppPlugin iApp in plugins)
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine(iApp.sAppText);
                        
                        pbList[iIdx] = new PictureBox();
                        pbList[iIdx].Name = iApp.sAppText;
                        pbList[iIdx].Location = new Point(iOffsetX * iCol + iSizeX*(iCol-1), iOffsetY * iRow + iSizeY*(iRow-1));
                        pbList[iIdx].Size = new Size(iSizeX, iSizeY);

                        pbList[iIdx].Click += new EventHandler(MainForm_Click);
                        
                        Bitmap bmp = iApp.appBitmap;
                        pbList[iIdx].Image = bmp;
                        pbList[iIdx].SizeMode = PictureBoxSizeMode.StretchImage;

                        pbList[iIdx].Tag = iApp.sAppText;
                        pbList[iIdx].Visible=true;

                        this.Controls.Add(pbList[iIdx]);
                        iIdx++;
                        iCol++;
                        if (iCol == 4)
                        {
                            iRow++;
                            iCol = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Exception: " + ex.Message);
                    }
                }
                this.Refresh();
                this.ResumeLayout();
            }

        }

        void MainForm_Click(object sender, EventArgs e)
        {
            string sApp = ((PictureBox)sender).Tag.ToString();
            System.Diagnostics.Debug.WriteLine(sApp);
            foreach (IAppPlugin iApp in plugins)
            {
                if (iApp.sAppText.Equals(sApp))
                {
                    ((System.Windows.Forms.Form)iApp).ShowDialog();
                    continue;
                }
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}