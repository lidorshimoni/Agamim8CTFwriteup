using System;
using System.Collections.Generic;
using System.Text;

public class Program {
    static int[] cipher = new int[] {
        0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
        0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 0x4, 0x1, 0x5, 0x6,
        0x31, 0x5F9, 0x37, 0xC, 0x5F0, 0x35, 0x2, 0x31, 0x3F, 0x5C9, 0x39, 0x30,
        0x3E, 0x39, 0xD, 0x5CE
    };

    static bool IsHebrewOrPunct(int c) {
        return c == 0x20 || (c >= 0x5D0 && c <= 0x5EA) || c == '.' || c == ',' || c == '!' || c == '?' || c == '-' || c == '"' || c == '\'';
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string[] cribs = new string[] {
            "קורס", "סייבר", "תלמידים", "שלום", "מגשימים", "כל הכבוד", "הצלחה", "בהצלחה",
            "ברוכים", "הבאים", "הצופן", "פענוח", "מחשב", "אבטחה", "אבטחת", "מידע", "הודעה",
            "ארבעים", "ושניים", "שניים", "כיתה", "מורה", "מדריך", "תרגיל", "משימה", "דגל",
            "זהו", "הוא", "היא", "אנחנו", "היינו", "אתם", "כולם", "שלכם", "שלנו", "סודי",
            "ביותר", "טוב", "מאוד", "איזה", "יופי", "פתרון", "המפתח", "מפתח", "הצלחתם"
        };

        foreach (var crib in cribs) {
            TestCrib(crib);
        }
    }

    static void TestCrib(string crib) {
        for (int i = 0; i <= cipher.Length - crib.Length; i++) {
            bool ok = true;
            char[] other = new char[crib.Length];
            for (int j = 0; j < crib.Length; j++) {
                int ch = cipher[i + j] ^ (int)crib[j];
                if (!IsHebrewOrPunct(ch)) {
                    ok = false;
                    break;
                }
                other[j] = (char)ch;
            }
            if (ok) {
                Console.WriteLine(string.Format("Pos {0,2}: '{1}' -> '{2}'", i + 1, crib, new string(other)));
            }
        }
    }
}
