using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Data
{
    class Book
    {
        private string name;
        private string company;
        private string writer;
        private string bookNo;
        private bool rental;

        public Book() { rental = false; }

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
