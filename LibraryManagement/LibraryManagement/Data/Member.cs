using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace LibraryManagement.Data
{
    class Member
    {
        // 이름, 학번, 주소, 전화번호, 비밀번호
        private string name;
        private string studentNo;
        private string address;
        private string phoneNumber;
        private string password;
        // 대출한 도서 목록
        public ArrayList rentalBookList;

        public Member()
        {
            rentalBookList = new ArrayList();
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string StudentNo
        {
            get { return studentNo; }
            set { studentNo = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
