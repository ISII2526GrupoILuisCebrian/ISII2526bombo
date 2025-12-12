using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.Deliveries
{
    public class AssignmentDetail_PO : PageObject
    {
        public AssignmentDetail_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) { }


        public bool CheckAssignment(string driver, string reward)
        {
            WaitForBeingVisible(By.Id("DriverName"));
            return _driver.FindElement(By.Id("DriverName")).Text.Contains(driver)
                && _driver.FindElement(By.Id("ExtraReward")).Text.Contains(reward);
        }

        public bool CheckOrders(List<string[]> expectedOrders)
        {
            return CheckBodyTable(expectedOrders, By.Id("AssignedOrders"));
        }
    }
}

