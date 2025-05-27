using CarRental.Controllers.Shared;
using CarRental.Domain.CouponModule;
using CarRental.Domain.PartnerModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Controllers.CouponModule
{
    public class CouponController : Controller<Coupon>
    {
        #region queries
        private const string sqlInsertCoupon =
           @"INSERT INTO [Coupon]
                (
                    [CouponName],
                    [Code],      
                    [MinimumValue],
                    [Value], 
                    [IsFixedDiscount],
                    [Validity],                    
                    [PartnerId] 
                )
            VALUES
                (
                    @CouponName,
                    @Code,
                    @MinimumValue,
                    @Value,
                    @IsFixedDiscount,
                    @Validity,
                    @PartnerId
                )";

        private const string sqlEditCoupon =
            @" UPDATE [Coupon]
                SET 
                    [CouponName] = @CouponName, 
                    [Code] = @Code, 
                    [MinimumValue] = @MinimumValue,
                    [Value] = @Value, 
                    [IsFixedDiscount] = @IsFixedDiscount,
                    [Validity] = @Validity,
                    [PartnerId] = @PartnerId
                WHERE [Id] = @Id";

        private const string sqlDeleteCoupon =
            @"DELETE FROM [Coupon] 
                WHERE [Id] = @Id";

        private const string sqlSelectAllCoupons =
            @"SELECT 
                    D.[Id],       
                    D.[CouponName],       
                    D.[Code], 
                    D.[MinimumValue],
                    D.[Value],                    
                    D.[IsFixedDiscount],                                                           
                    D.[Validity],
                    D.[PartnerId],
                    P.[Id] AS PartnerId,
                    P.[PartnerName]
            FROM
                [Coupon] AS D INNER JOIN
                [Partner] AS P
            ON
                D.PartnerId = P.Id";
        private const string sqlSelectCouponById =
            @"SELECT 
                    D.[Id],       
                    D.[CouponName],       
                    D.[Code], 
                    D.[MinimumValue],
                    D.[Value],                    
                    D.[IsFixedDiscount],                                                           
                    D.[Validity],
                    D.[PartnerId],
                    P.[Id] AS PartnerId,
                    P.[PartnerName]
            FROM
                [Coupon] AS D INNER JOIN
                [Partner] AS P
            ON
                D.PartnerId = P.Id
            WHERE 
                D.[Id] = @Id";

        private const string sqlSelectCouponByCode =
            @"SELECT 
                    D.[Id],       
                    D.[CouponName],       
                    D.[Code], 
                    D.[MinimumValue],
                    D.[Value],                    
                    D.[IsFixedDiscount],                                                           
                    D.[Validity],
                    D.[PartnerId],
                    P.[Id] AS PartnerId,
                    P.[PartnerName]
            FROM
                [Coupon] AS D INNER JOIN
                [Partner] AS P
            ON
                D.PartnerId = P.Id
            WHERE 
                D.[Code] = @Code";

        private const string sqlCouponExists =
            @"SELECT 
                COUNT(*) 
            FROM 
                [Coupon]
            WHERE 
                [Id] = @Id";

        private const string sqlCodeExists =
            @"SELECT 
                COUNT(*) 
            FROM 
                [Coupon]
            WHERE 
                [Code] = @Code";
        #endregion
        public override string InsertNew(Coupon coupon)
        {
            string validationResult = coupon.Validate();

            if (validationResult == "VALID")
                coupon.Id = Db.Insert(sqlInsertCoupon, GetCouponParameters(coupon));

            return validationResult;
        }

        public override List<Coupon> SelectAll()
        {
            return Db.GetAll(sqlSelectAllCoupons, ConvertToCoupon);
        }

        public override Coupon? SelectById(int id)
        {
            return Db.Get(sqlSelectCouponById, ConvertToCoupon, AddParameter("Id", id));
        }

        public Coupon? SelectByCode(string code)
        {
            return Db.Get(sqlSelectCouponByCode, ConvertToCoupon, AddParameter("Code", code));
        }

        public override string Edit(int id, Coupon coupon)
        {
            string validationResult = coupon.Validate();

            if (validationResult == "VALID")
            {
                coupon.Id = id;
                Db.Update(sqlEditCoupon, GetCouponParameters(coupon));
            }

            return validationResult;
        }

        public override bool Delete(int id)
        {
            try
            {
                Db.Delete(sqlDeleteCoupon, AddParameter("Id", id));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public override bool Exists(int id)
        {
            return Db.Exists(sqlCouponExists, AddParameter("Id", id));
        }

        public bool CodeExists(string code)
        {
            return Db.Exists(sqlCodeExists, AddParameter("Code", code));
        }

        private Dictionary<string, object> GetCouponParameters(Coupon coupon)
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("Id", coupon.Id);
            parameters.Add("CouponName", coupon.Name);
            parameters.Add("Code", coupon.Code);
            parameters.Add("MinimumValue", coupon.MinimumValue);
            parameters.Add("Value", coupon.Value);
            parameters.Add("IsFixedDiscount", coupon.IsFixedDiscount);
            parameters.Add("Validity", coupon.ExpirationDate);
            parameters.Add("PartnerId", coupon.Partner.Id);

            return parameters;
        }

        private Coupon ConvertToCoupon(IDataReader reader)
        {
            int id = Convert.ToInt32(reader["Id"]);
            string name = Convert.ToString(reader["CouponName"]);
            string code = Convert.ToString(reader["Code"]);
            double minimumValue = Convert.ToDouble(reader["MinimumValue"]);
            double value = Convert.ToDouble(reader["Value"]);            
            bool isFixedDiscount = Convert.ToBoolean(reader["IsFixedDiscount"]);
            DateTime expirationDate = Convert.ToDateTime(reader["Validity"]);

            int partnerId = Convert.ToInt32(reader["PartnerId"]);
            string partnerName = Convert.ToString(reader["PartnerName"]);
            Partner partner = new Partner(partnerId, partnerName);

            Coupon coupon = new Coupon(id, name, code, value, minimumValue, isFixedDiscount, expirationDate, partner);

            coupon.Id = id;

            return coupon;
        }
    }
}
