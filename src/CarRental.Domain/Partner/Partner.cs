using CarRental.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.PartnerModule
{
    public class Partner : BaseEntity
    {
        public string Name { get; }

        public Partner(int id, string? name)
        {
            Id = id;
            Name = name ?? string.Empty;
        }

        public override string Validate()
        {
            string validationResult = "";

            if (string.IsNullOrEmpty(Name))
                validationResult += "The Name field is required";
            if (validationResult == "")
                validationResult = "VALID";

            return validationResult;
        }

        public override bool Equals(object? obj)
        {
            return obj is Partner comparerdPartner &&
                   Id == comparerdPartner.Id &&
                   Name == comparerdPartner.Name;
        }

        public override int GetHashCode()
        {
            int hashCode = -1643562096;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }

        public override string ToString()
        {
            return $" {id}, {Name}";
        }
    }
}
