using System.Data.Entity;
using System.Numerics;
using DAL.Entities;


namespace DAL.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("Pannel")
        {
            Database.SetInitializer<DBContext>(new CreateDatabaseIfNotExists<DBContext>());
        }

        public DbSet<Catalogue> Catalogue { get; set; }
        public DbSet<Games> Games { get; set; }
        public DbSet<GPU> GPU { get; set; }
        public DbSet<RAM> RAM { get; set; }
        public DbSet<Servers> Servers { get; set; }
    }
}
