using System;

public class TestKeyLength {
    public static void Main() {
        int[] pos = new int[] { 6, 13, 17, 22, 28, 31, 36 };
        char[] ch = new char[] { 'ד', 'א', 'ת', 'ע', 'י', 'א', 'ש' };

        for (int L = 1; L <= 35; L++) {
            bool conflict = false;
            char[] key = new char[L];
            for (int i = 0; i < pos.Length; i++) {
                int mod = (pos[i] - 1) % L; // 0-indexed mod
                if (key[mod] != 0 && key[mod] != ch[i]) {
                    conflict = true;
                    break;
                }
                key[mod] = ch[i];
            }
            if (!conflict) {
                Console.WriteLine("Possible L = " + L + ": " + new string(key).Replace('\0', '.'));
            }
        }
    }
}
