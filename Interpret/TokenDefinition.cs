public static class Tokens {
    public static Token cell_start = new Token("[", TokenType.Cell);
    public static Token cell_end = new Token("]", TokenType.Cell);

    public static Token equal = new Token("=", TokenType.Operator);
    public static Token plus = new Token("+", TokenType.Operator);
    public static Token minus = new Token("-", TokenType.Operator);
    public static Token plus_equals = new Token("+=", TokenType.Operator);
    public static Token minus_equals = new Token("-=", TokenType.Operator);
    public static Token increment = new Token("++", TokenType.Operator);
    public static Token decrement = new Token("--", TokenType.Operator);

    public static Token IF = new Token("if", TokenType.Keyword);
    public static Token ELSE = new Token("else", TokenType.Keyword);
    public static Token WHILE = new Token("while", TokenType.Keyword);
    public static Token END = new Token("end", TokenType.Keyword);

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