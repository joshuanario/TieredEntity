namespace TieredEntity.AdjacencyList
{
    public class AdjacencyListStrategyFactory<TTiered> : ITierStrategyFactory<TTiered> where TTiered : AdjacencyListTiered
    {
        private static readonly AdjacencyListStrategyFactory<TTiered> _strategyFactory = new AdjacencyListStrategyFactory<TTiered>();

        public static AdjacencyListStrategyFactory<TTiered> Factory()
        {
            return _strategyFactory;
        }

        private AdjacencyListStrategyFactory()
        {
        }

        public ITierStrategy<TTiered> TierStrategy(IList<TTiered> list)
        {
            if (list == null)
            {
                throw new System.Exception("Collection should not be null");
            }
            
            return new AdjacencyListStrategy<TTiered>(list);
        }
    }
}