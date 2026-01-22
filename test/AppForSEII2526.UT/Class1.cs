using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT
{
    internal class Class1
    {
        //public int GetSelectedOrdersCount()
        //{
        //    return _driver
        //        .FindElements(By.XPath("//button[starts-with(@id,'removeOrder_')]"))
        //        .Count;
        //}

        //public void RemoveSelectedOrderById(int orderId)
        //{
        //    var removeButton = _driver.FindElement(By.Id($"removeOrder_{orderId}"));
        //    removeButton.Click();
        //}


        //public void SelectTwoOrders()
        //{
        //    WaitForBeingVisible(_tableOfOrdersBy);

        //    var addButtons = _driver
        //        .FindElement(_tableOfOrdersBy)
        //        .FindElement(By.TagName("tbody"))
        //        .FindElements(By.TagName("button"));

        //    addButtons[0].Click();
        //    addButtons[1].Click();
        //}



        //[Fact]
        //[Trait("LevelTesting", "Functional Testing")]
        //public void UC4_SCE5_AddRemoveOrders_ThenNavigateBack()
        //{
        //    GoToSelectOrders();

        //    selectOrders.SelectTwoOrders();

        //    // Remove first selected order
        //    selectOrders.RemoveSelectedOrderById(10);

        //    // Continue
        //    selectOrders.Continue();

        //    // Go back
        //    _driver.Navigate().Back();

        //    // ASSERT: one order remains selected
        //    Assert.Equal(1, selectOrders.GetSelectedOrdersCount());

        //    // ASSERT: continue still enabled
        //    Assert.True(
        //        _driver.FindElement(By.Id("continueButton")).Enabled,
        //        "Continue must remain enabled when one order is selected"
        //    );

    }
}
