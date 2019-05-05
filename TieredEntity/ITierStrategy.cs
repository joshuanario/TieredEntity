using System.Collections.Generic;

namespace TieredEntity
{
    public interface ITierStrategy<TTiered> where TTiered : class, ITiered
    {
        TTiered Get(int id);

        IList<TTiered> Seniors(TTiered model);

        IList<ISet<TTiered>> Juniors(TTiered model);

        IList<ISet<TTiered>> Hierarchy(TTiered model);

        int? TierDistance(TTiered source, TTiered target);
    }
}