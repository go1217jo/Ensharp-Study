﻿using System;
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
         MySqlDataReader reader = SelectQuery(sqlQuery);
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

      public bool InsertBook(string bookno, string bookname, string company, string writer)
      {
         string sqlQuery = "INSERT INTO book values ('" + bookno + "', '" + bookname + "', '" + company + "', '" + writer + "');";
         command = new MySqlCommand(sqlQuery, connect);
         if (command.ExecuteNonQuery() != 1)
            return false;
         else
            return true;
      }

      public bool UpdateMemberInformation(string studentNo, string modification, string attribute)
      {
         string sqlQuery = "UPDATE member set " + attribute + " = '" + modification + "' WHERE studentno = '" + studentNo + "';";
         
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
         MySqlDataReader reader = SelectQuery(sqlQuery);
         if (IsThereOneValue(reader, "studentno"))
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
   }
}
