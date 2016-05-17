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
            MyGraph<int> graph = new MyGraph<int>(cities.Count);
            for (int i = 0; i < cities.Count; i++) graph[i] = i;
            // 도로 로딩
            file = file.Skip(1 + count);
            count = int.Parse(file.First());
            foreach (string i in file.Skip(1).Take(count)) {
                string[] line = i.Split(' ');
                if (line.Length == 3) graph.MakeEdge(int.Parse(line[0]), int.Parse(line[1]), double.Parse(line[2]));
            }
            while (true) {
                Console.Write("시작점의 이름을 입력하세요. ");
                string input = Console.ReadLine();
                int from;
                if (cities.Contains(input)) {
                    from = cities.FindIndex(s => s == input);
                } else {
                    Console.WriteLine("그런 이름의 도시가 없습니다. 다시 입력하세요");
                    continue;
                }
                Console.Write("도착점의 이름을 입력하세요. ");
                input = Console.ReadLine();
                int to;
                if (cities.Contains(input)) {
                    to = cities.FindIndex(s => s == input);
                } else {
                    Console.WriteLine("그런 이름의 도시가 없습니다. 다시 입력하세요");
                    continue;
                }
                var res = graph.DijkstraShortedPath(from, to);
                if (res.Route.Count > 0) {
                    Console.WriteLine($"최단 거리는 {res.TotalDistance:0.##}입니다.");
                    Console.WriteLine("이하 경로를 출력합니다: ");
                    int prev = res.Route.First();
                    foreach (int i in res.Route.Skip(1)) {
                        Console.WriteLine($"{prev} ({cities[prev]}) -> {i} ({cities[i]}): Distance is {graph.GetWeightOfEdge(prev, i):0.##}");
                        prev = i;
                    }
                } else {
                    Console.WriteLine("최단 거리를 계산할 수 없습니다. (경로가 존재하지 않음)");
                }
            }
        }
    }
}
