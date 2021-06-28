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

                    Console.Write(" {0} ", s);
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

            //How many files to process in parallel
            const int step = 3;
            var options = new ParallelOptions();

            //Determine if at least 3 processors are available, if not just use the max available. 
            //Limiting processor usage to 3 because we are IO Bound. Parallel IO operations can be slower
            //than sequential because of variation in seeks on a disk, if the CPU was the bottleneck we could go as high as we wanted here.
            if (Environment.ProcessorCount < 3)
            {
                options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                };
            }
            else
            {
                options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = 3
                };
            }


            int i = 0;
            int limit = step;
            string line;


        

            //Loop through all files.
            while (i < allfiles.Length)
            {

                //Process 3 Files at a time as determined by i to limit and parallel options. Again bounded by IO device here. 
                //If using a slower hard drive performance may be worse than sequential due to simultaneous seeks. 
                Parallel.For(i, limit, options, i =>
                {

                    using (StreamReader sr = new StreamReader(allfiles[i]))
                    {

                        //Read A single line into memory at a time and process it as we read it in, this reduces active memory usage from the files
                        //and with processing in parallel may speed up 
                        line = sr.ReadLine();

                        while (line != null)
                        {

                            //Split on '=', then check if the key is in the dictionary. 
                            string[] splitline = line.Split('=');

                            if (!dict.ContainsKey(splitline[0]))
                            {
                                List<string> temp = new List<string>();
                                dict.TryAdd(splitline[0], temp);

                                //Lock the list corresponding to this key, as it is not thread safe.
                                lock (dict[splitline[0]])
                                {
                                    dict[splitline[0]].Add(splitline[1]);
                                }

                            }
                            else
                            {
                                //Lock the list corresponding to this key, as it is not thread safe.
                                lock (dict[splitline[0]])
                                {
                                    dict[splitline[0]].Add(splitline[1]);
                                }

                            }
                            //Read Next Line.
                            line = sr.ReadLine();
                        }

                        sr.Close();

                    }

                });

                //Increment our values to process next files.
                i += step;
                limit += step;

                if (limit > allfiles.Length - 1)
                {
                    limit = allfiles.Length;
                }

            }


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
