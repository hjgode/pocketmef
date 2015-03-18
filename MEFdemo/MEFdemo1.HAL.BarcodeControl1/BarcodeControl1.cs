using System;
using System.Collections.Generic;

using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.ComponentModel;
using System.ComponentModel.Composition;

using MEFdemo1.HAL.DeviceControlContracts;

using Intermec.DataCollection;

namespace MEFdemo1
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IBarcodeScanControl))]
    public partial class BarcodeControl1 : UserControl, IBarcodeScanControl
    {
        BarcodeReader bcr;
        public string _BarcodeText="";
        bool _bIsSuccess = false;
        public BarcodeControl1()
        {
            InitializeComponent();
            try
            {
                System.Diagnostics.Debug.WriteLine("BarcodeControl1 loading...");
                bcr = new BarcodeReader();
                bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);
                bcr.ThreadedRead(true);
                Intermec.DeviceManagement.SmartSystem.ITCSSApi ssApi = new Intermec.DeviceManagement.SmartSystem.ITCSSApi();
                StringBuilder sb = new StringBuilder(1024);
                uint uErr = 0;
                string sXml = "<Subsystem Name=\"Data Collection\">";
                sXml += "<Group Name=\"Scanners\" Instance=\"0\">";
                sXml += "<Group Name=\"Scanner Settings\">";
                sXml += "<Field Name=\"Hardware trigger\">1</Field>";
                sXml += "</Group>";
                sXml += "</Group>";
                sXml += "</Subsystem>";
                int iSize=sb.Capacity;
                uErr = ssApi.Set(sXml, sb, ref iSize, 3000);
                if (uErr != Intermec.DeviceManagement.SmartSystem.ITCSSErrors.E_SS_SUCCESS)
                {
                    System.Diagnostics.Debug.WriteLine("could not change HardwareTrigger: 0x" + uErr.ToString("X"));
                    MessageBox.Show("could not change HardwareTrigger");
                }
                System.Diagnostics.Debug.WriteLine("...BarcodeControl1 loaded");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }

        void bcr_BarcodeRead(object sender, BarcodeReadEventArgs bre)
        {
            _BarcodeText = bre.strDataBuffer;
            _bIsSuccess = true;
            ScanIsReady(_BarcodeText, true);
        }
        /// <summary>
        /// this text gives the barcode data
        /// </summary>
        public string BarcodeText
        {
            get
            {
                return _BarcodeText;
            }
        }
        /// <summary>
        /// return a bool for a good/bad scan
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return _bIsSuccess;
            }
        }
        //Create an event, do not use directly!
        //public event EventHandler ScanReady;
        public event MEFdemo1.HAL.DeviceControlContracts.BarcodeEventHandler ScanReady;
        delegate void deleScanIsReady(string sData, bool bIsSuccess);
        /// <summary>
        /// this will be called for successful and faulty scans
        /// </summary>
        private void ScanIsReady(string sData, bool bIsSuccess)
        {
            System.Diagnostics.Debug.WriteLine("ScanIsReady started...");
            if (this.InvokeRequired)
            {
                deleScanIsReady d = new deleScanIsReady(ScanIsReady);
                this.Invoke(d, new object[] { sData, bIsSuccess });
            }
            else
            {
                //OnScanReady(new EventArgs());
                _BarcodeText = sData;
                _bIsSuccess = bIsSuccess;
                OnScanReady(new MEFdemo1.HAL.DeviceControlContracts.BarcodeEventArgs(sData, _bIsSuccess)); //call event fire function
                //_bReadingBarcode = false;
            }
        }
        protected virtual void OnScanReady(EventArgs e)
        {
            if (ScanReady != null) //check if there is any listener
            {
                //fire event
                ScanReady(this, new MEFdemo1.HAL.DeviceControlContracts.BarcodeEventArgs(_BarcodeText, _bIsSuccess));
            }
        }
        protected virtual void OnScanReady(MEFdemo1.HAL.DeviceControlContracts.BarcodeEventArgs e)
        {
            if (ScanReady != null)
            {
                ScanReady(this, e);
            }
        }
        void addLog(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }
        public new void Dispose()
        {
            addLog("IntermecScanControl Dispose()...");
            //dispose BarcodeReader
            if (bcr != null)
            {
                //                addLog("IntermecScanControl Dispose(): Calling CancelRead(true)...");
                //                bcr.CancelRead(true);
                addLog("IntermecScanControl Dispose(): Disposing BarcodeReader...");
                bcr.ThreadedRead(false);
                bcr.BarcodeRead -= bcr_BarcodeRead;
                //bcr.BarcodeReadCanceled -= bcr_BarcodeReadCanceled;
                //bcr.BarcodeReadError -= bcr_BarcodeReadError;
                bcr.Dispose();
                bcr = null;
            }
        }
    }
}