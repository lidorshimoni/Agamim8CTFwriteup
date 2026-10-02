using System;
using System.Text;
using System.Collections.Generic;

public class TestRepeatingKeys {
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

        string[] keys = new string[] {
            // Kung Fu Panda
            "קונג פו פנדה", "קונג פו", "פנדה", "שיפו", "מאסטר שיפו", "אוגווי", "מאסטר אוגווי",
            "טאי לונג", "לוחם הדרקון", "מגילת הדרקון", "עמק השלום", "טיגריס", "קוף", "צפע", "עגור",
            "גמל שלמה", "מר פינג", "נודלס", "אטריות", "מרק אטריות", "חמשת הזועמים", "החמישייה הזועמת",
            // Course
            "אגמים", "קורס אגמים", "מחזור ח", "מחזור ח'", "שמונה", "רוורס", "מודיעין",
            "הנדסה הפוכה", "אסמבלי", "טכנולוגיה", "אלגוריתם", "אגמים 8", "אגמים ח", "חניך", "חניכים",
            "man agamim", "agamim", "man", "reverse"
        };

        foreach (var key in keys) {
            TestKey(key);
            // Also test without spaces
            string noSpace = key.Replace(" ", "");
            if (noSpace != key) {
                TestKey(noSpace);
            }
        }
    }

    static void TestKey(string key) {
        char[] pt = new char[cipher.Length];
        int valid = 0;
        for (int i = 0; i < cipher.Length; i++) {
            int k = (int)key[i % key.Length];
            int ch = cipher[i] ^ k;
            if (IsValidChar(ch)) {
                valid++;
                pt[i] = (char)ch;
            } else {
                pt[i] = '?';
            }
        }
        if (valid >= 30) {
            Console.WriteLine(string.Format("[{0}/{1}] Key '{2}': {3}", valid, cipher.Length, key, new string(pt)));
        }
    }
}
