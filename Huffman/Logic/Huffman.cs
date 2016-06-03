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
    }
}
