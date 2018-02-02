using System;
using System.Linq;

public class Tokenizer {

    string keywords = "if else while end";

    InputStream input;
    Token current = null;

    public Tokenizer(InputStream input) {
        this.input = input;
        kwLets = keywords.Replace(" ", "").ToCharArray().Distinct().ToArray();
        kwSplit = keywords.Split(' ');
    }

    bool IsKeyword(string c) => kwLets.Contains(c[0]);

    bool IsDigit(string c) => "0123456789".Contains(c);

    bool IsCell(string c) => "[]".Contains(c);

    bool IsOperator(string c) => "=+-".Contains(c);

    bool IsWhitespace(string c) => $" {(char)10}{(char)13}".Contains(c);

    void SkipComment() {
        ReadWhile((string c) => c != "\n");
        input.Next();
    }

    Token ReadNumber() {
        string num = ReadWhile(IsDigit);
        return new Token("num", num);
    }

    Token ReadOperator() {
        string op = ReadWhile(IsOperator);
        return new Token("op", op);
    }

    string[] kwSplit;
    char[] kwLets;
    Token ReadKeyword() {
        string kw = ReadWhile((string c) => Array.IndexOf(kwLets, c[0]) > -1);
        if (Array.IndexOf(kwSplit, kw) > -1) return new Token("kw", kw);
        throw input.Error("Invalid Keyword: " + kw);
    }

    string ReadWhile(Func<string, bool> action) {
        string res = "";
        while (!input.Eof() && action(input.Peek()))
            res += input.Next();
        return res;
    }

    Token ReadNext() {
        ReadWhile(IsWhitespace);
        if (input.Eof()) return null;

        string ch = input.Peek();
        if (ch == "#") {
            SkipComment();
            return ReadNext();
        }
        if (IsCell(ch)) {
            input.Next();
            return new Token("cell", ch);
        }
        if (IsDigit(ch)) return ReadNumber();
        if (IsKeyword(ch)) return ReadKeyword();
        if (IsOperator(ch)) return ReadOperator();

        throw input.Error("Invalid Character: " + ch);
    }

    public Token Peek() {
        if (current == null) current = ReadNext();
        return current;
    }

    public Token Next() {
        Token tok = current;
        current = null;
        if (tok == null) return ReadNext();
        return tok;
    }

    public bool Eof() => Peek() == null;
    public Exception Error(string msg) => input.Error(msg);
}

public class Token {
    public string type;
    public string val;

    public Token(string type, string val) {
        this.type = type;
        this.val = val;
    }
}