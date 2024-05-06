using NUnit.Framework;
using OpenQA.Selenium;
using System.Linq;
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

            var inputs = Browser.GetDriver().FindElements(By.TagName("input")).ToList();

            inputs.First().SendKeys("test@test.test");
            inputs.Skip(1).First().SendKeys("Pass123$");

            Browser.GetDriver().FindElement(By.TagName("button")).Click();

            Browser.Stop();
        }
    }
}