#define Day2

using System;
using System.Collections.Generic;
using System.Linq;

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
            foreach (var i in bt.AsLevelorderedEnumerable()) Console.Write($"{i.Data} ");
            Console.WriteLine();

#endif
#if Day2
            //BinarySearchTree<AddressBook> tree = new BinarySearchTree<AddressBook>();
            RedBlackTree<AddressBook> tree = new RedBlackTree<AddressBook>();
            while (true) {
                Console.Write("$ ");
                string cmd = Console.ReadLine();
                string[] splitcmd = cmd.Split(' ');
                try {
                    if (splitcmd[0].ToLower() == "read") {
                        try {
                            foreach (string s in System.IO.File.ReadLines(splitcmd[1])) {
                                List<string> splited = s.Split('|').Select(l => l.Trim()).ToList();
                                try {
                                    tree.Insert(new AddressBook {
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
                        } catch (System.IO.FileNotFoundException) {
                            Console.WriteLine("Specified file does not exist");
                        }
                    } else if (splitcmd[0].ToLower() == "list") {
                        foreach (var i in tree.AsInorderedEnumerable()) {
                            Console.WriteLine(i.Data.Name);
                            Console.WriteLine($"Company: {i.Data.Company}");
                            Console.WriteLine($"Address: {i.Data.Address}");
                            Console.WriteLine($"Zipcode: {i.Data.Zipcode}");
                            Console.WriteLine($"Phones: {i.Data.Phone1}, {i.Data.Phone2}");
                            Console.WriteLine($"Email: {i.Data.Email}");
                            Console.WriteLine($"Web: {i.Data.Web}");
                            Console.WriteLine();
                        }
                    } else if (splitcmd[0].ToLower() == "delete") {
                        try {
                            tree.Delete(new AddressBook { Name = cmd.Replace("delete ", "") });
                        } catch (ArgumentException) {
                            Console.WriteLine("Failed to find node");
                        }
                    } else if (splitcmd[0].ToLower() == "find") {
                        try {
                            AddressBook data = tree.SearchData(new AddressBook { Name = cmd.Replace("find ", "") });
                            Console.WriteLine(data.Name);
                            Console.WriteLine($"Company: {data.Company}");
                            Console.WriteLine($"Address: {data.Address}");
                            Console.WriteLine($"Zipcode: {data.Zipcode}");
                            Console.WriteLine($"Phones: {data.Phone1}, {data.Phone2}");
                            Console.WriteLine($"Email: {data.Email}");
                            Console.WriteLine($"Web: {data.Web}");
                            Console.WriteLine();
                        } catch (ArgumentException) {
                            Console.WriteLine("Failed to find node");
                        }
                    } else if (splitcmd[0].ToLower() == "exit") {
                        break;
                    } else {
                        Console.WriteLine("Commands: read (filename), list, find (name), delete (name), exit");
                    }
                } catch (IndexOutOfRangeException) {
                    Console.WriteLine("Commands: read (filename), list, find (name), delete (name), exit");
                }
            }
#endif
            // 바로 종료 방지
            Console.ReadLine();
        }
    }
}
