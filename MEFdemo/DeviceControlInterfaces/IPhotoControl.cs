using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace MEFdemo1.HAL.DeviceControlContracts
{
    /// <summary>
    /// Bilderfassung durch die Kamera über die Fototaste(n), inkl. Anzeige eines Previews und des finalen Bildes (z.B. in einer PictureBox)
    /// Größe des Controls 480X400
    /// </summary>
    public interface IPhotoControl : IComponent
    {
        /// <summary>
        /// Signalisiert die Erfassung eines Bildes. Wird ausgelöst wenn der Benutzer die Fototaste(n) loslässt. 
        /// </summary>
        event EventHandler ImageReady;

        /// <summary>
        /// Signalisiert den Preview eines Bildes. Wird ausgelöst wenn der Benutzer die Fototaste(n) gedrückt hält.
        /// Gleichzeitig wird im implementierenden Control das Bild als Preview angezeigt.
        /// </summary>
        /// <remarks>Das Event wird nur einmal pro gedrücktem Tastenzustand ausgelöst.</remarks>
        event EventHandler InPreview;

        /// <summary>
        /// Liefert der Name die Fototaste die soll gedrücht werden um Preview bzw Erfassing eines Bildes zu starten. Falls Preview und
        /// Erfassing werden mit unteschildlichen Tasten ausgelöst soll der Rückbagewert in Previewmodus geändert werden. 
        /// <remarks>
        /// Das Parentform enthält ein hinweis welche Taste im Moment gedrückt werden soll
        /// </remarks>
        /// </summary>
        string PhotoKey { get; }

        /// <summary>
        /// Methode, welche das erfasste Bild als JPEG abspeichert. 
        /// </summary>
        /// <remarks>Abspeicherung muss in der maximal möglichen Auflösung erfolgen.</remarks>
        /// <param name="savePath">Speicherort (Pfad,vollständiger Dateiname)</param>
        /// <returns><tt>true</tt> Speichervorgang erfolgreich, <tt>false</tt> Speicherung nicht erfolgreich</returns>
        bool SaveAsJpg(string savePath);
    }
}
