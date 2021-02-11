using System;

/*
    Tango12: A stream cipher based on BLAKE2b.
    Copyright (c) 2021 Samuel Lucas

    Permission is hereby granted, free of charge, to any person obtaining a copy of
    this software and associated documentation files (the "Software"), to deal in
    the Software without restriction, including without limitation the rights to
    use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
    the Software, and to permit persons to whom the Software is furnished to do so,
    subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
    SOFTWARE.
*/

namespace Tango12
{
    internal static class Arrays
    {
        internal static byte[] Concat(byte[] a, byte[] b)
        {
            const int index = 0;
            var concat = new byte[a.Length + b.Length];
            Array.Copy(a, index, concat, index, a.Length);
            Array.Copy(b, index, concat, a.Length, b.Length);
            return concat;
        }

        internal static byte[] Xor(byte[] message, byte[] keystream)
        {
            var ciphertext = new byte[message.Length];
            for (int i = 0; i < ciphertext.Length; i++)
            {
                ciphertext[i] = (byte)(message[i] ^ keystream[i]);
            }
            return ciphertext;
        }
    }
}
