using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAttendance.DbContexts;
using EAttendance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FlexSchool.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EAttendanceContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
             EAttendanceContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
          
            _context = context;
            
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            //ViewBag.SchoolName = new SelectList(_context.SchoolInfoes, "SchoolName", "SchoolName");
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var ExistingPhoneNo = _userManager.Users.FirstOrDefault(c => c.PhoneNumber == viewModel.PhoneNumber);
                if (ExistingPhoneNo != null)
                {
                    ModelState.AddModelError(string.Empty, "Phone number exist");
                    //ViewBag.SchoolName = new SelectList(_context.SchoolInfoes, "SchoolName", "SchoolName", viewModel.SchoolName);
                    return View();
                }
                else
                {
                    var user = new ApplicationUser
                    {
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        PhoneNumber = viewModel.PhoneNumber,
                        UserName = viewModel.Email,
                        Email = viewModel.Email,
                        SchoolName = viewModel.SchoolName
                    };
                    var result = await _userManager.CreateAsync(user, viewModel.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "InstituteAdmin");


                        return RedirectToAction("CreatedUser", "Account", new { id = user.Id });
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }

            //ViewBag.SchoolName = new SelectList(_context.SchoolInfoes, "SchoolName", "SchoolName", viewModel.SchoolName);
            return View(viewModel);
        }


        [AllowAnonymous]
        public async Task<IActionResult> CreatedUser(string id)
        {
          
            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel viewModel, string returnUrl)
        {
           
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(viewModel.Username);
                //var userEmail = await _userManager.FindByEmailAsync(viewModel.Username);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return View(viewModel);
                }
                var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    HttpContext.Session.SetString("currentUser", viewModel.Username);
                   
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {

                        return Redirect(returnUrl);
                    }
                    else
                    {
                        if (await _userManager.IsInRoleAsync(user, "GlobalAdmin"))
                        {
                            return RedirectToAction("AdminDashboard", "Dashboard");
                        }
                     else if (await _userManager.IsInRoleAsync(user, "Staff"))
                        {
                            return RedirectToAction("StaffDashboards", "Dashboard");
                        }
                        else 
                        {
                            //HttpContext.Session.SetString("currentSchool", user.SchoolName);
                            return RedirectToAction("StudentDashboard", "Dashboard");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(viewModel);
        }

       

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"email {email} is already in use");
            }
        }

        public IActionResult Signout()
        {
            HttpContext.Session.SetString("user", "");
            return RedirectToAction("Signin", "Account");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}