using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class EnumeratePermutations {
    static int[] c = new int[] { 0x33, 0x37, 0x3D, 0x8, 4, 1, 5, 6, 0x31 };

    static bool IsHebrew(int ch) {
        return ch >= 0x5D0 && ch <= 0x5EA;
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;

        // Multiset of 9 letters: 3 Alefs, 1 He, 1 Yod, 1 Nun, 1 Ayin, 1 Resh, 1 Shin
        char[] letters = new char[] { 'א', 'א', 'א', 'ה', 'י', 'נ', 'ע', 'ר', 'ש' };

        var distinctPerms = new HashSet<string>();
        Permute(letters, 0, distinctPerms);

        Console.WriteLine("Total permutations: " + distinctPerms.Count);

        var validWords = new List<Tuple<string, string, string>>();

        foreach (var p in distinctPerms) {
            bool ok = true;
            char[] w = new char[9];
            for (int i = 0; i < 9; i++) {
                int ch = c[i] ^ (int)p[i];
                if (!IsHebrew(ch)) { ok = false; break; }
                w[i] = (char)ch;
            }
            if (ok) {
                string w4 = new string(w, 0, 4);
                string w5 = new string(w, 4, 5);
                validWords.Add(Tuple.Create(w4, w5, p));
            }
        }

        Console.WriteLine("Valid Hebrew character combinations: " + validWords.Count);
        foreach (var item in validWords) {
            Console.WriteLine(string.Format("Word4: '{0}'  Word5: '{1}'  Initials: '{2}'", item.Item1, item.Item2, item.Item3));
        }
    }

    static void Permute(char[] a, int l, HashSet<string> result) {
        if (l == a.Length - 1) {
            result.Add(new string(a));
            return;
        }
        for (int i = l; i < a.Length; i++) {
            if (i != l && a[i] == a[l]) continue;
            Swap(a, l, i);
            Permute(a, l + 1, result);
            Swap(a, l, i);
        }
    }

    static void Swap(char[] a, int i, int j) {
        char t = a[i]; a[i] = a[j]; a[j] = t;
    }
}
