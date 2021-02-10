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
    internal static class Constants
    {
        internal const int BlockSize = 64;
        internal const int CounterLength = 64;
        internal const int NonceLength = 24;
        internal const int KeyLength = 64;
    }
}
