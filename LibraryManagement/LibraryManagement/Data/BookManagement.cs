using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace LibraryManagement.Data
{
    class BookManagement : Management
    {
        public enum Format {
            NameFormat, CompanyFormat, WriterFormat, BookNoFormat
        };

        ArrayList books;
        UI.ScreenUI drawer;

        public BookManagement()
        {
            books = new ArrayList();
            drawer = new UI.ScreenUI();
        }

        public Book ValueOf(int index)
        {
            return (Book)books[index];
        }

        public void Insert(object newObject)
        {
            Book newBook = (Book)newObject;
            books.Add(newObject);
        }

        public void Delete(object deleteObject)
        {
            Book deleteBook = (Book)deleteObject;
            books.Remove(deleteBook);
        }

        public ArrayList SearchBy(int format, string content)
        {
            Book temp;
            ArrayList returnList = new ArrayList();

            for (int i=0; i < books.Count; i++) {
                temp = (Book)books[i];
                switch (format)
                {
                    case (int)Format.NameFormat:
                        if (temp.Name.Equals(content))
                            returnList.Add(temp);
                        break;
                    case (int)Format.CompanyFormat:
                        if (temp.Company.Equals(content))
                            returnList.Add(temp);
                        break;
                    case (int)Format.WriterFormat:
                        if (temp.Writer.Equals(content))
                            returnList.Add(temp);
                        break;
                }
            }
            return returnList;
        }

        public void ModifyAs(int format, string content)
        {

        }

        public bool IsThereBook(string bookName, string writer)
        {
            for(int i=0;i<books.Count; i++) {
                Data.Book book = (Data.Book)books[i];
                if (book.Name.Equals(bookName) && book.Writer.Equals(writer))
                    return true;
            }
            return false;
        }

        public int Count() {
            return books.Count;
        }

        public int PrintBookList() {
            return drawer.PrintBookList(books);
        }
    }
}
