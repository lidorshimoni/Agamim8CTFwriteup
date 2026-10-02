using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class TestWord4 {
    static int[] c = new int[] { 0x33, 0x37, 0x3D, 0x8 };

    static bool IsHebrew(int ch) {
        return ch >= 0x5D0 && ch <= 0x5EA;
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string dictPath = @"C:\Users\lidor\.gemini\antigravity\brain\af5dee43-217e-42db-b0f1-f9e2bb692b52\.system_generated\steps\117\content.md";

        foreach (var line in File.ReadAllLines(dictPath)) {
            var parts = line.Split(' ');
            if (parts.Length >= 1) {
                string w = parts[0].Trim();
                if (w.Length == 4) {
                    bool ok = true;
                    char[] other = new char[4];
                    for (int i = 0; i < 4; i++) {
                        int ch = c[i] ^ (int)w[i];
                        if (!IsHebrew(ch)) { ok = false; break; }
                        other[i] = (char)ch;
                    }
                    if (ok) {
                        Console.WriteLine(string.Format("{0} <-> {1}", w, new string(other)));
                    }
                }
            }
        }
    }
}
