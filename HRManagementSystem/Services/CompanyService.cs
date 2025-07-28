using HRManagementSystem.Data;
using HRManagementSystem.Interface;
using HRManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CompanyService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            return await _context.Companies.Include(c => c.Subscription).ToListAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public async Task AddCompanyAsync(Company company)
        {
            if (company.LogoImage != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads/logos");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + company.LogoImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await company.LogoImage.CopyToAsync(fileStream);
                }

                company.LogoPath = "/uploads/logos/" + uniqueFileName;
            }

            company.CreatedAt = DateTime.Now;
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCompanyAsync(Company company)
        {
            var existing = await _context.Companies.FindAsync(company.Id);
            if (existing != null)
            {
                existing.Name = company.Name;
                existing.VatRegistrationNo = company.VatRegistrationNo;
                existing.TinNo = company.TinNo;
                existing.WebsiteLink = company.WebsiteLink;
                existing.Email = company.Email;
                existing.ContactNumber = company.ContactNumber;
                existing.Address = company.Address;
                existing.SubscriptionId = company.SubscriptionId;
                existing.UpdatedAt = DateTime.Now;

                if (company.LogoImage != null)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads/logos");
                    Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + company.LogoImage.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await company.LogoImage.CopyToAsync(fileStream);
                    }

                    existing.LogoPath = "/uploads/logos/" + uniqueFileName;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }
        }
    }
}
