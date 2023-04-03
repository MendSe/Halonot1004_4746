using System.Data.Entity;
using System.Numerics;
using DAL;
using DAL.Entities;


namespace AppServer.Models
{
    public class DSContext : DbContext
    {
        public DSContext() : base("Pannel")
        {
            Database.SetInitializer<DSContext>(new CreateDatabaseIfNotExists<DSContext>());
        }

        public DbSet<Catalogue> Catalogue { get; set; }
        public DbSet<Games> Games { get; set; }
        public DbSet<GPU> GPU { get; set; }
        public DbSet<RAM> RAM { get; set; }
        public DbSet<Servers> Servers { get; set; }
    }
}
