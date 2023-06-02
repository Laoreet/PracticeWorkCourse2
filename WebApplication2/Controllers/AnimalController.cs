using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.DataObjects;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Drawing;
using Org.BouncyCastle.Crypto.Paddings;
using System.Drawing.Drawing2D;
using static System.Collections.Specialized.BitVector32;
using System.Net.NetworkInformation;
using System.Text;
using ClosedXML.Excel;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace WebApplication2.Controllers
{
    public class AnimalController : Controller
    {
        private readonly animal_shelter_testContext _context;
        private readonly IWebHostEnvironment _webHost;
        public AnimalController(animal_shelter_testContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHost = hostEnvironment;
        }

        List<string> sexes = new List<string>() { "Самец", "Самка" };

        public IActionResult Index(int pg = 1)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                const int pageSize = 10;

            if (pg < 1)
                pg = 1;

            int recsCount = _context.Animals.Count();

            var paging = new Paging(recsCount, pg, pageSize);
            paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();

            int recSkip = (pg - 1) * pageSize;

            List<Animal> animals = _context.Animals.Include(p => p.AnimalType).Include(b => b.Station)
                    .Skip(recSkip).Take(paging.PageSize).ToList();

            this.ViewBag.Paging = paging;
            return View(animals.OrderBy(a => a.AnimalId));
            }
            return Redirect("~/Animal/AnalogViewNew");
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                Animal animal = new Animal();
                ViewBag.Stations = GetStations();
                ViewBag.AnimalTypes = GetAnimalTypes();
                ViewBag.Sexes = new SelectList(sexes);
                return View(animal);
            }
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Animal animal, IFormFile file)
        {
            try
            {
                if (animal.PhotoHelper != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(animal.PhotoHelper.OpenReadStream()))
                    {
                        animal.Photo = binaryReader.ReadBytes((int)animal.PhotoHelper.Length);

                        _context.Add(animal);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _context.Entry(animal).Reload();
                ModelState.AddModelError("", ex.Message);
                return View(animal);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                Animal animal = _context.Animals.Find(id);
                ViewBag.Stations = GetStations();
                ViewBag.AnimalTypes = GetAnimalTypes();
                ViewBag.Sexes = new SelectList(sexes);
                return View(animal);
            }
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Animal animal, IFormFile file)
        {
            try
            {
                if (animal.PhotoHelper != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        animal.PhotoHelper.OpenReadStream().CopyTo(memoryStream);
                        animal.Photo = memoryStream.ToArray();
                    }
                }
                _context.Attach(animal);
                _context.Entry(animal).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _context.Entry(animal).Reload();
                ModelState.AddModelError("", ex.Message);
                return View(animal);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.Animals == null)
            {
                return NotFound();
            }

            if (DB.status != "guest" && DB.status != "client")
            {
                var animal = await _context.Animals
                .Include(d => d.AnimalType)
                .Include(d => d.Station)
                .FirstOrDefaultAsync(m => m.AnimalId == id);

                if (animal == null)
                {
                    return NotFound();
                }
                return View(animal);
            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Animal animal)
        {
            _context.Attach(animal);
            _context.Entry(animal).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetStations()
        {
            var stationsList = new List<SelectListItem>();
            List<Station> Stations = _context.Stations.ToList();

            stationsList = Stations.Select(st => new SelectListItem()
            {
                Value = st.StationId.ToString(),
                Text = st.StationName
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "Выберите станцию"
            };
            stationsList.Insert(0, defItem);
            return stationsList;
        }

        private List<SelectListItem> GetAnimalTypes()
        {
            var typesList = new List<SelectListItem>();
            List<AnimalType> Types = _context.AnimalTypes.ToList();

            typesList = Types.Select(st => new SelectListItem()
            {
                Value = st.AnimalTypeId.ToString(),
                Text = st.AnimalTypeName
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "Выберите вид животного"
            };
            typesList.Insert(0, defItem);
            return typesList;
        }


        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }


        public IActionResult RenderImage(int id)
        {
            Animal animals = _context.Animals.Find(id);
            if (animals != null && animals.Photo != null &&
                animals.Photo.Length>1)
            {
                byte[] byteData = animals.Photo;
                return File(byteData, "image/png");
            }
            else
                return File(ImageToByteArray(Image.FromFile
                    ("Resources/missing.png")), "image/png");
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.Animals == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(d => d.AnimalType)
                .Include(d => d.Station)
                .FirstOrDefaultAsync(m => m.AnimalId == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        public IActionResult AnalogViewNew()
        {
            ViewBag.AnimalTypes = GetAnimalTypes();
            List<Animal> animals = _context.Animals.Include(p => p.AnimalType).Include(b => b.Station).ToList();
            return View(animals.OrderBy(a => a.AnimalId));
        }

        [HttpPost]
        public IActionResult AnalogViewNew(string type)
        {
            ViewBag.AnimalTypes = GetAnimalTypes();
            List<Animal> animals = null;
            if (type != null)
            {
                animals = _context.Animals.Include(p => p.AnimalType).Where(t => t.AnimalTypeId.ToString() == type).Include(b => b.Station).ToList();
            }
            else
            {
                animals = _context.Animals.Include(p => p.AnimalType).Include(b => b.Station).ToList();
            }
            return View(animals.OrderBy(a => a.AnimalId));
        }

        public async Task<IActionResult> AddAnimalApplication(int id) 
        {
            //Animal animal = _context.Animals.First(a => a.AnimalId == id);
            return Redirect("~/AnimalApplications/CreateExactAnimal?id="+id);
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.Animals.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "animals.csv");
                }
            }
            return View();
        }
    }
}
