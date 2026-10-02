using System;
using System.Text;
using System.Collections.Generic;

public class KungFuPandaSearch {
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

        string[] quotesAndTerms = new string[] {
            // Characters & Concepts
            "קונג פו פנדה", "קונג פו", "פנדה", "לוחם הדרקון", "מגילת הדרקון",
            "מאסטר אוגווי", "מאסטר שיפו", "אוגווי", "שיפו", "טאי לונג",
            "החמישייה הזועמת", "חמשת הזועמים", "טיגריס", "עגור", "קוף", "צפע", "גמל שלמה",
            "עמק השלום", "מר פינג", "מרק אטריות", "אטריות",
            "אין מרכיב סודי", "המרכיב הסודי", "מרכיב סודי", "סודי", "אין סוד",
            "הסוד הוא שאין סוד", "אין שום סוד", "אין שום מרכיב סודי",
            "הסוד הוא", "הסודי ביותר",
            "האתמול הוא היסטוריה", "והמחר הוא מסתורין", "אבל היום הוא מתנה",
            "אין תאונות", "אין מקריות", "שום דבר אינו מקרי",
            // Agamim terms
            "אגמים", "קורס אגמים", "מחזור ח", "מחזור ח'", "שמונה",
            "מן אגמים", "אגמים מחזור ח", "פקודת אגמים", "מדריך",
            "man agamim", "agamim"
        };

        foreach (var term in quotesAndTerms) {
            TestTerm(term);
        }
    }

    static void TestTerm(string term) {
        for (int i = 0; i <= cipher.Length - term.Length; i++) {
            bool ok = true;
            char[] other = new char[term.Length];
            for (int j = 0; j < term.Length; j++) {
                int ch = cipher[i + j] ^ (int)term[j];
                if (!IsValidChar(ch)) {
                    ok = false;
                    break;
                }
                other[j] = (char)ch;
            }
            if (ok) {
                Console.WriteLine(string.Format("Pos {0,2}: '{1}' <-> '{2}'", i + 1, term, new string(other)));
            }
        }
    }
}
