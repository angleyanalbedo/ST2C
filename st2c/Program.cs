using Antlr4.Runtime;

namespace st2c
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // 获得当前工作目录
            string currentDirectory = Environment.CurrentDirectory;
            // 输出当前工作目录
            Console.WriteLine($"Current Directory: {currentDirectory}");

            // 获取工作目录下的ST文件

            string[] stFiles = Directory.GetFiles(currentDirectory, "*.st");
            foreach (string stFile in stFiles)
            {
                //读取ST文件内容到budder[]

                try
                {
                    string fileContent = File.ReadAllText(stFile);
                    Console.WriteLine($"File: {stFile}");
                    Console.WriteLine(fileContent);
                    PLCSTPARSERLexer pLCSTPARSERLexer = new PLCSTPARSERLexer(new AntlrInputStream(fileContent));
                    CommonTokenStream commonTokenStream = new CommonTokenStream(pLCSTPARSERLexer);
                    PLCSTPARSERParser pLCSTPARSERParser = new PLCSTPARSERParser(commonTokenStream);
                    pLCSTPARSERParser.RemoveErrorListeners();
                    pLCSTPARSERParser.AddErrorListener(new ConsoleErrorListener<IToken>());
                    var tree = pLCSTPARSERParser.startpoint();
                    // 输出解析树
                    if (tree != null)
                    {
                        Console.WriteLine("Parsing completed successfully.");
                        Console.WriteLine(tree.ToStringTree(pLCSTPARSERParser));
                    }
                    else
                    {
                        Console.WriteLine("Parsing failed.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {stFile}: {ex.Message}");
                }

            }

            
        }
    }
}
