using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TieredEntity.NestedSet;
using TieredEntity.Web.AppDb;
using TieredEntity.Web.Models;

namespace TieredEntity.Web.Controllers
{
    public class NSDutyPositionController : Controller
    {
        private readonly Db _db;

        public NSDutyPositionController(Db db)
        {
            _db = db;
        }

        private void InitNSDutyPositions()
        {
            var dbset = _db.NsDutyPositions;
            if (dbset.Any(dp => string.Equals(dp.Title, Constants.Topdog)))
            {
                return;
            }
            var nsdps = new List<NSDutyPosition>();
            var top = new NSDutyPosition { Title = Constants.Topdog };
            nsdps.Add(top);
            var east = new NSDutyPosition { Title = Constants.Eastdog };
            nsdps.Add(east);
            var south = new NSDutyPosition { Title = Constants.Southdog };
            nsdps.Add(south);
            var west = new NSDutyPosition { Title = Constants.Westdog };
            nsdps.Add(west);
            var north = new NSDutyPosition { Title = Constants.Northdog };
            nsdps.Add(north);
            var easts = new List<NSDutyPosition>();
            foreach (var e in Constants.Eastsiders)
            {
                var add = new NSDutyPosition { Title = e };
                nsdps.Add(add);
                easts.Add(add);
            }

            var souths = new List<NSDutyPosition>();
            foreach (var s in Constants.Southsiders)
            {
                var add = new NSDutyPosition { Title = s };
                nsdps.Add(add);
                souths.Add(add);
            }

            var wests = new List<NSDutyPosition>();
            foreach (var w in Constants.Eastsiders)
            {
                var add = new NSDutyPosition { Title = w };
                nsdps.Add(add);
                wests.Add(add);
            }

            var norths = new List<NSDutyPosition>();
            foreach (var n in Constants.Eastsiders)
            {
                var add = new NSDutyPosition { Title = n };
                nsdps.Add(add);
                norths.Add(add);
            }
            dbset.AddRange(nsdps);
            _db.SaveChanges();

            var factory = NestedSetStrategyFactory<NSDutyPosition>.Factory();
            NestedSetStrategy<NSDutyPosition> strategy = (NestedSetStrategy<NSDutyPosition>)factory.TierStrategy(dbset);
            top.InitTree(strategy);
            top.Insert(south.VertexId, 1, strategy);
            top.Insert(west.VertexId, 0, strategy);

            foreach (var s in souths)
            {
                south.Insert(s.VertexId, 0, strategy);
            }

            foreach (var w in wests)
            {
                west.Insert(w.VertexId, 0, strategy);
            }

            top.Insert(north.VertexId, 2, strategy);
            top.Insert(east.VertexId, 0, strategy);

            foreach (var n in norths)
            {
                north.Insert(n.VertexId, 0, strategy);
            }

            foreach (var e in easts)
            {
                east.Insert(e.VertexId, 0, strategy);
            }

            _db.SaveChanges();
        }

        public IActionResult List()
        {
            InitNSDutyPositions();
            var dbSet = _db.NsDutyPositions;
            var factory = NestedSetStrategyFactory<NSDutyPosition>.Factory();
            var top = dbSet.FirstOrDefault(t => t.Title == Constants.Topdog);
            NestedSetStrategy<NSDutyPosition>
                strategy = (NestedSetStrategy<NSDutyPosition>)factory.TierStrategy(dbSet);
            var tree = strategy.Hierarchy(top);
            var count = tree.Last().Count;
            List<List<NSDutyPosition>> ret = new List<List<NSDutyPosition>>();
            foreach (var gen in tree)
            {
                List<NSDutyPosition> list = new List<NSDutyPosition>();
                int init = (count - gen.Count) / 2;
                for (int i = 0; i < init; i++)
                {
                    list.Add(null);
                }

                foreach (var t in gen)
                {
                    list.Add(t);
                }
                ret.Add(list);

                for (int i = 0; i < init; i++)
                {
                    list.Add(null);
                }
            }
            return View(ret);
        }
    }
}