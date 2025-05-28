using CarRental.Controllers.Shared;
using CarRental.Domain.PartnerModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Controllers.PartnerModule
{
    public class PartnerController : Controller<Partner>
    {
        #region queries
        private const string sqlInsertPartner =
            @"INSERT INTO Partner
                (
                    [PartnerName]
                ) 
                VALUES
                (
                    @PartnerName
                )";

        private const string sqlEditPartner =
            @"UPDATE Partner
                    SET
                        [PartnerName] = @PartnerName
                    WHERE 
                        Id = @Id";

        private const string sqlDeletePartner =
            @"DELETE 
                FROM
                        Partner
                    WHERE 
                        Id = @Id";

        private const string sqlSelectPartnerById =
            @"SELECT
                        [Id],
                    [PartnerName]
                FROM
                        Partner
                    WHERE 
                        Id = @Id";

        private const string sqlSelectAllPartners =
            @"SELECT
                        [Id],
                    [PartnerName]
                FROM
                        Partner";

        private const string sqlPartnerExists =
            @"SELECT 
                    COUNT(*) 
                FROM 
                    [Partner]
                WHERE 
                    [Id] = @Id";
        #endregion

        public override string InsertNew(Partner partner)
        {
            string validationResult = partner.Validate();

            if (validationResult == "VALID")
                partner.Id = Db.Insert(sqlInsertPartner, GetPartnerParameters(partner));

            return validationResult;
        }

        public override List<Partner> SelectAll()
        {
            return Db.GetAll(sqlSelectAllPartners, ConvertToPartner);
        }       

        public override Partner? SelectById(int id)
        {
            return Db.Get(sqlSelectPartnerById, ConvertToPartner, AddParameter("Id", id));
        }        
        public override string Edit(int id, Partner partner)
        {
            string validationResult = partner.Validate();

            if (validationResult == "VALID")
            {
                partner.Id = id;
                Db.Update(sqlEditPartner, GetPartnerParameters(partner));
            }

            return validationResult;
        }

        public override bool Delete(int id)
        {
            try
            {
                Db.Delete(sqlDeletePartner, AddParameter("Id", id));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public override bool Exists(int id)
        {
            return Db.Exists(sqlPartnerExists, AddParameter("Id", id));
        }

        private Dictionary<string, object> GetPartnerParameters(Partner partner)
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("Id", partner.Id);
            parameters.Add("PartnerName", partner.Name);

            return parameters;
        }
        private Partner ConvertToPartner(IDataReader reader)
        {
            int id = Convert.ToInt32(reader["Id"]);
            string? name = reader["PartnerName"] != DBNull.Value ? Convert.ToString(reader["PartnerName"]) : null;

            Partner partner = new Partner(id, name);

            partner.Id = id;

            return partner;
        }
    }
}
