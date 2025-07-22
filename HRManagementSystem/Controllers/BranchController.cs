using Microsoft.AspNetCore.Mvc;
using HRManagementSystem.Interface;
using HRManagementSystem.Models;

namespace HRManagementSystem.Controllers
{
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly ILogger<BranchController> _logger;

        public BranchController(IBranchService branchService, ILogger<BranchController> logger)
        {
            _branchService = branchService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Branch()
        {
            var branches = await _branchService.GetAllBranches();
            return View(branches);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBranch(Branch branch)
        {
            if (branch.Id == 0)
            {
                bool isCreated = await _branchService.CreateBranch(branch);
                if (!isCreated)
                {
                    TempData["ErrorMessage"] = "A Branch already exists for this SubscriptionId.";
                    return RedirectToAction("Branch");
                }
                TempData["SuccessMessage"] = "Branch Created Successfully";
            }
            else
            {
                bool isUpdated = await _branchService.UpdateBranch(branch);
                if (!isUpdated)
                {
                    TempData["ErrorMessage"] = "Branch name already exists or update failed";
                    return RedirectToAction("Branch");
                }
                TempData["SuccessMessage"] = "Branch Updated Successfully";
            }

            return RedirectToAction("Branch");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var result = await _branchService.DeleteBranch(id);

            if (result == "Branch not found.")
            {
                TempData["ErrorMessage"] = result;
                return RedirectToAction("Branch");
            }

            if (result == "Cannot delete this branch because users are assigned to it.")
            {
                TempData["ErrorMessage"] = result;
                return RedirectToAction("Branch");
            }

            TempData["SuccessMessage"] = result;
            return RedirectToAction("Branch");
        }
    }
}
