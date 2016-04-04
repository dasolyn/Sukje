using System;

namespace BinaryTree {
    class Program {
        static void Main(string[] args) {
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

            // 바로 종료 방지
            Console.ReadLine();
        }
    }
}
