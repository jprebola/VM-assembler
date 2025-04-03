public class Dup : IInstruction {
    private readonly int _offset;
    public Dup(int offset) {
        _offset = offset & ~3;
    }
    public int Encode() {
        return (0b1100 << 28) | _offset;
    }
}

public class Exit : IInstruction {
    private readonly int _code;
    public Exit(int code){
        _code = code;
    }
    public int Encode(){
        return (0b0000 << 28) | _code;
    }
}