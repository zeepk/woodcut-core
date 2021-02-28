using System;
using dotnet5_webapp.Models;

namespace dotnet5_webapp.Internal
{
    public class UserSearchResponse
    {
        public User? User { get; set; }
        public Boolean WasCreated { get; set; }

    }
}
