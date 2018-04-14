using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace LibraryManagement.Data
{
    class BookManagement : IManagement
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

        // 인덱스를 가지고 서적을 반환한다.
        public Book ValueOf(int index) 
        {
            return (Book)books[index];
        }

        // 도서 객체를 매개변수로 받아 도서 리스트에 추가한다.
        public void Insert(object newObject)
        {
            Book newBook = (Book)newObject;
            books.Add(newObject);
        }

        // 도서 객체를 매개변수로 받아 도서 리스트에 삭제한다.
        public void Delete(object deleteObject)
        {
            Book deleteBook = (Book)deleteObject;
            books.Remove(deleteBook);
        }

        // format에 따라 content에 맞는 것을 찾는다.
        public ArrayList SearchBy(int format, string content)
        {
            Book temp;
            ArrayList returnList = new ArrayList();

            // 도서리스트 내 일치하는 도서를 형식에 따라 검색한다.
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

        // 도서리스트 내에 현재 같은 도서가 있는지 확인, primary key는 책명, 저자이다.
        public bool IsThereBook(string bookName, string writer)
        {
            for(int i=0;i<books.Count; i++) {
                Data.Book book = (Data.Book)books[i];
                if (book.Name.Equals(bookName) && book.Writer.Equals(writer))
                    return true;
            }
            return false;
        }

        // 현재 도서 개수 반환
        public int Count() {
            return books.Count;
        }

        // 도서리스트 출력
        public int PrintBookList() {
            return drawer.PrintBookList(books);
        }
    }
}
