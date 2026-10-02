using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class ListCandidates {
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
        
        List<string> words = new List<string>();
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

        PrintValid(words, 0, 5, "Word 1");
        PrintValid(words, 6, 6, "Word 2");
        PrintValid(words, 22, 5, "Word 5");
        PrintValid(words, 36, 5, "Word 8");
    }

    static void PrintValid(List<string> words, int start, int len, string name) {
        Console.WriteLine("\n=== " + name + " ===");
        int count = 0;
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
                count++;
                if (count <= 35) {
                    Console.WriteLine(string.Format("  '{0}' -> '{1}'", w, new string(other)));
                }
            }
        }
        Console.WriteLine("Total valid: " + count);
    }
}
