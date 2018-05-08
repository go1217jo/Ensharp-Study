using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ImageSearch.Data
{
   class DBHandler
   {
      MySqlConnection connect = null;
      MySqlCommand command;
      MySqlDataReader reader = null;
      int no = 1;

      // DB 접속
      public DBHandler()
      {
         String databaseConnect = "Server=localhost;Database=image_search;Uid=root;Pwd=1q2w3e4r!;SslMode=none;Charset=utf8;";
         // connect MySQL
         connect = new MySqlConnection(databaseConnect);
         connect.Open();
      }

      public void Close()
      {
         if (reader != null)
            reader.Close();
         if (connect != null)
            connect.Close();
      }

      ~DBHandler()
      {
         Close();
      }

      // SELECT문을 실행하고 reader를 반환함
      public MySqlDataReader SelectQuery(string query)
      {
         command = new MySqlCommand(query, connect);
         return command.ExecuteReader();
      }

      public bool InsertLog(string keyword)
      {
         string now = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss");
         string sqlQuery = "INSERT INTO history values(" + (no++) + ", '" + now + "', '" + keyword + "');";

         command = new MySqlCommand(sqlQuery, connect);
         if (command.ExecuteNonQuery() != -1)
            return false;
         else
            return true;
      }
   }
}
