using AppForMovies.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit;

namespace AppForSEII2526.UIT.Deliveries
{
    public class UCScheduleDelivery_UIT : UC_UIT
    {
        private SelectOrdersForDelivery_PO selectOrders;

        private const string ADMIN_EMAIL = "buuble1212@gmail.com";
        private const string ADMIN_PASSWORD = "Gulbene2022!";
        private const string POSTAL_CODE = "28008";
        private const string DRIVER_DISPLAY = "Marco Rossi (Available)";
        private const string DRIVER_NAME = "Marco Rossi";

        public UCScheduleDelivery_UIT(ITestOutputHelper output)
            : base(output)
        {
            Initial_step_opening_the_web_page();
            selectOrders = new SelectOrdersForDelivery_PO(_driver, _output);
        }

      

        private void Precondition_Login()
        {
            Perform_login(ADMIN_EMAIL, ADMIN_PASSWORD);

            //fix required for timeout issues (tests failing because login not completed)
            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(d => !d.Url.Contains("/Account/Login"));
        }

        private void GoToSelectOrders()
        {
            Precondition_Login();
            _driver.Navigate().GoToUrl(_URI + "delivery/selectorders");
        }


        //no orders available
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_SCE2_2_NoOrdersAvailable()
        {
            GoToSelectOrders();

            Assert.True(
                _driver.FindElement(By.Id("ErrorsShown")).Text.Contains("No orders"),
                "Expected informative error when no orders are available. (Remove orders from database)"
            );
        }


        //filter orders
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_SCE3_3_FilterOrders()
        {
            GoToSelectOrders();

            selectOrders.FilterOrders(POSTAL_CODE, null);

            Assert.True(
                _driver.FindElement(By.Id("TableOfOrders"))
                       .FindElements(By.TagName("tr")).Count > 0,
                "Filtered orders should be shown"
            );
        }

        //No orders selected
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_SCE2_2_NoOrderSelected()
        {
            GoToSelectOrders();

            Assert.False(
                _driver.FindElement(By.Id("continueButton")).Enabled,
                "Continue button must be disabled when no orders are selected"
            );
        }


        //return to modify selected orders
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_SCE5_6_ModifySelectedOrders()
        {
            GoToSelectOrders();

            selectOrders.FilterOrders(POSTAL_CODE, null);
            selectOrders.SelectFirstOrder();
            selectOrders.Continue();

            _driver.Navigate().GoToUrl(_URI + "delivery/selectorders");

            Assert.True(
                _driver.PageSource.Contains("Selected Orders"),
                "Expected to return to order selection page"
            );
        }

        
        // missing driver
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_SCE6_7_MandatoryDriverMissing()
        {
            GoToSelectOrders();

            selectOrders.FilterOrders(POSTAL_CODE, null);
            selectOrders.SelectFirstOrder();
            selectOrders.Continue();

            var submitButton = _driver.FindElement(By.XPath("//button[@type='submit']"));

            Assert.True(submitButton.GetAttribute("disabled") != "", "Submit button must be disabled when driver is not selected");

        }
    }
}