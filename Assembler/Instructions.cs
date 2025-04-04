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
        _offset = (uint)(offset & ~0b11);
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
        return (0b0000 << 28) | _code; // i may have fucked this up shrug -- i think it's right CH 
    }
}
public class Swap : IInstruction {
    private readonly int _from;
    private readonly int _to;
    public Swap(){
        _from = 4;
        _to = 0;
    }
    public Swap(int from, int to){
        _from = from & 0xFF;
        _to = to & 0xFF;
    }
    
    public int Encode() {
        int opcode = 0b0000 << 28;
        int subcode = 0b0001 << 24;
        int fromEncoded = (_from >> 2) & 0xFFF; // 12 bits
        int toEncoded = (_to >> 2) & 0xFFF;
        return opcode | subcode | (fromEncoded << 12) | toEncoded;
    }
}

public class Nop : IInstruction {
    public int Encode() {
        return (0b0000 << 28) | (0b0010 << 24);
    }
}
public class Input : IInstruction {
    public int Encode() {
        return (0b0000 << 28) | (0b0100 << 24);
    }
}

public class Stinput : IInstruction {
    private int _maxChars;
    public Stinput(int size = 0x00FF_FFFF){
        _maxChars = size;
    }

    public int Encode() {
        return (0b00000101 << 24) | (_maxChars);
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
//i think this is right?
public class Debug : IInstruction {
    private readonly int _value;
    public Debug(int value = 0) {
        _value = value & 0xFFFFFF;  // ensure val fits in 24 bits
    }
    public int Encode() {
        return (0b0001 << 28) | _value;  // opcode in bits [31:28], val in bits [23:0]
    }
}
public class Call : IInstruction {
    private int _offset;
    public Call(int labelPos, int currentPos){
        _offset = labelPos - currentPos;
    }
    public int Encode(){
        return (0b0101 << 28) | (_offset << 2);
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
    public Goto(int labelPos, int currentPos){
        _reloffset = (labelPos - currentPos);
    }
    public int Encode(){
        return (0b0111 << 28) | (_reloffset << 2);
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
            return output | (conditionCode << 24) | (_offset << 2);
        }else{
            output = 0b1001 << 28;
            return output | (conditionCode << 24) | (_offset << 2);
        }
    }
}

public class Print : IInstruction {
    private int _offset, _format;
    public Print(int offset, char fmt) {
        _offset = offset;
        switch(fmt) {
            case 'h':
                _format = 0b01;
                break;
            
            case 'o':
                _format = 0b11;
                break;

            case 'b':
                _format = 0b10;
                break;
        }
    }


    public int Encode() {
        return (0b1100 << 28) | (_offset << 2) | _format;
    }  
}
