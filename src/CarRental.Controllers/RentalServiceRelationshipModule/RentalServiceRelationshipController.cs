using CarRental.Controllers.CustomersModule;
using CarRental.Controllers.CouponModule;
using CarRental.Controllers.EmployeeModule;
using CarRental.Controllers.RentalModule;
using CarRental.Controllers.ServiceModule;
using CarRental.Controllers.Shared;
using CarRental.Controllers.VehicleModule;
using CarRental.Domain.RentalModule;
using CarRental.Domain.RentalServiceRelationshipModule;
using CarRental.Domain.ServiceModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Controllers.RentalServiceRelationshipModule
{
    [SupportedOSPlatform("windows")]
    public class RentalServiceRelationshipController : Controller<RentalServiceRelationship>
    {
        private int id = 0;
        ServiceController serviceController = new ServiceController();
        RentalController rentalController = new RentalController(new VehicleController(), new EmployeeController(), new CustomerController(), new ServiceController(), new CouponController());
        #region relationship queries
        private const string sqlInsertRelationship =
                @"INSERT INTO [Service_Rental]
                (
                    [RentalId],
                    [ServiceId]
                )
                VALUES
                (
                    @RentalId,
                    @ServiceId
                );";

        private const string sqlEditRelationship =
        @"UPDATE [Service_Rental] 
                SET
                    [RentalId] = @RentalId,
                    [ServiceId] = @ServiceId
                WHERE 
                    [Id] = @Id;";

        private const string sqlSelectAllRelationships =
            @"SELECT * FROM [Service_Rental];";

        private const string sqlSelectRelationshipById =
            @"SELECT * FROM [Service_Rental] WHERE [Id] = @Id;";

        private const string sqlSelectRelationshipByRental =
            @"SELECT * FROM [Service_Rental] WHERE [RentalId] = @RentalId;";

        private const string sqlDeleteRelationship =
            @"DELETE FROM [Service_Rental] WHERE [Id] = @Id;";

        #endregion
        public override string Edit(int id, RentalServiceRelationship record)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            try
            {
                Db.Delete(sqlDeleteRelationship, AddParameter("Id", id));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public override bool Exists(int id)
        {
            return Db.Exists(sqlSelectRelationshipById, AddParameter("Id", id));
        }

        public override string InsertNew(RentalServiceRelationship record)
        {
            string validationResult = record.Validate();

            if (validationResult == "VALID")
                foreach (Service service in record.Services)
                {
                    id = service.Id;
                    record.Id = Db.Insert(sqlInsertRelationship, GetRelationshipParameters(record));
                }

            return validationResult;
        }

        public override RentalServiceRelationship? SelectById(int id)
        {
            return Db.Get(sqlSelectRelationshipById, ConvertToRelationship, AddParameter("Id", id));
        }

        public object SelectByRental(int rentalId)
        {
            return Db.GetAll(sqlSelectRelationshipByRental, ConvertToRelationship, AddParameter("RentalId", rentalId));
        }

        public override List<RentalServiceRelationship> SelectAll()
        {
            return Db.GetAll(sqlSelectAllRelationships, ConvertToRelationship);
        }
        private RentalServiceRelationship ConvertToRelationship(IDataReader reader)
        {
            var relationshipId = Convert.ToInt32(reader["Id"]);
            var rentalId = Convert.ToInt32(reader["RentalId"]);
            var serviceId = Convert.ToInt32(reader["ServiceId"]);

            List<Service> filteredServices = new List<Service>();
            foreach (Service item in serviceController.SelectAll())
                if (item.Id == serviceId)
                    filteredServices.Add(item);
            Rental rental = rentalController.SelectById(rentalId);

            return new RentalServiceRelationship(relationshipId, rental, filteredServices);
        }
        private Dictionary<string, object> GetRelationshipParameters(RentalServiceRelationship relationship)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Id", relationship.Id);
            parameters.Add("RentalId", relationship.Rental.Id);
            parameters.Add("ServiceId", id);

            return parameters;
        }
    }
}
