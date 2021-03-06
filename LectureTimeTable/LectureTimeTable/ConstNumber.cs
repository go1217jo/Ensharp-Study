﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectureTimeTable
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

      // 필드 인덱스
      public const int NO = 1;
      public const int MAJOR = 2;
      public const int SUBJECT_NO = 3;
      public const int CLASS = 4;
      public const int SUBJECT_NAME = 5;
      public const int COMPLETION_DIVISION = 6;
      public const int GRADE = 7;
      public const int CREDIT = 8;
      public const int TIME = 9;
      public const int ROOM = 10;
      public const int PROFESSOR_NAME = 11;
      public const int LANGUAGE = 12;

      // 수강신청, 관심과목 담기 모드 상수
      public const int REAL_CHOICE = 15;
      public const int FAKE_CHOICE = 16;

      // 최대 수강 학점
      public const int MAX_INTERESTED = 24;
      public const int MAX_APPLIED = 21;

      // 요일 상수
      public const int MONDAY = 0;
      public const int TUESDAY = 1;
      public const int WENSDAY = 2;
      public const int THURSDAY = 3;
      public const int FRIDAY = 4;
   }
}
