using System;
using System.Collections.Generic;
using System.Text;
using PortfolioInsight.Users;

namespace PortfolioInsight
{
    public partial class UserEntity
    {
        public User ToDto() =>
            new User
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                CreatedAt = CreatedAt,
                LastActivityAt = LastActivityAt,
                ActivityCount = ActivityCount,
                LastLoginAt = LastLoginAt,
                LoginCount = LoginCount
            };

        public UserEntity Assign(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            CreatedAt = user.CreatedAt;
            LastActivityAt = user.LastActivityAt;
            ActivityCount = user.ActivityCount;
            LastLoginAt = user.LastLoginAt;
            LoginCount = user.LoginCount;
            return this;
        }
    }
}
