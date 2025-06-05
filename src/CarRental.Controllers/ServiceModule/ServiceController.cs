using CarRental.Controllers.Shared;
using CarRental.Domain.ServiceModule;
using System;
using System.Collections.Generic;
using System.Data;

namespace CarRental.Controllers.ServiceModule
{
    public class ServiceController : Controller<Service>
    {
        #region queries
        private const string sqlInsertService =
            @"INSERT INTO Service
            (
                [Name],
                [IsDailyCharged],
                [Value]
            )
            VALUES
            (
                @Name,
                @IsDailyCharged,
                @Value
            )";
        private const string sqlSelectAllServices =
            @"SELECT 
                [Id],
                [Name],
                [IsDailyCharged],
                [Value]
            FROM 
                Service ORDER BY Id;";

        private const string sqlSelectServiceById =
            @"SELECT  
                [Id],
                [Name],
                [IsDailyCharged],
                [Value]
            FROM
                Service 
            WHERE 
                [Id] = @Id";

        private const string sqlEditService =
            @"UPDATE Service SET
                [Name] = @Name,
                [IsDailyCharged] = @IsDailyCharged,
                [Value] = @Value
            WHERE
                [Id] = @Id
            ";
        private const string sqlDeleteService =
            @"DELETE 
                FROM 
                Service 
            WHERE 
                [Id] = @Id";

        private const string sqlServiceExists =
            @"SELECT 
                COUNT(*) 
            FROM 
                [Service]
            WHERE 
                [Id] = @Id";
        #endregion

        public override string InsertNew(Service registro)
        {
            string resultadoValidacao = registro.Validate();

            if (resultadoValidacao == "VALID")
                registro.Id = Db.Insert(sqlInsertService, GetServiceParameters(registro));

            return resultadoValidacao;
        }
        public override List<Service> SelectAll()
        {
            return Db.GetAll(sqlSelectAllServices, ConvertToService);
        }
        public override Service? SelectById(int id)
        {
            return Db.Get(sqlSelectServiceById, ConvertToService, AddParameter("Id", id));
        }
        public override string Edit(int id, Service registro)
        {
            string resultadoValidacao = registro.Validate();

            if (resultadoValidacao == "VALID")
            {
                registro.Id = id;
                Db.Update(sqlEditService, GetServiceParameters(registro));
            }

            return resultadoValidacao;
        }
        public override bool Delete(int id)
        {
            try
            {
                Db.Delete(sqlDeleteService, AddParameter("Id", id));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public override bool Exists(int id)
        {
            return Db.Exists(sqlServiceExists, AddParameter("Id", id));
        }

        private Dictionary<string, object> GetServiceParameters(Service servico)
        {
            var parametros = new Dictionary<string, object>();

            parametros.Add("Id", servico.Id);
            parametros.Add("Name", servico.Name);
            parametros.Add("IsDailyCharged", servico.IsChargedDaily);
            parametros.Add("Value", servico.Value);

            return parametros;
        }
        
        private Service ConvertToService(IDataReader reader)
        {
            int id = Convert.ToInt32(reader["Id"]);
            string? name = reader["Name"] != DBNull.Value ? Convert.ToString(reader["Name"]) : string.Empty;
            bool isDailyCharged = Convert.ToBoolean(reader["IsDailyCharged"]);
            double value = Convert.ToDouble(reader["Value"]);

            Service servico = new Service(id, name, isDailyCharged, value);

            servico.Id = id;

            return servico;
        }
    }
}
