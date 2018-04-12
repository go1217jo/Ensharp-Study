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
        Hashtable nameTable;
        Hashtable studentNoTable;
        Hashtable addressTable;
        Hashtable phoneNumberTable;

        public MemberManagement()
        {
            members = new ArrayList();
            nameTable = new Hashtable();
            studentNoTable = new Hashtable();
            addressTable = new Hashtable();
            phoneNumberTable = new Hashtable();
        }

        public void Insert(object newObject)
        {
            Member newMember = (Member)newObject;
            members.Add(newMember);
            nameTable[newMember.Name] = newMember;
            studentNoTable[newMember.StudentNo] = newMember;
            addressTable[newMember.Address] = newMember;
            phoneNumberTable[newMember.PhoneNumber] = newMember;
        }

        public void Delete(object deleteObject)
        {
            Member deleteMember = (Member)deleteObject;
            nameTable.Remove(deleteMember.Name);
            studentNoTable.Remove(deleteMember.StudentNo);
            addressTable.Remove(deleteMember.Address);
            phoneNumberTable.Remove(deleteMember.PhoneNumber);
            members.Remove(deleteMember);
        }

        public ArrayList SearchBy(int format, string content)
        {
            return null;
        }

        public void ModifyAs(int format, string content)
        {

        }

        public bool IsThereMember(string studentNo) {
            return studentNoTable.Contains(studentNo);
        }
    }
}
