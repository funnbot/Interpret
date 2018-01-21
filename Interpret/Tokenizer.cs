using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Tokenizer {

    InputStream input;
    char current;

    public Tokenizer(InputStream input) {
        this.input = input;
    }

    public Token next() {
        current = input.next();

        remove_whitespace();
        if (is_comment(current)) {
            skip_comment();
            current = input.next();
            remove_whitespace();
        }

        if (is_cell(current)) return cell(current);
        if (is_operator(current)) return op(current);
        if (is_kw(current)) return keyword(current);

        if (is_int(current)) {
            string num = readwhile(is_int);
            return Int(num);
        }

        throw input.error("Invalid Character: " + current + " CharCode: " + (int)current);
    }

    string readwhile(Func<char, bool> action) {
        string result = "";
        result += current;
        while (action(input.peek()) && !eof()) {
            current = input.next();
            result += current;
        }
        return result;
    }

    void remove_whitespace() {
        if (whitespace(current)) {
            readwhile(whitespace);
            current = input.next();
        }
    }

    bool whitespace(char c) => (" " + (char)10 + (char)13).IndexOf(c) > -1;

    bool is_cell(char c) => "[]".IndexOf(c) > -1;
    Token cell(char c) {
        return Cell(c);
    }

    bool is_operator(char c) => "=+-".IndexOf(c) > -1;

    Token op(char c) {
        char nx = input.peek();
        string r = c.ToString();
        if (c == '+') {
            if (nx == '+') {
                input.next();
                r += "+";
            } else if (nx == '=') {
                input.next();
                r += "=";
            }
        } else if (c == '-') {
            if (nx == '-') {
                input.next();
                r += "-";
            } else if (nx == '=') {
                input.next();
                r += "=";
            }
        }
        return Op(r);
    }

    bool is_int(char c) => "0123456789".IndexOf(c) > -1;

    static string keywords = "if else while end";
    static string[] keywordsSplit = keywords.Split(' ');
    static char[] keywordLets = keywords.Replace(" ", "").ToCharArray().Distinct().ToArray();

    bool is_kw(char c) => keywords.IndexOf(c) > -1;
    Token keyword(char ch) {
        string kw = readwhile((char c) => {
            return Array.IndexOf(keywordLets, c) > -1;
        });
      
        if (Array.IndexOf(keywordsSplit, kw) > -1)
            return Kw(kw);

        throw input.error("Invalid Keyword: " + kw);
    }

    bool is_comment(char c) => c == '#';
    void skip_comment() {
        readwhile((char c) => {
            return c != (char)10;
        });
    }

    public static Token Cell(char i) {
        return new Token(i.ToString(), TokenType.Cell);
    }

    public static Token Op(string i) {
        return new Token(i, TokenType.Operator);
    }

    public static Token Kw(string i) {
        return new Token(i, TokenType.Keyword);
    }

    public static Token Int(string i) {
        return new Token(i, TokenType.Int);
    }

    public bool eof() => input.eof();
    public System.Exception error(string msg) => input.error(msg);
}

public struct Token {
    public string ch;
    public TokenType type;

    public Token(string ch, TokenType type) {
        this.ch = ch;
        this.type = type;
    }
}

public enum TokenType {
    Cell, // [ ]
    Operator, // = + - += -= ++ --
    Keyword, // if else while end
    Int, // Integer
}