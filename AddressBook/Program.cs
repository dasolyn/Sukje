using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AddressBook {
    class Program {
        static void Main(string[] args) {
            IEnumerable<string> data1 = null;
            List<AddressBook> data2 = new List<AddressBook>();
            while (true) {
                Console.Write("$ ");
                string[] cmd = Console.ReadLine().Split(' ');
                try {
                    if (cmd[0].ToLower() == "read") {
                        try {
                            data1 = File.ReadLines(cmd[1]);
                            data2.Clear();
                            foreach (string s in data1) {
                                List<string> splited = s.Split('|').Select(l => l.Trim()).ToList();
                                try {
                                    data2.Add(new AddressBook {
                                        Name = splited[0],
                                        Company = splited[1],
                                        Address = splited[2],
                                        Zipcode = splited[3],
                                        Phone1 = splited[4],
                                        Phone2 = splited[5],
                                        Email = splited[6],
                                        Web = splited[7]
                                    });
                                } catch (ArgumentOutOfRangeException) { }
                            }
                            data2.TrimExcess();
                        } catch (FileNotFoundException) {
                            Console.WriteLine("Specified file does not exist");
                        }
                    } else if (cmd[0].ToLower() == "sort") {
                        data2.Sort((a, b) => {
                            IComparable va = (IComparable)a.GetType().GetProperty(cmd[1]).GetValue(a);
                            IComparable vb = (IComparable)b.GetType().GetProperty(cmd[1]).GetValue(b);
                            return va.CompareTo(vb);
                        });
                    } else if (cmd[0].ToLower() == "print") {
                        foreach (AddressBook i in data2) {
                            Console.WriteLine(i.Name);
                            Console.WriteLine($"Company: {i.Company}");
                            Console.WriteLine($"Address: {i.Address}");
                            Console.WriteLine($"Zipcode: {i.Zipcode}");
                            Console.WriteLine($"Phones: {i.Phone1}, {i.Phone2}");
                            Console.WriteLine($"Email: {i.Email}");
                            Console.WriteLine($"Web: {i.Web}");
                            Console.WriteLine();
                        }
                    } else if (cmd[0].ToLower() == "exit") {
                        break;
                    } else {
                        Console.WriteLine("Commands: read (filename), print, sort (fieldname), exit");
                    }
                } catch (IndexOutOfRangeException) {
                    Console.WriteLine("Commands: read (filename), print, sort (fieldname), exit");
                }
            }
        }
    }
}
