using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ImageSearch.Data
{
   public class DBHandler
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
         no = TupleCount() + 1;
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
         string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
         string sqlQuery = "INSERT INTO history values(" + (no++) + ", '" + now + "', '" + keyword + "');";

         command = new MySqlCommand(sqlQuery, connect);
         if (command.ExecuteNonQuery() != -1)
            return false;
         else
            return true;
      }

      public int TupleCount()
      {
         int count = 0;
         string sqlQuery = "SELECT no FROM history;";
         reader = SelectQuery(sqlQuery);
         while (reader.Read())
            count++;
         reader.Close();

         return count;
      }

      public List<Log> ViewAllLog()
      {
         string sqlQuery = "SELECT * FROM history;";
         List<Log> logs = new List<Log>();
         reader = SelectQuery(sqlQuery);
         while(reader.Read())
         {
            Log log = new Log();
            log.LogTime = reader["searchtime"].ToString();
            log.Keyword = reader["keyword"].ToString();
            logs.Add(log);
         }
         reader.Close();

         return logs;
      }

      public bool DeleteLog(string deleteTime)
      {
         DateTime time = DateTime.Parse(deleteTime);
         string sqlQuery = "DELETE FROM history WHERE searchtime = '" + time.ToString("yyyy-MM-dd HH:mm:ss") + "';";
         command = new MySqlCommand(sqlQuery, connect);
         if (command.ExecuteNonQuery() != -1)
            return true;
         else
            return false;
      }
   }
}
