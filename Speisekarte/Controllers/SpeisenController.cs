using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Speisekarte.Data;
using SQLitePCL;
using System.Net.WebSockets;

namespace Speisekarte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeisenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SpeisenController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Route("Fill")]
        public void FillData()
        {
            Speise s1 = new Speise { Titel = "Hauptspeise", Notizen = "Meine Notizen dazu", Sterne = 5 };
            Zutat z1 = new Zutat { Beschreibung = "Mehl", Einheit = "gramm", Menge = 200 };
            Zutat z2 = new Zutat { Beschreibung = "Zucker", Einheit = "gramm", Menge = 50 };
            Zutat z3 = new Zutat { Beschreibung = "Öl", Einheit = "ml", Menge = 100 };

            s1.Zutaten.Add(z1);
            s1.Zutaten.Add(z2);
            s1.Zutaten.Add(z3);

            _context.Speisen.Add(s1);
            _context.SaveChanges();
        }


        [HttpGet]
        [Route("Getall")]
        public List<Speise> GetSpeisen()
        {
            var speisen = _context.Speisen
                .Include(speise => speise.Zutaten)
                .ToList();
            return speisen;
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int? id) 
        {
            if(id == null || _context.Speisen == null)
            {
                return NotFound();
            }

            var speise = await _context.Speisen.Include(speise => speise.Zutaten).SingleOrDefaultAsync(s => s.Id == id);

            if(speise == null)
            {
                return NotFound();
            }
            
            foreach(var zutat in speise.Zutaten)
            {
                _context.Zutaten.Remove(zutat);
            }

            _context.Speisen.Remove(speise);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
