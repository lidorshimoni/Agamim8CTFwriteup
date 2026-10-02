using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class FindWord5Detailed {
    static int[] c = new int[] { 4, 1, 5, 6, 0x31 };

    static bool IsValidHebrew(int ch) {
        return (ch >= 0x5D0 && ch <= 0x5EA);
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string dictPath = @"C:\Users\lidor\.gemini\antigravity\brain\af5dee43-217e-42db-b0f1-f9e2bb692b52\.system_generated\steps\117\content.md";

        HashSet<string> allWords = new HashSet<string>();
        List<string> fiveWords = new List<string>();

        foreach (var line in File.ReadAllLines(dictPath)) {
            var parts = line.Split(' ');
            if (parts.Length >= 1) {
                string w = parts[0].Trim();
                if (w.Length > 0 && !w.StartsWith("#") && !w.StartsWith("Title") && !w.StartsWith("Description") && !w.StartsWith("Source") && !w.StartsWith("-")) {
                    allWords.Add(w);
                    if (w.Length == 5) fiveWords.Add(w);
                }
            }
        }

        Console.WriteLine("Loaded " + fiveWords.Count + " 5-letter words.");

        foreach (var w in fiveWords) {
            bool ok = true;
            char[] other = new char[5];
            for (int j = 0; j < 5; j++) {
                int ch = c[j] ^ (int)w[j];
                if (!IsValidHebrew(ch)) { ok = false; break; }
                other[j] = (char)ch;
            }
            if (ok) {
                string otherStr = new string(other);
                // Check if otherStr is in dictionary, or print promising ones
                if (allWords.Contains(otherStr)) {
                    Console.WriteLine(string.Format("MATCH: '{0}' <-> '{1}'", w, otherStr));
                }
                if (w == "הסודי" || otherStr == "הסודי" || otherStr.Contains("נאור") || otherStr.Contains("אור") || otherStr.Contains("סוד")) {
                    Console.WriteLine(string.Format("INTERESTING: '{0}' <-> '{1}'", w, otherStr));
                }
            }
        }
    }
}
