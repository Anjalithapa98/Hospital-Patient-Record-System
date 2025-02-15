using Microsoft.AspNetCore.Mvc;
using HospitalPatientSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HospitalPatientSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HospitalPatientSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Doctor> _userManager;
        private readonly SignInManager<Doctor> _signInManager;

        // Constructor with injected dependencies
        public AccountController(ApplicationDbContext context, UserManager<Doctor> userManager, SignInManager<Doctor> signInManager)
        {
            _context = context;
            _userManager = userManager;  // Initialize UserManager
            _signInManager = signInManager;  // Initialize SignInManager
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = await _userManager.FindByEmailAsync(model.Email);
                if (doctor != null && await _userManager.CheckPasswordAsync(doctor, model.Password))
                {
                    // Check if the user has a role (Doctor or Admin)
                    var roles = await _userManager.GetRolesAsync(doctor);
                    if (roles.Contains("Doctor"))
                    {
                        await _signInManager.SignInAsync(doctor, isPersistent: false);  // Sign in the doctor
                        return RedirectToAction("DoctorDashboard", "Account");
                    }
                    else if (roles.Contains("Admin"))
                    {
                        await _signInManager.SignInAsync(doctor, isPersistent: false);  // Sign in the admin
                        return RedirectToAction("AdminDashboard", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User does not have a valid role.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }

        // Logout method (to allow doctor to log out)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();  // Sign out the user
            return RedirectToAction("Login");
        }

        // Doctor Dashboard (only accessible by Doctor)
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DoctorDashboard()
        {
            // Get the current logged-in doctor's ID
            var doctorIdString = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(doctorIdString))
            {
                return RedirectToAction("Login");
            }

            // Convert string ID to int safely
            if (!int.TryParse(doctorIdString, out int doctorId))
            {
                ModelState.AddModelError(string.Empty, "Invalid Doctor ID format.");
                return RedirectToAction("Login");
            }

            // Fetch patients assigned to this doctor
            var patients = await _context.Patients
                .Where(p => p.DoctorId == doctorId) // ✅ Now using int
                .ToListAsync();

            return View(patients);
        }


        // Admin Dashboard (only accessible by Admin)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            // Get the list of all doctors and their patients
            var doctorsWithPatients = await _context.Doctors
                .Include(d => d.Patients) // Assuming you have a navigation property for patients in Doctor
                .ToListAsync();

            return View(doctorsWithPatients);
        }
    }
}
