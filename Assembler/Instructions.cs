public class Dup : IInstruction {
    private readonly int _offset;
    public Dup(int offset) {
        _offset = offset & ~3;
    }
    public int Encode() {
        return (0b1100 << 28) | _offset;
    }
    
}

public class Push : IInstruction {
    private readonly int _value;
    public Push(int value){
        _value = value;
    }
    public int Encode() {
        int opcode = 0b1111 << 28;
        int masked = _value & 0x0FFFFFFF; // Keep lower 28 bits
        return opcode | masked;
    }
}

public class Pop : IInstruction {
    private readonly uint _offset;
    public Pop(uint offset){
        _offset = offset;
    }
    public int Encode(){
        return (1 << 28) | ((int)_offset & 0x0FFFFFFF);
    }
}

public class Exit : IInstruction {
    private readonly int _code;
    public Exit(int code){
        _code = code & 0xF;
    }
    public int Encode(){
        return (0b0 << 30) | _code;
    }
}

public class Stprint : IInstruction {
    private readonly int _offset;
    public Stprint(int offset){
        _offset = offset;
    }
    public int Encode(){
        return (0b0100 << 28) | (_offset & ~0b11);
    }
}

public class Call : IInstruction {
    private readonly int _offset;
    public Call(int offset){
        _offset = offset;
    }
    public int Encode(){
        return (0b0101 << 28) | (_offset & ~0b11);
    }
}

public class Return : IInstruction {
    private readonly int _offset;
    public Return(int offset){
        _offset = offset;
    }
    public int Encode(){
        return (0b0110 << 28) | (_offset & ~0b11);
    }
}

