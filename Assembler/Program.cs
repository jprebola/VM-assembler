using System;
using System.IO;
using System.Xml;

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

        while((line = sr.ReadLine()) != null){
            string[] data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach(var s in data){
                Console.WriteLine(s);
            }
        }
    }
}

