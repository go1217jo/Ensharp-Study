using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MemberManager.DAO
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
            String databaseConnect = "Server=localhost;Database=membermanager;Uid=root;Pwd=1q2w3e4r!;SslMode=none;Charset=utf8;";
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


    }
}
