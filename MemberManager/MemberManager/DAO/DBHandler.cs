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
    public class DBHandler
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

        // 현재 튜플이 하나만 존재하는지 확인한다.
        public bool IsThereOneValue(MySqlDataReader reader, string attribute)
        {
            List<string> objects = new List<string>();

            while (reader.Read())
                objects.Add(reader[attribute].ToString());
            reader.Close();

            if (objects.Count == 1)
                return true;
            else
                return false;
        }

        public bool InsertMember(string id, string password, string name, int sex, string birth, string mail)
        {
            string query = "INSERT INTO membermanager values('" + id + "', '" + password + "', '" + name + "', " + sex + ", '" + birth + "', '" + mail + "');";
            return ExecuteQuery(query);
        }

        public bool IsOverID(string id)
        {
            string query = "SELECT id FROM membermanager WHERE id = '" + id + "';";
            reader = SelectQuery(query);
            return !IsThereOneValue(reader, "id");
        }
    }
}
