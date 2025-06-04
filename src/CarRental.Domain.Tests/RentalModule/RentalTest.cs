using FluentAssertions;
using CarRental.Controllers.Shared;
using CarRental.Domain.CustomerModule;
using CarRental.Domain.EmployeeModule;
using CarRental.Domain.RentalModule;
using CarRental.Domain.VehicleModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CarRental.Domain.Tests.RentalModule
{
    [TestClass]
    [TestCategory("Domain")]
    public class RentalTest
    {
        Vehicle vehicle;
        Employee employee;
        Customer contractingCustomer;
        Customer driverCustomer;
        Rental rental;

        [TestMethod]
        public void ShouldCreateRental_WithContractingPFAndDriverPF()
        {
            vehicle = new Vehicle(0, "Ecosport", null, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'G', true, true, true, true, null);
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "user access", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            contractingCustomer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            driverCustomer = new Customer(1, "Arnaldo", "888.777.666.55", "Laguna Street", "97777-6666", "arnaldo@test.com", "98765432103", new DateTime(2020, 11, 11), true);
            rental = new Rental(0, vehicle, employee, contractingCustomer, driverCustomer, null, DateTime.Today, DateTime.Today.AddDays(5f), "UnlimitedKm", "None", null);

            string result = rental.Validate();

            result.Should().Be("VALID");
        }

        [TestMethod]
        public void ShouldCreateRental_WithContractingPFAndNoDriver()
        {
            vehicle = new Vehicle(0, "Ecosport", null, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'G', true, true, true, true, null);
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "user access", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            contractingCustomer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            rental = new Rental(0, vehicle, employee, contractingCustomer, null, null, DateTime.Today, DateTime.Today.AddDays(5f), "UnlimitedKm", "None", null);

            string result = rental.Validate();

            result.Should().Be("VALID");
        }

        [TestMethod]
        public void ShouldCreateRental_WithContractingPJAndDriverPF()
        {
            vehicle = new Vehicle(0, "Ecosport", null, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'G', true, true, true, true, null);
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "user access", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            contractingCustomer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), false);
            driverCustomer = new Customer(1, "Arnaldo", "888.777.666.55", "Laguna Street", "97777-6666", "arnaldo@test.com", "98765432103", new DateTime(2020, 11, 11), true);
            rental = new Rental(0, vehicle, employee, contractingCustomer, driverCustomer, null, DateTime.Today, DateTime.Today.AddDays(5f), "UnlimitedKm", "None", null);

            string result = rental.Validate();

            result.Should().Be("VALID");
        }

        [TestMethod]
        public void ShouldReturnError_WhenEmployeeIsNull()
        {
            vehicle = new Vehicle(0, "Ecosport", null, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'G', true, true, true, true, null);
            contractingCustomer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            driverCustomer = new Customer(1, "Arnaldo", "888.777.666.55", "Laguna Street", "97777-6666", "arnaldo@test.com", "98765432103", new DateTime(2020, 11, 11), true);
            rental = new Rental(0, vehicle, null, contractingCustomer, driverCustomer, null, DateTime.Today, DateTime.Today.AddDays(5f), "UnlimitedKm", "None", null);

            string result = rental.Validate();

            result.Should().Be("The renting employee cannot be null\n");
        }

        [TestMethod]
        public void ShouldReturnError_WhenContractingCustomerIsNull()
        {
            vehicle = new Vehicle(0, "Ecosport", null, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'G', true, true, true, true, null);
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "user access", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            driverCustomer = new Customer(1, "Arnaldo", "888.777.666.55", "Laguna Street", "97777-6666", "arnaldo@test.com", "98765432103", new DateTime(2020, 11, 11), true);
            rental = new Rental(0, vehicle, employee, null, driverCustomer, null, DateTime.Today, DateTime.Today.AddDays(5f), DateTime.Today.AddDays(5f), "UnlimitedKm", "None", 0, 0, true, null);

            string result = rental.Validate();

            result.Should().Be("The contracting customer cannot be null\n");
        }

        [TestMethod]
        public void ShouldReturnError_WhenContractingPJWithoutDriver()
        {
            vehicle = new Vehicle(0, "Ecosport", null, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'G', true, true, true, true, null);
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "user access", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            contractingCustomer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), false);
            rental = new Rental(0, vehicle, employee, contractingCustomer, null, null, DateTime.Today, DateTime.Today.AddDays(5f), "UnlimitedKm", "None", null);

            string result = rental.Validate();

            result.Should().Be("The driver cannot be null when the contracting customer is a legal entity\n");
        }

        [TestMethod]
        public void ShouldReturnError_WhenContractingPJAndDriverPJ()
        {
            vehicle = new Vehicle(0, "Ecosport", null, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'G', true, true, true, true, null);
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "user access", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            contractingCustomer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), false);
            driverCustomer = new Customer(1, "Arnaldo", "888.777.666.55", "Laguna Street", "97777-6666", "arnaldo@test.com", "98765432103", new DateTime(2020, 11, 11), false);
            rental = new Rental(0, vehicle, employee, contractingCustomer, driverCustomer, null, DateTime.Today, DateTime.Today.AddDays(5f), "UnlimitedKm", "None", null);

            string result = rental.Validate();

            result.Should().Be("The driver cannot be a legal entity.\n");
        }

        [TestMethod]
        public void ShouldReturnError_WhenPlanTypeIsInvalid()
        {
            vehicle = new Vehicle(0, "Ecosport", null, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'G', true, true, true, true, null);
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "user access", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            contractingCustomer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            driverCustomer = new Customer(1, "Arnaldo", "888.777.666.55", "Laguna Street", "97777-6666", "arnaldo@test.com", "98765432103", new DateTime(2020, 11, 11), true);
            rental = new Rental(0, vehicle, employee, contractingCustomer, driverCustomer, null, DateTime.Today, DateTime.Today.AddDays(5f), "InvalidPlan", "None", null);

            string result = rental.Validate();

            result.Should().Be("The plan type is invalid.\n");
        }

        [TestMethod]
        public void ShouldReturnError_WhenInsuranceTypeIsInvalid()
        {
            vehicle = new Vehicle(0, "Ecosport", null, "LPT-4652", "4DF56F78E8WE9WED", "Ford", "Silver", "Regular Gasoline", 60.5, 2018, 30000, 4, 5, 'G', true, true, true, true, null);
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "user access", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            contractingCustomer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            driverCustomer = new Customer(1, "Arnaldo", "888.777.666.55", "Laguna Street", "97777-6666", "arnaldo@test.com", "98765432103", new DateTime(2020, 11, 11), true);
            rental = new Rental(0, vehicle, employee, contractingCustomer, driverCustomer, null, DateTime.Today, DateTime.Today.AddDays(5f), "UnlimitedKm", "InvalidInsurance", null);

            string result = rental.Validate();

            result.Should().Be("The insurance type is invalid.\n");
        }
    }
}
