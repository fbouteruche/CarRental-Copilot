using FluentAssertions;
using CarRental.Domain.PartnerModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Tests.PartnerModule
{
    [TestClass]
    [TestCategory("Domain")]
    public class PartnerTest
    {
        [TestMethod]
        public void ShouldCreatePartner_Correctly()
        {
            // arrange
            Partner partner = new Partner(0, "NDD");

            // act
            var validationResult = partner.Validate();

            // assert
            validationResult.Should().Be("VALID");
        }

        [TestMethod]
        public void ShouldReturnError_InvalidName()
        {
            // arrange
            Partner partner = new Partner(0, "");

            // act
            var validationResult = partner.Validate();

            // assert
            validationResult.Should().Be("The Name field is required");
        }
    }
}
