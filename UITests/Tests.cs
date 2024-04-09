using NUnit.Framework;
using OpenQA.Selenium;
using UiTests;
using UiTests.Models;

namespace UITests
{
    public class Tests
    {
        [Test]
        public void MyFirstTest()
        {
            Browser.Start();
            Browser.GoTo("http://localhost:3000");
            Browser.GetDriver().FindElement(By.TagName("input")).SendKeys("Some name..");
            Browser.Stop();
        }
    }
}