using FluentAssertions;
using CarRental.Controllers.ServiceModule;
using CarRental.Controllers.Shared;
using CarRental.Domain.ServiceModule;
using CarRental.Controllers.Tests.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CarRental.Controllers.Tests.ServiceModule
{
    [TestClass]
    [TestCategory("Controllers")]
    public class ServiceControllerTests
    {
        ServiceController controller = null;
        Service newService;
        public ServiceControllerTests()
        {
            controller = new ServiceController();
            ResetDatabase.ResetAllTables();
        }
        [TestMethod]
        public void ShouldInsertAService()
        {
            // arrange
            newService = new Service(0, "name", true, 100);

            // act
            controller.InsertNew(newService);

            // assert
            Service foundService = controller.SelectById(newService.Id);
            foundService.Should().Be(newService);
        }
        [TestMethod]
        public void ShouldSelectTwoServices()
        {
            // arrange
            newService = new Service(0, "name", true, 100);

            // act
            controller.InsertNew(newService);
            controller.InsertNew(newService);

            // assert
            List<Service> foundServices = controller.SelectAll();
            foundServices.Count.Should().Be(2);
        }

        [TestMethod]
        public void ShouldEditAService()
        {
            // arrange
            newService = new Service(0, "name", true, 100);
            Service editedService = new Service(0, "Car Wash", false, 80);
            // act
            controller.InsertNew(newService);
            controller.Edit(newService.Id, editedService);

            // assert
            Service foundService = controller.SelectById(newService.Id);
            foundService.Should().Be(editedService);
        }

        [TestMethod]
        public void ShouldDeleteAService()
        {
            // arrange
            newService = new Service(0, "name", true, 100);

            // act
            controller.InsertNew(newService);
            controller.Delete(newService.Id);

            // assert
            List<Service> foundServices = controller.SelectAll();
            foundServices.Count.Should().Be(0);
        }
    }
}
