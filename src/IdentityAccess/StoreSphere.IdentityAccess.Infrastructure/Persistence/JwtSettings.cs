using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Infrastructure.Persistence
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;   // symmetric key for signing
        public string Issuer { get; set; } = string.Empty;   // who issued the token
        public string Audience { get; set; } = string.Empty; // who the token is for
        public int ExpiryMinutes { get; set; } = 60;         // token validity
    }
}
