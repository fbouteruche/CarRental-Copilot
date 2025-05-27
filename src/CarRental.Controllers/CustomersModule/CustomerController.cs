using CarRental.Controllers.Shared;
using CarRental.Domain.CustomerModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Controllers.CustomersModule
{
    public class CustomerController : Controller<Customer>
    {
        #region Queries
        private const string sqlInsertCustomers =
        @"
           INSERT INTO [Customer]
        (
            [Name],
            [UniqueRegister],
            [Address],
            [Email],
            [Phone],
            [IsIndividual],
            [DriverLicense],
            [DriverLicenseValidity]
        )
        VALUES
        (
            @Name,
            @UniqueRegister,
            @Address,
            @Email,
            @Phone,
            @IsIndividual,
            @DriverLicense,
            @DriverLicenseValidity
        )";

        private const string sqlEditCustomers =
        @"
                UPDATE [Customer] 
                 SET
                    [Name] = @Name,
                    [UniqueRegister] = @UniqueRegister,
                    [Address] = @Address,
                    [Phone] = @Phone,
                    [Email] = @Email,
                    [IsIndividual] = @IsIndividual,
                    [DriverLicense] = @DriverLicense,
                    [DriverLicenseValidity] = @DriverLicenseValidity
                WHERE [Id] = @Id;
            ";

        private const string sqlDeleteCustomers =
        @"
                DELETE FROM [Customer] WHERE [Id] = @Id
            ";

        private const string sqlSelectAllCustomers =
        @"
            SELECT * FROM [Customer]
            ";

        private const string sqlSelectCustomerById =
        @"
            SELECT * FROM [Customer] WHERE [Id] = @Id;
            ";

        private const string sqlCustomerExists =
            @"SELECT 
                COUNT(*) 
            FROM 
                [Customer]
            WHERE 
                [Id] = @Id";

        #endregion

        public override string Edit(int id, Customer record)
        {
            string validationResult = record.Validate();

            if (validationResult == "VALID")
            {
                record.Id = id;
                Db.Update(sqlEditCustomers, GetCustomerParameters(record));
            }

            return validationResult;
        }

        public override bool Delete(int id)
        {
            try
            {
                Db.Delete(sqlDeleteCustomers, AddParameter("Id", id));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public override bool Exists(int id)
        {
            return Db.Exists(sqlCustomerExists, AddParameter("Id", id));
        }

        public override string InsertNew(Customer record)
        {
            string validationResult = record.Validate();

            if (validationResult == "VALID")
            {
                record.Id = Db.Insert(sqlInsertCustomers, GetCustomerParameters(record));
            }
            return validationResult;
        }

        public override Customer? SelectById(int id)
        {
            return Db.Get(sqlSelectCustomerById, ConvertToCustomer, AddParameter("Id", id));
        }

        public override List<Customer> SelectAll()
        {
            return Db.GetAll(sqlSelectAllCustomers, ConvertToCustomer);
        }

        private Customer ConvertToCustomer(IDataReader reader)
        {
            DateTime? driverLicenseValidity = null;
            int id = Convert.ToInt32(reader["Id"]);
            string? name = reader["Name"] != DBNull.Value ? Convert.ToString(reader["Name"]) : string.Empty;
            string? uniqueRegister = reader["UniqueRegister"] != DBNull.Value ? Convert.ToString(reader["UniqueRegister"]) : string.Empty;
            string? address = reader["Address"] != DBNull.Value ? Convert.ToString(reader["Address"]) : string.Empty;
            string? phone = reader["Phone"] != DBNull.Value ? Convert.ToString(reader["Phone"]) : string.Empty;
            string? email = reader["Email"] != DBNull.Value ? Convert.ToString(reader["Email"]) : string.Empty;
            string? driverLicense = reader["DriverLicense"] != DBNull.Value ? Convert.ToString(reader["DriverLicense"]) : string.Empty;
            if(reader["DriverLicenseValidity"] != DBNull.Value)
                driverLicenseValidity = Convert.ToDateTime(reader["DriverLicenseValidity"]);
            bool isIndividual = Convert.ToBoolean(reader["IsIndividual"]);

            Customer customer = new Customer(id, name, uniqueRegister, address, phone, email, driverLicense, driverLicenseValidity, isIndividual);
            customer.Id = id;
            return customer;
        }

        private Dictionary<string, object> GetCustomerParameters(Customer customer)
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("Id", customer.Id);
            parameters.Add("Name", customer.Name);
            parameters.Add("UniqueRegister", customer.UniqueId);
            parameters.Add("Address", customer.Address);
            parameters.Add("Phone", customer.Phone);
            parameters.Add("Email", customer.Email);
            parameters.Add("DriverLicense", customer.DriverLicense);
            parameters.Add("DriverLicenseValidity", customer.LicenseExpiryDate);
            parameters.Add("IsIndividual", customer.IsPhysicalPerson);

            return parameters;
        }
    }
}
