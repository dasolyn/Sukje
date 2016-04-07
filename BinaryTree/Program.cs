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
            foreach (var i in bt.AsLevelOrderedEnumerable()) Console.Write($"{i.Data} ");
            Console.WriteLine();

#endif
#if Day2
            BinarySearchTree<AddressBook> bst = new BinarySearchTree<AddressBook>();
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
                                    bst.InsertNodeToBST(new Node<AddressBook>(new AddressBook {
                                        Name = splited[0],
                                        Company = splited[1],
                                        Address = splited[2],
                                        Zipcode = splited[3],
                                        Phone1 = splited[4],
                                        Phone2 = splited[5],
                                        Email = splited[6],
                                        Web = splited[7]
                                    }));
                                } catch (ArgumentOutOfRangeException) { }
                            }
                        } catch (System.IO.FileNotFoundException) {
                            Console.WriteLine("Specified file does not exist");
                        }
                    } else if (splitcmd[0].ToLower() == "list") {
                        foreach (var i in bst.AsInorderedEnumerable()) {
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
                            bst.DeleteNodeFromBST(new AddressBook { Name = cmd.Replace("delete ", "") });
                        } catch (ArgumentException) {
                            Console.WriteLine("Failed to find node");
                        }
                    } else if (splitcmd[0].ToLower() == "find") {
                        try {
                            Node<AddressBook> node = bst.SearchNodeFromBST(new AddressBook { Name = cmd.Replace("find ", "") });
                            Console.WriteLine(node.Data.Name);
                            Console.WriteLine($"Company: {node.Data.Company}");
                            Console.WriteLine($"Address: {node.Data.Address}");
                            Console.WriteLine($"Zipcode: {node.Data.Zipcode}");
                            Console.WriteLine($"Phones: {node.Data.Phone1}, {node.Data.Phone2}");
                            Console.WriteLine($"Email: {node.Data.Email}");
                            Console.WriteLine($"Web: {node.Data.Web}");
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
                    Console.WriteLine("Commands: read (filename), print, sort (fieldname), exit");
                }
            }
#endif
            // 바로 종료 방지
            Console.ReadLine();
        }
    }
}
