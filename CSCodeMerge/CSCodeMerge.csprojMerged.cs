using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

namespace CSCodeMerge {
    class CSCodeMerge {
        static List<string> Usings = new List<string>();
        static List<string> Code = new List<string>();
        static void Main(string[] args) {
            string fileName = args[0];
            ReadDirectory(Directory.GetCurrentDirectory());
            Code.RemoveAll(x => Usings.Contains(x));
            Usings.AddRange(Code);
            File.WriteAllLines(fileName, Usings);
        }
        static void ReadDirectory(string directory) {
            Console.Out.WriteLine("Reading " + directory);
            var files = Directory.GetFiles(directory).Where(path => path.EndsWith(".cs")).ToList();
            var code = files.Select(file => File.ReadAllLines(file).ToList()).SelectMany(x => x).ToList();
            Code.AddRange(code);
            foreach (var import in code.Where(line => line.StartsWith("using ")).Distinct()) {
                if (!Usings.Contains(import)) {
                    try {
                        Usings.Add(import);
                        ReadDirectory(@"..\\" + import.Replace("using ", "").Replace(".", @"\\").Replace(";",""));//.Remove(import.IndexOf("//")).Trim());
                    }
                    catch(Exception e) {
                        Console.Error.WriteLine(e.Message);
                        /* Directory not found ('using System') or illegal 
                         * character in path ('using x = System.Console;')
                           Either way, keep going like nothing happened.
                         */
                    }
                }
            }
            
        }
    }
}

namespace CSCodeMerge {
    class CSCodeMerge {
        static List<string> Usings = new List<string>();
        static List<string> Code = new List<string>();
        static void Main(string[] args) {
            string fileName = args[0];
            ReadDirectory(Directory.GetCurrentDirectory());
            Code.RemoveAll(x => Usings.Contains(x));
            Usings.AddRange(Code);
            File.WriteAllLines(fileName, Usings);
        }
        static void ReadDirectory(string directory) {
            Console.Out.WriteLine("Reading " + directory);
            var files = Directory.GetFiles(directory).Where(path => path.EndsWith(".cs")).ToList();
            var code = files.Select(file => File.ReadAllLines(file).ToList()).SelectMany(x => x).ToList();
            Code.AddRange(code);
            foreach (var import in code.Where(line => line.StartsWith("using ")).Distinct()) {
                if (!Usings.Contains(import)) {
                    try {
                        Usings.Add(import);
                        ReadDirectory(@"..\\" + import.Replace("using ", "");//.Remove(import.IndexOf("//")).Trim());
                    }
                    catch {
                        /* Directory not found ('using System') or illegal 
                         * character in path ('using x = System.Console;')
                           Either way, keep going like nothing happened.
                         */
                    }
                }
            }
            
        }
    }
}

namespace CSCodeMerge {
    class CSCodeMerge {
        static List<string> Usings = new List<string>();
        static List<string> Code = new List<string>();
        static void Main(string[] args) {
            string fileName = args[0];
            ReadDirectory(Directory.GetCurrentDirectory());
            Code.RemoveAll(x => Usings.Contains(x));
            Usings.AddRange(Code);
            File.WriteAllLines(fileName, Usings);
        }
        static void ReadDirectory(string directory) {
            Console.Out.WriteLine("Reading " + directory);
            var files = Directory.GetFiles(directory).Where(path => path.EndsWith(".cs")).ToList();
            var code = files.Select(file => File.ReadAllLines(file).ToList()).SelectMany(x => x).ToList();
            Code.AddRange(code);
            foreach (var import in code.Where(line => line.StartsWith("using ")).Distinct()) {
                if (!Usings.Contains(import)) {
                    try {
                        Usings.Add(import);
                        ReadDirectory(@"..\\" + import.Replace("using ", "");//.Remove(import.IndexOf("//")).Trim());
                    }
                    catch {
                        /* Directory not found ('using System') or illegal 
                         * character in path ('using x = System.Console;')
                           Either way, keep going like nothing happened.
                         */
                    }
                }
            }
            
        }
    }
}
