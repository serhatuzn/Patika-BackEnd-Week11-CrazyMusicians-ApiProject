using CrazyMusicians.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrazyMusicians.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusiciansController : ControllerBase
    {
        private static List<Musicians> _musicians = new List<Musicians>()
            {
                new Musicians {Id = 1, Name = "Ahmet Çalgı", Profession = "Ünlü Çalgı Çalar", FunFeature = "Her zaman yanlış nota çalar, ama çok eğlenceli."},
                new Musicians {Id = 2, Name = "Zeynep Melodi", Profession = "Popüler Melodi Yazarı", FunFeature = "Şarkıları yanlış anlaşılır ama çok popüler."},
                new Musicians { Id = 3, Name = "Cemil Akor", Profession = "Çılgın Akorist", FunFeature = "Akorları sık değiştirir, ama şaşırtıcı derecede yetenekli." },
                new Musicians { Id = 4, Name = "Fatma Nota", Profession = "Sürpriz Nota Üreticisi", FunFeature = "Nota üretirken sürekli sürprizler hazırlar." },
                new Musicians { Id = 5, Name = "Hasan Ritim", Profession = "Ritim Canavarı", FunFeature = "Her ritmi kendi tarzında yapar, hiç uymaz ama komiktir." },
                new Musicians { Id = 6, Name = "Elif Armoni", Profession = "Armoni Ustası", FunFeature = "Armonilerini bazen yanlış çalar, ama çok yaratıcıdır." },
                new Musicians { Id = 7, Name = "Ali Perde", Profession = "Perde Uygulayıcı", FunFeature = "Her perdeyi farklı şekilde çalar, her zaman sürprizlidir." },
                new Musicians { Id = 8, Name = "Ayşe Rezonans", Profession = "Rezonans Uzmanı", FunFeature = "Rezonans konusunda uzman, ama bazen çok gurultu çıkarır." },
                new Musicians { Id = 9, Name = "Murat Ton", Profession = "Tonlama Meraklısı", FunFeature = "Tonlamalarındaki farklılıklar bazen komik, ama oldukça ilginç." },
                new Musicians { Id = 10, Name = "Selin Akor", Profession = "Akor Sihirbazı", FunFeature = "Akorları değiştirdiğinde bazen sihirli bir hava yaratır." }
            };

        [HttpGet]
        public ActionResult<List<Musicians>> GetAll() // Tüm müzisyenleri getir, ve listele
        {
            return _musicians;
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id) // Id'ye göre müzisyen getir ve listele
        {
            var musician = _musicians.FirstOrDefault(x => x.Id == id);

            if (musician == null)
            {
                return NotFound();
            }
            return Ok(musician); // HTTP kodu 200 döner
        }

        [HttpPost]
        public ActionResult PostEkle(Musicians newMusician) // Yeni müzisyen ekle  
        {
            Musicians musician = new Musicians()
            {
                Id = _musicians.Max(x => x.Id) + 1, // Id'yi otomatik arttır
                Name = newMusician.Name,
                Profession = newMusician.Profession,
                FunFeature = newMusician.FunFeature
            };
            _musicians.Add(musician);

            return CreatedAtAction(nameof(Get), new { musician.Id }, musician);
        }

        [HttpPut("{id}")]
        public ActionResult PutGuncelle(int id, Musicians updatedMusician) // Müzisyen güncelle
        {
            Musicians? musician = _musicians.FirstOrDefault(x => x.Id == id);

            if (musician == null)
            {
                return NotFound();
            }

            // Müzisyen güncelleme işlemi
            musician.Name = updatedMusician.Name;
            musician.Profession = updatedMusician.Profession;
            musician.FunFeature = updatedMusician.FunFeature;

            return Ok(musician);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteMusician(int id) // İd'ye göre müzisyeni sil
        {
            Musicians? musician = _musicians.FirstOrDefault(x => x.Id == id); // Id'ye göre müzisyeni bul

            if (musician == null) // Eğer müzisyen yoksa Hata kontrolü yap
            {
                return NotFound();
            }

            _musicians.Remove(musician); // Müzisyeni silme işlemi
            return NoContent(); // HTTP kodu 204 döner
        }

        [HttpPatch("{id}")]
        public ActionResult Patch(int id, [FromBody] Musicians Updatedmusican) // Müzisyen güncelle ama eksik bilgi ile
        {
            var musician = _musicians.FirstOrDefault(x => x.Id == id); // Id'ye göre müzisyeni bul

            if (musician == null)
            {
                return NotFound(); // HTTP kodu 404 döner
            }

            musician.Name = Updatedmusican.Name ?? musician.Name; // Eğer yeni isim yoksa eski ismi kullan Kontrolü için ?? kullanılır
            musician.Profession = Updatedmusican.Profession ?? musician.Profession; // Eğer yeni meslek yoksa eski mesleği kullan
            musician.FunFeature = Updatedmusican.FunFeature ?? musician.FunFeature; // Eğer yeni özellik yoksa eski özelliği kullan

            return Ok(musician); // HTTP kodu 200 döner
        }

        [HttpGet("search")]
        public ActionResult<List<Musicians>> Search([FromQuery] string? name, [FromQuery] string? profession)
        {

            // Lınq sorgusu ile isim ve meslek arama
            var results = _musicians
                .Where(m => (string.IsNullOrEmpty(name) || m.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) && // StringComparison.OrdinalIgnoreCase büyük küçük harf duyarlılığını kaldırır
                            (string.IsNullOrEmpty(profession) || m.Profession.Contains(profession, StringComparison.OrdinalIgnoreCase))) // Where ile filtreleme yapılır
                .ToList(); // toList ile sonuçları listele

            return Ok(results);
        }
    }
    }