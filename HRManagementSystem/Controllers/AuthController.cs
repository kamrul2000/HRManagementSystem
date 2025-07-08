using Microsoft.AspNetCore.Mvc;
using HRManagementSystem.Models.ViewModel;
using HRManagementSystem.Interface;

namespace HRManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger )
        {
            _authService = authService;
            _logger = logger;

        }
       

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _authService.RegisterAsync(model);
                TempData["SuccessMessage"] = "Registration Successfull";
                return RedirectToAction("Login", "Auth");

            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(model);
            }
        }
    }
}
