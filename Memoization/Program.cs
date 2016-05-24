using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Memoization {
    class Program {
        static void Main(string[] args) {
            int[,] path;
            int from;
            int count;
            {
                IEnumerable<string> file = File.ReadLines("input_and_answer.txt").Where(s => s.Trim().Length != 0 && s[0] != '#');
                List<int> size = file.First().Split(' ').Select(s => int.Parse(s)).ToList();
                // path 배열: 첫번째 차원은 시작점 번호, 두번째 차원은 끝점 번호이며 내용은 A to B의 링크 갯수
                path = new int[size[0], size[0]];
                // 각 줄을 읽어들여 path 배열의 해당되는 위치를 1 가산
                foreach (List<int> i in file.Skip(1).Take(size[1]).Select(s => s.Split(' ').Select(t => int.Parse(t)).ToList())) {
                    path[i[0], i[1]]++;
                }
                // 마지막 줄 해석
                List<int> input = file.Skip(1 + size[1]).First().Split(' ').Select(s => int.Parse(s)).ToList();
                from = input[0];
                count = input[1];
            }
            {
                Memoization mem = new Memoization(path.GetLength(0));
                mem.PathToProb(path);
                for (int i = 0; i < path.GetLength(0); i++) {
                    Console.WriteLine(mem.GetProb(from, i, count));
                }
            }
            Console.ReadLine();
        }
        private class Memoization {
            // SortedDictionary는 C#에서 기본으로 제공하는 레드 블랙 트리입니다.
            private SortedDictionary<int, double[,]> cache;
            private double[,] prob;
            private int size;
            public void PathToProb(int[,] Path) {
                for (int i = 0; i < Path.GetLength(0); i++) {
                    int sum = 0;
                    for (int j = 0; j < Path.GetLength(1); j++) sum += Path[i, j];
                    for (int j = 0; j < Path.GetLength(1); j++) prob[i, j] = (double)Path[i, j] / sum;
                }
            }
            public double GetProb(int from, int to, int count) {
                if (count == 1) {
                    return prob[from, to];
                } else if (cache.ContainsKey(count) && cache[count][from, to] != -1) {
                    // 캐싱된 결과 리턴
                    return cache[count][from, to];
                } else {
                    double sum = 0;
                    for (int i = 0; i < size; i++) {
                        sum += prob[from, i] * GetProb(i, to, count - 1);
                    }
                    // 계산 결과 캐시
                    if (cache.ContainsKey(count)) {
                        cache[count][from, to] = sum;
                    } else {
                        cache[count] = new double[size, size];
                        for (int i = 0; i < size; i++)
                            for (int j = 0; j < size; j++)
                                cache[count][i, j] = -1;
                        cache[count][from, to] = sum;
                    }
                    return sum;
                }
            }
            public Memoization(int size) {
                this.size = size;
                prob = new double[size, size];
                cache = new SortedDictionary<int, double[,]>();
            }
        }
    }
}