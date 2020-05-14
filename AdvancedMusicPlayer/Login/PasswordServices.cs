using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedMusicPlayer.Login
{
    public class PasswordServices
    {
        /// <summary>
        /// it returns hashed password and salt if raw password  is passed
        /// </summary>
        /// <param name="rawPassword"></param>
        /// <returns>hashed password and salt</returns>
        public (string hashedPassword, byte[] salt) HashPassword(string rawPassword)
        {
            byte[] salt = GetSalt();
            string hashed = HashPassword(rawPassword, salt);
            return (hashed, salt);
        }

        /// <summary>
        /// it verifies hashded password, raw password and salt, and if they are matched then return true
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="rawPassword"></param>
        /// <param name="salt"></param>
        /// <returns>return true when hashed password, raw password and salt matched</returns>
        public bool VerifyPassword(string hashedPassword, string rawPassword, byte[] salt) =>
            hashedPassword == HashPassword(rawPassword, salt);

        /// <summary>
        /// hash raw password
        /// </summary>
        /// <param name="rawPassword"></param>
        /// <param name="salt"></param>
        /// <returns>return hashed password</returns>
        private string HashPassword(string rawPassword, byte[] salt) =>
        Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: rawPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

        /// <summary>
        /// get salt
        /// </summary>
        /// <returns>ret</returns>
        private byte[] GetSalt()
        {
            using (var gen = RandomNumberGenerator.Create())
            {
                var salt = new byte[128 / 8];
                gen.GetBytes(salt);
                return salt;
            }
        }

        public (string hashedPassword, byte[] salt) getHashedPassword(string password)
        {
            (string hashed, byte[] salt) = HashPassword(password);
            return (hashed, salt);
        }
    }
}
