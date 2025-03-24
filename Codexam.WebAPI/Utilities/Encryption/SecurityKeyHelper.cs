using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Codexam.WebAPI.Utilities.Encryption
{
    public static class SecurityKeyHelper
    {
        public static SecurityKey CreateSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}
