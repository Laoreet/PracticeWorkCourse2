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
    public class DonatorsController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public DonatorsController(animal_shelter_testContext context)
        {
            _context = context;
        }

        // GET: Donators
        public async Task<IActionResult> Index(int pg = 1)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                const int pageSize = 10;

                if (pg < 1)
                    pg = 1;

                int recsCount = _context.Donators.Count();

                var paging = new Paging(recsCount, pg, pageSize);
                paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();

                int recSkip = (pg - 1) * pageSize;

                List<Donator> animals = _context.Donators.Skip(recSkip).Take(paging.PageSize).ToList();

                this.ViewBag.Paging = paging;
                return View(animals.OrderBy(a => a.DonatorId));
            }
            return Redirect("~/Home/Index");
        }

        // GET: Donators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                if (id == null || _context.Donators == null)
                {
                    return NotFound();
                }

                var donator = await _context.Donators
                    .FirstOrDefaultAsync(m => m.DonatorId == id);
                if (donator == null)
                {
                    return NotFound();
                }

                return View(donator);
            }
            return Redirect("~/Home/Index");
        }

        // GET: Donators/Create
        public IActionResult Create()
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                return View();
            }
            return Redirect("~/Home/Index");
        }

        // POST: Donators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonatorId,DonatorNickname")] Donator donator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(donator);
        }

        // GET: Donators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                if (id == null || _context.Donators == null)
                {
                    return NotFound();
                }

                var donator = await _context.Donators.FindAsync(id);
                if (donator == null)
                {
                    return NotFound();
                }
                return View(donator);
            }
            return Redirect("~/Home/Index");
        }

        // POST: Donators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonatorId,DonatorNickname")] Donator donator)
        {
            if (id != donator.DonatorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonatorExists(donator.DonatorId))
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
            return View(donator);
        }

        // GET: Donators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                if (id == null || _context.Donators == null)
                {
                    return NotFound();
                }

                var donator = await _context.Donators
                    .FirstOrDefaultAsync(m => m.DonatorId == id);
                if (donator == null)
                {
                    return NotFound();
                }

                return View(donator);
            }
            return Redirect("~/Home/Index");
        }

        // POST: Donators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Donators == null)
            {
                return Problem("Entity set 'animal_shelter_testContext.Donators'  is null.");
            }
            var donator = await _context.Donators.FindAsync(id);
            if (donator != null)
            {
                _context.Donators.Remove(donator);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonatorExists(int id)
        {
            return (_context.Donators?.Any(e => e.DonatorId == id)).GetValueOrDefault();
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.Donators.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "donators.csv");
                }
            }
            return View();
        }
    }
}
