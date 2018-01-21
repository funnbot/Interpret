using System.IO;
using System;

namespace Interpret {
    
    class MainClass {
        public static void Main(string[] args) {

            if (args.Length == 0) {
                Console.WriteLine("Please enter a file path.");
                return;
            }

            string source = File.ReadAllText(args[0]);

            InputStream inputStream = new InputStream(source);
            Tokenizer tokenizer = new Tokenizer(inputStream);
            while (!tokenizer.eof()) {
                Token token = tokenizer.next();
                Console.WriteLine("Key " + token.ch + " Type: " + token.type);
            }
        }
    }

}