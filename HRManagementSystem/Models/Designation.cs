﻿namespace HRManagementSystem.Models
{
    public class Designation
    {
        public int ID { get; set; }
        public string? DesignationName { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
