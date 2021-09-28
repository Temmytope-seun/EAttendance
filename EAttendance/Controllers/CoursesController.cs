using EAttendance.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace EAttendance.Controllers
{
    public class CoursesController : Controller
    {
        private readonly EAttendanceContext _context;

        public CoursesController(EAttendanceContext context)
        {
            _context = context;
        }
        // GET: CoursesController
        public ActionResult Index()
        {
            var user = User.Identity.Name;
           
                var courseList = _context.Course.ToList();
           
            return View(courseList);
        }

        // GET: CoursesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CoursesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CoursesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: CoursesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CoursesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CoursesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CoursesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructorCourse = await _context.Course.FindAsync(id);
            _context.Course.Remove(instructorCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
