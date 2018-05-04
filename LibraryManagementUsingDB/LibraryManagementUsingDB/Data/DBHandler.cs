using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LibraryManagementUsingDB.Data
{
   /// <summary>
   ///  DB를 관리하는 클래스
   /// </summary>
   class DBHandler
   {
      MySqlConnection connect = null;
      MySqlCommand command;
      MySqlDataReader reader = null;

      // DB 접속
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

      // 모든 멤버 리스트를 반환한다.
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

      // 모든 책 리스트를 반환한다.
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

      // 현재 등록된 책 권수를 반환
      public int GetBookCount()
      {
         int count = 0;
         MySqlDataReader reader = SelectQuery("SELECT * FROM BOOK;");
         while (reader.Read())
            count++;
         reader.Close();

         return count;
      }

      // 책을 DB에 추가한다.
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

      // 책을 DB에 추가한다(Overriding)
      public bool InsertBook(Data.Book book)
      {
         return InsertBook(book.BookNo, book.Name, book.Company, book.Writer);
      }

      // 멤버 정보를 수정한다.
      public bool UpdateMemberInformation(string studentNo, string modification, string attribute)
      {
         string sqlQuery = "UPDATE member set " + attribute + " = '" + modification + "' WHERE studentno = '" + studentNo + "';";

         command = new MySqlCommand(sqlQuery, connect);
         if (command.ExecuteNonQuery() != 1)
            return false;
         else
            return true;
      }

      // 책 정보를 수정한다.
      public bool UpdateBookInformation(string bookNo, string modification, string attribute)
      {
         string sqlQuery = "UPDATE book set " + attribute + " = '" + modification + "' WHERE bookno = '" + bookNo + "';";

         command = new MySqlCommand(sqlQuery, connect);
         if (command.ExecuteNonQuery() != 1)
            return false;
         else
            return true;
      }

      // 멤버를 DB에 추가한다.
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

      // 멤버를 DB에 추가한다.(Overriding)
      public bool InsertMember(Student student)
      {
         return InsertMember(student.name, student.StudentNo, student.address, student.phoneNumber, student.Password);
      }

      // 중복되는 학번이 있는지 확인한다.
      public bool IsOverlapMember(string studentNo)
      {
         string sqlQuery = "SELECT studentno FROM MEMBER WHERE studentno = '" + studentNo + "';";
         reader = SelectQuery(sqlQuery);
         if (IsThereOneValue(reader, "studentno"))
            return true;
         else 
            return false;
      }
      
      // 중복되는 도서번호가 있는지 확인한다.
      public bool IsOverlapBook(string bookName, string writer)
      {
         string sqlQuery = "SELECT bookno FROM book WHERE bookname = '" + bookName + "' AND writer = '"+writer+"';";
         reader = SelectQuery(sqlQuery);
         if (IsThereOneValue(reader, "bookno"))
            return true;
         else
            return false;
      }

      // SELECT문을 실행하고 reader를 반환함
      public MySqlDataReader SelectQuery(string query)
      {
         command = new MySqlCommand(query, connect);
         return command.ExecuteReader();
      }

      // 현재 튜플이 하나만 존재하는지 확인한다.
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

      // 멤버정보를 객체에 로드한다.
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

      // 멤버를 삭제한다.
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

      // 책을 삭제한다.
      public bool DeleteBook(string bookno)
      {
         string sqlQuery1 = "DELETE FROM book WHERE bookno = '" + bookno + "';";

         command = new MySqlCommand(sqlQuery1, connect);
         if (command.ExecuteNonQuery() != 1)
            return false;
         else
            return true;
      }

      // 현재 대출 중인지 확인한다. 대출 중이면 true
      public bool CheckRental(string bookno)
      {
         string sqlQuery = "SELECT bno FROM rental WHERE bno = '" + bookno + "';";

         if (IsThereOneValue(SelectQuery(sqlQuery), "bno"))
            return true;
         else
            return false;
      }

      // attribute를 기준으로 책을 찾는다.
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

      // 책의 대출 기한을 연장한다.
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

         // 대출 횟수가 3번 이하면 대출 가능
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

      // 대출 중인 책을 빌린 사람과 같은 학번인가를 반환
      public bool IsBorrowSamePerson(string studentno, string bookno)
      {
         string sqlQuery = "SELECT bno FROM rental WHERE sno = '" + studentno + "' AND bno = '" + bookno + "';";
         if (IsThereOneValue(SelectQuery(sqlQuery), "bno"))
            return true;
         else
            return false;
      }

      //  rental 정보를 DB에 추가한다.
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

      // 해당 책의 만기일을 얻어온다.
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

      // DB에서 대출정보를 삭제한다.
      public bool DeleteRental(string bookno)
      {
         string sqlQuery = "DELETE FROM rental WHERE bno = '" + bookno + "';";
         command = new MySqlCommand(sqlQuery, connect);
         if (command.ExecuteNonQuery() == 1)
            return false;
         else
            return true;
      }

      // 해당 멤버의 대출 목록을 출력한다.
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
