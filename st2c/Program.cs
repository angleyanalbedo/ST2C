using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using st2c.PLCSymbolAndScope.PLCSymbols;
using st2c.PLCTranslator;
using st2c.staticCheckVisitor;

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
            if (stFiles.Length == 0)
            {
                Console.WriteLine("No ST files found in the current directory.");
                return;
            }
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
                    var parseTree = pLCSTPARSERParser.startpoint();
                    // 输出解析树
                    if (parseTree != null)
                    {
                        ParseTreeProperty<List<PLCSymbol>> property = new ParseTreeProperty<List<PLCSymbol>>();
                        PLCVisitor plcVisitor = new PLCVisitor(property);

                        plcVisitor.visit(parseTree);
                        PLCTranslatorNew translatorNew = new PLCTranslatorNew(property);

                        translatorNew.visit(parseTree);
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
            Console.WriteLine("Press any key to exit...");

        }
    }
}
