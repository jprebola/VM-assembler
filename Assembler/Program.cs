using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;

class Assembler{
    public static void Main(string[] args){
        StreamReader sr;
        string? line;
        string push_str, tmp;
        int j, str_start, str_end, comment_index;
        List<string> push_sub_strs;
        List<int> push_ints;
        List<IInstruction> instructions = new List<IInstruction>();

        if(args.Length < 2){
            Console.WriteLine("Usage: assemble <file.asm> <file.v>");
        }

        try{
            sr = new StreamReader(args[0]);
        }
        catch(Exception e){
            Console.WriteLine("Error reading file: " + e.Message);
            return;
        }

        uint lines = 1;
        uint numInstructions = 0;
        Dictionary<string, int> labels = new Dictionary<string, int>();

        push_sub_strs = new List<string>();
        push_ints = new List<int>();

        while((line = sr.ReadLine()) != null){
            List<string> data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            bool isInstruction = true;
            
            //check for label
            if(data.Count == 1 && data[0].Contains(':')){
                //remove ':' from the name and store label info in dictionary
                data[0].Remove(data[0].Length - 1, 1);
                labels[data[0]] = (int)numInstructions * 4;
                isInstruction = false;
            }

            //Check for comment
            for(int i = 0; i < data.Count; i++){
                if(data[i].Contains('#') && data[i].IndexOf('#') == 0){
                    data.RemoveRange(i, data.Count - i);
                    break;
                }
            }
            //DONT FUCKING FORGET HANDLING STPUSH W/ numInstructions

            if(data.Count > 0 && data[0].StartsWith("if")){
                 data[0].Remove(0, 2);
                 data.Insert(0, "if");
             }
 
             if(data.Count > 0 && data[0].StartsWith("print") && data[0].Length > 5){
                 data[0].Remove(0, 2);
                 data.Insert(0, "print");
             }

            //if we are parsing an instruction then increase instruction counter
            if(isInstruction && data.Count > 0){
                //add instruction to IInstruction list
                //TODO: this switch only checks for if and print. we need to handle to suffix of these functions somehow
            
                switch(data[0]){
                    case "exit":
                        Exit ex = new Exit(Int32.Parse(data[1]));
                        instructions.Add(ex);
                        break;

                    case "swap":
                        Swap sw;
                        if(data.Count > 2){
                            sw = new Swap(Int32.Parse(data[1]), Int32.Parse(data[2]));
                        }else{
                            sw = new Swap();
                        }
                        instructions.Add(sw);
                        break;
                    
                    case "nop":
                        Nop n = new Nop();
                        instructions.Add(n);
                        break;
                    case "input":
                        Input inp = new Input();
                        instructions.Add(inp);
                        break;

                    case "stinput":
                        if(data.Count > 1){
                            Stinput sin = new Stinput(Int32.Parse(data[1]));
                            instructions.Add(sin);
                        }else{
                            Stinput sin = new Stinput();
                            instructions.Add(sin);
                        }
                        break;
                    
                    case "debug":
                        int val = 0;
                        if(data.Count > 1){
                            string str = data[1];
                            try{
                                val = str.StartsWith("0x") ? Convert.ToInt32(str, 16) : Int32.Parse(str);
                            }
                            catch(Exception e){
                                Console.WriteLine("Error parsing debug value: " + e.Message);
                                return;
                            }
                        }
                        Debug d = new Debug(val);
                        instructions.Add(d);
                        break;
                    
                    case "pop":
                        Pop pop;

                        if(data.Count > 1){
                            pop = new Pop(UInt32.Parse(data[1]));
                            instructions.Add(pop);
                        }else{
                            pop = new Pop();
                            instructions.Add(pop);
                        }
                        break;
                    
                    case "add":
                        Add add = new Add();
                        instructions.Add(add);
                        break;
                    
                    case "sub":
                        Sub sub = new Sub();
                        instructions.Add(sub);
                        break;
                    
                    case "mul":
                        Mul mul = new Mul();
                        instructions.Add(mul);
                        break;

                    case "div":
                        Div div = new Div();
                        instructions.Add(div);
                        break;
                    
                    case "rem":
                        Rem rem = new Rem();
                        instructions.Add(rem);
                        break;
                    
                    case "and":
                        And and = new And();
                        instructions.Add(and);
                        break;
                    
                    case "or":
                        Or or = new Or();
                        instructions.Add(or);
                        break;
                    
                    case "xor":
                        Xor xor = new Xor();
                        instructions.Add(xor);
                        break;
                    
                    case "lsl":
                        Lsl lsl = new Lsl();
                        instructions.Add(lsl);
                        break;
                    
                    case "lsr":
                        Lsr lsr = new Lsr();
                        instructions.Add(lsr);
                        break;
                    
                    case "asr":
                        Asr asr = new Asr();
                        instructions.Add(asr);
                        break;
                    
                    case "neg":
                        Neg neg = new Neg();
                        instructions.Add(neg);
                        break;
                    
                    case "not":
                        Not not = new Not();
                        instructions.Add(not);
                        break;
                    
                    case "stprint":
                        Stprint stp;

                        if(data.Count > 1){
                            stp = new Stprint(Int32.Parse(data[1]));
                            instructions.Add(stp);
                        }else{
                            stp = new Stprint(0);
                            instructions.Add(stp);
                        }
                        break;
                    
                    //def someone check this
                    case "call":
                        Call c = new Call(labels[data[1]], (int)numInstructions * 4);
                        instructions.Add(c);    
                        break;
                    
                    case "return":
                        Return r;

                        if(data.Count > 1){
                            r = new Return(Int32.Parse(data[1]));
                            instructions.Add(r);
                        }else{
                            r = new Return(0);
                            instructions.Add(r);
                        }
                        break;
                    
                    case "goto":
                        Goto go = new Goto(labels[data[1]], (int)numInstructions * 4);
                        instructions.Add(go);
                        break;
                    
                    case "if":
                        If cond = new If(data[1], labels[data[2]], (int)numInstructions * 4);
                        instructions.Add(cond);
                        break;
                    
                    case "dup":
                        Dup dup;
                        if(data.Count > 1){
                            dup = new Dup(Int32.Parse(data[1]));
                            instructions.Add(dup);
                        }else{
                            dup = new Dup(0);
                            instructions.Add(dup);
                        }
                        break;
                    
                    case "print":
                    //Have to handle print suffix
                        Console.WriteLine("You called: " + data[0]);
                        break;
                    
                    case "dump":
                        Dump dmp = new Dump();
                        instructions.Add(dmp);
                        break;  
                    
                    case "push":
                        Console.WriteLine("You called: " + data[0]);
                        break;

                    case "stpush":
                        Console.WriteLine("You called: " + data[0]);
                      
                        /* Parse the string. */

                        /* Comments. */ 
                        comment_index = line.IndexOf("#");
                        if (comment_index != -1) {
                            tmp = line.Substring(0, comment_index);
                            //Console.WriteLine($"DEBUG: {tmp}");
                        }

                        /* Remove the quotes. */
                        str_start = line.IndexOf("\"");
                        str_end = line.LastIndexOf("\"");

                        if (str_start == -1 || str_start == str_end) {
                            /* TODO: Get the actual error for pushing a string with no quotes. */
                            Console.WriteLine("Bad stpush.");
                            return;
                        }

                        tmp = line.Substring(str_start + 1, (str_end - str_start - 1));
                        push_str = replace_escapes(tmp);

                        /* Build our substring list. */
                        push_sub_strs.Clear();
                        push_ints.Clear();

                        for (j = 0; j < push_str.Length; j += 3) {
                            if ((push_str.Length - j) >= 3) push_sub_strs.Add(push_str.Substring(j, 3));
                            else push_sub_strs.Add(push_str.Substring(j));
                        }

                        /* Convert the substrings to ints. */
                        for (j = 0; j < push_sub_strs.Count; j++) {
                            if (j == push_sub_strs.Count - 1) push_ints.Add( substr_to_int(push_sub_strs[j], true) );
                            else push_ints.Add( substr_to_int(push_sub_strs[j], false) );
                        }

                        /* TODO: Debug, remove this. */
                        Console.WriteLine($"The string being pushed: {push_str}");

                        /* Create the push instructions. */

                        for (j = push_ints.Count - 1; j != 0; j--) {
                            /* TODO: this. */
                        }

                        break;

                    default:
                        Console.WriteLine("Invalid Instruction: " + data[0]);
                        break;
                }
                numInstructions++;
            }
            //get string length to calc # of pushes needed for PC and IInstruction list   
            lines++;     
        }
        

        using (var stream = File.Open(args[1], FileMode.Create))
        {
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(0xEFBEADDE);
                foreach(var i in instructions){
                    writer.Write(i.Encode());
                }

                if(instructions.Count % 4 != 0){
                    int nop = (0b0 << 31) & (0b1 << 25);
                    for(int i = 0; i < 4 - (instructions.Count % 4); i++){
                        writer.Write(nop);
                    }
                }
            }
        }
    }

    /* Convert a substring into an int value for stpush. */
    public static int substr_to_int(string s, bool done) {
        int n;
        int last, len;

        len = s.Length;

        if (len > 3 || len == 0) throw new ArgumentOutOfRangeException("substr_to_int failed: s wasn't the correct size.");

        n = 0;
        last = (done) ? 0 : 1;
        n |= (int) s[0];
        if (len > 1) n |= ((int) s[1] << 8);
        if (len > 2) n |= ((int) s[2] << 16);
        n |= (last << 24);

        return n;
    }

    /* Converts escapes into their actual characters. */ 
    public static string replace_escapes(string s) {
        string esc;
        int i;

        esc = "";

        for (i = 0; i < s.Length; i++) {
            if (s[i] == '\\' && i + 1 < s.Length) {
                switch (s[i + 1]) {
                    case 'n':
                        esc += '\n';
                        break;
                    case '"':
                        esc += '\"';
                        break;
                    case '\\':
                        esc += '\\';
                        break;
                }
                i++;
            }
            else {
                esc += s[i];
            }
        }

        return esc;
    }
}


