using System;

public class Tokenizer {

    InputStream input;

    char current;

    string newLine = Environment.NewLine;

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
            return Tokens.Int(num);
        }

        throw input.error("Invalid Character: " + current + " CharCode: " + (int)current);
    }

    string readwhile(Func<char, bool> action) {
        string result = "";
        result += current;
        while (action(input.peek()) && !input.eof()) {
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
        if (c == '[') return Tokens.cell_start;
        // if (c == ']') 
        return Tokens.cell_end;
    }

    bool is_operator(char c) => "=+-".IndexOf(c) > -1;
    Token op(char c) {
        char nx = input.peek();
        if (c == '=') return Tokens.equal;
        if (c == '+') {
            if (nx == '+') {
                input.next();
                return Tokens.increment;
            }
            if (nx == '=') { 
                input.next();
                return Tokens.plus_equals;
            }
            return Tokens.plus;
        }
        //if (c == '-') {
        if (nx == '+') { 
            input.next();
            return Tokens.decrement;
        }
        if (nx == '=') { 
            input.next();
            return Tokens.minus_equals;
        }
        return Tokens.minus;
        //}
    }

    bool is_int(char c) => "0123456789".IndexOf(c) > -1;

    // string keywords = "if else while end";
    string keywordStarts = "iew";
    string keywordChars = "ifelswhnd";

    bool is_kw(char c) => keywordStarts.IndexOf(c) > -1;
    Token keyword(char ch) {
        string kw = readwhile((char c) => {
            return keywordChars.IndexOf(c) > -1;
        });
        if (kw == "if") return Tokens.IF;
        if (kw == "else") return Tokens.ELSE;
        if (kw == "while") return Tokens.WHILE;
        //if (kw == "end")
        return Tokens.END;
    }

    bool is_comment(char c) => c == '#';
    void skip_comment() {
        readwhile((char c) => {
            return c != (char)10;
        });
    }

    public bool eof() {
        return input.eof();
    }
}