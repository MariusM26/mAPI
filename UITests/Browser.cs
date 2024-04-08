#nullable disable
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading;
using System;
using UITests;

namespace UiTests
{
    public static class Browser
    {
        private static IWebDriver _webDriver;

        public static IWebDriver GetDriver() => _webDriver;

        public static string CurrentUrlEncoded { get => _webDriver.Url; set => _webDriver.Url = value; }


        #region Navigation
        public static void GoTo(string url)
        {
            Wait(500);
            _webDriver.Navigate().GoToUrl(url);
            Wait(500);
        }

        public static void Refresh()
        {
            _webDriver.Navigate().Refresh();
        }
        #endregion


        #region Wait
        public static void Wait(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
        }
        #endregion


        #region Setup
        public static void Start()
        {
            _webDriver = WebDriverProvider.Get();
        }

        public static void Stop()
        {
            if (_webDriver is null)
            {
                return;
            }

            _webDriver.Close();
            _webDriver.Quit();
            _webDriver.Dispose();
        }

        public static void StartNewWindow()
        {
            try
            {
                _webDriver.SwitchTo().NewWindow(WindowType.Window);
                Wait(500);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        public static void CloseExtraWindow(string originalWindowHandle)
        {
            _webDriver.Close();

            if (!_webDriver.WindowHandles.Contains(originalWindowHandle))
            {
                Assert.Fail("The original window handle could not be identified.");
            }

            _webDriver.SwitchTo().Window(originalWindowHandle);
            Wait(500);
        }
        #endregion


        #region Elements
        public static void SendKeyboardKeys(string keys)
        {
            var builder = new Actions(_webDriver);
            var sendKey = builder.SendKeys(keys).Build();

            sendKey.Perform();
        }

        public static void Hover(IWebElement element)
        {
            var action = new Actions(_webDriver);
            action.MoveToElement(element).Perform();
        }

        public static void PasteClipboardText()
        {
            var builder = new Actions(_webDriver);

            builder.KeyDown(Keys.Control)
                   .SendKeys("v")
                   .KeyUp(Keys.Control)
                   .Build()
                   .Perform();
        }
        #endregion
    }
}