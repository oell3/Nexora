using Microsoft.AspNetCore.Identity;


namespace Nexora.Services
{
    public class AuthService
    {
        private readonly PasswordHasher<object> _hasher = new();
        public string _Hashpassword(string password)
        {
            return _hasher.HashPassword(null, password);
        }


        public bool VerifyPassword(string password, string hash)
        {
            var result = _hasher.VerifyHashedPassword(null, hash, password);

            return result == PasswordVerificationResult.Success;
        }


    }

}
