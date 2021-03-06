﻿using System;
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

      // 쿼리 명령문을 실행하고 성공 여부를 반환
      public bool ExecuteQuery(string query)
      {
         command = new MySqlCommand(query, connect);
         if (command.ExecuteNonQuery() != 1)
            return false;
         else
            return true;
      }

      // SELECT문을 실행하고 reader를 반환함
      public MySqlDataReader SelectQuery(string query)
      {
         command = new MySqlCommand(query, connect);
         return command.ExecuteReader();
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
            book.ISBN = reader["ISBN"].ToString();
            book.Name = reader["bookname"].ToString();
            book.Company = reader["company"].ToString();
            book.Writer = reader["writer"].ToString();
            book.Price = int.Parse(reader["price"].ToString());
            book.Count = int.Parse(reader["COUNT"].ToString());
            book.Description = reader["description"].ToString();
            book.Pubdate = reader["pubdate"].ToString();
            books.Add(book);
         }
         reader.Close();
         
         return books;
      }

      // 해당 도서번호의 도서명을 반환한다
      public string GetBookName(string isbn)
      {
         string name = null;
         string sqlQuery = "SELECT bookname FROM book WHERE ISBN = '" + isbn + "';";
         reader = SelectQuery(sqlQuery);
         while (reader.Read())
            name = reader["bookname"].ToString();
         reader.Close();

         if (Encoding.Default.GetByteCount(name) >= 50)
            name = name.Substring(0, 25);
         return name + "..";
      }

      // 해당 남은 수량을 반환
      public int GetBookCount(string isbn)
      {
         int count = 0;
         MySqlDataReader reader = SelectQuery("SELECT COUNT FROM book WHERE ISBN = '" + isbn + "';");
         while (reader.Read())
            count = int.Parse(reader["COUNT"].ToString());
         reader.Close();

         return count;
      }

      public bool ModifyBookCount(string isbn, int count)
      {
         string sqlQuery = "UPDATE book SET COUNT = " + count + " WHERE ISBN = '" + isbn + "';";
         return ExecuteQuery(sqlQuery);
      }

      // 책을 DB에 추가한다.
      public bool InsertBook(string ISBN, string bookname, string company, string writer, int price, int count, string description, string pubdate)
      {
         string sqlQuery = "INSERT INTO book values ('" + ISBN + "', '" + bookname + "', '" + company + "', '" + writer + "', " + price + ", " + count + ", '" + description + "', '" + pubdate + "');";

         // 중복되는 도서가 없다면
         if (!IsOverlapBook(ISBN))
            return ExecuteQuery(sqlQuery);
         else
            return false;
      }

      // 책을 DB에 추가한다(Overriding)
      public bool InsertBook(Data.Book book)
      {
         return InsertBook(book.ISBN, book.GetName(), book.GetCompany(), book.GetWriter(), book.Price, book.Count, book.Description, book.Pubdate);
      }

      // 멤버 정보를 수정한다.
      public bool UpdateMemberInformation(string studentNo, string modification, string attribute)
      {
         string sqlQuery = "UPDATE member set " + attribute + " = '" + modification + "' WHERE studentno = '" + studentNo + "';";
         return ExecuteQuery(sqlQuery);
      }

      // 책 정보를 수정한다.
      public bool UpdateBookInformation(string ISBN, string modification, string attribute)
      {
         string sqlQuery = "UPDATE book set " + attribute + " = '" + modification + "' WHERE ISBN = '" + ISBN + "';";
         return ExecuteQuery(sqlQuery);
      }

      // 멤버를 DB에 추가한다.
      public bool InsertMember(string memberName, string studentNo, string address, string phoneNumber, string password)
      {
         string sqlQuery = "INSERT INTO member values ('" + studentNo + "', '" + memberName + "', '" + address + "', '" + phoneNumber + "', '" + password + "');";
         // 이미 있는 멤버와 학번이 같지 않은 경우 추가
         if (!IsOverlapMember(studentNo))
            return ExecuteQuery(sqlQuery);
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
      public bool IsOverlapBook(string ISBN)
      {
         string sqlQuery = "SELECT ISBN FROM book WHERE ISBN = '" + ISBN + "';";
         reader = SelectQuery(sqlQuery);
         if (IsThereOneValue(reader, "ISBN"))
            return true;
         else
            return false;
      }
      
      // 중복되는 대여도서가 있는지 확인한다.(overloading)
      public bool IsOverlapBook(string ISBN, string studentno)
      {
         string sqlQuery = "SELECT ISBN FROM rental WHERE ISBN = '" + ISBN + "' AND sno = '" + studentno + "';";
         reader = SelectQuery(sqlQuery);
         if (IsThereOneValue(reader, "ISBN"))
            return true;
         else
            return false;
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
            return ExecuteQuery(sqlQuery1);
         else
            return false;
      }

      // 책을 삭제한다.
      public bool DeleteBook(string ISBN)
      {
         string sqlQuery = "DELETE FROM book WHERE ISBN = '" + ISBN + "';";
         return ExecuteQuery(sqlQuery);
      }
            
      // attribute를 기준으로 책을 찾는다.
      public string SearchBook(string search, string attribute)
      {
         string ISBN = "";
         string sqlQuery = "SELECT ISBN FROM book WHERE " + attribute + " = '" + search + "';";

         reader = SelectQuery(sqlQuery);
         while (reader.Read())
            ISBN = reader["ISBN"].ToString();
         reader.Close();

         return ISBN;
      }

      // 책의 대출 기한을 연장한다.
      public string Extension(string ISBN)
      {
         // 만기일
         string dueDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
         string sqlQuery1 = "SELECT times FROM rental WHERE ISBN = '" + ISBN + "';";
         string sqlQuery2 = "UPDATE rental set times = times+1, dueto = '" + dueDate + "' WHERE ISBN = '" + ISBN + "';";

         if (GetDueTo(ISBN).Equals(dueDate))
            return null;

         reader = SelectQuery(sqlQuery1);
         int times = 0;
         while (reader.Read())
            times = (int)reader["times"];
         reader.Close();

         // 대출 횟수가 3번 이하면 대출 가능
         if (times < 3)
         {
            ExecuteQuery(sqlQuery2);
            return dueDate;
         }
         else
            return null;
      }

      // 대출 중인 책을 빌린 사람과 같은 학번인가를 반환
      public bool IsBorrowSamePerson(string studentno, string ISBN)
      {
         string sqlQuery = "SELECT bno FROM rental WHERE sno = '" + studentno + "' AND bno = '" + ISBN + "';";
         if (IsThereOneValue(SelectQuery(sqlQuery), "bno"))
            return true;
         else
            return false;
      }

      //  rental 정보를 DB에 추가한다.
      public bool Rental(string studentNo, string ISBN)
      {
         // 만기일
         string dueDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
         string sqlQuery = "INSERT INTO rental values ('" + studentNo + "', '" + dueDate + "', 0, '" + ISBN + "');";

         return ExecuteQuery(sqlQuery);
      }

      // 해당 책의 만기일을 얻어온다.
      public string GetDueTo(string ISBN)
      {
         string sqlQuery = "SELECT dueto FROM rental Where ISBN = '" + ISBN + "';";
         string dueto=null;
         reader = SelectQuery(sqlQuery);
         while (reader.Read())
            dueto = DateTime.Parse(reader["dueto"].ToString()).ToString("yyyy-MM-dd");
         reader.Close();
         return dueto;
      }

      // DB에서 대출정보를 삭제한다.
      public bool DeleteRental(string ISBN)
      {
         string sqlQuery = "DELETE FROM rental WHERE ISBN = '" + ISBN + "';";
         return ExecuteQuery(sqlQuery);
      }

      // 해당 멤버의 대출 목록을 출력한다.
      public List<Book> RentalList(string studentno)
      {
         List<Book> books = new List<Book>();
         string sqlQuery = "SELECT book.ISBN, bookname, company, writer, dueto, times FROM book , rental WHERE book.ISBN = rental.ISBN AND rental.sno = '" + studentno + "';";
         reader = SelectQuery(sqlQuery);
         while (reader.Read())
         {
            Book book = new Book();
            book.ISBN = reader["ISBN"].ToString();
            book.Name = reader["bookname"].ToString();
            book.Company = reader["company"].ToString();
            book.Writer = reader["writer"].ToString();
            book.dueto = DateTime.Parse(reader["dueto"].ToString()).ToString("yyyy-MM-dd");
            book.Extension = int.Parse(reader["times"].ToString());
            books.Add(book);
         }
         reader.Close();
         
         return books;
      }

      // 로그 내보내기
      public void ExportLog()
      {
         List<Log> logs = ViewAllLog();
         using (System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\log.txt", false))
         {
            file.WriteLine(" ============================================================================================================");
            file.WriteLine("          발생시간            실행자                           키워드                             로그 유형");
            file.WriteLine(" ============================================================================================================");
            for (int i = 0; i < logs.Count; i++)
               file.WriteLine(logs[i].PrintLogInformation());
         }
      }

      // 로그를 추가한다.
      public bool InsertLog(string membername, string keyword, string type)
      {
         string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
         string sqlQuery = "INSERT INTO history values('" + now + "', '" + membername + "', '" + keyword + "', '" + type + "');";
         ExportLog();
         return ExecuteQuery(sqlQuery);
      }

      // 모든 로그를 반환한다.
      public List<Log> ViewAllLog()
      {
         string sqlQuery = "SELECT * FROM history;";
         List<Log> logs = new List<Log>();
         reader = SelectQuery(sqlQuery);
         while (reader.Read())
         {
            Log log = new Log();
            log.LogTime = reader["searchtime"].ToString();
            log.Membername = reader["membername"].ToString();
            log.Keyword = reader["keyword"].ToString();
            log.Type = reader["type"].ToString();
            logs.Add(log);
         }
         reader.Close();

         return logs;
      }
      
      // 로그기록 초기화
      public bool ClearTable()
      {
         string sqlQuery = "Delete FROM history;";
         return ExecuteQuery(sqlQuery);
      }
   }
}
