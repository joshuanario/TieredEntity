using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TieredEntity.AdjacencyList;
using TieredEntity.NestedSet;
using TieredEntity.Web.AppDb;
using TieredEntity.Web.Models;

namespace TieredEntity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string[] jobRoles = new[] {"Officer", "Director", "Manager", "Supervisor", "Worker"};
        private readonly string topdog = "War Chief";
        private readonly string eastdog = "Warlord of the East";
        private readonly string[] eastsiders = new[] {"A Tribe", "B Tribe", "C Tribe", "D Tribe"};
        private readonly string southdog = "Warlord of the South";
        private readonly string[] southsiders = new[] {"E Tribe", "F Tribe", "G Tribe"};
        private readonly string westdog = "Warlord of the West";
        private readonly string[] westsiders = new[] {"H Tribe", "I Tribe"};
        private readonly string northdog = "Warlord of the North";
        private readonly string[] northsiders = new[] {"J Tribal Nation"};
        private readonly Db _db;

        public HomeController(Db db)
        {
            _db = db;
        }

        private void InitALDutyPositions()
        {
            var dbset = _db.AlDutyPositions;
            if (dbset.Any(dp => string.Equals(dp.Title, topdog)))
            {
                return;
            }
            var aldps = new List<ALDutyPosition>();
            var top = new ALDutyPosition { Title = topdog };
            aldps.Add(top);
            var east = new ALDutyPosition { Title = eastdog };
            aldps.Add(east);
            var south = new ALDutyPosition { Title = southdog };
            aldps.Add(south);
            var west = new ALDutyPosition { Title = westdog };
            aldps.Add(west);
            var north = new ALDutyPosition { Title = northdog };
            aldps.Add(north);
            var easts = new List<ALDutyPosition>();
            foreach (var e in eastsiders)
            {
                var add = new ALDutyPosition { Title = e };
                aldps.Add(add);
                easts.Add(add);
            }

            var souths = new List<ALDutyPosition>();
            foreach (var s in southsiders)
            {
                var add = new ALDutyPosition { Title = s };
                aldps.Add(add);
                souths.Add(add);
            }

            var wests = new List<ALDutyPosition>();
            foreach (var w in westsiders)
            {
                var add = new ALDutyPosition { Title = w };
                aldps.Add(add);
                wests.Add(add);
            }

            var norths = new List<ALDutyPosition>();
            foreach (var n in northsiders)
            {
                var add = new ALDutyPosition { Title = n };
                aldps.Add(add);
                norths.Add(add);
            }
            dbset.AddRange(aldps);
            _db.SaveChanges();

            var factory = AdjacencyListStrategyFactory<ALDutyPosition>.Factory();
            AdjacencyListStrategy<ALDutyPosition> strategy = (AdjacencyListStrategy<ALDutyPosition>) factory.TierStrategy(_db.AlDutyPositions);
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

        private void InitNSDutyPositions()
        {
            var dbset = _db.NsDutyPositions;
            if (dbset.Any(dp => string.Equals(dp.Title, topdog)))
            {
                return;
            }
            var nsdps = new List<NSDutyPosition>();
            var top = new NSDutyPosition { Title = topdog };
            nsdps.Add(top);
            var east = new NSDutyPosition { Title = eastdog };
            nsdps.Add(east);
            var south = new NSDutyPosition { Title = southdog };
            nsdps.Add(south);
            var west = new NSDutyPosition { Title = westdog };
            nsdps.Add(west);
            var north = new NSDutyPosition { Title = northdog };
            nsdps.Add(north);
            var easts = new List<NSDutyPosition>();
            foreach (var e in eastsiders)
            {
                var add = new NSDutyPosition { Title = e };
                nsdps.Add(add);
                easts.Add(add);
            }

            var souths = new List<NSDutyPosition>();
            foreach (var s in southsiders)
            {
                var add = new NSDutyPosition { Title = s };
                nsdps.Add(add);
                souths.Add(add);
            }

            var wests = new List<NSDutyPosition>();
            foreach (var w in westsiders)
            {
                var add = new NSDutyPosition { Title = w };
                nsdps.Add(add);
                wests.Add(add);
            }

            var norths = new List<NSDutyPosition>();
            foreach (var n in northsiders)
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

        public IActionResult Index()
        {
            InitALDutyPositions();
            InitNSDutyPositions();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ALJobRoles()
        {
            //todo
            return View();
        }

        public IActionResult ALDutyPositions()
        {
            //todo
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
