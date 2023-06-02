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
    public class DonationTypesController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public DonationTypesController(animal_shelter_testContext context)
        {
            _context = context;
        }

        // GET: DonationTypes
        public async Task<IActionResult> Index(int pg = 1)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                const int pageSize = 10;

                if (pg < 1)
                    pg = 1;

                int recsCount = _context.DonationTypes.Count();

                var paging = new Paging(recsCount, pg, pageSize);
                paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();

                int recSkip = (pg - 1) * pageSize;

                List<DonationType> animals = _context.DonationTypes.Skip(recSkip).Take(paging.PageSize).ToList();

                this.ViewBag.Paging = paging;
                return View(animals.OrderBy(a => a.DonationTypeId));
            }
            return Redirect("~/Home/Index");
        }

        // GET: DonationTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                if (id == null || _context.DonationTypes == null)
                {
                    return NotFound();
                }

                var donationType = await _context.DonationTypes
                    .FirstOrDefaultAsync(m => m.DonationTypeId == id);
                if (donationType == null)
                {
                    return NotFound();
                }

                return View(donationType);
            }
            return Redirect("~/Home/Index");
        }

        // GET: DonationTypes/Create
        public IActionResult Create()
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                return View();
            }
            return Redirect("~/Home/Index");
        }

        // POST: DonationTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonationTypeId,DonationTypeDescription")] DonationType donationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(donationType);
        }

        // GET: DonationTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                if (id == null || _context.DonationTypes == null)
                {
                    return NotFound();
                }

                var donationType = await _context.DonationTypes.FindAsync(id);
                if (donationType == null)
                {
                    return NotFound();
                }
                return View(donationType);
            }
            return Redirect("~/Home/Index");
        }

        // POST: DonationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonationTypeId,DonationTypeDescription")] DonationType donationType)
        {
            if (id != donationType.DonationTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationTypeExists(donationType.DonationTypeId))
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
            return View(donationType);
        }

        // GET: DonationTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                if (id == null || _context.DonationTypes == null)
                {
                    return NotFound();
                }

                var donationType = await _context.DonationTypes
                    .FirstOrDefaultAsync(m => m.DonationTypeId == id);
                if (donationType == null)
                {
                    return NotFound();
                }

                return View(donationType);
            }
            return Redirect("~/Home/Index");
        }

        // POST: DonationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DonationTypes == null)
            {
                return Problem("Entity set 'animal_shelter_testContext.DonationTypes'  is null.");
            }
            var donationType = await _context.DonationTypes.FindAsync(id);
            if (donationType != null)
            {
                _context.DonationTypes.Remove(donationType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonationTypeExists(int id)
        {
            return (_context.DonationTypes?.Any(e => e.DonationTypeId == id)).GetValueOrDefault();
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.DonationTypes.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "donation_types.csv");
                }
            }
            return View();
        }
    }
}
