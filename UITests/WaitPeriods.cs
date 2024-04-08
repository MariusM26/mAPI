using System;

namespace UiTests
{
    public static class WaitPeriods
    {
        public static readonly TimeSpan PageLoad = TimeSpan.FromSeconds(30);

        public static readonly TimeSpan ImplicitWait = TimeSpan.FromSeconds(3);

        public static readonly TimeSpan ExplicitWait = TimeSpan.FromSeconds(5);

        public static readonly TimeSpan PollingInterval = TimeSpan.FromMilliseconds(100);
    }
}