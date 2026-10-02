using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class Solver {
    static int[] cipher = new int[] {
        0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
        0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 0x4, 0x1, 0x5, 0x6,
        0x31, 0x5F9, 0x37, 0xC, 0x5F0, 0x35, 0x2, 0x31, 0x3F, 0x5C9, 0x39, 0x30,
        0x3E, 0x39, 0xD, 0x5CE
    };

    static bool IsValidHebrew(int c) {
        return (c >= 0x5D0 && c <= 0x5EA);
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string dictPath = @"C:\Users\lidor\.gemini\antigravity\brain\af5dee43-217e-42db-b0f1-f9e2bb692b52\.system_generated\steps\117\content.md";
        
        HashSet<string> words = new HashSet<string>();
        foreach (var line in File.ReadAllLines(dictPath)) {
            var parts = line.Split(' ');
            if (parts.Length >= 1) {
                string w = parts[0].Trim();
                if (w.Length > 0 && !w.StartsWith("#") && !w.StartsWith("Title") && !w.StartsWith("Description") && !w.StartsWith("Source") && !w.StartsWith("-")) {
                    bool allHeb = true;
                    foreach (char c in w) {
                        if (c < 0x5D0 || c > 0x5EA) { allHeb = false; break; }
                    }
                    if (allHeb) words.Add(w);
                }
            }
        }
        Console.WriteLine("Loaded " + words.Count + " Hebrew words.");

        // Word spans (0-indexed start, length):
        // Word 1: 0, 5
        // Word 2: 6, 6
        // Word 3: 13, 3
        // Word 4: 17, 4
        // Word 5: 22, 5
        // Word 6: 28, 2
        // Word 7: 31, 4
        // Word 8: 36, 5

        int[][] spans = new int[][] {
            new int[] { 0, 5 },
            new int[] { 6, 6 },
            new int[] { 13, 3 },
            new int[] { 17, 4 },
            new int[] { 22, 5 },
            new int[] { 28, 2 },
            new int[] { 31, 4 },
            new int[] { 36, 5 }
        };

        for (int s = 0; s < spans.Length; s++) {
            int start = spans[s][0];
            int len = spans[s][1];
            Console.WriteLine(string.Format("\n=== Word {0} (pos {1}-{2}, len {3}) ===", s + 1, start + 1, start + len, len));

            List<string> doubleHits = new List<string>();
            List<string> singleHits = new List<string>();

            foreach (var w in words) {
                if (w.Length != len) continue;
                bool ok = true;
                char[] other = new char[len];
                for (int j = 0; j < len; j++) {
                    int ch = cipher[start + j] ^ (int)w[j];
                    if (!IsValidHebrew(ch)) { ok = false; break; }
                    other[j] = (char)ch;
                }
                if (ok) {
                    string otherStr = new string(other);
                    if (words.Contains(otherStr)) {
                        doubleHits.Add(string.Format("'{0}' <-> '{1}'", w, otherStr));
                    } else {
                        singleHits.Add(string.Format("'{0}' -> '{1}'", w, otherStr));
                    }
                }
            }

            Console.WriteLine("DOUBLE HITS (both in dictionary): " + doubleHits.Count);
            foreach (var hit in doubleHits) {
                Console.WriteLine("  " + hit);
            }
            if (doubleHits.Count == 0) {
                Console.WriteLine("Sample single hits (first 10):");
                for (int i = 0; i < Math.Min(10, singleHits.Count); i++) {
                    Console.WriteLine("  " + singleHits[i]);
                }
            }
        }
    }
}
