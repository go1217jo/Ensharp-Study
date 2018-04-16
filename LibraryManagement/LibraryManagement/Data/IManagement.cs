using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace LibraryManagement.Data
{
    /// <summary>
    ///  AdminManagement 클래스와 UserManagement 클래스가 비슷한 관리 역할을 하는 것을 명시적으로 나타내는 인터페이스
    /// </summary>
    interface IManagement
    {
        void Insert(object newObject);

        void Delete(object deleteObject);

        ArrayList SearchBy(int format, string content);
    }
}
