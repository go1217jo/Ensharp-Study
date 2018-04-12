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
