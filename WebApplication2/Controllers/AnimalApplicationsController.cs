using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DataObjects;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AnimalApplicationsController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public AnimalApplicationsController(animal_shelter_testContext context)
        {
            _context = context;
        }

        // GET: AnimalApplications
        public async Task<IActionResult> Index(int pg = 1)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                const int pageSize = 10;

                if (pg < 1)
                    pg = 1;

                int recsCount = _context.AnimalApplications.Count();

                var paging = new Paging(recsCount, pg, pageSize);
                paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();

                int recSkip = (pg - 1) * pageSize;

                List<AnimalApplication> animals = _context.AnimalApplications.Include(p => p.Animal).Skip(recSkip).Take(paging.PageSize).ToList();

                this.ViewBag.Paging = paging;
                return View(animals.OrderBy(a => a.AnimalApplicationId));
            }
            return Redirect("~/Home/Index");
        }

        // GET: AnimalApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.AnimalApplications == null)
                {
                    return NotFound();
                }

                var animalApplication = await _context.AnimalApplications
                    .Include(a => a.Animal)
                    .FirstOrDefaultAsync(m => m.AnimalApplicationId == id);
                if (animalApplication == null)
                {
                    return NotFound();
                }
                return View(animalApplication);
            }
            return Redirect("~/Home/Index");
        }

        // GET: AnimalApplications/Create
        public IActionResult Create()
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name");
                ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
                return View();
            }
            return Redirect("~/Home/Index");
        }

        // GET: AnimalApplications/Create
        public IActionResult CreateExactAnimal(int id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                ViewData["AnimalId"] = new SelectList(_context.Animals.Where(a => a.AnimalId == id), "AnimalId", "Name");
                ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
                return View("Create");
            }
            return Redirect("~/Home/Index");
        }

        // POST: AnimalApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnimalApplicationId,AnimalId,Firstname,Sex,Age,PhoneNumber")] AnimalApplication animalApplication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animalApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalName", animalApplication.AnimalId);
            return View(animalApplication);
        }

        // GET: AnimalApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.AnimalApplications == null)
                {
                    return NotFound();
                }

                var animalApplication = await _context.AnimalApplications.FindAsync(id);
                if (animalApplication == null)
                {
                    return NotFound();
                }
                ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalName", animalApplication.AnimalId);
                ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
                return View(animalApplication);
            }
            return Redirect("~/Home/Index");
        }

        // POST: AnimalApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnimalApplicationId,AnimalId,Firstname,Sex,Age,PhoneNumber")] AnimalApplication animalApplication)
        {
            if (id != animalApplication.AnimalApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animalApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalApplicationExists(animalApplication.AnimalApplicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalName", animalApplication.AnimalId);
            return View(animalApplication);
        }

        // GET: AnimalApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.AnimalApplications == null)
                {
                    return NotFound();
                }

                var animalApplication = await _context.AnimalApplications
                    .Include(a => a.Animal)
                    .FirstOrDefaultAsync(m => m.AnimalApplicationId == id);
                if (animalApplication == null)
                {
                    return NotFound();
                }

                return View(animalApplication);
            }
            return Redirect("~/Home/Index");
        }

        // POST: AnimalApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AnimalApplications == null)
            {
                return Problem("Entity set 'animal_shelter_testContext.AnimalApplications'  is null.");
            }
            var animalApplication = await _context.AnimalApplications.FindAsync(id);
            if (animalApplication != null)
            {
                _context.AnimalApplications.Remove(animalApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalApplicationExists(int id)
        {
            return (_context.AnimalApplications?.Any(e => e.AnimalApplicationId == id)).GetValueOrDefault();
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.AnimalApplications.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "animal_applications.csv");
                }
            }
            return View();
        }
    }
}
