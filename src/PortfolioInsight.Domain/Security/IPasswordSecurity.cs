using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight.Security
{
    public interface IPasswordSecurity
    {
        string Hash(string password);
        void Verify(string storedPasswordHash, string password);
    }
}
