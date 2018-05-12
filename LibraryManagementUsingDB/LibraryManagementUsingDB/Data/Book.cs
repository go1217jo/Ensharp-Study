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
      private int extension = 0;
      
      public string dueto;
      public IOException.OutputProcessor outputProcessor;


      // 행 내용을 글자 간격에 맞춰 출력
      public void PrintInformation()
      {
         outputProcessor = new IOException.OutputProcessor();
         Console.Write(" {0}", outputProcessor.PrintFixString(ISBN, 26));
         Console.Write("{0}", outputProcessor.PrintFixString(Name, 50));
         Console.Write("{0}", outputProcessor.PrintFixString(Company, 34));
         Console.Write("{0}", outputProcessor.PrintFixString(Writer, 30));
         Console.Write("{0}", outputProcessor.PrintFixString(Price+"", 8));
         Console.Write("{0}", outputProcessor.PrintFixString(Count + "", 6));
         Console.Write("{0}", outputProcessor.PrintFixString(Pubdate, 10));
         Console.WriteLine();
      }
      
      // 대출 기간이 포함된 정보 출력
      public void PrintDuetoInformation()
      {
         outputProcessor = new IOException.OutputProcessor();
         Console.Write(" {0}", outputProcessor.PrintFixString(ISBN, 26));
         Console.Write("{0}", outputProcessor.PrintFixString(Name, 50));
         Console.Write("{0}", outputProcessor.PrintFixString(Company, 34));
         Console.Write("{0}", outputProcessor.PrintFixString(Writer, 30));
         Console.Write("{0}", outputProcessor.PrintFixString(dueto, 16));
         Console.Write("{0}", outputProcessor.PrintFixString(Extension+"회", 8));
         Console.WriteLine();
      }

      public Book() { }

      public Book(string name, string company, string writer)
      {
         this.name = name;
         this.company = company;
         this.writer = writer;
      }

      // 편집되지 않은 길이의 문자열을 반환
      public string GetName() { return name; }
      public string GetCompany() { return company; }
      public string GetWriter() { return writer; }

      public string Name
      {
         get {
            if (Encoding.Default.GetByteCount(name) >= 50)
               return name.Substring(0, 25) + "...";
            else
               return name;
         }
         set {
            string bookname = "";
            string[] words = value.Split(new string[] { "<b>", "</b>" }, StringSplitOptions.None);
            for (int idx = 0; idx < words.Length; idx++)
               bookname += words[idx];
            name = bookname;
         }
      }

      public string Company
      {
         get {
            if (Encoding.Default.GetByteCount(company) >= 34)
               return company.Substring(0, 17) + "...";
            else
               return company;
         }
         set {
            string companyOrigin = "";
            string[] words = value.Split(new string[] { "<b>", "</b>" }, StringSplitOptions.None);
            for (int idx = 0; idx < words.Length; idx++)
               companyOrigin += words[idx];
            company = companyOrigin;
         }
      }

      public string Writer
      {
         get {
            if (Encoding.Default.GetByteCount(writer) >= 30)
               return writer.Substring(0, 15) + "...";
            else
               return writer;
         }
         set {
            string writerOrigin = "";
            string[] words = value.Split(new string[] { "<b>", "</b>" }, StringSplitOptions.None);
            for (int idx = 0; idx < words.Length; idx++)
               writerOrigin += words[idx];
            writer = writerOrigin;
         }
      }

      public string ISBN
      {
         get { return isbn; }
         set { isbn = value; }
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
         set {
            if (value.Length >= 256)
               description = value.Substring(0, 255);
            else
               description = value;
         }
      }

      public int Extension
      {
         get { return extension; }
         set { extension = value; }
      }
   }
}
