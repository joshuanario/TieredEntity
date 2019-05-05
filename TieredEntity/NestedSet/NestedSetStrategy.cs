using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TieredEntity.NestedSet
{
    public class NestedSetStrategy<TTiered> : ITierStrategy<TTiered> where TTiered : NestedSetTiered
    {
        private readonly DbSet<TTiered> _dbSet;
        public NestedSetStrategy(DbSet<TTiered> dbSet)
        {
            _dbSet = dbSet;
        }

        public TTiered Get(int id)
        {
            return _dbSet.FirstOrDefault(t => t.VertexId == id);
        }

        public IList<TTiered> Seniors(TTiered model)
        {
            TTiered[] arr = _dbSet.Where(t => model.IsBetween(t) || t.TreeId == null)
                .OrderBy(t => -t.Left).ToArray().Where(model.IsBetween).ToArray();
            if (!arr.Any()) return new List<TTiered>();

            return new List<TTiered>(arr);
        }

        public IList<ISet<TTiered>> Juniors(TTiered model)
        {
            TTiered[] arr = _dbSet.Where(t => model.IsNesting(t) || t.TreeId == null)
                .OrderBy(t => t.Left).ToArray().Where(model.IsBetween).ToArray();
            if (!arr.Any()) return new List<ISet<TTiered>>();

            // modified pre-ordered tree traversal algorithm
            int[] genarr = new int[arr.Length];
            int gen = 0, maxgen = 0;
            SortedSet<int> traversed = new SortedSet<int>();

            for (int i = 0; i < arr.Length; i++)
            {
                var curr = arr[i];
                traversed.Add(curr.Right);
                genarr[i] = gen;
                if (!curr.IsALeaf)
                {
                    gen++;
                }
                else if (traversed.Contains(curr.Right + 1))
                {
                    gen--;
                }

                if (maxgen < gen)
                {
                    maxgen = gen;
                }
            }

            List<ISet<TTiered>> ret = new List<ISet<TTiered>>();
            for (int i = 0; i < maxgen + 1; i++)
            {
                ret.Add(new SortedSet<TTiered>());
            }

            for (int i = 0; i < arr.Length; i++)
            {
                var junior = arr[i];
                gen = genarr[i];
                ret[gen].Add(junior);
            }
            return ret;
        }

        public IList<ISet<TTiered>> Hierarchy(TTiered model)
        {
            List<ISet<TTiered>> ret = new List<ISet<TTiered>>();
            foreach (var senior in Seniors(model))
            {
                ret.Add(new SortedSet<TTiered> {senior});
            }
            ret.Add(new HashSet<TTiered> { model });
            ret.AddRange(Juniors(model));

            return ret;
        }

        public int? TierDistance(TTiered source, TTiered target)
        {
            if (source.IsBetween(target))
            {
                var seniors = Seniors(source);
                int i = seniors.IndexOf(target);
                return i < 0 ? (int?) null : i;
            }

            if (!source.IsNesting(target)) return null;
            var juniors = Juniors(source);
            for (int j = 0; j < juniors.Count; j++)
            {
                var gen = juniors[j];
                if (gen.Contains(target))
                {
                    return j;
                }
            }

            return null;
        }

        public void AddLeaf(TTiered target, int id, int index)
        {
            var source = Get(id);
            if (target?.TreeId == null || source == null) return;
            int magic = target.Left;
            if (index > 0)
            {
                var juniors = Juniors(target);
                if (juniors.Any())
                {
                    var set = (SortedSet<NestedSetTiered>)juniors[0];
                    var gen = new NestedSetTiered[set.Count];
                    set.CopyTo(gen);
                    var older = gen.Length <= index ? gen.Last() : gen[index - 1];
                    magic = older.Right;
                }
            }
            bool HasMagic(NestedSetTiered t)
            {
                return t.Left > magic || t.Right > magic;
            }

            void DoMagic(NestedSetTiered t)
            {
                t.Left = t.Left > magic ? t.Left + 2 : t.Left;
                t.Right = t.Right > magic ? t.Right + 2 : t.Right;
            }

            var treeid = target?.TreeId;
            NestedSetTiered[] arr = _dbSet.Where(t => t.TreeId == treeid || t.TreeId == null)
                .OrderBy(t => t.Left).ToArray().Where(t => t.TreeId == treeid).ToArray();

            // modified pre-ordered tree traversal algorithm
            foreach (var t in arr)
            {
                t.TreeId = treeid;
                if (HasMagic(t))
                {
                    DoMagic(t);
                }
            }

            source.Left = magic + 1;
            source.Right = magic + 2;
            source.TreeId = treeid;
        }

        public void PruneJuniors(TTiered model)
        {
            if (model.TreeId == null) return;
            var treeid = model.TreeId;
            NestedSetTiered[] arr = _dbSet.Where(t => t.TreeId == treeid || t.TreeId == null)
                .OrderBy(t => t.Left).ToArray().Where(t => t.TreeId == treeid).ToArray();
            if (!arr.Any()) return;

            // modified pre-ordered tree traversal algorithm
            var root = arr[0];
            treeid = root.TreeId == model.VertexId ?
                root.VertexId : root.TreeId;
            int magic = model.Right;
            bool WillKeep(NestedSetTiered t)
            {
                return t.Left > magic || t.Right > magic;
            }
            void Preserve(NestedSetTiered t)
            {
                t.TreeId = treeid;
                int diff = t.Right - t.Left + 1;
                t.Left = t.Left > magic ? t.Left - diff : t.Left;
                t.Right = t.Right > magic ? t.Right - diff : t.Right;
            }

            bool WillPrune(NestedSetTiered t)
            {
                return model.IsNesting(t);
            }

            foreach (var t in arr)
            {
                if (WillKeep(t))
                {
                    Preserve(t);
                }
                else if (WillPrune(t))
                {
                    t.LeaveTree<TTiered>();
                }
            }
        }
    }
}