using System;
using System.Text;

public class TestPos {
    static int[] cipher = new int[] {
        0xE, 0x2, 0x9, 0x3E, 0x3, 0x5F3, 0xA, 0x3B, 0x3B, 0x3B, 0x6, 0x3D, 0x5F0,
        0x7, 0x39, 0x39, 0x5CA, 0x33, 0x37, 0x3D, 0x8, 0x5C2, 0x4, 0x1, 0x5, 0x6,
        0x31, 0x5F9, 0x37, 0xC, 0x5F0, 0x35, 0x2, 0x31, 0x3F, 0x5C9, 0x39, 0x30,
        0x3E, 0x39, 0xD, 0x5CE
    };

    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        // Pos 24 (index 23): cipher is 1, 5, 6, 0x31
        // Let's test what gives נאור at pos 24:
        string name = "נאור";
        for (int i = 0; i < 4; i++) {
            int ch = cipher[23 + i] ^ (int)name[i];
            Console.WriteLine(string.Format("pos {0}: cipher=0x{1:X} ^ '{2}' = '{3}' (0x{4:X})", 24 + i, cipher[23 + i], name[i], (char)ch, ch));
        }
        // At pos 28 (index 27): cipher is 0x5F9
        // If pos 28 is space for one text, the other text is 0x5F9 ^ 0x20 = 0x5D9 = 'י'
        Console.WriteLine(string.Format("pos 28: 0x5F9 ^ ' ' = '{0}'", (char)(0x5F9 ^ 0x20)));

        // What about pos 23 (index 22)?
        // cipher[22] = 4
        // If Text A has ' ' at pos 22: cipher[21] is 0x5C2!
        // 0x5C2 ^ 0x20 = 'ע'!
        Console.WriteLine(string.Format("pos 22: 0x5C2 ^ ' ' = '{0}'", (char)(0x5C2 ^ 0x20)));
    }
}
