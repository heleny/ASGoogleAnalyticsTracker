using System;

namespace AS.Google.Analytics
{
    public interface IGoogleAnalyticsTracker {
        void TrackScreenView(string screenName, Boolean online);
        void TrackEvent(string category, string action, Boolean online);
        void TrackException(Exception exception);
    }
}
