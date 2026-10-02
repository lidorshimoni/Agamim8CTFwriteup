using System;
using System.Text;
using System.Collections.Generic;

public class Find6Letter {
    static int[] cipher = new int[] { 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D };

    static bool IsValidHebrew(int c) {
        return (c >= 0x5D0 && c <= 0x5EA);
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        string[] candidates = new string[] {
            "תלמידים", "ברוכים", "הצלחה", "בהצלחה", "מגשימים", "פענוח", "ההודעה", "הצפנה",
            "פרוטוקול", "מחשבים", "אבטחה", "אבטחת", "תוכנה", "חומרה", "לינוקס", "חלונות",
            "רשתות", "נתבים", "שרתים", "לקוחות", "מערכות", "הסודית", "הסודי", "הפתרון",
            "פתרונות", "שיעור", "תרגיל", "משימה", "מדריך", "מרצים", "שאלות", "תשובות",
            "ארבעים", "ושניים", "הבאים", "שלכם", "שלנו", "סודית", "פשוטה", "מורכבת",
            "ראשון", "אחרון", "שלום", "להתראות", "חברים", "יקרים", "מצוין", "מצוינת"
        };

        foreach (var w in candidates) {
            if (w.Length != 6) continue;
            bool ok = true;
            char[] other = new char[6];
            for (int i = 0; i < 6; i++) {
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
