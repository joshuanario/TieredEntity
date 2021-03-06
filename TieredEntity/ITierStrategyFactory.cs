﻿using Microsoft.EntityFrameworkCore;

namespace TieredEntity
{
    public interface ITierStrategyFactory<TTiered> where TTiered : class, ITiered
    {
        ITierStrategy<TTiered> TierStrategy(DbSet<TTiered> dbSet);
    }
}