using System;
using System.Text;
using System.Collections.Generic;

public class SearchPandaQuotes {
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

        string[] quotes = new string[] {
            // Quotes without spaces or with spaces
            "המרכיב הסודי הוא שום דבר",
            "המרכיבהסודיהואשוםדבר",
            "המרכיב הסודי של מרק האטריות",
            "המרכיבהסודישלמרקהאטריות",
            "כדי לעשות משהו מיוחד אתה צריך להאמין שהוא מיוחד",
            "כדילעשותמשהומיוחדאתהצריךלהאמיןשהואמיוחד",
            "האתמול הוא היסטוריה המחר הוא תעלומה והיום הוא מתנה",
            "האתמולהואהיסטוריההמחרהואתעלומהוהיוםהואמתנה",
            "האתמול הוא היסטוריה המחר הוא מסתורין והיום הוא מתנה",
            "האתמולהואהיסטוריההמחרהואמסתוריןוהיוםהואמתנה",
            "אין שום מרכיב סודי",
            "איןשוםמרכיבסודי",
            "אין מרכיב סודי",
            "איןמרכיבסודי",
            "מגילת הדרקון ריקה",
            "מגילתהדרקוןריקה",
            "הסוד הוא שאין סוד",
            "הסודהואשאיןסוד",
            "אין תאונות בעולם הזה",
            "איןתאונותבעולםהזה",
            "שום דבר אינו מקרי",
            "שוםדבראינומקרי",
            "לוחם הדרקון האמיתי הוא אתה",
            "לוחםהדרקוןהאמיתיהואאתה",
            "מאסטר שיפו מלמד את לוחם הדרקון",
            "מאסטרשיפומלמדתלוחםהדרקון",
            "מאסטר אוגווי בחר בפו להיות לוחם הדרקון",
            "מאסטראוגוויבחרבפוהיותלוחםהדרקון",
            "ברוכים הבאים לקורס אגמים מחזור ח בהצלחה",
            "ברוכיםהבאיםלקורסאגמיםמחזורחבהצלחה",
            "חניכי קורס אגמים מחזור שמונה בהצלחה לכולם",
            "חניכיקורסאגמיםמחזורשמונהבהצלחהלכולם"
        };

        foreach (var q in quotes) {
            TestQuote(q);
        }
    }

    static void TestQuote(string q) {
        int len = Math.Min(q.Length, cipher.Length);
        char[] other = new char[len];
        int valid = 0;
        for (int i = 0; i < len; i++) {
            int ch = cipher[i] ^ (int)q[i];
            if (IsValidChar(ch)) {
                valid++;
                other[i] = (char)ch;
            } else {
                other[i] = '?';
            }
        }
        double ratio = (double)valid / len;
        if (ratio >= 0.75) {
            Console.WriteLine(string.Format("[{0}/{1}] '{2}' -> '{3}'", valid, len, q, new string(other)));
        }
    }
}
