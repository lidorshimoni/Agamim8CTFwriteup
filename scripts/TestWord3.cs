using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class TestWord3 {
    static int[] c3 = new int[] { 0x7, 0x39, 0x39 };

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string dictPath = @"C:\Users\lidor\.gemini\antigravity\brain\af5dee43-217e-42db-b0f1-f9e2bb692b52\.system_generated\steps\117\content.md";

        var allowed = new HashSet<char>("אגדהחיזטילמנסעפצקרשת".ToCharArray());

        foreach (var line in File.ReadAllLines(dictPath)) {
            var parts = line.Split(' ');
            if (parts.Length >= 1) {
                string w = parts[0].Trim();
                if (w.Length == 3) {
                    bool ok = true;
                    char[] inits = new char[3];
                    for (int i = 0; i < 3; i++) {
                        char ch = (char)(c3[i] ^ (int)w[i]);
                        if (!allowed.Contains(ch)) { ok = false; break; }
                        inits[i] = ch;
                    }
                    if (ok) {
                        Console.WriteLine(string.Format("{0} -> {1}", w, new string(inits)));
                    }
                }
            }
        }
    }
}
