using HRManagementSystem.Data;
using HRManagementSystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace HRManagementSystem.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CompanyController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: /Company/Company
        public IActionResult Company()
        {
            var company = _context.Companies.FirstOrDefault();
            return View(company ?? new Company());
        }

        // POST: /Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company, IFormFile? LogoImage)
        {
            if (!ModelState.IsValid)
            {
                return View("Company", company);
            }

            // Handle logo upload if a new file is submitted
            if (LogoImage != null && LogoImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(LogoImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await LogoImage.CopyToAsync(stream);
                }

                company.LogoPath = "/uploads/" + fileName;
            }

            // Check if company already exists (assuming only 1 company allowed)
            var existingCompany = _context.Companies.FirstOrDefault();
            if (existingCompany != null)
            {
                // Update existing
                existingCompany.Name = company.Name;
                existingCompany.Address = company.Address;
                existingCompany.Email = company.Email;
                existingCompany.ContactNumber = company.ContactNumber;
                if (company.LogoPath != null)
                    existingCompany.LogoPath = company.LogoPath;

                _context.Companies.Update(existingCompany);
            }
            else
            {
                // Insert new
                _context.Companies.Add(company);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Company information saved successfully!";
            return RedirectToAction("Company");
        }
    }
}
