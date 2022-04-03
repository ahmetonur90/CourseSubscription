using CourseSubscription.Entity.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseSubscription.Data.Repository
{
    public class EFContext : DbContext
    {
        #region TABLES
        public DbSet<COURSE> COURSE { get; set; }
        public DbSet<TRAINING> TRAINING { get; set; }
        public DbSet<SUBSCRIPTION> SUBSCRIPTION { get; set; }

        #endregion

        #region STORED PROCEDURES
        public DbSet<Sp_UserSubscription> Sp_UserSubscription { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build();
            optionsBuilder.UseSqlServer(configuration.GetSection("ConnectionStrings:MyConn").Value);
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
        }

        public Int64 NextValueForSequence(string seq)
        {
            var p = new SqlParameter("@result", System.Data.SqlDbType.BigInt);
            p.Direction = System.Data.ParameterDirection.Output;
            this.Database.ExecuteSqlRaw("set @result = next value for " + seq, p);
            var nextVal = (Int64)p.Value;
            return nextVal;
        }

        public List<Sp_UserSubscription> GetUserSubscriptions(decimal usr_auto_key)
        {
            List<Sp_UserSubscription> lst = new List<Sp_UserSubscription>();
            lst = this.Sp_UserSubscription.FromSqlInterpolated($"Sp_UserSubscription @UsrAutoKey = {usr_auto_key}").ToList();

            return lst;
        }

    }

}
