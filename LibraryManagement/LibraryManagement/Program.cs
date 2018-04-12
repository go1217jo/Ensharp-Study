using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            UI.ScreenUI drawer = new UI.ScreenUI();
            Data.MemberManagement userManager = new Data.MemberManagement();
            Library.LibrarySystem system = new Library.LibrarySystem();
            Member.Login login = new Member.Login(userManager);

            system.InsertBook(new Data.Book("농담", "민음사", "밀란쿤데라"));
            system.InsertBook(new Data.Book("82년생 김지영", "민음사", "조남주"));
            system.InsertBook(new Data.Book("위험사회", "새물결", "울리히 백"));
           // system.InsertBook(new Data.Book("왜 세계의 절반은 굶주리는가?", "갈라파고스", "장 지글러"));
            system.InsertBook(new Data.Book("Pattern recognition", "Elsevier", "Theodoridis"));
            system.InsertBook(new Data.Book("Pattern recognition", "J.Wiley", "Schalkoff, Robert J"));

            while (true)
            {
                // 1 : 관리자 모드, 2 : 사용자 모드, 3 : 종료
                switch (drawer.FirstMenuScreen())
                {
                    case 1:
                        login.LoginAdmin(system);
                        break;
                    case 2:
                        login.UserMenuChoice(system);
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("프로그램이 종료되었습니다.");
                        return;
                }
            }
        }
    }
}
