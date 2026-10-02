using System;
using System.Text;

public class TestWords345 {
    static int[] cipher = new int[] {
        0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
        0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 0x4, 0x1, 0x5, 0x6,
        0x31, 0x5F9, 0x37, 0xC, 0x5F0, 0x35, 0x2, 0x31, 0x3F, 0x5C9, 0x39, 0x30,
        0x3E, 0x39, 0xD, 0x5CE
    };

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;

        // Pos 14..16: cipher[13..15] = 7, 0x39, 0x39
        // Pos 18..21: cipher[17..20] = 0x33, 0x37, 0x3D, 8
        // Pos 23..27: cipher[22..26] = 4, 1, 5, 6, 0x31

        string[] w3List = new string[] { "זהו", "הוא", "שזה", "סוד", "קוד", "שם", "רק", "אין", "אם", "דבר", "טוב", "גלה", "מצא" };
        foreach (var w3 in w3List) {
            if (w3.Length != 3) continue;
            char[] o3 = new char[3];
            for (int j = 0; j < 3; j++) o3[j] = (char)(cipher[13 + j] ^ (int)w3[j]);
            Console.WriteLine(string.Format("W3 '{0}' -> '{1}'", w3, new string(o3)));
        }

        string[] w4List = new string[] { "הקוד", "סודי", "קודם", "צופן", "מפתח", "הסוד", "הזהו", "שאין" };
        foreach (var w4 in w4List) {
            if (w4.Length != 4) continue;
            char[] o4 = new char[4];
            for (int j = 0; j < 4; j++) o4[j] = (char)(cipher[17 + j] ^ (int)w4[j]);
            Console.WriteLine(string.Format("W4 '{0}' -> '{1}'", w4, new string(o4)));
        }

        string[] w5List = new string[] { "הסודי", "המרק", "אגמים", "שיפו", "פנדה", "לוחם" };
        foreach (var w5 in w5List) {
            if (w5.Length != 5) continue;
            char[] o5 = new char[5];
            for (int j = 0; j < 5; j++) o5[j] = (char)(cipher[22 + j] ^ (int)w5[j]);
            Console.WriteLine(string.Format("W5 '{0}' -> '{1}'", w5, new string(o5)));
        }
    }
}
