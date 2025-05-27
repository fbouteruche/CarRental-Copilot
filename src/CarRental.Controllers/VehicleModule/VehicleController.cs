using CarRental.Controllers.Shared;
using CarRental.Domain.VehicleGroupModule;
using CarRental.Domain.VehicleImageModule;
using CarRental.Controllers.VehicleImageModule;
using CarRental.Domain.VehicleModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace CarRental.Controllers.VehicleModule
{
    public class VehicleController : Controller<Vehicle>
    {
        private VehicleImageController imageController = new VehicleImageController();
        #region queries
        private const string sqlInsertVehicle =
            @"INSERT INTO Vehicle
            (
                [Model],
                [VehicleGroupId],
                [Plate],
                [Chassis],      
                [Brand], 
                [Color],
                [FuelType],
                [TankCapacity],
                [Year],
                [Mileage],
                [NumberOfDoors],
                [PeopleCapacity],
                [TrunkSize],
                [HasAirConditioning],
                [HasPowerSteering],
                [HasAbsBrakes],
                [IsRented]
            )
            VALUES
            (
                @Model,
                @VehicleGroupId,
                @Plate,
                @Chassis,      
                @Brand,
                @Color,
                @FuelType,
                @TankCapacity,
                @Year,
                @Mileage,
                @NumberOfDoors,
                @PeopleCapacity,
                @TrunkSize,
                @HasAirConditioning,
                @HasPowerSteering,
                @HasAbsBrakes,
                @IsRented
            )";
        private const string sqlSelectAllVehicles =
            @"SELECT
                CV.[Id],
                CV.[Model],
                CV.[VehicleGroupId],
                CV.[Plate],
                CV.[Chassis],      
                CV.[Brand], 
                CV.[Color],
                CV.[FuelType],
                CV.[TankCapacity],
                CV.[Year],
                CV.[Mileage],
                CV.[NumberOfDoors],
                CV.[PeopleCapacity],
                CV.[TrunkSize],
                CV.[HasAirConditioning],
                CV.[HasPowerSteering],
                CV.[HasAbsBrakes],
                CV.[IsRented],
                CG.[Name],
                CG.[DailyPlanRate],
                CG.[DailyKmRate],
                CG.[ControlledPlanRate],
                CG.[ControlledKmLimit],
                CG.[ControlledExceededKmRate],
                CG.[FreePlanRate]
            FROM 
                [Vehicle] AS CV LEFT JOIN 
                [Vehicle_Group] AS CG
            ON
                CG.Id = CV.VehicleGroupId";
        private const string sqlSelectVehicleById =
            @"SELECT  
                CV.[Id],
                CV.[Model],
                CV.[VehicleGroupId],
                CV.[Plate],
                CV.[Chassis],      
                CV.[Brand], 
                CV.[Color],
                CV.[FuelType],
                CV.[TankCapacity],
                CV.[Year],
                CV.[Mileage],
                CV.[NumberOfDoors],
                CV.[PeopleCapacity],
                CV.[TrunkSize],
                CV.[HasAirConditioning],
                CV.[HasPowerSteering],
                CV.[HasAbsBrakes],
                CV.[IsRented],
                CG.[Name],
                CG.[DailyPlanRate],
                CG.[DailyKmRate],
                CG.[ControlledPlanRate],
                CG.[ControlledKmLimit],
                CG.[ControlledExceededKmRate],
                CG.[FreePlanRate]
            FROM 
                [Vehicle] AS CV LEFT JOIN 
                [Vehicle_Group] AS CG
            ON
                CG.Id = CV.VehicleGroupId
            WHERE 
                CV.[Id] = @Id";
        private const string sqlEditVehicle =
            @"UPDATE Vehicle SET
                [Model] = @Model,
                [VehicleGroupId] = @VehicleGroupId,
                [Plate] = @Plate,
                [Chassis] = @Chassis,
                [Brand] = @Brand,
                [Color] = @Color,
                [FuelType] = @FuelType,
                [TankCapacity] = @TankCapacity,
                [Year] = @Year,
                [Mileage] = @Mileage,
                [NumberOfDoors] = @NumberOfDoors,
                [PeopleCapacity] = @PeopleCapacity,
                [TrunkSize] = @TrunkSize,
                [HasAirConditioning] = @HasAirConditioning,
                [HasPowerSteering] = @HasPowerSteering,
                [HasAbsBrakes] = @HasAbsBrakes,
                [IsRented] = @IsRented
            WHERE
                [Id] = @Id
            ";
        private const string sqlDeleteVehicle =
            @"DELETE 
                FROM 
                Vehicle 
            WHERE 
                [Id] = @Id";
        private const string sqlVehicleExists =
            @"SELECT 
                COUNT(*) 
            FROM 
                [Vehicle]
            WHERE 
                [Id] = @Id";

        private const string sqlVehicleTotal =
            @"SELECT COUNT(*) AS QTD FROM[Vehicle]";
        #endregion
        public override string InsertNew(Vehicle vehicle)
        {
            string validationResult = vehicle.Validate();

            if (validationResult == "VALID")
            {
                vehicle.Id = Db.Insert(sqlInsertVehicle, GetVehicleParameters(vehicle));
                if (vehicle.Images != null)
                {
                    foreach (VehicleImage vehicleImage in vehicle.Images)
                    {
                        vehicleImage.VehicleId = vehicle.Id;
                        imageController.InsertNew(vehicleImage);
                    }
                }
            }
            return validationResult;
        }
        public override List<Vehicle> SelectAll()
        {
            List<Vehicle> vehicles = Db.GetAll(sqlSelectAllVehicles, ConvertToVehicle);

            foreach (Vehicle vehicle in vehicles)
            {
                vehicle.Images = imageController.SelectAllImagesOfVehicle(vehicle.Id);
            }

            return vehicles;
        }
        public override Vehicle SelectById(int id)
        {
            Vehicle vehicle = Db.Get(sqlSelectVehicleById, ConvertToVehicle, AddParameter("Id", id));
            vehicle.images = imageController.SelectAllImagesOfVehicle(id);
            return vehicle;
        }
        public override string Edit(int id, Vehicle vehicle)
        {
            string validationResult = vehicle.Validate();

            if (validationResult == "VALID")
            {
                vehicle.Id = id;
                Db.Update(sqlEditVehicle, GetVehicleParameters(vehicle));
                foreach (VehicleImage image in vehicle.images)
                    image.VehicleId = vehicle.Id;
                imageController.EditList(vehicle.images);
            }

            return validationResult;
        }
        public override bool Delete(int id)
        {
            try
            {
                Db.Delete(sqlDeleteVehicle, AddParameter("Id", id));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
     
        public override bool Exists(int id)
        {
            return Db.Exists(sqlVehicleExists, AddParameter("Id", id));
        }

        private Dictionary<string, object> GetVehicleParameters(Vehicle vehicle)
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("Id", vehicle.Id);
            parameters.Add("Model", vehicle.model);
            parameters.Add("VehicleGroupId", vehicle.vehicleGroup.Id);
            parameters.Add("Plate", vehicle.licensePlate);
            parameters.Add("Chassis", vehicle.chassis);
            parameters.Add("Brand", vehicle.brand);
            parameters.Add("Color", vehicle.color);
            parameters.Add("FuelType", vehicle.fuelType);
            parameters.Add("TankCapacity", vehicle.tankCapacity);
            parameters.Add("Year", vehicle.year);
            parameters.Add("Mileage", vehicle.mileage);
            parameters.Add("NumberOfDoors", vehicle.numberOfDoors);
            parameters.Add("PeopleCapacity", vehicle.passengerCapacity);
            parameters.Add("TrunkSize", vehicle.trunkSize);
            parameters.Add("HasAirConditioning", vehicle.hasAirConditioning);
            parameters.Add("HasPowerSteering", vehicle.hasPowerSteering);
            parameters.Add("HasAbsBrakes", vehicle.hasAbsBrakes);
            parameters.Add("IsRented", vehicle.isRented);

            return parameters;
        }

        private Vehicle ConvertToVehicle(IDataReader reader)
        {
            var id = Convert.ToInt32(reader["Id"]);
            var model = Convert.ToString(reader["Model"]);
            var vehicleGroupId = Convert.ToInt32(reader["VehicleGroupId"]);
            var licensePlate = Convert.ToString(reader["Plate"]);
            var chassis = Convert.ToString(reader["Chassis"]);
            var brand = Convert.ToString(reader["Brand"]);
            var color = Convert.ToString(reader["Color"]);
            var fuelType = Convert.ToString(reader["FuelType"]);
            var tankCapacity = Convert.ToDouble(reader["TankCapacity"]);
            var year = Convert.ToInt32(reader["Year"]);
            var mileage = Convert.ToDouble(reader["Mileage"]);
            var numberOfDoors = Convert.ToInt32(reader["NumberOfDoors"]);
            var passengerCapacity = Convert.ToInt32(reader["PeopleCapacity"]);
            var trunkSize = Convert.ToChar(reader["TrunkSize"]);
            var hasAirConditioning = Convert.ToBoolean(reader["HasAirConditioning"]);
            var hasPowerSteering = Convert.ToBoolean(reader["HasPowerSteering"]);
            var hasAbsBrakes = Convert.ToBoolean(reader["HasAbsBrakes"]);
            var isRented = Convert.ToBoolean(reader["IsRented"]);

            string name = Convert.ToString(reader["Name"]);
            double dailyPlanRate = Convert.ToDouble(reader["DailyPlanRate"]);
            double dailyPerKmRate = Convert.ToDouble(reader["DailyKmRate"]);
            double controlledPlanRate = Convert.ToDouble(reader["ControlledPlanRate"]);
            int controlledKmLimit = Convert.ToInt32(reader["ControlledKmLimit"]);
            double controlledExceededKmRate = Convert.ToDouble(reader["ControlledExceededKmRate"]);
            double unlimitedPlanRate = Convert.ToDouble(reader["FreePlanRate"]);

            VehicleGroup group = new VehicleGroup(vehicleGroupId, name, dailyPlanRate, dailyPerKmRate, controlledPlanRate, controlledKmLimit, controlledExceededKmRate, unlimitedPlanRate);

            Vehicle vehicle = new Vehicle(id, model, group, licensePlate, chassis, brand, color, fuelType, tankCapacity, year, mileage, numberOfDoors, passengerCapacity, trunkSize, hasAirConditioning, hasPowerSteering, hasAbsBrakes, isRented, null);

            vehicle.Id = id;

            return vehicle;
        }
        private int ConvertData(IDataReader reader)
        {
            return Convert.ToInt32(reader["QTD"]);
        }
    }
}
