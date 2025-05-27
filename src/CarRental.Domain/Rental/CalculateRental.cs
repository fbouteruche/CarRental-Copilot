using CarRental.Domain.CouponModule;
using CarRental.Domain.VehicleGroupModule;
using CarRental.Domain.ServiceModule;
using System;
using System.Collections.Generic;

namespace CarRental.Domain.Shared
{
    public static class CalculateRental
    {
        public const double CUSTOMER_INSURANCE_VALUE = 250.50f;
        public const double THIRD_PARTY_INSURANCE_VALUE = 500.75f;
        public const double GUARANTEE_VALUE = 1000f;
        public const double DAILY_LATE_FEE_PERCENTAGE = 0.1f;

        public static double CalculateGuarantee()
        {
            return GUARANTEE_VALUE;
        }

        public static double CalculateInsurance(string insuranceType)
        {
            double finalValue;
            if (insuranceType.Equals("CustomerInsurance"))
                finalValue = CUSTOMER_INSURANCE_VALUE;
            else if (insuranceType.Equals("ThirdPartyInsurance"))
                finalValue = THIRD_PARTY_INSURANCE_VALUE;
            else
                finalValue = 0;
            return finalValue;
        }

        public static double CalculatePlan(string planType, VehicleGroup? vehicleGroup, double kilometersDriven, DateTime startDate, DateTime endDate)
        {
            if (vehicleGroup == null)
                return 0;

            double daysInterval = (endDate - startDate).TotalDays;
            double dailyRate = 0;
            double perKmRate = 0;
            switch (planType)
            {
                case "DailyPlan": // Calculated by day and kilometers driven.
                    dailyRate = vehicleGroup.DailyPlanRate * daysInterval;
                    perKmRate = kilometersDriven * vehicleGroup.DailyPerKmRate;
                    break;

                case "ControlledKm": // Paid by day with a limit of kilometers per day. Exceeding the limit incurs additional charges.
                    dailyRate = vehicleGroup.ControlledPlanRate * daysInterval;
                    if (kilometersDriven > vehicleGroup.ControlledKmLimit)
                        perKmRate = (kilometersDriven - vehicleGroup.ControlledKmLimit) * vehicleGroup.ControlledExceededKmRate;
                    break;

                case "UnlimitedKm": // Only pays the daily rate with no kilometer limit.
                    dailyRate = vehicleGroup.UnlimitedPlanRate * daysInterval;
                    break;
            }
            return dailyRate + perKmRate;
        }

        public static double CalculateServices(List<Service> services, DateTime startDate, DateTime endDate)
        {
            double total = 0;
            double daysInterval = (endDate - startDate).TotalDays;
            foreach (Service service in services)
            {
                if (service.IsChargedDaily)
                    total += service.Value * daysInterval;
                else
                    total += service.Value;
            }
            return total;
        }

        public static double CalculateFuelDifference(double tankCapacity, double currentFuelPercentage, double pricePerLiter)
        {
            double currentTankLevel = tankCapacity * currentFuelPercentage;
            double tankDifference = tankCapacity - currentTankLevel;
            double amountToPay = tankDifference * pricePerLiter;

            return amountToPay;
        }

        public static double CalculateLateReturnFee(double totalPrice, DateTime expectedReturnDate, DateTime actualReturnDate)
        {
            double totalFee = 0;
            if (actualReturnDate > expectedReturnDate)
            {
                double daysDifference = (actualReturnDate - expectedReturnDate).TotalDays;
                double dailyFee = totalPrice * DAILY_LATE_FEE_PERCENTAGE;
                totalFee = dailyFee * daysDifference;
            }
            return totalFee;
        }

        public static double CalculateDiscountCoupon(double totalPrice, CouponModule.Coupon coupon)
        {
            double discount = 0;
            if (coupon != null)
            {
                bool isValidToday = DateTime.Now <= coupon.ExpirationDate;
                bool isAboveMinimumValue = totalPrice >= coupon.MinimumValue;

                if (isValidToday && isAboveMinimumValue)
                {
                    if (coupon.IsFixedDiscount)
                        discount = coupon.Value;
                    else
                    {
                        double discountPercentage = coupon.Value / 100;
                        discount = totalPrice * discountPercentage;
                    }
                }
            }
            return discount;
        }
    }
}