using EventTicket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicket.Core.Interfaces;

public interface ITokenService
{
    string GenerateToken(AppUser user, IList<string> roles);
}
