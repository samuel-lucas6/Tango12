[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/samuel-lucas6/Geralt/blob/main/LICENSE)
# Tango12
A stream cipher based on BLAKE2b. There are two modes:

1. Tango12d: *deterministic* mode. No nonce is required.
2. Tango12p: *probabilistic* mode. A nonce is required.

## Should I use this?
⚠️**NO, this is a demo of how a keyed hash function can be turned into a stream cipher.**

## How do I play around with this?
1. Install the [Sodium.Core](https://www.nuget.org/packages/Sodium.Core) NuGet package in [Visual Studio](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio).
2. Download the latest [release](https://github.com/samuel-lucas6/Tango12/releases).
3. Move the downloaded DLL file into your Visual Studio project folder.
3. Click on the ```Project``` tab and ```Add Project Reference...``` in Visual Studio.
4. Go to ```Browse```, click the ```Browse``` button, and select the downloaded DLL file.

Note that the [libsodium](https://doc.libsodium.org/) library requires the [Visual C++ Redistributable for Visual Studio 2015-2019](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads) to work on Windows. If you want your program to be portable, you must keep the ```vcruntime140.dll``` file in the same folder as the executable on Windows.

### Tango12d
⚠️**WARNING: Never reuse a key.**
```c#
const string message = "This is a test.";
const int keyLength = 64;

// The message could be a file
byte[] message = Encoding.UTF8.GetBytes(message);

// The key should be random for each message
byte[] key = SodiumCore.GetRandomBytes(keyLength);

// Encrypt the message
byte[] ciphertext = Tango12d.Encrypt(message, key);

// Decrypt the ciphertext
byte[] plaintext = Tango12d.Decrypt(ciphertext, key);
```

### Tango12p
⚠️**WARNING: Never reuse a nonce with the same key.**
```c#
const string message = "This is a test.";
const int nonceLength = 24;
const int keyLength = 32;

// The message could be a file
byte[] message = Encoding.UTF8.GetBytes(message);

// The nonce can be random. Increment the nonce for each message encrypted using the same key
byte[] nonce = SodiumCore.GetRandomBytes(nonceLength);

// The key can be random or derived using a KDF (e.g. Argon2, HKDF, etc)
byte[] key = SodiumCore.GetRandomBytes(keyLength);

// Encrypt the message
byte[] ciphertext = Tango12p.Encrypt(message, nonce, key);

// Decrypt the ciphertext
byte[] plaintext = Tango12p.Decrypt(ciphertext, nonce, key);
```

## How does it work?
### Constants
```c#
internal const int BlockSize = 64;
internal const int CounterLength = 64;
internal const int NonceLength = 24;
internal const int KeyLength = 64;
```

### Tango12d
1. An empty 64 byte counter is created.
```c#
var counter = new byte[Constants.CounterLength];
```
2. The message is read in blocks of 64 bytes.
```c#
var buffer = new byte[Constants.BlockSize];
using var memoryStream = new MemoryStream(message);
while ((bytesRead = memoryStream.Read(buffer, offset: 0, buffer.Length)) > 0)
```
3. For each block, BLAKE2b-512, with the counter as the message and the key as the key, is used to generate a keystream block.
```c#
byte[] keystreamBlock = GenericHash.Hash(counter, key, Constants.BlockSize);
```
4. The message block and keystream block are XORed together to produce the ciphertext block.
```c#
internal static byte[] Xor(byte[] message, byte[] keystream)
{
    var ciphertext = new byte[message.Length];
    for (int i = 0; i < ciphertext.Length; i++)
    {
        ciphertext[i] = (byte)(message[i] ^ keystream[i]);
    }
    return ciphertext;
}
```
5. The counter is incremented.
```c#
counter = Utilities.Increment(counter);
```
6. This continues until the end of the message is reached.

### Tango12p

1. An empty 64 byte counter is created.
```c#
var counter = new byte[Constants.CounterLength];
```
2. The counter and 24 byte nonce are concatenated together.
```c#
var counter = new byte[Constants.CounterLength];
counter = Arrays.Concat(counter, nonce);
```
3. The message is read in blocks of 64 bytes.
```c#
var buffer = new byte[Constants.BlockSize];
using var memoryStream = new MemoryStream(message);
while ((bytesRead = memoryStream.Read(buffer, offset: 0, buffer.Length)) > 0)
```
4. For each block, BLAKE2b-512, with the counter and nonce as the message and the key as the key, is used to generate a keystream block.
```c#
byte[] keystreamBlock = GenericHash.Hash(counter, key, Constants.BlockSize);
```
4. The message block and keystream block are XORed together to produce the ciphertext block.
```c#
internal static byte[] Xor(byte[] message, byte[] keystream)
{
    var ciphertext = new byte[message.Length];
    for (int i = 0; i < ciphertext.Length; i++)
    {
        ciphertext[i] = (byte)(message[i] ^ keystream[i]);
    }
    return ciphertext;
}
```
5. The counter is incremented.
```c#
counter = Utilities.Increment(counter);
```
6. This continues until the end of the message is reached.
