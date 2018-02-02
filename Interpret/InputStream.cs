using System;

public class InputStream {

    string source;
    int pos;
    int line = 1;
    int col;

    public InputStream(string source) {
        this.source = source;
    }

    public string Next() {
        string ch = source[pos++].ToString();
        if (ch == "\n") {
            line++;
            col = 0;
        } else col++;
        return ch;
    }

    public string Peek() => source[pos].ToString();

    public bool Eof() => pos >= source.Length;

    public Exception Error(string msg) {
        return new Exception(msg + "(" + line + ":" + col + ")");
    }
}
