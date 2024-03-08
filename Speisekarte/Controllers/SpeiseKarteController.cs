using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Speisekarte.Data;
using Speisekarte.Models;

namespace Speisekarte.Controllers
{
    public class SpeiseKarteController : Controller
    {
        private ApplicationDbContext _context;

        public SpeiseKarteController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var speisen = _context.Speisen.Include(speise => speise.Zutaten).ToList();
            return View();
        }

        public IActionResult CreateSpeiseForm()
        {
            //// Informationen für eine Drop-Down-Liste bereitstellen
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            SelectListItem eintrag1 = new SelectListItem("Yummy Good", "5");
            selectListItems.Add(eintrag1);
            SelectListItem eintrag2 = new SelectListItem("Good", "4");
            selectListItems.Add(eintrag2);
            SelectListItem eintrag3 = new SelectListItem("Nice", "3");
            selectListItems.Add(eintrag3);
            SelectListItem eintrag4 = new SelectListItem("Ok", "2");
            selectListItems.Add(eintrag4);
            SelectListItem eintrag5 = new SelectListItem("Awful", "1");
            selectListItems.Add(eintrag5);

            // Die Übergabe erfolgt über das ViewData-Dictionary
            ViewData["Kategorien"] = selectListItems;

            return View();
        }

        public IActionResult AddZutatForm(int? id)
        {
            if (id is not null && id > 0)
            {
                var speisenFromDB = _context.Speisen.FirstOrDefault(s => s.Id == id);
                if (speisenFromDB is not null)
                {
                    Zutat zutat = new Zutat();
                    zutat.SpeiseId = id;
                    return View(zutat);
                }
            }
            return View();
        }

        public IActionResult AddZutat(Zutat zutat)
        {
            if (zutat.SpeiseId is not null && zutat.SpeiseId > 0)
            {
                Speise? speisenFromDB = _context.Speisen.FirstOrDefault(x => x.Id == zutat.SpeiseId);
                if (speisenFromDB is not null)
                {
                    speisenFromDB.Zutaten.Add(zutat);
                    _context.SaveChanges();
                }
            }
            return View("Index");
        }

        public IActionResult CreateEditSpeiseListe(Speise speise)
        {
            if (speise.Id == 0)
            {
                _context.Speisen.Add(speise);
            }
            else
            {
                var geschenkFromDB = _context.Speisen.Include(speise => speise.Zutaten).SingleOrDefault(x => x.Id == speise.Id);
                if (geschenkFromDB == null)
                {
                    return NotFound();
                }
                else
                {
                    geschenkFromDB.Titel = speise.Titel;
                    geschenkFromDB.Notizen = speise.Notizen;
                    geschenkFromDB.Sterne = speise.Sterne;
                    geschenkFromDB.Zutaten = speise.Zutaten;
                }
            }

            _context.SaveChanges();

            return View("Index");
        }
    }
}
