using EAttendance.Models;
using EAttendance.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EAttendance.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly EAttendanceContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        // GET: Dashboard
        public DashboardController(UserManager<ApplicationUser> userManager, EAttendanceContext context)
        {
            _userManager = userManager;
           
            _context = context;
        }

       
       
      
        public ActionResult AdminDashboard()
        {
            var currentUser = User.Identity.Name;
            if (currentUser == null)
            { return RedirectToAction("Login", "Account"); }

            //var currentSchool = Session["currentSchool"].ToString();

            if (User.Identity.IsAuthenticated)
            {
                
                    // to get total number of Students in the record
                    int countALLStudents = (from row in _context.Student
                                            select row).Count();
                    ViewBag.AllStudents = countALLStudents;

                    int countActiveStudents = (from row in _context.Student
                                               where row.ActiveStudents == true
                                               select row).Count();
                    ViewBag.ActiveStudent = countActiveStudents;

                    //var percentActiveStudent = countActiveStudents / countALLStudents * 100;
                    //ViewBag.percentActiveStudent = percentActiveStudent;

                    // to get total number of staffs in the record
                    int countALLStaffs = (from row in _context.Staff
                                          select row).Count();
                    ViewBag.AllStaffs = countALLStaffs;

                    int countActiveStaffs = (from row in _context.Staff
                                             where row.ActiveStaff == true
                                             select row).Count();
                    ViewBag.ActiveStaff = countActiveStaffs;

                //var percentActiveStaff = countActiveStaffs / countALLStaffs * 100;
                //ViewBag.percentActiveStaff = percentActiveStaff;

                //code to calculate the fee analysis using progress bar


                return View();

            }
            return RedirectToAction("Login", "Account");
        }
       

        public ActionResult StudentDashboard()
        {
            var currentUser = User.Identity.Name;
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var student = _context.Student.FirstOrDefault(m => m.MatricNo == currentUser).StudentId;
          
            
            
            return View();
        }


        public ActionResult StaffDashboards()
        {
            var currentUser = User.Identity.Name;
            if (currentUser == null)
            { return RedirectToAction("Login", "Account"); }
            // var currentSchool = Session["currentSchool"].ToString();
           
            return View();
        }
    }
}