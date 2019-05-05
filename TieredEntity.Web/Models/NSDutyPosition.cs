using Microsoft.EntityFrameworkCore;
using TieredEntity.NestedSet;

namespace TieredEntity.Web.Models
{
    public class NSDutyPosition : NestedSetTiered
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<NSDutyPosition>();

            entity.HasKey(m => m.Id);
        }

        public int Id { get; set; }

        public string Title { get; set; }
        public override int VertexId => Id;
    }
}