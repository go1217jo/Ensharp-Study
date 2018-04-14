using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace LibraryManagement.Data
{
    interface IManagement
    {
        void Insert(object newObject);

        void Delete(object deleteObject);

        ArrayList SearchBy(int format, string content);
    }
}
