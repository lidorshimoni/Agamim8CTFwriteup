using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class SearchPashut {
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
        allWords.Add("אגמים");
        allWords.Add("אגמיסט");
        allWords.Add("שיפמן");
        allWords.Add("שמונה");
        allWords.Add("מחזור");
        allWords.Add("פנדה");
        allWords.Add("קורס");
        allWords.Add("לוחמי");
        allWords.Add("לוחם");

        // Pool for 5, 6, 7, 8:
        // א: 4, ג: 1, ה: 1, י: 2, מ: 1, נ: 3, ע: 2, ש: 2
        pool = new Dictionary<char, int> {
            {'א', 4}, {'ג', 1}, {'ה', 1}, {'י', 2},
            {'מ', 1}, {'נ', 3}, {'ע', 2}, {'ש', 2}
        };

        slotWords = new List<string>[4];
        for (int s = 0; s < 4; s++) {
            slotWords[s] = new List<string>();
            var sl = slots[s];
            foreach (var w in allWords) {
                if (w.Length != sl.Len) continue;
                bool ok = true;
                for (int j = 0; j < sl.Len; j++) {
                    char c = (char)(cipher[sl.Start + j] ^ (int)w[j]);
                    if (!pool.ContainsKey(c)) { ok = false; break; }
                }
                if (ok) slotWords[s].Add(w);
            }
            Console.WriteLine(string.Format("Slot {0} words: {1}", s + 5, slotWords[s].Count));
        }

        List<string> current = new List<string> { "מגילת", "הדרקון", "היא", "פשוט" };
        Search(0, current);
    }

    static void DeductWord(int s, string w) {
        var sl = slots[s];
        for (int j = 0; j < sl.Len; j++) {
            char c = (char)(cipher[sl.Start + j] ^ (int)w[j]);
            pool[c]--;
        }
    }

    static void RevertWord(int s, string w) {
        var sl = slots[s];
        for (int j = 0; j < sl.Len; j++) {
            char c = (char)(cipher[sl.Start + j] ^ (int)w[j]);
            pool[c]++;
        }
    }

    static bool CanDeduct(int s, string w) {
        var sl = slots[s];
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

    static void Search(int s, List<string> current) {
        if (s == 4) {
            bool perfect = pool.Values.All(v => v == 0);
            if (perfect) {
                Console.WriteLine("FOUND: " + string.Join(" ", current) + ".");
            }
            return;
        }

        foreach (var w in slotWords[s]) {
            if (CanDeduct(s, w)) {
                DeductWord(s, w);
                current.Add(w);
                Search(s + 1, current);
                current.RemoveAt(current.Count - 1);
                RevertWord(s, w);
            }
        }
    }
}
