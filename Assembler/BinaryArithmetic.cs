public class Dup : IInstruction {
    private readonly int _tag;
    public Add() {
        _tag = 0;
    }
    public int Encode() {
        return (0b0010 << 28) | _tag;
    }
}

public class Sub : IInstruction {
    private readonly int _tag;
    public Sub() {
        _tag = 1;
    }
    public int Encode() {
        return (0b0010 << 28) | _tag;
    }
}

public class Mul : IInstruction {
    private readonly int _tag;
    public Mul() {
        _tag = 2;
    }
    public int Encode() {
        return (0b0010 << 28) | _tag;
    }
}

public class Div : IInstruction {
    private readonly int _tag;
    public Div() {
        _tag = 3;
    }
    public int Encode() {
        return (0b0010 << 28) | _tag;
    }
}

public class Rem : IInstruction {
    private readonly int _tag;
    public Rem() {
        _tag = 4;
    }
    public int Encode() {
        return (0b0010 << 28) | _tag;
    }
}

public class And : IInstruction {
    private readonly int _tag;
    public And() {
        _tag = 5;
    }
    public int Encode() {
        return (0b0010 << 28) | _tag;
    }
}

public class Or : IInstruction {
    private readonly int _tag;
    public Or() {
        _tag = 6;
    }
    public int Encode() {
        return (0b0010 << 28) | _tag;
    }
}

public class Xor : IInstruction {
    private readonly int _tag;
    public Xor() {
        _tag = 7;
    }
    public int Encode() {
        return (0b0010 << 28) | _tag;
    }
}

public class Lsl : IInstruction {
    private readonly int _tag;
    public Lsl() {
        _tag = 8;
    }
    public int Encode() {
        return (0b0010 << 28) | _tag;
    }
}

public class Lsr : IInstruction {
    private readonly int _tag;
    public Lsr() {
        _tag = 9;
    }
    public int Encode() {
        return (0b0010 << 28) | _tag;
    }
}

public class Asr : IInstruction {
    private readonly int _tag;
    public Asr() {
        _tag = 11;
    }
    public int Encode() {
        return (0b0010 << 28) | _tag;
    }
}


