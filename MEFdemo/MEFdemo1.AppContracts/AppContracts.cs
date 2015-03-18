using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MEFdemo1.AppContracts
{
    //describe a public appPlugin interface (contract)
    public interface IAppPlugin : IComponent
    {
        Bitmap appBitmap{ get; }
        System.Windows.Forms.DialogResult DialogResult { get; }
        string sAppText { get; }
        string sReturnData { get; }
    }
}
