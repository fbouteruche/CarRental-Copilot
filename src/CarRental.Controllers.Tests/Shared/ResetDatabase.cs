using CarRental.Controllers.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Controllers.Tests.Shared
{
    public static class ResetDatabase
    {    
        public static void ResetAllTables()
        {
            // Delete in order that respects foreign key constraints
            // Start with child tables and work up to parent tables
            Db.Update("DELETE FROM [Service_Rental]");
            Db.Update("DELETE FROM [Rental]");
            Db.Update("DELETE FROM [Vehicle_Image]");
            Db.Update("DELETE FROM [Vehicle]");
            Db.Update("DELETE FROM [Vehicle_Group]");
            Db.Update("DELETE FROM [Employee]");
            Db.Update("DELETE FROM [Service]");            
            Db.Update("DELETE FROM [Customer]");
            Db.Update("DELETE FROM [Coupon]");
            Db.Update("DELETE FROM [Partner]");            

            // Reset identity columns
            Db.Update("DBCC CHECKIDENT('Service_Rental', RESEED, 0)");
            Db.Update("DBCC CHECKIDENT('Rental', RESEED, 0)");
            Db.Update("DBCC CHECKIDENT('Vehicle_Image', RESEED, 0)");
            Db.Update("DBCC CHECKIDENT('Vehicle', RESEED, 0)");
            Db.Update("DBCC CHECKIDENT('Vehicle_Group', RESEED, 0)");
            Db.Update("DBCC CHECKIDENT('Employee', RESEED, 0)");
            Db.Update("DBCC CHECKIDENT('Service', RESEED, 0)");
            Db.Update("DBCC CHECKIDENT('Customer', RESEED, 0)");
            Db.Update("DBCC CHECKIDENT('Coupon', RESEED, 0)");
            Db.Update("DBCC CHECKIDENT('Partner', RESEED, 0)");
        }
    }
}
