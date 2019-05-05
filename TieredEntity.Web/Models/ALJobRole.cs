using Microsoft.EntityFrameworkCore;
using TieredEntity.AdjacencyList;

namespace TieredEntity.Web.Models
{
    public class ALJobRole : AdjacencyListTiered
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ALJobRole>();

            entity.HasKey(m => m.Id);
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public override int VertexId => Id;
    }
}
