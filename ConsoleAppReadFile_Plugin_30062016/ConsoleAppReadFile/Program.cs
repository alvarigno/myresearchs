using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;
using EAGetMail;

namespace ConsoleAppReadFile
{
    class Program
    {

        myConnection myConn = new myConnection();

        private static void ParseEmail(string emlFile)
        {

            Mail oMail = new Mail("TryIt");
            
            oMail.Load(emlFile, false);

            // Parse Mail From, Sender
            Console.WriteLine("Header: {0}", oMail.Headers.ToString());

            // Parse Mail From, Sender
            Console.WriteLine("From: {0}", oMail.From.ToString());

            // Parse Mail To, Recipient
            EAGetMail.MailAddress[] addrs = oMail.To;
            for (int i = 0; i < addrs.Length; i++)
            {
                Console.WriteLine("To: {0}", addrs[i].ToString());
            }

            // Parse Mail CC
            addrs = oMail.Cc;
            for (int i = 0; i < addrs.Length; i++)
            {
                Console.WriteLine("To: {0}", addrs[i].ToString());
            }

            // Parse Mail Subject
            String personalprueba = oMail.Subject;
            personalprueba =  oMail.Subject;
            personalprueba = personalprueba.Replace("(Trial Version)", "");
            Console.WriteLine("Subject: "+ personalprueba);

            // Parse Mail Text/Plain body
            Console.WriteLine("TextBody: {0}", oMail.TextBody);

            // Parse Mail Html Body
            Console.WriteLine("HtmlBody: {0}", oMail.HtmlBody);

            // Parse Attachments
            EAGetMail.Attachment[] atts = oMail.Attachments;
            for (int i = 0; i < atts.Length; i++)
            {
                Console.WriteLine("Attachment: {0}", atts[i].Name);
            }
        }

        static void Main(string[] args)
        {

            //string curpath = Directory.GetCurrentDirectory();
            //string mailbox = String.Format("{0}\\inbox", curpath);
            string mailbox = String.Format("{0}\\mails_prueba", "C:\\");
            
            // If the folder is not existed, create it.
            if (!Directory.Exists(mailbox))
            {
                Directory.CreateDirectory(mailbox);
            }

            // Get all *.eml files in specified folder and parse it one by one.
            //string[] files = Directory.GetFiles(mailbox, "*.eml");
            string[] files = Directory.GetFiles(mailbox, "*.*");

            for (int i = 0; i < files.Length; i++)
            {
                ParseEmail(files[i]);
                Console.WriteLine("Cuenta: "+i);    
            }

            Console.ReadLine();

            /*            int counter = 0;
                        string line;

                        //Read line by line
                        string[] strArray = null;
                        string findThisString = "From:";
                        int strNumber;
                        int strIndex = 0;


                        // Read the file and display it line by line.
                        System.IO.StreamReader file =
                           new System.IO.StreamReader("C:\\mails_prueba\\1467129628.H461329P12217.cp01.chileautos.cl,S=4336_2,S");
                        while ((line = file.ReadLine()) != null)
                        {
                            Console.WriteLine(line);
                            strArray[counter] = line;
                            //Readiung line by line to find one tag
                            for (strNumber = 0; strNumber < strArray.Length; strNumber++)
                            {
                                strIndex = strArray[strNumber].IndexOf(findThisString);
                                Console.WriteLine(strArray[counter]);
                                if (strIndex >= 0)
                                    break;
                            }
                            //End

                            counter++;
                        }

                        file.Close();

                        // Suspend the screen.
                        Console.ReadLine();
                        */
        }
    }
}
