using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Library
{
   class BookManagement
   {
      Data.Student student;
      IOException.OutputProcessor outputProcessor;
      RentalManagement rentalManager;
      Data.DBHandler DB;

      public BookManagement(Data.Student student, Data.DBHandler DB, IOException.OutputProcessor outputProcessor)
      {
         this.student = student;
         this.outputProcessor = outputProcessor;
         this.DB = DB;
         rentalManager = new RentalManagement(student);
      }

      public BookManagement(IOException.OutputProcessor outputProcessor)
      {
         this.student = null;
         rentalManager = null;
         this.outputProcessor = outputProcessor;
      }

      public void UserRentalSystem()
      {
         switch(outputProcessor.MenuScreen(ConsoleUI.RENTAL_MENU))
         {
            case ConstNumber.MENULIST_1:
               SearchBook();
               break;
            case ConstNumber.MENULIST_2:
               ViewAllBook();
               break;
            case ConstNumber.MENULIST_3:
               rentalManager.ViewRentalList();
               break;
            case ConstNumber.MENULIST_4:
               return;
         }
      }

      public void SearchBook()
      {

      }

      public void ViewAllBook()
      {

      }
   }
}
