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
      MySqlConnection connect;
      MySqlCommand command;
      MySqlDataReader reader;

      public DBHandler()
      {
         String databaseConnect = "Server=localhost;Database=librarymanagement;Uid=root;Pwd=1q2w3e4r!;SslMode=none;";
         // connect MySQL
         connect = new MySqlConnection(databaseConnect);
         connect.Open();
      }

      public void Close()
      {
         reader.Close();
         connect.Close();
      }

      ~DBHandler()
      {
         Close();
      }

      public bool InsertBook(string bookno, string bookname, string company, string writer)
      {
         string sqlQuery = "INSERT INTO book values ('" + bookno + "', '" + bookname + "', '" + company + "', '" + writer + "');";
         command = new MySqlCommand(sqlQuery, connect);
         if (command.ExecuteNonQuery() != 1)
            return false;
         else
            return true;
      }

      public bool InsertMember(string memberName, string studentNo, string address, string phoneNumber, string password)
      {
         string sqlQuery = "INSERT INTO book values ('" + studentNo + "', '" + memberName + "', '" + address + "', '" + password + "');";
         command = new MySqlCommand(sqlQuery, connect);
         if (command.ExecuteNonQuery() != 1)
            return false;
         else
            return true;
      }

      public bool InsertRental(string sno, string bno, int loaned, string dueTo)
      {
         return true;
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
   }
}
