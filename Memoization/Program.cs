using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Memoization {
    class Program {
        static void Main(string[] args) {
            int[,] path;
            {
                IEnumerable<string> file = File.ReadLines("input_and_answer.txt").Where(s => s.Trim().Length != 0 && s[0] != '#');
                List<int> size = file.First().Split(' ').Select(s => int.Parse(s)).ToList();
                // path 배열: 첫번째 차원은 시작점 번호, 두번째 차원은 끝점 번호이며 내용은 A to B의 링크 갯수
                path = new int[size[0], size[0]];
                // 각 줄을 읽어들여 path 배열의 해당되는 위치를 1 가산
                foreach (List<int> i in file.Skip(1).Take(size[1]).Select(s => s.Split(' ').Select(t => int.Parse(t)).ToList())) {
                    path[i[0], i[1]]++;
                }
            }
            Memoization mem = new Memoization();
            mem.PathToProb(path);
            Console.WriteLine("r을 입력하면 캐시 초기화를, x를 입력하면 종료합니다.");
            while (true) {
                int from, count;
                try {
                    Console.Write("출발점 번호를 입력하세요. ");
                    string input = Console.ReadLine();
                    if (input.ToLower() == "r") {
                        mem.ClearCache();
                        Console.WriteLine("캐시가 초기화되었습니다.");
                        continue;
                    } else if (input.ToLower() == "x") {
                        break;
                    } else {
                        from = int.Parse(input);
                    }
                    Console.Write("시도 횟수를 입력하세요. ");
                    input = Console.ReadLine();
                    count = int.Parse(input);
                } catch (FormatException) {
                    Console.WriteLine("적절한 값이 입력되지 않았습니다.");
                    continue;
                }
                Stopwatch sw = Stopwatch.StartNew();
                for (int i = 0; i < path.GetLength(0); i++) {
                    Console.Write($"{mem.GetProb(from, i, count) * 100:#.###}% ");
                }
                sw.Stop();
                Console.WriteLine();
                Console.WriteLine($"{sw.ElapsedMilliseconds}ms 소요되었습니다.");
            }
        }
        private class Memoization {
            // SortedDictionary는 C#에서 기본으로 제공하는 레드 블랙 트리입니다.
            private SortedDictionary<Tuple<int, int, int>, double> cache = new SortedDictionary<Tuple<int, int, int>, double>();
            private SortedDictionary<Tuple<int, int>, double> prob = new SortedDictionary<Tuple<int, int>, double>();
            public void PathToProb(int[,] Path) {
                if (prob.Count > 0) prob.Clear();
                for (int i = 0; i < Path.GetLength(0); i++) {
                    int sum = 0;
                    for (int j = 0; j < Path.GetLength(1); j++) sum += Path[i, j];
                    for (int j = 0; j < Path.GetLength(1); j++) prob[new Tuple<int, int>(i, j)] = (double)Path[i, j] / sum;
                }
            }
            public double GetProb(int from, int to, int count) {
                var key = new Tuple<int, int, int>(from, to, count);
                if (count == 1) {
                    try {
                        return prob[new Tuple<int, int>(from, to)];
                    } catch (KeyNotFoundException) {
                        return 0;
                    }
                } else if (cache.ContainsKey(key)) {
                    // 캐싱된 결과 리턴
                    return cache[key];
                } else {
                    double sum = 0;
                    foreach (var i in prob.Where(s => s.Key.Item1 == from)) {
                        sum += prob[new Tuple<int, int>(from, i.Key.Item2)] * GetProb(i.Key.Item2, to, count - 1);
                    }
                    // 계산 결과 캐시
                    cache[key] = sum;
                    return sum;
                }
            }
            public void ClearCache() {
                cache.Clear();
            }
        }
    }
}