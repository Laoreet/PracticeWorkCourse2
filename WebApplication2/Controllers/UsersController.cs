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
    public class UsersController : Controller
    {
        private readonly animal_shelter_testContext _context;

        public UsersController(animal_shelter_testContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(int pg = 1)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                const int pageSize = 10;

                if (pg < 1)
                    pg = 1;

                int recsCount = _context.Users.Count();

                var paging = new Paging(recsCount, pg, pageSize);
                paging.CurrentTable = this.ControllerContext.RouteData.Values["controller"].ToString();

                int recSkip = (pg - 1) * pageSize;

                List<User> animals = _context.Users.Skip(recSkip).Take(paging.PageSize).ToList();

                this.ViewBag.Paging = paging;
                return View(animals.OrderBy(a => a.UserId));
            }
            return Redirect("~/Home/Index");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                if (id == null || _context.Users == null)
                {
                    return NotFound();
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(m => m.UserId == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            return Redirect("~/Home/Index");
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                return View();
            }
            return Redirect("~/Home/Index");
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Login,Password,Email,Level")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                if (id == null || _context.Users == null)
                {
                    return NotFound();
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            return Redirect("~/Home/Index");
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Login,Password,Email,Level")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (DB.status == "admin" || DB.status == "director")
            {
                if (id == null || _context.Users == null)
                {
                    return NotFound();
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(m => m.UserId == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            return Redirect("~/Home/Index");
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'animal_shelter_testContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        private bool UserExists(string login)
        {
            return (_context.Users?.Any(e => e.Login == login)).GetValueOrDefault();
        }



        public IActionResult Registration()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration([Bind("UserId,Login,Password,Email,Level")] User user)
        {
            user.Level = "client";
            if (!UserExists(user.Login) && ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                RedirectToAction("Login");
            }
            ModelState.AddModelError("", "Пользователь с таким логином уже существует!");
            return View(user);
        }


        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Login([Bind("UserId,Login,Password, Level")] User user)
        {
            if (UserExists(user.Login))
            {
                if (_context.Users.Any(a => a.Login == user.Login && a.Password == user.Password))
                {
                    DB.status = _context.Users.First(a => a.Login == user.Login).Level;

                    return Redirect("~/Home/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Неверный пароль!");
                    return View(user);
                }
            }
            ModelState.AddModelError("", "Неверный логин!");
            return View(user);

        }

        public IActionResult Logout()
        {
            DB.status = "guest";
            return Redirect("~/Home/Index");
        }

        public IActionResult ExportExcel()
        {
            var animals = _context.Users.ToList();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(DB.ToDataTable(animals.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users.csv");
                }
            }
            return View();
        }
    }
}
