using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCTranslator
{
    public class PLCTargetFile
    {
        private static FileStream targetFile;
        private static StreamWriter outputWriter;

        static TargetFileOutput()
        {
            try
            {
                targetFile = new FileStream("main.cpp", FileMode.Create);
                outputWriter = new StreamWriter(targetFile);
            }
            catch (Exception e)
            {
                throw new Exception("File initialization failed", e);
            }
        }
        public static void TargetFileWrite(String cppCodeStr) 
        {
            try
            {
                outputWriter.Write(cppCodeStr);
            }
            catch (Exception e)
            {
                Console.WriteLine("TargetFile Error: " + e.Message);
            }

        }
    }
}
