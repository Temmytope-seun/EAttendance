using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EAttendance.DbContexts;
using EAttendance.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace EAttendance.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly EAttendanceContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        static readonly WebClient MyClient = new WebClient();
        static readonly HttpClient client = new HttpClient();
        public StudentsController(EAttendanceContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var EAttendanceContext = _context.Student;
            return View(await EAttendanceContext.ToListAsync());
        }


        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "GlobalAdmin")]
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GlobalAdmin")]
        public async Task<IActionResult> Create(Student student)
        {
          

            if (ModelState.IsValid)
            {
               
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        //There is an error here
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Upload");
                        //"/" + Path.GetFileName(pic.FileName)
                        if (file.Length > 0)
                        {
                            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                            var extension = Path.GetExtension(file.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                student.ImageUrl = fileName;

                            }

                        }
                    }
                }
                _context.Add(student);
                await _context.SaveChangesAsync();
               

                var studentUser = new ApplicationUser { FirstName = student.FirstName, LastName = student.LastName, UserName = student.MatricNo, Email = student.Email};
                string password = student.LastName.ToLower() + "123";

                await _userManager.CreateAsync(studentUser, password);
                await _userManager.AddToRoleAsync(studentUser, "Student");
                await _context.SaveChangesAsync();

               
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
           
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "GlobalAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
           
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GlobalAdmin")]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    foreach (var Image in files)
                    {
                        if (Image != null && Image.Length > 0)
                        {
                            var file = Image;
                            //There is an error here
                            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Upload");
                            //"/" + Path.GetFileName(pic.FileName)
                            if (file.Length > 0)
                            {
                                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                                var extension = Path.GetExtension(file.FileName);
                                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                    student.ImageUrl = fileName;

                                }

                            }
                        }
                    }
                    _context.Update(student);
                    await _context.SaveChangesAsync();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
           
            return View(student);
        }

       

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
