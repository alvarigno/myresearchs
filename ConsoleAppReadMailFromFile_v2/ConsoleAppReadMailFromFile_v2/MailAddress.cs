using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppReadMailFromFile_v2
{

    public class MailAddress
    {
 //       private static char[] 2R = new char[] { '\'', '"', ' ', '\t', '\r', '\n' };
        protected static char[] artrimaddr = new char[] { '<', '>', ' ', '\t', '\r', '\n' };
        protected static char[] artrimname = new char[] { '<', '>', '\'', '"', ' ', '\t', '\r', '\n' };
        protected string m_additional;
        protected string m_address;
        protected object m_innerTag;
        protected string m_name;

        public MailAddress()
        {
        }

        public MailAddress(string address)
        {
            this.m_name = "";
            this.m_address = "";
            this.m_additional = "";
            string str = "";
            string strA = "";
            int length = address.LastIndexOf("<");
            if (length == -1)
            {
                str = address;
                if (str.IndexOf('@') == -1)
                {
                    strA = str;
                    strA = strA.Trim("\\\"".ToCharArray());
                    Console.WriteLine("mio 1:"+strA);
                }
            }
            else
            {
                str = address.Substring(length + 1);
                strA = address.Substring(0, length);
                if (string.Compare(strA, 0, "\\\"", 0, "\\\"".Length, true) == 0)
                {
                    strA = strA.Trim("\\\"".ToCharArray());
                    Console.WriteLine("mio 1:" +strA);
                }
            }
            this.m_name = strA;
            this.m_address = str;
        }

    }

}


