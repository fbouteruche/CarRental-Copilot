using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CarRental.Domain.CustomerModule;

namespace CarRental.Domain.Tests.CustomerModule
{
    [TestClass]
    [TestCategory("Domain")]
    public class CustomerTest
    {
        Customer customer;

        [TestMethod]
        public void ShouldCreateValidCustomer_CompleteIndividual()
        {
            customer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "11-1111111", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);

            string validationResult = customer.Validate();

            Assert.AreEqual("VALID", validationResult);
        }

        [TestMethod]
        public void ShouldCreateValidCustomer_CompleteCompany()
        {
            customer = new Customer(0, "Test Name", "11-1111111", "Customer Address", "11-1111111", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), false);

            string validationResult = customer.Validate();

            Assert.AreEqual("VALID", validationResult);
        }

        [TestMethod]
        public void ShouldCreateValidCustomer_WithoutPhone()
        {
            customer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "", "test@email.com", "978545956-90", new DateTime(2030, 01, 01), true);

            string validationResult = customer.Validate();

            Assert.AreEqual("VALID", validationResult);
        }

        [TestMethod]
        public void ShouldReturnError_WithoutEmailAndPhone()
        {
            customer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "", "", "978545956-90", new DateTime(2030, 01, 01), true);

            string validationResult = customer.Validate();

            Assert.AreEqual("The email is mandatory, incorrect, and must be valid\n", validationResult);
        }

        [TestMethod]
        public void ShouldReturnError_InvalidIndividual()
        {
            customer = new Customer(0, "", "", "", "", "", "", new DateTime(2000, 01, 01), true);

            string validationResult = customer.Validate();

            Assert.AreEqual("Invalid Driver's License\nDriver's License expired\nThe name cannot be null\nThe address cannot be null\nThe email is mandatory, incorrect, and must be valid\nThe SSN is not valid\n", validationResult);
        }

        [TestMethod]
        public void ShouldReturnError_IndividualWithoutDriverLicense()
        {
            customer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "", new DateTime(2030, 01, 01), true);

            string validationResult = customer.Validate();

            Assert.AreEqual("Invalid Driver's License\n", validationResult);
        }

        [TestMethod]
        public void ShouldReturnError_IndividualWithInvalidDriverLicenseDate()
        {
            customer = new Customer(0, "Test Name", "954.746.736-04", "Customer Address", "4932518000", "test@email.com", "978545956-90", new DateTime(2000, 01, 01), true);

            string validationResult = customer.Validate();

            Assert.AreEqual("Driver's License expired\n", validationResult);
        }

        [TestMethod]
        public void ShouldReturnError_InvalidCompany()
        {
            customer = new Customer(0, "", "", "", "", "", "", null, false);

            string validationResult = customer.Validate();

            Assert.AreEqual("The name cannot be null\nThe address cannot be null\nThe email is mandatory, incorrect, and must be valid\nThe EIN is not valid\n", validationResult);
        }

        #region Tests for properties inherited from Person
        [TestMethod]
        public void ShouldCreatePerson_Complete()
        {
            customer = new Customer(1, "name", "11111111111", "address", "999999999", "email@g.com", "978545956-90", new DateTime(2030, 01, 01), true);

            string validationResult = customer.Validate();

            Assert.AreEqual("VALID", validationResult);
        }

        [TestMethod]
        public void ShouldCreatePerson_WithoutPhone()
        {
            customer = new Customer(1, "name", "11111111111", "address", "", "email@g.com", "978545956-90", new DateTime(2030, 01, 01), true);

            string validationResult = customer.Validate();

            Assert.AreEqual("VALID", validationResult);
        }

        [TestMethod]
        public void ShouldReturnErrorPerson_CompletelyInvalidPerson()
        {
            customer = new Customer(1, "", "", "", "", "", "978545956-90", new DateTime(2030, 01, 01), true);

            string validationResult = customer.Validate();

            Assert.AreEqual("The name cannot be null\nThe address cannot be null\nThe email is mandatory, incorrect, and must be valid\nThe SSN is not valid\n", validationResult);
        }

        [TestMethod]
        public void ShouldReturnErrorPerson_InvalidPersonWithEmailWithoutAtSymbolAndOneDigitPhone()
        {
            customer = new Customer(1, "", "", "", "1", "a", "978545956-90", new DateTime(2030, 01, 01), true);

            string validationResult = customer.Validate();

            Assert.AreEqual("The name cannot be null\nThe address cannot be null\nThe email is mandatory, incorrect, and must be valid\nThe SSN is not valid\n", validationResult);
        }

        [TestMethod]
        public void ShouldReturnErrorPerson_ValidPersonWithEmailWithoutAtSymbolAndOneDigitPhone()
        {
            customer = new Customer(1, "name", "11111111111", "address", "9", "email", "978545956-90", new DateTime(2030, 01, 01), true);

            string validationResult = customer.Validate();

            Assert.AreEqual("The email is mandatory, incorrect, and must be valid\n", validationResult);
        }

        [TestMethod]
        public void ShouldReturnValidPerson_ValidPersonWithOneDigitPhone()
        {
            customer = new Customer(1, "name", "11111111111", "address", "9", "email@email.com", "978545956-90", new DateTime(2030, 01, 01), true);

            string validationResult = customer.Validate();

            Assert.AreEqual("VALID", validationResult);
        }
        #endregion
    }
}
