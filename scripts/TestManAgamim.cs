using System;
using System.Text;
using System.Collections.Generic;

public class TestManAgamim {
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
            "האגמיסט לא מוותר",
            "אגמיסט לא מוותר",
            "האגמיסט אינו מוותר",
            "אגמיסט אינו מוותר",
            "אגמיסט אמיתי לא מוותר",
            "האגמיסט האמיתי לא מוותר",
            "אגמיסט אף פעם לא מוותר",
            "האגמיסט אף פעם לא מוותר",
            "אגמיסט לא נכנע ולא מוותר",
            "האגמיסט לא נשבר",
            "אגמיסט לא נשבר",
            "האגמיסט תמיד",
            "אגמיסט תמיד",
            "טיפים לאגמיסט",
            "חוקי האגמיסט",
            "מדריך לאגמיסט",
            "אגמיסט יקר",
            "ברוך הבא לאגמים",
            "ברוכים הבאים לאגמים",
            "קורס אגמים מחזור ח",
            "פקודת אגמים",
            "שיפמן אומר",
            "מאסטר שיפמן",
            "המפקד שיפמן",
            "מפקד שיפמן"
        };

        foreach (var p in prefixes) {
            TestPrefix(p);
        }
    }

    static void TestPrefix(string prefix) {
        int len = Math.Min(prefix.Length, cipher.Length);
        char[] other = new char[len];
        int valid = 0;
        for (int i = 0; i < len; i++) {
            int ch = cipher[i] ^ (int)prefix[i];
            if (IsValidChar(ch)) {
                valid++;
                other[i] = (char)ch;
            } else {
                other[i] = '?';
            }
        }
        double ratio = (double)valid / len;
        if (ratio >= 0.70) {
            Console.WriteLine(string.Format("[{0}/{1}] '{2}' -> '{3}'", valid, len, prefix, new string(other)));
        }
    }
}
