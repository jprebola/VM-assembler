using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

class Assembler{
    public static void Main(string[] args){
        StreamReader sr;
        string? line;
        List<IInstruction> instructions = new List<IInstruction>();

        try{
            sr = new StreamReader(args[0]);
        }
        catch(Exception e){
            Console.WriteLine("Error reading file: " + e.Message);
            return;
        }

        uint lines = 1;
        uint numInstructions = 0;
        Dictionary<string, (int, int)> labels = new Dictionary<string, (int, int)>();

        while((line = sr.ReadLine()) != null){
            List<string> data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            bool isInstruction = true;
            
            //check for label
            if(data.Count == 1 && data[0].Contains(':')){
                //remove ':' from the name and store label info in dictionary
                data[0].Remove(data[0].Length - 1, 1);
                labels[data[0]] = ((int)lines, (int)numInstructions * 4);
                isInstruction = false;
            }

            //Check for comment
            for(int i = 0; i < data.Count; i++){
                if(data[i].Contains('#') && data[i].IndexOf('#') == 0){
                    data.RemoveRange(i, data.Count - i);
                    break;
                }
            }

            if(data.Count > 0 && data[0].StartsWith("if")){
                data[0].Remove(0, 2);
                data.Insert(0, "if");
            }

            if(data.Count > 0 && data[0].StartsWith("print") && data[0].Length > 5){
                data[0].Remove(0, 2);
                data.Insert(0, "print");
            }

            //DONT FUCKING FORGET HANDLING STPUSH W/ numInstructions

            //if we are parsing an instruction then increase instruction counter
            if(isInstruction && data.Count > 0){
                //add instruction to IInstruction list
                if(!data[0].Equals("stpush")){

                    //TODO: this switch only checks for if and print. we need to handle to suffix of these functions somehow
                    switch(data[0]){
                        case "exit":
                            Console.WriteLine("You called: " + data[0]);
                            break;

                        case "swap":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "nop":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        case "input":
                            Console.WriteLine("You called: " + data[0]);
                            break;

                        case "stinput":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "debug":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "pop":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "add":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "sub":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "mul":
                            Console.WriteLine("You called: " + data[0]);
                            break;

                        case "div":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "rem":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "and":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "or":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "xor":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "lsl":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "lsr":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "asr":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "neg":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "not":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "stprint":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "call":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "return":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "goto":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "if":
                            //Have to handle binary and uniary suffix
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "dup":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "print":
                        //Have to handle print suffix
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "dump":
                            Console.WriteLine("You called: " + data[0]);
                            break;
                        
                        case "push":
                            Console.WriteLine("You called: " + data[0]);
                            break;

                        default:
                            Console.WriteLine("Invalid Instruction: " + data[0]);
                            break;
                    }
                    numInstructions++;
                }else{
                //get string length to calc # of pushes needed for PC and IInstruction list

                    Console.WriteLine("STPUSH FOUND");
                }   
            }
            lines++;
        }
    }
}

