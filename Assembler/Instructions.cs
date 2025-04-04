using System.Runtime.InteropServices;
using System.Xml;

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
    private readonly long _offset;
    public Pop(uint offset){
        _offset = offset & ~0b11;
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

public class Goto : IInstruction {
    private readonly int _reloffset;
    public Goto(int reloffset){
        _reloffset = reloffset & ~0b11;
    }
    public int Encode(){
        return (0b0111 << 28) | (_reloffset & 0x0FFFFFFF);
    }
}

public class Dump : IInstruction {
    public Dump() {}
    public int Encode(){
        return 0b110 << 28;
    }
}

public class If : IInstruction {
    private readonly string _suffix;
    private readonly int _offset;

    public If(string suffix, int labelPos, int line){
        _suffix = suffix;
        _offset = labelPos - line;
    }
    public int Encode(){
        bool binary = true;
        int conditionCode = 0b0, output;

        switch(_suffix){
            case "eq":
                conditionCode = 0b000;
                break;
            
            case "ne":
                conditionCode = 0b001;
                break;
            
            case "lt":
                conditionCode = 0b010;
                break;
            
            case "gt":
                conditionCode = 0b011;
                break;
            
            case "le":
                conditionCode = 0b100;
                break;
            
            case "ge":
                conditionCode = 0b101;
                break;
            
            case "ez":
                conditionCode = 0b00;
                binary = false;
                break;
            
            case "nz":
                conditionCode = 0b01;
                binary = false;
                break;
            
            case "mi":
                conditionCode = 0b10;
                binary = false;
                break;
            
            case "pl":
                conditionCode = 0b11;
                binary = false;
                break;
        }

        if(binary){
            output = 0b1000 << 28;
            return output ^ (conditionCode << 24) ^ (_offset << 2);
        }else{
            output = 0b1001 << 28;
            return output ^ (conditionCode << 24) ^ (_offset << 2);
        }
    }
}
