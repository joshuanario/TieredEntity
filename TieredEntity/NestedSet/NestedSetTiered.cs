namespace TieredEntity.NestedSet
{
    public abstract class NestedSetTiered : ITiered
    {
        public abstract int VertexId { get; }

        public int? TreeId { get; set; }

        public int Left { get; set; }

        public int Right { get; set; }

        public bool IsBetween(NestedSetTiered target) => 
            target.TreeId != null && target.VertexId != VertexId &&
            target.TreeId == TreeId &&
            Left > target.Left && target.Left > Right;

        public bool IsNesting(NestedSetTiered target) =>
            target.TreeId != null && target.VertexId != VertexId &&
            target.TreeId == TreeId &&
            Left < target.Left && target.Left < Right;

        public bool IsALeaf => Left + 1 == Right;

        public int InitTree<TTiered>(NestedSetStrategy<TTiered> strategy = null) where TTiered : NestedSetTiered
        {
            strategy?.PruneJuniors((TTiered) this);

            TreeId = VertexId;
            Left = 0;
            Right = 1;

            return TreeId.Value;
        }

        public void LeaveTree<TTiered>(NestedSetStrategy<TTiered> strategy = null) where TTiered : NestedSetTiered
        {
            strategy?.PruneJuniors((TTiered) this);

            InitTree(strategy);
        }

        public int Insert<TTiered>(int id, int index, NestedSetStrategy<TTiered> strategy) where TTiered : NestedSetTiered
        {
            strategy.AddLeaf((TTiered)this,id, index);
            return TreeId.Value;
        }
    }
}
