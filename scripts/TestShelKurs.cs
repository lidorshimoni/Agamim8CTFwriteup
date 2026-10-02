using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class TestShelKurs {
    static int[] cipher = new int[] {
        0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
        0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 4, 1, 5, 6, 0x31,
        0x5F9, 0x37, 0xC, 0x5F0, 0x35, 2, 0x31, 0x3F, 0x5C9, 0x39, 0x30, 0x3E,
        0x39, 0xD, 0x5CE
    };

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string dictPath = @"C:\Users\lidor\.gemini\antigravity\brain\af5dee43-217e-42db-b0f1-f9e2bb692b52\.system_generated\steps\117\content.md";

        var words = new HashSet<string>();
        foreach (var line in File.ReadAllLines(dictPath)) {
            var parts = line.Split(' ');
            if (parts.Length >= 1) {
                string w = parts[0].Trim();
                if (w.Length > 0 && !w.StartsWith("#") && !w.StartsWith("Title") && !w.StartsWith("Description") && !w.StartsWith("-")) {
                    words.Add(w);
                }
            }
        }
        words.Add("שמונה");
        words.Add("אגמים");
        words.Add("אגמיסט");
        words.Add("קורס");

        // Remaining pool for 4, 5, 8 if 6 is "של" and 7 is "קורס":
        // pool: א: 4, ה: 1, י: 1, נ: 3, ע: 2, ר: 1, ש: 2
        var pool = new Dictionary<char, int> {
            {'א', 4}, {'ה', 1}, {'י', 1}, {'נ', 3}, {'ע', 2}, {'ר', 1}, {'ש', 2}
        };

        // Check Slot 5 (pos 23..27, len 5):
        Console.WriteLine("Slot 5 candidates with pool {א:4, ה:1, י:1, נ:3, ע:2, ר:1, ש:2}:");
        var s5Cand = new List<string>();
        foreach (var w in words) {
            if (w.Length != 5) continue;
            bool ok = true;
            var temp = new Dictionary<char, int>();
            for (int j = 0; j < 5; j++) {
                char c = (char)(cipher[22 + j] ^ (int)w[j]);
                if (!pool.ContainsKey(c)) { ok = false; break; }
                if (!temp.ContainsKey(c)) temp[c] = 0;
                temp[c]++;
                if (temp[c] > pool[c]) { ok = false; break; }
            }
            if (ok) {
                s5Cand.Add(w);
                Console.WriteLine("  " + w);
            }
        }

        // Check Slot 4 (pos 18..21, len 4):
        Console.WriteLine("\nSlot 4 candidates:");
        var s4Cand = new List<string>();
        foreach (var w in words) {
            if (w.Length != 4) continue;
            bool ok = true;
            var temp = new Dictionary<char, int>();
            for (int j = 0; j < 4; j++) {
                char c = (char)(cipher[17 + j] ^ (int)w[j]);
                if (!pool.ContainsKey(c)) { ok = false; break; }
                if (!temp.ContainsKey(c)) temp[c] = 0;
                temp[c]++;
                if (temp[c] > pool[c]) { ok = false; break; }
            }
            if (ok) {
                s4Cand.Add(w);
                Console.WriteLine("  " + w);
            }
        }

        // Check Slot 8 (pos 37..41, len 5):
        Console.WriteLine("\nSlot 8 candidates:");
        var s8Cand = new List<string>();
        foreach (var w in words) {
            if (w.Length != 5) continue;
            bool ok = true;
            var temp = new Dictionary<char, int>();
            for (int j = 0; j < 5; j++) {
                char c = (char)(cipher[36 + j] ^ (int)w[j]);
                if (!pool.ContainsKey(c)) { ok = false; break; }
                if (!temp.ContainsKey(c)) temp[c] = 0;
                temp[c]++;
                if (temp[c] > pool[c]) { ok = false; break; }
            }
            if (ok) {
                s8Cand.Add(w);
                Console.WriteLine("  " + w);
            }
        }
    }
}
