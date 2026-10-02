using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class MatchWords4And5 {
    static int[] c4 = new int[] { 0x33, 0x37, 0x3D, 0x8 };
    static int[] c5 = new int[] { 4, 1, 5, 6, 0x31 };

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
        words.Add("מחזור");
        words.Add("מפקד");
        words.Add("הסוד");

        // Remaining pool for words 4 and 5:
        // א: 3, ה: 1, י: 1, נ: 1, ע: 1, ר: 1, ש: 1
        var pool = new Dictionary<char, int> {
            {'א', 3}, {'ה', 1}, {'י', 1}, {'נ', 1}, {'ע', 1}, {'ר', 1}, {'ש', 1}
        };

        var w4List = new List<Tuple<string, string>>();
        foreach (var w in words) {
            if (w.Length != 4) continue;
            bool ok = true;
            var temp = new Dictionary<char, int>();
            char[] chars = new char[4];
            for (int j = 0; j < 4; j++) {
                char c = (char)(c4[j] ^ (int)w[j]);
                if (!pool.ContainsKey(c)) { ok = false; break; }
                if (!temp.ContainsKey(c)) temp[c] = 0;
                temp[c]++;
                if (temp[c] > pool[c]) { ok = false; break; }
                chars[j] = c;
            }
            if (ok) w4List.Add(Tuple.Create(w, new string(chars)));
        }

        var w5List = new List<Tuple<string, string>>();
        foreach (var w in words) {
            if (w.Length != 5) continue;
            bool ok = true;
            var temp = new Dictionary<char, int>();
            char[] chars = new char[5];
            for (int j = 0; j < 5; j++) {
                char c = (char)(c5[j] ^ (int)w[j]);
                if (!pool.ContainsKey(c)) { ok = false; break; }
                if (!temp.ContainsKey(c)) temp[c] = 0;
                temp[c]++;
                if (temp[c] > pool[c]) { ok = false; break; }
                chars[j] = c;
            }
            if (ok) w5List.Add(Tuple.Create(w, new string(chars)));
        }

        Console.WriteLine(string.Format("w4 candidates: {0}, w5 candidates: {1}", w4List.Count, w5List.Count));
        Console.WriteLine("w4 candidates:");
        foreach (var p in w4List) Console.WriteLine(string.Format("  '{0}' -> {1}", p.Item1, p.Item2));
        Console.WriteLine("w5 candidates:");
        foreach (var p in w5List) Console.WriteLine(string.Format("  '{0}' -> {1}", p.Item1, p.Item2));

        foreach (var p4 in w4List) {
            foreach (var p5 in w5List) {
                string combined = p4.Item2 + p5.Item2;
                var counts = new Dictionary<char, int>();
                foreach (char ch in combined) {
                    if (!counts.ContainsKey(ch)) counts[ch] = 0;
                    counts[ch]++;
                }
                bool match = true;
                foreach (var k in pool.Keys) {
                    if (!counts.ContainsKey(k) || counts[k] != pool[k]) { match = false; break; }
                }
                if (match) {
                    Console.WriteLine(string.Format("MATCH FOUND!\n  Word 4: '{0}' (initials: {1})\n  Word 5: '{0}' (initials: {2})\n", p4.Item1, p4.Item2, p5.Item2));
                    Console.WriteLine("Full sentence: מגילת הדרקון היא " + p4.Item1 + " " + p5.Item1 + " של קורס אגמים.");
                }
            }
        }
    }
}
