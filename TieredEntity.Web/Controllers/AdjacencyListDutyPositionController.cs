using Microsoft.AspNetCore.Mvc;
using TieredEntity.Web.AppDb;
using TieredEntity.Web.Models;
using TieredEntity.AdjacencyList;
using TieredEntity.Web.Extensions;

namespace TieredEntity.Web.Controllers;

[ApiController]
public class AdjacencyListDutyPositionController : ControllerBase
{
    private readonly Db _db;

    public AdjacencyListDutyPositionController(Db db)
    {
        _db = db;
        InitializeAdjacencyListDutyPositions();
    }

    private void InitializeAdjacencyListDutyPositions()
    {
        var dbset = _db.AlDutyPositions;
        if (dbset.Any(dp => string.Equals(dp.Title, Constants.Topdog)))
        {
            return;
        }
        var aldps = new List<ALDutyPosition>();
        var top = new ALDutyPosition { Title = Constants.Topdog };
        aldps.Add(top);
        var east = new ALDutyPosition { Title = Constants.Eastdog };
        aldps.Add(east);
        var south = new ALDutyPosition { Title = Constants.Southdog };
        aldps.Add(south);
        var west = new ALDutyPosition { Title = Constants.Westdog };
        aldps.Add(west);
        var north = new ALDutyPosition { Title = Constants.Northdog };
        aldps.Add(north);
        var easts = new List<ALDutyPosition>();
        foreach (var e in Constants.Eastsiders)
        {
            var add = new ALDutyPosition { Title = e };
            aldps.Add(add);
            easts.Add(add);
        }

        var souths = new List<ALDutyPosition>();
        foreach (var s in Constants.Southsiders)
        {
            var add = new ALDutyPosition { Title = s };
            aldps.Add(add);
            souths.Add(add);
        }

        var wests = new List<ALDutyPosition>();
        foreach (var w in Constants.Westsiders)
        {
            var add = new ALDutyPosition { Title = w };
            aldps.Add(add);
            wests.Add(add);
        }

        var norths = new List<ALDutyPosition>();
        foreach (var n in Constants.Northsiders)
        {
            var add = new ALDutyPosition { Title = n };
            aldps.Add(add);
            norths.Add(add);
        }
        dbset.AddRange(aldps);
        _db.SaveChanges();

        var factory = AdjacencyListStrategyFactory<ALDutyPosition>.Factory();
        AdjacencyListStrategy<ALDutyPosition> strategy = (AdjacencyListStrategy<ALDutyPosition>)factory.TierStrategy(_db.AlDutyPositions.ToList());
        top.Next(null, strategy);
        east.Next(top.VertexId, strategy);
        south.Next(top.VertexId, strategy);
        west.Next(top.VertexId, strategy);
        north.Next(top.VertexId, strategy);
        foreach (var e in easts)
        {
            e.Next(east.VertexId, strategy);
        }

        foreach (var s in souths)
        {
            s.Next(south.VertexId, strategy);
        }

        foreach (var w in wests)
        {
            w.Next(west.VertexId, strategy);
        }

        foreach (var n in norths)
        {
            n.Next(north.VertexId, strategy);
        }

        _db.SaveChanges();
    }

    [Route("/api/dutypositions/adjacencylist")]
    [HttpGet]
    public IList<IList<ALDutyPosition>> Get()
    {
        var dbSet = _db.AlDutyPositions;
        var top = dbSet.First(t => t.Title == Constants.Topdog);
        var tree = dbSet.ToList().Treeify<ALDutyPosition>(top);
        return tree;
    }
}
