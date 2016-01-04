using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace AS.Google.Analytics.Touch {

    public class Plugin : IMvxPlugin {
        public void Load() {
            Mvx.RegisterSingleton<IGoogleAnalyticsTracker>(new GoogleAnalyticsTracker());
        }
    }
}
