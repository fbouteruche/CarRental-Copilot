using FluentAssertions;
using CarRental.Controllers.VehicleGroupModule;
using CarRental.Controllers.Shared;
using CarRental.Domain.VehicleGroupModule;
using CarRental.Controllers.Tests.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CarRental.Controllers.Tests.VehicleGroupModule
{
    [TestClass]
    [TestCategory("Controllers")]
    public class VehicleGroupControllerTests
    {
        VehicleGroupController controller = null;

        public VehicleGroupControllerTests()
        {
            controller = new VehicleGroupController();
            ResetDatabase.ResetAllTables();
        }

        [TestMethod]
        public void ShouldInsert_VehicleGroup()
        {
            VehicleGroup newVehicleGroup = new VehicleGroup(0, "name", 12.50f, 25.73f, 13.99f, 200, 15f, 11.2f);

            controller.InsertNew(newVehicleGroup);

            var foundVehicleGroup = controller.SelectById(newVehicleGroup.Id);
            foundVehicleGroup.Should().Be(newVehicleGroup);
        }

        [TestMethod]
        public void ShouldUpdate_VehicleGroup()
        {
            VehicleGroup vehicleGroup = new VehicleGroup(0, "name", 12.50f, 25.73f, 13.99f, 200, 14f, 30.2f);
            controller.InsertNew(vehicleGroup);

            VehicleGroup newVehicleGroup = new VehicleGroup(0, "name", 5.12f, 37.52f, 99.31f, 2, 15f, 11.2f);

            controller.Edit(vehicleGroup.Id, newVehicleGroup);

            var updatedVehicleGroup = controller.SelectById(vehicleGroup.Id);
            updatedVehicleGroup.Should().Be(newVehicleGroup);
        }

        [TestMethod]
        public void ShouldDelete_VehicleGroup()
        {
            VehicleGroup vehicleGroup = new VehicleGroup(0, "name", 12.50f, 25.73f, 13.99f, 200, 15f, 11.2f);
            controller.InsertNew(vehicleGroup);

            controller.Delete(vehicleGroup.Id);

            var foundVehicleGroup = controller.SelectById(vehicleGroup.Id);
            foundVehicleGroup.Should().BeNull();
        }

        [TestMethod]
        public void ShouldSelect_VehicleGroupById()
        {
            VehicleGroup vehicleGroup = new VehicleGroup(0, "name", 12.50f, 25.73f, 13.99f, 200, 14f, 65.2f);
            controller.InsertNew(vehicleGroup);

            var foundVehicleGroup = controller.SelectById(vehicleGroup.Id);

            foundVehicleGroup.Should().NotBeNull();
        }

        [TestMethod]
        public void ShouldSelect_AllVehicleGroups()
        {
            VehicleGroup g1 = new VehicleGroup(0, "name", 12.50f, 25.73f, 13.99f, 200, 11f, 65f);
            controller.InsertNew(g1);
            VehicleGroup g2 = new VehicleGroup(0, "emon", 5.12f, 37.52f, 99.31f, 2, 4.5f, 50f);
            controller.InsertNew(g2);
            VehicleGroup g3 = new VehicleGroup(0, "meno", 5.21f, 35.72f, 93.91f, 20, 5f, 11f);
            controller.InsertNew(g3);

            List<VehicleGroup> groupedVehicleGroups = controller.SelectAll();

            groupedVehicleGroups.Should().HaveCount(3);
            groupedVehicleGroups[0].Name.Should().Be("name");
            groupedVehicleGroups[1].Name.Should().Be("emon");
            groupedVehicleGroups[2].Name.Should().Be("meno");
        }

        [TestMethod]
        public void ShouldReturnTrue_WhenVehicleGroupExists()
        {
            VehicleGroup vehicleGroup = new VehicleGroup(0, "name", 12.50f, 25.73f, 13.99f, 200, 5f, 30f);
            controller.InsertNew(vehicleGroup);

            bool exists = controller.Exists(vehicleGroup.Id);

            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldReturnFalse_WhenVehicleGroupDoesNotExist()
        {
            VehicleGroup vehicleGroup = new VehicleGroup(0, "name", 12.50f, 25.73f, 13.99f, 200, 15f, 11.2f);

            bool exists = controller.Exists(vehicleGroup.Id);

            exists.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldNotInsert_VehicleGroup_WhenNameAlreadyExists()
        {
            VehicleGroup newVehicleGroup = new VehicleGroup(0, "name", 12.50f, 25.73f, 13.99f, 200, 15f, 11.2f);
            controller.InsertNew(newVehicleGroup);
            VehicleGroup identicalVehicleGroup = new VehicleGroup(0, "name", 12.50f, 25.73f, 13.99f, 200, 15f, 11.2f);
            
            string response = controller.InsertNew(identicalVehicleGroup);

            response.Should().Be("The vehicle group name must be unique\n");
        }

        [TestMethod]
        public void ShouldNotUpdate_VehicleGroup_WhenNameAlreadyExists()
        {
            VehicleGroup vehicleGroupToEdit = new VehicleGroup(0, "name", 12.50f, 25.73f, 13.99f, 200, 11f, 50.5f);
            controller.InsertNew(vehicleGroupToEdit);
            VehicleGroup existingVehicleGroup = new VehicleGroup(0, "emon", 5.12f, 37.52f, 99.31f, 2, 5f, 90f);
            controller.InsertNew(existingVehicleGroup);

            VehicleGroup conflictingVehicleGroup = new VehicleGroup(0, "emon", 5.21f, 35.72f, 93.91f, 20, 13f, 85.3f);
            string response =  controller.Edit(vehicleGroupToEdit.Id, conflictingVehicleGroup);

            response.Should().Be("The vehicle group name must be unique\n");
        }
    }
}
