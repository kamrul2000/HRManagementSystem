﻿using HRManagementSystem.Data;
using HRManagementSystem.Interface;
using HRManagementSystem.Models;
using HRManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRManagementSystem.Services
{
    public class BranchService : IBranchService
    {
        private readonly ApplicationDbContext _context;
        private readonly BaseService _baseService;

        public BranchService(ApplicationDbContext context, BaseService baseService)
        {
            _context = context;
            _baseService = baseService;
        }

        public async Task<bool> CreateBranch(Branch branch)
        {
            try
            {
                var subscriptionIdClaim = _baseService.GetSubscriptionId();
                branch.SubscriptionId = subscriptionIdClaim;
                var company = await _context.Companies
                    .FirstOrDefaultAsync(c => c.SubscriptionId == subscriptionIdClaim);
                branch.CompanyId = company!.Id;

                _context.Branches.Add(branch);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> UpdateBranch(Branch branch)
        {
            try
            {
                var subscriptionIdClaim = _baseService.GetSubscriptionId();
                var existingBranch = await _context.Branches
                    .FirstOrDefaultAsync(b => b.SubscriptionId == subscriptionIdClaim && b.Id == branch.Id);

                if (existingBranch == null) return false;

                existingBranch.Name = branch.Name;
                existingBranch.Contact = branch.Contact;
                existingBranch.Address = branch.Address;
                existingBranch.UpdatedAt = DateTime.Now;

                _context.Branches.Update(existingBranch);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<string> DeleteBranch(int branchId)
        {
            var branch = await _context.Branches.FindAsync(branchId);
            if (branch == null)
                return "Branch not found.";

            var hasUsers = await _context.Users.AnyAsync(u => u.BranchId == branchId);
            if (hasUsers)
                return "Cannot delete this branch because users are assigned to it.";

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
            return "Branch deleted successfully.";
        }

        public async Task<List<Branch>> GetAllBranches()
        {
            var branchId = await _baseService.GetBranchId(_baseService.GetSubscriptionId(), _baseService.GetUserId());
            if (branchId == 0)
            {
                return await _context.Branches
                    .Where(b => b.SubscriptionId == _baseService.GetSubscriptionId())
                    .ToListAsync();
            }
            return await _context.Branches
                .Where(b => b.Id == branchId && b.SubscriptionId == _baseService.GetSubscriptionId())
                .ToListAsync();
        }
    }
}
