using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TieredEntity.NestedSet
{
    public class NestedSetStrategyFactory<TTiered> : ITierStrategyFactory<TTiered> where TTiered : NestedSetTiered
    {
        private static readonly NestedSetStrategyFactory<TTiered> _strategyFactory = new NestedSetStrategyFactory<TTiered>();

        public static NestedSetStrategyFactory<TTiered> Factory()
        {
            return _strategyFactory;
        }

        private NestedSetStrategyFactory()
        {
        }

        public ITierStrategy<TTiered> TierStrategy(DbSet<TTiered> dbSet)
        {
            if (dbSet == null)
            {
                throw new System.Exception("Collection should not be null");
            }
            
            return new NestedSetStrategy<TTiered>(dbSet);
        }
    }
}