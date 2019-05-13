using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TieredEntity.NestedSet;
using TieredEntity.Web.AppDb;
using TieredEntity.Web.Models;

namespace TieredEntity.Web.Controllers
{
    public class NSJobRoleController : Controller
    {
        private readonly Db _db;

        public NSJobRoleController(Db db)
        {
            _db = db;
        }

        private void InitNSDutyPositions()
        {
            var dbset = _db.NsJobRoles;
            if (dbset.Any(dp => string.Equals(dp.Title, Constants.JobRoles[0])))
            {
                return;
            }
            var nsjrs = new List<NSJobRole>();

            foreach (var jobRole in Constants.JobRoles)
            {
                nsjrs.Add(new NSJobRole { Title = jobRole });
            }
            dbset.AddRange(nsjrs);
            _db.SaveChanges();

            var factory = NestedSetStrategyFactory<NSJobRole>.Factory();
            NestedSetStrategy<NSJobRole> strategy = (NestedSetStrategy<NSJobRole>)factory.TierStrategy(dbset);
            nsjrs[0].InitTree(strategy);
            for (int i = 1; i < nsjrs.Count ; i++)
            {
                var sup = nsjrs[i - 1];
                var curr = nsjrs[i];
                sup.Insert(curr.VertexId, 0, strategy);
            }
            
            _db.SaveChanges();
        }

        public IActionResult List()
        {
            InitNSDutyPositions();
            var dbSet = _db.NsJobRoles;
            var factory = NestedSetStrategyFactory<NSJobRole>.Factory();
            var top = dbSet.FirstOrDefault(t => t.Title == Constants.JobRoles[0]);
            NestedSetStrategy<NSJobRole>
                strategy = (NestedSetStrategy<NSJobRole>)factory.TierStrategy(dbSet);
            var tree = strategy.Hierarchy(top);
            var count = tree.Last().Count;
            List<List<NSJobRole>> ret = new List<List<NSJobRole>>();
            foreach (var gen in tree)
            {
                List<NSJobRole> list = new List<NSJobRole>();
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