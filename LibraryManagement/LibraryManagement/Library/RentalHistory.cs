using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Library
{
    /// <summary>
    ///  대출 반납에 대한 정보를 담는 데이터 클래스
    /// </summary>
    class RentalHistory {
        Data.Book borrowedBook;
        DateTime borrowDate;
        DateTime dueDate;
        int extension;

        // 빌린 책 객체를 입력받아 대출정보를 만든다.
        public RentalHistory(Data.Book borrowed) {
            // 현재 시간이 빌린 시간
            borrowDate = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")));
            // 반납 기한은 빌린 시간에서 7일 뒤로 설정
            dueDate = borrowDate.AddDays(7);
            borrowedBook = borrowed;
            // 연장 횟수 초기화
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
                // 연장 가능하면 7일 연장한다.
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
