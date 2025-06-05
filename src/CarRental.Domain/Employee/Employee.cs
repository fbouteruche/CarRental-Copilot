using CarRental.Domain.PersonModule;
using System;
using System.Collections.Generic;

namespace CarRental.Domain.EmployeeModule
{
    public class Employee : Person
    {
        public int InternalRegistration { get; }
        public string LoginUsername { get; }
        public DateTime HiringDate { get; }
        public string JobTitle { get; }
        public double Salary { get; }

        public string UserPassword { get; }

        public Employee(int id, string? name, string? uniqueIdentifier, string? address, string? phone, string? email, int internalRegistrationId, string? loginUserName, string? userPassword, DateTime hiringDate, string? jobTitle, double salary, bool isPhysicalPerson)
        {
            this.id = id;
            Name = name ?? string.Empty;
            UniqueId = uniqueIdentifier ?? string.Empty;
            Address = address ?? string.Empty;
            Phone = phone ?? string.Empty;
            Email = email ?? string.Empty;
            IsPhysicalPerson = true;
            InternalRegistration = internalRegistrationId;
            LoginUsername = loginUserName ?? string.Empty;
            UserPassword = userPassword ?? string.Empty;
            HiringDate = hiringDate;
            JobTitle = jobTitle ?? string.Empty;
            Salary = salary;
            IsPhysicalPerson = isPhysicalPerson;
        }

        public override string Validate()
        {
            string validationResult = "";
            if (LoginUsername.Length == 0)
                validationResult += "The access username cannot be empty\n";
            if (InternalRegistration <= 0)
                validationResult += "Invalid registration number\n";
            if (Salary <= 0)
                validationResult += "The salary must be greater than $0.00\n";
            if (JobTitle.Length == 0)
                validationResult += "The employee must have a job title\n";
            if (HiringDate > DateTime.Now.AddMonths(2))
                validationResult += "Invalid hiring date\n";
            if (UserPassword.Length <= 3)
                validationResult += "The password cannot be less than three characters\n";
            if (base.ValidatePerson() != "VALID")
                validationResult += base.ValidatePerson();
            if (validationResult == "")
                validationResult += "VALID";
            return validationResult;
        }

        public override string ToString()
        {
            return $" {id} {Name} {InternalRegistration} {Phone} {LoginUsername} {JobTitle}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Employee comparedEmployee &&
                   id == comparedEmployee.id &&
                   Name == comparedEmployee.Name &&
                   UniqueId == comparedEmployee.UniqueId &&
                   Address == comparedEmployee.Address &&
                   Phone == comparedEmployee.Phone &&
                   Email == comparedEmployee.Email &&
                   IsPhysicalPerson == comparedEmployee.IsPhysicalPerson &&
                   InternalRegistration == comparedEmployee.InternalRegistration &&
                   LoginUsername == comparedEmployee.LoginUsername &&
                   HiringDate == comparedEmployee.HiringDate &&
                   JobTitle == comparedEmployee.JobTitle &&
                   Salary == comparedEmployee.Salary &&
                   UserPassword == comparedEmployee.UserPassword;
        }

        public override int GetHashCode()
        {
            int hashCode = 497940720;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UniqueId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Address);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Phone);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
            hashCode = hashCode * -1521134295 + IsPhysicalPerson.GetHashCode();
            hashCode = hashCode * -1521134295 + InternalRegistration.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LoginUsername);
            hashCode = hashCode * -1521134295 + HiringDate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(JobTitle);
            hashCode = hashCode * -1521134295 + Salary.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserPassword);
            return hashCode;
        }
    }
}
