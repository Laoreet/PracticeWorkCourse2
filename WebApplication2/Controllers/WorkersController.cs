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
    public class WorkersController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public WorkersController(animal_shelter_testContext context)
        {
            _context = context;
        }

        // GET: Workers
        public async Task<IActionResult> Index(int pg = 1)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                const int pageSize = 10;

                if (pg < 1)
                    pg = 1;

                int recsCount = _context.Workers.Count();

                var paging = new Paging(recsCount, pg, pageSize);
                paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();

                int recSkip = (pg - 1) * pageSize;

                List<Worker> workers = _context.Workers.Include(b => b.Station).Skip(recSkip).Take(paging.PageSize).ToList();

                this.ViewBag.Paging = paging;
                return View(workers.OrderBy(a => a.WorkerId));
            }
            return Redirect("~/Home/Index");
        }

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.Workers == null)
                {
                    return NotFound();
                }

                var worker = await _context.Workers
                    .Include(w => w.Station)
                    .FirstOrDefaultAsync(m => m.WorkerId == id);
                if (worker == null)
                {
                    return NotFound();
                }

                return View(worker);
            }
            return Redirect("~/Home/Index");
        }

        // GET: Workers/Create
        public IActionResult Create()
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName");
                ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
                return View();
            }
            return Redirect("~/Home/Index");
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkerId,StationId,Secondname,Firstname,Patronymic,Position,Sex,Age,PhoneNumber,Salary")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(worker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName", worker.StationId);
            ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
            return View(worker);
        }

        // GET: Workers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.Workers == null)
                {
                    return NotFound();
                }

                var worker = await _context.Workers.FindAsync(id);
                if (worker == null)
                {
                    return NotFound();
                }
                ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName", worker.StationId);
                ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
                return View(worker);
            }
            return Redirect("~/Home/Index");
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkerId,StationId,Secondname,Firstname,Patronymic,Position,Sex,Age,PhoneNumber,Salary")] Worker worker)
        {
            if (id != worker.WorkerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.WorkerId))
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
            ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName", worker.StationId);
            ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
            return View(worker);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.Workers == null)
                {
                    return NotFound();
                }

                var worker = await _context.Workers
                    .Include(w => w.Station)
                    .FirstOrDefaultAsync(m => m.WorkerId == id);
                if (worker == null)
                {
                    return NotFound();
                }

                return View(worker);
            }
            return Redirect("~/Home/Index");
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Workers == null)
            {
                return Problem("Entity set 'animal_shelter_testContext.Workers'  is null.");
            }
            var worker = await _context.Workers.FindAsync(id);
            if (worker != null)
            {
                _context.Workers.Remove(worker);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(int id)
        {
            return (_context.Workers?.Any(e => e.WorkerId == id)).GetValueOrDefault();
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.Workers.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "workers.csv");
                }
            }
            return View();
        }
    }
}
