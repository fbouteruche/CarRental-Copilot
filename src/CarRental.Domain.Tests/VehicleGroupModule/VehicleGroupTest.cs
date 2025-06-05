using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Domain.VehicleGroupModule;
using FluentAssertions;
using System;

namespace CarRental.Domain.Tests.VehicleGroupModule
{
    [TestClass]
    [TestCategory("Domain")]
    public class VehicleGroupTest
    {
        [TestMethod]
        public void ShouldCreateVehicleGroup_Correctly()
        {
            VehicleGroup vehicleGroup = new VehicleGroup(0, "nome", 12.3f, 15.5f, 20.5f, 30, 16.3f, 45.2f);

            string result = vehicleGroup.Validate();

            Assert.AreEqual("VALID", result);
        }

        [TestMethod]
        public void ShouldShowError_CompletelyIncorrectGroup()
        {
            VehicleGroup vehicleGroup = new VehicleGroup(0, "", 0f, 0f, 0f, 0, 0f, 0f);

            string result = vehicleGroup.Validate();
            //Tests with all invalid messages are complicated to maintain. Maybe we should change the model.
            Assert.AreEqual("The name cannot be null\nThe daily rate for the Daily Plan cannot be null\nThe per KM rate for the Daily Plan cannot be null\nThe daily rate for the Controlled Plan cannot be null\nThe KM limit for the Controlled Plan cannot be null\nThe exceeded KM rate for the Controlled Plan cannot be null\nThe daily rate for the Unlimited Plan cannot be null\n", 
                            result);
        }

        [TestMethod]
        public void ShouldShowError_OnlyNameCorrect()
        {
            VehicleGroup vehicleGroup = new VehicleGroup(0, "nome", 0f, 0f, 0f, 0, 0f, 0f);

            string result = vehicleGroup.Validate();
            //Tests with all invalid messages are complicated to maintain. Maybe we should change the model.
            Assert.AreEqual("The daily rate for the Daily Plan cannot be null\nThe per KM rate for the Daily Plan cannot be null\nThe daily rate for the Controlled Plan cannot be null\nThe KM limit for the Controlled Plan cannot be null\nThe exceeded KM rate for the Controlled Plan cannot be null\nThe daily rate for the Unlimited Plan cannot be null\n",
                            result);
        }

        [TestMethod]
        public void ShouldShowError_OnlyNameIncorrect()
        {
            VehicleGroup vehicleGroup = new VehicleGroup(0, "", 10f, 10f, 10f, 10, 10f, 10f);

            string result = vehicleGroup.Validate();

            Assert.AreEqual("The name cannot be null\n",
                            result);
        }
    }
}
