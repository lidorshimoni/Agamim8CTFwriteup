using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class CoherentSentencesSearch {
    static int[] cipher = new int[] {
        0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
        0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 4, 1, 5, 6, 0x31,
        0x5F9, 0x37, 0xC, 0x5F0, 0x35, 2, 0x31, 0x3F, 0x5C9, 0x39, 0x30, 0x3E,
        0x39, 0xD, 0x5CE
    };

    struct Slot {
        public int Start;
        public int Len;
    }

    static Slot[] slots = new Slot[] {
        new Slot { Start = 17, Len = 4 },  // Word 4: pos 18..21
        new Slot { Start = 22, Len = 5 },  // Word 5: pos 23..27
        new Slot { Start = 28, Len = 2 },  // Word 6: pos 29..30
        new Slot { Start = 31, Len = 4 },  // Word 7: pos 32..35
        new Slot { Start = 36, Len = 5 }   // Word 8: pos 37..41
    };


    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string dictPath = @"C:\Users\lidor\.gemini\antigravity\brain\af5dee43-217e-42db-b0f1-f9e2bb692b52\.system_generated\steps\117\content.md";

        var allWords = new HashSet<string>();
        foreach (var line in File.ReadAllLines(dictPath)) {
            var parts = line.Split(' ');
            if (parts.Length >= 1) {
                string w = parts[0].Trim();
                if (w.Length > 0 && !w.StartsWith("#") && !w.StartsWith("Title") && !w.StartsWith("Description") && !w.StartsWith("-")) {
                    allWords.Add(w);
                }
            }
        }
        allWords.Add("קורס");
        allWords.Add("אגמים");
        allWords.Add("אנשים");
        allWords.Add("הסופי");
        allWords.Add("פשוט");
        allWords.Add("כולם");

        // Candidates for Slot 4 (len 4):
        // Filter by meaningful Hebrew words
        string[] w4Meaningful = new string[] {
            "פשוט", "בקשר", "בקהל", "כושר", "דמות", "בחור", "דקות", "סופר", "קשור", "תופס"
        };

        // Candidates for Slot 6 (len 2):
        string[] w6Meaningful = new string[] {
            "של", "כל", "על", "עם"
        };

        // Candidates for Slot 7 (len 4):
        string[] w7Meaningful = new string[] {
            "קורס", "לנצח", "קודם", "לאדם", "לעולם"
        };

        // Candidates for Slot 8 (len 5):
        string[] w8Meaningful = new string[] {
            "אגמים", "אנשים", "בפנים", "בימים", "בעתיד"
        };

        // Base pool after Words 1..3 and fixed spaces:
        // Pool: א: 5, ג: 1, ה: 1, ח: 1, י: 2, מ: 2, נ: 3, ע: 2, ר: 1, ש: 2
        var basePool = new Dictionary<char, int> {
            {'א', 5}, {'ג', 1}, {'ה', 1}, {'ח', 1}, {'י', 2},
            {'מ', 2}, {'נ', 3}, {'ע', 2}, {'ר', 1}, {'ש', 2}
        };

        // Let's test all combinations of (w4, w6, w7, w8) and see what Word 5 MUST be!
        foreach (var w4 in w4Meaningful) {
            foreach (var w6 in w6Meaningful) {
                foreach (var w7 in w7Meaningful) {
                    foreach (var w8 in w8Meaningful) {
                        var p = new Dictionary<char, int>(basePool);
                        bool ok = true;

                        // Deduct w4
                        ok &= Deduct(p, 0, w4);
                        if (!ok) continue;

                        // Deduct w6
                        ok &= Deduct(p, 2, w6);
                        if (!ok) continue;

                        // Deduct w7
                        ok &= Deduct(p, 3, w7);
                        if (!ok) continue;

                        // Deduct w8
                        ok &= Deduct(p, 4, w8);
                        if (!ok) continue;

                        // Now exactly 5 letters remain in pool p!
                        int remCount = p.Values.Sum();
                        if (remCount != 5) continue;

                        // What word can be formed at Word 5 (Slot 1, pos 23..27) with the remaining 5 letters?
                        // Let's generate the letters:
                        var remLetters = new List<char>();
                        foreach (var kvp in p) {
                            for (int k = 0; k < kvp.Value; k++) remLetters.Add(kvp.Key);
                        }

                        // Try all permutations of remLetters at pos 23..27:
                        var perms = new HashSet<string>();
                        Permute(remLetters.ToArray(), 0, perms);

                        foreach (var perm in perms) {
                            char[] w5 = new char[5];
                            bool ok5 = true;
                            for (int j = 0; j < 5; j++) {
                                int ch = cipher[22 + j] ^ (int)perm[j];
                                if (ch < 0x5D0 || ch > 0x5EA) { ok5 = false; break; }
                                w5[j] = (char)ch;
                            }
                            if (ok5) {
                                string word5 = new string(w5);
                                if (allWords.Contains(word5)) {
                                    Console.WriteLine(string.Format("MATCH: מגילת הדרקון היא {0} {1} {2} {3} {4}.", w4, word5, w6, w7, w8));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    static bool Deduct(Dictionary<char, int> p, int slotIdx, string w) {
        var sl = slots[slotIdx];
        if (w.Length != sl.Len) return false;
        var temp = new Dictionary<char, int>();
        for (int j = 0; j < sl.Len; j++) {
            char c = (char)(cipher[sl.Start + j] ^ (int)w[j]);
            if (!p.ContainsKey(c)) return false;
            if (!temp.ContainsKey(c)) temp[c] = 0;
            temp[c]++;
            if (temp[c] > p[c]) return false;
        }
        foreach (var kvp in temp) {
            p[kvp.Key] -= kvp.Value;
        }
        return true;
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
