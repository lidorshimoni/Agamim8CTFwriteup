using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class GenerateWordLists {
    static int[] cipher = new int[] {
        0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
        0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 4, 1, 5, 6, 0x31,
        0x5F9, 0x37, 0xC, 0x5F0, 0x35, 2, 0x31, 0x3F, 0x5C9, 0x39, 0x30, 0x3E,
        0x39, 0xD, 0x5CE
    };

    static bool IsHebrew(int ch) {
        return ch >= 0x5D0 && ch <= 0x5EA;
    }

    struct SlotInfo {
        public string Name;
        public int Start;
        public int Len;
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string dictPath = @"C:\Users\lidor\.gemini\antigravity\brain\af5dee43-217e-42db-b0f1-f9e2bb692b52\.system_generated\steps\117\content.md";

        Dictionary<string, int> dict = new Dictionary<string, int>();
        int rank = 0;
        foreach (var line in File.ReadAllLines(dictPath)) {
            var parts = line.Split(' ');
            if (parts.Length >= 1) {
                string w = parts[0].Trim();
                if (w.Length > 0 && !dict.ContainsKey(w) && !w.StartsWith("#") && !w.StartsWith("Title") && !w.StartsWith("Description") && !w.StartsWith("-")) {
                    dict[w] = rank++;
                }
            }
        }

        SlotInfo[] slots = new SlotInfo[] {
            new SlotInfo { Name = "מילה 1 (5 אותיות, מיקומים 1-5)", Start = 0, Len = 5 },
            new SlotInfo { Name = "מילה 2 (6 אותיות, מיקומים 7-12)", Start = 6, Len = 6 },
            new SlotInfo { Name = "מילה 3 (3 אותיות, מיקומים 14-16)", Start = 13, Len = 3 },
            new SlotInfo { Name = "מילה 4 (4 אותיות, מיקומים 18-21)", Start = 17, Len = 4 },
            new SlotInfo { Name = "מילה 5 (5 אותיות, מיקומים 23-27)", Start = 22, Len = 5 },
            new SlotInfo { Name = "מילה 6 (2 אותיות, מיקומים 29-30)", Start = 28, Len = 2 },
            new SlotInfo { Name = "מילה 7 (4 אותיות, מיקומים 32-35)", Start = 31, Len = 4 },
            new SlotInfo { Name = "מילה 8 (5 אותיות, מיקומים 37-41)", Start = 36, Len = 5 }
        };

        foreach (var slot in slots) {
            Console.WriteLine("========================================");
            Console.WriteLine("=== " + slot.Name + " ===");
            Console.WriteLine("========================================");

            List<Tuple<string, string, int>> candidates = new List<Tuple<string, string, int>>();

            foreach (var kvp in dict) {
                string w = kvp.Key;
                if (w.Length != slot.Len) continue;

                bool valid = true;
                char[] other = new char[slot.Len];
                for (int j = 0; j < slot.Len; j++) {
                    int ch = cipher[slot.Start + j] ^ (int)w[j];
                    if (!IsHebrew(ch)) { valid = false; break; }
                    other[j] = (char)ch;
                }
                if (valid) {
                    string otherStr = new string(other);
                    int score = kvp.Value;
                    if (dict.ContainsKey(otherStr)) {
                        // Double dictionary hit! Give it a huge boost
                        score -= 50000;
                    }
                    candidates.Add(Tuple.Create(w, otherStr, score));
                }
            }

            // Sort by score (frequency and double hits first)
            var sorted = candidates.OrderBy(c => c.Item3).Take(30).ToList();
            foreach (var item in sorted) {
                bool doubleHit = dict.ContainsKey(item.Item2);
                string mark = doubleHit ? " [זוג מילים במילון!]" : "";
                Console.WriteLine(string.Format("  {0,-10} <->  {1,-10}{2}", item.Item1, item.Item2, mark));
            }
            Console.WriteLine();
        }
    }
}
