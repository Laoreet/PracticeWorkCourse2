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
    public class VolunteerApplicationsController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public VolunteerApplicationsController(animal_shelter_testContext context)
        {
            _context = context;
        }

        // GET: VolunteerApplications
        public async Task<IActionResult> Index(int pg = 1)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                const int pageSize = 10;

                if (pg < 1)
                    pg = 1;

                int recsCount = _context.VolunteerApplications.Count();

                var paging = new Paging(recsCount, pg, pageSize);
                paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();

                int recSkip = (pg - 1) * pageSize;

                List<VolunteerApplication> animals = _context.VolunteerApplications.Include(p => p.VolunteerGroup).Include(b => b.VolunteerOrg).Skip(recSkip).Take(paging.PageSize).ToList();

                this.ViewBag.Paging = paging;
                return View(animals.OrderBy(a => a.VolunteerApplicationId));
            }
            return Redirect("~/Home/Index");
        }

        // GET: VolunteerApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.VolunteerApplications == null)
                {
                    return NotFound();
                }

                var volunteerApplication = await _context.VolunteerApplications
                    .Include(v => v.VolunteerGroup)
                    .Include(v => v.VolunteerOrg)
                    .FirstOrDefaultAsync(m => m.VolunteerApplicationId == id);
                if (volunteerApplication == null)
                {
                    return NotFound();
                }

                return View(volunteerApplication);
            }
            return Redirect("~/Home/Index");
        }

        // GET: VolunteerApplications/Create
        public IActionResult Create()
        {
                ViewData["VolunteerGroupId"] = new SelectList(_context.VolunteerGroups, "VolunteerGroupId", "WorkType");
                ViewData["VolunteerOrgId"] = new SelectList(_context.VolunteerOrganizations, "VolunteerOrgId", "OrgName");
                ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
                return View();
        }

        // POST: VolunteerApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VolunteerApplicationId,VolunteerGroupId,VolunteerOrgId,Lastname,Firstname,Patronymic,Sex,Age,PhoneNumber")] VolunteerApplication volunteerApplication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteerApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VolunteerGroupId"] = new SelectList(_context.VolunteerGroups, "VolunteerGroupId", "WorkType", volunteerApplication.VolunteerGroupId);
            ViewData["VolunteerOrgId"] = new SelectList(_context.VolunteerOrganizations, "VolunteerOrgId", "OrgName", volunteerApplication.VolunteerOrgId);
            ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
            return View(volunteerApplication);
        }

        // GET: VolunteerApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.VolunteerApplications == null)
                {
                    return NotFound();
                }

                var volunteerApplication = await _context.VolunteerApplications.FindAsync(id);
                if (volunteerApplication == null)
                {
                    return NotFound();
                }
                ViewData["VolunteerGroupId"] = new SelectList(_context.VolunteerGroups, "VolunteerGroupId", "WorkType", volunteerApplication.VolunteerGroupId);
                ViewData["VolunteerOrgId"] = new SelectList(_context.VolunteerOrganizations, "VolunteerOrgId", "OrgName", volunteerApplication.VolunteerOrgId);
                ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
                return View(volunteerApplication);
            }
            return Redirect("~/Home/Index");
        }

        // POST: VolunteerApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerApplicationId,VolunteerGroupId,VolunteerOrgId,Lastname,Firstname,Patronymic,Sex,Age,PhoneNumber")] VolunteerApplication volunteerApplication)
        {
            if (id != volunteerApplication.VolunteerApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerApplicationExists(volunteerApplication.VolunteerApplicationId))
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
            ViewData["VolunteerGroupId"] = new SelectList(_context.VolunteerGroups, "VolunteerGroupId", "WorkType", volunteerApplication.VolunteerGroupId);
            ViewData["VolunteerOrgId"] = new SelectList(_context.VolunteerOrganizations, "VolunteerOrgId", "OrgName", volunteerApplication.VolunteerOrgId);
            ViewData["HumanSexes"] = new SelectList(DB.humanSexes);
            return View(volunteerApplication);
        }

        // GET: VolunteerApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (DB.status != "guest" && DB.status != "client")
            {
                if (id == null || _context.VolunteerApplications == null)
                {
                    return NotFound();
                }

                var volunteerApplication = await _context.VolunteerApplications
                    .Include(v => v.VolunteerGroup)
                    .Include(v => v.VolunteerOrg)
                    .FirstOrDefaultAsync(m => m.VolunteerApplicationId == id);
                if (volunteerApplication == null)
                {
                    return NotFound();
                }

                return View(volunteerApplication);
            }
            return Redirect("~/Home/Index");
        }

        // POST: VolunteerApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VolunteerApplications == null)
            {
                return Problem("Entity set 'animal_shelter_testContext.VolunteerApplications'  is null.");
            }
            var volunteerApplication = await _context.VolunteerApplications.FindAsync(id);
            if (volunteerApplication != null)
            {
                _context.VolunteerApplications.Remove(volunteerApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerApplicationExists(int id)
        {
            return (_context.VolunteerApplications?.Any(e => e.VolunteerApplicationId == id)).GetValueOrDefault();
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.VolunteerApplications.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "volunteer_applications.csv");
                }
            }
            return View();
        }
    }
}
