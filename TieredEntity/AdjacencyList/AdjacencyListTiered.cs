namespace TieredEntity.AdjacencyList
{
    public abstract class AdjacencyListTiered : ITiered
    {
        public abstract int VertexId { get; }

        public int? NextId { get; set; }

        private AdjacencyListTiered _next;

        public AdjacencyListTiered Next<TTiered>(AdjacencyListStrategy<TTiered> strategy) where TTiered : AdjacencyListTiered
        {
            if (NextId == null) return null;
            if (_next?.VertexId != NextId) return _next = strategy.Get(NextId.Value);
            return _next;
        }

        public AdjacencyListTiered Next<TTiered>(int? id, AdjacencyListStrategy<TTiered> strategy) where TTiered : AdjacencyListTiered
        {
            if (id == null) return null;
            _next = strategy.Get(id.Value);
            NextId = _next?.VertexId;
            return _next;
        }
    }
}
