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
    public class CarrierVolunteersController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public CarrierVolunteersController(animal_shelter_testContext context)
        {
            _context = context;
        }

        // GET: CarrierVolunteers
        public async Task<IActionResult> Index(int pg = 1)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                const int pageSize = 10;

                if (pg < 1)
                    pg = 1;

                int recsCount = _context.CarrierVolunteers.Count();

                var paging = new Paging(recsCount, pg, pageSize);

                paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();

                int recSkip = (pg - 1) * pageSize;

                List<CarrierVolunteer> animals = _context.CarrierVolunteers.Include(b => b.Station).Skip(recSkip).Take(paging.PageSize).ToList();

                this.ViewBag.Paging = paging;
                return View(animals.OrderBy(a => a.CarrierVolunteerId));
            }
            return Redirect("~/Home/Index");
        }

        // GET: CarrierVolunteers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.CarrierVolunteers == null)
                {
                    return NotFound();
                }

                var carrierVolunteer = await _context.CarrierVolunteers
                    .Include(c => c.Station)
                    .FirstOrDefaultAsync(m => m.CarrierVolunteerId == id);
                if (carrierVolunteer == null)
                {
                    return NotFound();
                }

                return View(carrierVolunteer);
            }
            return Redirect("~/Home/Index");
        }

        // GET: CarrierVolunteers/Create
        public IActionResult Create()
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName");
                return View();
            }
            return Redirect("~/Home/Index");
        }

        // POST: CarrierVolunteers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarrierVolunteerId,StationId,Fullname,PhoneNumber,Vehicle")] CarrierVolunteer carrierVolunteer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carrierVolunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName", carrierVolunteer.StationId);
            return View(carrierVolunteer);
        }

        // GET: CarrierVolunteers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.CarrierVolunteers == null)
                {
                    return NotFound();
                }

                var carrierVolunteer = await _context.CarrierVolunteers.FindAsync(id);
                if (carrierVolunteer == null)
                {
                    return NotFound();
                }
                ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName", carrierVolunteer.StationId);
                return View(carrierVolunteer);
            }
            return Redirect("~/Home/Index");
        }

        // POST: CarrierVolunteers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarrierVolunteerId,StationId,Fullname,PhoneNumber,Vehicle")] CarrierVolunteer carrierVolunteer)
        {
            if (id != carrierVolunteer.CarrierVolunteerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrierVolunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarrierVolunteerExists(carrierVolunteer.CarrierVolunteerId))
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
            ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName", carrierVolunteer.StationId);
            return View(carrierVolunteer);
        }

        // GET: CarrierVolunteers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.CarrierVolunteers == null)
                {
                    return NotFound();
                }

                var carrierVolunteer = await _context.CarrierVolunteers
                    .Include(c => c.Station)
                    .FirstOrDefaultAsync(m => m.CarrierVolunteerId == id);
                if (carrierVolunteer == null)
                {
                    return NotFound();
                }

                return View(carrierVolunteer);
            }
            return Redirect("~/Home/Index");
        }

        // POST: CarrierVolunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CarrierVolunteers == null)
            {
                return Problem("Entity set 'animal_shelter_testContext.CarrierVolunteers'  is null.");
            }
            var carrierVolunteer = await _context.CarrierVolunteers.FindAsync(id);
            if (carrierVolunteer != null)
            {
                _context.CarrierVolunteers.Remove(carrierVolunteer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarrierVolunteerExists(int id)
        {
            return (_context.CarrierVolunteers?.Any(e => e.CarrierVolunteerId == id)).GetValueOrDefault();
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.CarrierVolunteers.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "carrier_volunteers.csv");
                }
            }
            return View();
        }
    }
}
