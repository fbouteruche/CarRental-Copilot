using CarRental.Controllers.EmployeeModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Domain.EmployeeModule;
using System.Collections.Generic;
using System;
using CarRental.Controllers.Tests.Shared;

namespace CarRental.Controllers.Tests.EmployeeModule
{
    [TestClass]
    [TestCategory("Controllers")]
    public class EmployeeControllerTest
    {
        Employee employee;
        Employee employee2;
        EmployeeController controller; 

        public EmployeeControllerTest()
        {
            controller = new EmployeeController();
            ResetDatabase.ResetAllTables(); // Consider renaming ResetarBanco to ResetDatabase in the shared test utilities
        }

        [TestMethod]
        public void ShouldInsertEmployeeInDatabase()
        {
            // arrange
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);

            // act
            controller.InsertNew(employee);

            // assert
            Assert.AreEqual(employee, controller.SelectById(employee.Id));
        }

        [TestMethod]
        public void ShouldDeleteEmployeeFromDatabase()
        {
            // arrange
            employee = new Employee(0, "Test Name Removed", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            
            // act
            controller.InsertNew(employee);
            controller.Delete(employee.Id);
            Employee foundEmployee = controller.SelectById(employee.Id);

            // assert
            Assert.IsNull(foundEmployee);
        }

        [TestMethod]
        public void ShouldEditEmployeeInDatabase()
        {
            // arrange
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            Employee editedEmployee = new Employee(0, "Test Name 2", "954.746.736-04", "Employee2 Address", "4932518000", "test2@email.com", 1, "access user2", "12345", new DateTime(2021, 01, 01), "Salesperson2", 1000f, true);

            // act
            controller.InsertNew(employee);
            controller.Edit(employee.Id, editedEmployee);

            // assert
            Assert.AreEqual(editedEmployee, controller.SelectById(employee.Id));
        }

        [TestMethod]
        public void ShouldSelectAllEmployeesFromDatabase()
        {
            // arrange
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);
            employee2 = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 1, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);

            // act
            controller.InsertNew(employee);
            controller.InsertNew(employee2);
            var list = controller.SelectAll();

            // assert
            Assert.IsNotNull(list);
        }
    }
}
