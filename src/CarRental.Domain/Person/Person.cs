using CarRental.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.PersonModule
{
    public abstract class Person : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string UniqueId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsPhysicalPerson { get; set; }

        public virtual string ValidatePerson()
        {
            string validationResult = "";
            bool uniqueIdValidationResult = false;
            if (IsPhysicalPerson)
                uniqueIdValidationResult = ValidateSSN(UniqueId);
            else
                uniqueIdValidationResult = ValidateTaxId(UniqueId);

            if (this.Name.Length == 0)
                validationResult = "The name cannot be null\n";
            if (this.Address.Length <= 0)
                validationResult += "The address cannot be null\n";
            if (this.Email.Length == 0 || (!this.Email.Contains('@')))
                validationResult += "The email is mandatory, incorrect, and must be valid\n";
            if (!uniqueIdValidationResult)
            {
                if (IsPhysicalPerson)
                    validationResult += "The SSN is not valid\n";
                else
                    validationResult += "The EIN is not valid\n";
            }
            if (validationResult == "")
                validationResult = "VALID";

            return validationResult;
        }

        private static bool ValidateSSN(string ssn)
        {
            int[] multiplier1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempSsn;
            string digit;
            int sum;
            int remainder;
            ssn = ssn.Trim();
            ssn = ssn.Replace(".", "").Replace("-", "").Replace(",", "");
            if (ssn.Length != 11)
                return false;
            tempSsn = ssn.Substring(0, 9);
            sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempSsn[i].ToString()) * multiplier1[i];
            remainder = sum % 11;
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;
            digit = remainder.ToString();
            tempSsn = tempSsn + digit;
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempSsn[i].ToString()) * multiplier2[i];
            remainder = sum % 11;
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;
            digit = digit + remainder.ToString();
            return ssn.EndsWith(digit);
        }

        private static bool ValidateTaxId(string taxId)
        {
            //validate US tax ID (EIN)
            // This is a simplified version and may not cover all cases
            // In a real-world scenario, you would use a more robust validation method
            // For example, you could use a regex pattern to validate the format of the EIN
            // or use a library that provides EIN validation
            // For now, we'll just check if the length is 9 and if it contains only digits
            taxId = taxId.Trim();
            taxId = taxId.Replace("-", "").Replace(" ", "");
            if (taxId.Length != 9)
                return false;
            foreach (char c in taxId)
                {
                if (!char.IsDigit(c))
                    return false;
            }
            // Check if the first two digits are not both zeros
            if (taxId.StartsWith("00"))
                return false;
            // Check if the first digit is not a zero
            if (taxId[0] == '0')
                return false;
            // Check if the last digit is not a zero
            if (taxId[8] == '0')
                return false;
 
            return true;
        }
    }
}
