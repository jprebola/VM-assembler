using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

class Assembler{
    public static void Main(string[] args){
        StreamReader sr;
        string? line;

        try{
            sr = new StreamReader(args[0]);
        }
        catch(Exception e){
            Console.WriteLine(e.Message);
            return;
        }

        uint lines = 1;
        uint numInstructions = 0;
        Dictionary<string, (int, int)> labels = new Dictionary<string, (int, int)>();

        while((line = sr.ReadLine()) != null){
            List<string> data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            
            if(data.Count == 1 && data[0].Contains(':')){
                data[0].Remove(data[0].Length - 1, 1);
                labels[data[0]] = ((int)lines, (int)numInstructions * 4);
                numInstructions++;
            }

            for(int i = 0; i < data.Count; i++){
                if(data[i].Contains('#') && data[i].IndexOf('#') == 0){
                    data.RemoveRange(i, data.Count - i);
                    break;
                }
            }
            lines++;
        }

        foreach(var l in labels){
            Console.WriteLine(l.Value);
        }
    }
}

