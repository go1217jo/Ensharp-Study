using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.NaverAPI
{
   class SearchEngine
   {
      // keyword : 검색할 문자열
      public List<Data.Book> SearchBooks(string keyword, int count)
      {
         List<Data.Book> books = null;
         string url = "https://openapi.naver.com/v1/search/book?query=" + keyword; // 결과가 JSON 포맷

         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "&display=" + count);
         request.Headers.Add("X-Naver-Client-Id", "NnoFUiv7o5xIs3PKAFKR"); // 클라이언트 아이디
         request.Headers.Add("X-Naver-Client-Secret", "rS50GYs7BX");       // 클라이언트 시크릿
         request.ContentType = "application/json; charset=utf-8";
         request.Method = "GET";
         request.ServicePoint.Expect100Continue = false;

         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
         string status = response.StatusCode.ToString();

         if (status == "OK")
         {
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string responseFromServer = reader.ReadToEnd();
            books = Parsing(responseFromServer);
            reader.Close();
            stream.Close();
            return books;
         }
         else
            return null;
      }

      // 파싱한 뒤 검색된 정보들을 객체에 담는다
      public List<Data.Book> Parsing(string responseFromServer)
      {
         List<Data.Book> books = new List<Data.Book>();
         var json = JObject.Parse(responseFromServer);
         var items = json["items"];

         for(int idx=0; idx<items.Count(); idx++)
         {
            Data.Book book = new Data.Book();
            book.ISBN = items[idx]["isbn"].ToString();
            book.Name = items[idx]["title"].ToString();
            book.Company = items[idx]["publisher"].ToString();
            book.Writer = items[idx]["author"].ToString();
            // 가격이 소수점으로 들어오는 거 방지
            book.Price = int.Parse(items[idx]["price"].ToString().Split('.')[0]);
            book.Pubdate = items[idx]["pubdate"].ToString();
            book.Description = items[idx]["description"].ToString();
            book.Count = 9999;
            books.Add(book);
         }

         return books;
      }
   }
}
