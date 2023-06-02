using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DataObjects;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class StationController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public StationController(animal_shelter_testContext context)
        {
            _context = context;
        }
        public IActionResult Index(int pg = 1)
        {
            const int pageSize = 10;

            if (pg < 1)
                pg = 1;

            int recsCount = _context.Stations.Count();

            var paging = new Paging(recsCount, pg, pageSize);
            paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();

            int recSkip = (pg - 1) * pageSize;

            List<Station> stations = _context.Stations.Skip(recSkip).Take(paging.PageSize).ToList();

            this.ViewBag.Paging = paging;
            return View(stations);

        }

        [HttpGet]
        public IActionResult Create()
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                Station station = new Station();
                return View(station);
            }
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Station station)
        {
            _context.Add(station);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                Station station = _context.Stations.Find(id);
                return View(station);
            }
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Station station)
        {
            _context.Attach(station);
            _context.Entry(station).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                Station station = _context.Stations.Find(id);
                return View(station);
            }
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(Station station)
        {
            try
            {
                _context.Attach(station);
                _context.Entry(station).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _context.Entry(station).Reload();
                ModelState.AddModelError("", "Перед удалением станции необходимо, чтобы на нее не ссылались зависимые элементы (работники, животные и т.д.)");
                return View(station);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Station station = _context.Stations.Find(id);
            return View(station);
        }


        [HttpGet]
        public IActionResult CreateModalForm()
        {
            Station station = new Station();
            return PartialView("_CreateModalForm", station);
        }

        [HttpPost]
        public IActionResult CreateModalForm(Station station)
        {
            _context.Add(station);
            _context.SaveChanges();
            return NoContent();
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.Stations.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "stations.csv");
                }
            }
            return View();
        }
    }
}
