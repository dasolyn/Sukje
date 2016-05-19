using Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShortestPath {
    class Program {
        static void Main(string[] args) {
            IEnumerable<string> file = File.ReadLines("road_segments.txt").Where(s => s.Trim().Length != 0 && s[0] != '#');
            // 도시 로딩
            int count = int.Parse(file.First());
            List<string> cities = file.Skip(1).Take(count).ToList();
            MyGraph<int> graph = new MyGraph<int>();
            for (int i = 0; i < cities.Count; i++) graph[i] = i;
            Console.WriteLine($"도시 로딩이 완료되었습니다. {cities.Count}개의 도시가 있습니다.");
            // 도로 로딩
            file = file.Skip(1 + count);
            count = int.Parse(file.First());
            foreach (string i in file.Skip(1).Take(count)) {
                string[] line = i.Split(' ');
                if (line.Length == 3) graph.MakeEdge(int.Parse(line[0]), int.Parse(line[1]), double.Parse(line[2]));
            }
            Console.WriteLine($"도로 로딩이 완료되었습니다. {graph.EdgeCount}개의 도로가 있습니다.");
            while (true) {
                Console.WriteLine();
                Console.Write($"시작점의 이름 또는 번호를 입력하세요. ");
                string input = Console.ReadLine();
                int from;
                if (!int.TryParse(input, out from)) from = cities.FindIndex(s => s == input);
                if (from < 0 || from >= cities.Count) {
                    Console.WriteLine("그런 도시가 없습니다. 다시 입력하세요");
                    continue;
                }
                Console.WriteLine($"시작점: {from} ({cities[from]})");
                Console.Write("도착점의 이름 또는 번호를 입력하세요. ");
                input = Console.ReadLine();
                int to;
                if (!int.TryParse(input, out to)) to = cities.FindIndex(s => s == input);
                if (to < 0 || to >= cities.Count) {
                    Console.WriteLine("그런 도시가 없습니다. 다시 입력하세요");
                    continue;
                }
                Console.WriteLine($"도착점: {to} ({cities[to]})");
                var res = graph.DijkstraShortestPath(from, to);
                if (res.Route != null) {
                    Console.WriteLine();
                    Console.WriteLine($"최단 경로의 거리는 {res.TotalDistance:0.##}입니다.");
                    Console.WriteLine("이하 경로를 출력합니다: ");
                    int prev = res.Route.First();
                    foreach (int i in res.Route.Skip(1)) {
                        Console.WriteLine($"{prev} ({cities[prev]}) -> {i} ({cities[i]}) 도로 길이는 {graph.GetWeightOfEdge(prev, i):0.##}");
                        prev = i;
                    }
                } else {
                    Console.WriteLine("최단 거리를 계산할 수 없습니다. (경로가 존재하지 않음)");
                }
            }
        }
    }
}
