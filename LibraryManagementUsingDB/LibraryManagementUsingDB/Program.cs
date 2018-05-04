using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Copyright (C) 2007 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/// <summary>
/// 
/// 제작자 : 주영준
/// 
///  Admin ID : 12345678, Password : 1234
///  member ID : 14010994, Password : 12345678
/// </summary>

namespace LibraryManagementUsingDB
{
   class Program
   {
      static void Main(string[] args)
      {
         Data.DBHandler DB = new Data.DBHandler();
         Member.LoginManagement loginManagement = new Member.LoginManagement(DB);

         loginManagement.Login();         
      }
   }
}
