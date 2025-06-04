using FluentAssertions;
using CarRental.Controllers.PartnerModule;
using CarRental.Controllers.Shared;
using CarRental.Domain.PartnerModule;
using CarRental.Controllers.Tests.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Controllers.Tests.PartnerModule
{
    [TestClass]
    [TestCategory("Controllers")]
    public class PartnerControllerTest
    {
        PartnerController controller = null;
        Partner partner;
        public PartnerControllerTest()
        {
            controller = new PartnerController();
            ResetDatabase.ResetAllTables();
        }

        [TestMethod]
        public void ShouldInsertAPartner()
        {
            //arrange
            partner = new Partner(0, "Test Name");

            //action
            controller.InsertNew(partner);

            //assert
            var foundPartner = controller.SelectById(partner.Id);
            foundPartner.Should().Be(partner);
        }

        [TestMethod]
        public void ShouldSelectTwoPartners()
        {
            //arrange
            partner = new Partner(0, "Test Name");

            //action
            controller.InsertNew(partner);
            controller.InsertNew(partner);

            //assert
            List<Partner> foundPartners = controller.SelectAll();
            foundPartners.Count.Should().Be(2);
        }

        [TestMethod]
        public void ShouldEditAPartner()
        {
            //arrange
            partner = new Partner(0, "Test Name");
            Partner editedPartner = new Partner(0, "Changed Name");

            //action
            controller.InsertNew(partner);
            controller.Edit(partner.Id, editedPartner);

            //assert
            Partner foundPartner = controller.SelectById(partner.Id);
            foundPartner.Should().Be(editedPartner);
        }

        [TestMethod]
        public void ShouldDeleteAPartner()
        {
            //arrange
            partner = new Partner(0, "Test Name");

            //action
            controller.InsertNew(partner);
            List<Partner> insertedPartners = controller.SelectAll();
            controller.Delete(partner.Id);

            //assert
            List<Partner> partnersAfterDeletion = controller.SelectAll();
            partnersAfterDeletion.Count.Should().NotBe(insertedPartners.Count);
        }
    }
}
