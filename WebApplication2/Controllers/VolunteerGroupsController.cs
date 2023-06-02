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
    public class VolunteerGroupsController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public VolunteerGroupsController(animal_shelter_testContext context)
        {
            _context = context;
        }

        // GET: VolunteerGroups
        public async Task<IActionResult> Index(int pg = 1)
        {
                const int pageSize = 10;

                if (pg < 1)
                    pg = 1;

                int recsCount = _context.VolunteerGroups.Count();

                var paging = new Paging(recsCount, pg, pageSize);
                paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();

                int recSkip = (pg - 1) * pageSize;

                List<VolunteerGroup> animals = _context.VolunteerGroups.Include(b => b.Station).Skip(recSkip).Take(paging.PageSize).ToList();

                this.ViewBag.Paging = paging;
                return View(animals.OrderBy(a => a.VolunteerGroupId));
        }

        // GET: VolunteerGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
                if (id == null || _context.VolunteerGroups == null)
                {
                    return NotFound();
                }

                var volunteerGroup = await _context.VolunteerGroups
                    .Include(v => v.Station)
                    .FirstOrDefaultAsync(m => m.VolunteerGroupId == id);
                if (volunteerGroup == null)
                {
                    return NotFound();
                }

                return View(volunteerGroup);
        }

        // GET: VolunteerGroups/Create
        public IActionResult Create()
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName");
                return View();
            }
            return Redirect("~/Home/Index");
        }

        // POST: VolunteerGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VolunteerGroupId,StationId,WorkType")] VolunteerGroup volunteerGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteerGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName", volunteerGroup.StationId);
            return View(volunteerGroup);
        }

        // GET: VolunteerGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.VolunteerGroups == null)
                {
                    return NotFound();
                }

                var volunteerGroup = await _context.VolunteerGroups.FindAsync(id);
                if (volunteerGroup == null)
                {
                    return NotFound();
                }
                ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName", volunteerGroup.StationId);
                return View(volunteerGroup);
            }
            return Redirect("~/Home/Index");
        }

        // POST: VolunteerGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerGroupId,StationId,WorkType")] VolunteerGroup volunteerGroup)
        {
            if (id != volunteerGroup.VolunteerGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerGroupExists(volunteerGroup.VolunteerGroupId))
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
            ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName", volunteerGroup.StationId);
            return View(volunteerGroup);
        }

        // GET: VolunteerGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.VolunteerGroups == null)
                {
                    return NotFound();
                }

                var volunteerGroup = await _context.VolunteerGroups
                    .Include(v => v.Station)
                    .FirstOrDefaultAsync(m => m.VolunteerGroupId == id);
                if (volunteerGroup == null)
                {
                    return NotFound();
                }

                return View(volunteerGroup);
            }
            return Redirect("~/Home/Index");
        }

        // POST: VolunteerGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VolunteerGroups == null)
            {
                return Problem("Entity set 'animal_shelter_testContext.VolunteerGroups'  is null.");
            }
            var volunteerGroup = await _context.VolunteerGroups.FindAsync(id);
            if (volunteerGroup != null)
            {
                _context.VolunteerGroups.Remove(volunteerGroup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerGroupExists(int id)
        {
            return (_context.VolunteerGroups?.Any(e => e.VolunteerGroupId == id)).GetValueOrDefault();
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.VolunteerGroups.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "volunteer_groups.csv");
                }
            }
            return View();
        }

        public async Task<IActionResult> AddVolunteerApplication()
        {
            //Animal animal = _context.Animals.First(a => a.AnimalId == id);
            return Redirect("~/VolunteerApplications/Create");
        }
    }
}
