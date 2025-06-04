using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CarRental.Domain.VehicleModule;

namespace CarRental.Domain.Tests.VehicleModule
{
    [TestClass]
    [TestCategory("Domain")]
    public class VehicleTest
    {
        [TestMethod]
        public void ShouldCreateVehicle_Correct()
        {
            Vehicle vehicle = new Vehicle(0, "Ecosport", null, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'L', true, true, true, true, null);

            Assert.AreEqual("VALID", vehicle.Validate());
        }

        [TestMethod]
        public void ShouldShowErrorVehicle_CompletelyIncorrect()
        {
            Vehicle vehicle = new Vehicle(0, "", null, "", "", "", "", "", 0, 0, 0, 0, 0, 'a', false, false, false, false, null);

            string expected = "The field 'model' cannot be empty!\n" +
                              "The field 'licensePlate' cannot be empty!\n" +
                              "The field 'chassis' cannot be empty!\n" +
                              "The field 'brand' cannot be empty!\n" +
                              "The field 'color' cannot be empty!\n" +
                              "The field 'fuelType' cannot be empty!\n" +
                              "The field 'tankCapacity' cannot be empty!\n" +
                              "The field 'year' cannot be empty!\n" +
                              "The field 'mileage' cannot be empty!\n" +
                              "The field 'numberOfDoors' cannot be empty!\n" +
                              "The field 'passengerCapacity' cannot be empty!\n" +
                              "The field 'trunkSize' must be 'S', 'M', or 'L'!\n";

            Assert.AreEqual(expected, vehicle.Validate());
        }
    }
}
