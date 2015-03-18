using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace MEFdemo1.HAL.DeviceControlContracts
{
    /// <summary>
    /// Bilderfassung durch den Scanner, inkl. Anzeige einer Preview wenn der Benutzer die Scantaste(n) gedrückt hält. 
    /// Anzeige des finalen Bildes im selben Control, wenn Photovorgang abgeschlossen.
    /// Größe des Controls 480X400
    /// </summary>
    public interface IImagerControl : IComponent
    {
        /// <summary>
        /// Signalisiert die Erfassung eines Bildes. Wird ausgelöst wenn der Benutzer die Scantaste(n) loslässt. 
        /// </summary>
        event EventHandler ImageReady;

        /// <summary>
        /// Signalisiert den Preview eines Bildes. Wird ausgelöst wenn der Benutzer die Scantaste(n) gedrückt hält.
        /// Gleichzeitig wird im implementierenden Control das Bild als Preview angezeigt.
        /// </summary>
        /// <remarks>Das Event wird nur einmal pro gedrücktem Tastenzustand ausgelöst.</remarks>
        event EventHandler InPreview;

        /// <summary>
        /// Methode, welche das erfasste Bild als JPEG abspeichert. 
        /// </summary>
        /// <remarks>Abspeicherung muss in der maximal möglichen Auflösung erfolgen.</remarks>
        /// <param name="savePath">Speicherort (Pfad,vollständiger Dateiname)</param>
        /// <returns><tt>true</tt> Speichervorgang erfolgreich, <tt>false</tt> Speicherung nicht erfolgreich</returns>
        bool SaveAsJpg(string savePath);
    }
}
