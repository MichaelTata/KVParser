using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

using KVParser;
using System.Reflection;

namespace kvptest
{
    [TestClass]
    public class UnitTest1
    {
        string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


        [TestMethod]
        public void ATestMultipleWrite()
        {

            string dir = currentPath + "/testfiles";
            //Only tests the write once. 
            if (Directory.Exists(dir))
            {

            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(dir);
                KeyValParser.Writer(dir, 5, 3);

            }


            string[] allfiles = Directory.GetFiles(dir, "*.kv", SearchOption.AllDirectories);
     
            Assert.AreEqual(5, allfiles.Length);

        }


        [TestMethod]
        public void TestMultipleRead()
        {
            string dir = currentPath + "/testfiles";

            KeyValParser kvp = new KeyValParser(dir);
            kvp.Reader();
            kvp.DisplayDictionary();

            Assert.AreEqual(15, kvp.GetDictSize());
        }

        //This test needs the manual example file.
        [TestMethod]
        public void TestDuplicateRead()
        {
            string dir = Directory.GetCurrentDirectory();


            KeyValParser kvp = new KeyValParser(dir);
            kvp.Reader();
            kvp.DisplayDictionary();




            Assert.AreEqual(2, kvp.GetDictValFromKey("Node").Count);
            Assert.AreEqual(2, kvp.GetDictValFromKey("OBJECT_ID").Count);
            Assert.AreEqual(1, kvp.GetDictValFromKey("Key").Count);
        }

        //Assure manual example file present
        [TestMethod]
        public void TestStrictRead()
        {
            string dir = Directory.GetCurrentDirectory();


            KeyValParser kvp = new KeyValParser(dir);
            kvp.Reader();
            kvp.DisplayDictionary();




            Assert.AreEqual(2, kvp.GetDictValFromKey("Node").Count);
            Assert.AreEqual(1, kvp.GetDictValFromKey("node").Count);       
        }


        //Implemented After Time limit
        [TestMethod]
        public void A_StressVolumeWrite()
        {
            string dir = currentPath + "/VolumeStressTest";
            //Only tests the write once. 
            if (Directory.Exists(dir))
            {

            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(dir);
                KeyValParser.Writer(dir, 10000, 3);

            }


            string[] allfiles = Directory.GetFiles(dir, "*.kv", SearchOption.AllDirectories);

            
            Assert.AreEqual(10000, allfiles.Length);

        }

        //Implemented After Time limit
        [TestMethod]
        public void B_StressVolumeRead()
        {
            string dir = currentPath + "/VolumeStressTest";
            
            KeyValParser kvp = new KeyValParser(dir);
            kvp.Reader();

            
            Assert.AreEqual(30000, kvp.GetDictSize() + kvp.GetDuplicateCount());

        }

        //Implemented After Time limit
        [TestMethod]
        public void A_LargeStressWrite()
        {
            string dir = currentPath + "/FileStressTest";
            //Only tests the write once. 
            if (Directory.Exists(dir))
            {

            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(dir);
                KeyValParser.Writer(dir, 3, 1000000);

            }


            string[] allfiles = Directory.GetFiles(dir, "*.kv", SearchOption.AllDirectories);

            
            Assert.AreEqual(3, allfiles.Length);

        }

        //Implemented After Time limit
        [TestMethod]
        public void B_LargeStressRead()
        {
            string dir = currentPath + "/FileStressTest";

            KeyValParser kvp = new KeyValParser(dir);
            kvp.Reader();

            Assert.AreEqual(3000000, kvp.GetDictSize()+kvp.GetDuplicateCount());

        }


    }
}
