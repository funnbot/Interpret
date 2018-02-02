using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

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
            Parser parser = new Parser(tokenizer);
            List<Prog> parsed = parser.Parse();
            Console.Write(GetLogFor(parsed[0]));
        }

        public static string GetLogFor(object target) {
            var properties =
                from property in target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                select new {
                    Name = property.Name,
                    Value = property.GetValue(target, null)
                };

            var builder = new StringBuilder();

            foreach (var property in properties) {
                builder
                    .Append(property.Name)
                    .Append(" = ")
                    .Append(property.Value)
                    .AppendLine();
            }

            return builder.ToString();
        }
    }

}