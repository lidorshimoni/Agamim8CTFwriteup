using System;
using System.Text;
using System.Collections.Generic;

public class FindWord1 {
    static int[] cipher = new int[] { 0xE, 0x2, 0x9, 0x3E, 0x3 };

    static bool IsValidHebrew(int c) {
        return (c >= 0x5D0 && c <= 0x5EA);
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string[] candidates = new string[] {
            "ברוכים", "שלום", "הצופן", "הודעה", "ברכות", "קורס", "טכנולוגיה", "משימה", "תרגיל",
            "אנחנו", "היינו", "היום", "כולם", "איזה", "יופי", "מדריך", "תלמיד", "כיתה",
            "פתרון", "המפתח", "מפתח", "שאלה", "תשובה", "רוצים", "יודעים", "אוהבים",
            "האמת", "הסוד", "סודי", "מזל", "טוב", "חברים", "יקרים", "מחשב", "תוכנה",
            "מידע", "אבטחה", "אבטחת", "רשתות", "קובץ", "פקודה", "זיכרון", "חומרה"
        };

        foreach (var w in candidates) {
            if (w.Length != 5) continue;
            bool ok = true;
            char[] other = new char[5];
            for (int i = 0; i < 5; i++) {
                int ch = cipher[i] ^ (int)w[i];
                if (!IsValidHebrew(ch)) {
                    ok = false;
                    break;
                }
                other[i] = (char)ch;
            }
            if (ok) {
                Console.WriteLine(string.Format("'{0}' <-> '{1}'", w, new string(other)));
            }
        }
    }
}
