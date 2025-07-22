using System;
using HRManagementSystem.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Services
{
    public class BaseService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseService(ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetSubscriptionId()
        {
            var subscriptionIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("SubscriptionId")?.Value;
            return int.TryParse(subscriptionIdClaim, out var subscriptionId) ? subscriptionId : 0;
        }

        public int GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            return int.TryParse(userId, out var id) ? id : 0;
        }

        public async Task<int> GetBranchId(int subscriptionId, int userId)
        {
            var userBranch = await _appDbContext.Users.Where(u => u.Id == userId && u.SubscriptionId == subscriptionId).FirstOrDefaultAsync();
            return userBranch?.BranchId ?? 0;
        }
    }
}
