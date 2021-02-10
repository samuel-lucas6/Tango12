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
    /// <summary>Tango12 in probabilistic mode. A nonce is required.</summary>
    public static class Tango12p
    {
        /// <summary>Encrypts a message using Tango12p.</summary>
        /// <param name="message">The message to encrypt.</param>
        /// <param name="nonce">The 24 byte nonce.</param>
        /// <param name="key">The 64 byte key.</param>
        /// <remarks>Never reuse a nonce with the same key. Use a random nonce that gets incremented for each message encrypted using the same key.</remarks>
        /// <returns>The ciphertext.</returns>
        public static byte[] Encrypt(byte[] message, byte[] nonce, byte[] key)
        {
            ParameterValidation.Message(message);
            ParameterValidation.Nonce(nonce, Constants.NonceLength);
            ParameterValidation.Key(key, Constants.KeyLength);
            int bytesRead;
            int messageIndex = 0;
            var buffer = new byte[Constants.BlockSize];
            var counter = new byte[Constants.CounterLength];
            counter = Arrays.Concat(counter, nonce);
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

        /// <summary>Decrypts a ciphertext message using Tango12p.</summary>
        /// <param name="ciphertext">The ciphertext to decrypt.</param>
        /// <param name="nonce">The 24 byte nonce.</param>
        /// <param name="key">The 64 byte key.</param>
        /// <returns>The decrypted message.</returns>
        public static byte[] Decrypt(byte[] ciphertext, byte[] nonce, byte[] key)
        {
            return Encrypt(ciphertext, nonce, key);
        }
    }
}
