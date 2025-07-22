using HRManagementSystem.Models;

namespace HRManagementSystem.Interface
{
    public interface IBranchService
    {
        Task<List<Branch>> GetAllBranches();
        Task<bool> CreateBranch(Branch branch);
        Task<bool> UpdateBranch(Branch branch);
        Task<string> DeleteBranch(int id);
    }
}
