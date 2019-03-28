using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using PortfolioInsight.Security;
using Xunit;

namespace PortfolioInsight.Domain.Tests.Security
{
    public class PasswordSecurity_Verify_Should
    {
        [Fact]
        public void Verify_Password_Silently()
        {
            string password = "1Password";
            string hash = "RyusGZUymx1tpIexmeuIK2rWO2JT+bt27WjzxSTtQ76Q77e/";

            var security = new PasswordSecurity();
            security.Verify(hash, password);
        }

        [Fact]
        public void Throw_AuthenticationException_If_Password_Invalid()
        {
            string password = "wrong";
            string hash = "RyusGZUymx1tpIexmeuIK2rWO2JT+bt27WjzxSTtQ76Q77e/";

            var security = new PasswordSecurity();
            Assert.Throws<AuthenticationException>(() => security.Verify(hash, password));
        }
    }
}
