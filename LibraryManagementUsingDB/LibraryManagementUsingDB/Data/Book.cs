using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Data
{
   /// <summary>
   ///  도서 정보를 담는 데이터 클래스
   /// </summary>
   class Book
   {
      // 책번호, 이름, 출판사, 저자, 가격, 수량, 설명, 대출여부
      private string isbn;
      private string name;
      private string company;
      private string writer;
      private int price;
      private int count;
      private string pubdate;
      private string description;
      
      private bool rental;
      public string dueto;
      public IOException.OutputProcessor outputProcessor;

      public Book() { rental = false; }

      // 행 내용을 글자 간격에 맞춰 출력
      public void PrintInformation()
      {
         outputProcessor = new IOException.OutputProcessor();
         Console.Write("{0}", outputProcessor.PrintFixString(ISBN, 22));
         Console.Write("{0}", outputProcessor.PrintFixString(Name, 28));
         Console.Write("{0}", outputProcessor.PrintFixString(Company, 18));
         Console.Write("{0}", outputProcessor.PrintFixString(Writer, 20));
         Console.Write("{0}", outputProcessor.PrintFixString(Price+"", 8));
         Console.Write("{0}", outputProcessor.PrintFixString(Count + "", 6));
         Console.Write("{0}", outputProcessor.PrintFixString(Pubdate, 10));
      }
      
      // 대출 기간이 포함된 정보 출력
      public void PrintDuetoInformation()
      {
         outputProcessor = new IOException.OutputProcessor();
         Console.Write("{0}", outputProcessor.PrintFixString(ISBN, 13));
         Console.Write("{0}", outputProcessor.PrintFixString(Name, 28));
         Console.Write("{0}", outputProcessor.PrintFixString(Company, 18));
         Console.Write("{0}", outputProcessor.PrintFixString(Writer, 20));
         Console.Write("{0}", outputProcessor.PrintFixString(dueto, 16));
      }

      public Book(string name, string company, string writer)
      {
         rental = false;
         this.name = name;
         this.company = company;
         this.writer = writer;
      }

      public string Name
      {
         get { return name; }
         set { name = value; }
      }

      public string Company
      {
         get { return company; }
         set { company = value; }
      }

      public string Writer
      {
         get { return writer; }
         set { writer = value; }
      }

      public string ISBN
      {
         get { return isbn; }
         set { isbn = value; }
      }

      public bool Rental
      {
         get { return rental; }
         set { rental = value; }
      }

      public int Price
      {
         get { return price; }
         set { price = value; }
      }

      public int Count
      {
         get { return count; }
         set { count = value; }
      }

      public string Pubdate
      {
         get { return pubdate; }
         set { pubdate = value; }
      }

      public string Description
      {
         get { return description; }
         set { description = value; }
      }
   }
}
