using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.ViewManagement;

namespace FirmaGUI.Helpers
{
    internal class ScreenCaptureHandler
    {
        private readonly ApplicationDataContainer LocalSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public void SetSensitiveContentProtection(bool IsAllowed)
        {
            LocalSettings.Values["SensitiveContentProtectionEnabled"] = IsAllowed;
        }
        public bool IsSensitiveContentProtectionEnabled()
        {
            try
            {
                return (bool)LocalSettings.Values["SensitiveContentProtectionEnabled"];
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                SetSensitiveContentProtection(true);
                return true;
            }
        }
        public void SetScreenCapture(bool IsScreenCaptureAllowed)
        {
            ApplicationView.GetForCurrentView().IsScreenCaptureEnabled = (IsScreenCaptureAllowed || !IsSensitiveContentProtectionEnabled());

        }
    }
}
