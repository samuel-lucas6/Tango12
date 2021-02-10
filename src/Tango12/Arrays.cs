using System;

/*
    Tango12: A stream cipher based on BLAKE2b.
    Copyright(C) 2021 Samuel Lucas

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see https://www.gnu.org/licenses/.
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
