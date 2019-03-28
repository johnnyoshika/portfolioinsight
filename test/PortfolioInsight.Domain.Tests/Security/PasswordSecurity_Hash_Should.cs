using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using PortfolioInsight.Security;
using Xunit;

namespace PortfolioInsight.Domain.Tests.Security
{
    public class PasswordSecurity_Hash_Should
    {
        [Fact]
        public void Hash_Password_Uniquely()
        {
            var security = new PasswordSecurity();

            string password = "1Password";
            string hash1 = security.Hash(password);
            string hash2 = security.Hash(password);

            Assert.NotEqual(hash1, hash2);
        }
    }
}
