﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace MEFdemo1.HAL.DeviceControlContracts
{
    public class BarcodeEventArgs
    {
        public BarcodeEventArgs(string s, bool bSuccess) { Text = s; _bSuccess = bSuccess; }
        public String Text { get; private set; } // readonly
        public bool _bSuccess { get; private set; }
    }

    public delegate void BarcodeEventHandler(object sender, BarcodeEventArgs e);
    /// <summary>
    /// Barcodescan. Das Control ist unsichtbar
    /// </summary>
    public interface IBarcodeScanControl : IComponent
    {
        // Declare the delegate (if using non-generic pattern).
        event BarcodeEventHandler ScanReady;
        /// <summary>
        /// Signalisiert die Erfassung eines Barcode. Das Event wird sowohl bei erfolgreichem Scan, als auch bei einem nicht 
        /// erfolgreichem Scan ausgelöst.
        /// </summary>
        //event EventHandler ScanReady;
        /// <summary>
        /// Liefert Barcode, wenn interpretierbar und erfolgreich. Wenn der Barcodescan nicht erfolgreich war, wird eine Fehlermeldung als
        /// Rückgabeparameter zurückgegeben. Wenn der Scan zwar erfolgreich war, aber der gescannte Barcode nicht als String interpretierbar ist, dann erfolgt
        /// die Rückgabe des Strings "Barode nicht anzeigbar".
        /// </summary>
        string BarcodeText { get; }
        /// <summary>
        /// Liefert <tt>true</tt> wenn der Scan eines Barcodes erfolgreich war, sonst <tt>false</tt>. Standardbarcodetypen sollen unterstützt werden.
        /// </summary>
        bool IsSuccess { get; }
    }
}
