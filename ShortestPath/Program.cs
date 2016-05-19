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
            SortedDictionary<string, int> cities = new SortedDictionary<string, int>();
            DijkstraGraph<int, string> graph = new DijkstraGraph<int, string>();
            int index = 0;
            foreach (var i in file.Skip(1).Take(count)) {
                string dup = i;
                int avoiddup = 1;
                while (true) {
                    try {
                        cities.Add(dup, index);
                        break;
                    } catch (ArgumentException) {
                        dup = $"{i}_{avoiddup}";
                        avoiddup++;
                    }
                }
                graph[index] = dup;
                index++;
            }
            Console.WriteLine($"도시 로딩이 완료되었습니다. {cities.Count}개의 도시가 있습니다.");
            // 도로 로딩
            file = file.Skip(1 + count);
            count = int.Parse(file.First());
            foreach (string i in file.Skip(1).Take(count)) {
                string[] line = i.Split(' ');
                if (line.Length == 3) graph.MakeEdge(int.Parse(line[0]), int.Parse(line[1]), double.Parse(line[2]));
            }
            Console.WriteLine($"도로 로딩이 완료되었습니다. {count}개의 도로가 있습니다.");
            Console.WriteLine("중복되는 이름이 있을 경우 2번째 도시부터는 차례대로 _1, _2 ... 를 붙여 구별합니다.");
            Console.WriteLine("원하는 도시를 찾을 수 없는 경우 중복이 있는지 확인하고 도시 번호로도 찾아보세요.");
            while (true) {
                Console.WriteLine();
                Console.Write($"시작점의 이름 또는 번호를 입력하세요. ");
                string input = Console.ReadLine();
                int from;
                try {
                    from = cities[input];
                } catch (KeyNotFoundException) {
                    if (!int.TryParse(input, out from)) {
                        Console.WriteLine("그런 도시가 없습니다. 다시 입력하세요");
                        continue;
                    }
                }
                Console.WriteLine($"시작점: {from} ({graph[from]})");
                Console.Write("도착점의 이름 또는 번호를 입력하세요. ");
                input = Console.ReadLine();
                int to;
                try {
                    to = cities[input];
                } catch (KeyNotFoundException) {
                    if (!int.TryParse(input, out to)) {
                        Console.WriteLine("그런 도시가 없습니다. 다시 입력하세요");
                        continue;
                    }
                }
                Console.WriteLine($"도착점: {to} ({graph[to]})");
                Console.WriteLine();
                var res = graph.ShortestPath(from, to);
                if (res.Route != null) {
                    Console.WriteLine($"최단 거리는 {res.TotalDistance:0.##}입니다.");
                    Console.WriteLine("이하 경로를 출력합니다: ");
                    int prev = res.Route.First();
                    foreach (int i in res.Route.Skip(1)) {
                        Console.WriteLine($"{prev} ({graph[prev]}) -> {i} ({graph[i]}): 거리는 {graph.GetWeightOfEdge(prev, i):0.##}");
                        prev = i;
                    }
                } else {
                    Console.WriteLine("최단 거리를 계산할 수 없습니다. (경로가 존재하지 않음)");
                }
            }
        }
    }
}
