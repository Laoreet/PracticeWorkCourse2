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
    public class AnimalTypesController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public AnimalTypesController(animal_shelter_testContext context)
        {
            _context = context;
        }

        // GET: AnimalTypes
        public async Task<IActionResult> Index(int pg = 1)
        {
            const int pageSize = 10;

            if (pg < 1)
                pg = 1;

            int recsCount = _context.AnimalTypes.Count();

            var paging = new Paging(recsCount, pg, pageSize);
            paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();

            int recSkip = (pg - 1) * pageSize;

            List<AnimalType> animals = _context.AnimalTypes.Skip(recSkip).Take(paging.PageSize).ToList();

            this.ViewBag.Paging = paging;
            return View(animals.OrderBy(a => a.AnimalTypeId));
        }

        // GET: AnimalTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AnimalTypes == null)
            {
                return NotFound();
            }

            var animalType = await _context.AnimalTypes
                .FirstOrDefaultAsync(m => m.AnimalTypeId == id);
            if (animalType == null)
            {
                return NotFound();
            }

            return View(animalType);
        }

        // GET: AnimalTypes/Create
        public IActionResult Create()
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        // POST: AnimalTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnimalTypeId,AnimalTypeName")] AnimalType animalType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animalType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(animalType);
        }

        // GET: AnimalTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.AnimalTypes == null)
                {
                    return NotFound();
                }

                var animalType = await _context.AnimalTypes.FindAsync(id);
                if (animalType == null)
                {
                    return NotFound();
                }
                return View(animalType);
            }
            return RedirectToAction("Index");
        }

        // POST: AnimalTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnimalTypeId,AnimalTypeName")] AnimalType animalType)
        {
            if (id != animalType.AnimalTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animalType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalTypeExists(animalType.AnimalTypeId))
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
            return View(animalType);
        }

        // GET: AnimalTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.AnimalTypes == null)
                {
                    return NotFound();
                }

                var animalType = await _context.AnimalTypes
                    .FirstOrDefaultAsync(m => m.AnimalTypeId == id);
                if (animalType == null)
                {
                    return NotFound();
                }

                return View(animalType);
            }
            return RedirectToAction("Index");
        }

        // POST: AnimalTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AnimalTypes == null)
            {
                return Problem("Entity set 'animal_shelter_testContext.AnimalTypes'  is null.");
            }
            var animalType = await _context.AnimalTypes.FindAsync(id);
            if (animalType != null)
            {
                _context.AnimalTypes.Remove(animalType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalTypeExists(int id)
        {
            return (_context.AnimalTypes?.Any(e => e.AnimalTypeId == id)).GetValueOrDefault();
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.AnimalTypes.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "animal_types.csv");
                }
            }
            return View();
        }
    }
}
