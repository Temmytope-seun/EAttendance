using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EAttendance.DbContexts;
using EAttendance.Models;

namespace EAttendance.Controllers
{
    public class InstructorCoursesController : Controller
    {
        private readonly EAttendanceContext _context;

        public InstructorCoursesController(EAttendanceContext context)
        {
            _context = context;
        }

        // GET: InstructorCourses
        public async Task<IActionResult> Index()
        {
            var user = User.Identity.Name;
            if (User.IsInRole("Staff"))
            {
                var getStaff = _context.Staff.FirstOrDefault(m => m.Email == user);
                var courseList = _context.InstructorCourse.Include(i => i.Staff).Include(i => i.Course).Where(m => m.StaffId == getStaff.StaffId).ToList();
                return View(courseList);
            }
            var EAttendanceContext = _context.InstructorCourse.Include(i => i.Staff).Include(i => i.Course);
            return View(await EAttendanceContext.ToListAsync());
        }

        // GET: InstructorCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorCourse = await _context.InstructorCourse
                .Include(i => i.Staff)
                .Include(i => i.Course)
                .FirstOrDefaultAsync(m => m.InstructorCourseId == id);
            if (instructorCourse == null)
            {
                return NotFound();
            }

            return View(instructorCourse);
        }

        // GET: InstructorCourses/Create
        public IActionResult Create()
        {
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "FullName");
            ViewData["SubjectId"] = new SelectList(_context.Course, "CourseId", "CourseTitle");
            return View();
        }

        // POST: InstructorCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InstructorCourse instructorCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructorCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "FullName", instructorCourse.StaffId);
            ViewData["SubjectId"] = new SelectList(_context.Course, "CourseId", "CourseTitle", instructorCourse.CourseId);
            return View(instructorCourse);
        }

        // GET: InstructorCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorCourse = await _context.InstructorCourse.FindAsync(id);
            if (instructorCourse == null)
            {
                return NotFound();
            }
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "FullName", instructorCourse.StaffId);
            ViewData["SubjectId"] = new SelectList(_context.Course, "CourseId", "CourseTitle", instructorCourse.CourseId);
            return View(instructorCourse);
        }

        // POST: InstructorCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InstructorCourse instructorCourse)
        {
            if (id != instructorCourse.InstructorCourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructorCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorCourseExists(instructorCourse.InstructorCourseId))
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
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "FullName", instructorCourse.StaffId);
            ViewData["SubjectId"] = new SelectList(_context.Course, "CourseId", "CourseTitle", instructorCourse.CourseId);
            return View(instructorCourse);
        }

        // GET: InstructorCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorCourse = await _context.InstructorCourse
                .Include(i => i.Staff)
                .Include(i => i.Course)
                .FirstOrDefaultAsync(m => m.InstructorCourseId == id);
            if (instructorCourse == null)
            {
                return NotFound();
            }

            return View(instructorCourse);
        }

        // POST: InstructorCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructorCourse = await _context.InstructorCourse.FindAsync(id);
            _context.InstructorCourse.Remove(instructorCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorCourseExists(int id)
        {
            return _context.InstructorCourse.Any(e => e.InstructorCourseId == id);
        }
    }
}
