using System;
using System.Text;
using System.Collections.Generic;

public class TestSentences {
    static int[] cipher = new int[] {
        0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
        0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 0x4, 0x1, 0x5, 0x6,
        0x31, 0x5F9, 0x37, 0xC, 0x5F0, 0x35, 0x2, 0x31, 0x3F, 0x5C9, 0x39, 0x30,
        0x3E, 0x39, 0xD, 0x5CE
    };

    static bool IsValidChar(int c) {
        return c == 0x20 || (c >= 0x5D0 && c <= 0x5EA) || c == '.' || c == ',' || c == '!' || c == '?' || c == '-' || c == '"' || c == '\'';
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string[] prefixes = new string[] {
            "שלום לכל חניכי קורס אגמים מחזור ח",
            "ברוכים הבאים לקורס אגמים מחזור ח",
            "בהצלחה לכל חניכי קורס אגמים מחזור ח",
            "כל הכבוד לחניכי קורס אגמים מחזור ח",
            "ברוכים הבאים לחניכי מחזור ח באגמים",
            "שלום לכל 42 חניכי קורס אגמים מחזור",
            "אנחנו 42 חניכים בקורס אגמים מחזור",
            "היינו 42 חניכים בקורס אגמים מחזור",
            "חניכי קורס אגמים מחזור ח בהצלחה",
            "זהו הצופן הסודי של קורס אגמים",
            "הצופן הסודי של קורס אגמים מחזור ח",
            "ברוכים הבאים לקורס רוורס אגמים",
            "בהצלחה בקורס אגמים מחזור ח ",
            "קורס אגמים מחזור ח יוצא לדרך עכשיו",
            "משימת רוורס ראשונה בקורס אגמים ח",
            "התרגיל הראשון בקורס אגמים מחזור ח",
            "דגל הצופן של קורס אגמים מחזור ח"
        };

        foreach (var p in prefixes) {
            TestPrefix(p);
        }
    }

    static void TestPrefix(string prefix) {
        int len = Math.Min(prefix.Length, cipher.Length);
        char[] other = new char[len];
        int validCount = 0;
        for (int i = 0; i < len; i++) {
            int ch = cipher[i] ^ (int)prefix[i];
            if (IsValidChar(ch)) {
                validCount++;
                other[i] = (char)ch;
            } else {
                other[i] = '?';
            }
        }
        Console.WriteLine(string.Format("[{0}/{1}] '{2}' -> '{3}'", validCount, len, prefix, new string(other)));
    }
}
