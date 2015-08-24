using System;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace Metaco.Client.Addresses
{
	internal static class Base58CheckEncoder
	{
		public static string Encode(byte[] data, int offset, int count)
		{
			var toEncode = new byte[count + 4];
			Buffer.BlockCopy(data, offset, toEncode, 0, count);

			var hash = Hash.Hash256(data, offset, count);
			Buffer.BlockCopy(hash, 0, toEncode, count, 4);

			return EncodeDataCore(toEncode, 0, toEncode.Length);
		}

		private static string EncodeDataCore(byte[] data, int offset, int count)
		{
			var bn58 =  new BigInteger(58);
			var bn0 = BigInteger.Zero;

			// Convert big endian data to little endian
			// Extra zero at the end make sure bignum will interpret as a positive number
			var vchTmp =   data.SafeSubarray(offset, count).Reverse().Concat(new byte[] { 0x00 }).ToArray();

			// Convert little endian data to bignum
			var bn = new BigInteger(vchTmp);

			// Convert bignum to std::string
			var str = "";
			// Expected size increase from base58 conversion is approximately 137%
			// use 138% to be safe

			while (bn > bn0)
			{
				BigInteger rem;
				var dv = BigInteger.DivRem(bn, bn58, out rem);
				bn = dv;
				var c = (int)rem;
				str += PszBase58[c];
			}

			// Leading zeroes encoded as base58 zeros
			for (var i = offset; i < offset + count && data[i] == 0; i++)
				str += PszBase58[0];

			// Convert little endian std::string to big endian
			str = new String(str.ToCharArray().Reverse().ToArray()); //keep that way to be portable
			return str;
		}


		const string PszBase58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";


		public static byte[] Decode(string encoded)
		{
			if (encoded == null)
				throw new ArgumentNullException("encoded");

			var vchRet = DecodeDataCore(encoded);
			if (vchRet.Length < 4)
			{
				Array.Clear(vchRet, 0, vchRet.Length);
				throw new FormatException("Invalid checked base 58 string");
			}

			var len = vchRet.Length - 4;
			var calculatedHash = Hash.Hash256(vchRet, 0, len).SafeSubarray(0, 4);
			var expectedHash = vchRet.SafeSubarray(len, 4);

			if (!calculatedHash.ArrayEqual(expectedHash))
			{
				Array.Clear(vchRet, 0, vchRet.Length);
				throw new FormatException("Invalid hash of the base 58 string");
			}
			var segment = new byte[len];
			Buffer.BlockCopy(vchRet, 0, segment, 0, len);
			return segment;
		}

		private static byte[] DecodeDataCore(string encoded)
		{
			var result = new byte[0];
			if (encoded.Length == 0)
				return result;
			BigInteger bn58 = 58;
			BigInteger bn = 0;
			int i = 0;
			while (Char.IsWhiteSpace(encoded[i]))
			{
				i++;
				if (i >= encoded.Length)
					return result;
			}

			for (int y = i; y < encoded.Length; y++)
			{
				var p1 = PszBase58.IndexOf(encoded[y]);
				if (p1 == -1)
				{
					while (Char.IsWhiteSpace(encoded[y]))
					{
						y++;
						if (y >= encoded.Length)
							break;
					}
					if (y != encoded.Length)
						throw new FormatException("Invalid base 58 string");
					break;
				}
				var bnChar = new BigInteger(p1);
				bn = BigInteger.Multiply(bn, bn58);
				bn += bnChar;
			}

			// Get bignum as little endian data
			var vchTmp = bn.ToByteArray();
			if (vchTmp.All(b => b == 0))
				vchTmp = new byte[0];

			// Trim off sign byte if present
			if (vchTmp.Length >= 2 && vchTmp[vchTmp.Length - 1] == 0 && vchTmp[vchTmp.Length - 2] >= 0x80)
				vchTmp = vchTmp.SafeSubarray(0, vchTmp.Length - 1);

			// Restore leading zeros
			int nLeadingZeros = 0;
			for (int y = i; y < encoded.Length && encoded[y] == PszBase58[0]; y++)
				nLeadingZeros++;


			result = new byte[nLeadingZeros + vchTmp.Length];
			Array.Copy(vchTmp.Reverse().ToArray(), 0, result, nLeadingZeros, vchTmp.Length);
			return result;
		}
	}

	internal static class Hash
	{
		public static byte[] Hash256(byte[] data, int offset, int count)
		{
			using (var sha = new SHA256Managed())
			{
				var h = sha.ComputeHash(data, offset, count);
				return sha.ComputeHash(h, 0, h.Length);
			}
		}
	}

	internal static class ByteArrayExtensions
	{
		internal static byte[] SafeSubarray(this byte[] array, int offset, int count)
		{
			if (array == null)
				throw new ArgumentNullException("array");
			if (offset < 0 || offset > array.Length)
				throw new ArgumentOutOfRangeException("offset");
			if (count < 0 || offset + count > array.Length)
				throw new ArgumentOutOfRangeException("count");

			var data = new byte[count];
			Buffer.BlockCopy(array, offset, data, 0, count);
			return data;
		}

		public static bool ArrayEqual(this byte[] a, byte[] b)
		{
			if (a == null && b == null)
				return true;
			if (a == null || b == null)
				return false;
			if (a.Length != b.Length)
				return false;

			for (var i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
					return false;
			}
			return true;
		}
	}
}
