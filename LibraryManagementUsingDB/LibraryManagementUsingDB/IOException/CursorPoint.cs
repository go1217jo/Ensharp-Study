using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.IOException
{
    class CursorPoint
    {
        int cursorLeft;
        int cursorTop;

        public int CursorLeft
        {
            get { return cursorLeft; }
            set { this.cursorLeft = value; }
        }

        public int CursorTop
        {
            get { return cursorTop; }
            set { this.cursorTop = value; }
        }

        public CursorPoint() { }
        public CursorPoint(int cursorLeft, int cursorTop)
        {
            this.cursorLeft = cursorLeft;
            this.cursorTop = cursorTop;
        }
    }
}
