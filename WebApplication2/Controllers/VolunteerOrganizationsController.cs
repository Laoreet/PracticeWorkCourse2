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
    public class VolunteerOrganizationsController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public VolunteerOrganizationsController(animal_shelter_testContext context)
        {
            _context = context;
        }

        // GET: VolunteerOrganizations
        public async Task<IActionResult> Index(int pg = 1)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                const int pageSize = 10;

                if (pg < 1)
                    pg = 1;

                int recsCount = _context.VolunteerOrganizations.Count();

                var paging = new Paging(recsCount, pg, pageSize);
                paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();
                int recSkip = (pg - 1) * pageSize;

                List<VolunteerOrganization> animals = _context.VolunteerOrganizations.Skip(recSkip).Take(paging.PageSize).ToList();

                this.ViewBag.Paging = paging;
                return View(animals.OrderBy(a => a.VolunteerOrgId));
            }
            return Redirect("~/Home/Index");
        }

        // GET: VolunteerOrganizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.VolunteerOrganizations == null)
                {
                    return NotFound();
                }

                var volunteerOrganization = await _context.VolunteerOrganizations
                    .FirstOrDefaultAsync(m => m.VolunteerOrgId == id);
                if (volunteerOrganization == null)
                {
                    return NotFound();
                }

                return View(volunteerOrganization);
            }
            return Redirect("~/Home/Index");
        }

        // GET: VolunteerOrganizations/Create
        public IActionResult Create()
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                return View();
            }
            return Redirect("~/Home/Index");
        }

        // POST: VolunteerOrganizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VolunteerOrgId,OrgName,Adress,ContactNumber,ContactPerson")] VolunteerOrganization volunteerOrganization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteerOrganization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(volunteerOrganization);
        }

        // GET: VolunteerOrganizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.VolunteerOrganizations == null)
                {
                    return NotFound();
                }

                var volunteerOrganization = await _context.VolunteerOrganizations.FindAsync(id);
                if (volunteerOrganization == null)
                {
                    return NotFound();
                }
                return View(volunteerOrganization);
            }
            return Redirect("~/Home/Index");
        }

        // POST: VolunteerOrganizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerOrgId,OrgName,Adress,ContactNumber,ContactPerson")] VolunteerOrganization volunteerOrganization)
        {
            if (id != volunteerOrganization.VolunteerOrgId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerOrganization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerOrganizationExists(volunteerOrganization.VolunteerOrgId))
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
            return View(volunteerOrganization);
        }

        // GET: VolunteerOrganizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.VolunteerOrganizations == null)
                {
                    return NotFound();
                }

                var volunteerOrganization = await _context.VolunteerOrganizations
                    .FirstOrDefaultAsync(m => m.VolunteerOrgId == id);
                if (volunteerOrganization == null)
                {
                    return NotFound();
                }

                return View(volunteerOrganization);
            }
            return Redirect("~/Home/Index");
        }

        // POST: VolunteerOrganizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VolunteerOrganizations == null)
            {
                return Problem("Entity set 'animal_shelter_testContext.VolunteerOrganizations'  is null.");
            }
            var volunteerOrganization = await _context.VolunteerOrganizations.FindAsync(id);
            if (volunteerOrganization != null)
            {
                _context.VolunteerOrganizations.Remove(volunteerOrganization);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerOrganizationExists(int id)
        {
            return (_context.VolunteerOrganizations?.Any(e => e.VolunteerOrgId == id)).GetValueOrDefault();
        }


        public IActionResult ExportExcel()
        {
            var animals = _context.VolunteerOrganizations.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "volunteer_orgs.csv");
                }
            }
            return View();
        }
    }
}
