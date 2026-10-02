using System;
using System.Text;
using System.Collections.Generic;

public class Find4Letter {
    static int[] cipher = new int[] { 0x33, 0x37, 0x3D, 0x8 };

    static bool IsValidHebrew(int c) {
        return (c >= 0x5D0 && c <= 0x5EA);
    }

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        // List common 4-letter Hebrew words
        string[] candidates = new string[] {
            "שלום", "קורס", "מידע", "ספר", "דבר", "חדר", "בית", "איש", "מילה", "תוכן",
            "קודם", "צופן", "מפתח", "הקוד", "שפת", "קובץ", "קודם", "מערך", "שעור", "מבחן",
            "כיתה", "מורה", "קבוץ", "סודי", "דגלך", "חברך", "שמך", "אותם", "אותו", "אותה",
            "כולם", "שלכם", "שלנו", "שלהם", "הזהו", "הזאת", "האלה", "היום", "עכשיו", "תמיד",
            "פשוט", "קשה", "אחד", "שנים", "שלוש", "ארבע", "חמש", "שש", "שבע", "שמונה",
            "תשע", "עשר", "מאה", "אלף", "ארבע", "שתים", "זמן", "שעה", "דקה", "מקום",
            "דרך", "סייבר", "רשת", "נתב", "שרת", "לקוח", "פרוטוקול", "הודעה", "מידע",
            "אבטח", "מחשב", "תוכנה", "חומרה", "לינוקס", "חלונות", "פקודה", "זיכרון",
            "אחרי", "לפני", "בתוך", "מתוך", "מכל", "בכל", "לכל", "על", "עם", "את"
        };

        foreach (var w in candidates) {
            if (w.Length != 4) continue;
            bool ok = true;
            char[] other = new char[4];
            for (int i = 0; i < 4; i++) {
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
