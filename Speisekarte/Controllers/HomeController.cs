using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Speisekarte.Data;
using Speisekarte.Models;
using SQLitePCL;
using System.Diagnostics;
using System.Globalization;

namespace Speisekarte.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            string day = DateTimeFormatInfo.CurrentInfo.GetDayName(DateTime.Now.DayOfWeek);
            ViewData["TagesHit"] = $"Heute, am {day}, gibt es eine scharfe Soße gratis dazu";

            List<SelectListItem> selectListItems= new List<SelectListItem>();
            SelectListItem eintrag = new SelectListItem("Top", "3");
            selectListItems.Add(eintrag);
            SelectListItem eintrag2 = new SelectListItem("Medium", "2");
            selectListItems.Add(eintrag2);
            SelectListItem eintrag3 = new SelectListItem("Low", "1");
            selectListItems.Add(eintrag3);

            ViewData["Kategorien"] = selectListItems;

            var speisen = _context.Speisen.Include(speise => speise.Zutaten).ToList();
            return View(speisen);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetSpeisenPartial(string query)
        {
            List<Speise> speisen = new List<Speise>();

            if (string.IsNullOrEmpty(query))
            {
                speisen = _context.Speisen.Include(speise => speise.Zutaten).ToList();
            }
            else
            {
                speisen = _context.Speisen.Where(x => x.Titel.ToLower().Contains(query.ToLower())).Include(speise => speise.Zutaten).ToList();
            }
            return PartialView("index",speisen);
        }

        public IActionResult DeleteSpeise(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var speiseFromDB = _context.Speisen.Include(speise => speise.Zutaten).SingleOrDefault(x => x.Id == id);
            if (speiseFromDB is null)
            {
                return NotFound();
            }

            foreach (var zutat in speiseFromDB.Zutaten)
            {
                _context.Zutaten.Remove(zutat);
            }

            _context.Speisen.Remove(speiseFromDB);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}