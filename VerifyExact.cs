using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class VerifyExact {
    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        int[] cipher = new int[] {
            0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
            0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 4, 1, 5, 6, 0x31,
            0x5F9, 0x37, 0xC, 0x5F0, 0x35, 2, 0x31, 0x3F, 0x5C9, 0x39, 0x30, 0x3E,
            0x39, 0xD, 0x5CE
        };
        string sentence = "מגילת הדרקון היא דמות הבוגר של קורס אגמים.";

        var pool = new Dictionary<char, int> {
            {'א', 10}, {'ג', 1}, {'ד', 4}, {'ה', 1}, {'ח', 1},
            {'י', 3}, {'ל', 1}, {'מ', 3}, {'נ', 5}, {'ע', 5},
            {'ר', 2}, {'ש', 5}, {'ת', 1}
        };

        var used = new Dictionary<char, int>();
        for (int i = 0; i < 42; i++) {
            char c = (char)(cipher[i] ^ (int)sentence[i]);
            if (!used.ContainsKey(c)) used[c] = 0;
            used[c]++;
        }

        bool match = true;
        foreach (var k in pool.Keys.OrderBy(k => k)) {
            int u = used.ContainsKey(k) ? used[k] : 0;
            Console.WriteLine(string.Format("{0}: need {1}, got {2}", k, pool[k], u));
            if (u != pool[k]) match = false;
        }
        Console.WriteLine("PERFECT MATCH: " + match);
    }
}
