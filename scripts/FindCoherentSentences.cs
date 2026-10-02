using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class FindCoherentSentences {
    static int[] cipher = new int[] {
        0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
        0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 4, 1, 5, 6, 0x31,
        0x5F9, 0x37, 0xC, 0x5F0, 0x35, 2, 0x31, 0x3F, 0x5C9, 0x39, 0x30, 0x3E,
        0x39, 0xD, 0x5CE
    };

    struct Slot {
        public int Start;
        public int Len;
    }

    static Slot[] slots = new Slot[] {
        new Slot { Start = 0, Len = 5 },   // Word 1: pos 1..5
        new Slot { Start = 6, Len = 6 },   // Word 2: pos 7..12
        new Slot { Start = 13, Len = 3 },  // Word 3: pos 14..16
        new Slot { Start = 17, Len = 4 },  // Word 4: pos 18..21
        new Slot { Start = 22, Len = 5 },  // Word 5: pos 23..27
        new Slot { Start = 28, Len = 2 },  // Word 6: pos 29..30
        new Slot { Start = 31, Len = 4 },  // Word 7: pos 32..35
        new Slot { Start = 36, Len = 5 }   // Word 8: pos 37..41
    };

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string dictPath = @"C:\Users\lidor\.gemini\antigravity\brain\af5dee43-217e-42db-b0f1-f9e2bb692b52\.system_generated\steps\117\content.md";

        Dictionary<string, int> freqs = new Dictionary<string, int>();
        int rank = 0;
        foreach (var line in File.ReadAllLines(dictPath)) {
            var parts = line.Split(' ');
            if (parts.Length >= 1) {
                string w = parts[0].Trim();
                if (w.Length > 0 && !freqs.ContainsKey(w) && !w.StartsWith("#") && !w.StartsWith("Title") && !w.StartsWith("Description") && !w.StartsWith("-")) {
                    freqs[w] = rank++;
                }
            }
        }

        // Remaining pool for pos 18..42 (excluding spaces):
        // א: 5, ג: 1, ה: 1, ח: 1, י: 2, מ: 2, נ: 3, ע: 2, ר: 1, ש: 2
        var initialPool = new Dictionary<char, int> {
            {'א', 5}, {'ג', 1}, {'ה', 1}, {'ח', 1}, {'י', 2},
            {'מ', 2}, {'נ', 3}, {'ע', 2}, {'ר', 1}, {'ש', 2}
        };

        // For each slot from 3 to 7 (Words 4 to 8):
        for (int s = 3; s < 8; s++) {
            var sl = slots[s];
            Console.WriteLine(string.Format("\n=== SLOT {0} (pos {1}..{2}, len {3}) ===", s + 1, sl.Start + 1, sl.Start + sl.Len, sl.Len));
            var list = new List<Tuple<string, string, int>>();

            foreach (var kvp in freqs) {
                string w = kvp.Key;
                if (w.Length != sl.Len) continue;

                bool ok = true;
                char[] other = new char[sl.Len];
                var temp = new Dictionary<char, int>();

                for (int j = 0; j < sl.Len; j++) {
                    char c = (char)(cipher[sl.Start + j] ^ (int)w[j]);
                    if (!initialPool.ContainsKey(c)) { ok = false; break; }
                    if (!temp.ContainsKey(c)) temp[c] = 0;
                    temp[c]++;
                    if (temp[c] > initialPool[c]) { ok = false; break; }
                    other[j] = c;
                }

                if (ok) {
                    list.Add(Tuple.Create(w, new string(other), kvp.Value));
                }
            }

            Console.WriteLine(string.Format("Total candidates: {0}", list.Count));
            foreach (var item in list.OrderBy(x => x.Item3).Take(20)) {
                Console.WriteLine(string.Format("  {0,-10} (rank {1,5}) -> initials: {2}", item.Item1, item.Item3, item.Item2));
            }
        }
    }
}
