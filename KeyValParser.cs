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

        }





    }
}
