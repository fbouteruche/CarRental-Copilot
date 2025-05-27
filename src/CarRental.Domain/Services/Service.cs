using CarRental.Domain.Shared;
using System.Collections.Generic;

namespace CarRental.Domain.ServiceModule
{
    public class Service : BaseEntity
    {    
        public Service(int id, string name, bool isChargedDaily, double value)
        {
            this.id = id;
            this.Name = name;
            this.IsChargedDaily = isChargedDaily;
            this.Value = value;
        }
        public string Name { get; }
        public bool IsChargedDaily { get; }
        public double Value { get; }

        public override string Validate()
        {
            string validationResult = "";
            if (this.Name.Length == 0)
                validationResult = "The name cannot be null\n";
            if (this.Value <= 0)
                validationResult += "The value cannot be null";
            if (validationResult.Length == 0)
                validationResult = "VALID";
            return validationResult;
        }

        public override bool Equals(object? obj)
        {
            return obj is Service service &&
                   id == service.id &&
                   Name == service.Name &&
                   IsChargedDaily == service.IsChargedDaily &&
                   Value == service.Value;
        }

        public override int GetHashCode()
        {
            int hashCode = 1764119922;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + IsChargedDaily.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"[{id}, {Name}, {IsChargedDaily}, {Value}]";
        }
    }
}
