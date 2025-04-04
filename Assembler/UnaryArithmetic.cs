public class Neg : IInstruction {
    private readonly int _tag;
    public Neg() {
        _tag = 0;
    }
    public int Encode() {
        return (0b0011 << 28) | (_tag << 24);
    }
}

public class Not : IInstruction {
    private readonly int _tag;
    public Not() {
        _tag = 1;
    }
    public int Encode() {
        return (0b0011 << 28) | (_tag << 24);
    }
}


