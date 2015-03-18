using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace MEFdemo1.HAL.DeviceControlContracts
{
    /// <summary>
    /// Unterschrift
    /// Größe des Controls 480X400
    /// </summary>
   public interface ISignatureControl : IComponent
    {
        /// <summary>
        /// Signalisiert die Erfassung einer Unterschrift. Wird ausgelöst wenn der Benutzer auf einem entsprechendem Control
        /// unterschrieben hat.
        /// </summary>
        /// <remarks>
        /// Ein mehrmaliges Auslösen z.B. wenn der Benutzer den Stift absetzt ist tolerabel.
        /// </remarks>
        event EventHandler SignatureReady;
        /// <summary>
        /// Methode, welche die im Unterschriftencontrol erfasste Unterchrift als JPEG abspeichert. 
        /// </summary>
        /// <remarks>
        /// Abspeicherung muss in der maximal möglichen Auflösung erfolgen.</remarks>
        /// <param name="savePath">Speicherort (Pfad,vollständiger Dateiname)</param>
        /// <returns><tt>true</tt> Speichervorgang erfolgreich, <tt>false</tt> Speicherung nicht erfolgreich</returns>
        bool SaveAsJpg(string savePath);
    }
}
