using Sodium;
using System.IO;

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
