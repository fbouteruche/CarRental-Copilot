using CarRental.Controllers.Shared;
using CarRental.Domain.VehicleGroupModule;
using System;
using System.Collections.Generic;
using System.Data;

namespace CarRental.Controllers.VehicleGroupModule
{
    public class VehicleGroupController : Controller<VehicleGroup>
    {
        private const string sqlInsertVehicleGroup =
                @"INSERT INTO Vehicle_Group
                (
                    [Name],
                    [DailyPlanRate],
                    [DailyKmRate],
                    [ControlledPlanRate],
                    [ControlledKmLimit],
                    [ControlledExceededKmRate],
                    [FreePlanRate]
                )
                VALUES
                (
                    @Name,
                    @DailyPlanRate,
                    @DailyKmRate,
                    @ControlledPlanRate,
                    @ControlledKmLimit,
                    @ControlledExceededKmRate,
                    @FreePlanRate
                );";

        private const string sqlUpdateVehicleGroup =
                @"UPDATE Vehicle_Group 
                SET
                    [Name] = @Name,
                    [DailyPlanRate] = @DailyPlanRate,
                    [DailyKmRate] = @DailyKmRate,
                    [ControlledPlanRate] = @ControlledPlanRate,
                    [ControlledKmLimit] = @ControlledKmLimit,
                    [ControlledExceededKmRate] = @ControlledExceededKmRate,
                    [FreePlanRate] = @FreePlanRate
                WHERE [Id] = @Id;";

        private const string sqlDeleteVehicleGroup =
                @"DELETE FROM Vehicle_Group  WHERE [Id] = @Id;";

        private const string sqlSelectVehicleGroupById =
                @"SELECT * FROM Vehicle_Group WHERE [Id] = @Id;";

        private const string sqlSelectAllVehicleGroups =
                @"SELECT * FROM Vehicle_Group;";

        private const string sqlVehicleGroupExists =
                @"SELECT 
                    COUNT(*) 
                FROM 
                    [Vehicle_Group]
                WHERE 
                    [Id] = @Id";

        public override string InsertNew(VehicleGroup record)
        {
            string validationResult = record.Validate();

            List<VehicleGroup> registeredVehicleGroups = SelectAll();
            foreach (VehicleGroup group in registeredVehicleGroups)
            {
                if (record.Name == group.Name)
                    validationResult = "The vehicle group name must be unique\n";
            }

            if (validationResult == "VALID")
            {
                record.Id = Db.Insert(sqlInsertVehicleGroup, GetVehicleGroupParameters(record));
            }

            return validationResult;
        }

        public override string Edit(int id, VehicleGroup record)
        {
            string validationResult = record.Validate();

            List<VehicleGroup> registeredVehicleGroups = SelectAll();
            foreach (VehicleGroup group in registeredVehicleGroups)
            {
                if (id != group.Id && record.Name == group.Name)
                    validationResult = "The vehicle group name must be unique\n";
            }

            if (validationResult == "VALID")
            {
                record.Id = id;
                Db.Update(sqlUpdateVehicleGroup, GetVehicleGroupParameters(record));
            }

            return validationResult;
        }

        public override bool Delete(int id)
        {
            try
            {
                Db.Delete(sqlDeleteVehicleGroup, AddParameter("Id", id));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public override bool Exists(int id)
        {
            return Db.Exists(sqlVehicleGroupExists, AddParameter("Id", id));
        }

        public override VehicleGroup? SelectById(int id)
        {
            return Db.Get(sqlSelectVehicleGroupById, ConvertToVehicleGroup, AddParameter("Id", id));
        }

        public override List<VehicleGroup> SelectAll()
        {
            return Db.GetAll(sqlSelectAllVehicleGroups, ConvertToVehicleGroup);
        }

        private Dictionary<string, object> GetVehicleGroupParameters(VehicleGroup vehicleGroup)
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("Id", vehicleGroup.Id);
            parameters.Add("Name", vehicleGroup.Name);
            parameters.Add("DailyPlanRate", vehicleGroup.DailyPlanRate);
            parameters.Add("DailyKmRate", vehicleGroup.DailyPerKmRate);
            parameters.Add("ControlledPlanRate", vehicleGroup.ControlledPlanRate);
            parameters.Add("ControlledKmLimit", vehicleGroup.ControlledKmLimit);
            parameters.Add("ControlledExceededKmRate", vehicleGroup.ControlledExceededKmRate);
            parameters.Add("FreePlanRate", vehicleGroup.UnlimitedPlanRate);

            return parameters;
        }

        private VehicleGroup ConvertToVehicleGroup(IDataReader reader)
        {
            int id = Convert.ToInt32(reader["Id"]);
            string? name = reader["Name"] != DBNull.Value ? Convert.ToString(reader["Name"]) : string.Empty;
            double dailyPlanRate = Convert.ToDouble(reader["DailyPlanRate"]);
            double dailyPerKmRate = Convert.ToDouble(reader["DailyKmRate"]);
            double controlledPlanRate = Convert.ToDouble(reader["ControlledPlanRate"]);
            int controlledKmLimit = Convert.ToInt32(reader["ControlledKmLimit"]);
            double controlledExceededKmRate = Convert.ToDouble(reader["ControlledExceededKmRate"]);
            double unlimitedPlanRate = Convert.ToDouble(reader["FreePlanRate"]);

            VehicleGroup vehicleGroup = new VehicleGroup(id, name, dailyPlanRate, dailyPerKmRate, controlledPlanRate,
                controlledKmLimit, controlledExceededKmRate, unlimitedPlanRate);

            return vehicleGroup;
        }
    }
}
