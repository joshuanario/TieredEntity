using Microsoft.EntityFrameworkCore;
using TieredEntity.Web.Models;

namespace TieredEntity.Web.AppDb
{
    public class Db : DbContext
    {
        public const string NAME = "TIEREDENTITY.DB";

        public Db(DbContextOptions<Db> options)
            : base(options)
        { }
        
        public DbSet<ALJobRole> AlJobRoles { get; set; }
        public DbSet<ALDutyPosition> AlDutyPositions { get; set; }
        public DbSet<NSJobRole> NsJobRoles { get; set; }
        public DbSet<NSDutyPosition> NsDutyPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ALJobRole.OnModelCreating(modelBuilder);
            ALDutyPosition.OnModelCreating(modelBuilder);
            NSJobRole.OnModelCreating(modelBuilder);
            NSDutyPosition.OnModelCreating(modelBuilder);
        }
    }
}