using CarRental.Domain.CustomerModule;
using CarRental.Domain.CouponModule;
using CarRental.Domain.EmployeeModule;
using CarRental.Domain.ServiceModule;
using CarRental.Domain.Shared;
using CarRental.Domain.VehicleModule;
using System;
using System.Collections.Generic;

namespace CarRental.Domain.RentalModule
{
    public class Rental : BaseEntity
    {
        private Vehicle vehicle;
        private Employee rentingEmployee;
        private Customer contractingCustomer;
        private Customer driverCustomer;
        private Coupon coupon;
        private DateTime departureDate;
        private DateTime expectedReturnDate;
        private DateTime returnDate;
        private string planType;         // DailyPlan, ControlledKm, or UnlimitedKm
        private string insuranceType;    // CustomerInsurance, ThirdPartyInsurance, or None
        private double rentalPrice;
        private double returnPrice;
        private bool isOpen;
        private List<Service> services;

        // Constructor for common use (ISSUES IN TESTS. EQUALS RETURNS DIFFERENT)
        public Rental(int id, Vehicle vehicle, Employee rentingEmployee, Customer contractingCustomer, Customer driverCustomer, CouponModule.Coupon coupon, DateTime departureDate, DateTime expectedReturnDate, string planType, string insuranceType, List<Service> services)
        {
            this.id = id;
            this.vehicle = vehicle;
            this.rentingEmployee = rentingEmployee;
            this.contractingCustomer = contractingCustomer;
            this.driverCustomer = driverCustomer;
            this.coupon = coupon;
            this.departureDate = departureDate;
            this.expectedReturnDate = expectedReturnDate;
            this.planType = planType;
            this.insuranceType = insuranceType;
            this.services = services;

            isOpen = false;
            OpenRental(departureDate);
            returnDate = DateTime.MaxValue;
            returnPrice = 0;
        }

        // Constructor ONLY for loading from the database
        public Rental(int id, Vehicle vehicle, Employee rentingEmployee, Customer contractingCustomer, Customer driverCustomer, CouponModule.Coupon? coupon, DateTime departureDate, DateTime expectedReturnDate, DateTime returnDate, string? planType, string? insuranceType, double rentalPrice, double returnPrice, bool isOpen, List<Service> services)
        {
            this.id = id;
            this.vehicle = vehicle;
            this.rentingEmployee = rentingEmployee;
            this.contractingCustomer = contractingCustomer;
            this.driverCustomer = driverCustomer;
            this.coupon = coupon;
            this.departureDate = departureDate;
            this.expectedReturnDate = expectedReturnDate;
            this.returnDate = returnDate;
            this.planType = planType ?? string.Empty;
            this.insuranceType = insuranceType ?? string.Empty;
            this.rentalPrice = rentalPrice;
            this.returnPrice = returnPrice;
            this.isOpen = isOpen;
            this.services = services;
        }

        public Vehicle Vehicle { get => vehicle; }
        public Employee RentingEmployee { get => rentingEmployee; }
        public Customer ContractingCustomer { get => contractingCustomer; }
        public Customer DriverCustomer { get => driverCustomer; }
        public CouponModule.Coupon Coupon { get => coupon; }
        public DateTime DepartureDate { get => departureDate; }
        public DateTime ExpectedReturnDate { get => expectedReturnDate; }
        public DateTime ReturnDate { get => returnDate; }
        public string PlanType { get => planType; }
        public string InsuranceType { get => insuranceType; }
        public double RentalPrice { get => rentalPrice; }
        public double ReturnPrice { get => returnPrice; }
        public bool IsOpen { get => isOpen; }
        public List<Service> Services { get => services; set => services = value; }

        public void OpenRental(DateTime openingDate)
        {
            isOpen = true;
            departureDate = openingDate;
            vehicle.IsRented = true;
            rentalPrice = CalculateRental.CalculateInsurance(insuranceType);
            rentalPrice += CalculateRental.CalculateGuarantee();
            rentalPrice = Math.Round(rentalPrice, 2);
        }

        public void CloseRental(DateTime closingDate, double fuelSurcharge, double kilometersDriven)
        {
            isOpen = false;
            returnDate = closingDate;
            vehicle.Mileage += kilometersDriven;
            vehicle.IsRented = false;
            returnPrice = rentalPrice;
            returnPrice += fuelSurcharge;
            returnPrice += CalculateRental.CalculatePlan(planType, vehicle.VehicleGroup, kilometersDriven, departureDate, returnDate);
            returnPrice += CalculateRental.CalculateServices(services, departureDate, returnDate);
            returnPrice += CalculateRental.CalculateLateReturnFee(returnPrice, expectedReturnDate, returnDate);
            returnPrice -= CalculateRental.CalculateDiscountCoupon(returnPrice, coupon);
            returnPrice = Math.Round(returnPrice, 2);
        }

        public override string Validate()
        {
            string validationResult = "";
            if (this.vehicle == null)
                validationResult = "The vehicle cannot be null\n";

            if (this.rentingEmployee == null)
                validationResult += "The renting employee cannot be null\n";

            if (this.contractingCustomer == null)
                validationResult += "The contracting customer cannot be null\n";

            else if (!this.contractingCustomer.IsPhysicalPerson && this.driverCustomer == null)
                validationResult += "The driver cannot be null when the contracting customer is a legal entity\n";

            if (this.driverCustomer != null)
                if (!this.driverCustomer.IsPhysicalPerson)
                    validationResult += "The driver cannot be a legal entity.\n";

            if (!this.planType.Equals("DailyPlan") && !this.planType.Equals("ControlledKm") && !this.planType.Equals("UnlimitedKm"))
                validationResult += "The plan type is invalid.\n";

            if (!this.insuranceType.Equals("CustomerInsurance") && !this.insuranceType.Equals("ThirdPartyInsurance") && !this.insuranceType.Equals("None"))
                validationResult += "The insurance type is invalid.\n";

            if (this.DepartureDate >= this.ExpectedReturnDate)
                validationResult += "The return date cannot be earlier than the rental date.\n";

            if (validationResult == "")
                validationResult = "VALID";

            return validationResult;
        }

        public override string ToString()
        {
            return $"RentalModule = {id}, {vehicle}, {rentingEmployee}, {contractingCustomer}, {driverCustomer}, {departureDate}, {expectedReturnDate}, {returnDate}, {planType}, {insuranceType}, {rentalPrice}, {returnPrice}, {isOpen}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Rental rental &&
                   id == rental.id &&
                   EqualityComparer<Vehicle>.Default.Equals(vehicle, rental.vehicle) &&
                   EqualityComparer<Employee>.Default.Equals(rentingEmployee, rental.rentingEmployee) &&
                   EqualityComparer<Customer>.Default.Equals(contractingCustomer, rental.contractingCustomer) &&
                   EqualityComparer<Customer>.Default.Equals(driverCustomer, rental.driverCustomer) &&
                   departureDate == rental.departureDate &&
                   expectedReturnDate == rental.expectedReturnDate &&
                   returnDate == rental.returnDate &&
                   planType == rental.planType &&
                   insuranceType == rental.insuranceType &&
                   rentalPrice == rental.rentalPrice &&
                   returnPrice == rental.returnPrice &&
                   isOpen == rental.isOpen;
        }

        public override int GetHashCode()
        {
            int hashCode = 1457090499;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vehicle>.Default.GetHashCode(vehicle);
            hashCode = hashCode * -1521134295 + EqualityComparer<Employee>.Default.GetHashCode(rentingEmployee);
            hashCode = hashCode * -1521134295 + EqualityComparer<Customer>.Default.GetHashCode(contractingCustomer);
            hashCode = hashCode * -1521134295 + EqualityComparer<Customer>.Default.GetHashCode(driverCustomer);
            hashCode = hashCode * -1521134295 + departureDate.GetHashCode();
            hashCode = hashCode * -1521134295 + expectedReturnDate.GetHashCode();
            hashCode = hashCode * -1521134295 + returnDate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(planType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(insuranceType);
            hashCode = hashCode * -1521134295 + rentalPrice.GetHashCode();
            hashCode = hashCode * -1521134295 + returnPrice.GetHashCode();
            hashCode = hashCode * -1521134295 + isOpen.GetHashCode();
            return hashCode;
        }
    }
}