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

      public bool ExecuteQuery(string query)
      {
         command = new MySqlCommand(query, connect);
         if (command.ExecuteNonQuery() != -1)
            return true;
         else
            return false;
      }

      public bool InsertLog(string keyword)
      {
         string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
         string sqlQuery = "INSERT INTO history values('" + now + "', '" + keyword + "');";
         return ExecuteQuery(sqlQuery);
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
         return ExecuteQuery(sqlQuery);
      }

      // 기록에 이미 검색된 키워드가 있으면 검색 시간을 갱신한다.
      public bool UpdateTime(string keyword)
      {
         int count = 0;
         string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
         string sqlQuery1 = "SELECT keyword FROM history WHERE keyword = '" + keyword + "';";
         string sqlQuery2 = "UPDATE history SET searchtime = '" + now + "' WHERE keyword = '" + keyword + "';";

         reader = SelectQuery(sqlQuery1);
         while (reader.Read())
            count++;
         reader.Close();
         if (count != 0)
            return ExecuteQuery(sqlQuery2);
         else
            return false;
      }

      public bool ClearTable()
      {
         string sqlQuery1 = "DROP TABLE IF EXISTS history;";
         string sqlQuery2 = "CREATE TABLE history (searchtime datetime NOT NULL, keyword varchar(50) DEFAULT NULL, PRIMARY KEY(searchtime)) ENGINE = InnoDB DEFAULT CHARSET = utf8;";

         ExecuteQuery(sqlQuery1);
         return ExecuteQuery(sqlQuery2);
      }
   }
}
