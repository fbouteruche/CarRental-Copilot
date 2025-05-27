using CarRental.Domain.VehicleGroupModule;
using CarRental.Domain.VehicleImageModule;
using CarRental.Domain.Shared;
using System.Collections.Generic;
using System.Drawing;

namespace CarRental.Domain.VehicleModule
{
    public class Vehicle : BaseEntity
    {
        public string Model { get; set; } = string.Empty;
        public VehicleGroup? VehicleGroup { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Chassis { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public double TankCapacity { get; set; }
        public int Year { get; set; }
        public double Mileage { get; set; }
        public int NumberOfDoors { get; set; }
        public int PassengerCapacity { get; set; }
        public char TrunkSize { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasPowerSteering { get; set; }
        public bool HasAbsBrakes { get; set; }
        public bool IsRented { get; set; }
        public List<VehicleImage>? Images { get; set; }

        public Vehicle() { }

        public Vehicle(int id, string model, VehicleGroup vehicleGroup, string licensePlate, string chassis, string brand, string color, string fuelType, double tankCapacity, int year, double mileage, int numberOfDoors, int passengerCapacity, char trunkSize, bool hasAirConditioning, bool hasPowerSteering, bool hasAbsBrakes, bool isRented, List<VehicleImage>? images)
        {
            this.id = id;
            this.Model = model;
            this.VehicleGroup = vehicleGroup;
            this.LicensePlate = licensePlate;
            this.Chassis = chassis;
            this.Brand = brand;
            this.Color = color;
            this.FuelType = fuelType;
            this.TankCapacity = tankCapacity;
            this.Year = year;
            this.Mileage = mileage;
            this.NumberOfDoors = numberOfDoors;
            this.PassengerCapacity = passengerCapacity;
            this.TrunkSize = trunkSize;
            this.HasAirConditioning = hasAirConditioning;
            this.HasPowerSteering = hasPowerSteering;
            this.HasAbsBrakes = hasAbsBrakes;
            this.IsRented = isRented;
            this.Images = images;
        }

        public override string Validate()
        {
            string validationResult = "";

            if (this.Model.Length == 0)
                validationResult = "The field 'model' cannot be empty!\n";
            if (this.LicensePlate.Length == 0)
                validationResult += "The field 'licensePlate' cannot be empty!\n";
            if (this.Chassis.Length == 0)
                validationResult += "The field 'chassis' cannot be empty!\n";
            if (this.Brand.Length == 0)
                validationResult += "The field 'brand' cannot be empty!\n";
            if (this.Color.Length == 0)
                validationResult += "The field 'color' cannot be empty!\n";
            if (this.FuelType.Length == 0)
                validationResult += "The field 'fuelType' cannot be empty!\n";
            if (this.TankCapacity == 0)
                validationResult += "The field 'tankCapacity' cannot be empty!\n";
            if (this.Year <= 0)
                validationResult += "The field 'year' cannot be empty!\n";
            if (this.Mileage <= 0)
                validationResult += "The field 'mileage' cannot be empty!\n";
            if (this.NumberOfDoors <= 0)
                validationResult += "The field 'numberOfDoors' cannot be empty!\n";
            if (this.PassengerCapacity <= 0)
                validationResult += "The field 'passengerCapacity' cannot be empty!\n";
            if (this.TrunkSize != 'S' && this.TrunkSize != 'M' && this.TrunkSize != 'L')
                validationResult += "The field 'trunkSize' must be 'S', 'M', or 'L'!\n";
            if (validationResult == "")
                validationResult = "VALID";
            return validationResult;
        }

        public override string ToString()
        {
            return $"[{id}, {Model}, {VehicleGroup}, {LicensePlate}]";
        }

        public override bool Equals(object? obj)
        {
            var vehicle = obj as Vehicle;
            return vehicle != null &&
                   id == vehicle.id &&
                   Model == vehicle.Model &&
                   EqualityComparer<VehicleGroup?>.Default.Equals(VehicleGroup, vehicle.VehicleGroup) &&
                   LicensePlate == vehicle.LicensePlate &&
                   Chassis == vehicle.Chassis &&
                   Brand == vehicle.Brand &&
                   Color == vehicle.Color &&
                   FuelType == vehicle.FuelType &&
                   TankCapacity == vehicle.TankCapacity &&
                   Year == vehicle.Year &&
                   Mileage == vehicle.Mileage &&
                   NumberOfDoors == vehicle.NumberOfDoors &&
                   PassengerCapacity == vehicle.PassengerCapacity &&
                   TrunkSize == vehicle.TrunkSize &&
                   HasAirConditioning == vehicle.HasAirConditioning &&
                   HasPowerSteering == vehicle.HasPowerSteering &&
                   HasAbsBrakes == vehicle.HasAbsBrakes &&
                   IsRented == vehicle.IsRented
                   //TODO
                   //implemet deep Equals over the image collection &&
                   //(images == vehicle.images || (images.Count == 0 && vehicle.images == null) || (images.Count == 0 && vehicle.images.Count == 0))
                   ;
        }

        public override int GetHashCode()
        {
            int hashCode = -1113965374;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Model);
            hashCode = hashCode * -1521134295 + (VehicleGroup != null ? EqualityComparer<VehicleGroup>.Default.GetHashCode(VehicleGroup) : 0);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LicensePlate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Chassis);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Brand);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Color);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FuelType);
            hashCode = hashCode * -1521134295 + TankCapacity.GetHashCode();
            hashCode = hashCode * -1521134295 + Year.GetHashCode();
            hashCode = hashCode * -1521134295 + Mileage.GetHashCode();
            hashCode = hashCode * -1521134295 + NumberOfDoors.GetHashCode();
            hashCode = hashCode * -1521134295 + PassengerCapacity.GetHashCode();
            hashCode = hashCode * -1521134295 + TrunkSize.GetHashCode();
            hashCode = hashCode * -1521134295 + HasAirConditioning.GetHashCode();
            hashCode = hashCode * -1521134295 + HasPowerSteering.GetHashCode();
            hashCode = hashCode * -1521134295 + HasAbsBrakes.GetHashCode();
            hashCode = hashCode * -1521134295 + IsRented.GetHashCode();
            hashCode = hashCode * -1521134295 + (Images != null ? Images.GetHashCode() : 0);
            return hashCode;
        }
    }
}
