using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MEFdemo1.HAL.DeviceControlContracts
{
    /// <summary>
    /// Das Control ist unsichtbar
    /// </summary>
    public interface IUtilityControl
    {
        /// <summary>
        /// Wird gesetzt auf <tt>true</tt>, wenn ein Vibrieren ausgelöst werden soll.
        /// </summary>
        /// <param name="duration">Zeit in Milisekunde für das akustische Feedback.</param>
        void Vibration(int duration);
        /// <summary>
        /// Wird gesetzt auf <tt>true</tt>, wenn die Tastaturbeleuchtung angeschaltet werden soll. Wenn <tt>false</tt> wird die Tastaturbeleuchtung ausgeschaltet.
        /// </summary>
        bool KeyboardBacklightState { set; }
        /// <summary>
        /// Wird auf <tt>true</tt> gesetzt wenn SIP (Windows Mobil Bildschirmtastatur) aktiviert werden soll. In diesem Zustand wird eine Tastaturicon eingeblendet, welches
        /// den Aufruf der Tastatur ermöglicht. <tt>false</tt> führt zur Deaktivierung des entsprechenden Tastaturicons.
        /// </summary>
        bool SipState { set; } 
        /// <summary>
        /// Methode welche eine Erfolgesmeldungston auslöst.
        /// </summary>
        /// <param name="duration"> Zeit in Milisekunde für das akustische Feedback.</param>
        void GoodSound (int duration);
        /// <summary>
        /// Methode welche einen Fehlermeldungston auslöst.
        /// </summary>
        /// <param name="duration"> Zeit in Milisekunde für das akustische Feedback.</param>
        void BadSound (int duration);
        /// <summary>
        /// Liefert den Prozensatz des Akkufüllstandes zurück. Ein erneuter Aufruf der Methode führt
        /// zu einer Aktualisierung des zurückgelieferten Akkufüllstandes.
        /// </summary>
        int AccuState { get; }
    }
}
