using CarRental.Domain.RentalModule;
using CarRental.Domain.ServiceModule;
using CarRental.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.RentalServiceRelationshipModule
{
    public class RentalServiceRelationship : BaseEntity
    {
        public RentalModule.Rental Rental { get; }
        public List<Service> Services { get; }

        public RentalServiceRelationship(int id, RentalModule.Rental rental, List<Service> services)
        {
            this.Id = id;
            Rental = rental;
            Services = services;
        }

        public override string Validate()
        {
            string validationResult = "";
            if (Rental.Id == 0)
                validationResult = "Invalid rental ID";
            if (Services == null)
                validationResult = "No services selected";
            if (validationResult == "")
                validationResult = "VALID";
            return validationResult;
        }

        public override int GetHashCode()
        {
            int hashCode = 1438320420;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Rental>.Default.GetHashCode(Rental);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Service>>.Default.GetHashCode(Services);
            return hashCode;
        }

        public override string ToString()
        {
            return $"{id} {Rental} {Services}";
        }

        public override bool Equals(object? obj)
        {
            return obj is RentalServiceRelationship relationship &&
                   Id == relationship.Id &&
                   EqualityComparer<Rental>.Default.Equals(Rental, relationship.Rental) &&
                   EqualityComparer<List<Service>>.Default.Equals(Services, relationship.Services);
        }
    }
}