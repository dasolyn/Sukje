using System;
using System.Diagnostics;

namespace algorithms2 {
    class Program {
        private const int N = 10000;
        private const int M = 10000;
        static void Main(string[] args) {
            Pqueue<int> pqueue;

            // 배열 방식
            pqueue = new ArrayPqueue<int>();
            Random rand = new Random();
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < N; i++) pqueue.Insert(rand.Next(N));
            for (int i = 0; i < M; i++) {
                int rint = rand.Next(2);
                if (rint == 0) pqueue.Insert(rand.Next(N));
                else pqueue.ExtractMax();
            }
            watch.Stop();

            Console.WriteLine($"Array Pqueue: {watch.ElapsedMilliseconds} ms");

            // 힙 방식
            pqueue = new HeapPqueue<int>();
            watch.Restart();
            for (int i = 0; i < N; i++) pqueue.Insert(rand.Next(N));
            for (int i = 0; i < M; i++) {
                int rint = rand.Next(2);
                if (rint == 0) pqueue.Insert(rand.Next(N));
                else pqueue.ExtractMax();
            }
            watch.Stop();
            
            Console.WriteLine($"Heap Pqueue: {watch.ElapsedMilliseconds} ms");
            
            Console.ReadLine(); // 자동 종료 방지
        }
    }
}
