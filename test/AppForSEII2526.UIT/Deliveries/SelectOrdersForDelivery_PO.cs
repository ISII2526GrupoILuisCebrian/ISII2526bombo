using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.Deliveries
{
    public class SelectOrdersForDelivery_PO : PageObject
    {
        private By _postalCodeBy = By.Id("inputPostalCode");
        private By _minPriceBy = By.Id("inputMinPrice");
        private By _searchOrdersBy = By.Id("searchOrders");
        private By _tableOfOrdersBy = By.Id("TableOfOrders");
        private By _continueButtonBy = By.Id("continueButton");

        public SelectOrdersForDelivery_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) { }

        public void FilterOrders(string postalCode, string? minPrice)
        {
            WaitForBeingVisible(_postalCodeBy);
            _driver.FindElement(_postalCodeBy).SendKeys(postalCode);

            if (minPrice != null)
                _driver.FindElement(_minPriceBy).SendKeys(minPrice);

            _driver.FindElement(_searchOrdersBy).Click();
            Thread.Sleep(2000);
        }

        public void SelectFirstOrder()
        {
            WaitForBeingVisible(_tableOfOrdersBy);

            var firstAddButton = _driver
                .FindElement(_tableOfOrdersBy)
                .FindElement(By.TagName("tbody"))
                .FindElements(By.TagName("button"))
                .First();

            firstAddButton.Click();
        }


        public void Continue()
        {
            WaitForBeingClickable(_continueButtonBy);
            _driver.FindElement(_continueButtonBy).Click();
        }

        public bool CheckOrders(List<string[]> expectedOrders)
        {
            return CheckBodyTable(expectedOrders, _tableOfOrdersBy);
        }
    }
}

