using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TieredEntity
{
    public static class DbSetExtensions
    {
        public static IList<TTiered> Seniors<TTiered>(this DbSet<TTiered> dbSet, TTiered model, ITierStrategy<TTiered> strategy)
            where TTiered : class, ITiered
        {
            return strategy.Seniors(model);
        }

        public static IList<ISet<TTiered>> Juniors<TTiered>(this DbSet<TTiered> dbSet, TTiered model, ITierStrategy<TTiered> strategy)
            where TTiered : class, ITiered
        {
            return strategy.Juniors(model);
        }

        public static IList<ISet<TTiered>> Hierarchy<TTiered>(this DbSet<TTiered> dbSet, TTiered model, ITierStrategy<TTiered> strategy)
            where TTiered : class, ITiered
        {
            return strategy.Hierarchy(model);
        }

        public static int? TierDistance<TTiered>(this DbSet<TTiered> dbSet, TTiered source, TTiered target, ITierStrategy<TTiered> strategy)
            where TTiered : class, ITiered
        {
            if (source.GetType() != target.GetType())
            {
                throw new Exception("Source and Target are not the same type.");
            }

            return strategy.TierDistance(source, target);
        }
    }
}