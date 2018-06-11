using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberManager.Data
{
    public class MemberVO
    {
        string id;
        string password;
        string name;
        int sex;
        string birth;
        string mail;
        string phonenumber;
        string address;

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

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        public string Birth
        {
            get { return birth; }
            set { birth = value; }
        }

        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        public string PhoneNumber
        {
            get { return phonenumber; }
            set { phonenumber = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }
    }
}
