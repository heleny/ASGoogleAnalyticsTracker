using Android.Gms.Analytics;
using Android.App;
using System;

namespace AS.Google.Analytics.Droid
{
    public class GoogleAnalyticsTracker :  IGoogleAnalyticsTracker
    {
        private const string OFFLINE = "Offline";
        private static string trackingId;
        private static GoogleAnalyticsTracker Instance;

        private GoogleAnalyticsTracker(string trackingID) {
            var analytics = GoogleAnalytics.GetInstance (Application.Context);
            analytics.SetLocalDispatchPeriod(10);
            GaTracker = analytics.NewTracker(trackingId);
            GaTracker.EnableAdvertisingIdCollection(true);
            GaTracker.EnableExceptionReporting(true);
        }

        public static GoogleAnalyticsTracker GetInstance(string trackingID) {
            if (Instance == null || trackingId != trackingID) {
                Instance = new GoogleAnalyticsTracker(trackingID);
            }

            return Instance;
        }

        public void TrackScreenView(string screenName, Boolean online) {
            screenName = (online ? "" : OFFLINE) + " - " + screenName;
            GaTracker.SetScreenName(screenName);
            GaTracker.Send(new HitBuilders.ScreenViewBuilder().Build());
        }

        public void TrackEvent(string category, string eventName, Boolean online) {
            string action = (online ? "" : OFFLINE) + " - " + eventName;
            GaTracker.Send(new HitBuilders.EventBuilder()
                .SetCategory(category)
                .SetAction(action)
                .Build());
        }

        /**
         * Track caught exceptions 
         */
        public void TrackException(Exception exception) {
            string description = exception.Message + "\nStackTrace:\n" + exception.StackTrace;
            GaTracker.Send(new HitBuilders.ExceptionBuilder()
                .SetDescription(description)
                .SetFatal(true)
                .Build());
        }

        public Tracker GaTracker { get; private set; }
    }
}

