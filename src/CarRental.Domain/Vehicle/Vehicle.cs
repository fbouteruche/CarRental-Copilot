using CarRental.Domain.VehicleGroupModule;
using CarRental.Domain.VehicleImageModule;
using CarRental.Domain.Shared;
using System.Collections.Generic;
using System.Drawing;

namespace CarRental.Domain.VehicleModule
{
    public class Vehicle : BaseEntity
    {
        public string model = "";
        public VehicleGroup? vehicleGroup;
        public string licensePlate = "";
        public string chassis = "";
        public string brand = "";
        public string color = "";
        public string fuelType = "";
        public double tankCapacity;
        public int year;
        public double mileage;
        public int numberOfDoors;
        public int passengerCapacity;
        public char trunkSize;
        public bool hasAirConditioning;
        public bool hasPowerSteering;
        public bool hasAbsBrakes;
        public bool isRented;
        public List<VehicleImage>? images;

        public Vehicle() { }

        public Vehicle(int id, string model, VehicleGroup vehicleGroup, string licensePlate, string chassis, string brand, string color, string fuelType, double tankCapacity, int year, double mileage, int numberOfDoors, int passengerCapacity, char trunkSize, bool hasAirConditioning, bool hasPowerSteering, bool hasAbsBrakes, bool isRented, List<VehicleImage> images)
        {
            this.id = id;
            this.model = model;
            this.vehicleGroup = vehicleGroup;
            this.licensePlate = licensePlate;
            this.chassis = chassis;
            this.brand = brand;
            this.color = color;
            this.fuelType = fuelType;
            this.tankCapacity = tankCapacity;
            this.year = year;
            this.mileage = mileage;
            this.numberOfDoors = numberOfDoors;
            this.passengerCapacity = passengerCapacity;
            this.trunkSize = trunkSize;
            this.hasAirConditioning = hasAirConditioning;
            this.hasPowerSteering = hasPowerSteering;
            this.hasAbsBrakes = hasAbsBrakes;
            this.isRented = isRented;
            this.images = images;
        }

        public override string Validate()
        {
            string validationResult = "";

            if (this.model.Length == 0)
                validationResult = "The field 'model' cannot be empty!\n";
            if (this.licensePlate.Length == 0)
                validationResult += "The field 'licensePlate' cannot be empty!\n";
            if (this.chassis.Length == 0)
                validationResult += "The field 'chassis' cannot be empty!\n";
            if (this.brand.Length == 0)
                validationResult += "The field 'brand' cannot be empty!\n";
            if (this.color.Length == 0)
                validationResult += "The field 'color' cannot be empty!\n";
            if (this.fuelType.Length == 0)
                validationResult += "The field 'fuelType' cannot be empty!\n";
            if (this.tankCapacity == 0)
                validationResult += "The field 'tankCapacity' cannot be empty!\n";
            if (this.year <= 0)
                validationResult += "The field 'year' cannot be empty!\n";
            if (this.mileage <= 0)
                validationResult += "The field 'mileage' cannot be empty!\n";
            if (this.numberOfDoors <= 0)
                validationResult += "The field 'numberOfDoors' cannot be empty!\n";
            if (this.passengerCapacity <= 0)
                validationResult += "The field 'passengerCapacity' cannot be empty!\n";
            if (this.trunkSize != 'S' && this.trunkSize != 'M' && this.trunkSize != 'L')
                validationResult += "The field 'trunkSize' must be 'S', 'M', or 'L'!\n";
            if (validationResult == "")
                validationResult = "VALID";
            return validationResult;
        }

        public override string ToString()
        {
            return $"[{id}, {model}, {vehicleGroup}, {licensePlate}]";
        }

        public override bool Equals(object? obj)
        {
            var vehicle = obj as Vehicle;
            return !(vehicle is null) &&
                   id == vehicle.id &&
                   model == vehicle.model &&
                   EqualityComparer<VehicleGroup?>.Default.Equals(vehicleGroup, vehicle.vehicleGroup) &&
                   licensePlate == vehicle.licensePlate &&
                   chassis == vehicle.chassis &&
                   brand == vehicle.brand &&
                   color == vehicle.color &&
                   fuelType == vehicle.fuelType &&
                   tankCapacity == vehicle.tankCapacity &&
                   year == vehicle.year &&
                   mileage == vehicle.mileage &&
                   numberOfDoors == vehicle.numberOfDoors &&
                   passengerCapacity == vehicle.passengerCapacity &&
                   trunkSize == vehicle.trunkSize &&
                   hasAirConditioning == vehicle.hasAirConditioning &&
                   hasPowerSteering == vehicle.hasPowerSteering &&
                   hasAbsBrakes == vehicle.hasAbsBrakes &&
                   isRented == vehicle.isRented
                   //TODO
                   //implemet deep Equals over the image collection &&
                   //(images == vehicle.images || (images.Count == 0 && vehicle.images == null) || (images.Count == 0 && vehicle.images.Count == 0))
                   ;
        }

        public override int GetHashCode()
        {
            int hashCode = -1113965374;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(model);
            hashCode = hashCode * -1521134295 + EqualityComparer<VehicleGroup?>.Default.GetHashCode(vehicleGroup);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(licensePlate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(chassis);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(brand);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(color);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(fuelType);
            hashCode = hashCode * -1521134295 + tankCapacity.GetHashCode();
            hashCode = hashCode * -1521134295 + year.GetHashCode();
            hashCode = hashCode * -1521134295 + mileage.GetHashCode();
            hashCode = hashCode * -1521134295 + numberOfDoors.GetHashCode();
            hashCode = hashCode * -1521134295 + passengerCapacity.GetHashCode();
            hashCode = hashCode * -1521134295 + trunkSize.GetHashCode();
            hashCode = hashCode * -1521134295 + hasAirConditioning.GetHashCode();
            hashCode = hashCode * -1521134295 + hasPowerSteering.GetHashCode();
            hashCode = hashCode * -1521134295 + hasAbsBrakes.GetHashCode();
            hashCode = hashCode * -1521134295 + isRented.GetHashCode();
            hashCode = hashCode * -1521134295 + (images != null ? images.GetHashCode() : 0);
            return hashCode;
        }
    }
}
