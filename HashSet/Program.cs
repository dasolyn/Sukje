using System;

namespace HashSet {
    class Program {
        static void Main(string[] args) {
            MyHashSet<string> myhashset = new MyHashSet<string>();
            while (true) {
                Console.Write("$ ");
                string cmd = Console.ReadLine();
                string[] splitcmd = cmd.Split(' ');
                try {
                    if (splitcmd[0].ToLower() == "add") {
                        string item = cmd.Replace("add ", "");
                        if (item.Length > 0) {
                            if (myhashset.Add(item)) Console.WriteLine("Success");
                            else Console.WriteLine("Failed : There are duplicate conflict");
                        } else {
                            Console.WriteLine("Failed : Input a string to add");
                        }
                    } else if (splitcmd[0].ToLower() == "list") {
                        if (splitcmd.Length == 1) {
                            int count = 0;
                            foreach (string i in myhashset) {
                                Console.Write($"{i} ");
                                if (++count % 10 == 0) Console.WriteLine();
                            }
                            Console.WriteLine();
                        } else {
                            if (splitcmd[1].ToLower() == "detail") {
                                for (int i = 0; i < myhashset.HashTableSize; i++) {
                                    Console.Write($"Hash value {i}: ");
                                    foreach (string j in myhashset.AsEnumerableByHashValue(i)) Console.Write($"{j} ");
                                    Console.WriteLine();
                                }
                            }
                        }
                    } else if (splitcmd[0].ToLower() == "remove") {
                        string item = cmd.Replace("remove ", "");
                        if (item.Length > 0) {
                            if (myhashset.Remove(item)) Console.WriteLine("Success");
                            else Console.WriteLine("Failed : There are no specific element");
                        } else {
                            Console.WriteLine("Failed : Input a string to remove");
                        }
                    } else if (splitcmd[0].ToLower() == "find") {
                        string item = cmd.Replace("find ", "");
                        if (item.Length > 0) {
                            if (myhashset.Contains(item)) Console.WriteLine("Success : Specific element is in HashSet");
                            else Console.WriteLine("Failed : There are no specific element");
                        } else {
                            Console.WriteLine("Failed : Input a string to find");
                        }
                    } else {
                        Console.WriteLine("command: add (item), list, remove (item), find (item)");
                    }
                } catch (ArgumentException) {
                    Console.WriteLine("command: add (item), list, remove, find");
                }
            }
        }
    }
}
