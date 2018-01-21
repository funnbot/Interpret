﻿

public class InputStream {

    char[] source;
    public int pos;
    public int line = 1;
    public int col;

    public InputStream(string rawSource) {
        source = rawSource.ToCharArray();
    }

    public char next() {
        if (eof()) throw error("Ran past end of file");
        char ch = peek();
        if (ch == '\n') {
            line++;
            col = 0;
        } else {
            col++;
        }
        pos++;
        return ch;
    }

    public char peek() => source[pos];

    public bool eof() => pos >= source.Length - 1;

    public System.Exception error(string msg) {
        return new System.Exception(msg + "(" + line + ":" + col + ")");
    }
}
