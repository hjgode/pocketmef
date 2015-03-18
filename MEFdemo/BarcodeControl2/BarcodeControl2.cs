using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

//using System.ComponentModel;
using System.ComponentModel.Composition;

using MEFdemo1.HAL.DeviceControlContracts;

namespace MEFdemo1
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IBarcodeScanControl))]
    public partial class BarcodeControl2 : UserControl, IBarcodeScanControl
    {
        public string _BarcodeText = "";
        bool _bIsSuccess = false;
        System.Windows.Forms.Timer timer1;
        public BarcodeControl2()
        {
            InitializeComponent();
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = (5000);
            timer1.Enabled=true;
        }
        int iCounter = 0;
        void timer1_Tick(object sender, EventArgs e)
        {
            _bIsSuccess = !_bIsSuccess;
            _BarcodeText = "timer fired " + (++iCounter).ToString();
            ScanIsReady(_BarcodeText, _bIsSuccess);
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
    }
}