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
    public class DonationsController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public DonationsController(animal_shelter_testContext context)
        {
            _context = context;
        }

        // GET: Donations
        public async Task<IActionResult> Index(int pg = 1)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                const int pageSize = 10;

                if (pg < 1)
                    pg = 1;

                int recsCount = _context.Donations.Count();

                var paging = new Paging(recsCount, pg, pageSize);
                paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();

                int recSkip = (pg - 1) * pageSize;

                List<Donation> animals = _context.Donations.Include(p => p.DonationType).Include(b => b.Station)
                    .Include(f => f.Donator).Skip(recSkip).Take(paging.PageSize).ToList();

                this.ViewBag.Paging = paging;
                return View(animals.OrderBy(a => a.DonationId));
            }
            return Redirect("~/Home/Index");
        }

        // GET: Donations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                if (id == null || _context.Donations == null)
                {
                    return NotFound();
                }

                var donation = await _context.Donations
                    .Include(d => d.DonationType)
                    .Include(d => d.Donator)
                    .Include(d => d.Station)
                    .FirstOrDefaultAsync(m => m.DonationId == id);
                if (donation == null)
                {
                    return NotFound();
                }

                return View(donation);
            }
            return Redirect("~/Home/Index");
        }

        // GET: Donations/Create
        public IActionResult Create()
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                ViewData["DonationTypeId"] = new SelectList(_context.DonationTypes, "DonationTypeId", "DonationTypeDescription");
                ViewData["DonatorId"] = new SelectList(_context.Donators, "DonatorId", "DonatorNickname");
                ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName");
                return View();
            }
            return Redirect("~/Home/Index");
        }

        // POST: Donations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonationId,DonatorId,DonationTypeId,StationId,DonationAmount")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DonationTypeId"] = new SelectList(_context.DonationTypes, "DonationTypeId", "DonationTypeDescription", donation.DonationTypeId);
            ViewData["DonatorId"] = new SelectList(_context.Donators, "DonatorId", "DonatorNickname", donation.DonatorId);
            ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName", donation.StationId);
            return View(donation);
        }

        // GET: Donations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                if (id == null || _context.Donations == null)
                {
                    return NotFound();
                }

                var donation = await _context.Donations.FindAsync(id);
                if (donation == null)
                {
                    return NotFound();
                }
                ViewData["DonationTypeId"] = new SelectList(_context.DonationTypes, "DonationTypeId", "DonationTypeDescription", donation.DonationTypeId);
                ViewData["DonatorId"] = new SelectList(_context.Donators, "DonatorId", "DonatorNickname", donation.DonatorId);
                ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName", donation.StationId);
                return View(donation);
            }
            return Redirect("~/Home/Index");
        }

        // POST: Donations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonationId,DonatorId,DonationTypeId,StationId,DonationAmount")] Donation donation)
        {
            if (id != donation.DonationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationExists(donation.DonationId))
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
            ViewData["DonationTypeId"] = new SelectList(_context.DonationTypes, "DonationTypeId", "DonationTypeDescription", donation.DonationTypeId);
            ViewData["DonatorId"] = new SelectList(_context.Donators, "DonatorId", "DonatorNickname", donation.DonatorId);
            ViewData["StationId"] = new SelectList(_context.Stations, "StationId", "StationName", donation.StationId);
            return View(donation);
        }

        // GET: Donations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                if (id == null || _context.Donations == null)
                {
                    return NotFound();
                }

                var donation = await _context.Donations
                    .Include(d => d.DonationType)
                    .Include(d => d.Donator)
                    .Include(d => d.Station)
                    .FirstOrDefaultAsync(m => m.DonationId == id);
                if (donation == null)
                {
                    return NotFound();
                }

                return View(donation);
            }
            return Redirect("~/Home/Index");
        }

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Donations == null)
            {
                return Problem("Entity set 'animal_shelter_testContext.Donations'  is null.");
            }
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonationExists(int id)
        {
            return (_context.Donations?.Any(e => e.DonationId == id)).GetValueOrDefault();
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.Donations.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "donations.csv");
                }
            }
            return View();
        }
    }
}
