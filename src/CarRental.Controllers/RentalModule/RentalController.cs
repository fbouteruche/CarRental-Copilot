using CarRental.Controllers.CustomersModule;
using CarRental.Controllers.CouponModule;
using CarRental.Controllers.EmployeeModule;
using CarRental.Controllers.ServiceModule;
using CarRental.Controllers.Shared;
using CarRental.Controllers.VehicleModule;
using CarRental.Domain.CustomerModule;
using CarRental.Domain.CouponModule;
using CarRental.Domain.EmployeeModule;
using CarRental.Domain.RentalModule;
using CarRental.Domain.ServiceModule;
using CarRental.Domain.VehicleModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Versioning;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Controllers.RentalModule
{
    [SupportedOSPlatform("windows")]
    public class RentalController : Controller<Rental>
    {
        private VehicleController? vehicleController = null;
        private EmployeeController? employeeController = null;
        private CustomerController? customerController = null;
        private ServiceController? serviceController = null;
        private CouponController couponController = new CouponController();

        public RentalController(VehicleController vehicleController, EmployeeController employeeController, CustomerController customerController, ServiceController serviceController, CouponController couponController)
        {
            this.vehicleController = vehicleController;
            this.employeeController = employeeController;
            this.customerController = customerController;
            this.serviceController = serviceController;
            //this.couponController = couponController; // If you want to use a different couponController instance, uncomment this line.
        }

        #region queries
        private const string sqlInsertRental =
                @"INSERT INTO [Rental]
                (
                    [VehicleId],
                    [EmployeeId],
                    [ContractingClientId],
                    [DriverClientId],
                    [CouponId],
                    [DepartureDate],
                    [ExpectedReturnDate],
                    [ReturnDate],
                    [PlanType],
                    [InsuranceType],
                    [RentalPrice],
                    [ReturnPrice],
                    [IsOpen]
                )
                VALUES
                (
                    @VehicleId,
                    @EmployeeId,
                    @ContractingClientId,
                    @DriverClientId,
                    @CouponId,
                    @DepartureDate,
                    @ExpectedReturnDate,
                    @ReturnDate,
                    @PlanType,
                    @InsuranceType,
                    @RentalPrice,
                    @ReturnPrice,
                    @IsOpen
                );";

        private const string sqlUpdateRental =
        @"UPDATE [Rental] 
                SET
                    [VehicleId] = @VehicleId,
                    [EmployeeId] = @EmployeeId,
                    [ContractingClientId] = @ContractingClientId,
                    [DriverClientId] = @DriverClientId,
                    [CouponId] = @CouponId,
                    [DepartureDate] = @DepartureDate,
                    [ExpectedReturnDate] = @ExpectedReturnDate,
                    [ReturnDate] = @ReturnDate,
                    [PlanType] = @PlanType,
                    [InsuranceType] = @InsuranceType,
                    [RentalPrice] = @RentalPrice,
                    [ReturnPrice] = @ReturnPrice,
                    [IsOpen] = @IsOpen
                WHERE 
                    [Id] = @Id;";

        private const string sqlSelectAllRentals =
            @"SELECT * FROM [Rental];";

        private const string sqlSelectRentalById =
            @"SELECT * FROM [Rental] WHERE [Id] = @Id;";

        private const string sqlDeleteRental =
                @"DELETE FROM [Rental] WHERE [Id] = @Id;";

        private string sqlSelectServiceIdByRentalId =
            @"SELECT [ServiceId] FROM [Service_Rental]
               WHERE [RentalId] = @RentalId";
        #endregion

        public override string InsertNew(Rental record)
        {
            string validationResult = record.Validate();

            if (validationResult == "VALID")
                record.Id = Db.Insert(sqlInsertRental, GetRentalParameters(record));

            return validationResult;
        }
        public override List<Rental> SelectAll()
        {
            return Db.GetAll(sqlSelectAllRentals, ConvertToRental);
        }
        public override Rental? SelectById(int id)
        {
            return Db.Get(sqlSelectRentalById, ConvertToRental, AddParameter("Id", id));
        }

        private List<Service> SelectServicesByRentalId(int rentalId)
        {
            List<Service> rentalServices = new List<Service>();
            List<int> serviceIds = Db.GetAll(sqlSelectServiceIdByRentalId, ConvertToInt, AddParameter("RentalId", rentalId));
            foreach (int serviceId in serviceIds)
            {
                Service? service = serviceController?.SelectById(serviceId);
                if (service != null)
                {
                    rentalServices.Add(service);
                }
            }
            return rentalServices;
        }

        public override string Edit(int id, Rental record)
        {
            string validationResult = record.Validate();

            if (validationResult == "VALID")
            {
                record.Id = id;
                Db.Update(sqlUpdateRental, GetRentalParameters(record));
            }

            return validationResult;
        }
        public override bool Delete(int id)
        {
            try
            {
                Db.Delete(sqlDeleteRental, AddParameter("Id", id));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public override bool Exists(int id)
        {
            return Db.Exists(sqlSelectRentalById, AddParameter("Id", id));
        }

        private Dictionary<string, object> GetRentalParameters(Rental rental)
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("Id", rental.Id);
            parameters.Add("VehicleId", rental.Vehicle.Id);
            parameters.Add("EmployeeId", rental.RentingEmployee.Id);
            parameters.Add("ContractingClientId", rental.ContractingCustomer.Id);
            parameters.Add("DriverClientId", rental.DriverCustomer.Id);
            if (rental.Coupon != null)
                parameters.Add("CouponId", rental.Coupon.Id);
            else
                parameters.Add("CouponId", DBNull.Value);
            parameters.Add("DepartureDate", rental.DepartureDate);
            parameters.Add("ExpectedReturnDate", rental.ExpectedReturnDate);
            parameters.Add("ReturnDate", rental.ReturnDate);
            parameters.Add("PlanType", rental.PlanType);
            parameters.Add("InsuranceType", rental.InsuranceType);
            parameters.Add("RentalPrice", rental.RentalPrice);
            parameters.Add("ReturnPrice", rental.ReturnPrice);
            parameters.Add("IsOpen", rental.IsOpen);
            return parameters;
        }

        private int ConvertToInt(IDataReader reader)
        {
            return Convert.ToInt32(reader["ServiceId"]);
        }

        private Rental ConvertToRental(IDataReader reader)
        {
            var id = Convert.ToInt32(reader["Id"]);
            var vehicleId = Convert.ToInt32(reader["VehicleId"]);
            var employeeId = Convert.ToInt32(reader["EmployeeId"]);
            var contractingClientId = Convert.ToInt32(reader["ContractingClientId"]);
            var driverClientId = Convert.ToInt32(reader["DriverClientId"]);
            var couponId = 0;
            if (reader["CouponId"] != DBNull.Value)
                couponId = Convert.ToInt32(reader["CouponId"]);
            var departureDate = Convert.ToDateTime(reader["DepartureDate"]);
            var expectedReturnDate = Convert.ToDateTime(reader["ExpectedReturnDate"]);
            var returnDate = Convert.ToDateTime(reader["ReturnDate"]);
            var planType = Convert.ToString(reader["PlanType"]) ?? string.Empty;
            var insuranceType = Convert.ToString(reader["InsuranceType"]) ?? string.Empty;
            var rentalPrice = Convert.ToDouble(reader["RentalPrice"]);
            var returnPrice = Convert.ToDouble(reader["ReturnPrice"]);
            var isOpen = Convert.ToBoolean(reader["IsOpen"]);

            List<Service> rentalServices = SelectServicesByRentalId(id);

            Vehicle? vehicle = vehicleController?.SelectById(vehicleId);
            Employee? rentingEmployee = employeeController?.SelectById(employeeId);
            Customer? contractingCustomer = customerController?.SelectById(contractingClientId);
            Customer? driverCustomer = customerController?.SelectById(driverClientId);
            Coupon? coupon = null;
            if (couponId != 0)
                coupon = couponController.SelectById(couponId);

            if (vehicle == null || rentingEmployee == null || contractingCustomer == null || driverCustomer == null)
            {
                throw new ArgumentException("Required entity not found in the database");
            }

            return new Rental(id, vehicle, rentingEmployee, contractingCustomer, driverCustomer, coupon, departureDate, expectedReturnDate, returnDate, planType, insuranceType, rentalPrice, returnPrice, isOpen, rentalServices);
        }
    }
}
