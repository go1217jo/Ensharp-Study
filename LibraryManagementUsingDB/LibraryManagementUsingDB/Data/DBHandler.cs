using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LibraryManagementUsingDB.Data
{
   class DBHandler
   {
      MySqlConnection connect = null;
      MySqlCommand command;
      MySqlDataReader reader = null;

      public DBHandler()
      {
         String databaseConnect = "Server=localhost;Database=librarymanagement;Uid=root;Pwd=1q2w3e4r!;SslMode=none;Charset=utf8;";
         // connect MySQL
         connect = new MySqlConnection(databaseConnect);
         connect.Open();
      }

      public void Close()
      {
         if(reader != null)
            reader.Close();
         if(connect != null)
            connect.Close();
      }

      ~DBHandler()
      {
         Close();
      }

      public List<Student> GetAllMember()
      {
         List<Student> students = new List<Student>();
         string sqlQuery = "SELECT * FROM member;";
         reader = SelectQuery(sqlQuery);
         while(reader.Read())
         {
            Student student = new Student();
            student.StudentNo = reader["studentno"].ToString();
            student.name = reader["membername"].ToString();
            student.address = reader["address"].ToString();
            student.phoneNumber = reader["phonenumber"].ToString();
            students.Add(student);
         }
         reader.Close();
         return students;
      }

      public List<Book> GetAllBooks()
      {
         List<Book> books = new List<Book>();
         string sqlQuery = "SELECT * FROM book;";
         reader = SelectQuery(sqlQuery);
         while (reader.Read())
         {
            Book book = new Book();
            book.BookNo = reader["bookno"].ToString();
            book.Name = reader["bookname"].ToString();
            book.Company = reader["company"].ToString();
            book.Writer = reader["writer"].ToString();
            books.Add(book);
         }
         reader.Close();
         for (int i = 0; i < books.Count; i++)
            books[i].Rental = CheckRental(books[i].BookNo);

         return books;
      }

      public int GetBookCount()
      {
         int count = 0;
         MySqlDataReader reader = SelectQuery("SELECT * FROM BOOK;");
         while (reader.Read())
            count++;
         reader.Close();

         return count;
      }

      public bool InsertBook(string bookno, string bookname, string company, string writer)
      {
         string sqlQuery = "INSERT INTO book values ('" + bookno + "', '" + bookname + "', '" + company + "', '" + writer + "');";

         // 중복되는 도서가 없다면
         if (!IsOverlapBook(bookname, writer))
         {
            command = new MySqlCommand(sqlQuery, connect);
            if (command.ExecuteNonQuery() != 1)
               return false;
            else
               return true;
         }
         else
            return false;
      }

      public bool InsertBook(Data.Book book)
      {
         return InsertBook(book.BookNo, book.Name, book.Company, book.Writer);
      }

      public bool UpdateMemberInformation(string studentNo, string modification, string attribute)
      {
         string sqlQuery = "UPDATE member set " + attribute + " = '" + modification + "' WHERE studentno = '" + studentNo + "';";

         command = new MySqlCommand(sqlQuery, connect);
         if (command.ExecuteNonQuery() != 1)
            return false;
         else
            return true;
      }

      public bool UpdateBookInformation(string bookNo, string modification, string attribute)
      {
         string sqlQuery = "UPDATE book set " + attribute + " = '" + modification + "' WHERE bookno = '" + bookNo + "';";

         command = new MySqlCommand(sqlQuery, connect);
         if (command.ExecuteNonQuery() != 1)
            return false;
         else
            return true;
      }

      public bool InsertMember(string memberName, string studentNo, string address, string phoneNumber, string password)
      {
         string sqlQuery = "INSERT INTO member values ('" + studentNo + "', '" + memberName + "', '" + address + "', '" + phoneNumber + "', '" + password + "');";
         // 이미 있는 멤버와 학번이 같지 않은 경우 추가
         if (!IsOverlapMember(studentNo))
         {
            command = new MySqlCommand(sqlQuery, connect);
            if (command.ExecuteNonQuery() != 1)
               return false;
            else
               return true;
         }
         else
            return false;
      }

      public bool InsertMember(Student student)
      {
         return InsertMember(student.name, student.StudentNo, student.address, student.phoneNumber, student.Password);
      }

      public bool InsertRental(string sno, string bno, int loaned, string dueTo)
      {
         return true;
      }

      public bool IsOverlapMember(string studentNo)
      {
         string sqlQuery = "SELECT studentno FROM MEMBER WHERE studentno = '" + studentNo + "';";
         reader = SelectQuery(sqlQuery);
         if (IsThereOneValue(reader, "studentno"))
            return true;
         else 
            return false;
      }

      public bool IsOverlapBook(string bookName, string writer)
      {
         string sqlQuery = "SELECT bookno FROM book WHERE bookname = '" + bookName + "' AND writer = '"+writer+"';";
         reader = SelectQuery(sqlQuery);
         if (IsThereOneValue(reader, "bookno"))
            return true;
         else
            return false;
      }

      public MySqlDataReader SelectQuery(string query)
      {
         command = new MySqlCommand(query, connect);
         return command.ExecuteReader();
      }

      public bool IsThereOneValue(MySqlDataReader reader, string attribute)
      {
         List<string> members = new List<string>();

         while (reader.Read())
            members.Add(reader[attribute].ToString());
         reader.Close();

         if (members.Count == 1)
            return true;
         else
            return false;
      }

      public void LoadMemberInformation(Data.Student student)
      {
         string sqlQuery = "SELECT membername, address, phonenumber FROM MEMBER WHERE studentno = "+student.StudentNo+";";
         reader = SelectQuery(sqlQuery);
         while(reader.Read())
         {
            student.name = reader["membername"].ToString();
            student.address = reader["address"].ToString();
            student.phoneNumber = reader["phonenumber"].ToString();
         }
         reader.Close();
      }

      /*
      public void LoadBookInformation(Data.Book book, string bookno)
      {
         string sqlQuery = "SELECT * FROM book WHERE bookno = '" + bookno + "';";
         reader = SelectQuery(sqlQuery);
         while (reader.Read())
         {
            book.BookNo = reader["bookno"].ToString();
            book.Name = reader["bookname"].ToString();
            book.Company = reader["company"].ToString();
            book.Writer = reader["writer"].ToString();
            book.Rental = CheckRental(book.BookNo);
         }
         reader.Close();
      }*/

      public bool DeleteMember(string studentNo)
      {
         string sqlQuery1 = "DELETE FROM member WHERE studentno = '" + studentNo + "';";
         string sqlQuery2 = "SELECT studentno FROM member WHERE studentno = '" + studentNo + "';";

         // 삭제할 멤버가 존재한다면
         if (IsThereOneValue(SelectQuery(sqlQuery2), "studentno"))
         {
            command = new MySqlCommand(sqlQuery1, connect);
            if (command.ExecuteNonQuery() != 1)
               return false;
            else
               return true;
         }
         else
            return false;
      }

      public bool DeleteBook(string bookno)
      {
         string sqlQuery1 = "DELETE FROM book WHERE bookno = '" + bookno + "';";

         command = new MySqlCommand(sqlQuery1, connect);
         if (command.ExecuteNonQuery() != 1)
            return false;
         else
            return true;
      }

      public bool CheckRental(string bookno)
      {
         string sqlQuery = "SELECT bno FROM rental WHERE bno = '" + bookno + "';";

         if (IsThereOneValue(SelectQuery(sqlQuery), "bno"))
            return true;
         else
            return false;
      }

      public string SearchBook(string search, string attribute)
      {
         string bookno = "";
         string sqlQuery = "SELECT bookno FROM book WHERE " + attribute + " = '" + search + "';";

         reader = SelectQuery(sqlQuery);
         while (reader.Read())
            bookno = reader["bookno"].ToString();
         reader.Close();

         return bookno;
      }

      public bool Extension(string bookno)
      {
         // 만기일
         string dueDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
         string sqlQuery1 = "SELECT times FROM rental WHERE bno = '" + bookno + "';";
         string sqlQuery2 = "UPDATE rental set times = times+1, dueto = '" + dueDate + "' WHERE bno = '" + bookno + "';";

         reader = SelectQuery(sqlQuery1);
         int times = 0;
         while (reader.Read())
            times = (int)reader["times"];
         reader.Close();

         if (times < 3)
         {
            command = new MySqlCommand(sqlQuery2, connect);
            if (command.ExecuteNonQuery() != 1)
               return false;
            else
               return true;
         }
         else
            return false;
      }

      public bool IsBorrowSamePerson(string studentno, string bookno)
      {
         string sqlQuery = "SELECT bno FROM rental WHERE sno = '" + studentno + "' AND bno = '" + bookno + "';";
         if (IsThereOneValue(SelectQuery(sqlQuery), "bno"))
            return true;
         else
            return false;
      }

      public bool Rental(string studentNo, string bookno)
      {
         // 만기일
         string dueDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
         string sqlQuery = "INSERT INTO rental values ('" + studentNo + "', '" + bookno + "', " + "'"+dueDate+"', 0);";

         command = new MySqlCommand(sqlQuery, connect);

         if (command.ExecuteNonQuery() != 1)
            return false;
         else
            return true;
      }

      public string GetDueTo(string bookno)
      {
         string sqlQuery = "SELECT dueto FROM rental Where bno = '" + bookno + "';";
         string dueto=null;
         reader = SelectQuery(sqlQuery);
         while (reader.Read())
            dueto = reader["dueto"].ToString();
         reader.Close();
         return dueto;
      }

      public bool DeleteRental(string bookno)
      {
         string sqlQuery = "DELETE FROM rental WHERE bno = '" + bookno + "';";
         command = new MySqlCommand(sqlQuery, connect);
         if (command.ExecuteNonQuery() == 1)
            return false;
         else
            return true;
      }

      public List<Book> RentalList(string studentno)
      {
         List<Book> books = new List<Book>();
         string sqlQuery = "SELECT bookno, bookname, company, writer, dueto FROM book, rental WHERE book.bookno = rental.bno AND rental.sno = '" + studentno + "';";
         reader = SelectQuery(sqlQuery);
         while (reader.Read())
         {
            Book book = new Book();
            book.BookNo = reader["bookno"].ToString();
            book.Name = reader["bookname"].ToString();
            book.Company = reader["company"].ToString();
            book.Writer = reader["writer"].ToString();
            book.dueto = reader["dueto"].ToString();
            books.Add(book);
         }
         reader.Close();
         
         return books;
      }
   }
}
