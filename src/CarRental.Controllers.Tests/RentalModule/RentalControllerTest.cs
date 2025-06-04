using FluentAssertions;
using CarRental.Controllers.CustomersModule;
using CarRental.Controllers.CouponModule;
using CarRental.Controllers.EmployeeModule;
using CarRental.Controllers.VehicleGroupModule;
using CarRental.Controllers.RentalModule;
using CarRental.Controllers.ServiceModule;
using CarRental.Controllers.Shared;
using CarRental.Controllers.VehicleModule;
using CarRental.Domain.CustomerModule;
using CarRental.Domain.EmployeeModule;
using CarRental.Domain.VehicleGroupModule;
using CarRental.Domain.RentalModule;
using CarRental.Domain.VehicleModule;
using CarRental.Controllers.Tests.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CarRental.Controllers.Tests.RentalModule
{
    [TestClass]
    [TestCategory("Controllers")]
    public class RentalControllerTest
    {
        RentalController controller = null;
        VehicleGroupController vehicleGroupController = null;
        VehicleController vehicleController = null;
        EmployeeController employeeController = null;
        CustomerController customerController = null;
        ServiceController serviceController = null;
        CouponController couponController = null;
        VehicleGroup vehicleGroup;
        Vehicle vehicle;
        Employee employee;
        Customer contractingCustomer;
        Customer driverCustomer;
        Rental rental;

        public RentalControllerTest()
        {
            vehicleGroupController = new VehicleGroupController();
            vehicleController = new VehicleController();
            employeeController = new EmployeeController();
            customerController = new CustomerController();
            serviceController = new ServiceController();
            couponController = new CouponController();
            controller = new RentalController(vehicleController, employeeController, customerController, serviceController, couponController);
            ResetDatabase.ResetAllTables();
        }

        [TestMethod]
        public void ShouldInsertARental()
        {
            vehicleGroup = new VehicleGroup(0, "name", 12.3f, 15.5f, 20.5f, 30, 16.3f, 45.2f);
            vehicleGroupController.InsertNew(vehicleGroup);
            vehicle = new Vehicle(0, "Ecosport", vehicleGroup, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'L', true, true, true, true, null);
            vehicleController.InsertNew(vehicle);
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 001, "user access", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            employeeController.InsertNew(employee);
            contractingCustomer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            customerController.InsertNew(contractingCustomer);
            driverCustomer = new Customer(0, "Nardolindo", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            customerController.InsertNew(driverCustomer);

            rental = new Rental(0, vehicle, employee, contractingCustomer, driverCustomer, null, DateTime.Today, DateTime.Today.AddDays(5f), DateTime.Today.AddDays(5f), "UnlimitedKm", "None", 0, 0, false, null);
            controller.InsertNew(rental);

            var foundRental = controller.SelectById(rental.Id);
            foundRental.Should().Be(rental);
        }

        [TestMethod]
        public void ShouldSelectTwoRentals()
        {
            vehicleGroup = new VehicleGroup(0, "name", 12.3f, 15.5f, 20.5f, 30, 16.3f, 45.2f);
            vehicleGroupController.InsertNew(vehicleGroup);
            vehicle = new Vehicle(0, "Ecosport", vehicleGroup, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'L', true, true, true, true, null);
            vehicleController.InsertNew(vehicle);
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 001, "user access", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            employeeController.InsertNew(employee);
            contractingCustomer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            customerController.InsertNew(contractingCustomer);
            driverCustomer = new Customer(0, "Nardolindo", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            customerController.InsertNew(driverCustomer);

            rental = new Rental(0, vehicle, employee, contractingCustomer, driverCustomer, null, DateTime.Today, DateTime.Today.AddDays(5f), "UnlimitedKm", "None", null);
            controller.InsertNew(rental);
            Rental anotherRental = new Rental(0, vehicle, employee, contractingCustomer, driverCustomer, null, DateTime.Today.AddDays(-10), DateTime.Today.AddDays(15), "DailyPlan", "CustomerInsurance", null);
            controller.InsertNew(anotherRental);

            List<Rental> foundRentals = controller.SelectAll();
            foundRentals.Count.Should().Be(2);
        }

        [TestMethod]
        public void ShouldEditARental()
        {
            vehicleGroup = new VehicleGroup(0, "name", 12.3f, 15.5f, 20.5f, 30, 16.3f, 45.2f);
            vehicleGroupController.InsertNew(vehicleGroup);
            vehicle = new Vehicle(0, "Ecosport", vehicleGroup, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'L', true, true, true, true, null);
            vehicleController.InsertNew(vehicle);
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 001, "user access", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            employeeController.InsertNew(employee);
            contractingCustomer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            customerController.InsertNew(contractingCustomer);
            driverCustomer = new Customer(0, "Nardolindo", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            customerController.InsertNew(driverCustomer);

            rental = new Rental(0, vehicle, employee, contractingCustomer, driverCustomer, null, DateTime.Today, DateTime.Today.AddDays(5f), "UnlimitedKm", "None", null);
            controller.InsertNew(rental);
            Rental anotherRental = new Rental(0, vehicle, employee, contractingCustomer, driverCustomer, null, DateTime.Today, DateTime.Today.AddDays(5f), DateTime.Today.AddDays(5f), "UnlimitedKm", "None", 0, 0, false, null);
            controller.Edit(rental.Id, anotherRental);

            var foundRental = controller.SelectById(rental.Id);
            foundRental.Should().Be(anotherRental);
        }

        [TestMethod]
        public void ShouldDeleteAVehicle()
        {
            vehicleGroup = new VehicleGroup(0, "name", 12.3f, 15.5f, 20.5f, 30, 16.3f, 45.2f);
            vehicleGroupController.InsertNew(vehicleGroup);
            vehicle = new Vehicle(0, "Ecosport", vehicleGroup, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'L', true, true, true, true, null);
            vehicleController.InsertNew(vehicle);
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 001, "user access", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            employeeController.InsertNew(employee);
            contractingCustomer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            customerController.InsertNew(contractingCustomer);
            driverCustomer = new Customer(0, "Nardolindo", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            customerController.InsertNew(driverCustomer);

            rental = new Rental(0, vehicle, employee, contractingCustomer, driverCustomer, null, DateTime.Today, DateTime.Today.AddDays(5f), "UnlimitedKm", "None", null);
            controller.InsertNew(rental);
            controller.Delete(rental.Id);

            List<Rental> foundRentals = controller.SelectAll();
            foundRentals.Count.Should().Be(0);
        }

    }
}
