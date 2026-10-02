using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class FindW4W5Pairs {
    static int[] c = new int[] { 0x33, 0x37, 0x3D, 0x8, 4, 1, 5, 6, 0x31 };

    static bool IsHebrew(int ch) {
        return ch >= 0x5D0 && ch <= 0x5EA;
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;

        string[] realW4 = new string[] {
            "דקות", "בקשר", "תמות", "בקול", "קופר", "דמות", "כושר", "כופר", "בקהל", "בחול",
            "פשוט", "כולם", "שלכם", "אתכם", "שלנו", "אותם", "אותו", "אותך", "אותי",
            "חלקם", "חלקך", "ביחד", "כוחם", "כבוד", "יחדו", "איתם", "כולו", "כולה"
        };

        foreach (var w4 in realW4) {
            var pool = new Dictionary<char, int> {
                {'א', 3}, {'ה', 1}, {'י', 1}, {'נ', 1}, {'ע', 1}, {'ר', 1}, {'ש', 1}
            };
            bool ok = true;
            char[] inits4 = new char[4];
            for (int i = 0; i < 4; i++) {
                int ch = c[i] ^ (int)w4[i];
                char chInit = (char)ch;
                if (!pool.ContainsKey(chInit) || pool[chInit] <= 0) {
                    ok = false;
                    break;
                }
                pool[chInit]--;
                inits4[i] = chInit;
            }

            if (ok) {
                Console.WriteLine(string.Format("\n=== W4: '{0}' (initials: {1}) ===", w4, new string(inits4)));
                Console.Write("Remaining 5 initials for Word 5: ");
                List<char> rem = new List<char>();
                foreach (var kvp in pool) {
                    for (int k = 0; k < kvp.Value; k++) rem.Add(kvp.Key);
                }
                Console.WriteLine(new string(rem.ToArray()));

                // Enumerate all distinct permutations of these 5 initials for Word 5:
                var perms = new HashSet<string>();
                Permute5(rem.ToArray(), 0, perms);

                var candidates5 = new List<Tuple<string, string>>();
                foreach (var p in perms) {
                    bool ok5 = true;
                    char[] w5 = new char[5];
                    for (int j = 0; j < 5; j++) {
                        int ch = c[4 + j] ^ (int)p[j];
                        if (!IsHebrew(ch)) { ok5 = false; break; }
                        w5[j] = (char)ch;
                    }
                    if (ok5) {
                        candidates5.Add(Tuple.Create(new string(w5), p));
                    }
                }

                Console.WriteLine(string.Format("Valid 5-letter Hebrew letter outputs: {0}", candidates5.Count));
                foreach (var c5 in candidates5) {
                    // Filter out outputs with final letters in the middle or weird letter shapes:
                    string s = c5.Item1;
                    if (s.StartsWith("ה") || s.StartsWith("מ") || s.StartsWith("ב") || s.StartsWith("ש") || s.StartsWith("כ") || s.StartsWith("ל") || s.StartsWith("א")) {
                        Console.WriteLine(string.Format("  w5: '{0}' (from initials: {1})", s, c5.Item2));
                    }
                }
            }
        }
    }

    static void Permute5(char[] a, int l, HashSet<string> result) {
        if (l == a.Length - 1) {
            result.Add(new string(a));
            return;
        }
        for (int i = l; i < a.Length; i++) {
            if (i != l && a[i] == a[l]) continue;
            Swap(a, l, i);
            Permute5(a, l + 1, result);
            Swap(a, l, i);
        }
    }

    static void Swap(char[] a, int i, int j) {
        char t = a[i]; a[i] = a[j]; a[j] = t;
    }
}
