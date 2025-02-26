using Codexam.WebAPI.Entities;

namespace Codexam.WebAPI.Utilities.JWT
{
    public interface ITokenHelper
    {
        public AccessToken CreateToken(User user);
    }
}
