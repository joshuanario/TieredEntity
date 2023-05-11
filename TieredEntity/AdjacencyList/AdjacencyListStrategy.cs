namespace TieredEntity.AdjacencyList
{
    public class AdjacencyListStrategy<TTiered> : ITierStrategy<TTiered> where TTiered : AdjacencyListTiered
    {
        private readonly IList<TTiered> _list;
        public AdjacencyListStrategy(IList<TTiered> list) 
        {
            _list = list;
        }

        public TTiered Get(int id)
        {
            return _list.FirstOrDefault(t => t.VertexId == id);
        }

        public IList<TTiered> Seniors(TTiered model)
        {
            List<TTiered> ret = new List<TTiered>();
            TTiered next = (TTiered) model.Next(this);
            while (next != null)
            {
                ret.Add(next);
                next = (TTiered) next.Next(this);
            }

            return ret;
        }

        public IList<ISet<TTiered>> Juniors(TTiered model)
        {
            HashSet<TTiered> CurrToPrev(HashSet<TTiered> currgen)
            {
                TTiered[] arr = _list.Where(t => currgen.Any(c => t.NextId == c.VertexId)).ToArray();
                HashSet<TTiered> prevgen = new HashSet<TTiered>(arr);
                return prevgen;
            }
            List<ISet<TTiered>> ret = new List<ISet<TTiered>>();
            HashSet<TTiered> currset = new HashSet<TTiered>() { model };
            HashSet<TTiered> prevset = CurrToPrev(currset);

            while (prevset.Any())
            {
                ret.Add(prevset);
                prevset = CurrToPrev(prevset);
            }

            return ret;
        }

        public IList<ISet<TTiered>> Hierarchy(TTiered model)
        {
            List<ISet<TTiered>> ret = new List<ISet<TTiered>>();
            IList<TTiered> seniors = Seniors(model);
            foreach (TTiered adjacencyListTiered in seniors)
            {
                ret.Add(new HashSet<TTiered> { adjacencyListTiered });
            }
            ret.Add(new HashSet<TTiered> { model });
            ret.AddRange(Juniors(model));

            return ret;
        }

        public int? TierDistance(TTiered source, TTiered target)
        {
            int TraverseSeniors()
            {
                var seniors = Seniors(source);
                for (int i = 0; i < seniors.Count; i++)
                {
                    var senior = seniors[i];
                    if (senior.VertexId == target.VertexId)
                    {
                        return i + 1;
                    }
                }

                return -1;
            }

            int TraverseJuniors()
            {
                var juniors = Juniors(target);
                for (int i = 0; i < juniors.Count; i++)
                {
                    var junior = juniors[i];
                    if (junior.Any(j => j.VertexId == target.VertexId))
                    {
                        return i + 1;
                    }
                }

                return -1;
            }

            int seniordist = TraverseSeniors(), juniordist = TraverseJuniors();

            if (seniordist > -1) return seniordist;
            if (juniordist > -1) return juniordist;
            return null;
        }
        
        public async Task<int?> TierDistanceAsync(TTiered source, TTiered target)
        {
            // todo add cancellation
            Task<int> TraverseSeniors()
            {
                var seniors = Seniors(source);
                for (int i = 0; i < seniors.Count; i++)
                {
                    var senior = seniors[i];
                    if (senior.VertexId == target.VertexId)
                    {
                        return Task.FromResult(i + 1);
                    }
                }

                return Task.FromResult(-1);
            }

            // todo add cancellation
            Task<int> TraverseJuniors()
            {
                var juniors = Juniors(target);
                for (int i = 0; i < juniors.Count; i++)
                {
                    var junior = juniors[i];
                    if (junior.Any(j => j.VertexId == target.VertexId))
                    {
                        return Task.FromResult(i + 1);
                    }
                }

                return Task.FromResult(-1);
            }

            int seniordist = -1, juniordist = -1;

            var seniortraverse = TraverseSeniors();
            var juniortraverse = TraverseJuniors();
            var traverses = new List<Task> { seniortraverse, juniortraverse };
            Task traversed = await Task.WhenAny(traverses);
            if (traversed == seniortraverse)
            {
                seniordist = await seniortraverse;
            }
            if (traversed == juniortraverse)
            {
                juniordist = await juniortraverse;
            }

            if (seniordist > -1) return seniordist;
            if (juniordist > -1) return juniordist;
            return null;
        }
    }
}