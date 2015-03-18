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

using MEFdemo1.HAL.DeviceControlContracts;
using MEFdemo1.AppContracts;

namespace AppPlugin2
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IAppPlugin))]
    public partial class BarcodeForm : Form, IAppPlugin
    {
        string _sReturn = "";
        public string sReturnData
        {
            get { return _sReturn; }
        }
        public string sAppText
        {
            get { return "Barcode Form"; }
        }
        public Bitmap appBitmap
        {
            get
            {
                System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream("AppPlugin2.Bulbgraph.jpg");
                Bitmap bmp = new Bitmap(s);

                return bmp;
            }
        }

        //[Import(typeof(IBarcodeScanControl))] //if Import is used here, the catalog is unable to find the control!
        private IBarcodeScanControl conScan;

        DirectoryCatalog catalog2;
        CompositionContainer container2;

        public BarcodeForm()
        {
            InitializeComponent();
            try
            {
                string sPath="";
                if (isIntermec)
                    sPath = "MEFdemo1.HAL.Intermec.*Control*.dll";
                else
                    sPath = "MEFdemo1.HAL.ACME.*Control*.dll";

                //I was unable to use the different catalog and let it look in a subfolder
                //so the plugin names are used as a filter
                catalog2 = new DirectoryCatalog(".", sPath);

                foreach (string s in catalog2.LoadedFiles)
                    System.Diagnostics.Debug.WriteLine(s);

                container2 = new CompositionContainer(catalog2);

#if DEBUG
                //some diagnostics...
                //see http://mef.codeplex.com/wikipage?title=Debugging%20and%20Diagnostics&referringTitle=Guide
                // using Samples\.... as in MEF preview 7 and 8
                CompositionInfo ci = new CompositionInfo(catalog2, container2);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.IO.TextWriter tw = new System.IO.StreamWriter(ms);
                CompositionInfoTextFormatter.Write(ci, tw);
                System.Diagnostics.Debug.WriteLine(Encoding.UTF8.GetString(ms.GetBuffer(), 0, ms.GetBuffer().Length));
                foreach (PartDefinitionInfo pi in ci.PartDefinitions)
                {
                    System.Diagnostics.Debug.WriteLine("isRejected: " + pi.IsRejected.ToString());
                    System.Diagnostics.Debug.WriteLine("partInfo: " + pi.PartDefinition.ToString());
                }
                tw.Close();
#endif
                container2.ComposeParts(this);
                initBarcode();
            }
            catch (Exception ex)
            {
                if (ex is ChangeRejectedException)
                    MessageBox.Show("HW-Components not found!");
                else
                    MessageBox.Show("Exception in ComposeParts: " + ex.Message + "\n" + ex.StackTrace);
                this.Close();
            }
        }
        Control ctrScan;
        private void initBarcode()
        {
            ctrScan = conScan as Control;
            if (ctrScan != null)
            {
                this.Controls.Add(ctrScan);
                ctrScan.Visible = false;
                conScan.ScanReady += new BarcodeEventHandler(conScan_ScanReady);    //using new eventargs driven interface
                System.Diagnostics.Debug.WriteLine("initBarcode(): added scanner control");
            }
            
        }
        private void deInitBarcode()
        {
            if (conScan != null)
            {
                conScan.Dispose();
                conScan = null;
                System.Diagnostics.Debug.WriteLine("deInitBarcode(): removed scanner control");
            }
        }
        void conScan_ScanReady(object sender, BarcodeEventArgs e)
        {
            textBox1.Text = e.Text;
            if (e._bSuccess)
            {
                textBox1.BackColor = Color.Green;
            }
            else
            {
                textBox1.BackColor = Color.Pink;
            }
        }

        private void ScanControl_Ready(object sender, EventArgs e)
        {
            textBox1.Text = conScan.BarcodeText;
            if (conScan.IsSuccess)
            {
                textBox1.BackColor = Color.Green;
            }
            else
            {
                textBox1.BackColor = Color.Pink;
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            deInitBarcode();
            this.Close();
        }
        bool isIntermec
        {
            get
            {
                return System.IO.File.Exists(@"\Windows\itc50.dll");
            }
        }

        private void BarcodeForm_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("BarcodeForm_Load!");
        }

        private void BarcodeForm_Closed(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("BarcodeForm Closed!");
        }
           
    }
}