using System;
using System.Text;

public class TestNext {
    static int[] cipher = new int[] {
        0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
        0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 0x4, 0x1, 0x5, 0x6,
        0x31, 0x5F9, 0x37, 0xC, 0x5F0, 0x35, 0x2, 0x31, 0x3F, 0x5C9, 0x39, 0x30,
        0x3E, 0x39, 0xD, 0x5CE
    };

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        // Pos 28 is space (index 27).
        // Let's test what pos 29, 30, 31 could be:
        // Notice pos 31 (index 30) is 0x5F0!
        // That means pos 31 IS A SPACE!
        // If pos 31 is a space:
        // Word 6 is pos 29..30 (only 2 letters!).
        // So pos 29..30 is a 2-letter word!
        // Then pos 31 is space!
        // What 2-letter words could be at pos 29..30?
        // Let's test all common 2-letter Hebrew words:
        string[] twoLetterWords = new string[] {
            "זה", "כל", "על", "לא", "לו", "לי", "אם", "גם", "מה", "כי", "רק", "יש",
            "אל", "בו", "בי", "בך", "הם", "הן", "פה", "שם", "צו", "קו", "דף", "עץ", "רב", "עד", "כו"
        };

        foreach (var w in twoLetterWords) {
            int c1 = cipher[28] ^ (int)w[0];
            int c2 = cipher[29] ^ (int)w[1];
            Console.WriteLine(string.Format("Word '{0}': other = '{1}{2}' (0x{3:X}, 0x{4:X})", w, (char)c1, (char)c2, c1, c2));
        }
    }
}
