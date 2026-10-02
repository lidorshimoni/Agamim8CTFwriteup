using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class SolveWithStudentPool {
    static int[] cipher = new int[] {
        0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
        0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 4, 1, 5, 6, 0x31,
        0x5F9, 0x37, 0xC, 0x5F0, 0x35, 2, 0x31, 0x3F, 0x5C9, 0x39, 0x30, 0x3E,
        0x39, 0xD, 0x5CE
    };

    // The 42 students' initials:
    // א: 10 (אורי, אורי, איה, איתי, איתי, אלון, אלי, אלכס, אמיר, אשל)
    // ג: 1 (גל)
    // ד: 4 (דוד, דנה, דניאל, דניאל)
    // ה: 1 (הדר)
    // ח: 1 (חן)
    // י: 3 (יהונתן, יהלי, יואב)
    // ל: 1 (לידור)
    // מ: 3 (מור, מיתר, מקס)
    // נ: 5 (נגה, נדב, נועם, ניר, נתאי)
    // ע: 5 (עומר, עידו, עילאי, עילי, עמית)
    // ר: 2 (רותם, רעות)
    // ש: 5 (שגיא, שון, שי, שירה, שליו)
    // ת: 1 (תו)

    static Dictionary<char, int> GetInitialPool() {
        var pool = new Dictionary<char, int>();
        pool['א'] = 10;
        pool['ג'] = 1;
        pool['ד'] = 4;
        pool['ה'] = 1;
        pool['ח'] = 1;
        pool['י'] = 3;
        pool['ל'] = 1;
        pool['מ'] = 3;
        pool['נ'] = 5;
        pool['ע'] = 5;
        pool['ר'] = 2;
        pool['ש'] = 5;
        pool['ת'] = 1;
        return pool;
    }

    struct Slot {
        public int Start;
        public int Len;
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string dictPath = @"C:\Users\lidor\.gemini\antigravity\brain\af5dee43-217e-42db-b0f1-f9e2bb692b52\.system_generated\steps\117\content.md";

        // Space / period positions consume:
        // pos 6: ד (1)
        // pos 13: א (1)
        // pos 17: ת (1)
        // pos 22: ע (1)
        // pos 28: י (1)
        // pos 31: א (1)
        // pos 36: ש (1)
        // pos 42: נ (1)
        var basePool = GetInitialPool();
        basePool['ד']--;
        basePool['א']--;
        basePool['ת']--;
        basePool['ע']--;
        basePool['י']--;
        basePool['א']--;
        basePool['ש']--;
        basePool['נ']--;

        Console.WriteLine("Remaining pool after spaces:");
        foreach (var kvp in basePool.OrderBy(k => k.Key)) {
            Console.WriteLine(string.Format("  {0}: {1}", kvp.Key, kvp.Value));
        }

        // Load dictionary words
        HashSet<string> words = new HashSet<string>();
        foreach (var line in File.ReadAllLines(dictPath)) {
            var parts = line.Split(' ');
            if (parts.Length >= 1) {
                string w = parts[0].Trim();
                if (w.Length > 0 && !w.StartsWith("#") && !w.StartsWith("Title") && !w.StartsWith("Description") && !w.StartsWith("-")) {
                    words.Add(w);
                }
            }
        }
        // Add course words
        words.Add("מגילת");
        words.Add("הדרקון");
        words.Add("שיפמן");
        words.Add("אגמים");
        words.Add("אגמיסט");
        words.Add("לוחם");
        words.Add("לוחמי");

        Slot[] slots = new Slot[] {
            new Slot { Start = 0, Len = 5 },   // Word 1: pos 1..5
            new Slot { Start = 6, Len = 6 },   // Word 2: pos 7..12
            new Slot { Start = 13, Len = 3 },  // Word 3: pos 14..16
            new Slot { Start = 17, Len = 4 },  // Word 4: pos 18..21
            new Slot { Start = 22, Len = 5 },  // Word 5: pos 23..27
            new Slot { Start = 28, Len = 2 },  // Word 6: pos 29..30
            new Slot { Start = 31, Len = 4 },  // Word 7: pos 32..35
            new Slot { Start = 36, Len = 5 }   // Word 8: pos 37..41
        };

        // For each slot, find words that ONLY use letters present in the remaining pool
        for (int s = 0; s < slots.Length; s++) {
            var slot = slots[s];
            Console.WriteLine(string.Format("\n=== Slot {0} (pos {1}..{2}, len {3}) ===", s + 1, slot.Start + 1, slot.Start + slot.Len, slot.Len));
            var candidates = new List<Tuple<string, string>>();

            foreach (var w in words) {
                if (w.Length != slot.Len) continue;

                // If s == 1 (Word 2), check if pos 10 has 'ל'
                if (s == 1) {
                    // pos 10 is index 3 in word 2
                    int ch10 = cipher[9] ^ (int)w[3];
                    if (ch10 != 'ל') continue;
                }

                bool ok = true;
                char[] initials = new char[slot.Len];
                var localCounts = new Dictionary<char, int>();

                for (int j = 0; j < slot.Len; j++) {
                    int ch = cipher[slot.Start + j] ^ (int)w[j];
                    char c = (char)ch;
                    if (!basePool.ContainsKey(c)) { ok = false; break; }
                    if (!localCounts.ContainsKey(c)) localCounts[c] = 0;
                    localCounts[c]++;
                    if (localCounts[c] > basePool[c]) { ok = false; break; }
                    initials[j] = c;
                }

                if (ok) {
                    candidates.Add(Tuple.Create(w, new string(initials)));
                }
            }

            Console.WriteLine(string.Format("Found {0} candidates for Slot {1}:", candidates.Count, s + 1));
            foreach (var c in candidates.Take(25)) {
                Console.WriteLine(string.Format("  '{0}' -> initials '{1}'", c.Item1, c.Item2));
            }
        }
    }
}
