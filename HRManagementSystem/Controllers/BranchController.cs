using HRManagementSystem.Interface;
using HRManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.Controllers
{
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        // GET: /Branch/Register
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var branches = await _branchService.GetAllBranches();
            return View(branches); // ✅ Pass model to the view
        }

        // POST: /Branch/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Branch branch)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fix the errors in the form.";
                return View(branch);
            }

            var success = await _branchService.CreateBranch(branch);
            if (success)
            {
                TempData["Success"] = "Branch created successfully!";
                return RedirectToAction("Register");
            }

            TempData["Error"] = "Something went wrong while creating the branch.";
            return View(branch);
        }

        // GET: /Branch/List
        public async Task<IActionResult> List()
        {
            var branches = await _branchService.GetAllBranches();
            return View(branches);
        }

        // GET: /Branch/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var branches = await _branchService.GetAllBranches();
            var branch = branches.FirstOrDefault(b => b.Id == id);
            if (branch == null)
                return NotFound();

            return View(branch);
        }

        // POST: /Branch/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Branch branch)
        {
            if (!ModelState.IsValid)
                return View(branch);

            var result = await _branchService.UpdateBranch(branch);
            if (result)
            {
                TempData["Success"] = "Branch updated successfully!";
                return RedirectToAction("List");
            }

            TempData["Error"] = "Error updating branch.";
            return View(branch);
        }

        // GET: /Branch/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _branchService.DeleteBranch(id);
            TempData["Message"] = result;
            return RedirectToAction("List");
        }
    }
}
