using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Data
{
    class Book
    {
        // 이름, 출판사, 저자, 책번호, 대출여부
        private string name;
        private string company;
        private string writer;
        private string bookNo;
        private bool rental;
        public UI.ScreenUI drawer;

        public Book() { rental = false; }

        // 행 내용을 글자 간격에 맞춰 출력
        public void PrintInformation() {
            drawer = new UI.ScreenUI();
            Console.Write("       {0}", drawer.PrintFixString(BookNo, 14));
            Console.Write(" {0}", drawer.PrintFixString(Name, 25));
            Console.Write(" {0}", drawer.PrintFixString(Company, 15));
            Console.Write(" {0}", drawer.PrintFixString(Writer, 20));
            if (Rental)
                Console.WriteLine("     대출 중    ");
            else
                Console.WriteLine("     보유 중    ");
        }

        public Book(string name, string company, string writer)
        {
            rental = false;
            this.name = name;
            this.company = company;
            this.writer = writer;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Company
        {
            get { return company; }
            set { company = value; }
        }

        public string Writer
        {
            get { return writer; }
            set { writer = value; }
        }

        public string BookNo
        {
            get { return bookNo; }
            set { bookNo = value; }
        }

        public bool Rental
        {
            get { return rental; }
            set { rental = value; }
        }
    }
}
