using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class SearchMeet {
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

    static Dictionary<char, int> pool;
    static List<string>[] slotWords;

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string dictPath = @"C:\Users\lidor\.gemini\antigravity\brain\af5dee43-217e-42db-b0f1-f9e2bb692b52\.system_generated\steps\117\content.md";

        HashSet<string> allWords = new HashSet<string>();
        foreach (var line in File.ReadAllLines(dictPath)) {
            var parts = line.Split(' ');
            if (parts.Length >= 1) {
                string w = parts[0].Trim();
                if (w.Length > 0 && !w.StartsWith("#") && !w.StartsWith("Title") && !w.StartsWith("Description") && !w.StartsWith("-")) {
                    allWords.Add(w);
                }
            }
        }
        allWords.Add("מגילת");
        allWords.Add("הדרקון");
        allWords.Add("מאת");
        allWords.Add("קורס");
        allWords.Add("אגמים");
        allWords.Add("שיפמן");

        pool = new Dictionary<char, int> {
            {'א', 10}, {'ג', 1}, {'ד', 4}, {'ה', 1}, {'ח', 1},
            {'י', 3}, {'ל', 1}, {'מ', 3}, {'נ', 5}, {'ע', 5},
            {'ר', 2}, {'ש', 5}, {'ת', 1}
        };

        pool['ד']--; pool['א']--; pool['ת']--; pool['ע']--;
        pool['י']--; pool['א']--; pool['ש']--; pool['נ']--;

        DeductWord(0, "מגילת");
        DeductWord(1, "הדרקון");
        DeductWord(2, "מאת");

        slotWords = new List<string>[8];
        for (int s = 3; s < 8; s++) {
            slotWords[s] = new List<string>();
            var sl = slots[s];
            foreach (var w in allWords) {
                if (w.Length != sl.Len) continue;
                bool ok = true;
                for (int j = 0; j < sl.Len; j++) {
                    int ch = cipher[sl.Start + j] ^ (int)w[j];
                    char c = (char)ch;
                    if (!pool.ContainsKey(c)) { ok = false; break; }
                }
                if (ok) slotWords[s].Add(w);
            }
        }

        List<string> current = new List<string> { "מגילת", "הדרקון", "מאת" };
        Search(3, current);
    }

    static void DeductWord(int slotIdx, string w) {
        var sl = slots[slotIdx];
        for (int j = 0; j < sl.Len; j++) {
            char c = (char)(cipher[sl.Start + j] ^ (int)w[j]);
            pool[c]--;
        }
    }

    static void RevertWord(int slotIdx, string w) {
        var sl = slots[slotIdx];
        for (int j = 0; j < sl.Len; j++) {
            char c = (char)(cipher[sl.Start + j] ^ (int)w[j]);
            pool[c]++;
        }
    }

    static bool CanDeduct(int slotIdx, string w) {
        var sl = slots[slotIdx];
        var temp = new Dictionary<char, int>();
        for (int j = 0; j < sl.Len; j++) {
            char c = (char)(cipher[sl.Start + j] ^ (int)w[j]);
            if (!pool.ContainsKey(c)) return false;
            if (!temp.ContainsKey(c)) temp[c] = 0;
            temp[c]++;
            if (temp[c] > pool[c]) return false;
        }
        return true;
    }

    static void Search(int slotIdx, List<string> current) {
        if (slotIdx == 8) {
            bool perfect = pool.Values.All(v => v == 0);
            if (perfect) {
                Console.WriteLine(string.Join(" ", current) + ".");
            }
            return;
        }

        foreach (var w in slotWords[slotIdx]) {
            if (CanDeduct(slotIdx, w)) {
                DeductWord(slotIdx, w);
                current.Add(w);
                Search(slotIdx + 1, current);
                current.RemoveAt(current.Count - 1);
                RevertWord(slotIdx, w);
            }
        }
    }
}
