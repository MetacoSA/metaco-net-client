using System;
using System.Linq;

namespace MetacoClient.Addresses
{
	public static class AddressUtils
	{
		// base58-encode: [one-byte namespace][one-byte version][payload][4-byte checksum]
		private const byte ColoredAddreesNamespace = 0x13;
		private const byte P2PKH_MainNet = 0x0;
		private const byte P2SH_MainNet  = 0x5;
		private const byte P2PKH_TestNet = 0x6F;
		private const byte P2SH_TestNet  = 0xC4;
		private static readonly byte[] ValidVersions = new[] {P2PKH_MainNet, P2PKH_TestNet, P2SH_MainNet, P2SH_TestNet};

		public static string ToColoredAddress(string address)
		{
			var baddr = Base58CheckEncoder.Decode(address);
			VerifyBitcoinAddress(baddr);

			var caddr = new byte[baddr.Length + 1];
			caddr[0] = ColoredAddreesNamespace;
			Buffer.BlockCopy(baddr, 0, caddr, 1, baddr.Length);
			return Base58CheckEncoder.Encode(caddr, 0, caddr.Length);
		}

		public static string ToBitcoinAddress(string address)
		{
			var caddr = Base58CheckEncoder.Decode(address);
			VerifyColoredAddress(caddr);
			return Base58CheckEncoder.Encode(caddr, 1, caddr.Length-1);
		}

		private static void VerifyBitcoinAddress(byte[] addr)
		{
			VerifyAddress(addr, 0);
		}

		private static void VerifyColoredAddress(byte[] addr)
		{
			VerifyAddress(addr, 1);
			if (addr[0] != ColoredAddreesNamespace)
				throw new FormatException("address isn't a valid colored address : namespace byte invalid");
		}

		private static void VerifyAddress(byte[] addr, int offset)
		{
			if (addr.Length != 21 + offset)
				throw new FormatException("address isn't a valid address : length too big");

			if (!ValidVersions.Contains(addr[offset]))
				throw new FormatException("address isn't a valid address : version byte invalid");
		}
	}
}
