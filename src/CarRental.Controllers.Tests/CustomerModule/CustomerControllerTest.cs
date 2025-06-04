using FluentAssertions;
using CarRental.Controllers.CustomersModule;
using CarRental.Controllers.Shared;
using CarRental.Domain.CustomerModule;
using CarRental.Controllers.Tests.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CarRental.Controllers.Tests.CustomerModule
{
    [TestClass]
    [TestCategory("Controllers")]
    public class CustomerControllerTest
    {
        CustomerController controller = null;
        Customer customer;
        public CustomerControllerTest()
        {
            controller = new CustomerController();
            ResetDatabase.ResetAllTables();
        }
        [TestMethod]
        public void ShouldInsert_NewCustomer()
        {
            // arrange
            Customer customer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);

            // act
            controller.InsertNew(customer);

            // assert
            Customer foundCustomer = controller.SelectById(customer.Id);
            foundCustomer.Should().Be(customer);
        }

        [TestMethod]
        public void ShouldUpdate_Customer()
        {
            // arrange
            customer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            controller.InsertNew(customer);

            Customer editedCustomer = new Customer(2, "Arnaldo", "888.777.666.55", "Laguna Street", "97777-6666", "arnaldo@test.com", "98765432103", new DateTime(2020, 11, 11), true);

            // act
            controller.Edit(customer.Id, editedCustomer);

            // assert
            Customer updatedCustomer = controller.SelectById(customer.Id);
            updatedCustomer.Should().Be(customer);
        }

        [TestMethod]
        public void ShouldDelete_Customer()
        {
            // arrange
            Customer customer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            controller.InsertNew(customer);

            // act
            controller.Delete(customer.Id);

            // assert
            Customer foundCustomer = controller.SelectById(customer.Id);
            foundCustomer.Should().BeNull();
        }

        [TestMethod]
        public void ShouldSelect_AllCustomers()
        {
            Customer c1 = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);

            controller.InsertNew(c1);
            controller.InsertNew(c1);

            var customers = controller.SelectAll();

            customers.Should().HaveCount(2);
            customers[0].Name.Should().Be("Test Name");
            customers[1].Name.Should().Be("Test Name");
            ResetDatabase.ResetAllTables();
        }

        [TestMethod]
        public void ShouldSelect_Customer_ById()
        {
            // arrange
            Customer customer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);
            controller.InsertNew(customer);

            // act
            Customer foundCustomer = controller.SelectById(customer.Id);

            // assert
            foundCustomer.Should().NotBeNull();
        }        
    }
}
