using System;
using System.Collections.Generic;
using System.Data;
using CarRental.Controllers.Shared;
using CarRental.Domain.EmployeeModule;

namespace CarRental.Controllers.EmployeeModule
{
    public class EmployeeController : Controller<Employee>
    {

        #region Queries
        private const string insertCommand = @"INSERT INTO Employee
                                (
                                    [Name],
                                    [UniqueRegister],
                                    [Address],
                                    [Phone],
                                    [Email],
                                    [IsIndividual],
                                    [InternalRegister],
                                    [AccessUser],
                                    [Password],
                                    [Role],
                                    [Salary],
                                    [AdmissionDate]
                                )
                                VALUES
                                (
                                    @Name,
                                    @UniqueRegister,
                                    @Address,
                                    @Phone,
                                    @Email,
                                    @IsIndividual,
                                    @InternalRegister,
                                    @AccessUser,
                                    @Password,
                                    @Role,
                                    @Salary,
                                    @AdmissionDate
                                );";
        private const string updateCommand = @"UPDATE Employee 
                                SET
                                    [Name] = @Name,
                                    [UniqueRegister] = @UniqueRegister,
                                    [Address] = @Address,
                                    [Phone] = @Phone,
                                    [Email] = @Email,
                                    [IsIndividual] = @IsIndividual,
                                    [InternalRegister] = @InternalRegister,
                                    [AccessUser] = @AccessUser,
                                    [Password] = @Password,
                                    [Role] = @Role,
                                    [Salary] = @Salary,
                                    [AdmissionDate] = @AdmissionDate
                                WHERE [Id] = @Id;";
        private const string deleteCommand = @"DELETE FROM Employee WHERE [Id] = @Id;";
        private const string selectAllCommand = "SELECT * FROM Employee;";
        private const string selectByIdCommand = "SELECT * FROM Employee WHERE [Id] = @Id;";
        #endregion

        public override string Edit(int id, Employee employee)
        {
            string validationResult = employee.Validate();
            if (validationResult == "VALID")
            {
                employee.Id = id;
                Db.Update(updateCommand, GetEmployeeParameters(employee));
            }
            return validationResult;
        }

        public override bool Delete(int id)
        {
            try
            {
                Db.Delete(deleteCommand, AddParameter("Id", id));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override bool Exists(int id)
        {
            return Db.Exists(selectByIdCommand, AddParameter("Id", id));
        }

        public override string InsertNew(Employee employee)
        {
            string validationResult = employee.Validate();
            if (validationResult == "VALID")
                employee.Id = Db.Insert(insertCommand, GetEmployeeParameters(employee));

            return validationResult;
        }

        public override Employee? SelectById(int id)
        {
            return Db.Get(selectByIdCommand, ConvertToEmployee, AddParameter("Id", id));
        }

        public override List<Employee> SelectAll()
        {
            return Db.GetAll(selectAllCommand, ConvertToEmployee);
        }

        private Dictionary<string, object> GetEmployeeParameters(Employee employee)
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("Id", employee.Id);
            parameters.Add("Name", employee.Name);
            parameters.Add("UniqueRegister", employee.UniqueId);
            parameters.Add("Address", employee.Address);
            parameters.Add("Phone", employee.Phone);
            parameters.Add("Email", employee.Email);
            parameters.Add("InternalRegister", employee.InternalRegistration);
            parameters.Add("AccessUser", employee.LoginUsername);
            parameters.Add("Password", employee.UserPassword);
            parameters.Add("AdmissionDate", employee.HiringDate);
            parameters.Add("Role", employee.JobTitle);
            parameters.Add("Salary", float.Parse(Convert.ToString(employee.Salary)));
            parameters.Add("IsIndividual", Convert.ToBoolean(employee.IsPhysicalPerson));

            return parameters;
        }

        private Employee ConvertToEmployee(IDataReader reader)
        {
            int id = Convert.ToInt32(reader["Id"]);
            string name = Convert.ToString(reader["Name"]);
            string uniqueId = Convert.ToString(reader["UniqueRegister"]);
            string address = Convert.ToString(reader["Address"]);
            string phone = Convert.ToString(reader["Phone"]);
            string email = Convert.ToString(reader["Email"]);
            int internalRegistration = Convert.ToInt32(reader["InternalRegister"]);
            string loginUsername = Convert.ToString(reader["AccessUser"]);
            string userPassword = Convert.ToString(reader["Password"]);
            DateTime hiringDate = Convert.ToDateTime(reader["AdmissionDate"]);
            string jobTitle = Convert.ToString(reader["Role"]);
            double salary = Convert.ToDouble(Convert.ToString(reader["Salary"]));
            bool isPhysicalPerson = Convert.ToBoolean(reader["IsIndividual"]);

            Employee employee = new Employee(id, name, uniqueId, address, phone, email, internalRegistration, loginUsername, userPassword, hiringDate, jobTitle, salary, isPhysicalPerson);

            employee.Id = id;

            return employee;
        }
    }
}
