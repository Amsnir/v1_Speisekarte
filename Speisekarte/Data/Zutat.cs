using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Speisekarte.Data
{
    public class Zutat
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Beschreibung { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Einheit { get; set; } = string.Empty;
        public decimal Menge { get; set; }
        [JsonIgnore]
        public Speise? Speise { get; set; }
        public int? SpeiseId { get; set; }

        public override string ToString()
        {
            return $"{Menge} {Einheit} {Beschreibung}";
        }
    }
}
