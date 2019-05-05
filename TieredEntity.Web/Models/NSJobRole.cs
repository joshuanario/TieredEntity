using Microsoft.EntityFrameworkCore;
using TieredEntity.NestedSet;

namespace TieredEntity.Web.Models
{
    public class NSJobRole : NestedSetTiered
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<NSJobRole>();

            entity.HasKey(m => m.Id);
        }

        public int Id { get; set; }

        public string Title { get; set; }
        public override int VertexId => Id;
    }
}