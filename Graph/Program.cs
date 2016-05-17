#define Q2

using BinaryTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Math;

namespace Graph {
    class Program {
        static void Main(string[] args) {
#if Q1
            MyGraph<int> graph = null;
            IEnumerable<string> file = File.ReadLines("adjmatrix.txt");
            foreach (string i in file.Take(1)) {
                graph = new MyGraph<int>(int.Parse(i));
                for (int j = 0; j < graph.Size; j++) graph[j] = j + 1;
            }
            int count = 0;
            foreach (string i in file.Skip(1)) {
                IList<int> splited = i.Split(' ').Select(s => int.Parse(s)).ToList();
                for (int j = 0; j < splited.Count; j++) {
                    try {
                        if (splited[j] == 1) graph.MakeNewEdge(count, j);
                    } catch (ArgumentException) {
                        continue;
                    }
                }
                count++;
            }
            for (int i = 0; i < graph.Size; i++) {
                Console.Write($"{graph[i]}: ");
                for (int j = 0; j < graph.Size; j++) if (graph.CheckEdge(i, j)) Console.Write($"{graph[j]} ");
                Console.WriteLine();
            }
#endif
#if Q2
            int index = 0;
            RedBlackTree<Data> rbtree = new RedBlackTree<Data>();
            Console.WriteLine("Inserting elements to red-black tree... Please wait a minute.");
            Stopwatch watch = Stopwatch.StartNew();
            foreach (string i in File.ReadLines("Alabama AL Distances.TXT").Skip(1)) {
                List<string> splited = i.Split('\t').ToList();
                // 번호 추가
                string placename = splited[1];
                int avoidduplicate = 1;
                while (true) {
                    try {
                        rbtree.Search(new Data { PlaceName = placename });
                        // 중복 있음
                        placename = $"{splited[1]}{avoidduplicate}";
                        avoidduplicate++;
                    } catch (ArgumentException) {
                        rbtree.Insert(new Data { Index = index, PlaceName = placename, Longitude = double.Parse(splited[2]), Latitude = double.Parse(splited[3]) });
                        index++;
                        break;
                    }
                }
            }
            watch.Stop();
            int TotalNumberOfElements = rbtree.Count();
            Console.WriteLine($"Finished! {TotalNumberOfElements} elements, {watch.ElapsedMilliseconds / 1000d:0.##} seconds");
            Console.WriteLine("Building graph...");
            watch.Restart();
            MyGraph<Data> graph = new MyGraph<Data>(TotalNumberOfElements);
            {
                int i = 0;
                foreach (Node<Data> j in rbtree) graph[i++] = j.Data;
            }
            object MakeEdgeLock = new object();
            for (int i = 0; i < graph.Size; i++) {
                Parallel.For(i + 1, graph.Size, j => {
                    double dist = CalDistance(graph[i].Latitude, graph[i].Longitude, graph[j].Latitude, graph[j].Longitude);
                    if (dist < 10 * 1000) lock (MakeEdgeLock) graph.MakeEdge(i, j);
                });
            }
            watch.Stop();
            Console.WriteLine($"Finished! {watch.ElapsedMilliseconds / 1000d:0.##} seconds");
            while (true) {
                try {
                    Console.Write("Input city name: ");
                    string city = Console.ReadLine();
                    if (city.ToLower() == "exit") break;
                    Data found = rbtree.SearchData(new Data { PlaceName = city });
                    Console.Write("Input hop number: ");
                    int hop = int.Parse(Console.ReadLine());
                    foreach (Data i in graph.AsBFSTraversalEnumerable(found.Index, d => d == hop)) Console.Write($"{i.PlaceName}, ");
                    Console.WriteLine();
                } catch (ArgumentException) {
                    Console.WriteLine("Failed to find given city name.");
                    continue;
                } catch (FormatException) {
                    continue;
                }
            }
#endif
#if Q3
            Stopwatch watch = new Stopwatch();
            RedBlackTree<Data> rbtree = null;
            Console.WriteLine("Inserting elements to red-black tree... Please wait a minute.");
            {
                watch.Restart();
                rbtree = new RedBlackTree<Data>();
                int index = 0;
                foreach (string i in File.ReadLines("Alabama AL Distances.TXT").Skip(1)) {
                    List<string> splited = i.Split('\t').ToList();
                    // 번호 추가
                    string placename = splited[1];
                    int avoidduplicate = 1;
                    while (true) {
                        try {
                            rbtree.Search(new Data { PlaceName = placename });
                            // 중복 있음
                            placename = $"{splited[1]}{avoidduplicate}";
                            avoidduplicate++;
                        } catch (ArgumentException) {
                            rbtree.Insert(new Data { Index = index, PlaceName = placename, Longitude = double.Parse(splited[2]), Latitude = double.Parse(splited[3]) });
                            index++;
                            break;
                        }
                    }
                }
                watch.Stop();
            }
            int TotalNumberOfElements = rbtree.Count();
            Console.WriteLine($"Finished! {TotalNumberOfElements} elements, {watch.ElapsedMilliseconds / 1000d:0.##} seconds");
            Console.WriteLine("Building graph...");
            MyGraph<Data> graph = null;
            {
                watch.Restart();
                graph = new MyGraph<Data>(TotalNumberOfElements);
                {
                    int i = 0;
                    foreach (Node<Data> j in rbtree) graph[i++] = j.Data;
                }
                object MakeEdgeLock = new object();
                for (int i = 0; i < graph.Size; i++) {
                    Parallel.For(i + 1, graph.Size, j => {
                        double dist = CalDistance(graph[i].Latitude, graph[i].Longitude, graph[j].Latitude, graph[j].Longitude);
                        if (dist < 10 * 1000 && graph[i].Latitude < graph[j].Latitude && graph[i].Longitude < graph[j].Longitude)
                            lock (MakeEdgeLock) graph.MakeDirectedEdge(i, j);
                    });
                }
                watch.Stop();
            }
            Console.WriteLine($"Finished! {watch.ElapsedMilliseconds / 1000d:0.##} seconds");
            Console.Write($"Topological Ordering (10 elements per press enter) : ");
            Console.ReadLine();
            {
                int index = 0;
                foreach (Data i in graph.AsTopologyOrderEnumerable()) {
                    Console.WriteLine(i.PlaceName);
                    if ((index + 1) % 10 == 0) Console.ReadLine();
                    index++; 
                }
            }
#endif
#if Q4
            Stopwatch watch = new Stopwatch();
            RedBlackTree<Data> rbtree = null;
            Console.WriteLine("Inserting elements to red-black tree... Please wait a minute.");
            {
                watch.Restart();
                rbtree = new RedBlackTree<Data>();
                int index = 0;
                foreach (string i in File.ReadLines("Alabama AL Distances.TXT").Skip(1)) {
                    List<string> splited = i.Split('\t').ToList();
                    // 번호 추가
                    string placename = splited[1];
                    int avoidduplicate = 1;
                    while (true) {
                        try {
                            rbtree.Search(new Data { PlaceName = placename });
                            // 중복 있음
                            placename = $"{splited[1]}{avoidduplicate}";
                            avoidduplicate++;
                        } catch (ArgumentException) {
                            rbtree.Insert(new Data { Index = index, PlaceName = placename, Longitude = double.Parse(splited[2]), Latitude = double.Parse(splited[3]) });
                            index++;
                            break;
                        }
                    }
                }
                watch.Stop();
            }
            int TotalNumberOfElements = rbtree.Count();
            Console.WriteLine($"Finished! {TotalNumberOfElements} elements, {watch.ElapsedMilliseconds / 1000d:0.##} seconds");
            Console.WriteLine("Building graph...");
            MyGraph<Data> graph = null;
            {
                watch.Restart();
                graph = new MyGraph<Data>(TotalNumberOfElements);
                {
                    int i = 0;
                    foreach (Node<Data> j in rbtree) graph[i++] = j.Data;
                }
                object MakeEdgeLock = new object();
                for (int i = 0; i < graph.Size; i++) {
                    Parallel.For(i + 1, graph.Size, j => {
                        double dist = CalDistance(graph[i].Latitude, graph[i].Longitude, graph[j].Latitude, graph[j].Longitude);
                        if (dist < 15 * 1000) lock (MakeEdgeLock) graph.MakeEdge(i, j);
                    });
                }
                watch.Stop();
            }
            Console.WriteLine($"Finished! {watch.ElapsedMilliseconds / 1000d:0.##} seconds");
            Console.Write($"Kruskal MST Edge (10 edges per press enter) : ");
            Console.ReadLine();
            {
                int index = 0;
                foreach (var i in graph.AsKruskalMSTEdgeEnumerable()) {
                    Console.WriteLine($"{graph[i.Item1].PlaceName} - {graph[i.Item2].PlaceName}");
                    if ((index + 1) % 10 == 0) Console.ReadLine();
                    index++;
                }
            }
#endif
#if Q5
            List<Point> list = new List<Point>();
            foreach (string i in File.ReadLines("input_prim.txt").Skip(1)) {
                IEnumerable<int> p = i.Split(' ').Select(s => int.Parse(s));
                int x = p.FirstOrDefault();
                int y = p.Skip(1).FirstOrDefault();
                list.Add(new Point { x = x, y = y });
            }
            MyGraph<Point> graph = new MyGraph<Point>(list.Count);
            for (int i = 0; i < list.Count; i++) {
                graph[i] = list[i];
            }
            for (int i = 0; i < list.Count; i++) {
                for (int j = i + 1; j < list.Count; j++) {
                    double distance = Sqrt(Pow(list[i].x - list[j].x, 2) + Pow(list[i].y - list[j].y, 2));
                    graph.MakeEdge(i, j, distance);
                }
            }
            Console.Write("Prim MST Edge (10 edges per press enter) : ");
            Console.ReadLine();
            {
                int index = 0;
                foreach (var i in graph.AsPrimMSTEdgeEnumerable()) {
                    Console.WriteLine($"{i.Item1}({list[i.Item1].x:#.##}, {list[i.Item1].y:#.##}) - {i.Item2}({list[i.Item2].x:#.##}, {list[i.Item2].y:#.##})");
                    if ((index + 1) % 10 == 0) Console.ReadLine();
                    index++;
                }
            }
#endif
            // 바로 종료 방지
            if (Debugger.IsAttached) Debugger.Break();
        }
#if Q2 || Q3 || Q4
        private class Data : IComparable<Data> {
            public int Index;
            public string PlaceName;
            public double Longitude;
            public double Latitude;
            public int CompareTo(Data other) {
                return PlaceName.CompareTo(other.PlaceName);
            }
        }
        private static double CalDistance(double lat1, double lon1, double lat2, double lon2) {
            double theta = lon1 - lon2;
            double dist = Sin(Deg2Rad(lat1)) * Sin(Deg2Rad(lat2)) + Cos(Deg2Rad(lat1)) * Cos(Deg2Rad(lat2)) * Cos(Deg2Rad(theta));
            return Rad2Deg(Acos(dist)) * 60.0 * 1.1515 * 1.609344 * 1000.0;
        }
        private static double Deg2Rad(double deg) => deg * PI / 180.0;
        private static double Rad2Deg(double rad) => rad * 180.0 / PI;
#endif
#if Q5
        private class Point {
            public int x;
            public int y;
        }
#endif
    }
}
