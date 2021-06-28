using System;
using System.IO;
using KVParser;

namespace Program
{
    class Driver
    {
        static void Main(string[] args)
        {

            //When using the driver executable, pass 0(or Nothing at all) to read, or 1 to write.
            //For both options, you can also include path to search or write to as the next argument. 
            if (args.Length > 0)
            {
                string dir;
                if (args.Length == 2)
                {
                    dir = args[1];
                }
                else
                {
                    dir = Directory.GetCurrentDirectory();
                }


                if (args[0] == "0" || args[0] == "read")
                {
                    //Read all .kv files in current directory then display the key+values.
                    KeyValParser kvp = new KeyValParser(dir);
                    kvp.Reader();
                    kvp.DisplayDictionary();
                }

                if (args[0] == "1" || args[0] == "write")
                {

                    //Default to current working directory, create 50 files, with 1000 examples each.
                    KeyValParser.Writer(dir, 50, 1000);
                }

            }
            else //If not run with any args, just run the reader for the current directory. 
            {
                KeyValParser kvp = new KeyValParser();
                kvp.Reader();
                kvp.DisplayDictionary();

            }





        }
    }
}
