using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight.Users
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastActivityAt { get; set; }
        public int ActivityCount { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int LoginCount { get; set; }
    }
}
