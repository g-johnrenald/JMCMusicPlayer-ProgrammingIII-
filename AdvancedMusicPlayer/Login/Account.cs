using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedMusicPlayer
{
    public class Account
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public byte[] Salt { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public Account(){}

        /// <summary>
        /// user information
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Account(string username, (string hashedPassword, byte[] salt) hashSalt)
        {
            this.Username = username;
            this.HashedPassword = hashSalt.hashedPassword;
            this.Salt = hashSalt.salt;
        }
    }
}
