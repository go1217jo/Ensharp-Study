using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Library
{
    class RentalHistory {
        Data.Book borrowedBook;
        DateTime borrowDate;
        DateTime dueDate;
        int extension;

        public RentalHistory(Data.Book borrowed) {
            borrowDate = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")));
            dueDate = borrowDate.AddDays(7);
            borrowedBook = borrowed;
            extension = 0;
        }

        public Data.Book GetBook()
        {
            return borrowedBook;
        }

        // 책 대출 기간을 연장한다.
        public bool Extend() {
            // 연장 횟수가 이미 2번이므로 2번을 초과하여 연장할 수 없다.
            if (extension == 2)
                return false;
            else
            {
                dueDate = dueDate.AddDays(7);
                extension++;
                return true;
            }
        }

        // 대출 기한을 얻는다
        public string getDueDay() {
            return dueDate.ToString("yyyyMMdd");
        }
    }
}
