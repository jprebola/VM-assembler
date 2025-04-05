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
        uint labelPos = 0;
        uint numInstructions = 0;
        Dictionary<string, int> labels = new Dictionary<string, int>();

        push_sub_strs = new List<string>();
        push_ints = new List<int>();

        //first pass
        while((line = sr.ReadLine()) != null){
            List<string> data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            if(data.Count == 1 && data[0].Contains(':')){
                string labelName = data[0].Substring(0, data[0].Length - 1);
                labels[labelName] = (int)labelPos * 4;
            }else if (data.Count > 0 && !data[0].StartsWith("#")){
                labelPos++;
            }
        }

        // Reset stream for second pass
        sr.BaseStream.Seek(0, SeekOrigin.Begin);
        sr.DiscardBufferedData();

        //second pass
        while((line = sr.ReadLine()) != null){
            List<string> data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            //bool isInstruction = true;
            
            
            //check for label
            if(data.Count == 1 && data[0].Contains(':')){
                continue;
            }

            //Check for comment
            for(int i = 0; i < data.Count; i++){
                if(data[i].Contains('#') && data[i].IndexOf('#') == 0){
                    data.RemoveRange(i, data.Count - i);
                    break;
                }
            }

            //handle instruction letter cases
            if(data.Count > 0){
                data[0] = data[0].ToLower();
            }
            

            if(data.Count > 0 && data[0].StartsWith("if")){
                try{
                    string getIf = data[0].Substring(0, 2);
                    string suf = data[0].Substring(2, 2);
                    data.RemoveAt(0);
                    data.Insert(0, suf);
                    data.Insert(0, getIf);
                }catch (Exception e){
                    Console.WriteLine(e);
                }
            }
 
             if(data.Count > 0 && data[0].StartsWith("print")){
                string printSuffix;
                if(data[0].Contains('h')){
                    printSuffix = "h";
                }else if(data[0].Contains('o')){
                    printSuffix = "o";
                }else if(data[0].Contains('b')){
                    printSuffix = "b";
                }else{
                    printSuffix = "d";
                }

                 data.RemoveAt(0);
                 data.Insert(0, printSuffix);
                 data.Insert(0, "print");
             }

            //if we are parsing an instruction then increase instruction counter
            if(data.Count > 0){
                //add instruction to IInstruction list
                //TODO: this switch only checks for if and print. we need to handle to suffix of these functions somehow
            
                switch(data[0]){
                    case "exit":
                        int check = Int32.Parse(data[1]);
                        if(check > 255) Console.WriteLine($"{lines}: exit code would be truncated");
                        Exit ex = new Exit(check);
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
                            int offset;
                            if(data[1].StartsWith("0x") || data[1].StartsWith("0X")){
                                //parse as hex
                                offset = Convert.ToInt32(data[1].Substring(2), 16);
                            } else {
                                //parse as decimal
                                offset = Int32.Parse(data[1]);
                            }
                            
                            if(offset < 0){
                                Console.WriteLine($"{lines}: offset to pop() is negative.");
                                return;
                            }
                            pop = new Pop((uint)offset);
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
                        //Console.WriteLine($"labels[label] = {labels[data[1]]}" );
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
                            int offset;
                            if(data[1].StartsWith("0x") || data[1].StartsWith("0X")){
                                //parse as hex
                                offset = Convert.ToInt32(data[1].Substring(2), 16);
                                dup = new Dup(offset);
                                instructions.Add(dup);
                            } else {
                                //parse as decimal
                                offset = Int32.Parse(data[1]);
                                dup = new Dup(offset);
                                instructions.Add(dup);
                            }
                        }else{
                            dup = new Dup(0);
                            instructions.Add(dup);
                        }
                        break;
                    
                    case "print":
                    //Have to handle print suffix
                        Print pr;

                        if(data.Count > 2){
                            int offset;

                            if (data[2].StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                            {
                                // Parse as hexadecimal (skip the "0x")
                                offset = Convert.ToInt32(data[2].Substring(2), 16);
                                pr = new Print(offset, (char) data[1].ElementAt(0));
                                instructions.Add(pr);
                            }else {
                                // Parse as decimal
                                offset = Convert.ToInt32(data[2], 10);
                                pr = new Print(offset, (char) data[1].ElementAt(0));
                                instructions.Add(pr);
                            }
                        }else{
                            pr = new Print(0, (char) data[1].ElementAt(0));
                            instructions.Add(pr);
                        }
                        break;
                    
                    case "dump":
                        Dump dmp = new Dump();
                        instructions.Add(dmp);
                        break;  
                    
                    case "push":
                        Push push;

                        if(data.Count > 1){
                            if(data[1].StartsWith("0x") || data[1].StartsWith("0X")){
                                //parse hex
                                push = new Push(Convert.ToInt32(data[1].Substring(2), 16));
                            } else {
                                //parse decimal
                                push = new Push(Int32.Parse(data[1]));
                            }
                            instructions.Add(push);
                        }else{
                            push = new Push(0);
                            instructions.Add(push);
                        }
                        break;

                    case "stpush":
                      
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

                        for (j = push_ints.Count - 1; j >= 0; j--) {
                            /* TODO: this. */

                            /* Debug. */ 
                            //Console.WriteLine($"{push_sub_strs[j]} // {push_ints[j]}");
                            Push stPush = new Push(push_ints[j]);
                            instructions.Add(stPush);
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

        //pad to 4 instructions
        if(instructions.Count % 4 != 0){
            int count = 4 - (instructions.Count % 4);
            for(int i = 0; i < count; i++){
                Nop padding = new Nop();
                instructions.Add(padding);
            }
        }
        
        //write to the file
        using (var stream = File.Open(args[1], FileMode.Create))
        {
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(0xEFBEADDE);
                foreach(var i in instructions){
                    writer.Write(i.Encode());
                }

                // always pad to next multiple of 4 instructions
                /*
                int nop = (0b0000 << 28) | (0b0010 << 24);
                for(int i = 0; i < 4 - (instructions.Count % 4); i++){
                    writer.Write(nop);
                }*/
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


