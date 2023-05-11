using Microsoft.AspNetCore.Mvc;
using TieredEntity.Web.AppDb;
using TieredEntity.Web.Models;
using TieredEntity.NestedSet;
using TieredEntity.Web.Extensions;

namespace TieredEntity.Web.Controllers;

[ApiController]
public class NestedSetJobRoleController : ControllerBase
{
    private readonly Db _db;

    public NestedSetJobRoleController(Db db)
    {
        _db = db;
        InitializeNestedJobRoles();
    }
    
    private void InitializeNestedJobRoles()
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
        NestedSetStrategy<NSJobRole> strategy = (NestedSetStrategy<NSJobRole>)factory.TierStrategy(dbset.ToList());
        nsjrs[0].InitTree(strategy);
        for (int i = 1; i < nsjrs.Count ; i++)
        {
            var sup = nsjrs[i - 1];
            var curr = nsjrs[i];
            sup.Insert(curr.VertexId, 0, strategy);
        }
        
        _db.SaveChanges();
    }

    [Route("/api/jobroles/nestedset")]
    [HttpGet]
    public IList<IList<NSJobRole>> Get()
    {
        var dbSet = _db.NsJobRoles;
        var top = dbSet.First(t => t.Title == Constants.JobRoles[0]);
        var tree = dbSet.ToList().Treeify<NSJobRole>(top);
        return tree;
    }
}
