using AppForMovies.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.Deliveries
{
    public class UCScheduleDelivery_UIT : UC_UIT
    {
        private SelectOrdersForDelivery_PO selectOrders;

        public UCScheduleDelivery_UIT(ITestOutputHelper output) : base(output)
        {
            Initial_step_opening_the_web_page();
            selectOrders = new SelectOrdersForDelivery_PO(_driver, _output);
        }

        private void Precondition_Login()
        {
            Perform_login("buuble1212@gmail.com", "Gulbene2022!");
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_Delivery_BasicFlow()
        {
            // arrange
            Precondition_Login();

            var createAssignment = new CreateDeliveryAssignment_PO(_driver, _output);
            var detail = new AssignmentDetail_PO(_driver, _output);

            // act
            _driver.Navigate().GoToUrl(_URI + "delivery/selectorders");

            selectOrders.FilterOrders("02006", null);
            selectOrders.SelectFirstOrder();
            selectOrders.Continue();

            createAssignment.FillAssignmentInfo(
                "Marco Rossi (Available)",
                DateTime.Today.AddDays(2),
                "10",
                "Handle with care"
            );
            createAssignment.Confirm();

            // assert
            //Assert.True(detail.CheckAssignment("Marco Rossi", "10"));

        }
    }
}

