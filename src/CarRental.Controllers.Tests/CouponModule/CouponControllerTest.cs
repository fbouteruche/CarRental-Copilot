using FluentAssertions;
using CarRental.Controllers.CouponModule;
using CarRental.Controllers.PartnerModule;
using CarRental.Controllers.Shared;
using CarRental.Domain.CouponModule;
using CarRental.Domain.PartnerModule;
using CarRental.Controllers.Tests.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Controllers.Tests.CouponModule
{
    [TestClass]
    [TestCategory("Controllers")]
    public class CouponControllerTest
    {
        CouponController controller = null;
        PartnerController partnerController = null;
        Coupon coupon;
        Partner partner;
        public CouponControllerTest()
        {
            controller = new CouponController();
            partnerController = new PartnerController();
            ResetDatabase.ResetAllTables();            
        }

        [TestMethod]
        public void ShouldInsertAPartner()
        {
            // arrange
            InsertPartner();
            coupon = new Coupon(0, "Coupon001", "COUPONCODE", 500, 2000, true, DateTime.Today.AddDays(30), partner);

            // act
            controller.InsertNew(coupon);

            // assert
            Coupon foundCoupon = controller.SelectById(coupon.Id);
            foundCoupon.Should().Be(coupon);
        }        

        [TestMethod]
        public void ShouldSelectTwoPartners()
        {
            // arrange
            InsertPartner();
            coupon = new Coupon(0, "Coupon001", "COUPONCODE", 500, 2000, true, DateTime.Today.AddDays(30), partner);

            // act
            controller.InsertNew(coupon);
            controller.InsertNew(coupon);

            // assert
            List<Coupon> foundPartners = controller.SelectAll();
            foundPartners.Count.Should().Be(2);
        }

        [TestMethod]
        public void ShouldEditAPartner()
        {
            // arrange
            InsertPartner();
            coupon = new Coupon(0, "Coupon001", "COUPONCODE", 500, 2000, true, DateTime.Today.AddDays(30), partner);
            Coupon editedCoupon = new Coupon(0, "CouponEdited", "EDITED", 50, 2000, false, DateTime.Today.AddDays(10), partner);

            // act
            controller.InsertNew(coupon);
            controller.Edit(coupon.Id, editedCoupon);

            // assert
            Coupon foundPartner = controller.SelectById(coupon.Id);
            foundPartner.Should().Be(editedCoupon);
        }

        [TestMethod]
        public void ShouldDeleteAPartner()
        {
            // arrange
            InsertPartner();
            coupon = new Coupon(0, "Coupon001", "COUPONCODE", 500, 2000, true, DateTime.Today.AddDays(30), partner);

            // act
            controller.InsertNew(coupon);
            List<Coupon> insertedCoupons = controller.SelectAll();
            controller.Delete(coupon.Id);

            // assert
            List<Coupon> dbAfterDelete = controller.SelectAll();
            dbAfterDelete.Count.Should().NotBe(insertedCoupons.Count);
        }

        private void InsertPartner()
        {
            partner = new Partner(0, "Test Name");
            partnerController.InsertNew(partner);
        }
    }
}
