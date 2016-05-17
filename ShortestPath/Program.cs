using Graph;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShortestPath {
    class Program {
        static void Main(string[] args) {
            IEnumerable<string> file = File.ReadLines("road_segments.txt").Where(s => s[0] != '#' && s.Trim().Length != 0);
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
        }
    }
}
