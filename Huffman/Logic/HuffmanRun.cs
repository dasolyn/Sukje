using System;

namespace Huffman {
    public class HuffmanRun : IEquatable<HuffmanRun> {
        public byte Symbol { get; set; }
        public int Length { get; set; }
        public int Frequency { get; set; }
        public HuffmanRun(byte Symbol) {
            this.Symbol = Symbol;
            Length = 1;
            Frequency = 1;
        }

        public override bool Equals(object other) {
            HuffmanRun o = other as HuffmanRun;
            if (o == null) return false;
            else return Equals(o);
        }
        public bool Equals(HuffmanRun other) {
            return Symbol == other.Symbol && Length == other.Length;
        }
        public override int GetHashCode() {
            return unchecked(Symbol.GetHashCode() + Length.GetHashCode());
        }
        public static bool operator ==(HuffmanRun a, HuffmanRun b) {
            return a.Equals(b);
        }
        public static bool operator !=(HuffmanRun a, HuffmanRun b) {
            return !a.Equals(b);
        }
    }
}
