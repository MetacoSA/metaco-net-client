using System;
using MetacoClient.Addresses;
using NUnit.Framework;

namespace MetacoClient.Tests
{
	[TestFixture]
	public class AddressUtilTest
	{
		//Base-58:    16UwLL9Risc3QfPqBUvKofHmBQ7wMtjvM
		//Hex:        00 010966776006953D5567439E5E39F86A0D273BEE D61967F6
		//Base-58:    akB4NBW9UuCmHuepksob6yfZs6naHtRCPNy
		//Hex:        13 00 010966776006953D5567439E5E39F86A0D273BEE 852783AA
		[Test]
		public void CanConvertToBitcoinAddress()
		{
			Assert.AreEqual(
				"16UwLL9Risc3QfPqBUvKofHmBQ7wMtjvM", 
				AddressUtils.ToBitcoinAddress(AddressUtils.ToColoredAddress("16UwLL9Risc3QfPqBUvKofHmBQ7wMtjvM")));

			Assert.AreEqual(
				"akB4NBW9UuCmHuepksob6yfZs6naHtRCPNy",
				AddressUtils.ToColoredAddress(AddressUtils.ToBitcoinAddress("akB4NBW9UuCmHuepksob6yfZs6naHtRCPNy")));
		}

		[Test]
		public void CanDetectInvalidAddresses()
		{
			Assert.Throws<FormatException>(()=> AddressUtils.ToBitcoinAddress("juan"));
			Assert.Throws<FormatException>(() => AddressUtils.ToColoredAddress("juan"));
		}
	}
}
