using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace LibraryManagement.Library
{
    /// <summary>
    ///  도서에 대한 추가, 삭제, 보기, 대여, 반납과 같은 서비스를 제공하는 클래스
    /// </summary>
    class LibrarySystem
    {
        int bookNumber = 1;
        UI.ScreenUI drawer = new UI.ScreenUI();
        UI.KeyInput inputProcessor = new UI.KeyInput();
        ArrayList rentalHistoryList;
        Data.BookManagement bookKeepList;

        public LibrarySystem() {
            rentalHistoryList = new ArrayList();
            bookKeepList = new Data.BookManagement();
        }
        
        // 현재 모든 도서 목록을 출력
        public int PrintAllBookList() {
            return bookKeepList.PrintBookList();
        }

        // 인덱스에 따라 도서 객체를 반환한다.
        public Data.Book ValueOf(int index)
        {
            return bookKeepList.ValueOf(index);
        }

        // 매개변수로 빌릴 책 객체와 빌리는 회원 객체를 받고 대출 처리를 한다.
        public void Rental(Data.Book book, Data.Member rentalMember) {
            if (book.Rental) {  // 책이 대출중이면
                Console.WriteLine("\n   현재 선택하신 책은 대출 중입니다.");
                inputProcessor.PressAnyKey();
                Console.Clear();
            }
            else  //  책이 보유 중 이면
            {
                // 대출 도서로 추가하고 대출 상태로 변경
                RentalHistory rentalBook = new RentalHistory(book);
                rentalHistoryList.Add(rentalBook);
                rentalMember.rentalBookList.Add(book);
                book.Rental = true;
                Console.WriteLine("\n   대출되었습니다.\n   반납 기한은 " + rentalBook.getDueDay() + "까지 입니다.");
                inputProcessor.PressAnyKey();
                Console.Clear();
            }
        }

        // 책을 도서목록에 추가한다.
        public void InsertBook(Data.Book newBook)
        {
            if (bookKeepList.IsThereBook(newBook.Name, newBook.Writer)) {
                Console.Write("이미 등록된 책입니다.");
                inputProcessor.PressAnyKey();
            }
            else
            {
                // 도서 번호를 만든다.
                newBook.BookNo = "";
                for (int i=0; i< 6 - (bookNumber + "").Length; i++)
                    newBook.BookNo += "0";
                newBook.BookNo += (bookNumber + "");
                bookNumber++;

                // 책 리스트에 추가한다.
                bookKeepList.Insert(newBook);
            }
        }

        // 책을 도서목록에서 삭제한다.
        public void DeleteBook(Data.Book deletedBook)
        {
            if (drawer.YesOrNo("해당 도서를 삭제하시겠습니까?") == 1)
            {
                // 도서 삭제
                bookKeepList.Delete(deletedBook);
                Console.Write("\n\t책이 성공적으로 삭제되었습니다.");
            }
            else
                Console.Write("\n\t삭제 실패하였습니다.");
            inputProcessor.PressAnyKey();
            Console.Clear();
        }

        // 키워드에 따라 책을 검색한다.
        public ArrayList SearchBook()
        {
            string searchItem;
            // 도서 검색화면을 출력하고 사용자로부터 입력받는다.
            // 1 : 도서명 검색, 2 : 출판사 검색, 3 : 저자 검색
            switch(drawer.BookSearchScreen())
            {
                case 1:
                    Console.Write("\n    검색할 도서명 > ");
                    searchItem = inputProcessor.ReadAndCheckString(25, 18, 20, 1, false);
                    return bookKeepList.SearchBy((int)Data.BookManagement.Format.NameFormat, searchItem);
                case 2:
                    Console.Write("\n    검색할 출판사 > ");
                    searchItem = inputProcessor.ReadAndCheckString(25, 18, 20, 1, false);
                    return bookKeepList.SearchBy((int)Data.BookManagement.Format.CompanyFormat, searchItem);
                case 3:
                    Console.Write("\n      검색할 저자 > ");
                    searchItem = inputProcessor.ReadAndCheckString(25, 18, 20, 1, false);
                    return bookKeepList.SearchBy((int)Data.BookManagement.Format.WriterFormat, searchItem);
            }
            return null;
        }

        // 도서를 검색하고 수정한다.
        public bool SearchAndModifyBook()
        {
            string searchItem;
            // 도서 검색화면을 출력하고 검색 형식을 입력받는다.
            int format = drawer.BookSearchScreen();
            ArrayList searchResult = null;
            int index;

            // 1 : 도서명 검색, 2 : 출판사 검색, 3 : 저자 검색
            switch (format)
            {
                case 1:
                    Console.Write("\n    검색할 도서명 > ");
                    searchItem = inputProcessor.ReadAndCheckString(30, 18, 20, 1, false);
                    searchResult = bookKeepList.SearchBy((int)Data.BookManagement.Format.NameFormat, searchItem);
                    break;
                case 2:
                    Console.Write("\n    검색할 출판사 > ");
                    searchItem = inputProcessor.ReadAndCheckString(30, 18, 20, 1, false);
                    searchResult = bookKeepList.SearchBy((int)Data.BookManagement.Format.CompanyFormat, searchItem);
                    break;
                case 3:
                    Console.Write("\n      검색할 저자 > ");
                    searchItem = inputProcessor.ReadAndCheckString(30, 18, 20, 1, false);
                    searchResult = bookKeepList.SearchBy((int)Data.BookManagement.Format.WriterFormat, searchItem);
                    break;
                default:
                    searchResult = new ArrayList();
                    break;
            }

            // 검색 결과가 없다면
            if (searchResult.Count == 0) {
                Console.Clear();
                Console.WriteLine("\n검색 결과가 없습니다.");
                inputProcessor.PressAnyKey();
                Console.Clear();
                return false;
            }

            // 검색결과가 있다면 검색된 도서목록을 출력한다.
            index = drawer.PrintBookList(searchResult) - 1;
            if (index != searchResult.Count) {
                Console.Write("\n수정 사항 입력 > ");
                ModifyBook((Data.Book)searchResult[index], format - 1, inputProcessor.ReadAndCheckString(25, 18, 17, 1, false));
                return true;
            }
            else
                return false;
        }

        // 책을 형식에 따라 수정한다.
        public void ModifyBook(Data.Book modifiedBook, int format, string content)
        {
            switch (format)
            {
                case (int)Data.BookManagement.Format.NameFormat:
                    modifiedBook.Name = content;
                    break;
                case (int)Data.BookManagement.Format.CompanyFormat:
                    modifiedBook.Company = content;
                    break;
                case (int)Data.BookManagement.Format.WriterFormat:
                    modifiedBook.Writer = content;
                    break;
            }
        }

        // 반납할 도서 객체와 반납 회원 객체를 받아 책을 반납처리한다.
        public void Return(Data.Book book, Data.Member rentalMember)
        {
            book.Rental = false;
            for(int i=0;i<rentalHistoryList.Count; i++)
            {
                RentalHistory temp = (RentalHistory)rentalHistoryList[i];
                // 빌린 도서 목록에서 제외함으로 인해 반납처리됨
                if (temp.GetBook().Equals(book)) {
                    rentalHistoryList.Remove(temp);
                    break;
                }
            }
            // 회원의 대출 도서 목록에서도 제외
            rentalMember.rentalBookList.Remove(book);
            Console.WriteLine("\n   반납되었습니다.");

            inputProcessor.PressAnyKey();
            Console.Clear();
        }

        // 책을 연장 시도한다.
        public void Extension(Data.Book book)
        {
            for (int i = 0; i < rentalHistoryList.Count; i++)
            {
                RentalHistory temp = (RentalHistory)rentalHistoryList[i];
                // 연장하려는 도서객체와 일치하면 연장을 시도한다.
                if (temp.GetBook().Equals(book))
                {
                    if (temp.Extend()) {
                        Console.WriteLine("\n   연장되었습니다.");
                        Console.WriteLine("\n   반납 기한은 " + temp.getDueDay() + "입니다.");
                    }
                    else
                        Console.WriteLine("\n   연장 실패하였습니다.");
                    break;
                }
            }
            inputProcessor.PressAnyKey();
            Console.Clear();
        }
    }
}
