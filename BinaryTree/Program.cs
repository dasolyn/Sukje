#define Day2

using System;
using System.Collections.Generic;

namespace BinaryTree {
    class Program {
        static void Main(string[] args) {
#if Day1
            // 초기화
            BinaryTree<string> bt = new BinaryTree<string>();
            bt.SetNodeByTraversal(new Node<string>("fox"));
            bt.SetNodeByTraversal(new Node<string>("bear"), NodeTraversal.Left);
            bt.SetNodeByTraversal(new Node<string>("goose"), NodeTraversal.Right);
            bt.SetNodeByTraversal(new Node<string>("ant"), NodeTraversal.Left, NodeTraversal.Left);
            bt.SetNodeByTraversal(new Node<string>("dog"), NodeTraversal.Left, NodeTraversal.Right);
            bt.SetNodeByTraversal(new Node<string>("cat"), NodeTraversal.Left, NodeTraversal.Right, NodeTraversal.Left);
            bt.SetNodeByTraversal(new Node<string>("eagle"), NodeTraversal.Left, NodeTraversal.Right, NodeTraversal.Right);
            bt.SetNodeByTraversal(new Node<string>("hippo"), NodeTraversal.Right, NodeTraversal.Right);
            bt.SetNodeByTraversal(new Node<string>("iguana"), NodeTraversal.Right, NodeTraversal.Right, NodeTraversal.Right);

            // 출력
            Console.Write("Inorder: ");
            foreach (var i in bt.AsInorderedEnumerable()) Console.Write($"{i.Data} ");
            Console.WriteLine();
            Console.Write("Preorder: ");
            foreach (var i in bt.AsPreorderedEnumerable()) Console.Write($"{i.Data} ");
            Console.WriteLine();
            Console.Write("Postorder: ");
            foreach (var i in bt.AsPostorderedEnumerable()) Console.Write($"{i.Data} ");
            Console.WriteLine();
            Console.Write("Levelorder: ");
            foreach (var i in bt.AsLevelOrderedEnumerable()) Console.Write($"{i.Data} ");
            Console.WriteLine();

#endif
#if Day2
            BinaryTree<AddressBook> bt = new BinaryTree<AddressBook>();
            while (true) {
                Console.Write("$ ");
                string[] cmd = Console.ReadLine().Split(' ');
                try {
                    if (cmd[0].ToLower() == "read") {
                        try {
                            foreach (string s in System.IO.File.ReadLines(cmd[1])) {
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
#endif
            // 바로 종료 방지
            Console.ReadLine();
        }
    }
}
