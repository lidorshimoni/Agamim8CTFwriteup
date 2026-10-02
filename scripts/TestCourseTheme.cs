using System;
using System.Text;
using System.Collections.Generic;

public class TestCourseTheme {
    static int[] cipher = new int[] {
        0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
        0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 0x4, 0x1, 0x5, 0x6,
        0x31, 0x5F9, 0x37, 0xC, 0x5F0, 0x35, 0x2, 0x31, 0x3F, 0x5C9, 0x39, 0x30,
        0x3E, 0x39, 0xD, 0x5CE
    };

    static bool IsValidHebrew(int c) {
        return (c >= 0x5D0 && c <= 0x5EA);
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;

        // The user said:
        // "ב Man agamim מופיע רק תיאור שאומר שהאגמיסט לא מוותר ובלה בלה בלה."
        // "לא היתה פקודה קשורה."
        // "חידה זו לא נפתרה עדיין ולא אמרו לנו מה הפתרון."
        // "היה מפקד שיפמן"

        // Let's test words at Word 1 (5 letters):
        // Word 1: pos 1..5: cipher 0xE, 0x2, 0x9, 0x3E, 0x3
        // Word 2: pos 7..12: cipher 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D
        // Word 3: pos 14..16: cipher 7, 0x39, 0x39
        // Word 4: pos 18..21: cipher 0x33, 0x37, 0x3D, 8
        // Word 5: pos 23..27: cipher 4, 1, 5, 6, 0x31
        // Word 6: pos 29..30: cipher 0x37, 0xC
        // Word 7: pos 32..35: cipher 0x35, 2, 0x31, 0x3F
        // Word 8: pos 37..41: cipher 0x39, 0x30, 0x3E, 0x39, 0xD

        string[] w1Candidates = new string[] {
            "שיפמן", "מאסטר", "אגמים", "חניכי", "לוחמי", "תמיד", "לעולם", "בשביל", "פקודת", "מפקדת", "מסלול"
        };
        TestSlot(w1Candidates, 0, 5, "Word 1");

        string[] w2Candidates = new string[] {
            "אגמיסט", "שיפמן", "האגמים", "באגמים", "מחזורח", "לוחמים", "הדרקון", "הזועמת"
        };
        TestSlot(w2Candidates, 6, 6, "Word 2");

        string[] w3Candidates = new string[] {
            "שלא", "הוא", "היא", "אשר", "טוב", "דבר", "הרב", "קוד", "סוד"
        };
        TestSlot(w3Candidates, 13, 3, "Word 3");

        string[] w4Candidates = new string[] {
            "נכנע", "מוותר", "עוזב", "נשבר", "מנצח", "קדוש", "הפרס", "דפוס", "נדרש"
        };
        TestSlot(w4Candidates, 17, 4, "Word 4");

        string[] w5Candidates = new string[] {
            "מוותר", "הסודי", "לעולם", "באמת", "בקרב", "ראשון"
        };
        TestSlot(w5Candidates, 22, 5, "Word 5");

        string[] w7Candidates = new string[] {
            "הכוח", "סיפר", "הסוד", "לוחם", "פנדה"
        };
        TestSlot(w7Candidates, 31, 4, "Word 7");

        string[] w8Candidates = new string[] {
            "שיפמן", "אגמים", "הדרקון", "שלומו", "נצחון", "קבלתי"
        };
        TestSlot(w8Candidates, 36, 5, "Word 8");
    }

    static void TestSlot(string[] list, int start, int len, string name) {
        Console.WriteLine("\nTesting " + name + ":");
        foreach (var w in list) {
            if (w.Length != len) continue;
            bool ok = true;
            char[] other = new char[len];
            for (int j = 0; j < len; j++) {
                int ch = cipher[start + j] ^ (int)w[j];
                if (!IsValidHebrew(ch)) { ok = false; break; }
                other[j] = (char)ch;
            }
            if (ok) {
                Console.WriteLine(string.Format("  '{0}' <-> '{1}'", w, new string(other)));
            }
        }
    }
}
