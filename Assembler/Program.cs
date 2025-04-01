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
            List<string> data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            for(int i = 0; i < data.Count; i++){
                if(data[i].Contains('#')){
                    data.RemoveRange(i, data.Count - i);
                    break;
                }
            }

            foreach(string s in data){
                Console.Write(s + " ");
            }
            Console.Write('\n');
        }
    }
}

