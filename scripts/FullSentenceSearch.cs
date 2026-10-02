using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class FullSentenceSearch {
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

    // Spaces at 6, 13, 17, 22, 28, 31, 36 and period at 42
    // pos 6: ד (5F3 ^ 20 = ד)
    // pos 13: א (5F0 ^ 20 = א)
    // pos 17: ת (5CA ^ 20 = ת)
    // pos 22: ע (5C2 ^ 20 = ע)
    // pos 28: י (5F9 ^ 20 = י)
    // pos 31: א (5F0 ^ 20 = א)
    // pos 36: ש (5C9 ^ 20 = ש)
    // pos 42: נ (5CE ^ 2E = נ)

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
        allWords.Add("היא");
        allWords.Add("פשוט");
        allWords.Add("כולם");
        allWords.Add("שלכם");
        allWords.Add("אתכם");

        // Initial pool
        pool = new Dictionary<char, int> {
            {'א', 10}, {'ג', 1}, {'ד', 4}, {'ה', 1}, {'ח', 1},
            {'י', 3}, {'ל', 1}, {'מ', 3}, {'נ', 5}, {'ע', 5},
            {'ר', 2}, {'ש', 5}, {'ת', 1}
        };

        // Subtract the 8 fixed space/period positions
        pool['ד']--;
        pool['א']--;
        pool['ת']--;
        pool['ע']--;
        pool['י']--;
        pool['א']--;
        pool['ש']--;
        pool['נ']--;

        // Slot 1 is "מגילת", Slot 2 is "הדרקון", Slot 3 is "היא"
        // Let's verify slot 1, 2, 3:
        DeductWord(0, "מגילת");
        DeductWord(1, "הדרקון");
        DeductWord(2, "היא");

        Console.WriteLine("Pool before searching Slots 4..8:");
        foreach (var kvp in pool.OrderBy(k => k.Key)) {
            Console.WriteLine(string.Format("  {0}: {1}", kvp.Key, kvp.Value));
        }

        // Pre-filter candidate words for slots 4, 5, 6, 7, 8
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
            Console.WriteLine(string.Format("Slot {0} prefiltered words: {1}", s + 1, slotWords[s].Count));
        }

        // Backtracking search from slot 3 (Word 4) to slot 7 (Word 8)
        List<string> current = new List<string> { "מגילת", "הדרקון", "היא" };
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

    static int solutionsFound = 0;

    static void Search(int slotIdx, List<string> current) {
        if (slotIdx == 8) {
            // Check if pool is completely 0!
            bool perfect = pool.Values.All(v => v == 0);
            solutionsFound++;
            Console.WriteLine(string.Format("\n*** SOLUTION FOUND (#{0}, perfect pool match: {1}) ***", solutionsFound, perfect));
            Console.WriteLine("Sentence: " + string.Join(" ", current) + ".");

            // Print initials
            StringBuilder initials = new StringBuilder();
            int pos = 0;
            for (int s = 0; s < 8; s++) {
                string w = current[s];
                for (int j = 0; j < w.Length; j++) {
                    initials.Append((char)(cipher[pos++] ^ (int)w[j]));
                }
                if (s < 7) {
                    initials.Append((char)(cipher[pos++] ^ 0x20));
                } else {
                    initials.Append((char)(cipher[pos++] ^ 0x2E));
                }
            }
            Console.WriteLine("Initials: " + initials.ToString());
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
