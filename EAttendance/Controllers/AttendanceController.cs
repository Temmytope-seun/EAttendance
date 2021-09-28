using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAttendance.DbContexts;
using EAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace EAttendance.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly EAttendanceContext _context;

        public AttendanceController(EAttendanceContext context)
        {
            _context = context;
        }
        // GET: AttendanceController
        public ActionResult Index()
        {
            var currentUser = User.Identity.Name;
            if (currentUser == null)
            { return RedirectToAction("Login", "Account"); }
            if (User.IsInRole("Student"))
            {
                var attendance = _context.Attendance.Include(m =>m.Student).Include(m =>m.Course).Where(m => m.Student.MatricNo == currentUser).ToList();
                return View(attendance);
            }
            var allattendance = _context.Attendance.Include(m => m.Student).Include(m => m.Course).ToList();
            return View(allattendance);
        }

        // GET: AttendanceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AttendanceController/Create
        public ActionResult Post(string code, int id)
        {
            var getStudent = _context.Student.FirstOrDefault(m => m.FingerPrintID == id);
            var getCourse = _context.Course.FirstOrDefault(m => m.CourseCode == code);
            var markattendance = new Attendance
            {
                FingerPrintID = id,
                Student = getStudent,
                CourseCode = code,
                Course = getCourse

            };

            _context.Add(markattendance);
            _context.SaveChanges();
            return Ok("Ok");
        }

        // POST: AttendanceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: AttendanceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AttendanceController/Edit/5
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

        // GET: AttendanceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AttendanceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
