using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedMusicPlayer.Login
{
    public class Authentication
    {
        PasswordServices ps = new PasswordServices();

        /// <summary>
        /// Authenticates account using password services
        /// </summary>
        /// <param name="account"></param>
        public bool Authenticate((string username, string rawPassword) account, List<Account> accounts)
        {
            // get matched account from account list
            Account matchedAccount = accounts.SingleOrDefault(u => u.Username == account.username);
            if (matchedAccount != null && ps.VerifyPassword(matchedAccount.HashedPassword, account.rawPassword, matchedAccount.Salt)) // veryfies password
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
