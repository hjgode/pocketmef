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

namespace MEFdemo1
{
    public partial class BarcodeForm : Form
    {
        [Import(typeof(IBarcodeScanControl))]
        private IBarcodeScanControl conScan;

        public BarcodeForm()
        {
            InitializeComponent();
            try
            {
                string sPath="";
                if (isIntermec)
                    sPath = "./intermec";
                else
                    sPath = "./ACME";

                //MEFdemo1.HAL.BarcodeControl1.dll
                DirectoryCatalog catalog = new DirectoryCatalog(sPath, "MEFdemo1.HAL.*Control*.dll");

                foreach (string s in catalog.LoadedFiles)
                    System.Diagnostics.Debug.WriteLine(s);

                CompositionContainer container = new CompositionContainer(catalog);

                //some diagnostics...
                //see http://mef.codeplex.com/wikipage?title=Debugging%20and%20Diagnostics&referringTitle=Guide
                // using Samples\.... as in MEF preview 7 and 8
                //var ci = new CompositionInfo(catalog, container);
                //System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //System.IO.TextWriter tw = new System.IO.StreamWriter(ms);
                //CompositionInfoTextFormatter.Write(ci, tw);
                //System.Diagnostics.Debug.WriteLine(Encoding.UTF8.GetString(ms.GetBuffer(), 0, ms.GetBuffer().Length));
                //foreach(PartDefinitionInfo pi in ci.PartDefinitions){
                //    System.Diagnostics.Debug.WriteLine("isRejected: " + pi.IsRejected.ToString());
                //    System.Diagnostics.Debug.WriteLine("partInfo: " + pi.PartDefinition.ToString());
                //}
                //tw.Close();

                container.ComposeParts(this);
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
        private void initBarcode()
        {
            Control ctrScan = conScan as Control;
            if (ctrScan != null)
            {
                this.Controls.Add(ctrScan);
                ctrScan.Visible = false;
            }
            //conScan.ScanReady += ScanControl_Ready;
            conScan.ScanReady += new BarcodeEventHandler(conScan_ScanReady);    //using new eventargs driven interface
            
        }
        private void deInitBarcode()
        {
            if (conScan != null)
            {
                conScan.Dispose();
                conScan = null;
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
            Application.Exit();
        }
        bool isIntermec
        {
            get
            {
                return System.IO.File.Exists(@"\Windows\itc50.dll");
            }
        }
           
    }
}