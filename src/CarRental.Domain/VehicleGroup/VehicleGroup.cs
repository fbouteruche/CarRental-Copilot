using CarRental.Domain.Shared;
using System.Collections.Generic;

namespace CarRental.Domain.VehicleGroupModule
{
    public class VehicleGroup : BaseEntity
    {
        private string name;
        private double dailyPlanRate;
        private double dailyPerKmRate;
        private double controlledPlanRate;
        private int controlledKmLimit;
        private double controlledExceededKmRate;
        private double unlimitedPlanRate;

        public string Name { get => name; }
        public double DailyPlanRate { get => dailyPlanRate; }
        public double DailyPerKmRate { get => dailyPerKmRate; }
        public double ControlledPlanRate { get => controlledPlanRate; }
        public int ControlledKmLimit { get => controlledKmLimit; }
        public double ControlledExceededKmRate { get => controlledExceededKmRate; }
        public double UnlimitedPlanRate { get => unlimitedPlanRate; }

        public VehicleGroup(int id, string name, double dailyPlanRate, double dailyPerKmRate, double controlledPlanRate, int controlledKmLimit, double controlledExceededKmRate, double unlimitedPlanRate)
        {
            this.id = id;
            this.name = name;
            this.dailyPlanRate = dailyPlanRate;
            this.dailyPerKmRate = dailyPerKmRate;
            this.controlledPlanRate = controlledPlanRate;
            this.controlledKmLimit = controlledKmLimit;
            this.controlledExceededKmRate = controlledExceededKmRate;
            this.unlimitedPlanRate = unlimitedPlanRate;
        }

        public override string Validate()
        {
            string validationResult = "";

            if (this.name.Length <= 0)
                validationResult = "The name cannot be null\n";

            if (this.dailyPlanRate <= 0f)
                validationResult += "The daily rate for the Daily Plan cannot be null\n";

            if (this.dailyPerKmRate <= 0f)
                validationResult += "The per KM rate for the Daily Plan cannot be null\n";

            if (this.controlledPlanRate <= 0f)
                validationResult += "The daily rate for the Controlled Plan cannot be null\n";

            if (this.controlledKmLimit <= 0)
                validationResult += "The KM limit for the Controlled Plan cannot be null\n";

            if (this.controlledExceededKmRate <= 0f)
                validationResult += "The exceeded KM rate for the Controlled Plan cannot be null\n";

            if (this.unlimitedPlanRate <= 0f)
                validationResult += "The daily rate for the Unlimited Plan cannot be null\n";

            if (validationResult.Length == 0)
                validationResult = "VALID";

            return validationResult;
        }

        public override bool Equals(object? obj)
        {
            return obj is VehicleGroup vehicleGroup &&
                   id == vehicleGroup.id &&
                   name == vehicleGroup.name &&
                   dailyPlanRate == vehicleGroup.dailyPlanRate &&
                   dailyPerKmRate == vehicleGroup.dailyPerKmRate &&
                   controlledPlanRate == vehicleGroup.controlledPlanRate &&
                   controlledKmLimit == vehicleGroup.controlledKmLimit &&
                   controlledExceededKmRate == vehicleGroup.controlledExceededKmRate &&
                   unlimitedPlanRate == vehicleGroup.unlimitedPlanRate;
        }

        public override int GetHashCode()
        {
            int hashCode = -1513818721;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + dailyPlanRate.GetHashCode();
            hashCode = hashCode * -1521134295 + dailyPerKmRate.GetHashCode();
            hashCode = hashCode * -1521134295 + controlledPlanRate.GetHashCode();
            hashCode = hashCode * -1521134295 + controlledKmLimit.GetHashCode();
            hashCode = hashCode * -1521134295 + controlledExceededKmRate.GetHashCode();
            hashCode = hashCode * -1521134295 + unlimitedPlanRate.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{id} {name}";
        }
    }
}