using Antlr4.Runtime;

namespace st2c
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            PLCSTPARSERLexer pLCSTPARSERLexer = new PLCSTPARSERLexer(new AntlrInputStream(Console.In));
            CommonTokenStream commonTokenStream = new CommonTokenStream(pLCSTPARSERLexer);
            PLCSTPARSERParser pLCSTPARSERParser = new PLCSTPARSERParser(commonTokenStream);
            pLCSTPARSERParser.RemoveErrorListeners();
            pLCSTPARSERParser.AddErrorListener(new ConsoleErrorListener<IToken>());
            var tree = pLCSTPARSERParser.startpoint();
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
    }
}
