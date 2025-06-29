using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace st2c.PLCTargetFileOutPut
{
    public class TargetFileOutput
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

        public TargetFileOutput()
        {
        }

        public static void WriteSentence(string outputSentence)
        {
            outputWriter.Write(outputSentence);
        }

        public static void WriteTarget(string readSentence)
        {
            try
            {
                WriteSentence(readSentence);
            }
            catch (Exception e)
            {
                Console.WriteLine("TargetFile Error: " + e.Message);
            }
        }

        public static void CloseBufferAndFileWriter()
        {
            outputWriter.Close();
            targetFile.Close();
        }

        public static void CloseWriter()
        {
            try
            {
                CloseBufferAndFileWriter();
            }
            catch (Exception e)
            {
                Console.WriteLine("Close Writer Error: " + e.Message);
            }
        }
    }
}
