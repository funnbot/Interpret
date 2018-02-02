using System;
using System.Collections;
using System.Collections.Generic;

public class Parser {

    List<Prog> prog;
    Tokenizer input;

    public Parser(Tokenizer input) {
        this.input = input; 
        prog = new List<Prog>();
        while (!input.Eof()) {
            prog.Add(ParseExpression());
        }
    }

    Prog ParseExpression() {
        Token tok = input.Next();
        if (tok.val == "[") return ParseCell(tok);
        return new Prog();
    }

    Prog ParseCell(Token tok) {
        Token nx = input.Next();
        if (nx.val == "[") {
            input.Next();
            return new CellProg(new CellProg(ParseInt(input.Peek())));
        } if (nx.type == "num") 
            return new CellProg(ParseInt(nx));
        throw TypeError(nx.val);
    }

    Prog ParseInt(Token tok) {
        int x;
        if (!Int32.TryParse(tok.val, out x)) 
            throw TypeError(tok.val);
        return new NumProg(x);
    }

    Exception TypeError(string msg) => input.Error("Unexpected Token: " + msg);
    public List<Prog> Parse() => prog;
}

public class Prog {
    public Prog index;
}
public class CellProg : Prog {
    public Prog index;

    public CellProg(Prog index) {
        this.index = index;
    }
}

public class NumProg : Prog {
    public int val;

    public NumProg(int val) {
        this.val = val;
    }
}

public class BoolProg : Prog {
    public bool val;

    public BoolProg(bool val) {
        this.val = val;
    }
}

public class CondProg : Prog {
    public string type;
    public Prog cond;
    public Prog then;
    public Prog or;

    public CondProg(string type, Prog cond, Prog then, Prog or) {
        this.type = type;
        this.cond = cond;
        this.then = then;
        this.or = or;
    }
}

public class AssignProg : Prog {
    public string op;
    public Prog left;
    public Prog right;

    public AssignProg(string op, Prog left, Prog right) {
        this.op = op;
        this.left = left;
        this.right = right;
    }
}

public class BinProg : Prog {
    public AssignProg op;
    public Prog left;
    public Prog right;

    public BinProg(AssignProg op, Prog left, Prog right) {
        this.op = op;
        this.left = left;
        this.right = right;
    }
}