using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.Deliveries
{
    public class CreateDeliveryAssignment_PO : PageObject
    {
        private By _driverSelectBy = By.Id("DriverSelect");
        private By _deadlineBy = By.Id("Deadline");
        private By _extraRewardBy = By.Id("ExtraReward");
        private By _messageBy = By.Id("PersonalMessage");
        private By _confirmButtonBy = By.XPath("//button[@type='submit']");
        private By _dialogBy = By.Id("DialogOKSaveDelete");

        public CreateDeliveryAssignment_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) { }

        public void FillAssignmentInfo(
            string driverName,
            DateTime deadline,
            string? reward,
            string? message)
        {
            // wait until page is ready
            WaitForBeingVisible(_driverSelectBy);

            // driver
            SelectElement select = new SelectElement(
                _driver.FindElement(_driverSelectBy));
            select.SelectByText(driverName);

            // deadlin
            InputDateInDatePicker(_deadlineBy, deadline);

            // reward
            if (!string.IsNullOrEmpty(reward))
            {
                _driver.FindElement(_extraRewardBy).Clear();
                _driver.FindElement(_extraRewardBy).SendKeys(reward);
            }

            // msg
            if (!string.IsNullOrEmpty(message))
            {
                _driver.FindElement(_messageBy).Clear();
                _driver.FindElement(_messageBy).SendKeys(message);
            }
        }

        public void Confirm()
        {
            WaitForBeingClickable(_confirmButtonBy);
            _driver.FindElement(_confirmButtonBy).Click();
        }

    }
}
