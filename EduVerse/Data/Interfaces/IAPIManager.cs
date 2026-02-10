using System;
using System.Collections.Generic;
using System.Text;

namespace EduVerse.Data.Interfaces
{
    public interface IAPIManager
    {
        Task<bool> GetIsRegistrationOpenAsync();
        Task<DateTime> GetRegistrationOpenUntilDateAsync();
    }
}
