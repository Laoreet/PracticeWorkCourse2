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
    public class VolunteersController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public VolunteersController(animal_shelter_testContext context)
        {
            _context = context;
        }

        // GET: Volunteers
        public async Task<IActionResult> Index(int pg = 1)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                const int pageSize = 10;

                if (pg < 1)
                    pg = 1;

                int recsCount = _context.Volunteers.Count();

                var paging = new Paging(recsCount, pg, pageSize);
                paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();
                int recSkip = (pg - 1) * pageSize;

                List<Volunteer> volunteers = _context.Volunteers.Include(b => b.VolunteerOrg).Include(v => v.VolunteerGroup).Skip(recSkip).Take(paging.PageSize).ToList();

                this.ViewBag.Paging = paging;
                return View(volunteers.OrderBy(a => a.VolunteerId));
            }
            return Redirect("~/Home/Index");
        }

        // GET: Volunteers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.Volunteers == null)
                {
                    return NotFound();
                }

                var volunteer = await _context.Volunteers
                    .Include(v => v.VolunteerGroup)
                    .Include(v => v.VolunteerOrg)
                    .FirstOrDefaultAsync(m => m.VolunteerId == id);
                if (volunteer == null)
                {
                    return NotFound();
                }

                return View(volunteer);
            }
            return Redirect("~/Home/Index");
        }

        // GET: Volunteers/Create
        public IActionResult Create()
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                ViewData["VolunteerGroupId"] = new SelectList(_context.VolunteerGroups, "VolunteerGroupId", "WorkType");
                ViewData["VolunteerOrgId"] = new SelectList(_context.VolunteerOrganizations, "VolunteerOrgId", "OrgName");
                ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
                return View();
            }
            return Redirect("~/Home/Index");
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VolunteerId,VolunteerOrgId,VolunteerGroupId,Lastname,Firstname,Patronymic,Sex,Age,PhoneNumber")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VolunteerGroupId"] = new SelectList(_context.VolunteerGroups, "VolunteerGroupId", "WorkType", volunteer.VolunteerGroupId);
            ViewData["VolunteerOrgId"] = new SelectList(_context.VolunteerOrganizations, "VolunteerOrgId", "OrgName", volunteer.VolunteerOrgId);
            ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
            return View(volunteer);
        }

        // GET: Volunteers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.Volunteers == null)
                {
                    return NotFound();
                }

                var volunteer = await _context.Volunteers.FindAsync(id);
                if (volunteer == null)
                {
                    return NotFound();
                }
                ViewData["VolunteerGroupId"] = new SelectList(_context.VolunteerGroups, "VolunteerGroupId", "WorkType", volunteer.VolunteerGroupId);
                ViewData["VolunteerOrgId"] = new SelectList(_context.VolunteerOrganizations, "VolunteerOrgId", "OrgName", volunteer.VolunteerOrgId);
                ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
                return View(volunteer);
            }
            return Redirect("~/Home/Index");
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerId,VolunteerOrgId,VolunteerGroupId,Lastname,Firstname,Patronymic,Sex,Age,PhoneNumber")] Volunteer volunteer)
        {
            if (id != volunteer.VolunteerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerExists(volunteer.VolunteerId))
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
            ViewData["VolunteerGroupId"] = new SelectList(_context.VolunteerGroups, "VolunteerGroupId", "WorkType", volunteer.VolunteerGroupId);
            ViewData["VolunteerOrgId"] = new SelectList(_context.VolunteerOrganizations, "VolunteerOrgId", "OrgName", volunteer.VolunteerOrgId);
            ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
            return View(volunteer);
        }

        // GET: Volunteers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.Volunteers == null)
                {
                    return NotFound();
                }

                var volunteer = await _context.Volunteers
                    .Include(v => v.VolunteerGroup)
                    .Include(v => v.VolunteerOrg)
                    .FirstOrDefaultAsync(m => m.VolunteerId == id);
                if (volunteer == null)
                {
                    return NotFound();
                }

                return View(volunteer);
            }
            return Redirect("~/Home/Index");
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Volunteers == null)
            {
                return Problem("Entity set 'animal_shelter_testContext.Volunteers'  is null.");
            }
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool VolunteerExists(int id)
        {
            return (_context.Volunteers?.Any(e => e.VolunteerId == id)).GetValueOrDefault();
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.Volunteers.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "volunteers.csv");
                }
            }
            return View();
        }
    }
}
