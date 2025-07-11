﻿using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace HRManagementSystem.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        [StringLength(60)]
        public string? Email { get; set; }
        [StringLength(160)]
        public string? Password { get; set; }
        public int? Mobile { get; set; }
        [StringLength(250)]
        public string? Address { get; set; }
        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
