using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TieredEntity.AdjacencyList;
using TieredEntity.Web.AppDb;
using TieredEntity.Web.Models;

namespace TieredEntity.Web.Controllers
{
    public class ALJobRoleController : Controller
    {
        private readonly Db _db;

        public ALJobRoleController(Db db)
        {
            _db = db;
        }

        private void InitALDutyPositions()
        {
            var dbset = _db.AlJobRoles;
            if (dbset.Any(dp => string.Equals(dp.Title, Constants.JobRoles[0])))
            {
                return;
            }
            var aljrs = new List<ALJobRole>();
            foreach (var jobRole in Constants.JobRoles)
            {
                aljrs.Add(new ALJobRole { Title = jobRole });
            }
            dbset.AddRange(aljrs);
            _db.SaveChanges();

            var factory = AdjacencyListStrategyFactory<ALJobRole>.Factory();
            AdjacencyListStrategy<ALJobRole> strategy = (AdjacencyListStrategy<ALJobRole>)factory.TierStrategy(_db.AlJobRoles);

            aljrs[0].Next(null, strategy);
            for (int i = 0; i < aljrs.Count - 1; i++)
            {
                var curr = aljrs[i];
                var sub = aljrs[i + 1];
                sub.Next(curr.VertexId, strategy);
            }

            _db.SaveChanges();
        }

        public IActionResult List()
        {
            InitALDutyPositions();
            var dbSet = _db.AlJobRoles;
            var factory = AdjacencyListStrategyFactory<ALJobRole>.Factory();
            var top = dbSet.FirstOrDefault(t => t.Title == Constants.JobRoles[0]);
            AdjacencyListStrategy<ALJobRole> strategy = (AdjacencyListStrategy<ALJobRole>)factory.TierStrategy(dbSet);
            var tree = strategy.Hierarchy(top);
            var count = tree.Last().Count;
            List<List<ALJobRole>> ret = new List<List<ALJobRole>>(tree.Count);
            foreach (var gen in tree)
            {
                List<ALJobRole> list = new List<ALJobRole>();
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