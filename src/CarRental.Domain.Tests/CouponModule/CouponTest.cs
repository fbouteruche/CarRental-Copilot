using FluentAssertions;
using CarRental.Domain.CouponModule;
using CarRental.Domain.PartnerModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CarRental.Domain.Tests.CouponModule
{
    [TestClass]
    [TestCategory("Domain")]
    public class CouponTest
    {
        Partner partner = new Partner(1, "Test Partner");

        [TestMethod]
        public void ShouldCreateCoupon_Complete_Percentage()
        {
            //arrange
            Coupon coupon = new Coupon(0, "Coupon Name", "NDD10TECH", 10, 2000, false, DateTime.Now.AddDays(10), partner);

            //action
            string validationResult = coupon.Validate();

            //assert
            validationResult.Should().Be("VALID");
        }

        [TestMethod]
        public void ShouldCreateCoupon_Complete_Fixed()
        {
            //arrange
            Coupon coupon = new Coupon(0, "Coupon Name", "NDD10TECH", 10, 2000, true, DateTime.Now.AddDays(10), partner);

            //action
            string validationResult = coupon.Validate();

            //assert
            validationResult.Should().Be("VALID");
        }

        [TestMethod]
        public void ShouldCreateCoupon_CompletelyInvalid_Percentage()
        {
            //arrange
            Coupon coupon = new Coupon(0, "", "", 101, 0, false, DateTime.Now.AddDays(10), null);

            //action
            string validationResult = coupon.Validate();

            //assert
            validationResult.Should().Be("The name field is required\nThe code field is required\nThe discount percentage cannot be greater than 100%\nA partner is required\n");
        }

        [TestMethod]
        public void ShouldCreateCoupon_CompletelyInvalid_Fixed()
        {
            //arrange
            Coupon coupon = new Coupon(0, "", "", 0, 0, true, DateTime.Now.AddDays(10), null);

            //action
            string validationResult = coupon.Validate();

            //assert
            validationResult.Should().Be("The name field is required\nThe code field is required\nThe value cannot be negative or 0 (zero)\nA partner is required\n");
        }

        [TestMethod]
        public void ShouldReturnError_ValueZero_Fixed()
        {
            //arrange
            Coupon coupon = new Coupon(0, "Coupon Name", "NDD10TECH", 0, 2000, true, DateTime.Now.AddDays(10), partner);

            //action
            string validationResult = coupon.Validate();

            //assert
            validationResult.Should().Be("The value cannot be negative or 0 (zero)\n");
        }

        [TestMethod]
        public void ShouldReturnError_PercentageGreaterThanHundred_Percentage()
        {
            //arrange
            Coupon coupon = new Coupon(0, "Coupon Name", "NDD10TECH", 101, 2000, false, DateTime.Now.AddDays(10), partner);

            //action
            string validationResult = coupon.Validate();

            //assert
            validationResult.Should().Be("The discount percentage cannot be greater than 100%\n");
        }

        [TestMethod]
        public void ShouldReturnError_EmptyName()
        {
            //arrange
            Coupon coupon = new Coupon(0, "", "NDD10TECH", 10, 2000, true, DateTime.Now.AddDays(10), partner);

            //action
            string validationResult = coupon.Validate();

            //assert
            validationResult.Should().Be("The name field is required\n");
        }

        [TestMethod]
        public void ShouldReturnError_NullPartner()
        {
            //arrange
            Coupon coupon = new Coupon(0, "Coupon Name", "NDD10TECH", 10, 2000, true, DateTime.Now.AddDays(10), null);

            //action
            string validationResult = coupon.Validate();

            //assert
            validationResult.Should().Be("A partner is required\n");
        }
    }
}
