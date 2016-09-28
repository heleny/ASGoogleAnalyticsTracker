using System;
using Google.Analytics;
using AS.Google.Analytics;

namespace AS.Google.Analytics.Touch
{
    public class GoogleAnalyticsTracker : IGoogleAnalyticsTracker
    {
        private const string OFFLINE = "Offline";
        private static string trackingId;
        private static GoogleAnalyticsTracker Instance;

        private GoogleAnalyticsTracker(string trackingID) {
            trackingId = trackingID;
            Gai.SharedInstance.Logger.SetLogLevel(LogLevel.Info);
#if (DEBUG || ADHOC)
            Gai.SharedInstance.DispatchInterval = 10;
#else
            Gai.SharedInstance.DispatchInterval = 24 * 60 * 60;
#endif
            Gai.SharedInstance.TrackUncaughtExceptions = true;  // tack uncaught exception
            GaTracker = Gai.SharedInstance.GetTracker(trackingId);
            GaTracker.SetAllowIdfaCollection(true);
        }

        public static GoogleAnalyticsTracker GetInstance(string trackingID) {
            if (Instance == null || trackingId != trackingID) {
                Instance = new GoogleAnalyticsTracker(trackingID);
            }

            return Instance;
        }

        public void TrackScreenView(string screenName, Boolean online) {
            Gai.SharedInstance.DefaultTracker.Set(GaiConstants.ScreenName, (online ? "" : OFFLINE) + " - " + screenName);
            Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());
        }

        public void TrackEvent(string category, string eventName, Boolean online) {
            string action = (online ? "" : OFFLINE) + " - " + eventName;
            Gai.SharedInstance.DefaultTracker.Set(GaiConstants.Event, action);
            Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateEvent(category, action, null, null).Build());
        }

        /**
         * Track caught exceptions 
         */
        public void TrackException(Exception exception) {
            string description = exception.Message + "\nStackTrace:\n" + exception.StackTrace;
            Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateException(description, null).Build());
        }

        public ITracker GaTracker { get; private set; }
    }
}

