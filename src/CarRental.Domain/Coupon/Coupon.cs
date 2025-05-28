using CarRental.Domain.PartnerModule;
using CarRental.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.CouponModule
{
    public class Coupon : BaseEntity
    {
        public string Name { get; }
        public string Code { get; }
        public double Value { get; }
        public double MinimumValue { get; }
        public bool IsFixedDiscount { get; }
        public DateTime ExpirationDate { get; }
        public Partner Partner { get; }

        public Coupon(int id, string? name, string? code, double value, double minimumValue, bool isFixedDiscount, DateTime expirationDate, Partner partner)
        {
            Id = id;
            Name = name ?? string.Empty;
            Code = code ?? string.Empty;
            Value = value;
            MinimumValue = minimumValue;
            IsFixedDiscount = isFixedDiscount;
            ExpirationDate = expirationDate;
            Partner = partner;
        }
        public override string Validate()
        {
            string validationResult = "";

            if (Name.Length == 0)
                validationResult += "The name field is required\n";
            if (Code.Length == 0)
                validationResult += "The code field is required\n";
            if (Value <= 0)
                validationResult += "The value cannot be negative or 0 (zero)\n";
            if (!IsFixedDiscount && Value > 100)
                validationResult += "The discount percentage cannot be greater than 100%\n";
            if (Partner == null)
                validationResult += "A partner is required\n";
            if (validationResult == "")
                validationResult = "VALID";
            return validationResult;
        }
        public override string ToString()
        {
            return $"[{id}, {Name}, {Code}, {IsFixedDiscount}, {Value}]";
        }

        public override bool Equals(object? obj)
        {
            return obj is Coupon comparedCoupon &&
                   Id == comparedCoupon.Id &&
                   Name == comparedCoupon.Name &&
                   Code == comparedCoupon.Code &&
                   Value == comparedCoupon.Value &&
                   MinimumValue == comparedCoupon.MinimumValue &&
                   IsFixedDiscount == comparedCoupon.IsFixedDiscount &&
                   ExpirationDate == comparedCoupon.ExpirationDate &&
                   EqualityComparer<Partner>.Default.Equals(Partner, comparedCoupon.Partner);
        }

        public override int GetHashCode()
        {
            int hashCode = -1097376669;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Code);
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + MinimumValue.GetHashCode();
            hashCode = hashCode * -1521134295 + IsFixedDiscount.GetHashCode();
            hashCode = hashCode * -1521134295 + ExpirationDate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Partner>.Default.GetHashCode(Partner);
            return hashCode;
        }
    }
}
