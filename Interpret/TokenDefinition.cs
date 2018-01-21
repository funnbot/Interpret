public static class Tokens {
    public static Token Cell(string i) {
        return new Token(i, TokenType.Cell);
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