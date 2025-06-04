using CarRental.Domain.EmployeeModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CarRental.Domain.Tests.EmployeeModule
{
    [TestClass]
    [TestCategory("Domain")]
    public class EmployeeTest
    {
        Employee employee;

        [TestMethod]
        public void ShouldCreateEmployee_Complete()
        {
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 001, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);

            string result = employee.Validate();

            Assert.AreEqual("VALID", result);
        }

        [TestMethod]
        public void ShouldReturnError_EmptyAccessUsername()
        {
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 001, "", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);

            string result = employee.Validate();

            Assert.AreEqual("The access username cannot be empty\n", result);
        }

        [TestMethod]
        public void ShouldReturnError_ZeroRegistrationNumber()
        {
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 000, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);

            string result = employee.Validate();

            Assert.AreEqual("Invalid registration number\n", result);
        }

        [TestMethod]
        public void ShouldReturnError_ZeroSalary()
        {
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 001, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 0f, true);

            string result = employee.Validate();

            Assert.AreEqual("The salary must be greater than $0.00\n", result);
        }

        [TestMethod]
        public void ShouldReturnError_EmptyJobTitle()
        {
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 001, "access user", "12345", new DateTime(2021, 01, 01), "", 1000f, true);

            string result = employee.Validate();

            Assert.AreEqual("The employee must have a job title\n", result);
        }

        [TestMethod]
        public void ShouldReturnError_InvalidHiringDate()
        {
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 001, "access user", "12345", new DateTime(2030, 01, 01), "Salesperson", 1000f, true);

            string result = employee.Validate();

            Assert.AreEqual("Invalid hiring date\n", result);
        }

        [TestMethod]
        public void ShouldReturnError_EmployeeCompletelyInvalid()
        {
            employee = new Employee(0, "Test Name", "954.746.736-04", "Employee Address", "4932518000", "test@email.com", 0, "", "12345", new DateTime(2030, 01, 01), "", 0f, true);

            string result = employee.Validate();

            Assert.AreEqual("The access username cannot be empty\nInvalid registration number\nThe salary must be greater than $0.00\nThe employee must have a job title\nInvalid hiring date\n", result);
        }

        #region Tests for properties inherited from Person
        [TestMethod]
        public void ShouldCreatePerson_Complete()
        {
            employee = new Employee(1, "name", "11111111111", "address", "999999999", "email@g.com", 001, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);

            string result = employee.Validate();

            Assert.AreEqual("VALID", result);
        }

        [TestMethod]
        public void ShouldCreatePerson_WithoutPhone()
        {
            employee = new Employee(1, "name", "11111111111", "address", "", "email@g.com", 001, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);

            string result = employee.Validate();

            Assert.AreEqual("VALID", result);
        }

        [TestMethod]
        public void ShouldReturnErrorPerson_CompletelyInvalidPerson()
        {
            employee = new Employee(1, "", "", "", "", "", 001, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);

            string result = employee.Validate();

            Assert.AreEqual("The name cannot be null\nThe address cannot be null\nThe email is mandatory, incorrect, and must be valid\nThe SSN is not valid\n", result);
        }

        [TestMethod]
        public void ShouldReturnErrorPerson_InvalidPersonWithEmailWithoutAtSymbolAndOneDigitPhone()
        {
            employee = new Employee(1, "", "", "", "1", "a", 001, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);

            string result = employee.Validate();

            Assert.AreEqual("The name cannot be null\nThe address cannot be null\nThe email is mandatory, incorrect, and must be valid\nThe SSN is not valid\n", result);
        }

        [TestMethod]
        public void ShouldReturnErrorPerson_ValidPersonWithEmailWithoutAtSymbolAndOneDigitPhone()
        {
            employee = new Employee(1, "name", "11111111111", "address", "9", "email", 001, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);

            string result = employee.Validate();

            Assert.AreEqual("The email is mandatory, incorrect, and must be valid\n", result);
        }

        [TestMethod]
        public void ShouldReturnValidPerson_ValidPersonWithOneDigitPhone()
        {
            employee = new Employee(1, "name", "11111111111", "address", "9", "email@email.com", 001, "access user", "12345", new DateTime(2021, 01, 01), "Salesperson", 1000f, true);

            string result = employee.Validate();

            Assert.AreEqual("VALID", result);
        }
        #endregion
    }
}
