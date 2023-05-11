using TieredEntity.NestedSet;

namespace TieredEntity.Web.Extensions;

public static class NestedSetExtension
{
    public static IList<IList<T>> Treeify<T>(this IList<T> list, T top) where T : NestedSetTiered
    {
        var factory = NestedSetStrategyFactory<T>.Factory();
        NestedSetStrategy<T> strategy = (NestedSetStrategy<T>)factory.TierStrategy(list);
        var tree = strategy.Hierarchy(top);
        var count = tree.Last().Count;
        IList<IList<T>> ret = new List<IList<T>>(tree.Count);
        foreach (var gen in tree)
        {
            List<T> mylist = new List<T>();
            int init = (count - gen.Count) / 2;
            for (int i = 0; i < init; i++)
            {
                mylist.Add(null);
            }

            foreach (var t in gen)
            {
                mylist.Add(t);
            }

            for (int i = 0; i < init; i++)
            {
                mylist.Add(null);
            }
            ret.Add(mylist);
        }
        return ret;
    }
}