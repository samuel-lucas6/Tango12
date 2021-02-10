using Sodium;
using System.IO;

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
    /// <summary>Tango12 in deterministic mode. No nonce is required.</summary>
    public static class Tango12d
    {
        /// <summary>Encrypts a message using Tango12d.</summary>
        /// <param name="message">The message to encrypt.</param>
        /// <param name="key">The 64 byte key.</param>
        /// <remarks>Never reuse a key.</remarks>
        /// <returns>The ciphertext.</returns>
        public static byte[] Encrypt(byte[] message, byte[] key)
        {
            ParameterValidation.Message(message);
            ParameterValidation.Key(key, Constants.KeyLength);
            int bytesRead;
            int messageIndex = 0;
            var buffer = new byte[Constants.BlockSize];
            var counter = new byte[Constants.CounterLength];
            using var memoryStream = new MemoryStream(message);
            while ((bytesRead = memoryStream.Read(buffer, offset: 0, buffer.Length)) > 0)
            {
                byte[] keystreamBlock = GenericHash.Hash(counter, key, Constants.BlockSize);
                buffer = Arrays.Xor(buffer, keystreamBlock);
                for (int i = 0; i < bytesRead; i++)
                {
                    message[messageIndex + i] = buffer[i];
                }
                counter = Utilities.Increment(counter);
                messageIndex += Constants.BlockSize;
            }
            return message;
        }

        /// <summary>Decrypts a ciphertext message using Tango12d.</summary>
        /// <param name="ciphertext">The ciphertext to decrypt.</param>
        /// <param name="key">The 64 byte key.</param>
        /// <returns>The decrypted message.</returns>
        public static byte[] Decrypt(byte[] ciphertext, byte[] key)
        {
            return Encrypt(ciphertext, key);
        }
    }
}
