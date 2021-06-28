using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace KVParser
{
    public class KeyValParser
    {
        //List is unsafe even inside concurrentdict. Thread safe by key access, but actual value may be changed in unsafe manner via multiple threads, so we will use a lock around the list.
        private ConcurrentDictionary<string, List<string>> dict;
        private string dir;

        /// <summary>
        /// Defualt constructor, uses current directory as path to read.
        /// </summary>
        public KeyValParser()
        {
            //Shared dictionary to hold all files kv pairs.
            dict = new ConcurrentDictionary<string, List<string>>();
            dir = Directory.GetCurrentDirectory();

        }

        /// <summary>
        /// Parameterized constructor to set up class with a directory. Uses passed directory to read files from. 
        ///
        /// </summary>
        /// <param name="directory"></param>
        public KeyValParser(string directory)
        {
            dict = new ConcurrentDictionary<string, List<string>>();
            dir = directory;
        }

        /// <summary>
        /// Displays to console the contents of the dictionaries(all key value pairs)
        /// </summary>
        public void DisplayDictionary()
        {

            foreach (KeyValuePair<string, List<string>> kv in dict)
            {
                Console.Write("Key:{0}     Values:", kv.Key);
                foreach (string s in kv.Value)
                {

                    Console.Write("  {0} ", s);
                }
                Console.Write("\n");
            }

        }


        /// <summary>
        /// Helper Function to grab values for a certain key. 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public List<string> GetDictValFromKey(string val)
        {
            if (dict.ContainsKey(val))
            {
                return dict[val];
            }

            return null;
        }


        /// <summary>
        /// Helper Function to grab the dictionaries size. 
        /// </summary>
        /// <returns></returns>
        public int GetDictSize()
        {
            return dict.Count;
        }


        /// <summary>
        /// KeyValue(.kv) File Reader & Parser. Searches through everything in dir(directory and subdirectories) and takes all key value pairs in kv files
        /// and stores them in a dictionary. 
        /// </summary>
        /// <param name="dir"></param>
        public void Reader()
        {
            //Get all files with kv extension in the directory. This will find in subdirectories as well. 
            string[] allfiles = Directory.GetFiles(dir, "*.kv", SearchOption.AllDirectories);




        }



        /// <summary>
        /// Writes Random key value pairs to a file. Directory is where all files will be written
        /// filecount is the amount of files to create
        /// and examplecount is the amount of examples per file to write.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="filecount"></param>
        /// <param name="examplecount"></param>
        public static void Writer(string dir, int filecount, int examplecount)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random randomize = new Random();
            var newstring = new char[12];

            Directory.SetCurrentDirectory(dir);

            for (int i = 0; i < filecount; i++)
            {
                string file = "kvfile" + i + ".kv";


                using (StreamWriter sw = new StreamWriter(file))
                {
                    for (int j = 0; j < examplecount; j++)
                    {

                        for (int k = 0; k < newstring.Length; k++)
                        {
                            newstring[k] = characters[randomize.Next(characters.Length)];
                        }

                        string result = new string(newstring);

                        int value = randomize.Next();

                        sw.WriteLine(result + "=" + value);
                    }
                }
            }
        }





    }
}
