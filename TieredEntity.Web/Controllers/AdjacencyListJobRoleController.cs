using Microsoft.AspNetCore.Mvc;
using TieredEntity.Web.AppDb;
using TieredEntity.Web.Models;
using TieredEntity.AdjacencyList;
using TieredEntity.Web.Extensions;

namespace TieredEntity.Web.Controllers;

[ApiController]
public class AdjacencyListJobRoleController : ControllerBase
{
    private readonly Db _db;

    public AdjacencyListJobRoleController(Db db)
    {
        _db = db;
        InitializeAdjacencyListJobRoles();
    }
    
    private void InitializeAdjacencyListJobRoles()
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
        AdjacencyListStrategy<ALJobRole> strategy = (AdjacencyListStrategy<ALJobRole>)factory.TierStrategy(_db.AlJobRoles.ToList());

        aljrs[0].Next(null, strategy);
        for (int i = 0; i < aljrs.Count - 1; i++)
        {
            var curr = aljrs[i];
            var sub = aljrs[i + 1];
            sub.Next(curr.VertexId, strategy);
        }

        _db.SaveChanges();
    }

    [Route("/api/jobroles/adjacencylist")]
    [HttpGet]
    public IList<IList<ALJobRole>> Get()
    {
        var dbSet = _db.AlJobRoles;
        var top = dbSet.First(t => t.Title == Constants.JobRoles[0]);
        var tree = dbSet.ToList().Treeify<ALJobRole>(top);
        return tree;
    }
}
