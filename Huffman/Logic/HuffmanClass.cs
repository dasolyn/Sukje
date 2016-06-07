using System.Collections.Generic;

namespace Huffman {
    public static class HuffmanClass {
        public static List<HuffmanRun> CollectRuns(byte[] b) {
            List<HuffmanRun> listrun = new List<HuffmanRun>();
            HuffmanRun run = null;
            foreach (byte ch in b) {
                if (run == null) {
                    run = new HuffmanRun(ch);
                } else {
                    if (run.Symbol == ch) {
                        run.Length++;
                    } else {
                        HuffmanRun found = listrun.Find(t => t == run);
                        if (found == null) listrun.Add(run);
                        else found.Frequency++;
                        run = new HuffmanRun(ch);
                    }
                }
            }
            // 마지막 문자 저장
            if (run != null) {
                HuffmanRun found = listrun.Find(t => t == run);
                if (found == null) listrun.Add(run);
                else found.Frequency++;
            }
            return listrun;
        }
        public static HuffmanRun CreateHuffmanTree(List<HuffmanRun> CollectedRuns) {
            MinPQueue<HuffmanRun> heap = new MinPQueue<HuffmanRun>(CollectedRuns, (a, b) => a.Frequency.CompareTo(b.Frequency));
            while (heap.Count > 1) {
                HuffmanRun run1 = heap.Pop();
                HuffmanRun run2 = heap.Pop();
                heap.Push(new HuffmanRun(run1, run2));
            }
            return heap.Pop();
        }
    }
}
