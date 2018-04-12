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

        public MemberManagement()
        {
            members = new ArrayList();
        }

        public void Insert(object newObject)
        {
            Member newMember = (Member)newObject;
            members.Add(newMember);
        }

        public void Delete(object deleteObject)
        {
            Member deleteMember = (Member)deleteObject;
            members.Remove(deleteMember);
        }

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

        public void ModifyAs(int format, string content)
        {

        }

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
