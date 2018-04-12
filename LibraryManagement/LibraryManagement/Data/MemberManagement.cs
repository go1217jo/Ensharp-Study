using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace LibraryManagement.Data
{
    class MemberManagement : Management
    {
        public enum Format {
            NameFormat, StudentNoFormat, AddressFormat, PhoneNumberFormat
        };

        ArrayList members;
        UI.ScreenUI drawer;
        UI.KeyInput inputProcessor;

        // 학번 검색하여 나온 회원을 수정한다.
        public void ModifyMember(Member member) {
            switch(drawer.MemberModifyScreen())
            {
                case 1:
                    Console.Write("\n수정 내용(이름) > ");
                    member.Name = inputProcessor.ReadAndCheckString(18, 18, 18, 1, true);
                    break;
                case 2:
                    Console.Write("\n수정 내용(주소) > ");
                    member.Address = inputProcessor.ReadAndCheckString(30, 18, 18, 1, false);
                    break;
                case 3:
                    Console.Write("\n수정 내용(전화번호) > ");
                    member.Address = inputProcessor.ReadAndCheckString(15, 18, 22, 1, true);
                    break;
                case 4:
                    Console.Write("\n수정 내용(비밀번호) > ");
                    member.Password = inputProcessor.ReadAndCheckString(18, 18, 22, 1, true);
                    break;
            }
            Console.Clear();
        }

        // 회원들의 정보를 출력한다.
        public int PrintMemberList()
        {
            int choice = 1;
            Console.SetWindowSize(96, 39);
            Data.Member member;
            while (true)
            {
                Console.WriteLine("\n =============================================================================================");
                Console.WriteLine("      이   름           학   번                  주소                  핸드폰 번호");
                Console.WriteLine(" =============================================================================================");
                for (int i = 0; i < members.Count; i++)
                {
                     member = (Data.Member)members[i];
                    if (choice - 1 == i)
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("      {0}", drawer.PrintFixString(member.Name, 14));
                    Console.Write("   {0}", drawer.PrintFixString(member.StudentNo, 14));
                    Console.Write(" {0}", drawer.PrintFixString(member.Address, 31));
                    Console.Write(" {0}", drawer.PrintFixString(member.PhoneNumber, 20));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }
                Console.SetCursorPosition(0, 3 + choice);

                if (inputProcessor.ChoiceByKey())
                    return choice;

                if (Console.CursorTop < 4)
                    Console.SetCursorPosition(Console.CursorLeft - 1, 4);
                if (Console.CursorTop > 3 + ((members.Count == 0) ? 1 : members.Count))
                    Console.SetCursorPosition(Console.CursorLeft, 3 + ((members.Count == 0) ? 1 : members.Count));

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 3;

                Console.Clear();
            }
        }

        public MemberManagement()
        {
            members = new ArrayList();
            drawer = new UI.ScreenUI();
            inputProcessor = new UI.KeyInput();
        }

        // 회원 추가
        public void Insert(object newObject)
        {
            Member newMember = (Member)newObject;
            members.Add(newMember);
        }

        // 회원 삭제
        public void Delete(object deleteObject)
        {
            Member deleteMember = (Member)deleteObject;
            members.Remove(deleteMember);
        }

        // 형식에 따라 content와 일치하는 회원을 찾는다
        public ArrayList SearchBy(int format, string content)
        {
            Member temp;
            ArrayList returnList = new ArrayList();

            for (int i = 0; i < members.Count; i++)
            {
                temp = (Member)members[i];
                switch (format)
                {
                    case (int)Format.NameFormat:
                        if (temp.Name.Equals(content))
                            returnList.Add(temp);
                        break;
                    case (int)Format.StudentNoFormat:
                        if (temp.StudentNo.Equals(content))
                            returnList.Add(temp);
                        break;
                    case (int)Format.AddressFormat:
                        if (temp.Address.Equals(content))
                            returnList.Add(temp);
                        break;
                    case (int)Format.PhoneNumberFormat:
                        if (temp.PhoneNumber.Equals(content))
                            returnList.Add(temp);
                        break;
                }
            }
            return returnList;
        }
        
        // 학번을 기본키로 회원이 존재하는지 확인
        public bool IsThereMember(string studentNo) {
            for (int i = 0; i < members.Count; i++)
            {
                Member member = (Member)members[i];
                if (member.StudentNo.Equals(studentNo))
                    return true;
            }
            return false;
        }
    }
}
