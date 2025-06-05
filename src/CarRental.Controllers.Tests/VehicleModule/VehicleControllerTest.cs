using FluentAssertions;
using CarRental.Controllers.VehicleGroupModule;
using CarRental.Controllers.Shared;
using CarRental.Controllers.VehicleModule;
using CarRental.Domain.VehicleGroupModule;
using CarRental.Domain.VehicleModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Domain.VehicleImageModule;
using System.Collections.Generic;
using CarRental.Controllers.Tests.Shared;

namespace CarRental.Controllers.Tests.VehicleModule
{
    [TestClass]
    [TestCategory("Controllers")]
    public class VehicleControllerTests
    {
        VehicleController controller = null;
        VehicleGroupController vehicleGroupController = null;
        Vehicle newVehicle;
        VehicleGroup vehicleGroup;
        List<VehicleImage> images;

        public VehicleControllerTests()
        {
            controller = new VehicleController();
            vehicleGroupController = new VehicleGroupController();

            ResetDatabase.ResetAllTables();
        }
        [TestMethod]
        public void ShouldInsertVehicle()
        {
            // arrange
            vehicleGroup = new VehicleGroup(0, "SUV", 10.0, 10.5, 10, 100, 15.5, 45.8);
            vehicleGroupController.InsertNew(vehicleGroup);
            newVehicle = new Vehicle(0, "Ecosport", vehicleGroup, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Prata", "Gasolina Comum", 60.5, 2018, 30000, 4, 5, 'L', true, true, true, true, null);

            // act
            controller.InsertNew(newVehicle);

            // assert
            Vehicle foundVehicle = controller.SelectById(newVehicle.Id);
            foundVehicle.Should().Be(newVehicle);
        }

        [TestMethod]
        public void ShouldSelectTwoVehicles()
        {
            // arrange
            vehicleGroup = new VehicleGroup(0, "SUV", 10.0, 10.5, 10, 100, 15.5, 45.8);
            vehicleGroupController.InsertNew(vehicleGroup);
            newVehicle = new Vehicle(0, "Ecosport", vehicleGroup, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Prata", "Gasolina Comum", 60.5, 2018, 30000, 4, 5, 'L', true, true, true, true, null);

            // act
            controller.InsertNew(newVehicle);
            controller.InsertNew(newVehicle);

            // assert
            List<Vehicle> foundVehicles = controller.SelectAll();
            foundVehicles.Count.Should().Be(2);
        }

        [TestMethod]
        public void ShouldEditVehicle()
        {
            // arrange
            images = new List<VehicleImage>();
            vehicleGroup = new VehicleGroup(0, "SUV", 10.0, 10.5, 10, 100, 15.5, 45.8);
            vehicleGroupController.InsertNew(vehicleGroup);
            newVehicle = new Vehicle(0, "Ecosport", vehicleGroup, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Prata", "Gasolina Comum", 60.5, 2018, 30000, 4, 5, 'L', true, true, true, true, null);

            VehicleGroup editedGroup = new VehicleGroup(0, "Pique Velozes e Furiosos", 100, 60.5, 40, 300, 45.2, 500);
            vehicleGroupController.InsertNew(editedGroup);
            Vehicle editedVehicle = new Vehicle(0, "Monza Tubarão Turbão Rebaixado", editedGroup, "ABC1234", "1ABCD12A12AB1AB1ABC", "Chevrolet", "Bordo", "Etanol", 60.5, 1996, 240000, 4, 5, 'L', false, false, false, false, images);
            // act
            controller.InsertNew(newVehicle);
            controller.Edit(newVehicle.Id, editedVehicle);

            // assert
            Vehicle foundVehicle = controller.SelectById(newVehicle.Id);
            foundVehicle.Should().Be(editedVehicle);
        }

        [TestMethod]
        public void ShouldDeleteVehicle()
        {
            // arrange
            vehicleGroup = new VehicleGroup(0, "SUV", 10.0, 10.5, 10, 100, 15.5, 45.8);
            vehicleGroupController.InsertNew(vehicleGroup);
            newVehicle = new Vehicle(0, "Ecosport", vehicleGroup, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Prata", "Gasolina Comum", 60.5, 2018, 30000, 4, 5, 'G', true, true, true, true, null);

            // act
            controller.InsertNew(newVehicle);
            controller.Delete(newVehicle.Id);

            // assert
            List<Vehicle> foundVehicles = controller.SelectAll();
            foundVehicles.Count.Should().Be(0);
        }
    }
}
