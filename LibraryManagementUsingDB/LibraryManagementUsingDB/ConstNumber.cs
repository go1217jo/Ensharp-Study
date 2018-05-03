using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB
{
   class ConstNumber
   {
      // 메뉴 인덱스 상수
      public const int MENULIST_1 = 0;
      public const int MENULIST_2 = 1;
      public const int MENULIST_3 = 2;
      public const int MENULIST_4 = 3;
      public const int MENULIST_5 = 4;
      public const int MENULIST_6 = 5;

      // 로그인 상태 상수
      public const int LOGIN_FAIL = 0;
      public const int LOGIN_USER = 1;
      public const int LOGIN_ADMIN = 2;

      //  키보드 키 상수
      public const int LEFT = 4;
      public const int UP = 8;
      public const int RIGHT = 6;
      public const int DOWN = 5;
      public const int ENTER = 0;
      public const int ESC = -1;

      // 포맷 제한 상수
      public const int GENERAL_LIMIT = 10;
      public const int NUMBER_LIMIT = 11;

      // 멤버 정보 검색 상수
      public const int MEMBER_NAME = 0;
      public const int MEMBER_ADDRESS = 1;
      public const int MEMBER_PHONENUMBER = 2;
   }
}
