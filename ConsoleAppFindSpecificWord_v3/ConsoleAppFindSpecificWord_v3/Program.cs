using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppFindSpecificWord_v3
{

    #region Class displaying Comments

    class Program
    {

            static void Main(string[] args)
            {
                //Set the actual path from your system
                const string strFilePath = @"C:\mails_prueba\1467129628.H461329P12217.cp01.chileautos.cl,S=4336_2,S";
                ReturnComments objReturnComemnts = new ReturnComments();

                Console.WriteLine("Single Line Comments : ");
                String strSingleLine = objReturnComemnts.GetCommentsinUpperCase(strFilePath, "SingleLine");
                Console.WriteLine(strSingleLine);
                Console.WriteLine("MultiLine Comments : ");
                String strMultileLine = objReturnComemnts.GetCommentsinUpperCase(strFilePath, "MultiLine");
                Console.WriteLine(strMultileLine);
                Console.Read();

                // Suspend the screen.
                Console.ReadLine();
            }

        #endregion

        #region Class containing methods to read comments
        class ReturnComments
        {
            public String GetCommentsinUpperCase(String strFilePath, String strCase)
            {
                int counter = 0;
                string line;
                string strRetunComments = String.Empty;
                StringBuilder sbSingleLine = new StringBuilder();
                StringBuilder sbMultiLine = new StringBuilder();
                //Create streamReader object
                StreamReader file = new StreamReader(strFilePath);
                //Read line by line
                while ((line = file.ReadLine()) != null)
                {
                    switch (strCase)
                    {
                        case "SingleLine":
                            if (line.Contains("//")) //for singleline comment
                            {
                                //Get rest part of line in uppercase and move the counter to next line
                                String strLine = line.Replace("//", "");
                                strLine.ToUpper();
                                sbSingleLine.AppendLine(strLine);
                                counter++;
                            }
                            strRetunComments = sbSingleLine.ToString();
                            break;
                        case "MultiLine":
                            if (line.Contains("/*")) //for multiple line comments
                            {
                                String strLine = line.Replace("/*", "");
                                strLine.ToUpper();
                                sbMultiLine.AppendLine(strLine);
                                counter++;
                            }
                            if (line.Contains("*/")) //Termination of multiple line comments
                            {
                                String strLine = line.Replace("*/", "");
                                strLine.ToUpper();
                                sbMultiLine.AppendLine(strLine);
                                counter++;
                            }

                            strRetunComments = sbMultiLine.ToString();
                            break;
                        default:
                            //by default line which does not fall in above cases may be treated as a part of multiple comment
                            //in this case I supposed that this is a part of multiline comment
                            line.ToUpper();
                            sbMultiLine.AppendLine(line);
                            counter++;
                            break;
                    }

                }
                sbSingleLine = null;
                sbMultiLine = null;
                file.Close();

                return strRetunComments;
            }
        }
        #endregion

        /* Note : From the help of above you can also get the other details like Author Name, Description etc.
         * by Checking cases. */

    }
}
