using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Member
{
    // id와 password를 동시에 전달하기 위한 클래스
    class LoginInfo
    {
        string id;
        string password;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public LoginInfo(string id, string password) {
            this.id = id;
            this.password = password;
        }

        
    }
}
